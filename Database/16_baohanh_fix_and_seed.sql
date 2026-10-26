-- ============================================================================
-- SCRIPT 16: SỬA LỖI UNICODE BẢO HÀNH & BỔ SUNG VIEW vw_QuanLyBaoHanh
-- Database: QuanLyBanLaptop_V2
-- ============================================================================
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

USE QuanLyBanLaptop_V2;
GO

-- 1. Sửa lỗi Unicode trong Trigger trg_BH_KiemTra
CREATE OR ALTER TRIGGER dbo.trg_BH_KiemTra
    ON dbo.BAO_HANH_SUA_CHUA
    AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra máy có đang ở trạng thái hợp lệ để tiếp nhận bảo hành / sửa chữa
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN dbo.LAPTOP_SERIAL_DATA s ON s.SerialNo = i.SerialNo
        WHERE NOT EXISTS (SELECT 1 FROM deleted d WHERE d.MaPhieuBH = i.MaPhieuBH)
          AND s.TrangThai NOT IN (N'Đã bán', N'Đang bảo hành')
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50050, N'Máy chưa bán hoặc đang không có trong hồ sơ khách hàng.', 1;
    END;

    -- Kiểm tra nếu là bảo hành miễn phí thì máy phải còn hạn bảo hành
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN dbo.LAPTOP_SERIAL s ON s.SerialNo = i.SerialNo
        WHERE NOT EXISTS (SELECT 1 FROM deleted d WHERE d.MaPhieuBH = i.MaPhieuBH)
          AND i.LoaiDichVu = N'Bảo hành'
          AND (s.NgayHetBH IS NULL OR s.NgayHetBH < CAST(i.NgayNhan AS DATE))
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50051, N'Máy đã hết hạn bảo hành của hãng, vui lòng tạo phiếu Sửa chữa tính phí.', 1;
    END;

    -- Cập nhật trạng thái số serial: Khi đang xử lý thì gán 'Đang bảo hành', khi đã trả khách thì hoàn lại 'Đã bán'
    UPDATE s
    SET TrangThai = CASE 
                        WHEN i.TinhTrangBH IN (N'Tiếp nhận', N'Đang xử lý') THEN N'Đang bảo hành' 
                        ELSE N'Đã bán' 
                    END
    FROM dbo.LAPTOP_SERIAL_DATA s
    JOIN inserted i ON i.SerialNo = s.SerialNo
    WHERE s.TrangThai IN (N'Đã bán', N'Đang bảo hành');
END;
GO

-- 2. Cập nhật CHECK constraint cho TinhTrangBH để hỗ trợ đầy đủ quy trình
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK__BAO_HANH___TinhT__1332DBDC')
    ALTER TABLE dbo.BAO_HANH_SUA_CHUA DROP CONSTRAINT CK__BAO_HANH___TinhT__1332DBDC;
GO

IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_BH_TinhTrang')
    ALTER TABLE dbo.BAO_HANH_SUA_CHUA DROP CONSTRAINT CK_BH_TinhTrang;
GO

ALTER TABLE dbo.BAO_HANH_SUA_CHUA WITH CHECK ADD CONSTRAINT CK_BH_TinhTrang
    CHECK (TinhTrangBH IN (N'Tiếp nhận', N'Đang xử lý', N'Đã hoàn thành', N'Đã trả khách'));
GO

-- 3. Tạo View hiển thị toàn diện quản lý bảo hành vw_QuanLyBaoHanh
IF OBJECT_ID('dbo.vw_QuanLyBaoHanh', 'V') IS NOT NULL
    DROP VIEW dbo.vw_QuanLyBaoHanh;
GO

CREATE VIEW dbo.vw_QuanLyBaoHanh
AS
SELECT 
    b.MaPhieuBH,
    b.SerialNo,
    s.MaLaptop,
    l.TenLaptop,
    h.MaHD,
    h.MaKH,
    k.TenKH,
    k.Sdt,
    b.MaNV,
    nv.TenNV,
    b.LoaiDichVu,
    b.NgayNhan,
    b.NgayTra,
    b.MoTaLoi,
    b.ChiPhi,
    b.TinhTrangBH,
    s.NgayHetBH,
    CASE WHEN s.NgayHetBH >= CAST(GETDATE() AS DATE) THEN 1 ELSE 0 END AS ConBaoHanh
FROM dbo.BAO_HANH_SUA_CHUA b
JOIN dbo.LAPTOP_SERIAL s ON s.SerialNo = b.SerialNo
JOIN dbo.LAPTOP l ON l.MaLaptop = s.MaLaptop
LEFT JOIN dbo.HOA_DON h ON h.MaHD = s.MaHD
LEFT JOIN dbo.KHACH_HANG k ON k.MaKH = h.MaKH
LEFT JOIN dbo.NHAN_VIEN nv ON nv.MaNV = b.MaNV;
GO

-- 4. Bổ sung một số phiếu mẫu thực tế để phục vụ thống kê & demo
IF NOT EXISTS (SELECT 1 FROM dbo.BAO_HANH_SUA_CHUA WHERE MaPhieuBH = 'BH003')
BEGIN
    INSERT INTO dbo.BAO_HANH_SUA_CHUA(MaPhieuBH, SerialNo, MaNV, LoaiDichVu, NgayNhan, NgayTra, MoTaLoi, ChiPhi, TinhTrangBH)
    VALUES ('BH003', 'LT001-PN002-002', 'NV002', N'Bảo hành', '2026-10-23 10:15:00', NULL, N'Bàn phím bị liệt phím cách (Space), gửi bảo hành hãng thay cụm phím', 0, N'Đang xử lý');
END;

IF NOT EXISTS (SELECT 1 FROM dbo.BAO_HANH_SUA_CHUA WHERE MaPhieuBH = 'BH004')
BEGIN
    INSERT INTO dbo.BAO_HANH_SUA_CHUA(MaPhieuBH, SerialNo, MaNV, LoaiDichVu, NgayNhan, NgayTra, MoTaLoi, ChiPhi, TinhTrangBH)
    VALUES ('BH004', 'LT005-PN005-001', 'NV003', N'Sửa chữa', '2026-10-22 14:30:00', '2026-10-24 16:00:00', N'Nâng cấp RAM 16GB và vệ sinh bôi keo tản nhiệt MX4', 1250000, N'Đã hoàn thành');
END;
GO

PRINT N'Script 16: Sửa lỗi Unicode bảo hành và tạo view vw_QuanLyBaoHanh hoàn tất thành công!';
GO

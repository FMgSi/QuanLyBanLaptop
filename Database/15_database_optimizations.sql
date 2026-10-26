-- ============================================================================
-- SCRIPT 15: TỐI ƯU HÓA TOÀN DIỆN CSDL & XỬ LÝ EDGE CASES NGHIỆP VỤ
-- Database: QuanLyBanLaptop_V2
-- ============================================================================
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

USE QuanLyBanLaptop_V2;
GO

-- 1. Tối ưu hóa Stored Procedure dbo.sp_TaoLichTraGop:
--    Đọc động các thông số từ CAU_HINH, bảo toàn tròn số từng đồng cho kỳ cuối
CREATE OR ALTER PROCEDURE dbo.sp_TaoLichTraGop 
    @MaHD NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- Đọc cấu hình trả góp từ CAU_HINH nếu chưa có trong hợp đồng
    DECLARE @defaultDown DECIMAL(5,4) = ISNULL((SELECT CAST(GiaTri AS DECIMAL(5,4))/100.0 FROM dbo.CAU_HINH WHERE Khoa = 'TRA_TRUOC_TOI_THIEU'), 0.30);
    DECLARE @rate3  DECIMAL(5,2) = ISNULL((SELECT CAST(GiaTri AS DECIMAL(5,2)) FROM dbo.CAU_HINH WHERE Khoa = 'LAI_SUAT_3_THANG'), 2.4);
    DECLARE @rate6  DECIMAL(5,2) = ISNULL((SELECT CAST(GiaTri AS DECIMAL(5,2)) FROM dbo.CAU_HINH WHERE Khoa = 'LAI_SUAT_6_THANG'), 4.0);
    DECLARE @rate9  DECIMAL(5,2) = ISNULL((SELECT CAST(GiaTri AS DECIMAL(5,2)) FROM dbo.CAU_HINH WHERE Khoa = 'LAI_SUAT_9_THANG'), 6.0);
    DECLARE @rate12 DECIMAL(5,2) = ISNULL((SELECT CAST(GiaTri AS DECIMAL(5,2)) FROM dbo.CAU_HINH WHERE Khoa = 'LAI_SUAT_12_THANG'), 8.0);

    IF NOT EXISTS (SELECT 1 FROM dbo.HOP_DONG_TRA_GOP WHERE MaHD = @MaHD)
    BEGIN
        INSERT INTO dbo.HOP_DONG_TRA_GOP(MaHD, TyLeTraTruoc, LaiSuat)
        SELECT MaHD, @defaultDown, 
            CASE ChuKyTraGop 
                WHEN 3 THEN @rate3 
                WHEN 6 THEN @rate6 
                WHEN 9 THEN @rate9 
                ELSE @rate12 
            END
        FROM dbo.HOA_DON_DATA 
        WHERE MaHD = @MaHD AND PhuongThucThanhToan = N'Trả góp';
    END;

    DECLARE @tong DECIMAL(18,2), @ky INT, @ngay DATE, @rate DECIMAL(5,2), @down DECIMAL(5,4);
  
    SELECT @tong = h.TongTien, @ky = h.ChuKyTraGop, @ngay = CAST(h.NgayLap AS DATE),
           @rate = c.LaiSuat, @down = c.TyLeTraTruoc
    FROM dbo.HOA_DON h
    JOIN dbo.HOP_DONG_TRA_GOP c ON c.MaHD = h.MaHD
    WHERE h.MaHD = @MaHD AND h.PhuongThucThanhToan = N'Trả góp';

    IF @ky IS NULL 
        THROW 51201, N'Thiếu hóa đơn hoặc điều khoản trả góp.', 1;

    IF EXISTS (SELECT 1 FROM dbo.LICH_TRA_GOP WHERE MaHD = @MaHD)
        THROW 51202, N'Hóa đơn đã có lịch trả góp.', 1;

    -- Tính toán phân bổ số tiền kỳ
    DECLARE @loan DECIMAL(18,2) = @tong - ROUND(@tong * @down, 0);
    DECLARE @payable DECIMAL(18,2) = @loan + ROUND(@loan * @rate / 100.0, 0);
    DECLARE @monthly DECIMAL(18,2) = FLOOR(@payable / @ky);

    IF @monthly <= 0 
        THROW 51203, N'Số tiền quá nhỏ để chia kỳ.', 1;

    -- Kỳ cuối cùng sẽ gánh toàn bộ phần dư do làm tròn (ROUND/FLOOR)
    INSERT INTO dbo.LICH_TRA_GOP(MaHD, KyThu, NgayDenHan, SoTienPhaiTra, TienPhat)
    SELECT @MaHD, So, DATEADD(MONTH, So, @ngay),
           CASE WHEN So = @ky THEN @payable - @monthly * (@ky - 1) ELSE @monthly END, 0
    FROM dbo.fn_DaySo(@ky);
END;
GO

-- 2. Tối ưu hóa Stored Procedure dbo.sp_ThanhToanKy
CREATE OR ALTER PROCEDURE dbo.sp_ThanhToanKy
    @MaHD      NVARCHAR(30),
    @KyThu     INT,
    @NgayTra   DATE = NULL,
    @TienPhat  DECIMAL(18,2) = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;

        UPDATE dbo.LICH_TRA_GOP
        SET NgayTra = ISNULL(@NgayTra, CAST(GETDATE() AS DATE)),
            SoTienDaTra = SoTienPhaiTra,
            TienPhat = ISNULL(@TienPhat, 0)
        WHERE MaHD = @MaHD AND KyThu = @KyThu AND NgayTra IS NULL;
        
        IF @@ROWCOUNT = 0 
            THROW 50040, N'Kỳ trả góp không tồn tại hoặc đã được thanh toán.', 1;

        -- Khi tất cả các kỳ đều đã thanh toán, tự động đánh dấu hóa đơn hoàn tất
        IF NOT EXISTS (SELECT 1 FROM dbo.LICH_TRA_GOP WHERE MaHD = @MaHD AND NgayTra IS NULL)
        BEGIN
            UPDATE dbo.HOA_DON_DATA 
            SET TinhTrang = N'Đã thanh toán' 
            WHERE MaHD = @MaHD;
        END;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END;
GO

-- 3. Sửa lỗi Unicode hiển thị thông báo lỗi trong trg_CTHD_Xoa
CREATE OR ALTER TRIGGER dbo.trg_CTHD_Xoa
    ON dbo.CHI_TIET_HD
    AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1
               FROM dbo.BAO_HANH_SUA_CHUA b
               JOIN dbo.LAPTOP_SERIAL_DATA s ON s.SerialNo = b.SerialNo
               JOIN deleted d ON d.MaHD = s.MaHD AND d.MaLaptop = s.MaLaptop)
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50042, N'Không thể xóa: máy trong hóa đơn đã có phiếu bảo hành/sửa chữa.', 1;
    END;

    UPDATE s
    SET MaHD = NULL, TrangThai = N'Trong kho'
    FROM dbo.LAPTOP_SERIAL_DATA s
    JOIN deleted d ON d.MaHD = s.MaHD AND d.MaLaptop = s.MaLaptop;
END;
GO

-- 4. Sửa lỗi Unicode trong trigger tr_compat_HOA_DON_U
CREATE OR ALTER TRIGGER dbo.[tr_compat_HOA_DON_U] 
    ON dbo.[HOA_DON] 
    INSTEAD OF UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON;
    IF EXISTS(SELECT [MaHD] FROM inserted EXCEPT SELECT [MaHD] FROM deleted)
    BEGIN
        IF (SELECT COUNT(*) FROM inserted) <> 1 
            THROW 51010, N'Chỉ đổi một mã mỗi lần.', 1;
        UPDATE b 
        SET b.[MaHD]=i.[MaHD], b.[MaKH]=i.[MaKH], b.[MaNV]=i.[MaNV], b.[NgayLap]=i.[NgayLap],
            b.[PhuongThucThanhToan]=i.[PhuongThucThanhToan], b.[ChuKyTraGop]=i.[ChuKyTraGop], b.[TinhTrang]=i.[TinhTrang] 
        FROM dbo.[HOA_DON_DATA] b 
        JOIN deleted d ON b.[MaHD]=d.[MaHD] 
        CROSS JOIN inserted i;
    END
    ELSE 
    BEGIN
        UPDATE b 
        SET b.[MaKH]=i.[MaKH], b.[MaNV]=i.[MaNV], b.[NgayLap]=i.[NgayLap],
            b.[PhuongThucThanhToan]=i.[PhuongThucThanhToan], b.[ChuKyTraGop]=i.[ChuKyTraGop], b.[TinhTrang]=i.[TinhTrang] 
        FROM dbo.[HOA_DON_DATA] b 
        JOIN inserted i ON b.[MaHD]=i.[MaHD]; 
    END;
END;
GO

-- 5. Bổ sung các Index tăng tốc độ truy vấn Thống kê và Báo cáo
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_HOA_DON_DATA_NgayLap' AND object_id = OBJECT_ID('dbo.HOA_DON_DATA'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_HOA_DON_DATA_NgayLap 
    ON dbo.HOA_DON_DATA(NgayLap) 
    INCLUDE(MaHD, MaKH, MaNV, PhuongThucThanhToan, TinhTrang);
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_CHI_TIET_HD_MaHD' AND object_id = OBJECT_ID('dbo.CHI_TIET_HD'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_CHI_TIET_HD_MaHD 
    ON dbo.CHI_TIET_HD(MaHD) 
    INCLUDE(MaLaptop, SoLuong, DonGia, GiaVon);
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_LICH_TRA_GOP_MaHD_NgayTra' AND object_id = OBJECT_ID('dbo.LICH_TRA_GOP'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_LICH_TRA_GOP_MaHD_NgayTra 
    ON dbo.LICH_TRA_GOP(MaHD, NgayTra, NgayDenHan) 
    INCLUDE(KyThu, SoTienPhaiTra, SoTienDaTra, TienPhat);
END;
GO

-- Chạy đồng bộ tình trạng trễ hạn theo ngày hiện tại
EXEC dbo.sp_CapNhatTreHan;
GO

PRINT N'Script 15: Tối ưu hóa Database và xử lý edge cases hoàn tất thành công!';
GO

/*
    14_cauhinh_and_statistics_seed.sql
    - Sửa lỗi encoding Unicode trong trg_CTHD_Them và sp_TaoLichTraGop
    - Bổ sung các tham số cấu hình trả góp và hạng thành viên vào CAU_HINH nếu chưa có.
    - Bổ sung dữ liệu hóa đơn bán hàng mẫu từ tháng 07/2026 đến tháng 10/2026
      để phục vụ biểu đồ doanh thu và báo cáo thống kê trực quan.
*/
USE QuanLyBanLaptop_V2;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;
SET XACT_ABORT ON;

-- 1. Sửa trigger trg_CTHD_Them để gán đúng TrangThai = N'Đã bán'
EXEC('
CREATE OR ALTER TRIGGER dbo.trg_CTHD_Them
    ON dbo.CHI_TIET_HD
    AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted i JOIN dbo.LAPTOP_DATA l ON l.MaLaptop = i.MaLaptop WHERE l.NgungKinhDoanh = 1)
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50040, N''Hóa đơn có laptop đã ngừng kinh doanh.'', 1;
    END;

    UPDATE c
    SET ThoiGianBaoHanh = ISNULL(c.ThoiGianBaoHanh, l.ThoiGianBaoHanh),
        GiaVon = ISNULL(c.GiaVon, ISNULL((SELECT SUM(p.SoLuong * p.DonGiaNhap) / NULLIF(SUM(p.SoLuong), 0)
                                          FROM dbo.CHI_TIET_PN p
                                          WHERE p.MaLaptop = c.MaLaptop), 0))
    FROM dbo.CHI_TIET_HD c
    JOIN inserted i ON i.MaHD = c.MaHD AND i.MaLaptop = c.MaLaptop
    JOIN dbo.LAPTOP_DATA l ON l.MaLaptop = c.MaLaptop;

    ;WITH need AS (
        SELECT i.MaHD, i.MaLaptop, i.SoLuong,
               SUM(i.SoLuong) OVER (PARTITION BY i.MaLaptop ORDER BY i.MaHD ROWS UNBOUNDED PRECEDING) - i.SoLuong AS Dau
        FROM inserted i
    ),
    kho AS (
        SELECT SerialNo, MaLaptop,
               ROW_NUMBER() OVER (PARTITION BY MaLaptop ORDER BY SerialNo) AS rn
        FROM dbo.LAPTOP_SERIAL_DATA WITH (UPDLOCK)
        WHERE TrangThai = N''Trong kho''
          AND MaLaptop IN (SELECT MaLaptop FROM inserted)
    )
    UPDATE s
    SET MaHD = n.MaHD,
        TrangThai = N''Đã bán''
    FROM dbo.LAPTOP_SERIAL_DATA s
    JOIN kho k ON k.SerialNo = s.SerialNo
    JOIN need n ON n.MaLaptop = k.MaLaptop AND k.rn > n.Dau AND k.rn <= n.Dau + n.SoLuong
    JOIN dbo.CHI_TIET_HD c ON c.MaHD = n.MaHD AND c.MaLaptop = n.MaLaptop;

    DECLARE @DaGan INT = @@ROWCOUNT;
    IF @DaGan < (SELECT SUM(SoLuong) FROM inserted)
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50041, N''Không đủ hàng tồn kho để bán.'', 1;
    END;
END;
');

-- 2. Sửa thủ tục sp_TaoLichTraGop để dùng đúng N'Trả góp'
EXEC('
CREATE OR ALTER PROCEDURE dbo.sp_TaoLichTraGop @MaHD nvarchar(30)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.HOP_DONG_TRA_GOP WHERE MaHD = @MaHD)
        INSERT dbo.HOP_DONG_TRA_GOP(MaHD, TyLeTraTruoc, LaiSuat)
        SELECT MaHD, 0.30, CASE ChuKyTraGop WHEN 3 THEN 2.4 WHEN 6 THEN 4 WHEN 9 THEN 6 ELSE 8 END
        FROM dbo.HOA_DON_DATA WHERE MaHD = @MaHD AND PhuongThucThanhToan = N''Trả góp'';

    DECLARE @tong decimal(18,2), @ky int, @ngay date, @rate decimal(5,2), @down decimal(5,4);
    SELECT @tong = h.TongTien, @ky = h.ChuKyTraGop, @ngay = CAST(h.NgayLap AS date),
           @rate = c.LaiSuat, @down = c.TyLeTraTruoc
    FROM dbo.HOA_DON h
    JOIN dbo.HOP_DONG_TRA_GOP c ON c.MaHD = h.MaHD
    WHERE h.MaHD = @MaHD AND h.PhuongThucThanhToan = N''Trả góp'';

    IF @ky IS NULL THROW 51201, N''Thiếu hóa đơn hoặc điều khoản trả góp.'', 1;
    IF EXISTS(SELECT 1 FROM dbo.LICH_TRA_GOP WHERE MaHD = @MaHD)
        THROW 51202, N''Hóa đơn đã có lịch trả góp.'', 1;

    DECLARE @loan decimal(18,2) = @tong - ROUND(@tong * @down, 0);
    DECLARE @payable decimal(18,2) = @loan + ROUND(@loan * @rate / 100, 0);
    DECLARE @monthly decimal(18,2) = FLOOR(@payable / @ky);
    IF @monthly <= 0 THROW 51203, N''Số tiền quá nhỏ để chia kỳ.'', 1;

    INSERT dbo.LICH_TRA_GOP(MaHD, KyThu, NgayDenHan, SoTienPhaiTra, TienPhat)
    SELECT @MaHD, So, DATEADD(month, So, @ngay),
           CASE WHEN So = @ky THEN @payable - @monthly * (@ky - 1) ELSE @monthly END, 0
    FROM dbo.fn_DaySo(@ky);
END;
');

-- 3. Bổ sung tham số cấu hình trả góp & hạng thẻ vào CAU_HINH
IF NOT EXISTS (SELECT 1 FROM CAU_HINH WHERE Khoa = 'TRA_TRUOC_TOI_THIEU')
    INSERT INTO CAU_HINH (Khoa, GiaTri, MoTa) VALUES ('TRA_TRUOC_TOI_THIEU', '30', N'Tỷ lệ trả trước tối thiểu (%)');

IF NOT EXISTS (SELECT 1 FROM CAU_HINH WHERE Khoa = 'LAI_SUAT_3_THANG')
    INSERT INTO CAU_HINH (Khoa, GiaTri, MoTa) VALUES ('LAI_SUAT_3_THANG', '2.4', N'Lãi suất kỳ hạn 3 tháng (%)');

IF NOT EXISTS (SELECT 1 FROM CAU_HINH WHERE Khoa = 'LAI_SUAT_6_THANG')
    INSERT INTO CAU_HINH (Khoa, GiaTri, MoTa) VALUES ('LAI_SUAT_6_THANG', '4.0', N'Lãi suất kỳ hạn 6 tháng (%)');

IF NOT EXISTS (SELECT 1 FROM CAU_HINH WHERE Khoa = 'LAI_SUAT_9_THANG')
    INSERT INTO CAU_HINH (Khoa, GiaTri, MoTa) VALUES ('LAI_SUAT_9_THANG', '6.0', N'Lãi suất kỳ hạn 9 tháng (%)');

IF NOT EXISTS (SELECT 1 FROM CAU_HINH WHERE Khoa = 'LAI_SUAT_12_THANG')
    INSERT INTO CAU_HINH (Khoa, GiaTri, MoTa) VALUES ('LAI_SUAT_12_THANG', '8.0', N'Lãi suất kỳ hạn 12 tháng (%)');

IF NOT EXISTS (SELECT 1 FROM CAU_HINH WHERE Khoa = 'TIEN_PHAT_NGAY')
    INSERT INTO CAU_HINH (Khoa, GiaTri, MoTa) VALUES ('TIEN_PHAT_NGAY', '20000', N'Tiền phạt trễ hạn mỗi ngày (VND)');

IF NOT EXISTS (SELECT 1 FROM CAU_HINH WHERE Khoa = 'NGUONG_BAC')
    INSERT INTO CAU_HINH (Khoa, GiaTri, MoTa) VALUES ('NGUONG_BAC', '10000000', N'Tổng chi tiêu tối thiểu lên hạng Bạc (VND)');

IF NOT EXISTS (SELECT 1 FROM CAU_HINH WHERE Khoa = 'NGUONG_VANG')
    INSERT INTO CAU_HINH (Khoa, GiaTri, MoTa) VALUES ('NGUONG_VANG', '30000000', N'Tổng chi tiêu tối thiểu lên hạng Vàng (VND)');

IF NOT EXISTS (SELECT 1 FROM CAU_HINH WHERE Khoa = 'NGUONG_VIP')
    INSERT INTO CAU_HINH (Khoa, GiaTri, MoTa) VALUES ('NGUONG_VIP', '50000000', N'Tổng chi tiêu tối thiểu lên hạng VIP (VND)');

PRINT N'Đã hoàn tất kiểm tra và cập nhật bảng CAU_HINH.';

-- 4. Dữ liệu mẫu giao dịch bán hàng từ tháng 07/2026 đến tháng 10/2026
-- Bổ sung thêm serial "Trong kho" nếu máy còn thiếu serial để bán
INSERT INTO dbo.LAPTOP_SERIAL_DATA (SerialNo, MaLaptop, MaPN, TrangThai)
SELECT l.MaLaptop + '-DEMO-' + RIGHT('000' + CAST(n.number AS VARCHAR(3)), 3),
       l.MaLaptop,
       ISNULL((SELECT TOP 1 p.MaPN FROM dbo.CHI_TIET_PN p WHERE p.MaLaptop = l.MaLaptop), 'PN001'),
       N'Trong kho'
FROM dbo.LAPTOP_DATA l
CROSS JOIN (SELECT 1 AS number UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5) n
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.LAPTOP_SERIAL_DATA s 
    WHERE s.SerialNo = l.MaLaptop + '-DEMO-' + RIGHT('000' + CAST(n.number AS VARCHAR(3)), 3)
);

DECLARE @Items dbo.TVP_ChiTietBan;
DECLARE @SoHoaDonMoi INT = 0;

BEGIN TRY
    -- Tháng 07/2026
    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaHD = 'HD005')
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT005', 1);
        EXEC dbo.sp_TaoHoaDon 'HD005', 'KH005', 'NV003', N'Tiền mặt', 0, '2026-07-20T11:00:00', @Items;
        SET @SoHoaDonMoi += 1;
    END;

    -- Tháng 08/2026
    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaHD = 'HD006')
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT006', 1);
        EXEC dbo.sp_TaoHoaDon 'HD006', 'KH001', 'NV002', N'Chuyển khoản', 0, '2026-08-03T10:15:00', @Items;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaHD = 'HD007')
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT008', 1);
        EXEC dbo.sp_TaoHoaDon 'HD007', 'KH002', 'NV003', N'Trả góp', 3, '2026-08-12T13:30:00', @Items;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaHD = 'HD008')
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT010', 1);
        EXEC dbo.sp_TaoHoaDon 'HD008', 'KH004', 'NV002', N'Tiền mặt', 0, '2026-08-27T16:00:00', @Items;
        SET @SoHoaDonMoi += 1;
    END;

    -- Tháng 09/2026
    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaHD = 'HD009')
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT004', 1);
        EXEC dbo.sp_TaoHoaDon 'HD009', 'KH003', 'NV003', N'Chuyển khoản', 0, '2026-09-05T09:30:00', @Items;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaHD = 'HD010')
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT007', 1);
        EXEC dbo.sp_TaoHoaDon 'HD010', 'KH005', 'NV002', N'Trả góp', 6, '2026-09-14T14:15:00', @Items;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaHD = 'HD011')
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT009', 1);
        EXEC dbo.sp_TaoHoaDon 'HD011', 'KH001', 'NV003', N'Tiền mặt', 0, '2026-09-22T16:30:00', @Items;
        SET @SoHoaDonMoi += 1;
    END;

    -- Tháng 10/2026
    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaHD = 'HD012')
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT002', 1);
        EXEC dbo.sp_TaoHoaDon 'HD012', 'KH002', 'NV002', N'Trả góp', 12, '2026-10-16T10:00:00', @Items;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaHD = 'HD013')
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT001', 1);
        EXEC dbo.sp_TaoHoaDon 'HD013', 'KH004', 'NV003', N'Chuyển khoản', 0, '2026-10-20T15:30:00', @Items;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaHD = 'HD014')
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT005', 1);
        EXEC dbo.sp_TaoHoaDon 'HD014', 'KH003', 'NV002', N'Tiền mặt', 0, '2026-10-22T09:45:00', @Items;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaHD = 'HD015')
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT006', 1);
        EXEC dbo.sp_TaoHoaDon 'HD015', 'KH005', 'NV003', N'Tiền mặt', 0, '2026-10-24T11:20:00', @Items;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaHD = 'HD016')
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT008', 1);
        EXEC dbo.sp_TaoHoaDon 'HD016', 'KH001', 'NV002', N'Chuyển khoản', 0, '2026-10-25T09:00:00', @Items;
        SET @SoHoaDonMoi += 1;
    END;

    PRINT N'Đã thêm ' + CAST(@SoHoaDonMoi AS NVARCHAR(10)) + N' hóa đơn mẫu phục vụ thống kê.';
END TRY
BEGIN CATCH
    PRINT N'Lỗi nạp hóa đơn mẫu: ' + ERROR_MESSAGE();
END CATCH;

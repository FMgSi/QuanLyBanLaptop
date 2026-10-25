/*
    Dữ liệu giao dịch mẫu cho màn hình Thống kê doanh thu.
    Chạy an toàn nhiều lần: mỗi hóa đơn được nhận diện bằng ngày giờ + khách hàng + nhân viên.
    Dùng sp_BanHang để đồng bộ hóa đơn, tồn kho, serial, hạng khách và lịch trả góp.
*/
USE QuanLyBanLaptop;
SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @Items dbo.TVP_BanHang;
DECLARE @MaHD VARCHAR(10);
DECLARE @SoHoaDonMoi INT = 0;

BEGIN TRY
    BEGIN TRANSACTION;

    -- Tháng 07: thêm mốc doanh thu và sản phẩm văn phòng.
    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaKH='KH005' AND MaNV='NV003' AND NgayLap=CONVERT(DATETIME,'2026-07-20T11:00:00',126))
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT005',1);
        EXEC dbo.sp_BanHang 'KH005','NV003',N'Tiền mặt',0,@Items,'2026-07-20T11:00:00',@MaHD OUTPUT;
        SET @SoHoaDonMoi += 1;
    END;

    -- Tháng 08: nhiều nhân viên, sản phẩm và phương thức thanh toán.
    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaKH='KH001' AND MaNV='NV002' AND NgayLap=CONVERT(DATETIME,'2026-08-03T10:15:00',126))
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT006',1);
        EXEC dbo.sp_BanHang 'KH001','NV002',N'Chuyển khoản',0,@Items,'2026-08-03T10:15:00',@MaHD OUTPUT;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaKH='KH002' AND MaNV='NV003' AND NgayLap=CONVERT(DATETIME,'2026-08-12T13:30:00',126))
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT008',1),('LT005',1);
        EXEC dbo.sp_BanHang 'KH002','NV003',N'Trả góp',3,@Items,'2026-08-12T13:30:00',@MaHD OUTPUT;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaKH='KH004' AND MaNV='NV002' AND NgayLap=CONVERT(DATETIME,'2026-08-27T16:00:00',126))
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT010',1);
        EXEC dbo.sp_BanHang 'KH004','NV002',N'Tiền mặt',0,@Items,'2026-08-27T16:00:00',@MaHD OUTPUT;
        SET @SoHoaDonMoi += 1;
    END;

    -- Tháng 09: thêm ngày bán khác nhau và một hóa đơn trả góp 6 tháng.
    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaKH='KH003' AND MaNV='NV003' AND NgayLap=CONVERT(DATETIME,'2026-09-05T09:30:00',126))
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT004',1);
        EXEC dbo.sp_BanHang 'KH003','NV003',N'Chuyển khoản',0,@Items,'2026-09-05T09:30:00',@MaHD OUTPUT;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaKH='KH005' AND MaNV='NV002' AND NgayLap=CONVERT(DATETIME,'2026-09-14T14:15:00',126))
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT007',1),('LT003',1);
        EXEC dbo.sp_BanHang 'KH005','NV002',N'Trả góp',6,@Items,'2026-09-14T14:15:00',@MaHD OUTPUT;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaKH='KH001' AND MaNV='NV003' AND NgayLap=CONVERT(DATETIME,'2026-09-22T16:30:00',126))
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT009',1);
        EXEC dbo.sp_BanHang 'KH001','NV003',N'Tiền mặt',0,@Items,'2026-09-22T16:30:00',@MaHD OUTPUT;
        SET @SoHoaDonMoi += 1;
    END;

    -- Tháng 10: thêm nhiều ngày để xem chi tiết theo ngày trong tháng hiện tại.
    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaKH='KH002' AND MaNV='NV002' AND NgayLap=CONVERT(DATETIME,'2026-10-16T10:00:00',126))
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT002',1);
        EXEC dbo.sp_BanHang 'KH002','NV002',N'Trả góp',12,@Items,'2026-10-16T10:00:00',@MaHD OUTPUT;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaKH='KH004' AND MaNV='NV003' AND NgayLap=CONVERT(DATETIME,'2026-10-20T15:30:00',126))
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT001',1);
        EXEC dbo.sp_BanHang 'KH004','NV003',N'Chuyển khoản',0,@Items,'2026-10-20T15:30:00',@MaHD OUTPUT;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaKH='KH003' AND MaNV='NV002' AND NgayLap=CONVERT(DATETIME,'2026-10-22T09:45:00',126))
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT005',1);
        EXEC dbo.sp_BanHang 'KH003','NV002',N'Tiền mặt',0,@Items,'2026-10-22T09:45:00',@MaHD OUTPUT;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaKH='KH005' AND MaNV='NV003' AND NgayLap=CONVERT(DATETIME,'2026-10-24T11:20:00',126))
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT006',1);
        EXEC dbo.sp_BanHang 'KH005','NV003',N'Tiền mặt',0,@Items,'2026-10-24T11:20:00',@MaHD OUTPUT;
        SET @SoHoaDonMoi += 1;
    END;

    IF NOT EXISTS (SELECT 1 FROM HOA_DON WHERE MaKH='KH001' AND MaNV='NV002' AND NgayLap=CONVERT(DATETIME,'2026-10-25T09:00:00',126))
    BEGIN
        DELETE FROM @Items; INSERT @Items VALUES ('LT008',1);
        EXEC dbo.sp_BanHang 'KH001','NV002',N'Chuyển khoản',0,@Items,'2026-10-25T09:00:00',@MaHD OUTPUT;
        SET @SoHoaDonMoi += 1;
    END;

    COMMIT TRANSACTION;
    PRINT N'Đã thêm ' + CAST(@SoHoaDonMoi AS NVARCHAR(10)) + N' hóa đơn mẫu; các hóa đơn đã có được giữ nguyên.';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;

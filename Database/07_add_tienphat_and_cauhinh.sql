/* =====================================================================
   07_add_tienphat_and_cauhinh.sql
   - Thêm cột TienPhat vào LICH_TRA_GOP (phục vụ quản lý trả góp, thu tiền phạt trễ hạn)
   - Tạo bảng CAU_HINH nếu chưa có (lưu mức phạt TIEN_PHAT_NGAY, ngưỡng thành viên)
   - Cập nhật sp_ThanhToanKy hỗ trợ tham số @TienPhat và @MaHD NVARCHAR(30)
   - Cập nhật sp_TaoLichTraGop hỗ trợ @MaHD NVARCHAR(30)
   - Bổ sung hàm fn_BaoCaoDoanhThu cho module Thống Kê
   - Bổ sung hàm fn_TongChiTieu, fn_HangThanhVien, fn_ConBaoHanh
   ===================================================================== */
USE QuanLyBanLaptop_V2;
GO
SET NOCOUNT ON;

/* 1. Thêm cột TienPhat vào LICH_TRA_GOP */
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('LICH_TRA_GOP') AND name = 'TienPhat')
BEGIN
    ALTER TABLE LICH_TRA_GOP ADD TienPhat DECIMAL(18,2) NOT NULL DEFAULT 0;
END
GO

/* 2. Tạo bảng CAU_HINH và nạp dữ liệu mặc định */
IF OBJECT_ID('dbo.CAU_HINH', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.CAU_HINH (
        Khoa   VARCHAR(50)    PRIMARY KEY,
        GiaTri NVARCHAR(100)  NOT NULL,
        MoTa   NVARCHAR(200)  NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM CAU_HINH WHERE Khoa = 'TIEN_PHAT_NGAY')
    INSERT INTO CAU_HINH (Khoa, GiaTri, MoTa) VALUES ('TIEN_PHAT_NGAY', N'20000', N'Tiền phạt trễ hạn mỗi ngày (VNĐ)');

IF NOT EXISTS (SELECT 1 FROM CAU_HINH WHERE Khoa = 'NGUONG_BAC')
    INSERT INTO CAU_HINH (Khoa, GiaTri, MoTa) VALUES ('NGUONG_BAC', N'10000000', N'Tổng chi tiêu tối thiểu để lên hạng Bạc');

IF NOT EXISTS (SELECT 1 FROM CAU_HINH WHERE Khoa = 'NGUONG_VANG')
    INSERT INTO CAU_HINH (Khoa, GiaTri, MoTa) VALUES ('NGUONG_VANG', N'30000000', N'Tổng chi tiêu tối thiểu để lên hạng Vàng');

IF NOT EXISTS (SELECT 1 FROM CAU_HINH WHERE Khoa = 'NGUONG_VIP')
    INSERT INTO CAU_HINH (Khoa, GiaTri, MoTa) VALUES ('NGUONG_VIP', N'50000000', N'Tổng chi tiêu tối thiểu để lên hạng VIP');
GO

/* 3. Cập nhật sp_ThanhToanKy hỗ trợ tham số @TienPhat và @MaHD NVARCHAR(30) */
IF OBJECT_ID('dbo.sp_ThanhToanKy', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_ThanhToanKy;
GO
CREATE PROCEDURE dbo.sp_ThanhToanKy
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
        UPDATE LICH_TRA_GOP
        SET NgayTra = ISNULL(@NgayTra, CAST(GETDATE() AS DATE)),
            SoTienDaTra = SoTienPhaiTra,
            TienPhat = ISNULL(@TienPhat, 0)
        WHERE MaHD = @MaHD AND KyThu = @KyThu AND NgayTra IS NULL;
        
        IF @@ROWCOUNT = 0 THROW 50040, N'Kỳ trả góp không tồn tại hoặc đã được thanh toán.', 1;

        IF NOT EXISTS (SELECT 1 FROM LICH_TRA_GOP WHERE MaHD = @MaHD AND NgayTra IS NULL)
            UPDATE HOA_DON SET TinhTrang = N'Đã thanh toán' WHERE MaHD = @MaHD;
        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

/* 4. Cập nhật sp_TaoLichTraGop hỗ trợ @MaHD NVARCHAR(30) */
IF OBJECT_ID('dbo.sp_TaoLichTraGop', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_TaoLichTraGop;
GO
CREATE PROCEDURE dbo.sp_TaoLichTraGop @MaHD NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @tong DECIMAL(18,2), @ky INT, @ngay DATE;
    SELECT @tong = TongTien, @ky = ChuKyTraGop, @ngay = CAST(NgayLap AS DATE)
    FROM HOA_DON WHERE MaHD = @MaHD AND PhuongThucThanhToan = N'Trả góp';

    IF @ky IS NULL THROW 50030, N'Hóa đơn không tồn tại hoặc không phải trả góp.', 1;
    IF EXISTS (SELECT 1 FROM LICH_TRA_GOP WHERE MaHD = @MaHD) THROW 50031, N'Hóa đơn đã có lịch trả góp.', 1;

    DECLARE @moiKy DECIMAL(18,2) = ROUND(@tong / @ky, 0);
    INSERT LICH_TRA_GOP (MaHD, KyThu, NgayDenHan, SoTienPhaiTra, TienPhat)
    SELECT @MaHD, d.So, DATEADD(MONTH, d.So, @ngay),
           CASE WHEN d.So = @ky THEN @tong - @moiKy * (@ky - 1) ELSE @moiKy END,
           0
    FROM dbo.fn_DaySo(@ky) d;
END
GO

/* 5. Bổ sung hàm fn_BaoCaoDoanhThu cho module Thống Kê */
IF OBJECT_ID('dbo.fn_BaoCaoDoanhThu', 'IF') IS NOT NULL DROP FUNCTION dbo.fn_BaoCaoDoanhThu;
GO
CREATE FUNCTION dbo.fn_BaoCaoDoanhThu (@Tu DATE, @Den DATE)
RETURNS TABLE
AS RETURN
    SELECT CAST(hd.NgayLap AS DATE)                     AS Ngay,
           COUNT(DISTINCT hd.MaHD)                      AS SoHoaDon,
           SUM(ct.SoLuong)                              AS SoMayBan,
           SUM(ct.SoLuong * ct.DonGia)                  AS DoanhThu,
           SUM(ct.SoLuong * ct.GiaVon)                  AS GiaVon,
           SUM(ct.SoLuong * (ct.DonGia - ct.GiaVon))    AS LoiNhuan
    FROM HOA_DON hd
    JOIN CHI_TIET_HD ct ON ct.MaHD = hd.MaHD
    WHERE hd.NgayLap >= @Tu AND hd.NgayLap < DATEADD(DAY, 1, CAST(@Den AS DATETIME))
    GROUP BY CAST(hd.NgayLap AS DATE);
GO

/* 6. Bổ sung các hàm phụ trợ fn_TongChiTieu, fn_HangThanhVien, fn_ConBaoHanh */
IF OBJECT_ID('dbo.fn_ConBaoHanh', 'FN') IS NULL
EXEC('
CREATE FUNCTION dbo.fn_ConBaoHanh (@SerialNo VARCHAR(40), @Ngay DATETIME)
RETURNS BIT
AS
BEGIN
    DECLARE @het DATE = (SELECT NgayHetBH FROM LAPTOP_SERIAL WHERE SerialNo = @SerialNo);
    RETURN CASE WHEN @het IS NOT NULL AND CAST(@Ngay AS DATE) <= @het THEN 1 ELSE 0 END;
END');
GO

IF OBJECT_ID('dbo.fn_TongChiTieu', 'FN') IS NULL
EXEC('
CREATE FUNCTION dbo.fn_TongChiTieu (@MaKH VARCHAR(10))
RETURNS DECIMAL(18,2)
AS
BEGIN
    RETURN (SELECT ISNULL(SUM(TongTien), 0) FROM HOA_DON WHERE MaKH = @MaKH);
END');
GO

IF OBJECT_ID('dbo.fn_HangThanhVien', 'FN') IS NULL
EXEC('
CREATE FUNCTION dbo.fn_HangThanhVien (@TongChiTieu DECIMAL(18,2))
RETURNS NVARCHAR(30)
AS
BEGIN
    DECLARE @bac  DECIMAL(18,2) = ISNULL((SELECT CAST(GiaTri AS DECIMAL(18,2)) FROM CAU_HINH WHERE Khoa = ''NGUONG_BAC''), 10000000);
    DECLARE @vang DECIMAL(18,2) = ISNULL((SELECT CAST(GiaTri AS DECIMAL(18,2)) FROM CAU_HINH WHERE Khoa = ''NGUONG_VANG''), 30000000);
    DECLARE @vip  DECIMAL(18,2) = ISNULL((SELECT CAST(GiaTri AS DECIMAL(18,2)) FROM CAU_HINH WHERE Khoa = ''NGUONG_VIP''), 50000000);
    RETURN CASE WHEN @TongChiTieu >= @vip  THEN N''VIP''
                WHEN @TongChiTieu >= @vang THEN N''Vàng''
                WHEN @TongChiTieu >= @bac  THEN N''Bạc''
                ELSE N''Đồng'' END;
END');
GO

PRINT N'Script 07_add_tienphat_and_cauhinh.sql hoan tat!';

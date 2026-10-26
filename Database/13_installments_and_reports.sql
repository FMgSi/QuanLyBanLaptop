-- Chốt điều khoản từng hợp đồng; giữ lịch sử các lịch trả góp cũ.
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET XACT_ABORT ON;
BEGIN TRY
BEGIN TRAN;
IF OBJECT_ID('dbo.HOA_DON_DATA','U') IS NULL THROW 51200,N'Cần migration 11.',1;
IF OBJECT_ID('dbo.HOP_DONG_TRA_GOP','U') IS NULL
BEGIN
    CREATE TABLE dbo.HOP_DONG_TRA_GOP(
        MaHD nvarchar(30) NOT NULL PRIMARY KEY REFERENCES dbo.HOA_DON_DATA(MaHD),
        TyLeTraTruoc decimal(5,4) NOT NULL CHECK(TyLeTraTruoc>=0 AND TyLeTraTruoc<1),
        LaiSuat decimal(5,2) NOT NULL CHECK(LaiSuat>=0),
        LaHopDongCu bit NOT NULL DEFAULT 0);
    -- Hợp đồng cũ giữ nguyên lịch DB: không tự giả định đã thu trước 30%.
    INSERT dbo.HOP_DONG_TRA_GOP(MaHD,TyLeTraTruoc,LaiSuat,LaHopDongCu)
        SELECT MaHD,0,0,1 FROM dbo.HOA_DON_DATA WHERE PhuongThucThanhToan=N'Trả góp';
END;
EXEC(N'CREATE OR ALTER PROCEDURE dbo.sp_TaoLichTraGop @MaHD nvarchar(30)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.HOP_DONG_TRA_GOP WHERE MaHD=@MaHD)
        INSERT dbo.HOP_DONG_TRA_GOP(MaHD,TyLeTraTruoc,LaiSuat)
        SELECT MaHD,0.30,CASE ChuKyTraGop WHEN 3 THEN 2.4 WHEN 6 THEN 4 WHEN 9 THEN 6 ELSE 8 END
        FROM dbo.HOA_DON_DATA WHERE MaHD=@MaHD AND PhuongThucThanhToan=N''Trả góp'';
    DECLARE @tong decimal(18,2),@ky int,@ngay date,@rate decimal(5,2),@down decimal(5,4);
    SELECT @tong=h.TongTien,@ky=h.ChuKyTraGop,@ngay=CAST(h.NgayLap AS date),
        @rate=c.LaiSuat,@down=c.TyLeTraTruoc
    FROM dbo.HOA_DON h JOIN dbo.HOP_DONG_TRA_GOP c ON c.MaHD=h.MaHD
    WHERE h.MaHD=@MaHD AND h.PhuongThucThanhToan=N''Trả góp'';
    IF @ky IS NULL THROW 51201,N''Thiếu hóa đơn hoặc điều khoản trả góp.'',1;
    IF EXISTS(SELECT 1 FROM dbo.LICH_TRA_GOP WHERE MaHD=@MaHD)
        THROW 51202,N''Hóa đơn đã có lịch trả góp.'',1;
    DECLARE @loan decimal(18,2)=@tong-ROUND(@tong*@down,0);
    DECLARE @payable decimal(18,2)=@loan+ROUND(@loan*@rate/100,0);
    DECLARE @monthly decimal(18,2)=FLOOR(@payable/@ky);
    IF @monthly<=0 THROW 51203,N''Số tiền quá nhỏ để chia kỳ.'',1;
    INSERT dbo.LICH_TRA_GOP(MaHD,KyThu,NgayDenHan,SoTienPhaiTra,TienPhat)
    SELECT @MaHD,So,DATEADD(month,So,@ngay),
        CASE WHEN So=@ky THEN @payable-@monthly*(@ky-1) ELSE @monthly END,0
    FROM dbo.fn_DaySo(@ky);
END;');
EXEC(N'CREATE OR ALTER VIEW dbo.vw_CongNo AS
SELECT h.MaHD,h.MaKH,k.TenKH,h.TongTien,
    SUM(l.SoTienDaTra) AS DaTra,
    SUM(l.SoTienPhaiTra-l.SoTienDaTra) AS ConNo,
    SUM(CASE WHEN l.NgayTra IS NULL AND l.NgayDenHan<CAST(GETDATE() AS date) THEN 1 ELSE 0 END) AS SoKyTre,
    MIN(CASE WHEN l.NgayTra IS NULL THEN l.NgayDenHan END) AS KyDenHanGanNhat
FROM dbo.HOA_DON h JOIN dbo.KHACH_HANG k ON k.MaKH=h.MaKH
JOIN dbo.LICH_TRA_GOP l ON l.MaHD=h.MaHD GROUP BY h.MaHD,h.MaKH,k.TenKH,h.TongTien;');

-- Báo cáo doanh thu dùng giá sau giảm, giá vốn đã chốt tại giao dịch.
EXEC(N'CREATE OR ALTER FUNCTION dbo.fn_BaoCaoDoanhThu(@Tu date,@Den date)
RETURNS TABLE AS RETURN
SELECT CAST(h.NgayLap AS date) Ngay,COUNT(DISTINCT h.MaHD) SoHoaDon,
 SUM(c.SoLuong) SoMayBan,SUM(c.ThanhTien) DoanhThu,SUM(c.SoLuong*c.GiaVon) GiaVon,
 SUM(c.ThanhTien-c.SoLuong*c.GiaVon) LoiNhuan
FROM dbo.HOA_DON h JOIN dbo.CHI_TIET_HD c ON c.MaHD=h.MaHD
WHERE h.NgayLap>=@Tu AND h.NgayLap<DATEADD(day,1,CAST(@Den AS datetime))
GROUP BY CAST(h.NgayLap AS date);');
EXEC(N'CREATE OR ALTER VIEW dbo.vw_DoanhThuThang AS
SELECT YEAR(hd.NgayLap) AS Nam, MONTH(hd.NgayLap) AS Thang,
       COUNT(DISTINCT hd.MaHD)                   AS SoHoaDon,
       SUM(ct.SoLuong)                           AS SoMayBan,
       SUM(ct.ThanhTien)               AS DoanhThu,
       SUM(ct.SoLuong * ct.GiaVon)               AS GiaVon,
       SUM(ct.ThanhTien - ct.SoLuong * ct.GiaVon) AS LoiNhuan
FROM HOA_DON hd JOIN CHI_TIET_HD ct ON ct.MaHD = hd.MaHD
GROUP BY YEAR(hd.NgayLap), MONTH(hd.NgayLap);');
EXEC(N'CREATE OR ALTER VIEW dbo.vw_DoanhThuLaptop AS
SELECT l.MaLaptop, l.TenLaptop,
       SUM(ct.SoLuong)                           AS SoMayBan,
       SUM(ct.ThanhTien)               AS DoanhThu,
       SUM(ct.ThanhTien - ct.SoLuong * ct.GiaVon) AS LoiNhuan
FROM CHI_TIET_HD ct JOIN LAPTOP l ON l.MaLaptop = ct.MaLaptop
GROUP BY l.MaLaptop, l.TenLaptop;');
EXEC(N'CREATE OR ALTER VIEW dbo.vw_DoanhThuNhanVien AS
SELECT nv.MaNV, nv.TenNV,
       COUNT(DISTINCT hd.MaHD)                   AS SoHoaDon,
       SUM(ct.ThanhTien)               AS DoanhThu,
       SUM(ct.ThanhTien - ct.SoLuong * ct.GiaVon) AS LoiNhuan
FROM HOA_DON hd
JOIN CHI_TIET_HD ct ON ct.MaHD = hd.MaHD
JOIN NHAN_VIEN nv ON nv.MaNV = hd.MaNV
GROUP BY nv.MaNV, nv.TenNV;');
COMMIT;
END TRY
BEGIN CATCH
IF @@TRANCOUNT>0 ROLLBACK;
THROW;
END CATCH;


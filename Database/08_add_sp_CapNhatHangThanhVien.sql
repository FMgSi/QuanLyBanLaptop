/* =====================================================================
   MIGRATION 08: BỔ SUNG STORED PROCEDURE sp_CapNhatHangThanhVien
   Tự động cập nhật hạng thành viên (Đồng, Bạc, Vàng, VIP) theo tổng chi tiêu
   ===================================================================== */

USE QuanLyBanLaptop_V2;
GO

-- 1. Bổ sung hàm fn_TongChiTieu nếu chưa có
IF OBJECT_ID('dbo.fn_TongChiTieu', 'FN') IS NULL
BEGIN
    EXEC('CREATE FUNCTION dbo.fn_TongChiTieu (@MaKH VARCHAR(10))
    RETURNS DECIMAL(18,2)
    AS
    BEGIN
        DECLARE @Tong DECIMAL(18,2);
        SELECT @Tong = ISNULL(SUM(TongTien), 0)
        FROM HOA_DON_BAN
        WHERE MaKH = @MaKH AND TrangThai IN (N''Đã thanh toán'', N''Đang trả góp'', N''Hoàn tất'');
        RETURN @Tong;
    END');
END
GO

-- 2. Bổ sung hàm fn_HangThanhVien nếu chưa có
IF OBJECT_ID('dbo.fn_HangThanhVien', 'FN') IS NULL
BEGIN
    EXEC('CREATE FUNCTION dbo.fn_HangThanhVien (@TongChiTieu DECIMAL(18,2))
    RETURNS NVARCHAR(20)
    AS
    BEGIN
        IF @TongChiTieu >= 50000000 RETURN N''VIP'';
        IF @TongChiTieu >= 20000000 RETURN N''Vàng'';
        IF @TongChiTieu >= 5000000  RETURN N''Bạc'';
        RETURN N''Đồng'';
    END');
END
GO

-- 3. Tạo / cập nhật Stored Procedure sp_CapNhatHangThanhVien
IF OBJECT_ID('dbo.sp_CapNhatHangThanhVien', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CapNhatHangThanhVien;
GO

CREATE PROCEDURE dbo.sp_CapNhatHangThanhVien
    @MaKH VARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE kh
    SET HangThanhVien = dbo.fn_HangThanhVien(dbo.fn_TongChiTieu(kh.MaKH))
    FROM KHACH_HANG kh
    WHERE @MaKH IS NULL OR kh.MaKH = @MaKH;
END
GO

-- 4. Chạy cập nhật một lượt cho tất cả khách hàng hiện có
EXEC dbo.sp_CapNhatHangThanhVien;
GO

PRINT N'Migration 08 completed successfully: sp_CapNhatHangThanhVien created and all customer tiers updated.';
GO

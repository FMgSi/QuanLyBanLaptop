-- =============================================================
-- Migration 09: Đồng bộ seq_MaNV và nâng cấp sp_ThemNhanVien
-- Tránh lỗi trùng khóa chính và hỗ trợ sinh mã NVxx tự động hiển thị trên form
-- =============================================================
USE QuanLyBanLaptop_V2;
GO

DECLARE @maxId INT = 0;
SELECT @maxId = ISNULL(MAX(CAST(SUBSTRING(MaNV, 3, 10) AS INT)), 0) FROM NHAN_VIEN WHERE MaNV LIKE 'NV%';
DECLARE @nextVal INT = @maxId + 1;
DECLARE @sql NVARCHAR(MAX) = N'ALTER SEQUENCE dbo.seq_MaNV RESTART WITH ' + CAST(@nextVal AS NVARCHAR(10)) + ';';
EXEC sp_executesql @sql;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ThemNhanVien
    @MaNV      VARCHAR(10) OUTPUT,
    @TenNV     NVARCHAR(50),
    @GioiTinh  NVARCHAR(10),
    @NgaySinh  DATE = NULL,
    @SDT       VARCHAR(15) = NULL,
    @DiaChi    NVARCHAR(200) = NULL,
    @ChucVu    NVARCHAR(50),
    @TenDN     VARCHAR(50),
    @MatKhau   NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF LEN(LTRIM(RTRIM(@TenNV))) = 0 THROW 50200, N'Tên nhân viên không được để trống.', 1;
    IF @GioiTinh NOT IN (N'Nam', N'Nữ') THROW 50201, N'Giới tính không hợp lệ.', 1;
    IF @ChucVu NOT IN (N'Quản trị viên', N'Nhân viên') THROW 50202, N'Chức vụ không hợp lệ.', 1;
    IF LEN(LTRIM(RTRIM(@TenDN))) = 0 THROW 50203, N'Tên đăng nhập không được để trống.', 1;
    IF EXISTS (SELECT 1 FROM NHAN_VIEN WHERE TenDN = @TenDN) THROW 50204, N'Tên đăng nhập đã tồn tại.', 1;

    IF @MaNV IS NULL OR @MaNV = '' OR @MaNV = N'(Tự động)'
    BEGIN
        DECLARE @so INT = NEXT VALUE FOR dbo.seq_MaNV;
        SET @MaNV = dbo.fn_Ma('NV', @so, 3);
    END

    WHILE EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = @MaNV)
    BEGIN
        DECLARE @nextSo INT = NEXT VALUE FOR dbo.seq_MaNV;
        SET @MaNV = dbo.fn_Ma('NV', @nextSo, 3);
    END

    INSERT NHAN_VIEN (MaNV, TenNV, GioiTinh, NgaySinh, SDT, DiaChi, ChucVu, TenDN, MatKhau)
    VALUES (@MaNV, @TenNV, @GioiTinh, @NgaySinh, NULLIF(@SDT,''), @DiaChi, @ChucVu, @TenDN, '');
    EXEC dbo.sp_DatMatKhau @MaNV, @MatKhau;
END
GO

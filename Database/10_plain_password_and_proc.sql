USE QuanLyBanLaptop_V2;
GO

-- 1. Cap nhat mat khau plain text cho cac nhan vien hien co
UPDATE NHAN_VIEN SET MatKhau = 'admin123' WHERE TenDN = 'admin';
UPDATE NHAN_VIEN SET MatKhau = '123456' WHERE TenDN = 'binh_sale';
UPDATE NHAN_VIEN SET MatKhau = '123456' WHERE TenDN = 'cuong_kho';
UPDATE NHAN_VIEN SET MatKhau = '123456' WHERE MatKhau LIKE '%$%' OR LEN(MatKhau) > 30;
GO

-- 2. sp_DatMatKhau: Luu truc tiep mat khau khong hash
CREATE OR ALTER PROCEDURE dbo.sp_DatMatKhau
    @MaNV        VARCHAR(10),
    @MatKhauMoi NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE NHAN_VIEN SET MatKhau = @MatKhauMoi WHERE MaNV = @MaNV;
    IF @@ROWCOUNT = 0 THROW 50002, N'Nhan vien khong ton tai.', 1;
END
GO

-- 3. sp_ThemNhanVien: Luu truc tiep mat khau khong hash
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
    IF LEN(LTRIM(RTRIM(@TenNV))) = 0 THROW 50200, N'Ten nhan vien khong duoc de trong.', 1;
    IF @GioiTinh NOT IN (N'Nam', N'Nữ') THROW 50201, N'Gioi tinh khong hop le.', 1;
    IF @ChucVu NOT IN (N'Quản trị viên', N'Nhân viên') THROW 50202, N'Chuc vu khong hop le.', 1;
    IF LEN(LTRIM(RTRIM(@TenDN))) = 0 THROW 50203, N'Ten dang nhap khong duoc de trong.', 1;
    IF EXISTS (SELECT 1 FROM NHAN_VIEN WHERE TenDN = @TenDN) THROW 50204, N'Ten dang nhap da ton tai.', 1;

    IF @MaNV IS NULL OR @MaNV = '' OR @MaNV = N'(Tự động)'
    BEGIN
        DECLARE @nextSo INT = NEXT VALUE FOR dbo.seq_MaNV;
        WHILE EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = dbo.fn_Ma('NV', @nextSo, 3))
        BEGIN
            SET @nextSo = NEXT VALUE FOR dbo.seq_MaNV;
        END
        SET @MaNV = dbo.fn_Ma('NV', @nextSo, 3);
    END

    INSERT NHAN_VIEN (MaNV, TenNV, GioiTinh, NgaySinh, SDT, DiaChi, ChucVu, TenDN, MatKhau)
    VALUES (@MaNV, @TenNV, @GioiTinh, @NgaySinh, NULLIF(@SDT,''), @DiaChi, @ChucVu, @TenDN, @MatKhau);
END
GO

-- 4. sp_CapNhatNhanVien: Cho phep sua mat khau truc tiep
CREATE OR ALTER PROCEDURE dbo.sp_CapNhatNhanVien
    @MaNV     VARCHAR(10),
    @TenNV    NVARCHAR(50),
    @GioiTinh NVARCHAR(10),
    @NgaySinh DATE = NULL,
    @SDT      VARCHAR(15) = NULL,
    @DiaChi   NVARCHAR(200) = NULL,
    @ChucVu   NVARCHAR(50),
    @TenDN    VARCHAR(50),
    @MatKhau  NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = @MaNV)
        THROW 50205, N'Nhan vien khong ton tai.', 1;
    IF LEN(LTRIM(RTRIM(@TenNV))) = 0 THROW 50200, N'Ten nhan vien khong duoc de trong.', 1;
    IF @GioiTinh NOT IN (N'Nam', N'Nữ') THROW 50201, N'Gioi tinh khong hop le.', 1;
    IF @ChucVu NOT IN (N'Quản trị viên', N'Nhân viên') THROW 50202, N'Chuc vu khong hop le.', 1;
    IF EXISTS (SELECT 1 FROM NHAN_VIEN WHERE TenDN = @TenDN AND MaNV <> @MaNV)
        THROW 50204, N'Ten dang nhap da ton tai.', 1;

    UPDATE NHAN_VIEN
    SET TenNV     = @TenNV,
        GioiTinh  = @GioiTinh,
        NgaySinh  = @NgaySinh,
        SDT       = NULLIF(@SDT,''),
        DiaChi    = @DiaChi,
        ChucVu    = @ChucVu,
        TenDN     = @TenDN,
        MatKhau   = CASE WHEN @MatKhau IS NOT NULL AND @MatKhau <> '' THEN @MatKhau ELSE MatKhau END
    WHERE MaNV = @MaNV;
END
GO

-- 5. sp_TimKiemNhanVien: Tra ve them cot MatKhau
CREATE OR ALTER PROCEDURE dbo.sp_TimKiemNhanVien
    @TuKhoa NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaNV, TenNV, GioiTinh,
           CONVERT(NVARCHAR(10), NgaySinh, 103) AS NgaySinh,
           SDT, DiaChi, ChucVu, TenDN, MatKhau,
           CASE DaNghi WHEN 1 THEN N'Đã nghỉ' ELSE N'Đang làm' END AS TrangThai
    FROM NHAN_VIEN
    WHERE @TuKhoa IS NULL OR @TuKhoa = ''
       OR TenNV LIKE N'%' + @TuKhoa + N'%'
       OR MaNV  LIKE N'%' + @TuKhoa + N'%'
       OR SDT   LIKE N'%' + @TuKhoa + N'%'
    ORDER BY MaNV;
END
GO

-- 6. sp_DangNhap: Kiem tra truc tiep mat khau plain text hoac fallback
CREATE OR ALTER PROCEDURE dbo.sp_DangNhap
    @TenDN   VARCHAR(50),
    @MatKhau NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaNV, TenNV, ChucVu, TenDN
    FROM NHAN_VIEN
    WHERE TenDN = @TenDN
      AND (MatKhau = @MatKhau OR dbo.fn_KiemTraMatKhau(MatKhau, @MatKhau) = 1)
      AND DaNghi = 0;
END
GO

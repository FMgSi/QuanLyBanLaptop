/* =====================================================================
   04_add_gioitinh.sql
   - Thêm cột GioiTinh vào NHAN_VIEN và KHACH_HANG
   - Thêm cột NgaySinh vào KHACH_HANG
   - Thêm cột DiaChi vào NHAN_VIEN (nếu chưa có)
   - Tạo stored procedure CRUD cho KHACH_HANG và NHAN_VIEN (đầy đủ)
   Chạy script này trên database QuanLyBanLaptop đã có sẵn.
   ===================================================================== */
USE QuanLyBanLaptop;
GO
SET NOCOUNT ON;

/* 1. Thêm cột GioiTinh vào NHAN_VIEN */
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('NHAN_VIEN') AND name = 'GioiTinh')
    ALTER TABLE NHAN_VIEN ADD GioiTinh NVARCHAR(10) NOT NULL DEFAULT N'Nam'
        CONSTRAINT CK_NV_GioiTinh CHECK (GioiTinh IN (N'Nam', N'Nữ'));
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('NHAN_VIEN') AND name = 'NgaySinh')
    ALTER TABLE NHAN_VIEN ADD NgaySinh DATE NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('NHAN_VIEN') AND name = 'DiaChi')
    ALTER TABLE NHAN_VIEN ADD DiaChi NVARCHAR(200) NULL;
GO

/* 2. Thêm cột GioiTinh + NgaySinh + DaKhoa vào KHACH_HANG */
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('KHACH_HANG') AND name = 'GioiTinh')
    ALTER TABLE KHACH_HANG ADD GioiTinh NVARCHAR(10) NOT NULL DEFAULT N'Nam'
        CONSTRAINT CK_KH_GioiTinh CHECK (GioiTinh IN (N'Nam', N'Nữ'));
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('KHACH_HANG') AND name = 'NgaySinh')
    ALTER TABLE KHACH_HANG ADD NgaySinh DATE NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('KHACH_HANG') AND name = 'DaKhoa')
    ALTER TABLE KHACH_HANG ADD DaKhoa BIT NOT NULL DEFAULT 0;
GO

/* 3. SP CRUD KHACH_HANG */

IF OBJECT_ID('dbo.sp_ThemKhachHang', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_ThemKhachHang;
GO
CREATE PROCEDURE dbo.sp_ThemKhachHang
    @MaKH          VARCHAR(10),
    @TenKH         NVARCHAR(50),
    @GioiTinh      NVARCHAR(10),
    @NgaySinh      DATE = NULL,
    @Sdt           VARCHAR(15) = NULL,
    @DiaChi        NVARCHAR(200) = NULL,
    @HangThanhVien NVARCHAR(30) = N'Đồng'
AS
BEGIN
    SET NOCOUNT ON;
    SET @MaKH = UPPER(LTRIM(RTRIM(@MaKH)));
    IF LEN(@MaKH) = 0 THROW 50105, N'Mã khách hàng không được để trống.', 1;
    IF EXISTS (SELECT 1 FROM KHACH_HANG WHERE UPPER(MaKH) = @MaKH)
        THROW 50106, N'Mã khách hàng đã tồn tại trong hệ thống. Vui lòng nhập mã khác.', 1;
    IF LEN(LTRIM(RTRIM(@TenKH))) = 0 THROW 50100, N'Tên khách hàng không được để trống.', 1;
    IF @GioiTinh NOT IN (N'Nam', N'Nữ') THROW 50101, N'Giới tính không hợp lệ (Nam / Nữ).', 1;
    IF @HangThanhVien NOT IN (N'Đồng', N'Bạc', N'Vàng', N'VIP') SET @HangThanhVien = N'Đồng';
    IF @Sdt IS NOT NULL AND @Sdt <> '' AND EXISTS (SELECT 1 FROM KHACH_HANG WHERE Sdt = @Sdt)
        THROW 50102, N'Số điện thoại đã tồn tại trong hệ thống.', 1;

    INSERT KHACH_HANG (MaKH, TenKH, GioiTinh, NgaySinh, Sdt, DiaChi, HangThanhVien, DaKhoa)
    VALUES (@MaKH, @TenKH, @GioiTinh, @NgaySinh, NULLIF(@Sdt,''), @DiaChi, @HangThanhVien, 0);
END
GO

IF OBJECT_ID('dbo.sp_CapNhatKhachHang', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_CapNhatKhachHang;
GO
CREATE PROCEDURE dbo.sp_CapNhatKhachHang
    @MaKH          VARCHAR(10),
    @TenKH         NVARCHAR(50),
    @GioiTinh      NVARCHAR(10),
    @NgaySinh      DATE = NULL,
    @Sdt           VARCHAR(15) = NULL,
    @DiaChi        NVARCHAR(200) = NULL,
    @HangThanhVien NVARCHAR(30) = N'Đồng'
AS
BEGIN
    SET NOCOUNT ON;
    SET @MaKH = UPPER(LTRIM(RTRIM(@MaKH)));
    IF NOT EXISTS (SELECT 1 FROM KHACH_HANG WHERE UPPER(MaKH) = @MaKH)
        THROW 50103, N'Khách hàng không tồn tại.', 1;
    IF LEN(LTRIM(RTRIM(@TenKH))) = 0 THROW 50100, N'Tên khách hàng không được để trống.', 1;
    IF @GioiTinh NOT IN (N'Nam', N'Nữ') THROW 50101, N'Giới tính không hợp lệ.', 1;
    IF @HangThanhVien NOT IN (N'Đồng', N'Bạc', N'Vàng', N'VIP') SET @HangThanhVien = N'Đồng';
    IF @Sdt IS NOT NULL AND @Sdt <> '' AND EXISTS (SELECT 1 FROM KHACH_HANG WHERE Sdt = @Sdt AND UPPER(MaKH) <> @MaKH)
        THROW 50102, N'Số điện thoại đã tồn tại trong hệ thống.', 1;

    UPDATE KHACH_HANG
    SET TenKH = @TenKH, GioiTinh = @GioiTinh, NgaySinh = @NgaySinh,
        Sdt = NULLIF(@Sdt,''), DiaChi = @DiaChi, HangThanhVien = @HangThanhVien
    WHERE UPPER(MaKH) = @MaKH;
END
GO

IF OBJECT_ID('dbo.sp_KhoaKhachHang', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_KhoaKhachHang;
GO
CREATE PROCEDURE dbo.sp_KhoaKhachHang
    @MaKH   VARCHAR(10),
    @DaKhoa BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    SET @MaKH = UPPER(LTRIM(RTRIM(@MaKH)));
    IF NOT EXISTS (SELECT 1 FROM KHACH_HANG WHERE UPPER(MaKH) = @MaKH)
        THROW 50103, N'Khách hàng không tồn tại.', 1;
    UPDATE KHACH_HANG SET DaKhoa = @DaKhoa WHERE UPPER(MaKH) = @MaKH;
END
GO

IF OBJECT_ID('dbo.sp_XoaKhachHang', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_XoaKhachHang;
GO
CREATE PROCEDURE dbo.sp_XoaKhachHang @MaKH VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    SET @MaKH = UPPER(LTRIM(RTRIM(@MaKH)));
    IF NOT EXISTS (SELECT 1 FROM KHACH_HANG WHERE UPPER(MaKH) = @MaKH)
        THROW 50103, N'Khách hàng không tồn tại.', 1;
    IF EXISTS (SELECT 1 FROM HOA_DON WHERE UPPER(MaKH) = @MaKH)
        THROW 50104, N'Không thể xóa khách hàng đã có hóa đơn. Hãy dùng chức năng Khóa thay thế.', 1;
    DELETE KHACH_HANG WHERE UPPER(MaKH) = @MaKH;
END
GO

IF OBJECT_ID('dbo.sp_TimKiemKhachHang', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_TimKiemKhachHang;
GO
CREATE PROCEDURE dbo.sp_TimKiemKhachHang @TuKhoa NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET @TuKhoa = LTRIM(RTRIM(@TuKhoa));
    SELECT MaKH, TenKH, GioiTinh, HangThanhVien,
           CONVERT(NVARCHAR(10), NgaySinh, 103) AS NgaySinh,
           Sdt, DiaChi,
           CASE DaKhoa WHEN 1 THEN N'Đã khóa' ELSE N'Hoạt động' END AS TrangThai
    FROM KHACH_HANG
    WHERE @TuKhoa IS NULL OR @TuKhoa = ''
       OR TenKH LIKE N'%' + @TuKhoa + N'%'
       OR MaKH  LIKE N'%' + @TuKhoa + N'%'
       OR GioiTinh LIKE N'%' + @TuKhoa + N'%'
       OR HangThanhVien LIKE N'%' + @TuKhoa + N'%'
       OR Sdt   LIKE N'%' + @TuKhoa + N'%'
       OR DiaChi LIKE N'%' + @TuKhoa + N'%'
    ORDER BY 
       CASE WHEN PATINDEX('%[0-9]%', MaKH) > 0 
            THEN CAST(SUBSTRING(MaKH, PATINDEX('%[0-9]%', MaKH), 10) AS INT) 
            ELSE 999999 END,
       MaKH;
END
GO

/* 5. SP CRUD NHAN_VIEN */

IF OBJECT_ID('dbo.sp_ThemNhanVien', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_ThemNhanVien;
GO

IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'seq_MaNV')
BEGIN
    DECLARE @maxNV INT;
    SELECT @maxNV = ISNULL(MAX(CAST(SUBSTRING(MaNV, 3, LEN(MaNV)) AS INT)), 0) FROM NHAN_VIEN;
    DECLARE @sqlNV NVARCHAR(500) = N'CREATE SEQUENCE dbo.seq_MaNV AS INT START WITH ' + CAST(@maxNV + 1 AS NVARCHAR(10)) + N' INCREMENT BY 1;';
    EXEC sp_executesql @sqlNV;
END
GO

CREATE PROCEDURE dbo.sp_ThemNhanVien
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

    DECLARE @so INT = NEXT VALUE FOR dbo.seq_MaNV;
    SET @MaNV = dbo.fn_Ma('NV', @so, 3);

    INSERT NHAN_VIEN (MaNV, TenNV, GioiTinh, NgaySinh, SDT, DiaChi, ChucVu, TenDN, MatKhau)
    VALUES (@MaNV, @TenNV, @GioiTinh, @NgaySinh, NULLIF(@SDT,''), @DiaChi, @ChucVu, @TenDN, '');
    EXEC dbo.sp_DatMatKhau @MaNV, @MatKhau;
END
GO

IF OBJECT_ID('dbo.sp_CapNhatNhanVien', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_CapNhatNhanVien;
GO
CREATE PROCEDURE dbo.sp_CapNhatNhanVien
    @MaNV     VARCHAR(10),
    @TenNV    NVARCHAR(50),
    @GioiTinh NVARCHAR(10),
    @NgaySinh DATE = NULL,
    @SDT      VARCHAR(15) = NULL,
    @DiaChi   NVARCHAR(200) = NULL,
    @ChucVu   NVARCHAR(50),
    @TenDN    VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = @MaNV)
        THROW 50205, N'Nhân viên không tồn tại.', 1;
    IF LEN(LTRIM(RTRIM(@TenNV))) = 0 THROW 50200, N'Tên nhân viên không được để trống.', 1;
    IF @GioiTinh NOT IN (N'Nam', N'Nữ') THROW 50201, N'Giới tính không hợp lệ.', 1;
    IF @ChucVu NOT IN (N'Quản trị viên', N'Nhân viên') THROW 50202, N'Chức vụ không hợp lệ.', 1;
    IF EXISTS (SELECT 1 FROM NHAN_VIEN WHERE TenDN = @TenDN AND MaNV <> @MaNV)
        THROW 50204, N'Tên đăng nhập đã tồn tại.', 1;

    UPDATE NHAN_VIEN
    SET TenNV = @TenNV, GioiTinh = @GioiTinh, NgaySinh = @NgaySinh,
        SDT = NULLIF(@SDT,''), DiaChi = @DiaChi, ChucVu = @ChucVu, TenDN = @TenDN
    WHERE MaNV = @MaNV;
END
GO

IF OBJECT_ID('dbo.sp_KhoaNhanVien', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_KhoaNhanVien;
GO
CREATE PROCEDURE dbo.sp_KhoaNhanVien @MaNV VARCHAR(10), @DaNghi BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = @MaNV)
        THROW 50205, N'Nhân viên không tồn tại.', 1;
    UPDATE NHAN_VIEN SET DaNghi = @DaNghi WHERE MaNV = @MaNV;
END
GO

IF OBJECT_ID('dbo.sp_XoaNhanVien', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_XoaNhanVien;
GO
CREATE PROCEDURE dbo.sp_XoaNhanVien @MaNV VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = @MaNV)
        THROW 50205, N'Nhân viên không tồn tại.', 1;
    IF EXISTS (SELECT 1 FROM HOA_DON WHERE MaNV = @MaNV)
        THROW 50206, N'Không thể xóa nhân viên đã có hóa đơn. Hãy dùng chức năng Khóa thay thế.', 1;
    IF EXISTS (SELECT 1 FROM PHIEU_NHAP WHERE MaNV = @MaNV)
        THROW 50207, N'Không thể xóa nhân viên đã có phiếu nhập. Hãy dùng chức năng Khóa thay thế.', 1;
    DELETE NHAN_VIEN WHERE MaNV = @MaNV;
END
GO

IF OBJECT_ID('dbo.sp_TimKiemNhanVien', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_TimKiemNhanVien;
GO
CREATE PROCEDURE dbo.sp_TimKiemNhanVien @TuKhoa NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaNV, TenNV, GioiTinh,
           CONVERT(NVARCHAR(10), NgaySinh, 103) AS NgaySinh,
           SDT, DiaChi, ChucVu, TenDN,
           CASE DaNghi WHEN 1 THEN N'Đã nghỉ' ELSE N'Đang làm' END AS TrangThai
    FROM NHAN_VIEN
    WHERE @TuKhoa IS NULL OR @TuKhoa = ''
       OR TenNV LIKE N'%' + @TuKhoa + N'%'
       OR MaNV  LIKE N'%' + @TuKhoa + N'%'
       OR SDT   LIKE N'%' + @TuKhoa + N'%'
    ORDER BY MaNV;
END
GO

PRINT N'Script 04 hoan tat: da them GioiTinh, NgaySinh, DiaChi va cac stored procedure CRUD.';

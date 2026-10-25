USE QuanLyBanLaptop_V2;
GO
SET NOCOUNT ON;

/* 1. Tạo view tblNhanVien, tblKhachHang, tblHang nếu chưa có */
IF OBJECT_ID('dbo.tblNhanVien', 'V') IS NOT NULL DROP VIEW dbo.tblNhanVien;
GO
CREATE VIEW dbo.tblNhanVien AS
SELECT MaNV, TenNV, SDT, ChucVu, TenDN, MatKhau AS Passw, MatKhau, DaNghi, GioiTinh, NgaySinh, DiaChi
FROM NHAN_VIEN;
GO

IF OBJECT_ID('dbo.tblKhachHang', 'V') IS NOT NULL DROP VIEW dbo.tblKhachHang;
GO
CREATE VIEW dbo.tblKhachHang AS
SELECT MaKH, TenKH, DiaChi, Sdt, HangThanhVien, GioiTinh, NgaySinh, DaKhoa
FROM KHACH_HANG;
GO

IF OBJECT_ID('dbo.tblHang', 'V') IS NOT NULL DROP VIEW dbo.tblHang;
GO
CREATE VIEW dbo.tblHang AS
SELECT MaLaptop AS MaHang, TenLaptop AS TenHang, Loai, CPU, RAM, SSD, DonGia AS DonGiaBan, DonGia, SoLuongTon, ThoiGianBaoHanh, MaNCC, Anh AS HinhAnh, Anh, NgungKinhDoanh
FROM LAPTOP;
GO

/* 2. Cập nhật sp_DangNhap để hỗ trợ đăng nhập bằng MaNV và Passw */
IF OBJECT_ID('dbo.sp_DangNhap', 'P') IS NOT NULL
BEGIN
    EXEC('
    ALTER PROCEDURE dbo.sp_DangNhap @TenDN VARCHAR(50), @MatKhau NVARCHAR(100)
    AS
    BEGIN
        SET NOCOUNT ON;
        SELECT MaNV, TenNV, ChucVu
        FROM NHAN_VIEN
        WHERE (TenDN = @TenDN OR MaNV = @TenDN)
          AND DaNghi = 0
          AND (dbo.fn_KiemTraMatKhau(MatKhau, @MatKhau) = 1 OR MatKhau = @MatKhau);
    END');
END
GO

/* 3. Thêm cột GiamGia và ThanhTien vào CHI_TIET_HD nếu chưa có */
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('CHI_TIET_HD') AND name = 'GiamGia')
    ALTER TABLE CHI_TIET_HD ADD GiamGia DECIMAL(5,2) NOT NULL DEFAULT 0;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('CHI_TIET_HD') AND name = 'ThanhTien')
    ALTER TABLE CHI_TIET_HD ADD ThanhTien DECIMAL(18,2) NOT NULL DEFAULT 0;
GO

/* 4. Mở rộng độ dài cột MaHD sang NVARCHAR(30) */

-- Drop check constraints on LAPTOP_SERIAL referencing MaHD
DECLARE @chkSql NVARCHAR(MAX) = N'';
SELECT @chkSql += N'ALTER TABLE LAPTOP_SERIAL DROP CONSTRAINT ' + QUOTENAME(name) + N';' + CHAR(10)
FROM sys.check_constraints
WHERE parent_object_id = OBJECT_ID('LAPTOP_SERIAL')
  AND (name IN ('CK_Serial_Ban', 'CK_Serial_HD') OR definition LIKE '%MaHD%');
IF LEN(@chkSql) > 0 EXEC sp_executesql @chkSql;
GO

-- Drop all foreign keys that reference HOA_DON or CHI_TIET_HD or have MaHD
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + N'.' + QUOTENAME(OBJECT_NAME(parent_object_id))
             + N' DROP CONSTRAINT ' + QUOTENAME(name) + N';' + CHAR(10)
FROM sys.foreign_keys
WHERE referenced_object_id IN (OBJECT_ID('HOA_DON'), OBJECT_ID('CHI_TIET_HD'))
   OR parent_object_id IN (OBJECT_ID('CHI_TIET_HD'), OBJECT_ID('LICH_TRA_GOP'), OBJECT_ID('LAPTOP_SERIAL'));

IF LEN(@sql) > 0 EXEC sp_executesql @sql;
GO

-- Drop PKs
DECLARE @pkSql NVARCHAR(MAX) = N'';
SELECT @pkSql += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + N'.' + QUOTENAME(OBJECT_NAME(parent_object_id))
               + N' DROP CONSTRAINT ' + QUOTENAME(name) + N';' + CHAR(10)
FROM sys.key_constraints
WHERE type = 'PK' AND parent_object_id IN (OBJECT_ID('LICH_TRA_GOP'), OBJECT_ID('CHI_TIET_HD'), OBJECT_ID('HOA_DON'));

IF LEN(@pkSql) > 0 EXEC sp_executesql @pkSql;
GO

-- Drop index if exists
IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('LAPTOP_SERIAL') AND name = 'IX_Serial_MaHD')
    DROP INDEX IX_Serial_MaHD ON LAPTOP_SERIAL;
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('HOA_DON') AND name = 'IX_HoaDon_MaKH')
    DROP INDEX IX_HoaDon_MaKH ON HOA_DON;
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('HOA_DON') AND name = 'IX_HoaDon_MaNV')
    DROP INDEX IX_HoaDon_MaNV ON HOA_DON;
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('HOA_DON') AND name = 'IX_HoaDon_NgayLap')
    DROP INDEX IX_HoaDon_NgayLap ON HOA_DON;
GO

-- Alter columns to NVARCHAR(30)
ALTER TABLE HOA_DON ALTER COLUMN MaHD NVARCHAR(30) NOT NULL;
ALTER TABLE CHI_TIET_HD ALTER COLUMN MaHD NVARCHAR(30) NOT NULL;
ALTER TABLE LICH_TRA_GOP ALTER COLUMN MaHD NVARCHAR(30) NOT NULL;
ALTER TABLE LAPTOP_SERIAL ALTER COLUMN MaHD NVARCHAR(30) NULL;
GO

-- Recreate PKs
ALTER TABLE HOA_DON ADD CONSTRAINT PK_HOA_DON PRIMARY KEY (MaHD);
ALTER TABLE CHI_TIET_HD ADD CONSTRAINT PK_CHI_TIET_HD PRIMARY KEY (MaHD, MaLaptop);
ALTER TABLE LICH_TRA_GOP ADD CONSTRAINT PK_LICH_TRA_GOP PRIMARY KEY (MaHD, KyThu);
GO

-- Recreate FKs
ALTER TABLE HOA_DON ADD CONSTRAINT FK_HD_KhachHang FOREIGN KEY (MaKH) REFERENCES KHACH_HANG(MaKH);
ALTER TABLE HOA_DON ADD CONSTRAINT FK_HD_NhanVien FOREIGN KEY (MaNV) REFERENCES NHAN_VIEN(MaNV);
ALTER TABLE CHI_TIET_HD ADD CONSTRAINT FK_CTHD_HoaDon FOREIGN KEY (MaHD) REFERENCES HOA_DON(MaHD) ON DELETE CASCADE;
ALTER TABLE CHI_TIET_HD ADD CONSTRAINT FK_CTHD_Laptop FOREIGN KEY (MaLaptop) REFERENCES LAPTOP(MaLaptop);
ALTER TABLE LICH_TRA_GOP ADD CONSTRAINT FK_LTG_HoaDon FOREIGN KEY (MaHD) REFERENCES HOA_DON(MaHD) ON DELETE CASCADE;
ALTER TABLE LAPTOP_SERIAL ADD CONSTRAINT FK_Serial_CTHD FOREIGN KEY (MaHD, MaLaptop) REFERENCES CHI_TIET_HD(MaHD, MaLaptop);
GO

-- Recreate CK_Serial_Ban
ALTER TABLE LAPTOP_SERIAL ADD CONSTRAINT CK_Serial_Ban CHECK (
    (TrangThai IN (N'Trong kho', N'Hỏng') AND MaHD IS NULL)
    OR (TrangThai IN (N'Đã bán', N'Đang bảo hành') AND MaHD IS NOT NULL)
);
GO

-- Recreate indexes
CREATE INDEX IX_Serial_MaHD ON LAPTOP_SERIAL(MaHD);
CREATE INDEX IX_HoaDon_MaKH ON HOA_DON(MaKH);
CREATE INDEX IX_HoaDon_MaNV ON HOA_DON(MaNV);
CREATE INDEX IX_HoaDon_NgayLap ON HOA_DON(NgayLap);
GO

-- Update trigger trg_CapNhatTongTienHD
IF OBJECT_ID('dbo.trg_CapNhatTongTienHD', 'TR') IS NOT NULL DROP TRIGGER dbo.trg_CapNhatTongTienHD;
GO
CREATE TRIGGER trg_CapNhatTongTienHD ON CHI_TIET_HD
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE hd
    SET TongTien = ISNULL((
        SELECT SUM(CASE WHEN ct.ThanhTien > 0 THEN ct.ThanhTien 
                        ELSE ct.SoLuong * ct.DonGia * (1.0 - ISNULL(ct.GiamGia, 0) / 100.0) 
                   END)
        FROM CHI_TIET_HD ct
        WHERE ct.MaHD = hd.MaHD
    ), 0)
    FROM HOA_DON hd
    WHERE hd.MaHD IN (SELECT MaHD FROM inserted UNION SELECT MaHD FROM deleted);
END
GO

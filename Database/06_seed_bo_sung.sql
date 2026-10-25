USE QuanLyBanLaptop_V2;
GO
SET NOCOUNT ON;

-- 1. Cập nhật thông tin nhân viên hiện có
UPDATE NHAN_VIEN SET GioiTinh = N'Nam', NgaySinh = '1990-05-15', DiaChi = N'Hoàn Kiếm, Hà Nội', DaNghi = 0 WHERE MaNV = 'NV001';
UPDATE NHAN_VIEN SET GioiTinh = N'Nữ',  NgaySinh = '1995-08-20', DiaChi = N'Đống Đa, Hà Nội',  DaNghi = 0 WHERE MaNV = 'NV002';
UPDATE NHAN_VIEN SET GioiTinh = N'Nam', NgaySinh = '1992-11-10', DiaChi = N'Cầu Giấy, Hà Nội', DaNghi = 0 WHERE MaNV = 'NV003';
GO

-- 2. Thêm nhân viên mới nếu chưa có
IF NOT EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = 'NV004')
BEGIN
    INSERT INTO NHAN_VIEN (MaNV, TenNV, GioiTinh, NgaySinh, SDT, DiaChi, ChucVu, TenDN, MatKhau, DaNghi)
    VALUES ('NV004', N'Lê Thị Hương', N'Nữ', '1997-04-18', '0934567890', N'Thanh Xuân, Hà Nội', N'Nhân viên', 'huong_sale', '8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC1B6B', 0);
END
GO

IF NOT EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = 'NV005')
BEGIN
    INSERT INTO NHAN_VIEN (MaNV, TenNV, GioiTinh, NgaySinh, SDT, DiaChi, ChucVu, TenDN, MatKhau, DaNghi)
    VALUES ('NV005', N'Phạm Tiến Đạt', N'Nam', '1996-09-22', '0978123456', N'Hai Bà Trưng, Hà Nội', N'Nhân viên', 'dat_kythuat', '8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC1B6B', 0);
END
GO

-- 3. Cập nhật khách hàng hiện có
UPDATE KHACH_HANG SET GioiTinh = N'Nam', NgaySinh = '1995-03-15', DaKhoa = 0 WHERE MaKH = 'KH001';
UPDATE KHACH_HANG SET GioiTinh = N'Nữ',  NgaySinh = '1998-07-22', DaKhoa = 0 WHERE MaKH = 'KH002';
UPDATE KHACH_HANG SET GioiTinh = N'Nam', NgaySinh = '1992-12-10', DaKhoa = 0 WHERE MaKH = 'KH003';
UPDATE KHACH_HANG SET GioiTinh = N'Nam', NgaySinh = '1989-09-05', DaKhoa = 0 WHERE MaKH = 'KH004';
UPDATE KHACH_HANG SET GioiTinh = N'Nữ',  NgaySinh = '2001-01-18', DaKhoa = 0 WHERE MaKH = 'KH005';
GO

-- 4. Thêm khách hàng mới
IF NOT EXISTS (SELECT 1 FROM KHACH_HANG WHERE MaKH = 'KH006')
    INSERT INTO KHACH_HANG (MaKH, TenKH, GioiTinh, NgaySinh, Sdt, DiaChi, HangThanhVien, DaKhoa)
    VALUES ('KH006', N'Hoàng Tuấn Minh', N'Nam', '1994-06-14', '0912987654', N'Hà Đông, Hà Nội', N'Bạc', 0);

IF NOT EXISTS (SELECT 1 FROM KHACH_HANG WHERE MaKH = 'KH007')
    INSERT INTO KHACH_HANG (MaKH, TenKH, GioiTinh, NgaySinh, Sdt, DiaChi, HangThanhVien, DaKhoa)
    VALUES ('KH007', N'Trương Mỹ Linh', N'Nữ', '1999-10-08', '0983456789', N'Ba Đình, Hà Nội', N'Đồng', 0);

IF NOT EXISTS (SELECT 1 FROM KHACH_HANG WHERE MaKH = 'KH008')
    INSERT INTO KHACH_HANG (MaKH, TenKH, GioiTinh, NgaySinh, Sdt, DiaChi, HangThanhVien, DaKhoa)
    VALUES ('KH008', N'Ngô Văn Thành', N'Nam', '1988-02-25', '0904561234', N'Quận 7, TP. HCM', N'Vàng', 0);

IF NOT EXISTS (SELECT 1 FROM KHACH_HANG WHERE MaKH = 'KH009')
    INSERT INTO KHACH_HANG (MaKH, TenKH, GioiTinh, NgaySinh, Sdt, DiaChi, HangThanhVien, DaKhoa)
    VALUES ('KH009', N'Đặng Phương Mai', N'Nữ', '2000-11-12', '0977654321', N'Sơn Trà, Đà Nẵng', N'VIP', 0);

IF NOT EXISTS (SELECT 1 FROM KHACH_HANG WHERE MaKH = 'KH010')
    INSERT INTO KHACH_HANG (MaKH, TenKH, GioiTinh, NgaySinh, Sdt, DiaChi, HangThanhVien, DaKhoa)
    VALUES ('KH010', N'Vũ Đình Khang', N'Nam', '1996-08-30', '0966554433', N'Ngô Quyền, Hải Phòng', N'Đồng', 0);
GO

-- 5. Cập nhật chi tiết hóa đơn (GiamGia, ThanhTien)
UPDATE CHI_TIET_HD
SET GiamGia = ISNULL(GiamGia, 0),
    ThanhTien = CASE WHEN ThanhTien > 0 THEN ThanhTien ELSE SoLuong * DonGia * (1.0 - ISNULL(GiamGia, 0) / 100.0) END;
GO

-- 6. Cập nhật tổng tiền hóa đơn
UPDATE hd
SET TongTien = ISNULL((
    SELECT SUM(CASE WHEN ct.ThanhTien > 0 THEN ct.ThanhTien ELSE ct.SoLuong * ct.DonGia END)
    FROM CHI_TIET_HD ct
    WHERE ct.MaHD = hd.MaHD
), 0)
FROM HOA_DON hd;
GO

PRINT N'Script 06_seed_bo_sung.sql hoan tat!';

SET XACT_ABORT ON;
IF EXISTS(SELECT 1 FROM dbo.NHAN_VIEN) THROW 52001,N'Database đã có dữ liệu. Không nạp demo.',1;
BEGIN TRANSACTION;
GO
INSERT NHA_CUNG_CAP (MaNCC, TenNCC, DiaChi, Sdt) VALUES
('NCC01', N'Công ty TNHH Máy Tính Asus Việt Nam',  N'Quận 1, TP. Hồ Chí Minh', '0281234567'),
('NCC02', N'Đại lý Phân phối Acer Toàn Cầu',        N'Cầu Giấy, Hà Nội',        '0247654321'),
('NCC03', N'Nhà cung cấp Dell Việt Nam',            N'Quận 3, TP. Hồ Chí Minh', '0289998888'),
('NCC04', N'Công ty Thương Mại HP Việt Nam',        N'Thanh Xuân, Hà Nội',      '0243332211'),
('NCC05', N'Nhà Phân Phối Lenovo Việt Nam',         N'Đống Đa, Hà Nội',         '0245556666');
INSERT LOAI_MAY (TenLoai, MoTa) VALUES
(N'Gaming',    N'Laptop chuyên chơi game và đồ họa nặng'),
(N'Văn phòng', N'Laptop học tập và làm việc văn phòng nhẹ nhàng'),
(N'Mỏng nhẹ',  N'Laptop cao cấp mỏng nhẹ, pin lâu, tiện di chuyển'),
(N'Đồ họa',    N'Laptop chuyên render đồ họa, dựng video'),
(N'Doanh nhân', N'Laptop mỏng nhẹ sang trọng cho doanh nhân');
INSERT LAPTOP (MaLaptop, TenLaptop, Loai, CPU, RAM, SSD, DonGiaNhap, DonGia, SoLuongTon, ThoiGianBaoHanh, MaNCC, Anh, GhiChu) VALUES
('LT001', N'Acer Nitro 5 AN515',      N'Gaming',    N'Core i5 12500H', '8GB',  '512GB', 14800000, 18500000, 0, 12, 'NCC02', N'LT001.jpg', N'Laptop gaming quốc dân'),
('LT002', N'Asus ROG Strix G15',      N'Gaming',    N'Ryzen 7 5800H',  '16GB', '512GB', 19600000, 24500000, 0, 24, 'NCC01', N'LT002.jpg', N'Hiệu năng cao, tản nhiệt tốt'),
('LT003', N'Dell Inspiron 15 3520',   N'Văn phòng', N'Core i5 1235U',  '8GB',  '512GB', 11360000, 14200000, 0, 12, 'NCC03', N'LT003.jpg', N'Văn phòng bền bỉ'),
('LT004', N'HP Pavilion 15',          N'Văn phòng', N'Core i5 1335U',  '16GB', '512GB', 13440000, 16800000, 0, 12, 'NCC04', N'LT004.jpg', N'Thiết kế sang trọng'),
('LT005', N'Lenovo IdeaPad Slim 3',   N'Văn phòng', N'Ryzen 5 7520U',  '8GB',  '256GB', 8400000,  10500000, 0, 24, 'NCC05', N'LT005.jpg', N'Mỏng nhẹ học tập'),
('LT006', N'Asus ZenBook 14 OLED',    N'Mỏng nhẹ',  N'Core i5 1340P',  '16GB', '512GB', 17520000, 21900000, 0, 24, 'NCC01', N'LT006.jpg', N'Màn hình OLED sắc nét'),
('LT007', N'Dell Vostro 3430',        N'Văn phòng', N'Core i3 1305U',  '8GB',  '256GB', 8960000,  11200000, 0, 12, 'NCC03', N'LT007.jpg', N'Phù hợp kế toán'),
('LT008', N'Acer Swift Go 14',        N'Mỏng nhẹ',  N'Core i5 13500H', '16GB', '512GB', 14320000, 17900000, 0, 12, 'NCC02', N'LT008.jpg', N'Mỏng nhẹ vỏ nhôm'),
('LT009', N'HP Victus 16',            N'Gaming',    N'Ryzen 5 7640HS', '16GB', '512GB', 15840000, 19800000, 0, 12, 'NCC04', N'LT009.jpg', N'Gaming màn lớn 16 inch'),
('LT010', N'Lenovo Legion 5',         N'Gaming',    N'Core i7 13700H', '16GB', '1TB',   26000000, 32500000, 0, 36, 'NCC05', N'LT010.jpg', N'Đỉnh cao đồ họa & game');
INSERT NHAN_VIEN (MaNV, TenNV, SDT, ChucVu, TenDN, MatKhau) VALUES
('NV001', N'Nguyễn Văn An',   '0912345678', N'Quản trị viên', 'admin',     'admin123'),
('NV002', N'Trần Thị Bình',   '0987654321', N'Nhân viên',     'binh_sale', '123456'),
('NV003', N'Lê Hoàng Cường',  '0905556666', N'Nhân viên',     'cuong_kho', '123456');
EXEC dbo.sp_DatMatKhau 'NV001', N'admin123';
EXEC dbo.sp_DatMatKhau 'NV002', N'123456';
EXEC dbo.sp_DatMatKhau 'NV003', N'123456';
INSERT KHACH_HANG (MaKH, TenKH, DiaChi, Sdt) VALUES
('KH001', N'Phạm Minh Đăng',    N'Cầu Giấy, Hà Nội',          '0901112222'),
('KH002', N'Đỗ Thùy Chi',       N'Ba Đình, Hà Nội',           '0982223333'),
('KH003', N'Nguyễn Hoàng Long', N'Quận Hải Châu, Đà Nẵng',    '0903334444'),
('KH004', N'Vũ Quốc Anh',       N'Quận 1, TP. Hồ Chí Minh',   '0914445555'),
('KH005', N'Bùi Phương Thảo',   N'Lê Chân, Hải Phòng',        '0985556666');
GO
DECLARE @pn VARCHAR(10);
DECLARE @nhap dbo.TVP_NhapHang;
INSERT @nhap VALUES ('LT002', 5, 21000000), ('LT006', 7, 15500000);
EXEC dbo.sp_NhapHang 'NCC01', 'NV003', @nhap, '2026-06-20 08:00', @pn OUTPUT;
DELETE @nhap;
INSERT @nhap VALUES ('LT001', 10, 15000000), ('LT008', 6, 15600000);
EXEC dbo.sp_NhapHang 'NCC02', 'NV003', @nhap, '2026-06-25 15:00', @pn OUTPUT;
DELETE @nhap;
INSERT @nhap VALUES ('LT003', 15, 12200000), ('LT007', 8, 10500000);
EXEC dbo.sp_NhapHang 'NCC03', 'NV003', @nhap, '2026-07-02 10:00', @pn OUTPUT;
DELETE @nhap;
INSERT @nhap VALUES ('LT004', 12, 14500000), ('LT009', 4, 17000000);
EXEC dbo.sp_NhapHang 'NCC04', 'NV003', @nhap, '2026-07-05 09:30', @pn OUTPUT;
DELETE @nhap;
INSERT @nhap VALUES ('LT005', 20, 8800000), ('LT010', 3, 28000000);
EXEC dbo.sp_NhapHang 'NCC05', 'NV003', @nhap, '2026-07-08 14:00', @pn OUTPUT;
GO
DECLARE @hd VARCHAR(10);
DECLARE @ban dbo.TVP_BanHang;
INSERT @ban VALUES ('LT001', 1), ('LT003', 1);
EXEC dbo.sp_BanHang 'KH001', 'NV002', N'Chuyển khoản', 0, @ban, '2026-10-01 10:30', @hd OUTPUT;
DELETE @ban; INSERT @ban VALUES ('LT003', 1);
EXEC dbo.sp_BanHang 'KH002', 'NV002', N'Tiền mặt', 0, @ban, '2026-10-05 14:15', @hd OUTPUT;
DELETE @ban; INSERT @ban VALUES ('LT002', 2);
EXEC dbo.sp_BanHang 'KH003', 'NV002', N'Trả góp', 6, @ban, '2026-10-10 09:00', @hd OUTPUT;
DELETE @ban; INSERT @ban VALUES ('LT004', 1);
EXEC dbo.sp_BanHang 'KH004', 'NV002', N'Trả góp', 3, @ban, '2026-07-14 16:45', @hd OUTPUT;
GO
DECLARE @bh VARCHAR(10);
EXEC dbo.sp_TiepNhanBaoHanh 'LT001-PN002-001', 'NV002', N'Bảo hành',
     N'Lỗi quạt tản nhiệt kêu to, vệ sinh và tra keo hãng', 0, '2026-10-18 09:30', @bh OUTPUT;
EXEC dbo.sp_CapNhatBaoHanh @bh, N'Đã trả khách', '2026-10-20 15:00';
EXEC dbo.sp_TiepNhanBaoHanh 'LT002-PN001-001', 'NV002', N'Sửa chữa',
     N'Khách làm rơi vỡ bản lề góc phải (từ chối BH, sửa dịch vụ)', 850000, '2026-10-21 14:00', @bh OUTPUT;
GO
PRINT N'Seed hoàn tất. Chạy 04_verify.sql để kiểm tra, kết quả đúng là không có dòng nào.';
COMMIT;
GO
ALTER SEQUENCE dbo.seq_AppEmployee RESTART WITH 4;
ALTER SEQUENCE dbo.seq_AppCustomer RESTART WITH 6;
ALTER SEQUENCE dbo.seq_AppSupplier RESTART WITH 6;
ALTER SEQUENCE dbo.seq_AppLaptop RESTART WITH 11;
GO

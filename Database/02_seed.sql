/* =====================================================================
   QuanLyBanLaptop - 02_seed.sql  (chạy sau 01_schema.sql)
   Danh mục nhập trực tiếp; giao dịch (nhập, bán, trả góp, bảo hành) gọi qua
   stored procedure nên tồn kho, serial, tổng tiền, hạng thành viên luôn nhất quán.

   Tài khoản mẫu: admin / admin123 ; binh_sale / 123456 ; cuong_kho / 123456
   ===================================================================== */
USE QuanLyBanLaptop;
GO
SET NOCOUNT ON;

/* ---------- Cấu hình ---------- */
INSERT CAU_HINH (Khoa, GiaTri, MoTa) VALUES
('NGUONG_BAC',  N'10000000', N'Tổng chi tiêu tối thiểu để lên hạng Bạc'),
('NGUONG_VANG', N'30000000', N'Tổng chi tiêu tối thiểu để lên hạng Vàng'),
('NGUONG_VIP',  N'50000000', N'Tổng chi tiêu tối thiểu để lên hạng VIP');

/* ---------- Danh mục ---------- */
INSERT NHA_CUNG_CAP (MaNCC, TenNCC, DiaChi, Sdt) VALUES
('NCC01', N'Công ty TNHH Máy Tính Asus Việt Nam',  N'Quận 1, TP. Hồ Chí Minh', '0281234567'),
('NCC02', N'Đại lý Phân phối Acer Toàn Cầu',        N'Cầu Giấy, Hà Nội',        '0247654321'),
('NCC03', N'Nhà cung cấp Dell Việt Nam',            N'Quận 3, TP. Hồ Chí Minh', '0289998888'),
('NCC04', N'Công ty Thương Mại HP Việt Nam',        N'Thanh Xuân, Hà Nội',      '0243332211'),
('NCC05', N'Nhà Phân Phối Lenovo Việt Nam',         N'Đống Đa, Hà Nội',         '0245556666');

-- SoLuongTon = 0: tồn kho được cộng bởi sp_NhapHang bên dưới
INSERT LAPTOP (MaLaptop, TenLaptop, Loai, CPU, RAM, SSD, DonGia, SoLuongTon, ThoiGianBaoHanh, MaNCC, HinhAnh) VALUES
('LT001', N'Acer Nitro 5 AN515',      N'Gaming',    N'Core i5 12500H', '8GB',  '512GB', 18500000, 0, 12, 'NCC02', N'acer_nitro_5.jpg'),
('LT002', N'Asus ROG Strix G15',      N'Gaming',    N'Ryzen 7 5800H',  '16GB', '512GB', 24500000, 0, 24, 'NCC01', N'asus_rog_g15.jpg'),
('LT003', N'Dell Inspiron 15 3520',   N'Văn phòng', N'Core i5 1235U',  '8GB',  '512GB', 14200000, 0, 12, 'NCC03', N'dell_inspiron_3520.jpg'),
('LT004', N'HP Pavilion 15',          N'Văn phòng', N'Core i5 1335U',  '16GB', '512GB', 16800000, 0, 12, 'NCC04', N'hp_pavilion_15.jpg'),
('LT005', N'Lenovo IdeaPad Slim 3',   N'Văn phòng', N'Ryzen 5 7520U',  '8GB',  '256GB', 10500000, 0, 24, 'NCC05', N'lenovo_ideapad_slim3.jpg'),
('LT006', N'Asus ZenBook 14 OLED',    N'Mỏng nhẹ',  N'Core i5 1340P',  '16GB', '512GB', 21900000, 0, 24, 'NCC01', N'asus_zenbook_14.jpg'),
('LT007', N'Dell Vostro 3430',        N'Văn phòng', N'Core i3 1305U',  '8GB',  '256GB', 11200000, 0, 12, 'NCC03', N'dell_vostro_3430.jpg'),
('LT008', N'Acer Swift Go 14',        N'Mỏng nhẹ',  N'Core i5 13500H', '16GB', '512GB', 17900000, 0, 12, 'NCC02', N'acer_swift_go.jpg'),
('LT009', N'HP Victus 16',            N'Gaming',    N'Ryzen 5 7640HS', '16GB', '512GB', 19800000, 0, 12, 'NCC04', N'hp_victus_16.jpg'),
('LT010', N'Lenovo Legion 5',         N'Gaming',    N'Core i7 13700H', '16GB', '1TB',   32500000, 0, 36, 'NCC05', N'lenovo_legion_5.jpg');

INSERT NHAN_VIEN (MaNV, TenNV, SDT, ChucVu, TenDN, MatKhau) VALUES
('NV001', N'Nguyễn Văn An',   '0912345678', N'Quản trị viên', 'admin',     ''),
('NV002', N'Trần Thị Bình',   '0987654321', N'Nhân viên',     'binh_sale', ''),
('NV003', N'Lê Hoàng Cường',  '0905556666', N'Nhân viên',     'cuong_kho', '');
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

/* ---------- Nhập hàng (mỗi phiếu một nhà cung cấp) ---------- */
DECLARE @pn VARCHAR(10);
DECLARE @nhap dbo.TVP_NhapHang;

INSERT @nhap VALUES ('LT002', 5, 21000000), ('LT006', 7, 15500000);
EXEC dbo.sp_NhapHang 'NCC01', 'NV003', @nhap, '2026-06-20 08:00', @pn OUTPUT;   -- PN001

DELETE @nhap;
INSERT @nhap VALUES ('LT001', 10, 15000000), ('LT008', 6, 15600000);
EXEC dbo.sp_NhapHang 'NCC02', 'NV003', @nhap, '2026-06-25 15:00', @pn OUTPUT;   -- PN002

DELETE @nhap;
INSERT @nhap VALUES ('LT003', 15, 12200000), ('LT007', 8, 10500000);
EXEC dbo.sp_NhapHang 'NCC03', 'NV003', @nhap, '2026-07-02 10:00', @pn OUTPUT;   -- PN003

DELETE @nhap;
INSERT @nhap VALUES ('LT004', 12, 14500000), ('LT009', 4, 17000000);
EXEC dbo.sp_NhapHang 'NCC04', 'NV003', @nhap, '2026-07-05 09:30', @pn OUTPUT;   -- PN004

DELETE @nhap;
INSERT @nhap VALUES ('LT005', 20, 8800000), ('LT010', 3, 28000000);
EXEC dbo.sp_NhapHang 'NCC05', 'NV003', @nhap, '2026-07-08 14:00', @pn OUTPUT;   -- PN005
GO

/* ---------- Bán hàng ---------- */
DECLARE @hd VARCHAR(10);
DECLARE @ban dbo.TVP_BanHang;

-- HD001: chuyển khoản, 2 máy
INSERT @ban VALUES ('LT001', 1), ('LT003', 1);
EXEC dbo.sp_BanHang 'KH001', 'NV002', N'Chuyển khoản', 0, @ban, '2026-10-01 10:30', @hd OUTPUT;

-- HD002: tiền mặt
DELETE @ban; INSERT @ban VALUES ('LT003', 1);
EXEC dbo.sp_BanHang 'KH002', 'NV002', N'Tiền mặt', 0, @ban, '2026-10-05 14:15', @hd OUTPUT;

-- HD003: trả góp 6 kỳ, 2 máy ROG (chưa đến kỳ nào)
DELETE @ban; INSERT @ban VALUES ('LT002', 2);
EXEC dbo.sp_BanHang 'KH003', 'NV002', N'Trả góp', 6, @ban, '2026-10-10 09:00', @hd OUTPUT;

-- HD004: trả góp 3 kỳ từ tháng 7 -> kỳ 1 (14/08) chưa đóng => vw_HoaDon báo "Trễ hạn"
DELETE @ban; INSERT @ban VALUES ('LT004', 1);
EXEC dbo.sp_BanHang 'KH004', 'NV002', N'Trả góp', 3, @ban, '2026-07-14 16:45', @hd OUTPUT;
GO

/* ---------- Bảo hành / sửa chữa ---------- */
DECLARE @bh VARCHAR(10);

-- BH001: máy LT001 của HD001, còn bảo hành -> đã trả khách
EXEC dbo.sp_TiepNhanBaoHanh 'PN002-LT001-001', 'NV002', N'Bảo hành',
     N'Lỗi quạt tản nhiệt kêu to, vệ sinh và tra keo hãng', 0, '2026-10-18 09:30', @bh OUTPUT;
EXEC dbo.sp_CapNhatBaoHanh @bh, N'Đã trả khách', '2026-10-20 15:00';

-- BH002: máy LT002 của HD003, khách làm rơi vỡ -> sửa chữa tính phí, đang xử lý
EXEC dbo.sp_TiepNhanBaoHanh 'PN001-LT002-001', 'NV002', N'Sửa chữa',
     N'Khách làm rơi vỡ bản lề góc phải (từ chối BH, sửa dịch vụ)', 850000, '2026-10-21 14:00', @bh OUTPUT;
GO

PRINT N'Seed hoàn tất. Chạy 03_doi_soat.sql để kiểm tra, kết quả đúng là không có dòng nào.';

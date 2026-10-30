SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET XACT_ABORT ON;
GO
CREATE OR ALTER PROCEDURE dbo.sp_CapNhatBaoHanh
    @MaPhieuBH VARCHAR(10),
    @TinhTrang NVARCHAR(50),
    @NgayTra   DATETIME = NULL,
    @ChiPhi    DECIMAL(18,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        IF @TinhTrang NOT IN (N'Đang xử lý', N'Đã hoàn thành', N'Đã trả khách') THROW 50060, N'Tình trạng không hợp lệ.', 1;
        BEGIN TRAN;
        DECLARE @serial VARCHAR(40), @loai NVARCHAR(30);
        SELECT @serial = SerialNo, @loai = LoaiDichVu FROM BAO_HANH_SUA_CHUA WITH (UPDLOCK,HOLDLOCK) WHERE MaPhieuBH = @MaPhieuBH;
        IF NOT EXISTS(SELECT 1 FROM BAO_HANH_SUA_CHUA WHERE MaPhieuBH=@MaPhieuBH) THROW 50061, N'Phiếu không tồn tại.', 1;
        UPDATE BAO_HANH_SUA_CHUA
        SET TinhTrangBH = @TinhTrang,
            ChiPhi  = CASE WHEN @loai = N'Sửa chữa' AND @ChiPhi IS NOT NULL THEN @ChiPhi ELSE ChiPhi END,
            NgayTra = CASE WHEN @TinhTrang = N'Đã trả khách' THEN ISNULL(@NgayTra, GETDATE()) ELSE NgayTra END
        WHERE MaPhieuBH = @MaPhieuBH;
        IF @TinhTrang = N'Đã trả khách'
            UPDATE LAPTOP_SERIAL SET TrangThai = N'Đã bán' WHERE SerialNo = @serial;
        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_ThemKhachHang
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
CREATE OR ALTER PROCEDURE dbo.sp_XoaKhachHang @MaKH VARCHAR(10)
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
CREATE OR ALTER PROCEDURE dbo.sp_TimKiemKhachHang @TuKhoa NVARCHAR(100) = NULL
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
CREATE OR ALTER PROCEDURE dbo.sp_KhoaKhachHang
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
CREATE OR ALTER PROCEDURE dbo.sp_DoiSerial @SerialCu VARCHAR(40), @SerialMoi VARCHAR(40)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM LAPTOP_SERIAL WHERE SerialNo = @SerialMoi) THROW 50024, N'Serial mới đã tồn tại.', 1;
    UPDATE LAPTOP_SERIAL SET SerialNo = @SerialMoi WHERE SerialNo = @SerialCu AND TrangThai = N'Trong kho';
    IF @@ROWCOUNT = 0 THROW 50025, N'Serial không tồn tại hoặc máy không còn trong kho.', 1;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_CapNhatKhachHang
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
CREATE OR ALTER PROCEDURE dbo.sp_NgungKinhDoanh @MaLaptop VARCHAR(10), @Ngung BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE LAPTOP SET NgungKinhDoanh = @Ngung WHERE MaLaptop = @MaLaptop;
    IF @@ROWCOUNT = 0 THROW 50004, N'Laptop không tồn tại.', 1;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_CapNhatHangThanhVien @MaKH varchar(10)=NULL
        AS BEGIN SET NOCOUNT ON; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_XoaNhanVien @MaNV VARCHAR(10)
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
CREATE OR ALTER PROCEDURE dbo.sp_TiepNhanBaoHanh
    @SerialNo   VARCHAR(40),
    @MaNV       VARCHAR(10),
    @LoaiDichVu NVARCHAR(30),
    @MoTaLoi    NVARCHAR(255),
    @ChiPhi     DECIMAL(18,2) = 0,
    @NgayNhan   DATETIME = NULL,
    @MaPhieuBH  VARCHAR(10) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @Ngay DATETIME = ISNULL(@NgayNhan, GETDATE());
    BEGIN TRY
        IF @LoaiDichVu NOT IN (N'Bảo hành', N'Sửa chữa') THROW 50050, N'Loại dịch vụ không hợp lệ.', 1;
        IF NOT EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = @MaNV AND DaNghi = 0) THROW 50051, N'Nhân viên không hợp lệ.', 1;
        BEGIN TRAN;
        IF NOT EXISTS (SELECT 1 FROM LAPTOP_SERIAL WITH (UPDLOCK,HOLDLOCK) WHERE SerialNo = @SerialNo AND TrangThai = N'Đã bán')
            THROW 50052, N'Serial không tồn tại, chưa bán, hoặc máy đang được bảo hành/sửa.', 1;
        IF @LoaiDichVu = N'Bảo hành' AND dbo.fn_ConBaoHanh(@SerialNo, @Ngay) = 0
            THROW 50053, N'Máy đã hết hạn bảo hành, hãy chọn dịch vụ Sửa chữa.', 1;
        DECLARE @so INT = NEXT VALUE FOR dbo.seq_MaBH;
        SET @MaPhieuBH = dbo.fn_Ma('BH', @so, 3);
        INSERT BAO_HANH_SUA_CHUA (MaPhieuBH, SerialNo, MaNV, LoaiDichVu, NgayNhan, MoTaLoi, ChiPhi)
        VALUES (@MaPhieuBH, @SerialNo, @MaNV, @LoaiDichVu, @Ngay, @MoTaLoi,
                CASE WHEN @LoaiDichVu = N'Bảo hành' THEN 0 ELSE @ChiPhi END);
        UPDATE LAPTOP_SERIAL SET TrangThai = N'Đang bảo hành' WHERE SerialNo = @SerialNo;
        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_NhapHang
    @MaNCC    VARCHAR(10),
    @MaNV     VARCHAR(10),
    @Items    dbo.TVP_NhapHang READONLY,
    @NgayNhap DATETIME = NULL,
    @MaPN     VARCHAR(10) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @Loi NVARCHAR(400) = NULL;
    DECLARE @Ngay DATETIME = ISNULL(@NgayNhap, GETDATE());
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM @Items) THROW 50020, N'Phiếu nhập phải có ít nhất một laptop.', 1;
        IF NOT EXISTS (SELECT 1 FROM NHA_CUNG_CAP WHERE MaNCC = @MaNCC) THROW 50021, N'Nhà cung cấp không tồn tại.', 1;
        IF NOT EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = @MaNV AND DaNghi = 0) THROW 50022, N'Nhân viên không hợp lệ.', 1;
        SELECT TOP 1 @Loi = N'Laptop ' + i.MaLaptop + N' không tồn tại hoặc không thuộc nhà cung cấp ' + @MaNCC + N'.'
        FROM @Items i LEFT JOIN LAPTOP l ON l.MaLaptop = i.MaLaptop
        WHERE l.MaLaptop IS NULL OR l.MaNCC <> @MaNCC;
        IF @Loi IS NOT NULL THROW 50023, @Loi, 1;
        BEGIN TRAN;
        DECLARE @so INT = NEXT VALUE FOR dbo.seq_MaPN;
        SET @MaPN = dbo.fn_Ma('PN', @so, 3);
        INSERT PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (@MaPN, @MaNCC, @MaNV, @Ngay);
        INSERT CHI_TIET_PN (MaPN, MaLaptop, SoLuong, DonGiaNhap)
        SELECT @MaPN, MaLaptop, SoLuong, DonGiaNhap FROM @Items;
        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_BanHang
    @MaKH        VARCHAR(10),
    @MaNV        VARCHAR(10),
    @PhuongThuc  NVARCHAR(50),
    @ChuKyTraGop INT = 0,
    @Items       dbo.TVP_BanHang READONLY,
    @NgayLap     DATETIME = NULL,
    @MaHD        VARCHAR(10) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @Loi NVARCHAR(400) = NULL;
    DECLARE @Ngay DATETIME = ISNULL(@NgayLap, GETDATE());
    DECLARE @tongMay INT;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM @Items) THROW 50010, N'Hóa đơn phải có ít nhất một laptop.', 1;
        IF NOT EXISTS (SELECT 1 FROM KHACH_HANG WHERE MaKH = @MaKH) THROW 50011, N'Khách hàng không tồn tại.', 1;
        IF NOT EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = @MaNV AND DaNghi = 0) THROW 50012, N'Nhân viên không hợp lệ.', 1;
        IF @PhuongThuc NOT IN (N'Tiền mặt', N'Chuyển khoản', N'Trả góp') THROW 50013, N'Phương thức thanh toán không hợp lệ.', 1;
        IF @PhuongThuc = N'Trả góp' AND @ChuKyTraGop NOT IN (3, 6, 9, 12) THROW 50014, N'Trả góp phải chọn chu kỳ 3, 6, 9 hoặc 12 tháng.', 1;
        IF @PhuongThuc <> N'Trả góp' AND @ChuKyTraGop <> 0 THROW 50015, N'Chỉ hóa đơn trả góp mới có chu kỳ trả góp.', 1;
        BEGIN TRAN;
        SELECT TOP 1 @Loi = N'Laptop ' + i.MaLaptop + N' không tồn tại hoặc đã ngừng kinh doanh.'
        FROM @Items i LEFT JOIN LAPTOP l WITH (UPDLOCK, HOLDLOCK) ON l.MaLaptop = i.MaLaptop
        WHERE l.MaLaptop IS NULL OR l.NgungKinhDoanh = 1;
        IF @Loi IS NOT NULL THROW 50016, @Loi, 1;
        SELECT TOP 1 @Loi = N'Laptop ' + l.MaLaptop + N' chỉ còn ' + CAST(l.SoLuongTon AS NVARCHAR(10))
                          + N' máy, không đủ ' + CAST(i.SoLuong AS NVARCHAR(10)) + N'.'
        FROM @Items i JOIN LAPTOP l ON l.MaLaptop = i.MaLaptop
        WHERE l.SoLuongTon < i.SoLuong;
        IF @Loi IS NOT NULL THROW 50017, @Loi, 1;
        DECLARE @so INT = NEXT VALUE FOR dbo.seq_MaHD;
        SET @MaHD = dbo.fn_Ma('HD', @so, 3);
        INSERT HOA_DON (MaHD, MaKH, MaNV, NgayLap, PhuongThucThanhToan, ChuKyTraGop, TinhTrang)
        VALUES (@MaHD, @MaKH, @MaNV, @Ngay, @PhuongThuc, @ChuKyTraGop,
                CASE WHEN @PhuongThuc = N'Trả góp' THEN N'Đang trả góp' ELSE N'Đã thanh toán' END);
        INSERT CHI_TIET_HD (MaHD, MaLaptop, SoLuong, DonGia, GiaVon, ThoiGianBaoHanh)
        SELECT @MaHD, i.MaLaptop, i.SoLuong, l.DonGia, dbo.fn_GiaVonTrungBinh(l.MaLaptop), l.ThoiGianBaoHanh
        FROM @Items i JOIN LAPTOP l ON l.MaLaptop = i.MaLaptop;
        IF @PhuongThuc = N'Trả góp' EXEC dbo.sp_TaoLichTraGop @MaHD;
        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_ThanhToanKy
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
        UPDATE dbo.LICH_TRA_GOP
        SET NgayTra = ISNULL(@NgayTra, CAST(GETDATE() AS DATE)),
            SoTienDaTra = SoTienPhaiTra,
            TienPhat = ISNULL(@TienPhat, 0)
        WHERE MaHD = @MaHD AND KyThu = @KyThu AND NgayTra IS NULL;
        IF @@ROWCOUNT = 0
            THROW 50040, N'Kỳ trả góp không tồn tại hoặc đã được thanh toán.', 1;
        IF NOT EXISTS (SELECT 1 FROM dbo.LICH_TRA_GOP WHERE MaHD = @MaHD AND NgayTra IS NULL)
        BEGIN
            UPDATE dbo.HOA_DON_DATA
            SET TinhTrang = N'Đã thanh toán'
            WHERE MaHD = @MaHD;
        END;
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_CapNhatTreHan
    AS
    BEGIN
        SET NOCOUNT ON;
        UPDATE h
        SET TinhTrang = v.TinhTrangMoi
        FROM dbo.HOA_DON_DATA h
        JOIN dbo.vw_TinhTrangTraGop v ON v.MaHD = h.MaHD
        WHERE h.TinhTrang <> v.TinhTrangMoi;
    END;
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
    IF LEN(@MatKhau)<6 THROW 50210,N'Mật khẩu phải có ít nhất 6 ký tự.',1;
    INSERT NHAN_VIEN (MaNV, TenNV, GioiTinh, NgaySinh, SDT, DiaChi, ChucVu, TenDN, MatKhau)
    VALUES (@MaNV, @TenNV, @GioiTinh, @NgaySinh, NULLIF(@SDT,''), @DiaChi, @ChucVu, @TenDN, @MatKhau);
END
GO
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
CREATE OR ALTER PROCEDURE dbo.sp_KhoaNhanVien @MaNV VARCHAR(10), @DaNghi BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = @MaNV)
        THROW 50205, N'Nhân viên không tồn tại.', 1;
    UPDATE NHAN_VIEN SET DaNghi = @DaNghi WHERE MaNV = @MaNV;
END
GO
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
    IF NULLIF(@MatKhau,N'') IS NOT NULL AND LEN(@MatKhau)<6 THROW 50210,N'Mật khẩu phải có ít nhất 6 ký tự.',1;
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
CREATE OR ALTER PROCEDURE dbo.sp_DatMatKhau
    @MaNV        VARCHAR(10),
    @MatKhauMoi NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF LEN(@MatKhauMoi)<6 THROW 50210,N'Mật khẩu phải có ít nhất 6 ký tự.',1;
    UPDATE NHAN_VIEN SET MatKhau=@MatKhauMoi WHERE MaNV=@MaNV;
    IF @@ROWCOUNT = 0 THROW 50002, N'Nhan vien khong ton tai.', 1;
END
GO
CREATE OR ALTER PROCEDURE dbo.sp_DoiMatKhau @MaNV VARCHAR(10), @MatKhauCu NVARCHAR(100), @MatKhauMoi NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = @MaNV AND dbo.fn_KiemTraMatKhau(MatKhau, @MatKhauCu) = 1)
        THROW 50003, N'Mật khẩu hiện tại không đúng.', 1;
    EXEC dbo.sp_DatMatKhau @MaNV, @MatKhauMoi;
END
GO
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
CREATE OR ALTER PROCEDURE dbo.sp_TaoLichTraGop
    @MaHD NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @defaultDown DECIMAL(5,4) = ISNULL((SELECT CAST(GiaTri AS DECIMAL(9,4))/100.0 FROM dbo.CAU_HINH WHERE Khoa = 'TRA_TRUOC_TOI_THIEU'), 0.30);
    DECLARE @rate3  DECIMAL(5,2) = ISNULL((SELECT CAST(GiaTri AS DECIMAL(5,2)) FROM dbo.CAU_HINH WHERE Khoa = 'LAI_SUAT_3_THANG'), 2.4);
    DECLARE @rate6  DECIMAL(5,2) = ISNULL((SELECT CAST(GiaTri AS DECIMAL(5,2)) FROM dbo.CAU_HINH WHERE Khoa = 'LAI_SUAT_6_THANG'), 4.0);
    DECLARE @rate9  DECIMAL(5,2) = ISNULL((SELECT CAST(GiaTri AS DECIMAL(5,2)) FROM dbo.CAU_HINH WHERE Khoa = 'LAI_SUAT_9_THANG'), 6.0);
    DECLARE @rate12 DECIMAL(5,2) = ISNULL((SELECT CAST(GiaTri AS DECIMAL(5,2)) FROM dbo.CAU_HINH WHERE Khoa = 'LAI_SUAT_12_THANG'), 8.0);
    IF NOT EXISTS (SELECT 1 FROM dbo.HOP_DONG_TRA_GOP WHERE MaHD = @MaHD)
    BEGIN
        INSERT INTO dbo.HOP_DONG_TRA_GOP(MaHD, TyLeTraTruoc, LaiSuat)
        SELECT MaHD, @defaultDown,
            CASE ChuKyTraGop
                WHEN 3 THEN @rate3
                WHEN 6 THEN @rate6
                WHEN 9 THEN @rate9
                ELSE @rate12
            END
        FROM dbo.HOA_DON_DATA
        WHERE MaHD = @MaHD AND PhuongThucThanhToan = N'Trả góp';
    END;
    DECLARE @tong DECIMAL(18,2), @ky INT, @ngay DATE, @rate DECIMAL(5,2), @down DECIMAL(5,4);
    SELECT @tong = h.TongTien, @ky = h.ChuKyTraGop, @ngay = CAST(h.NgayLap AS DATE),
           @rate = c.LaiSuat, @down = c.TyLeTraTruoc
    FROM dbo.HOA_DON h
    JOIN dbo.HOP_DONG_TRA_GOP c ON c.MaHD = h.MaHD
    WHERE h.MaHD = @MaHD AND h.PhuongThucThanhToan = N'Trả góp';
    IF @ky IS NULL
        THROW 51201, N'Thiếu hóa đơn hoặc điều khoản trả góp.', 1;
    IF EXISTS (SELECT 1 FROM dbo.LICH_TRA_GOP WHERE MaHD = @MaHD)
        THROW 51202, N'Hóa đơn đã có lịch trả góp.', 1;
    DECLARE @loan DECIMAL(18,2) = @tong - ROUND(@tong * @down, 0);
    DECLARE @payable DECIMAL(18,2) = @loan + ROUND(@loan * @rate / 100.0, 0);
    DECLARE @monthly DECIMAL(18,2) = FLOOR(@payable / @ky);
    IF @monthly <= 0
        THROW 51203, N'Số tiền quá nhỏ để chia kỳ.', 1;
    INSERT INTO dbo.LICH_TRA_GOP(MaHD, KyThu, NgayDenHan, SoTienPhaiTra, TienPhat)
    SELECT @MaHD, So, DATEADD(MONTH, So, @ngay),
           CASE WHEN So = @ky THEN @payable - @monthly * (@ky - 1) ELSE @monthly END, 0
    FROM dbo.fn_DaySo(@ky);
END;
GO
CREATE OR ALTER TRIGGER [dbo].[trg_BH_KiemTra]
    ON [dbo].[BAO_HANH_SUA_CHUA]
    AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN dbo.LAPTOP_SERIAL_DATA s ON s.SerialNo = i.SerialNo
        WHERE NOT EXISTS (SELECT 1 FROM deleted d WHERE d.MaPhieuBH = i.MaPhieuBH)
          AND s.TrangThai <> N'Đã bán'
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50050, N'Máy chưa bán hoặc đang không có trong hồ sơ khách hàng.', 1;
    END;
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN dbo.LAPTOP_SERIAL s ON s.SerialNo = i.SerialNo
        WHERE NOT EXISTS (SELECT 1 FROM deleted d WHERE d.MaPhieuBH = i.MaPhieuBH)
          AND i.LoaiDichVu = N'Bảo hành'
          AND (s.NgayHetBH IS NULL OR s.NgayHetBH < CAST(i.NgayNhan AS DATE))
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50051, N'Máy đã hết hạn bảo hành của hãng, vui lòng tạo phiếu Sửa chữa tính phí.', 1;
    END;
    UPDATE s
    SET TrangThai = CASE
                        WHEN i.TinhTrangBH IN (N'Tiếp nhận', N'Đang xử lý', N'Đã hoàn thành') THEN N'Đang bảo hành'
                        ELSE N'Đã bán'
                    END
    FROM dbo.LAPTOP_SERIAL_DATA s
    JOIN inserted i ON i.SerialNo = s.SerialNo
    WHERE s.TrangThai IN (N'Đã bán', N'Đang bảo hành');
END;
GO
CREATE OR ALTER TRIGGER [dbo].[trg_Lich_TinhTrang]
    ON [dbo].[LICH_TRA_GOP]
    AFTER INSERT, UPDATE, DELETE
    AS
    BEGIN
        SET NOCOUNT ON;
        UPDATE h
        SET TinhTrang = v.TinhTrangMoi
        FROM dbo.HOA_DON_DATA h
        JOIN dbo.vw_TinhTrangTraGop v ON v.MaHD = h.MaHD
        WHERE h.TinhTrang <> v.TinhTrangMoi
          AND h.MaHD IN (SELECT MaHD FROM inserted UNION SELECT MaHD FROM deleted);
    END;
GO
CREATE OR ALTER TRIGGER [dbo].[trg_CTPN_Them]
    ON [dbo].[CHI_TIET_PN]
    AFTER INSERT
    AS
    BEGIN
        SET NOCOUNT ON;
        IF EXISTS (SELECT 1
                   FROM inserted i
                   JOIN dbo.PHIEU_NHAP_DATA p ON p.MaPN = i.MaPN
                   JOIN dbo.LAPTOP_DATA l     ON l.MaLaptop = i.MaLaptop
                   WHERE l.MaNCC <> p.MaNCC)
        BEGIN
            ROLLBACK TRANSACTION;
            THROW 50030, N'Laptop không thuộc nhà cung cấp của phiếu nhập.', 1;
        END;
        IF EXISTS(SELECT 1 FROM inserted WHERE SoLuong>10000)
            THROW 50032,N'Mỗi dòng nhập tối đa 10.000 máy; hãy chia thành các phiếu nhập.',1;
        INSERT INTO dbo.LAPTOP_SERIAL_DATA(SerialNo,MaLaptop,MaPN)
        SELECT i.MaLaptop+'-'+i.MaPN+'-'+dbo.fn_Ma('',CONVERT(int,n.So),3),i.MaLaptop,i.MaPN
        FROM inserted i CROSS APPLY dbo.fn_DaySo(i.SoLuong) n;
    END;
GO
CREATE OR ALTER TRIGGER [dbo].[trg_CTPN_Xoa]
    ON [dbo].[CHI_TIET_PN]
    AFTER DELETE
    AS
    BEGIN
        SET NOCOUNT ON;
        IF EXISTS (SELECT 1
                   FROM dbo.LAPTOP_SERIAL_DATA s
                   JOIN deleted d ON d.MaPN = s.MaPN AND d.MaLaptop = s.MaLaptop
                   WHERE s.TrangThai <> N'Trong kho')
        BEGIN
            ROLLBACK TRANSACTION;
            THROW 50031, N'Không thể xóa dòng nhập: đã có máy được bán hoặc xuất bảo hành.', 1;
        END;
        DELETE s
        FROM dbo.LAPTOP_SERIAL_DATA s
        JOIN deleted d ON d.MaPN = s.MaPN AND d.MaLaptop = s.MaLaptop;
    END;
GO
CREATE OR ALTER TRIGGER [dbo].[trg_CTHD_Them]
    ON [dbo].[CHI_TIET_HD]
    AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted i JOIN dbo.LAPTOP_DATA l ON l.MaLaptop = i.MaLaptop WHERE l.NgungKinhDoanh = 1)
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50040, N'Hóa đơn có laptop đã ngừng kinh doanh.', 1;
    END;
    UPDATE c
    SET ThoiGianBaoHanh = ISNULL(c.ThoiGianBaoHanh, l.ThoiGianBaoHanh),
        GiaVon = ISNULL(c.GiaVon, ISNULL((SELECT SUM(p.SoLuong * p.DonGiaNhap) / NULLIF(SUM(p.SoLuong), 0)
                                          FROM dbo.CHI_TIET_PN p
                                          WHERE p.MaLaptop = c.MaLaptop), 0))
    FROM dbo.CHI_TIET_HD c
    JOIN inserted i ON i.MaHD = c.MaHD AND i.MaLaptop = c.MaLaptop
    JOIN dbo.LAPTOP_DATA l ON l.MaLaptop = c.MaLaptop;
    ;WITH need AS (
        SELECT i.MaHD, i.MaLaptop, i.SoLuong,
               SUM(i.SoLuong) OVER (PARTITION BY i.MaLaptop ORDER BY i.MaHD ROWS UNBOUNDED PRECEDING) - i.SoLuong AS Dau
        FROM inserted i
    ),
    kho AS (
        SELECT SerialNo, MaLaptop,
               ROW_NUMBER() OVER (PARTITION BY MaLaptop ORDER BY SerialNo) AS rn
        FROM dbo.LAPTOP_SERIAL_DATA WITH (UPDLOCK,HOLDLOCK)
        WHERE TrangThai = N'Trong kho'
          AND MaLaptop IN (SELECT MaLaptop FROM inserted)
    )
    UPDATE s
    SET MaHD = n.MaHD,
        TrangThai = N'Đã bán'
    FROM dbo.LAPTOP_SERIAL_DATA s
    JOIN kho k ON k.SerialNo = s.SerialNo
    JOIN need n ON n.MaLaptop = k.MaLaptop AND k.rn > n.Dau AND k.rn <= n.Dau + n.SoLuong
    JOIN dbo.CHI_TIET_HD c ON c.MaHD = n.MaHD AND c.MaLaptop = n.MaLaptop;
    DECLARE @DaGan INT = @@ROWCOUNT;
    IF @DaGan < (SELECT SUM(SoLuong) FROM inserted)
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50041, N'Không đủ hàng tồn kho để bán.', 1;
    END;
END;
GO
CREATE OR ALTER TRIGGER [dbo].[trg_CTHD_Xoa]
    ON [dbo].[CHI_TIET_HD]
    AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1
               FROM dbo.BAO_HANH_SUA_CHUA b
               JOIN dbo.LAPTOP_SERIAL_DATA s ON s.SerialNo = b.SerialNo
               JOIN deleted d ON d.MaHD = s.MaHD AND d.MaLaptop = s.MaLaptop)
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50042, N'Không thể xóa: máy trong hóa đơn đã có phiếu bảo hành/sửa chữa.', 1;
    END;
    UPDATE s
    SET MaHD = NULL, TrangThai = N'Trong kho'
    FROM dbo.LAPTOP_SERIAL_DATA s
    JOIN deleted d ON d.MaHD = s.MaHD AND d.MaLaptop = s.MaLaptop;
END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_LAPTOP_SERIAL_D] ON [dbo].[LAPTOP_SERIAL] INSTEAD OF DELETE AS BEGIN SET NOCOUNT ON; DELETE b FROM dbo.[LAPTOP_SERIAL_DATA] b JOIN deleted d ON b.[SerialNo]=d.[SerialNo]; END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_LAPTOP_SERIAL_I] ON [dbo].[LAPTOP_SERIAL] INSTEAD OF INSERT AS BEGIN SET NOCOUNT ON; INSERT dbo.[LAPTOP_SERIAL_DATA]([SerialNo],[MaLaptop],[MaPN],[MaHD],[TrangThai]) SELECT i.[SerialNo],i.[MaLaptop],i.[MaPN],i.[MaHD],COALESCE(i.[TrangThai],(N'Trong kho')) FROM inserted i; END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_LAPTOP_SERIAL_U] ON [dbo].[LAPTOP_SERIAL] INSTEAD OF UPDATE AS BEGIN SET NOCOUNT ON;
            IF EXISTS(SELECT [SerialNo] FROM inserted EXCEPT SELECT [SerialNo] FROM deleted)
            BEGIN
                IF (SELECT COUNT(*) FROM inserted)<>1 THROW 51010,N'Chỉ đổi một mã mỗi lần.',1;
                UPDATE b SET b.[SerialNo]=i.[SerialNo],b.[MaLaptop]=i.[MaLaptop],b.[MaPN]=i.[MaPN],b.[MaHD]=i.[MaHD],b.[TrangThai]=i.[TrangThai] FROM dbo.[LAPTOP_SERIAL_DATA] b JOIN deleted d ON b.[SerialNo]=d.[SerialNo] CROSS JOIN inserted i;
            END
            ELSE UPDATE b SET b.[MaLaptop]=i.[MaLaptop],b.[MaPN]=i.[MaPN],b.[MaHD]=i.[MaHD],b.[TrangThai]=i.[TrangThai] FROM dbo.[LAPTOP_SERIAL_DATA] b JOIN inserted i ON b.[SerialNo]=i.[SerialNo]; END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_HOA_DON_D] ON [dbo].[HOA_DON] INSTEAD OF DELETE AS BEGIN SET NOCOUNT ON; DELETE b FROM dbo.[HOA_DON_DATA] b JOIN deleted d ON b.[MaHD]=d.[MaHD]; END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_HOA_DON_I] ON [dbo].[HOA_DON] INSTEAD OF INSERT AS BEGIN SET NOCOUNT ON; INSERT dbo.[HOA_DON_DATA]([MaHD],[MaKH],[MaNV],[NgayLap],[PhuongThucThanhToan],[ChuKyTraGop],[TinhTrang]) SELECT i.[MaHD],i.[MaKH],i.[MaNV],COALESCE(i.[NgayLap],(getdate())),COALESCE(i.[PhuongThucThanhToan],(N'Tiền mặt')),COALESCE(i.[ChuKyTraGop],((0))),COALESCE(i.[TinhTrang],(N'Đã thanh toán')) FROM inserted i; END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_HOA_DON_U]
    ON [dbo].[HOA_DON]
    INSTEAD OF UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS(SELECT [MaHD] FROM inserted EXCEPT SELECT [MaHD] FROM deleted)
    BEGIN
        IF (SELECT COUNT(*) FROM inserted) <> 1
            THROW 51010, N'Chỉ đổi một mã mỗi lần.', 1;
        UPDATE b
        SET b.[MaHD]=i.[MaHD], b.[MaKH]=i.[MaKH], b.[MaNV]=i.[MaNV], b.[NgayLap]=i.[NgayLap],
            b.[PhuongThucThanhToan]=i.[PhuongThucThanhToan], b.[ChuKyTraGop]=i.[ChuKyTraGop], b.[TinhTrang]=i.[TinhTrang]
        FROM dbo.[HOA_DON_DATA] b
        JOIN deleted d ON b.[MaHD]=d.[MaHD]
        CROSS JOIN inserted i;
    END
    ELSE
    BEGIN
        UPDATE b
        SET b.[MaKH]=i.[MaKH], b.[MaNV]=i.[MaNV], b.[NgayLap]=i.[NgayLap],
            b.[PhuongThucThanhToan]=i.[PhuongThucThanhToan], b.[ChuKyTraGop]=i.[ChuKyTraGop], b.[TinhTrang]=i.[TinhTrang]
        FROM dbo.[HOA_DON_DATA] b
        JOIN inserted i ON b.[MaHD]=i.[MaHD];
    END;
END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_KHACH_HANG_D] ON [dbo].[KHACH_HANG] INSTEAD OF DELETE AS BEGIN SET NOCOUNT ON; DELETE b FROM dbo.[KHACH_HANG_DATA] b JOIN deleted d ON b.[MaKH]=d.[MaKH]; END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_KHACH_HANG_I] ON [dbo].[KHACH_HANG] INSTEAD OF INSERT AS BEGIN SET NOCOUNT ON; INSERT dbo.[KHACH_HANG_DATA]([MaKH],[TenKH],[DiaChi],[Sdt],[GioiTinh],[NgaySinh],[DaKhoa]) SELECT i.[MaKH],i.[TenKH],i.[DiaChi],i.[Sdt],COALESCE(i.[GioiTinh],(N'Nam')),i.[NgaySinh],COALESCE(i.[DaKhoa],((0))) FROM inserted i; END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_KHACH_HANG_U] ON [dbo].[KHACH_HANG] INSTEAD OF UPDATE AS BEGIN SET NOCOUNT ON;
            IF EXISTS(SELECT [MaKH] FROM inserted EXCEPT SELECT [MaKH] FROM deleted)
            BEGIN
                IF (SELECT COUNT(*) FROM inserted)<>1 THROW 51010,N'Chỉ đổi một mã mỗi lần.',1;
                UPDATE b SET b.[MaKH]=i.[MaKH],b.[TenKH]=i.[TenKH],b.[DiaChi]=i.[DiaChi],b.[Sdt]=i.[Sdt],b.[GioiTinh]=i.[GioiTinh],b.[NgaySinh]=i.[NgaySinh],b.[DaKhoa]=i.[DaKhoa] FROM dbo.[KHACH_HANG_DATA] b JOIN deleted d ON b.[MaKH]=d.[MaKH] CROSS JOIN inserted i;
            END
            ELSE UPDATE b SET b.[TenKH]=i.[TenKH],b.[DiaChi]=i.[DiaChi],b.[Sdt]=i.[Sdt],b.[GioiTinh]=i.[GioiTinh],b.[NgaySinh]=i.[NgaySinh],b.[DaKhoa]=i.[DaKhoa] FROM dbo.[KHACH_HANG_DATA] b JOIN inserted i ON b.[MaKH]=i.[MaKH]; END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_PHIEU_NHAP_D] ON [dbo].[PHIEU_NHAP] INSTEAD OF DELETE AS BEGIN SET NOCOUNT ON; DELETE b FROM dbo.[PHIEU_NHAP_DATA] b JOIN deleted d ON b.[MaPN]=d.[MaPN]; END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_PHIEU_NHAP_I] ON [dbo].[PHIEU_NHAP] INSTEAD OF INSERT AS BEGIN SET NOCOUNT ON; INSERT dbo.[PHIEU_NHAP_DATA]([MaPN],[MaNCC],[MaNV],[NgayNhap]) SELECT i.[MaPN],i.[MaNCC],i.[MaNV],COALESCE(i.[NgayNhap],(getdate())) FROM inserted i; END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_PHIEU_NHAP_U] ON [dbo].[PHIEU_NHAP] INSTEAD OF UPDATE AS BEGIN SET NOCOUNT ON;
            IF EXISTS(SELECT [MaPN] FROM inserted EXCEPT SELECT [MaPN] FROM deleted)
            BEGIN
                IF (SELECT COUNT(*) FROM inserted)<>1 THROW 51010,N'Chỉ đổi một mã mỗi lần.',1;
                UPDATE b SET b.[MaPN]=i.[MaPN],b.[MaNCC]=i.[MaNCC],b.[MaNV]=i.[MaNV],b.[NgayNhap]=i.[NgayNhap] FROM dbo.[PHIEU_NHAP_DATA] b JOIN deleted d ON b.[MaPN]=d.[MaPN] CROSS JOIN inserted i;
            END
            ELSE UPDATE b SET b.[MaNCC]=i.[MaNCC],b.[MaNV]=i.[MaNV],b.[NgayNhap]=i.[NgayNhap] FROM dbo.[PHIEU_NHAP_DATA] b JOIN inserted i ON b.[MaPN]=i.[MaPN]; END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_LAPTOP_D] ON [dbo].[LAPTOP] INSTEAD OF DELETE AS BEGIN SET NOCOUNT ON; DELETE b FROM dbo.[LAPTOP_DATA] b JOIN deleted d ON b.[MaLaptop]=d.[MaLaptop]; END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_LAPTOP_I] ON [dbo].[LAPTOP] INSTEAD OF INSERT AS BEGIN SET NOCOUNT ON; INSERT dbo.[LAPTOP_DATA]([MaLaptop],[TenLaptop],[Loai],[CPU],[RAM],[SSD],[DonGiaNhap],[DonGia],[ThoiGianBaoHanh],[MaNCC],[Anh],[GhiChu],[NgungKinhDoanh]) SELECT i.[MaLaptop],i.[TenLaptop],i.[Loai],i.[CPU],i.[RAM],i.[SSD],COALESCE(i.[DonGiaNhap],((0))),i.[DonGia],COALESCE(i.[ThoiGianBaoHanh],((12))),i.[MaNCC],i.[Anh],i.[GhiChu],COALESCE(i.[NgungKinhDoanh],((0))) FROM inserted i; END;
GO
CREATE OR ALTER TRIGGER [dbo].[tr_compat_LAPTOP_U] ON [dbo].[LAPTOP] INSTEAD OF UPDATE AS BEGIN SET NOCOUNT ON;
            IF EXISTS(SELECT [MaLaptop] FROM inserted EXCEPT SELECT [MaLaptop] FROM deleted)
            BEGIN
                IF (SELECT COUNT(*) FROM inserted)<>1 THROW 51010,N'Chỉ đổi một mã mỗi lần.',1;
                UPDATE b SET b.[MaLaptop]=i.[MaLaptop],b.[TenLaptop]=i.[TenLaptop],b.[Loai]=i.[Loai],b.[CPU]=i.[CPU],b.[RAM]=i.[RAM],b.[SSD]=i.[SSD],b.[DonGiaNhap]=i.[DonGiaNhap],b.[DonGia]=i.[DonGia],b.[ThoiGianBaoHanh]=i.[ThoiGianBaoHanh],b.[MaNCC]=i.[MaNCC],b.[Anh]=i.[Anh],b.[GhiChu]=i.[GhiChu],b.[NgungKinhDoanh]=i.[NgungKinhDoanh] FROM dbo.[LAPTOP_DATA] b JOIN deleted d ON b.[MaLaptop]=d.[MaLaptop] CROSS JOIN inserted i;
            END
            ELSE UPDATE b SET b.[TenLaptop]=i.[TenLaptop],b.[Loai]=i.[Loai],b.[CPU]=i.[CPU],b.[RAM]=i.[RAM],b.[SSD]=i.[SSD],b.[DonGiaNhap]=i.[DonGiaNhap],b.[DonGia]=i.[DonGia],b.[ThoiGianBaoHanh]=i.[ThoiGianBaoHanh],b.[MaNCC]=i.[MaNCC],b.[Anh]=i.[Anh],b.[GhiChu]=i.[GhiChu],b.[NgungKinhDoanh]=i.[NgungKinhDoanh] FROM dbo.[LAPTOP_DATA] b JOIN inserted i ON b.[MaLaptop]=i.[MaLaptop]; END;
GO

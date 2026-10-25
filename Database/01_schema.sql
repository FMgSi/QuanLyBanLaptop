/* =====================================================================
   QuanLyBanLaptop - 01_schema.sql
   Bảng + ràng buộc + index + hàm + trigger + stored procedure + view.

   !!! Script DROP rồi tạo lại CSDL QuanLyBanLaptop. Đừng chạy trên DB đang
   có dữ liệu cần giữ.

   Quy ước (giống kiểu BLL của LibraryManagerWF):
     - Hàm GHI  : stored procedure sp_*, chạy trong 1 transaction, lỗi nghiệp vụ
                  ném THROW 500xx (C# bắt SqlException, lấy ex.Message).
     - Hàm ĐỌC  : view vw_* / hàm bảng fn_* (gán thẳng vào DataGridView).
     - Hàm phụ  : hàm vô hướng fn_* (băm mật khẩu, sinh mã, giá vốn, hạng...).
     - Mã hóa đơn/phiếu nhập/phiếu BH sinh bằng SEQUENCE (không trùng khi nhiều người dùng).
     - Danh sách hàng truyền bằng table-valued parameter (TVP_BanHang, TVP_NhapHang).
   ===================================================================== */

IF DB_ID(N'QuanLyBanLaptop_V2') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLyBanLaptop_V2 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyBanLaptop_V2;
END
GO
CREATE DATABASE QuanLyBanLaptop_V2;
GO
USE QuanLyBanLaptop_V2;
GO

/* =====================================================================
   1. BẢNG
   ===================================================================== */

-- Cấu hình nghiệp vụ, chỉnh được mà không sửa code
CREATE TABLE CAU_HINH (
    Khoa   VARCHAR(50)    PRIMARY KEY,
    GiaTri NVARCHAR(100)  NOT NULL,
    MoTa   NVARCHAR(200)  NULL
);

CREATE TABLE NHA_CUNG_CAP (
    MaNCC  VARCHAR(10)    PRIMARY KEY,
    TenNCC NVARCHAR(100)  NOT NULL,
    DiaChi NVARCHAR(200)  NULL,
    Sdt    VARCHAR(15)    NULL
);

CREATE TABLE LOAI_MAY (
    TenLoai NVARCHAR(50)  PRIMARY KEY,
    MoTa    NVARCHAR(200) NULL
);

CREATE TABLE LAPTOP (
    MaLaptop        VARCHAR(10)   PRIMARY KEY,
    TenLaptop       NVARCHAR(100) NOT NULL,
    Loai            NVARCHAR(50)  NULL REFERENCES LOAI_MAY(TenLoai),
    CPU             NVARCHAR(50)  NULL,
    RAM             VARCHAR(10)   NULL,
    SSD             VARCHAR(10)   NULL,
    DonGiaNhap      DECIMAL(18,2) NULL DEFAULT 0,
    DonGia          DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),
    SoLuongTon      INT           NOT NULL DEFAULT 0 CHECK (SoLuongTon >= 0),  -- chỉ sửa qua sp_NhapHang/sp_BanHang
    ThoiGianBaoHanh INT           NOT NULL DEFAULT 12 CHECK (ThoiGianBaoHanh >= 0),  -- tháng
    MaNCC           VARCHAR(10)   NOT NULL REFERENCES NHA_CUNG_CAP(MaNCC),
    Anh             NVARCHAR(255) NULL,
    GhiChu          NVARCHAR(500) NULL,
    NgungKinhDoanh  BIT           NOT NULL DEFAULT 0   -- soft delete
);

CREATE TABLE NHAN_VIEN (
    MaNV    VARCHAR(10)   PRIMARY KEY,
    TenNV   NVARCHAR(50)  NOT NULL,
    SDT     VARCHAR(15)   NULL,
    ChucVu  NVARCHAR(50)  NOT NULL CHECK (ChucVu IN (N'Quản trị viên', N'Nhân viên')),
    TenDN   VARCHAR(50)   NOT NULL UNIQUE,
    MatKhau VARCHAR(100)  NOT NULL,                    -- dạng salt$hash, xem sp_DatMatKhau
    DaNghi  BIT           NOT NULL DEFAULT 0           -- soft delete
);

CREATE TABLE KHACH_HANG (
    MaKH          VARCHAR(10)   PRIMARY KEY,
    TenKH         NVARCHAR(50)  NOT NULL,
    DiaChi        NVARCHAR(200) NULL,
    Sdt           VARCHAR(15)   NULL,
    HangThanhVien NVARCHAR(30)  NOT NULL DEFAULT N'Đồng'
        CHECK (HangThanhVien IN (N'Đồng', N'Bạc', N'Vàng', N'VIP'))   -- dẫn xuất: sp_CapNhatHangThanhVien
);
-- SĐT duy nhất nhưng cho phép nhiều khách chưa có SĐT
CREATE UNIQUE INDEX UX_KhachHang_Sdt ON KHACH_HANG(Sdt) WHERE Sdt IS NOT NULL;

CREATE TABLE HOA_DON (
    MaHD                VARCHAR(10)   PRIMARY KEY,
    MaKH                VARCHAR(10)   NOT NULL REFERENCES KHACH_HANG(MaKH),
    MaNV                VARCHAR(10)   NOT NULL REFERENCES NHAN_VIEN(MaNV),
    NgayLap             DATETIME      NOT NULL DEFAULT GETDATE(),
    PhuongThucThanhToan NVARCHAR(50)  NOT NULL DEFAULT N'Tiền mặt',
    ChuKyTraGop         INT           NOT NULL DEFAULT 0,
    TinhTrang           NVARCHAR(30)  NOT NULL DEFAULT N'Đã thanh toán',   -- "Trễ hạn" tính ở vw_HoaDon
    TongTien            DECIMAL(18,2) NOT NULL DEFAULT 0,                  -- do trigger giữ đồng bộ
    CONSTRAINT CK_HD_PhuongThuc CHECK (PhuongThucThanhToan IN (N'Tiền mặt', N'Chuyển khoản', N'Trả góp')),
    CONSTRAINT CK_HD_ChuKy      CHECK (ChuKyTraGop IN (0, 3, 6, 9, 12)),
    CONSTRAINT CK_HD_TinhTrang  CHECK (TinhTrang IN (N'Đã thanh toán', N'Đang trả góp')),
    CONSTRAINT CK_HD_TraGop     CHECK (
           (PhuongThucThanhToan = N'Trả góp'  AND ChuKyTraGop > 0)
        OR (PhuongThucThanhToan <> N'Trả góp' AND ChuKyTraGop = 0 AND TinhTrang = N'Đã thanh toán'))
);

CREATE TABLE CHI_TIET_HD (
    MaHD            VARCHAR(10)   NOT NULL REFERENCES HOA_DON(MaHD),
    MaLaptop        VARCHAR(10)   NOT NULL REFERENCES LAPTOP(MaLaptop),
    SoLuong         INT           NOT NULL CHECK (SoLuong > 0),
    DonGia          DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),            -- giá bán tại thời điểm bán
    GiaVon          DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (GiaVon >= 0),  -- giá vốn tại thời điểm bán
    ThoiGianBaoHanh INT           NOT NULL DEFAULT 12 CHECK (ThoiGianBaoHanh >= 0), -- tháng, chụp lúc bán
    PRIMARY KEY (MaHD, MaLaptop)
);

-- Lịch trả góp từng kỳ
CREATE TABLE LICH_TRA_GOP (
    MaHD          VARCHAR(10)   NOT NULL REFERENCES HOA_DON(MaHD),
    KyThu         INT           NOT NULL CHECK (KyThu > 0),
    NgayDenHan    DATE          NOT NULL,
    SoTienPhaiTra DECIMAL(18,2) NOT NULL CHECK (SoTienPhaiTra > 0),
    NgayTra       DATE          NULL,
    SoTienDaTra   DECIMAL(18,2) NOT NULL DEFAULT 0,
    PRIMARY KEY (MaHD, KyThu),
    CONSTRAINT CK_Lich_DaTra CHECK (
           (NgayTra IS NULL     AND SoTienDaTra = 0)
        OR (NgayTra IS NOT NULL AND SoTienDaTra = SoTienPhaiTra))
);

CREATE TABLE PHIEU_NHAP (
    MaPN     VARCHAR(10)   PRIMARY KEY,
    MaNCC    VARCHAR(10)   NOT NULL REFERENCES NHA_CUNG_CAP(MaNCC),
    MaNV     VARCHAR(10)   NOT NULL REFERENCES NHAN_VIEN(MaNV),
    NgayNhap DATETIME      NOT NULL DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0                              -- do trigger giữ đồng bộ
);

CREATE TABLE CHI_TIET_PN (
    MaPN       VARCHAR(10)   NOT NULL REFERENCES PHIEU_NHAP(MaPN),
    MaLaptop   VARCHAR(10)   NOT NULL REFERENCES LAPTOP(MaLaptop),
    SoLuong    INT           NOT NULL CHECK (SoLuong > 0),
    DonGiaNhap DECIMAL(18,2) NOT NULL CHECK (DonGiaNhap >= 0),
    PRIMARY KEY (MaPN, MaLaptop)
);

-- Từng chiếc máy. Nhập hàng sinh serial tự động (PNxxx-LTxxx-001...),
-- có thể đổi sang serial thật bằng sp_DoiSerial khi còn "Trong kho".
CREATE TABLE LAPTOP_SERIAL (
    SerialNo  VARCHAR(40) PRIMARY KEY,
    MaLaptop  VARCHAR(10) NOT NULL REFERENCES LAPTOP(MaLaptop),
    MaPN      VARCHAR(10) NOT NULL REFERENCES PHIEU_NHAP(MaPN),
    MaHD      VARCHAR(10) NULL,
    NgayHetBH DATE        NULL,
    TrangThai NVARCHAR(20) NOT NULL DEFAULT N'Trong kho'
        CHECK (TrangThai IN (N'Trong kho', N'Đã bán', N'Đang bảo hành', N'Hỏng')),
    CONSTRAINT FK_Serial_CTHD FOREIGN KEY (MaHD, MaLaptop) REFERENCES CHI_TIET_HD(MaHD, MaLaptop),
    CONSTRAINT CK_Serial_Ban CHECK (
           (TrangThai IN (N'Trong kho', N'Hỏng') AND MaHD IS NULL)
        OR (TrangThai IN (N'Đã bán', N'Đang bảo hành') AND MaHD IS NOT NULL))
);

CREATE TABLE BAO_HANH_SUA_CHUA (
    MaPhieuBH   VARCHAR(10)   PRIMARY KEY,
    SerialNo    VARCHAR(40)   NOT NULL REFERENCES LAPTOP_SERIAL(SerialNo),  -- suy ra MaHD, MaLaptop từ serial
    MaNV        VARCHAR(10)   NOT NULL REFERENCES NHAN_VIEN(MaNV),
    LoaiDichVu  NVARCHAR(30)  NOT NULL CHECK (LoaiDichVu IN (N'Bảo hành', N'Sửa chữa')),
    NgayNhan    DATETIME      NOT NULL DEFAULT GETDATE(),
    NgayTra     DATETIME      NULL,
    MoTaLoi     NVARCHAR(255) NOT NULL,
    ChiPhi      DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (ChiPhi >= 0),
    TinhTrangBH NVARCHAR(50)  NOT NULL DEFAULT N'Đang xử lý'
        CHECK (TinhTrangBH IN (N'Đang xử lý', N'Đã hoàn thành', N'Đã trả khách')),
    CONSTRAINT CK_BH_Ngay    CHECK (NgayTra IS NULL OR NgayTra >= NgayNhan),
    CONSTRAINT CK_BH_ChiPhi  CHECK (LoaiDichVu = N'Sửa chữa' OR ChiPhi = 0),
    CONSTRAINT CK_BH_DaTra   CHECK (TinhTrangBH <> N'Đã trả khách' OR NgayTra IS NOT NULL)
);
GO

/* =====================================================================
   2. INDEX (FK + cột ngày dùng cho báo cáo)
   ===================================================================== */
CREATE INDEX IX_Laptop_MaNCC          ON LAPTOP(MaNCC);
CREATE INDEX IX_HoaDon_MaKH           ON HOA_DON(MaKH);
CREATE INDEX IX_HoaDon_MaNV           ON HOA_DON(MaNV);
CREATE INDEX IX_HoaDon_NgayLap        ON HOA_DON(NgayLap);
CREATE INDEX IX_CTHD_MaLaptop         ON CHI_TIET_HD(MaLaptop);
CREATE INDEX IX_PhieuNhap_MaNCC       ON PHIEU_NHAP(MaNCC);
CREATE INDEX IX_PhieuNhap_NgayNhap    ON PHIEU_NHAP(NgayNhap);
CREATE INDEX IX_CTPN_MaLaptop         ON CHI_TIET_PN(MaLaptop);
CREATE INDEX IX_Serial_Kho            ON LAPTOP_SERIAL(MaLaptop, TrangThai);
CREATE INDEX IX_Serial_MaHD           ON LAPTOP_SERIAL(MaHD);
CREATE INDEX IX_BH_SerialNo           ON BAO_HANH_SUA_CHUA(SerialNo);
CREATE INDEX IX_Lich_NgayDenHan       ON LICH_TRA_GOP(NgayDenHan) WHERE NgayTra IS NULL;
GO

/* =====================================================================
   3. SEQUENCE + KIỂU DỮ LIỆU BẢNG (TVP)
   ===================================================================== */
CREATE SEQUENCE seq_MaHD AS INT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_MaPN AS INT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_MaBH AS INT START WITH 1 INCREMENT BY 1;
GO

CREATE TYPE dbo.TVP_BanHang AS TABLE (
    MaLaptop VARCHAR(10) NOT NULL PRIMARY KEY,
    SoLuong  INT         NOT NULL CHECK (SoLuong > 0)
);
CREATE TYPE dbo.TVP_NhapHang AS TABLE (
    MaLaptop   VARCHAR(10)   NOT NULL PRIMARY KEY,
    SoLuong    INT           NOT NULL CHECK (SoLuong > 0),
    DonGiaNhap DECIMAL(18,2) NOT NULL CHECK (DonGiaNhap >= 0)
);
GO

/* =====================================================================
   4. HÀM PHỤ (vô hướng / bảng)
   ===================================================================== */

-- Sinh mã: fn_Ma('HD', 7, 3) = 'HD007'. Số vượt độ rộng thì giữ nguyên (không cắt).
CREATE FUNCTION dbo.fn_Ma (@TienTo VARCHAR(5), @So INT, @DoRong INT)
RETURNS VARCHAR(40)
AS
BEGIN
    DECLARE @s VARCHAR(10) = CAST(@So AS VARCHAR(10));
    RETURN @TienTo + CASE WHEN LEN(@s) >= @DoRong THEN @s
                          ELSE RIGHT(REPLICATE('0', @DoRong) + @s, @DoRong) END;
END
GO

-- Dãy số 1..@n (tối đa 10.000), dùng để sinh serial / kỳ trả góp
CREATE FUNCTION dbo.fn_DaySo (@n INT)
RETURNS TABLE
AS RETURN
    WITH a(x) AS (SELECT x FROM (VALUES (1),(1),(1),(1),(1),(1),(1),(1),(1),(1)) v(x))
    SELECT TOP (@n) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS So
    FROM a a1 CROSS JOIN a a2 CROSS JOIN a a3 CROSS JOIN a a4
    ORDER BY So;
GO

-- Mật khẩu: SHA-256(salt + mật khẩu), hex in hoa. Lưu dạng "salt$hash".
-- Lưu ý: băm theo UTF-16 (NVARCHAR). Nếu băm ở C# thì dùng Encoding.Unicode.
CREATE FUNCTION dbo.fn_BamMatKhau (@Salt VARCHAR(32), @MatKhau NVARCHAR(100))
RETURNS CHAR(64)
AS
BEGIN
    RETURN CONVERT(CHAR(64), HASHBYTES('SHA2_256', @Salt + @MatKhau), 2);
END
GO

CREATE FUNCTION dbo.fn_KiemTraMatKhau (@DaLuu VARCHAR(100), @MatKhauNhap NVARCHAR(100))
RETURNS BIT
AS
BEGIN
    DECLARE @vt INT = CHARINDEX('$', @DaLuu);
    IF @vt = 0 RETURN 0;
    DECLARE @salt VARCHAR(32) = LEFT(@DaLuu, @vt - 1);
    RETURN CASE WHEN @DaLuu = @salt + '$' + dbo.fn_BamMatKhau(@salt, @MatKhauNhap) THEN 1 ELSE 0 END;
END
GO

-- Giá vốn bình quân gia quyền theo các lần nhập
CREATE FUNCTION dbo.fn_GiaVonTrungBinh (@MaLaptop VARCHAR(10))
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @v DECIMAL(18,2);
    SELECT @v = SUM(SoLuong * DonGiaNhap) / NULLIF(SUM(SoLuong), 0)
    FROM CHI_TIET_PN WHERE MaLaptop = @MaLaptop;
    RETURN ISNULL(@v, 0);
END
GO

CREATE FUNCTION dbo.fn_TongChiTieu (@MaKH VARCHAR(10))
RETURNS DECIMAL(18,2)
AS
BEGIN
    RETURN (SELECT ISNULL(SUM(TongTien), 0) FROM HOA_DON WHERE MaKH = @MaKH);
END
GO

CREATE FUNCTION dbo.fn_HangThanhVien (@TongChiTieu DECIMAL(18,2))
RETURNS NVARCHAR(30)
AS
BEGIN
    DECLARE @bac  DECIMAL(18,2) = (SELECT CAST(GiaTri AS DECIMAL(18,2)) FROM CAU_HINH WHERE Khoa = 'NGUONG_BAC');
    DECLARE @vang DECIMAL(18,2) = (SELECT CAST(GiaTri AS DECIMAL(18,2)) FROM CAU_HINH WHERE Khoa = 'NGUONG_VANG');
    DECLARE @vip  DECIMAL(18,2) = (SELECT CAST(GiaTri AS DECIMAL(18,2)) FROM CAU_HINH WHERE Khoa = 'NGUONG_VIP');
    RETURN CASE WHEN @TongChiTieu >= @vip  THEN N'VIP'
                WHEN @TongChiTieu >= @vang THEN N'Vàng'
                WHEN @TongChiTieu >= @bac  THEN N'Bạc'
                ELSE N'Đồng' END;
END
GO

-- Máy còn bảo hành tại ngày @Ngay không?
CREATE FUNCTION dbo.fn_ConBaoHanh (@SerialNo VARCHAR(40), @Ngay DATETIME)
RETURNS BIT
AS
BEGIN
    DECLARE @het DATE = (SELECT NgayHetBH FROM LAPTOP_SERIAL WHERE SerialNo = @SerialNo);
    RETURN CASE WHEN @het IS NOT NULL AND CAST(@Ngay AS DATE) <= @het THEN 1 ELSE 0 END;
END
GO

-- Báo cáo doanh thu / lợi nhuận theo ngày trong khoảng [@Tu, @Den]
CREATE FUNCTION dbo.fn_BaoCaoDoanhThu (@Tu DATE, @Den DATE)
RETURNS TABLE
AS RETURN
    SELECT CAST(hd.NgayLap AS DATE)                     AS Ngay,
           COUNT(DISTINCT hd.MaHD)                      AS SoHoaDon,
           SUM(ct.SoLuong)                              AS SoMayBan,
           SUM(ct.SoLuong * ct.DonGia)                  AS DoanhThu,
           SUM(ct.SoLuong * ct.GiaVon)                  AS GiaVon,
           SUM(ct.SoLuong * (ct.DonGia - ct.GiaVon))    AS LoiNhuan
    FROM HOA_DON hd
    JOIN CHI_TIET_HD ct ON ct.MaHD = hd.MaHD
    WHERE hd.NgayLap >= @Tu AND hd.NgayLap < DATEADD(DAY, 1, CAST(@Den AS DATETIME))
    GROUP BY CAST(hd.NgayLap AS DATE);
GO

/* =====================================================================
   5. TRIGGER GIỮ ĐỒNG BỘ TỔNG TIỀN
   ===================================================================== */
CREATE TRIGGER trg_CapNhatTongTienHD ON CHI_TIET_HD
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE hd
    SET TongTien = ISNULL((SELECT SUM(SoLuong * DonGia) FROM CHI_TIET_HD ct WHERE ct.MaHD = hd.MaHD), 0)
    FROM HOA_DON hd
    WHERE hd.MaHD IN (SELECT MaHD FROM inserted UNION SELECT MaHD FROM deleted);
END
GO

CREATE TRIGGER trg_CapNhatTongTienPN ON CHI_TIET_PN
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE pn
    SET TongTien = ISNULL((SELECT SUM(SoLuong * DonGiaNhap) FROM CHI_TIET_PN ct WHERE ct.MaPN = pn.MaPN), 0)
    FROM PHIEU_NHAP pn
    WHERE pn.MaPN IN (SELECT MaPN FROM inserted UNION SELECT MaPN FROM deleted);
END
GO

/* =====================================================================
   6. STORED PROCEDURE
   ===================================================================== */

/* ---------- 6.1 Tài khoản ---------- */

CREATE PROCEDURE dbo.sp_DatMatKhau @MaNV VARCHAR(10), @MatKhauMoi NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF LEN(@MatKhauMoi) < 6 THROW 50001, N'Mật khẩu phải có ít nhất 6 ký tự.', 1;
    DECLARE @salt VARCHAR(32) = LOWER(LEFT(REPLACE(CONVERT(VARCHAR(36), NEWID()), '-', ''), 16));
    UPDATE NHAN_VIEN SET MatKhau = @salt + '$' + dbo.fn_BamMatKhau(@salt, @MatKhauMoi) WHERE MaNV = @MaNV;
    IF @@ROWCOUNT = 0 THROW 50002, N'Nhân viên không tồn tại.', 1;
END
GO

-- Trả về 1 dòng (MaNV, TenNV, ChucVu) nếu đúng, không trả dòng nào nếu sai
CREATE PROCEDURE dbo.sp_DangNhap @TenDN VARCHAR(50), @MatKhau NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaNV, TenNV, ChucVu
    FROM NHAN_VIEN
    WHERE TenDN = @TenDN AND DaNghi = 0 AND dbo.fn_KiemTraMatKhau(MatKhau, @MatKhau) = 1;
END
GO

CREATE PROCEDURE dbo.sp_DoiMatKhau @MaNV VARCHAR(10), @MatKhauCu NVARCHAR(100), @MatKhauMoi NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = @MaNV AND dbo.fn_KiemTraMatKhau(MatKhau, @MatKhauCu) = 1)
        THROW 50003, N'Mật khẩu hiện tại không đúng.', 1;
    EXEC dbo.sp_DatMatKhau @MaNV, @MatKhauMoi;
END
GO

/* ---------- 6.2 Khách hàng / Laptop ---------- */

-- @MaKH = NULL: cập nhật hạng cho tất cả khách
CREATE PROCEDURE dbo.sp_CapNhatHangThanhVien @MaKH VARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE kh
    SET HangThanhVien = dbo.fn_HangThanhVien(dbo.fn_TongChiTieu(kh.MaKH))
    FROM KHACH_HANG kh
    WHERE @MaKH IS NULL OR kh.MaKH = @MaKH;
END
GO

CREATE PROCEDURE dbo.sp_NgungKinhDoanh @MaLaptop VARCHAR(10), @Ngung BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE LAPTOP SET NgungKinhDoanh = @Ngung WHERE MaLaptop = @MaLaptop;
    IF @@ROWCOUNT = 0 THROW 50004, N'Laptop không tồn tại.', 1;
END
GO

/* ---------- 6.3 Nhập hàng ---------- */

CREATE PROCEDURE dbo.sp_NhapHang
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

        INSERT LAPTOP_SERIAL (SerialNo, MaLaptop, MaPN)
        SELECT @MaPN + '-' + i.MaLaptop + '-' + dbo.fn_Ma('', d.So, 3), i.MaLaptop, @MaPN
        FROM @Items i CROSS APPLY dbo.fn_DaySo(i.SoLuong) d;

        UPDATE l SET SoLuongTon = l.SoLuongTon + i.SoLuong
        FROM LAPTOP l JOIN @Items i ON i.MaLaptop = l.MaLaptop;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

-- Đổi serial tự sinh sang serial thật in trên máy (chỉ khi còn trong kho)
CREATE PROCEDURE dbo.sp_DoiSerial @SerialCu VARCHAR(40), @SerialMoi VARCHAR(40)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM LAPTOP_SERIAL WHERE SerialNo = @SerialMoi) THROW 50024, N'Serial mới đã tồn tại.', 1;
    UPDATE LAPTOP_SERIAL SET SerialNo = @SerialMoi WHERE SerialNo = @SerialCu AND TrangThai = N'Trong kho';
    IF @@ROWCOUNT = 0 THROW 50025, N'Serial không tồn tại hoặc máy không còn trong kho.', 1;
END
GO

/* ---------- 6.4 Bán hàng + trả góp ---------- */

-- Tạo lịch trả góp cho hóa đơn (gọi nội bộ từ sp_BanHang)
CREATE PROCEDURE dbo.sp_TaoLichTraGop @MaHD VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @tong DECIMAL(18,2), @ky INT, @ngay DATE;
    SELECT @tong = TongTien, @ky = ChuKyTraGop, @ngay = CAST(NgayLap AS DATE)
    FROM HOA_DON WHERE MaHD = @MaHD AND PhuongThucThanhToan = N'Trả góp';

    IF @ky IS NULL THROW 50030, N'Hóa đơn không tồn tại hoặc không phải trả góp.', 1;
    IF EXISTS (SELECT 1 FROM LICH_TRA_GOP WHERE MaHD = @MaHD) THROW 50031, N'Hóa đơn đã có lịch trả góp.', 1;

    DECLARE @moiKy DECIMAL(18,2) = ROUND(@tong / @ky, 0);
    INSERT LICH_TRA_GOP (MaHD, KyThu, NgayDenHan, SoTienPhaiTra)
    SELECT @MaHD, d.So, DATEADD(MONTH, d.So, @ngay),
           CASE WHEN d.So = @ky THEN @tong - @moiKy * (@ky - 1) ELSE @moiKy END   -- kỳ cuối gánh phần dư làm tròn
    FROM dbo.fn_DaySo(@ky) d;
END
GO

CREATE PROCEDURE dbo.sp_BanHang
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

        -- Khóa các dòng laptop để chặn bán vượt tồn khi nhiều người bán cùng lúc
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

        -- Chụp giá bán, giá vốn, thời hạn bảo hành tại thời điểm bán
        INSERT CHI_TIET_HD (MaHD, MaLaptop, SoLuong, DonGia, GiaVon, ThoiGianBaoHanh)
        SELECT @MaHD, i.MaLaptop, i.SoLuong, l.DonGia, dbo.fn_GiaVonTrungBinh(l.MaLaptop), l.ThoiGianBaoHanh
        FROM @Items i JOIN LAPTOP l ON l.MaLaptop = i.MaLaptop;

        -- Gán serial (máy nhập trước bán trước)
        SELECT @tongMay = SUM(SoLuong) FROM @Items;
        ;WITH xepHang AS (
            SELECT s.SerialNo, s.MaLaptop,
                   ROW_NUMBER() OVER (PARTITION BY s.MaLaptop ORDER BY s.SerialNo) AS rn
            FROM LAPTOP_SERIAL s WITH (UPDLOCK, HOLDLOCK)
            WHERE s.TrangThai = N'Trong kho' AND s.MaLaptop IN (SELECT MaLaptop FROM @Items)
        )
        UPDATE s
        SET MaHD = @MaHD, TrangThai = N'Đã bán',
            NgayHetBH = DATEADD(MONTH, ct.ThoiGianBaoHanh, CAST(@Ngay AS DATE))
        FROM LAPTOP_SERIAL s
        JOIN xepHang x ON x.SerialNo = s.SerialNo
        JOIN @Items i ON i.MaLaptop = x.MaLaptop AND x.rn <= i.SoLuong
        JOIN CHI_TIET_HD ct ON ct.MaHD = @MaHD AND ct.MaLaptop = s.MaLaptop;
        IF @@ROWCOUNT <> @tongMay THROW 50018, N'Số serial trong kho không khớp tồn kho. Hãy chạy 03_doi_soat.sql.', 1;

        UPDATE l SET SoLuongTon = l.SoLuongTon - i.SoLuong
        FROM LAPTOP l JOIN @Items i ON i.MaLaptop = l.MaLaptop;

        IF @PhuongThuc = N'Trả góp' EXEC dbo.sp_TaoLichTraGop @MaHD;
        EXEC dbo.sp_CapNhatHangThanhVien @MaKH;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

-- Thu một kỳ trả góp; thu đủ mọi kỳ thì hóa đơn chuyển "Đã thanh toán"
CREATE PROCEDURE dbo.sp_ThanhToanKy @MaHD VARCHAR(10), @KyThu INT, @NgayTra DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRAN;
        UPDATE LICH_TRA_GOP
        SET NgayTra = ISNULL(@NgayTra, CAST(GETDATE() AS DATE)), SoTienDaTra = SoTienPhaiTra
        WHERE MaHD = @MaHD AND KyThu = @KyThu AND NgayTra IS NULL;
        IF @@ROWCOUNT = 0 THROW 50040, N'Kỳ trả góp không tồn tại hoặc đã được thanh toán.', 1;

        IF NOT EXISTS (SELECT 1 FROM LICH_TRA_GOP WHERE MaHD = @MaHD AND NgayTra IS NULL)
            UPDATE HOA_DON SET TinhTrang = N'Đã thanh toán' WHERE MaHD = @MaHD;
        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

/* ---------- 6.5 Bảo hành / sửa chữa ---------- */

CREATE PROCEDURE dbo.sp_TiepNhanBaoHanh
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
        IF NOT EXISTS (SELECT 1 FROM LAPTOP_SERIAL WITH (UPDLOCK) WHERE SerialNo = @SerialNo AND TrangThai = N'Đã bán')
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

-- Đổi trạng thái phiếu; "Đã trả khách" thì máy trở lại "Đã bán"
CREATE PROCEDURE dbo.sp_CapNhatBaoHanh
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
        SELECT @serial = SerialNo, @loai = LoaiDichVu FROM BAO_HANH_SUA_CHUA WITH (UPDLOCK) WHERE MaPhieuBH = @MaPhieuBH;
        IF @serial IS NULL THROW 50061, N'Phiếu không tồn tại.', 1;

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

/* =====================================================================
   7. VIEW BÁO CÁO / TRA CỨU
   ===================================================================== */

-- Hóa đơn kèm tên + tình trạng hiện tại ("Trễ hạn" được tính, không lưu tay)
CREATE VIEW dbo.vw_HoaDon AS
SELECT hd.MaHD, hd.NgayLap, hd.MaKH, kh.TenKH, hd.MaNV, nv.TenNV,
       hd.PhuongThucThanhToan, hd.ChuKyTraGop, hd.TongTien,
       CASE WHEN hd.TinhTrang = N'Đang trả góp' AND EXISTS (
                 SELECT 1 FROM LICH_TRA_GOP l
                 WHERE l.MaHD = hd.MaHD AND l.NgayTra IS NULL AND l.NgayDenHan < CAST(GETDATE() AS DATE))
            THEN N'Trễ hạn' ELSE hd.TinhTrang END AS TinhTrang
FROM HOA_DON hd
JOIN KHACH_HANG kh ON kh.MaKH = hd.MaKH
JOIN NHAN_VIEN  nv ON nv.MaNV = hd.MaNV;
GO

CREATE VIEW dbo.vw_CongNo AS
SELECT hd.MaHD, hd.MaKH, kh.TenKH, hd.TongTien,
       SUM(l.SoTienDaTra)                                   AS DaTra,
       hd.TongTien - SUM(l.SoTienDaTra)                     AS ConNo,
       SUM(CASE WHEN l.NgayTra IS NULL AND l.NgayDenHan < CAST(GETDATE() AS DATE) THEN 1 ELSE 0 END) AS SoKyTre,
       MIN(CASE WHEN l.NgayTra IS NULL THEN l.NgayDenHan END) AS KyDenHanGanNhat
FROM HOA_DON hd
JOIN KHACH_HANG kh ON kh.MaKH = hd.MaKH
JOIN LICH_TRA_GOP l ON l.MaHD = hd.MaHD
GROUP BY hd.MaHD, hd.MaKH, kh.TenKH, hd.TongTien;
GO

CREATE VIEW dbo.vw_TonKho AS
SELECT l.MaLaptop, l.TenLaptop, l.SoLuongTon,
       SUM(CASE WHEN s.TrangThai = N'Trong kho'     THEN 1 ELSE 0 END) AS SoMayTrongKho,
       SUM(CASE WHEN s.TrangThai = N'Đang bảo hành' THEN 1 ELSE 0 END) AS SoMayDangBaoHanh,
       SUM(CASE WHEN s.TrangThai = N'Hỏng'          THEN 1 ELSE 0 END) AS SoMayHong,
       l.SoLuongTon - SUM(CASE WHEN s.TrangThai = N'Trong kho' THEN 1 ELSE 0 END) AS ChenhLech
FROM LAPTOP l LEFT JOIN LAPTOP_SERIAL s ON s.MaLaptop = l.MaLaptop
GROUP BY l.MaLaptop, l.TenLaptop, l.SoLuongTon;
GO

CREATE VIEW dbo.vw_DoanhThuThang AS
SELECT YEAR(hd.NgayLap) AS Nam, MONTH(hd.NgayLap) AS Thang,
       COUNT(DISTINCT hd.MaHD)                   AS SoHoaDon,
       SUM(ct.SoLuong)                           AS SoMayBan,
       SUM(ct.SoLuong * ct.DonGia)               AS DoanhThu,
       SUM(ct.SoLuong * ct.GiaVon)               AS GiaVon,
       SUM(ct.SoLuong * (ct.DonGia - ct.GiaVon)) AS LoiNhuan
FROM HOA_DON hd JOIN CHI_TIET_HD ct ON ct.MaHD = hd.MaHD
GROUP BY YEAR(hd.NgayLap), MONTH(hd.NgayLap);
GO

CREATE VIEW dbo.vw_DoanhThuLaptop AS
SELECT l.MaLaptop, l.TenLaptop,
       SUM(ct.SoLuong)                           AS SoMayBan,
       SUM(ct.SoLuong * ct.DonGia)               AS DoanhThu,
       SUM(ct.SoLuong * (ct.DonGia - ct.GiaVon)) AS LoiNhuan
FROM CHI_TIET_HD ct JOIN LAPTOP l ON l.MaLaptop = ct.MaLaptop
GROUP BY l.MaLaptop, l.TenLaptop;
GO

CREATE VIEW dbo.vw_DoanhThuNhanVien AS
SELECT nv.MaNV, nv.TenNV,
       COUNT(DISTINCT hd.MaHD)                   AS SoHoaDon,
       SUM(ct.SoLuong * ct.DonGia)               AS DoanhThu,
       SUM(ct.SoLuong * (ct.DonGia - ct.GiaVon)) AS LoiNhuan
FROM HOA_DON hd
JOIN CHI_TIET_HD ct ON ct.MaHD = hd.MaHD
JOIN NHAN_VIEN nv ON nv.MaNV = hd.MaNV
GROUP BY nv.MaNV, nv.TenNV;
GO

CREATE VIEW dbo.vw_BaoHanh AS
SELECT bh.MaPhieuBH, bh.SerialNo, s.MaLaptop, lt.TenLaptop, s.MaHD, hd.MaKH, kh.TenKH,
       bh.LoaiDichVu, bh.NgayNhan, bh.NgayTra, bh.MoTaLoi, bh.ChiPhi, bh.TinhTrangBH,
       s.NgayHetBH, dbo.fn_ConBaoHanh(bh.SerialNo, bh.NgayNhan) AS ConHanLucNhan
FROM BAO_HANH_SUA_CHUA bh
JOIN LAPTOP_SERIAL s ON s.SerialNo = bh.SerialNo
JOIN LAPTOP lt       ON lt.MaLaptop = s.MaLaptop
JOIN HOA_DON hd      ON hd.MaHD = s.MaHD
JOIN KHACH_HANG kh   ON kh.MaKH = hd.MaKH;
GO

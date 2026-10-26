-- Chạy sau migration 11; không reset tài khoản hay mật khẩu hiện có.
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET XACT_ABORT ON;
BEGIN TRY
BEGIN TRAN;
IF OBJECT_ID('dbo.LAPTOP_DATA','U') IS NULL THROW 51100,N'Cần chạy migration 11 trước.',1;
-- Chuyển mật khẩu rõ sang hash có salt mà vẫn giữ mật khẩu đăng nhập cũ.
UPDATE n SET MatKhau=s.salt+'$'+dbo.fn_BamMatKhau(s.salt,CONVERT(nvarchar(100),n.MatKhau))
FROM dbo.NHAN_VIEN n CROSS APPLY(SELECT CONVERT(varchar(32),CRYPT_GEN_RANDOM(16),2) salt) s
WHERE NOT (CHARINDEX('$',MatKhau) IN(17,33) AND LEN(MatKhau)-CHARINDEX('$',MatKhau)=64
    AND RIGHT(MatKhau,64) COLLATE Latin1_General_BIN2 NOT LIKE '%[^0-9A-Fa-f]%');
EXEC(N'CREATE OR ALTER PROCEDURE dbo.sp_DatMatKhau
    @MaNV        VARCHAR(10),
    @MatKhauMoi NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF LEN(@MatKhauMoi)<6 THROW 50210,N''Mật khẩu phải có ít nhất 6 ký tự.'',1;
    DECLARE @salt varchar(32)=CONVERT(varchar(32),CRYPT_GEN_RANDOM(16),2);
    UPDATE NHAN_VIEN SET MatKhau=@salt+''$''+dbo.fn_BamMatKhau(@salt,@MatKhauMoi) WHERE MaNV=@MaNV;
    IF @@ROWCOUNT = 0 THROW 50002, N''Nhan vien khong ton tai.'', 1;
END
');
EXEC(N'CREATE OR ALTER PROCEDURE dbo.sp_ThemNhanVien
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
    IF LEN(LTRIM(RTRIM(@TenNV))) = 0 THROW 50200, N''Ten nhan vien khong duoc de trong.'', 1;
    IF @GioiTinh NOT IN (N''Nam'', N''Nữ'') THROW 50201, N''Gioi tinh khong hop le.'', 1;
    IF @ChucVu NOT IN (N''Quản trị viên'', N''Nhân viên'') THROW 50202, N''Chuc vu khong hop le.'', 1;
    IF LEN(LTRIM(RTRIM(@TenDN))) = 0 THROW 50203, N''Ten dang nhap khong duoc de trong.'', 1;
    IF EXISTS (SELECT 1 FROM NHAN_VIEN WHERE TenDN = @TenDN) THROW 50204, N''Ten dang nhap da ton tai.'', 1;

    IF @MaNV IS NULL OR @MaNV = '''' OR @MaNV = N''(Tự động)''
    BEGIN
        DECLARE @nextSo INT = NEXT VALUE FOR dbo.seq_MaNV;
        WHILE EXISTS (SELECT 1 FROM NHAN_VIEN WHERE MaNV = dbo.fn_Ma(''NV'', @nextSo, 3))
        BEGIN
            SET @nextSo = NEXT VALUE FOR dbo.seq_MaNV;
        END
        SET @MaNV = dbo.fn_Ma(''NV'', @nextSo, 3);
    END

    IF LEN(@MatKhau)<6 THROW 50210,N''Mật khẩu phải có ít nhất 6 ký tự.'',1;
    DECLARE @salt varchar(32)=CONVERT(varchar(32),CRYPT_GEN_RANDOM(16),2);
    INSERT NHAN_VIEN (MaNV, TenNV, GioiTinh, NgaySinh, SDT, DiaChi, ChucVu, TenDN, MatKhau)
    VALUES (@MaNV, @TenNV, @GioiTinh, @NgaySinh, NULLIF(@SDT,''''), @DiaChi, @ChucVu, @TenDN, @salt+''$''+dbo.fn_BamMatKhau(@salt,@MatKhau));
END
');
EXEC(N'CREATE OR ALTER PROCEDURE dbo.sp_CapNhatNhanVien
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
        THROW 50205, N''Nhan vien khong ton tai.'', 1;
    IF LEN(LTRIM(RTRIM(@TenNV))) = 0 THROW 50200, N''Ten nhan vien khong duoc de trong.'', 1;
    IF @GioiTinh NOT IN (N''Nam'', N''Nữ'') THROW 50201, N''Gioi tinh khong hop le.'', 1;
    IF @ChucVu NOT IN (N''Quản trị viên'', N''Nhân viên'') THROW 50202, N''Chuc vu khong hop le.'', 1;
    IF EXISTS (SELECT 1 FROM NHAN_VIEN WHERE TenDN = @TenDN AND MaNV <> @MaNV)
        THROW 50204, N''Ten dang nhap da ton tai.'', 1;

    IF NULLIF(@MatKhau,N'''') IS NOT NULL AND LEN(@MatKhau)<6 THROW 50210,N''Mật khẩu phải có ít nhất 6 ký tự.'',1;
    DECLARE @salt varchar(32)=CONVERT(varchar(32),CRYPT_GEN_RANDOM(16),2);
    UPDATE NHAN_VIEN
    SET TenNV     = @TenNV,
        GioiTinh  = @GioiTinh,
        NgaySinh  = @NgaySinh,
        SDT       = NULLIF(@SDT,''''),
        DiaChi    = @DiaChi,
        ChucVu    = @ChucVu,
        TenDN     = @TenDN,
        MatKhau   = CASE WHEN @MatKhau IS NOT NULL AND @MatKhau <> '''' THEN @salt+''$''+dbo.fn_BamMatKhau(@salt,@MatKhau) ELSE MatKhau END
    WHERE MaNV = @MaNV;
END
');
EXEC(N'CREATE OR ALTER PROCEDURE dbo.sp_TimKiemNhanVien
    @TuKhoa NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaNV, TenNV, GioiTinh,
           CONVERT(NVARCHAR(10), NgaySinh, 103) AS NgaySinh,
           SDT, DiaChi, ChucVu, TenDN, CAST(NULL AS varchar(100)) AS MatKhau,
           CASE DaNghi WHEN 1 THEN N''Đã nghỉ'' ELSE N''Đang làm'' END AS TrangThai
    FROM NHAN_VIEN
    WHERE @TuKhoa IS NULL OR @TuKhoa = ''''
       OR TenNV LIKE N''%'' + @TuKhoa + N''%''
       OR MaNV  LIKE N''%'' + @TuKhoa + N''%''
       OR SDT   LIKE N''%'' + @TuKhoa + N''%''
    ORDER BY MaNV;
END
');
EXEC(N'CREATE OR ALTER PROCEDURE dbo.sp_DangNhap
    @TenDN   VARCHAR(50),
    @MatKhau NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaNV, TenNV, ChucVu, TenDN
    FROM NHAN_VIEN
    WHERE TenDN = @TenDN
      AND dbo.fn_KiemTraMatKhau(MatKhau, @MatKhau) = 1
      AND DaNghi = 0;
END
');
-- Sequence cấp mã nguyên tử, không dùng MAX+1 trên các máy khách.
DECLARE @keys table(TableName sysname,ColumnName sysname,SequenceName sysname);
INSERT @keys VALUES('LAPTOP_DATA','MaLaptop','seq_AppLaptop'),
('NHA_CUNG_CAP','MaNCC','seq_AppSupplier'),('KHACH_HANG_DATA','MaKH','seq_AppCustomer'),
('NHAN_VIEN','MaNV','seq_AppEmployee');
DECLARE @table sysname,@column sysname,@sequence sysname,@next bigint,@sql nvarchar(max);
DECLARE keys_cursor CURSOR LOCAL FAST_FORWARD FOR SELECT * FROM @keys;
OPEN keys_cursor;
FETCH NEXT FROM keys_cursor INTO @table,@column,@sequence;
WHILE @@FETCH_STATUS=0
BEGIN
    IF OBJECT_ID('dbo.'+@sequence,'SO') IS NULL
    BEGIN
        SET @sql=N'SELECT @next=ISNULL(MAX(TRY_CONVERT(bigint,SUBSTRING('+QUOTENAME(@column)+
          N',PATINDEX(''%[0-9]%'','+QUOTENAME(@column)+N'+''0''),30))),0)+1 FROM dbo.'+QUOTENAME(@table);
        EXEC sys.sp_executesql @sql,N'@next bigint OUTPUT',@next OUTPUT;
        SET @sql=N'CREATE SEQUENCE dbo.'+QUOTENAME(@sequence)+N' AS bigint START WITH '+CONVERT(nvarchar(30),@next)+N' INCREMENT BY 1;';
        EXEC sys.sp_executesql @sql;
    END;
    FETCH NEXT FROM keys_cursor INTO @table,@column,@sequence;
END;
CLOSE keys_cursor;
DEALLOCATE keys_cursor;
IF OBJECT_ID('dbo.seq_AppInvoice','SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.seq_AppInvoice AS bigint START WITH 1 INCREMENT BY 1;');

COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK;
    THROW;
END CATCH;

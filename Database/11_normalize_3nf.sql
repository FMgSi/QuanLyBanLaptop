-- Chuẩn hóa dữ liệu dẫn xuất; chạy sau 01..10 trên bản sao đã sao lưu.
-- Không đổi DB trong script: chọn DB đích bằng sqlcmd -d hoặc SSMS.
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRAN;
    IF OBJECT_ID('dbo.LAPTOP_DATA','U') IS NOT NULL
        THROW 51000, N'Migration 11 đã áp dụng. Không chạy lại.', 1;
    IF COL_LENGTH('dbo.CHI_TIET_HD','GiamGia') IS NULL
        THROW 51001, N'Cần hoàn thành migration 01..10 trước.', 1;

    -- Dừng khi tiền lịch sử không khớp; không tự sửa giá trị giao dịch.
    IF EXISTS (SELECT 1 FROM dbo.CHI_TIET_HD WHERE GiamGia NOT BETWEEN 0 AND 100
        OR (ThanhTien <> 0 AND ThanhTien <> CONVERT(decimal(18,2),SoLuong*DonGia*(1-GiamGia/100.0))))
        THROW 51002, N'Chi tiết hóa đơn có giảm giá/thành tiền không hợp lệ. Đối soát trước.', 1;
    IF EXISTS (SELECT 1 FROM dbo.HOA_DON h OUTER APPLY (
        SELECT SUM(CONVERT(decimal(18,2),c.SoLuong*c.DonGia*(1-c.GiamGia/100.0))) total
        FROM dbo.CHI_TIET_HD c WHERE c.MaHD=h.MaHD) t WHERE h.TongTien<>ISNULL(t.total,0))
        THROW 51003, N'Tổng hóa đơn không khớp chi tiết. Đối soát trước.', 1;
    IF EXISTS (SELECT 1 FROM dbo.LAPTOP l WHERE l.SoLuongTon <>
        (SELECT COUNT(*) FROM dbo.LAPTOP_SERIAL s WHERE s.MaLaptop=l.MaLaptop AND s.TrangThai=N'Trong kho'))
        THROW 51004, N'Tồn kho không khớp serial. Đối soát trước.', 1;
    IF EXISTS (SELECT 1 FROM dbo.PHIEU_NHAP p OUTER APPLY (
        SELECT SUM(c.SoLuong*c.DonGiaNhap) total FROM dbo.CHI_TIET_PN c WHERE c.MaPN=p.MaPN) t
        WHERE p.TongTien<>ISNULL(t.total,0))
        THROW 51005, N'Tổng phiếu nhập không khớp chi tiết. Đối soát trước.', 1;
    IF EXISTS (SELECT 1 FROM dbo.LAPTOP_SERIAL s
        LEFT JOIN dbo.CHI_TIET_PN p ON p.MaPN=s.MaPN AND p.MaLaptop=s.MaLaptop WHERE p.MaPN IS NULL)
        THROW 51006, N'Serial thiếu chi tiết phiếu nhập tương ứng.', 1;
    -- Giữ nguyên hạn bảo hành lịch sử khác công thức bằng quan hệ ngoại lệ riêng.
    SELECT TOP(0) s.SerialNo,h.MaHD,s.NgayHetBH
    INTO dbo.SERIAL_BAO_HANH_DAC_THU
    FROM dbo.LAPTOP_SERIAL s CROSS JOIN dbo.HOA_DON h;
    ALTER TABLE dbo.SERIAL_BAO_HANH_DAC_THU ADD CONSTRAINT PK_SerialBaoHanhDacThu PRIMARY KEY(SerialNo,MaHD);
    ALTER TABLE dbo.SERIAL_BAO_HANH_DAC_THU ADD CONSTRAINT FK_BHDacThu_Serial
        FOREIGN KEY(SerialNo) REFERENCES dbo.LAPTOP_SERIAL(SerialNo);
    ALTER TABLE dbo.SERIAL_BAO_HANH_DAC_THU ADD CONSTRAINT FK_BHDacThu_HD
        FOREIGN KEY(MaHD) REFERENCES dbo.HOA_DON(MaHD) ON DELETE CASCADE;
    INSERT dbo.SERIAL_BAO_HANH_DAC_THU(SerialNo,MaHD,NgayHetBH)
    SELECT s.SerialNo,s.MaHD,s.NgayHetBH FROM dbo.LAPTOP_SERIAL s
    JOIN dbo.CHI_TIET_HD c ON c.MaHD=s.MaHD AND c.MaLaptop=s.MaLaptop
    JOIN dbo.HOA_DON h ON h.MaHD=s.MaHD
    WHERE s.NgayHetBH IS NULL OR s.NgayHetBH<>DATEADD(month,c.ThoiGianBaoHanh,CAST(h.NgayLap AS date));

    -- Thành tiền chỉ có một công thức; không còn cột nhập tay gây sai lệch.
    DROP TRIGGER IF EXISTS dbo.trg_CapNhatTongTienHD;
    DROP TRIGGER IF EXISTS dbo.trg_CapNhatTongTienPN;
    DROP TRIGGER IF EXISTS dbo.trg_CTHD_TongTien;
    DROP TRIGGER IF EXISTS dbo.trg_CTPN_TongTien;
    DROP TRIGGER IF EXISTS dbo.trg_HD_HangThanhVien;
    DECLARE @ddl nvarchar(max)=N'';
    SELECT @ddl+=N'ALTER TABLE dbo.CHI_TIET_HD DROP CONSTRAINT '+QUOTENAME(name)+N';'
    FROM sys.default_constraints WHERE parent_object_id=OBJECT_ID('dbo.CHI_TIET_HD')
        AND parent_column_id=COLUMNPROPERTY(OBJECT_ID('dbo.CHI_TIET_HD'),'ThanhTien','ColumnId');
    EXEC sys.sp_executesql @ddl;
    ALTER TABLE dbo.CHI_TIET_HD DROP COLUMN ThanhTien;
    EXEC(N'ALTER TABLE dbo.CHI_TIET_HD ADD ThanhTien AS
        CONVERT(decimal(18,2),SoLuong*DonGia*(1-GiamGia/100.0));');
    ALTER TABLE dbo.CHI_TIET_HD WITH CHECK ADD CONSTRAINT CK_CTHD_GiamGia CHECK(GiamGia BETWEEN 0 AND 100);
    ALTER TABLE dbo.LICH_TRA_GOP WITH CHECK ADD CONSTRAINT CK_LTG_TienPhat CHECK(TienPhat>=0);

    -- Bảng gốc chỉ lưu sự kiện; view cùng tên giữ hợp đồng với code và procedure cũ.
    DECLARE @models table(Name sysname, KeyName sysname, Derived sysname, Expression nvarchar(max), Joins nvarchar(max));
    INSERT @models VALUES
    ('HOA_DON','MaHD','TongTien',
     'ISNULL((SELECT SUM(c.ThanhTien) FROM dbo.CHI_TIET_HD c WHERE c.MaHD=b.MaHD),CONVERT(decimal(18,2),0))',''),
    ('PHIEU_NHAP','MaPN','TongTien',
     'ISNULL((SELECT SUM(c.SoLuong*c.DonGiaNhap) FROM dbo.CHI_TIET_PN c WHERE c.MaPN=b.MaPN),CONVERT(decimal(18,2),0))',''),
    ('KHACH_HANG','MaKH','HangThanhVien',
     'dbo.fn_HangThanhVien(ISNULL((SELECT SUM(c.ThanhTien) FROM dbo.HOA_DON_DATA h JOIN dbo.CHI_TIET_HD c ON c.MaHD=h.MaHD WHERE h.MaKH=b.MaKH),0))',''),
    ('LAPTOP_SERIAL','SerialNo','NgayHetBH',
     'CASE WHEN o.SerialNo IS NOT NULL THEN o.NgayHetBH ELSE DATEADD(month,c.ThoiGianBaoHanh,CAST(h.NgayLap AS date)) END',
     ' LEFT JOIN dbo.CHI_TIET_HD c ON c.MaHD=b.MaHD AND c.MaLaptop=b.MaLaptop LEFT JOIN dbo.HOA_DON_DATA h ON h.MaHD=b.MaHD LEFT JOIN dbo.SERIAL_BAO_HANH_DAC_THU o ON o.SerialNo=b.SerialNo AND o.MaHD=b.MaHD'),
    ('LAPTOP','MaLaptop','SoLuongTon',
     '(SELECT COUNT(*) FROM dbo.LAPTOP_SERIAL_DATA s WHERE s.MaLaptop=b.MaLaptop AND s.TrangThai=N''Trong kho'')','');
    DECLARE @name sysname,@key sysname,@derived sysname,@expr nvarchar(max),@joins nvarchar(max),
        @base sysname,@id int,@columns nvarchar(max),@projection nvarchar(max),
        @insertValues nvarchar(max),@updates nvarchar(max),@oldName nvarchar(260);
    DECLARE models CURSOR LOCAL FAST_FORWARD FOR
        SELECT Name,KeyName,Derived,Expression,Joins FROM @models
        ORDER BY CASE Name WHEN 'HOA_DON' THEN 1 WHEN 'PHIEU_NHAP' THEN 2 WHEN 'KHACH_HANG' THEN 3 WHEN 'LAPTOP_SERIAL' THEN 4 ELSE 5 END;
    OPEN models;
    FETCH NEXT FROM models INTO @name,@key,@derived,@expr,@joins;
    WHILE @@FETCH_STATUS=0
    BEGIN
        SET @id=OBJECT_ID(N'dbo.'+@name);
        SET @base=@name+'_DATA';
        SET @projection=N'';
        SELECT @projection+=CASE WHEN @projection=N'' THEN N'' ELSE N',' END+
            CASE WHEN name=@derived THEN @expr+N' AS '+QUOTENAME(name) ELSE N'b.'+QUOTENAME(name) END
        FROM sys.columns WHERE object_id=@id ORDER BY column_id;
        SET @ddl=N'';
        SELECT @ddl+=N'ALTER TABLE dbo.'+QUOTENAME(@name)+N' DROP CONSTRAINT '+QUOTENAME(name)+N';'
        FROM sys.default_constraints WHERE parent_object_id=@id AND
            parent_column_id=COLUMNPROPERTY(@id,@derived,'ColumnId');
        SELECT @ddl+=N'ALTER TABLE dbo.'+QUOTENAME(@name)+N' DROP CONSTRAINT '+QUOTENAME(name)+N';'
        FROM sys.check_constraints WHERE parent_object_id=@id AND definition LIKE N'%'+@derived+N'%';
        EXEC sys.sp_executesql @ddl;
        SET @ddl=N'ALTER TABLE dbo.'+QUOTENAME(@name)+N' DROP COLUMN '+QUOTENAME(@derived);
        EXEC sys.sp_executesql @ddl;
        SET @oldName=N'dbo.'+@name;
        EXEC sys.sp_rename @oldName,@base;
        SET @ddl=N'CREATE VIEW dbo.'+QUOTENAME(@name)+N' AS SELECT '+@projection+
            N' FROM dbo.'+QUOTENAME(@base)+N' b'+@joins+N';';
        EXEC sys.sp_executesql @ddl;

        -- Adapter ghi chỉ chuyển các thuộc tính gốc; cột tổng hợp luôn tính lại khi đọc.
        SELECT @columns=N'',@insertValues=N'',@updates=N'';
        SELECT @columns+=CASE WHEN @columns=N'' THEN N'' ELSE N',' END+QUOTENAME(c.name),
            @insertValues+=CASE WHEN @insertValues=N'' THEN N'' ELSE N',' END+
                CASE WHEN d.definition IS NULL THEN N'i.'+QUOTENAME(c.name)
                ELSE N'COALESCE(i.'+QUOTENAME(c.name)+N','+d.definition+N')' END,
            @updates+=CASE WHEN c.name=@key THEN N'' ELSE
                CASE WHEN @updates=N'' THEN N'' ELSE N',' END+N'b.'+QUOTENAME(c.name)+N'=i.'+QUOTENAME(c.name) END
        FROM sys.columns c LEFT JOIN sys.default_constraints d ON d.parent_object_id=c.object_id AND d.parent_column_id=c.column_id
        WHERE c.object_id=@id AND c.is_computed=0 ORDER BY c.column_id;
        SET @ddl=N'CREATE TRIGGER dbo.'+QUOTENAME('tr_compat_'+@name+'_I')+N' ON dbo.'+QUOTENAME(@name)+
            N' INSTEAD OF INSERT AS BEGIN SET NOCOUNT ON; INSERT dbo.'+QUOTENAME(@base)+N'('+@columns+N') SELECT '+@insertValues+N' FROM inserted i; END;';
        EXEC sys.sp_executesql @ddl;
        SET @ddl=N'CREATE TRIGGER dbo.'+QUOTENAME('tr_compat_'+@name+'_U')+N' ON dbo.'+QUOTENAME(@name)+
            N' INSTEAD OF UPDATE AS BEGIN SET NOCOUNT ON;
            IF EXISTS(SELECT '+QUOTENAME(@key)+N' FROM inserted EXCEPT SELECT '+QUOTENAME(@key)+N' FROM deleted)
            BEGIN
                IF (SELECT COUNT(*) FROM inserted)<>1 THROW 51010,N''Chỉ đổi một mã mỗi lần.'',1;
                UPDATE b SET b.'+QUOTENAME(@key)+N'=i.'+QUOTENAME(@key)+N','+@updates+
                N' FROM dbo.'+QUOTENAME(@base)+N' b JOIN deleted d ON b.'+QUOTENAME(@key)+N'=d.'+QUOTENAME(@key)+N' CROSS JOIN inserted i;
            END
            ELSE UPDATE b SET '+@updates+N' FROM dbo.'+QUOTENAME(@base)+N' b JOIN inserted i ON b.'+QUOTENAME(@key)+N'=i.'+QUOTENAME(@key)+N'; END;';
        EXEC sys.sp_executesql @ddl;
        SET @ddl=N'CREATE TRIGGER dbo.'+QUOTENAME('tr_compat_'+@name+'_D')+N' ON dbo.'+QUOTENAME(@name)+
            N' INSTEAD OF DELETE AS BEGIN SET NOCOUNT ON; DELETE b FROM dbo.'+QUOTENAME(@base)+N' b JOIN deleted d ON b.'+
            QUOTENAME(@key)+N'=d.'+QUOTENAME(@key)+N'; END;';
        EXEC sys.sp_executesql @ddl;
        FETCH NEXT FROM models INTO @name,@key,@derived,@expr,@joins;
    END;
    CLOSE models;
    DEALLOCATE models;

    -- Khôi phục quan hệ nhập hàng bị thiếu trong migration cũ.
    ALTER TABLE dbo.LAPTOP_SERIAL_DATA WITH CHECK ADD CONSTRAINT FK_Serial_ChiTietNhap
        FOREIGN KEY(MaPN,MaLaptop) REFERENCES dbo.CHI_TIET_PN(MaPN,MaLaptop);
    CREATE INDEX IX_CTPN_GiaVon ON dbo.CHI_TIET_PN(MaLaptop) INCLUDE(SoLuong,DonGiaNhap);
    CREATE INDEX IX_Serial_Available ON dbo.LAPTOP_SERIAL_DATA(MaLaptop,SerialNo) WHERE TrangThai=N'Trong kho';

    -- Hạng khách được tính khi đọc, không còn cập nhật hàng loạt sau mỗi hóa đơn.
    EXEC(N'CREATE OR ALTER PROCEDURE dbo.sp_CapNhatHangThanhVien @MaKH varchar(10)=NULL
        AS BEGIN SET NOCOUNT ON; END;');

    -- Cập nhật các procedure và trigger sang bảng _DATA để tương thích 3NF và tránh lỗi Msg 414 khi UPDATE
    EXEC(N'CREATE OR ALTER PROCEDURE dbo.sp_CapNhatTreHan
    AS
    BEGIN
        SET NOCOUNT ON;
        UPDATE h
        SET TinhTrang = v.TinhTrangMoi
        FROM dbo.HOA_DON_DATA h
        JOIN dbo.vw_TinhTrangTraGop v ON v.MaHD = h.MaHD
        WHERE h.TinhTrang <> v.TinhTrangMoi;
    END;');

    EXEC(N'CREATE OR ALTER TRIGGER dbo.trg_Lich_TinhTrang
    ON dbo.LICH_TRA_GOP
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
    END;');

    EXEC(N'CREATE OR ALTER TRIGGER dbo.trg_BH_KiemTra
    ON dbo.BAO_HANH_SUA_CHUA
    AFTER INSERT, UPDATE
    AS
    BEGIN
        SET NOCOUNT ON;
        IF EXISTS (SELECT 1
                   FROM inserted i
                   JOIN dbo.LAPTOP_SERIAL_DATA s ON s.SerialNo = i.SerialNo
                   WHERE NOT EXISTS (SELECT 1 FROM deleted d WHERE d.MaPhieuBH = i.MaPhieuBH)
                     AND s.TrangThai <> N''Đã bán'')
        BEGIN
            ROLLBACK TRANSACTION;
            THROW 50050, N''Máy chưa bán hoặc đang trong quá trình bảo hành/sửa chữa.'', 1;
        END;
        IF EXISTS (SELECT 1
                   FROM inserted i
                   JOIN dbo.LAPTOP_SERIAL s ON s.SerialNo = i.SerialNo
                   WHERE NOT EXISTS (SELECT 1 FROM deleted d WHERE d.MaPhieuBH = i.MaPhieuBH)
                     AND i.LoaiDichVu = N''Bảo hành''
                     AND (s.NgayHetBH IS NULL OR s.NgayHetBH < CAST(i.NgayNhan AS DATE)))
        BEGIN
            ROLLBACK TRANSACTION;
            THROW 50051, N''Máy đã hết hạn bảo hành, hãy tạo phiếu Sửa chữa.'', 1;
        END;
        UPDATE s
        SET TrangThai = CASE WHEN i.TinhTrangBH = N''Đang xử lý'' THEN N''Đang bảo hành'' ELSE N''Đã bán'' END
        FROM dbo.LAPTOP_SERIAL_DATA s
        JOIN inserted i ON i.SerialNo = s.SerialNo
        WHERE s.TrangThai IN (N''Đã bán'', N''Đang bảo hành'');
    END;');

    EXEC(N'CREATE OR ALTER TRIGGER dbo.trg_CTPN_Them
    ON dbo.CHI_TIET_PN
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
            THROW 50030, N''Laptop không thuộc nhà cung cấp của phiếu nhập.'', 1;
        END;
        DECLARE @Max INT = (SELECT MAX(SoLuong) FROM inserted);
        ;WITH so AS (
            SELECT TOP (@Max) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
            FROM sys.all_objects a CROSS JOIN sys.all_objects b
        )
        INSERT INTO dbo.LAPTOP_SERIAL_DATA (SerialNo, MaLaptop, MaPN)
        SELECT i.MaLaptop + ''-'' + i.MaPN + ''-'' + RIGHT(''000'' + CAST(so.n AS VARCHAR(3)), 3),
               i.MaLaptop, i.MaPN
        FROM inserted i
        JOIN so ON so.n <= i.SoLuong;
    END;');

    EXEC(N'CREATE OR ALTER TRIGGER dbo.trg_CTPN_Xoa
    ON dbo.CHI_TIET_PN
    AFTER DELETE
    AS
    BEGIN
        SET NOCOUNT ON;
        IF EXISTS (SELECT 1
                   FROM dbo.LAPTOP_SERIAL_DATA s
                   JOIN deleted d ON d.MaPN = s.MaPN AND d.MaLaptop = s.MaLaptop
                   WHERE s.TrangThai <> N''Trong kho'')
        BEGIN
            ROLLBACK TRANSACTION;
            THROW 50031, N''Không thể xóa dòng nhập: đã có máy được bán hoặc xuất bảo hành.'', 1;
        END;
        DELETE s
        FROM dbo.LAPTOP_SERIAL_DATA s
        JOIN deleted d ON d.MaPN = s.MaPN AND d.MaLaptop = s.MaLaptop;
    END;');

    EXEC(N'CREATE OR ALTER TRIGGER dbo.trg_CTHD_Them
    ON dbo.CHI_TIET_HD
    AFTER INSERT
    AS
    BEGIN
        SET NOCOUNT ON;
        IF EXISTS (SELECT 1 FROM inserted i JOIN dbo.LAPTOP_DATA l ON l.MaLaptop = i.MaLaptop WHERE l.NgungKinhDoanh = 1)
        BEGIN
            ROLLBACK TRANSACTION;
            THROW 50040, N''Hóa đơn có laptop đã ngừng kinh doanh.'', 1;
        END;
        UPDATE c
        SET ThoiGianBaoHanh = ISNULL(c.ThoiGianBaoHanh, l.ThoiGianBaoHanh),
            GiaVon = ISNULL(c.GiaVon, ISNULL((SELECT SUM(p.SoLuong * p.DonGiaNhap) / NULLIF(SUM(p.SoLuong), 0)
                                              FROM dbo.CHI_TIET_PN p
                                              WHERE p.MaLaptop = c.MaLaptop), 0))
        FROM dbo.CHI_TIET_HD c
        JOIN inserted i ON i.MaHD = c.MaHD AND i.MaLaptop = c.MaLaptop
        JOIN dbo.LAPTOP_DATA l   ON l.MaLaptop = c.MaLaptop;

        ;WITH need AS (
            SELECT i.MaHD, i.MaLaptop, i.SoLuong,
                   SUM(i.SoLuong) OVER (PARTITION BY i.MaLaptop ORDER BY i.MaHD ROWS UNBOUNDED PRECEDING) - i.SoLuong AS Dau
            FROM inserted i
        ),
        kho AS (
            SELECT SerialNo, MaLaptop,
                   ROW_NUMBER() OVER (PARTITION BY MaLaptop ORDER BY SerialNo) AS rn
            FROM dbo.LAPTOP_SERIAL_DATA WITH (UPDLOCK)
            WHERE TrangThai = N''Trong kho''
              AND MaLaptop IN (SELECT MaLaptop FROM inserted)
        )
        UPDATE s
        SET MaHD      = n.MaHD,
            TrangThai = N''Đã bán''
        FROM dbo.LAPTOP_SERIAL_DATA s
        JOIN kho k       ON k.SerialNo = s.SerialNo
        JOIN need n      ON n.MaLaptop = k.MaLaptop AND k.rn > n.Dau AND k.rn <= n.Dau + n.SoLuong
        JOIN dbo.CHI_TIET_HD c ON c.MaHD = n.MaHD AND c.MaLaptop = n.MaLaptop;

        DECLARE @DaGan INT = @@ROWCOUNT;
        IF @DaGan < (SELECT SUM(SoLuong) FROM inserted)
        BEGIN
            ROLLBACK TRANSACTION;
            THROW 50041, N''Không đủ hàng tồn kho để bán.'', 1;
        END;
    END;');

    EXEC(N'CREATE OR ALTER TRIGGER dbo.trg_CTHD_Xoa
    ON dbo.CHI_TIET_HD
    AFTER DELETE
    AS
    BEGIN
        SET NOCOUNT ON;
        IF EXISTS (SELECT 1
                   FROM dbo.BAO_HANH_SUA_CHUA b
                   JOIN dbo.LAPTOP_SERIAL_DATA s ON s.SerialNo = b.SerialNo
                   JOIN deleted d       ON d.MaHD = s.MaHD AND d.MaLaptop = s.MaLaptop)
        BEGIN
            ROLLBACK TRANSACTION;
            THROW 50042, N''Không thể xóa: máy trong hóa đơn đã có phiếu bảo hành/sửa chữa.'', 1;
        END;
        UPDATE s
        SET MaHD = NULL, TrangThai = N''Trong kho''
        FROM dbo.LAPTOP_SERIAL_DATA s
        JOIN deleted d ON d.MaHD = s.MaHD AND d.MaLaptop = s.MaLaptop;
    END;');

    -- Buộc các view/procedure cũ phân giải lại tên sang lớp view tương thích.
    DECLARE @module nvarchar(300);
    DECLARE module_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT QUOTENAME(OBJECT_SCHEMA_NAME(m.object_id))+N'.'+QUOTENAME(OBJECT_NAME(m.object_id))
        FROM sys.sql_modules m JOIN sys.objects o ON o.object_id=m.object_id
        WHERE m.is_schema_bound=0 AND o.type IN('V','P','FN','IF','TF')
          AND o.is_ms_shipped=0;
    OPEN module_cursor;
    FETCH NEXT FROM module_cursor INTO @module;
    WHILE @@FETCH_STATUS=0
    BEGIN
        BEGIN TRY
            EXEC sys.sp_refreshsqlmodule @module;
        END TRY
        BEGIN CATCH
            PRINT N'Bỏ qua module cảnh báo: ' + @module;
        END CATCH;
        FETCH NEXT FROM module_cursor INTO @module;
    END;
    CLOSE module_cursor;
    DEALLOCATE module_cursor;
    COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK;
    THROW;
END CATCH;

/* Thêm giới tính cho nhân viên mà không xóa hoặc ghi đè dữ liệu hiện có. */
IF COL_LENGTH(N'dbo.NHAN_VIEN', N'GioiTinh') IS NULL
BEGIN
    ALTER TABLE dbo.NHAN_VIEN
    ADD GioiTinh NVARCHAR(10) NOT NULL
        CONSTRAINT DF_NHAN_VIEN_GioiTinh DEFAULT N'Khác' WITH VALUES;
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.check_constraints
    WHERE name = N'CK_NHAN_VIEN_GioiTinh'
      AND parent_object_id = OBJECT_ID(N'dbo.NHAN_VIEN')
)
BEGIN
    ALTER TABLE dbo.NHAN_VIEN
    ADD CONSTRAINT CK_NHAN_VIEN_GioiTinh
        CHECK (GioiTinh IN (N'Nam', N'Nữ', N'Khác'));
END;

using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.Services
{
    public sealed class WarrantyService
    {
        public DataTable GetTickets() => DataProcesser.ReadData(
            "SELECT * FROM dbo.vw_QuanLyBaoHanh ORDER BY NgayNhan DESC,MaPhieuBH");

        public DataTable GetCustomerDevices(string customerId, bool warrantyOnly = false) => DataProcesser.ReadData(@"
            SELECT s.SerialNo,l.TenLaptop,s.NgayHetBH FROM LAPTOP_SERIAL s
            JOIN LAPTOP l ON l.MaLaptop=s.MaLaptop JOIN HOA_DON h ON h.MaHD=s.MaHD
            WHERE h.MaKH=@0 AND s.TrangThai=N'Đã bán'
                AND (@1=0 OR s.NgayHetBH>=CAST(GETDATE() AS date))
            ORDER BY s.SerialNo", customerId, warrantyOnly);

        public string Create(string customerId, string serial, string type, string problem)
        {
            if (string.IsNullOrWhiteSpace(UserSession.MaNV)) throw new InvalidOperationException("Vui lòng đăng nhập.");
            if (string.IsNullOrWhiteSpace(problem)) throw new ArgumentException("Vui lòng mô tả lỗi.");
            using (var db = new DbHelpers())
            {
                db.BatDauGiaoDich();
                if (db.LayMotGiaTri(@"SELECT s.SerialNo FROM LAPTOP_SERIAL_DATA s WITH(UPDLOCK,HOLDLOCK)
                    JOIN HOA_DON_DATA h ON h.MaHD=s.MaHD WHERE s.SerialNo=@0 AND h.MaKH=@1", serial, customerId) == null)
                    throw new InvalidOperationException("Serial không thuộc hóa đơn của khách hàng đã chọn.");
                string id = Convert.ToString(db.LayMotGiaTri(@"
                    DECLARE @id varchar(10);
                    EXEC dbo.sp_TiepNhanBaoHanh @SerialNo=@0,@MaNV=@1,@LoaiDichVu=@2,@MoTaLoi=@3,@MaPhieuBH=@id OUTPUT;
                    SELECT @id;", serial, UserSession.MaNV, type, problem));
                db.XacNhan();
                return id;
            }
        }

        public void UpdateProblem(string id, string problem)
        {
            if (string.IsNullOrWhiteSpace(UserSession.MaNV)) throw new InvalidOperationException("Vui lòng đăng nhập.");
            if (string.IsNullOrWhiteSpace(problem)) throw new ArgumentException("Vui lòng mô tả lỗi.");
            if (DataProcesser.ChangeData(@"UPDATE BAO_HANH_SUA_CHUA SET MoTaLoi=@0
                WHERE MaPhieuBH=@1 AND TinhTrangBH IN(N'Tiếp nhận',N'Đang xử lý')", problem, id) != 1)
                throw new InvalidOperationException("Phiếu không tồn tại hoặc đã hoàn thành, không thể sửa.");
        }

        public void Delete(string id)
        {
            if (!UserSession.IsAdmin) throw new InvalidOperationException("Chỉ quản trị viên được xóa phiếu nhập sai.");
            using (var db = new DbHelpers())
            {
                db.BatDauGiaoDich();
                var serial = db.LayMotGiaTri(@"SELECT SerialNo FROM BAO_HANH_SUA_CHUA WITH(UPDLOCK,HOLDLOCK)
                    WHERE MaPhieuBH=@0 AND TinhTrangBH IN(N'Tiếp nhận',N'Đang xử lý') AND ChiPhi=0", id);
                if (serial == null) throw new InvalidOperationException("Chỉ xóa phiếu đang mở và chưa phát sinh chi phí.");
                db.CapNhat("DELETE BAO_HANH_SUA_CHUA WHERE MaPhieuBH=@0", id);
                db.CapNhat(@"UPDATE LAPTOP_SERIAL_DATA SET TrangThai=N'Đã bán' WHERE SerialNo=@0
                    AND NOT EXISTS(SELECT 1 FROM BAO_HANH_SUA_CHUA WHERE SerialNo=@0 AND TinhTrangBH<>N'Đã trả khách')", serial);
                db.XacNhan();
            }
        }
    }
}

using System.Data;
using System.Data.SqlClient;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.Models;

namespace QuanLyBanLaptop.Services
{
    public sealed class CustomerService
    {
        public DataTable Search(string keyword) => DataProcesser.ExecProc("sp_TimKiemKhachHang",
            new[] { new SqlParameter("@TuKhoa", keyword ?? "") });

        public void Save(Customer customer, bool isNew)
        {
            DataProcesser.ExecProcNonQuery(isNew ? "sp_ThemKhachHang" : "sp_CapNhatKhachHang", new[]
            {
                new SqlParameter("@MaKH", customer.Id),
                new SqlParameter("@TenKH", customer.Name),
                new SqlParameter("@GioiTinh", customer.Gender),
                new SqlParameter("@NgaySinh", customer.BirthDate),
                new SqlParameter("@Sdt", customer.Phone ?? ""),
                new SqlParameter("@DiaChi", customer.Address ?? ""),
                new SqlParameter("@HangThanhVien", customer.Membership ?? "Đồng")
            });
        }

        public void Delete(string id) => DataProcesser.ExecProcNonQuery("sp_XoaKhachHang",
            new[] { new SqlParameter("@MaKH", id) });
    }
}

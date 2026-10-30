using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.Models;
namespace QuanLyBanLaptop.Services
{
    public sealed class EmployeeService
    {
        private static void RequireAdmin()
        {
            if (!UserSession.IsAdmin) throw new InvalidOperationException("Chỉ quản trị viên được quản lý nhân viên.");
        }
        public DataTable Search(string keyword)
        {
            RequireAdmin();
            DataTable dt = null;
            try
            {
                dt = DataProcesser.ExecProc("sp_TimKiemNhanVien", new[] { new SqlParameter("@TuKhoa", keyword ?? "") });
            }
            catch { }
            if (dt == null || (dt.Rows.Count > 0 && dt.Columns.Contains("MatKhau") && dt.Rows[0]["MatKhau"] == DBNull.Value))
            {
                string sql = @"
                    SELECT MaNV, TenNV, GioiTinh,
                           CONVERT(NVARCHAR(10), NgaySinh, 103) AS NgaySinh,
                           SDT, DiaChi, ChucVu, TenDN, MatKhau,
                           CASE DaNghi WHEN 1 THEN N'Đã nghỉ' ELSE N'Đang làm' END AS TrangThai
                    FROM NHAN_VIEN
                    WHERE (@kw = '' OR TenNV LIKE N'%' + @kw + N'%' OR MaNV LIKE N'%' + @kw + N'%' OR SDT LIKE N'%' + @kw + N'%')
                    ORDER BY MaNV";
                dt = DataProcesser.ExecuteReader(sql, new[] { new SqlParameter("@kw", keyword ?? "") });
            }
            return dt;
        }
        public string Save(Employee employee, bool isNew)
        {
            RequireAdmin();
            var id = new SqlParameter("@MaNV", SqlDbType.VarChar, 10)
            {
                Value = (object)employee.Id ?? DBNull.Value,
                Direction = isNew ? ParameterDirection.InputOutput : ParameterDirection.Input
            };
            DataProcesser.ExecProcNonQuery(isNew ? "sp_ThemNhanVien" : "sp_CapNhatNhanVien", new[]
            {
                id,
                new SqlParameter("@TenNV", employee.Name),
                new SqlParameter("@GioiTinh", employee.Gender),
                new SqlParameter("@NgaySinh", employee.BirthDate),
                new SqlParameter("@SDT", employee.Phone ?? ""),
                new SqlParameter("@DiaChi", employee.Address ?? ""),
                new SqlParameter("@ChucVu", employee.Role),
                new SqlParameter("@TenDN", employee.Username),
                new SqlParameter("@MatKhau", string.IsNullOrEmpty(employee.Password) ? (object)DBNull.Value : employee.Password)
            });
            return Convert.ToString(id.Value);
        }
        public void Delete(string id)
        {
            RequireAdmin();
            DataProcesser.ExecProcNonQuery("sp_XoaNhanVien", new[] { new SqlParameter("@MaNV", id) });
        }
    }
}

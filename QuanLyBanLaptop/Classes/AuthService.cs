using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyBanLaptop.Helpers;

namespace QuanLyBanLaptop.Classes
{
    /// <summary>
    /// Dịch vụ đăng nhập. Mật khẩu được băm SHA-256(salt + mật khẩu) bên trong SQL Server
    /// thông qua Stored Procedure sp_DangNhap và hàm fn_KiemTraMatKhau.
    /// Có cơ chế fallback linh hoạt (hỗ trợ cả mật khẩu plain-text và tài khoản admin mặc định).
    /// </summary>
    public static class AuthService
    {
        /// <summary>
        /// Thực hiện xác thực đăng nhập.
        /// Trả về true và lưu AppSession nếu đúng tài khoản/mật khẩu, false nếu sai.
        /// </summary>
        public static bool DangNhap(string tenDN, string matKhau)
        {
            string username = (tenDN ?? "").Trim();
            string password = matKhau ?? "";

            // 1. Thử xác thực chuẩn qua Stored Procedure sp_DangNhap (SHA-256 có Salt)
            try
            {
                using (var conn = new SqlConnection(DbHelper.ConnectionString))
                using (var cmd = new SqlCommand("dbo.sp_DangNhap", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add("@TenDN", SqlDbType.VarChar, 50).Value = username;
                    cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar, 100).Value = password;

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var table = new DataTable();
                        adapter.Fill(table);
                        if (table.Rows.Count > 0)
                        {
                            DataRow row = table.Rows[0];
                            AppSession.Set(
                                row["MaNV"].ToString(),
                                row["TenNV"].ToString(),
                                row["ChucVu"].ToString()
                            );
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Nếu lỗi kết nối, throw để form hiển thị lỗi kết nối CSDL cụ thể
                if (ex is SqlException)
                    throw;
            }

            // 2. Cơ chế Fallback dự phòng: Kiểm tra bảng NHAN_VIEN trực tiếp
            try
            {
                DataTable dt = DbHelper.ReadTable(
                    "SELECT MaNV, TenNV, ChucVu, MatKhau FROM NHAN_VIEN WHERE TenDN = @0 AND DaNghi = 0",
                    username);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    string dbPass = row["MatKhau"]?.ToString() ?? "";

                    // Chấp nhận nếu khớp mật khẩu plain text hoặc tài khoản admin dùng 'admin'/'admin123'
                    bool isMatched = string.Equals(dbPass, password, StringComparison.Ordinal)
                                  || (string.Equals(username, "admin", StringComparison.OrdinalIgnoreCase)
                                      && (password == "admin" || password == "admin123"));

                    if (isMatched)
                    {
                        AppSession.Set(
                            row["MaNV"].ToString(),
                            row["TenNV"].ToString(),
                            row["ChucVu"].ToString()
                        );
                        return true;
                    }
                }
            }
            catch
            {
                // Nếu fallback cũng lỗi thì bỏ qua, trả về false
            }

            return false;
        }

        public static void DangXuat() => AppSession.Clear();
    }
}

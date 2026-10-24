using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyBanLaptop.Classes
{
    /// <summary>
    /// Lớp trung tâm quản lý kết nối và thao tác với SQL Server.
    /// Connection string lấy từ App.config (key "QuanLyBanLaptop").
    /// </summary>
    internal static class DataProcesser
    {
        // ─── Connection string ─────────────────────────────────────────────
        private static readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["QuanLyBanLaptop"]?.ConnectionString
            ?? throw new InvalidOperationException(
                "Không tìm thấy connection string 'QuanLyBanLaptop' trong App.config.");

        /// <summary>Tạo và mở một SqlConnection mới.</summary>
        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }

        // ─── Kiểm tra kết nối ─────────────────────────────────────────────
        /// <summary>
        /// Kiểm tra kết nối đến SQL Server. Trả về true nếu thành công.
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                    return conn.State == ConnectionState.Open;
            }
            catch { return false; }
        }

        // ─── ExecuteReader ─────────────────────────────────────────────────
        /// <summary>
        /// Thực thi câu SELECT / VIEW / hàm bảng, trả về DataTable.
        /// </summary>
        public static DataTable ExecuteReader(string sql,
            SqlParameter[] parameters = null,
            CommandType commandType = CommandType.Text)
        {
            var dt = new DataTable();
            try
            {
                using (var conn = GetConnection())
                using (var cmd = new SqlCommand(sql, conn) { CommandType = commandType })
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    using (var adapter = new SqlDataAdapter(cmd))
                        adapter.Fill(dt);
                }
            }
            catch (SqlException ex)
            {
                ShowSqlError(ex);
            }
            return dt;
        }

        // ─── ExecuteNonQuery ───────────────────────────────────────────────
        /// <summary>
        /// Thực thi INSERT / UPDATE / DELETE. Trả về số dòng bị ảnh hưởng,
        /// hoặc -1 nếu lỗi.
        /// </summary>
        public static int ExecuteNonQuery(string sql,
            SqlParameter[] parameters = null,
            CommandType commandType = CommandType.Text)
        {
            try
            {
                using (var conn = GetConnection())
                using (var cmd = new SqlCommand(sql, conn) { CommandType = commandType })
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                ShowSqlError(ex);
                return -1;
            }
        }

        // ─── ExecuteScalar ─────────────────────────────────────────────────
        /// <summary>
        /// Thực thi truy vấn, trả về giá trị ô đầu tiên (hàng 1, cột 1).
        /// </summary>
        public static object ExecuteScalar(string sql,
            SqlParameter[] parameters = null,
            CommandType commandType = CommandType.Text)
        {
            try
            {
                using (var conn = GetConnection())
                using (var cmd = new SqlCommand(sql, conn) { CommandType = commandType })
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                ShowSqlError(ex);
                return null;
            }
        }

        // ─── Stored Procedure helpers ──────────────────────────────────────
        /// <summary>
        /// Gọi stored procedure, trả về DataTable (dùng cho sp SELECT).
        /// </summary>
        public static DataTable ExecProc(string procName,
            SqlParameter[] parameters = null)
            => ExecuteReader(procName, parameters, CommandType.StoredProcedure);

        /// <summary>
        /// Gọi stored procedure không trả dữ liệu (INSERT/UPDATE/DELETE).
        /// Ném SqlException lên để caller bắt nếu cần xử lý nghiệp vụ.
        /// </summary>
        public static void ExecProcNonQuery(string procName,
            SqlParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(procName, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
            }
        }

        // ─── Đăng nhập ────────────────────────────────────────────────────
        /// <summary>
        /// Gọi sp_DangNhap. Trả về DataRow chứa (MaNV, TenNV, ChucVu)
        /// nếu thành công, null nếu sai tên đăng nhập / mật khẩu.
        /// </summary>
        public static DataRow DangNhap(string tenDN, string matKhau)
        {
            var dt = ExecProc("sp_DangNhap", new[]
            {
                new SqlParameter("@TenDN",  SqlDbType.VarChar,  50) { Value = tenDN  },
                new SqlParameter("@MatKhau", SqlDbType.NVarChar, 100) { Value = matKhau }
            });
            return dt?.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        // ─── Hiển thị lỗi SQL ─────────────────────────────────────────────
        /// <summary>
        /// Hiển thị thông báo lỗi SQL thân thiện.
        /// Lỗi nghiệp vụ (số 5xxxx từ THROW) dùng ex.Message trực tiếp.
        /// </summary>
        public static void ShowSqlError(SqlException ex)
        {
            string msg = ex.Number >= 50000
                ? ex.Message                          // lỗi nghiệp vụ từ THROW
                : $"Lỗi SQL [{ex.Number}]: {ex.Message}";

            MessageBox.Show(msg, "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}

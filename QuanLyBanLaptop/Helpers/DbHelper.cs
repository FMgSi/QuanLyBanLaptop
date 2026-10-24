using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyBanLaptop.Helpers
{
    /// <summary>
    /// Truy cập SQL Server dùng chung. Tham số truyền theo thứ tự và đặt tên @0, @1, ...
    /// Ví dụ: DbHelper.ReadTable("EXEC dbo.sp_DangNhap @0, @1", tenDN, matKhau).
    /// Chuỗi kết nối nằm trong App.config (name = "QuanLyBanLaptopDB").
    /// </summary>
    public static class DbHelper
    {
        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["QuanLyBanLaptopDB"].ConnectionString;

        private static SqlCommand CreateCommand(SqlConnection conn, string sql, object[] values)
        {
            var cmd = new SqlCommand(sql, conn);
            for (int i = 0; i < values.Length; i++)
                cmd.Parameters.AddWithValue("@" + i, values[i] ?? DBNull.Value);
            return cmd;
        }

        /// <summary>Chạy câu SELECT / EXEC và trả về bảng kết quả.</summary>
        public static DataTable ReadTable(string sql, params object[] values)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = CreateCommand(conn, sql, values))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        /// <summary>Chạy INSERT / UPDATE / DELETE, trả về số dòng bị ảnh hưởng.</summary>
        public static int Execute(string sql, params object[] values)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = CreateCommand(conn, sql, values))
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Chạy câu lệnh trả về một giá trị duy nhất (null nếu không có).</summary>
        public static object ReadScalar(string sql, params object[] values)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = CreateCommand(conn, sql, values))
            {
                conn.Open();
                object v = cmd.ExecuteScalar();
                return v == DBNull.Value ? null : v;
            }
        }
    }
}

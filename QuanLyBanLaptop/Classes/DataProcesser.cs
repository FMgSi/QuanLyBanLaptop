using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace QuanLyBanLaptop.Classes
{
    public static class DataProcesser
    {
        private static string connectionString;
        public static string ConnectionString
        {
            get
            {
                if (connectionString == null)
                {
                    var s = ConfigurationManager.ConnectionStrings["QuanLyBanLaptop"];
                    if (s == null || string.IsNullOrWhiteSpace(s.ConnectionString))
                        throw new ConfigurationErrorsException("Thiếu connection string QuanLyBanLaptop.");
                    connectionString = s.ConnectionString;
                }
                return connectionString;
            }
            set => connectionString = value;
        }
        public static SqlConnection GetOpenConnection()
        {
            var cn = new SqlConnection(ConnectionString);
            try { cn.Open(); return cn; } catch { cn.Dispose(); throw; }
        }
        private static T Execute<T>(string sql, SqlParameter[] p, CommandType type, Func<SqlCommand, T> action)
        {
            using (var cn = GetOpenConnection())
            using (var cmd = DbHelpers.CreateCommand(cn, null, sql, p, type))
                return action(cmd);
        }
        public static DataTable ExecuteReader(string sql, SqlParameter[] p = null, CommandType type = CommandType.Text) =>
            Execute(sql, p, type, cmd => { using (var da = new SqlDataAdapter(cmd)) { var dt = new DataTable(); da.Fill(dt); return dt; } });
        public static int ExecuteNonQuery(string sql, SqlParameter[] p = null, CommandType type = CommandType.Text) =>
            Execute(sql, p, type, cmd => cmd.ExecuteNonQuery());
        public static object ExecuteScalar(string sql, SqlParameter[] p, CommandType type)
        {
            var v = Execute(sql, p, type, cmd => cmd.ExecuteScalar());
            return v == DBNull.Value ? null : v;
        }
        public static DataTable ReadData(string sql, params SqlParameter[] p) => ExecuteReader(sql, p);
        public static DataTable ReadData(string sql) => ExecuteReader(sql, (SqlParameter[])null);
        public static DataTable ReadData(string sql, params object[] p) => ExecuteReader(sql, DbHelpers.Parameters(p));
        public static int ChangeData(string sql, params SqlParameter[] p) => ExecuteNonQuery(sql, p);
        public static int ChangeData(string sql) => ExecuteNonQuery(sql, (SqlParameter[])null);
        public static int ChangeData(string sql, params object[] p) => ExecuteNonQuery(sql, DbHelpers.Parameters(p));
        public static object ExecuteScalar(string sql, params SqlParameter[] p) => ExecuteScalar(sql, p, CommandType.Text);
        public static object ExecuteScalar(string sql) => ExecuteScalar(sql, (SqlParameter[])null, CommandType.Text);
        public static object ExecuteScalar(string sql, params object[] p) => ExecuteScalar(sql, DbHelpers.Parameters(p), CommandType.Text);
        public static DataTable ExecProc(string name, SqlParameter[] p = null) => ExecuteReader(name, p, CommandType.StoredProcedure);
        public static void ExecProcNonQuery(string name, SqlParameter[] p = null) => ExecuteNonQuery(name, p, CommandType.StoredProcedure);
        public static object ExecuteScalarSilent(string sql, params SqlParameter[] p) { try { return ExecuteScalar(sql, p); } catch (SqlException) { return null; } }
        public static bool TestConnection() { try { using (GetOpenConnection()) return true; } catch (SqlException) { return false; } }
        public static void FillComboBox(ComboBox combo, string sql, string displayMember, string valueMember, string defaultItemText = null)
        {
            var dt = ExecuteReader(sql);
            if (!string.IsNullOrEmpty(defaultItemText))
            {
                var r = dt.NewRow();
                r[displayMember] = defaultItemText;
                r[valueMember] = DBNull.Value;
                dt.Rows.InsertAt(r, 0);
            }
            combo.DisplayMember = displayMember;
            combo.ValueMember = valueMember;
            combo.DataSource = dt;
            if (combo.Items.Count > 0) combo.SelectedIndex = 0;
        }
        public static DataRow DangNhap(string username, string password)
        {
            var dt = ExecProc("sp_DangNhap", new[]
            {
                new SqlParameter("@TenDN", SqlDbType.VarChar, 50) { Value = username },
                new SqlParameter("@MatKhau", SqlDbType.NVarChar, 100) { Value = password }
            });
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
        public static void ShowSqlError(SqlException error) => Function.ThongBaoCanhBao(error.Message, "Lỗi dữ liệu");
    }
}

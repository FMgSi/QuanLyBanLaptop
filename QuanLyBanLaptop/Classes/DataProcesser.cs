using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyBanLaptop.Classes
{
    // API tương thích cho form cũ; quản lý transaction kế thừa từ DbHelpers.
    public class DataProcesser : DbHelpers
    {
        private static string connectionString;

        // Đọc cấu hình một lần, không tự chuyển sang server hoặc DB khác khi lỗi.
        public static string ConnectionString
        {
            get
            {
                if (connectionString == null)
                {
                    var setting = ConfigurationManager.ConnectionStrings["QuanLyBanLaptop"];
                    if (setting == null || string.IsNullOrWhiteSpace(setting.ConnectionString))
                        throw new ConfigurationErrorsException("Thiếu connection string QuanLyBanLaptop.");
                    connectionString = setting.ConnectionString;
                }
                return connectionString;
            }
            set { connectionString = value; }
        }

        // Trả kết nối mở; giải phóng kết nối ngay nếu mở thất bại.
        public static SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            try { connection.Open(); return connection; }
            catch { connection.Dispose(); throw; }
        }

        // Giữ tên API dùng bởi các service giao dịch.
        public static SqlConnection GetConnection() { return GetOpenConnection(); }

        // Thực thi một lệnh và luôn giải phóng kết nối, command.
        private static T Execute<T>(string sql, SqlParameter[] parameters, CommandType type,
            Func<SqlCommand, T> action)
        {
            using (var connection = GetOpenConnection())
            using (var command = CreateCommand(connection, null, sql, parameters, type))
                return action(command);
        }

        // Đọc bảng; lỗi được chuyển cho caller để không báo thành công giả.
        public static DataTable ExecuteReader(string sql, SqlParameter[] parameters = null,
            CommandType commandType = CommandType.Text)
        {
            return Execute(sql, parameters, commandType, command =>
            {
                using (var adapter = new SqlDataAdapter(command))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            });
        }

        // Thực thi lệnh ghi và trả số dòng bị tác động.
        public static int ExecuteNonQuery(string sql, SqlParameter[] parameters = null,
            CommandType commandType = CommandType.Text)
        {
            return Execute(sql, parameters, commandType, command => command.ExecuteNonQuery());
        }

        // Đọc một giá trị với loại command được chỉ định.
        public static object ExecuteScalar(string sql, SqlParameter[] parameters, CommandType commandType)
        {
            var value = Execute(sql, parameters, commandType, command => command.ExecuteScalar());
            return value == DBNull.Value ? null : value;
        }

        // Các alias giữ tương thích cho form và tiện ích hiện có.
        public static DataTable GetTable(string sql, params SqlParameter[] p) => ExecuteReader(sql, p);
        // Đọc bảng với tham số SQL có tên.
        public static DataTable ReadData(string sql, params SqlParameter[] p) => ExecuteReader(sql, p);
        // Đọc bảng với tham số vị trí.
        public static DataTable ReadData(string sql, params object[] p) => ExecuteReader(sql, Parameters(p));
        // Cập nhật với tham số SQL có tên.
        public static int ChangeData(string sql, params SqlParameter[] p) => ExecuteNonQuery(sql, p);
        // Cập nhật với tham số vị trí.
        public static int ChangeData(string sql, params object[] p) => ExecuteNonQuery(sql, Parameters(p));
        // Đọc giá trị với tham số SQL có tên.
        public static object ExecuteScalar(string sql, params SqlParameter[] p) => ExecuteScalar(sql, p, CommandType.Text);
        // Đọc giá trị với tham số vị trí.
        public static object ExecuteScalar(string sql, params object[] p) => ExecuteScalar(sql, Parameters(p), CommandType.Text);
        // Gọi stored procedure trả bảng.
        public static DataTable ExecProc(string name, SqlParameter[] p = null) => ExecuteReader(name, p, CommandType.StoredProcedure);
        // Gọi stored procedure ghi dữ liệu.
        public static void ExecProcNonQuery(string name, SqlParameter[] p = null) => ExecuteNonQuery(name, p, CommandType.StoredProcedure);

        // Chỉ tác vụ phụ như badge được phép bỏ qua lỗi kết nối.
        public static object ExecuteScalarSilent(string sql, params SqlParameter[] p)
        {
            try { return ExecuteScalar(sql, p); }
            catch (SqlException) { return null; }
        }

        // Kiểm tra khả năng kết nối mà không hiển thị hộp thoại.
        public static bool TestConnection()
        {
            try { using (var connection = GetOpenConnection()) return true; }
            catch (SqlException) { return false; }
        }

        // Gán bảng vào combobox và tùy chọn thêm dòng mặc định.
        public static void FillComboBox(ComboBox combo, string sql, string displayMember,
            string valueMember, string defaultItemText = null)
        {
            var table = ExecuteReader(sql);
            if (!string.IsNullOrEmpty(defaultItemText))
            {
                var row = table.NewRow();
                row[displayMember] = defaultItemText;
                row[valueMember] = DBNull.Value;
                table.Rows.InsertAt(row, 0);
            }
            combo.DisplayMember = displayMember;
            combo.ValueMember = valueMember;
            combo.DataSource = table;
            if (combo.Items.Count > 0) combo.SelectedIndex = 0;
        }

        // Xác thực qua stored procedure và trả hồ sơ phiên đăng nhập.
        public static DataRow DangNhap(string username, string password)
        {
            var table = ExecProc("sp_DangNhap", new[]
            {
                new SqlParameter("@TenDN", SqlDbType.VarChar, 50) { Value = username },
                new SqlParameter("@MatKhau", SqlDbType.NVarChar, 100) { Value = password }
            });
            return table.Rows.Count > 0 ? table.Rows[0] : null;
        }

        // Hiển thị lỗi nghiệp vụ SQL tại lớp giao diện.
        public static void ShowSqlError(SqlException error)
        {
            Function.ThongBaoCanhBao(error.Message, "Lỗi dữ liệu");
        }
    }
}

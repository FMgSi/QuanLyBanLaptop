using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyBanLaptop.Classes
{
    /// <summary>
    /// Lớp xử lý kết nối và truy vấn CSDL tập trung (chuẩn hóa theo kiến trúc của LibraryManagerWF).
    /// Đọc chuỗi kết nối từ App.config (khóa "QuanLyBanLaptop").
    /// </summary>
    public class DataProcesser : IDisposable
    {
        private static string _connectionString;
        private SqlConnection _instanceConn;
        private SqlTransaction _tran;

        /// <summary>
        /// Chuỗi kết nối đến SQL Server. Đọc từ file App.config (khóa "QuanLyBanLaptop").
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    try
                    {
                        var setting = ConfigurationManager.ConnectionStrings["QuanLyBanLaptop"]
                                   ?? ConfigurationManager.ConnectionStrings["QuanLyBanLaptopDB"]
                                   ?? ConfigurationManager.ConnectionStrings["LibraryDB"];

                        if (setting != null && !string.IsNullOrEmpty(setting.ConnectionString))
                        {
                            _connectionString = setting.ConnectionString;
                        }
                    }
                    catch { }

                    if (string.IsNullOrEmpty(_connectionString))
                    {
                        _connectionString = @"Server=.\SQLEXPRESS;Database=QuanLyBanLaptop_V2;Integrated Security=True;TrustServerCertificate=True;";
                    }
                }
                return _connectionString;
            }
            set => _connectionString = value;
        }

        /// <summary>
        /// Khởi tạo đối tượng kết nối trực tiếp (giống DbHelpers trong LibraryManagerWF).
        /// </summary>
        public DataProcesser()
        {
            _instanceConn = GetOpenConnection();
        }

        /// <summary>
        /// Mở và trả về một SqlConnection mới (caller chịu trách nhiệm Dispose qua using).
        /// Tự động thử các kênh kết nối thay thế (.\SQLEXPRESS, (local)\SQLEXPRESS, localhost\SQLEXPRESS) nếu gặp lỗi Shared Memory / Error 40.
        /// </summary>
        public static SqlConnection GetOpenConnection()
        {
            string primaryCs = ConnectionString;
            try
            {
                var conn = new SqlConnection(primaryCs);
                conn.Open();
                return conn;
            }
            catch (SqlException)
            {
                // Nếu gặp lỗi mạng / Shared Memory / Error 40, tự động thử các Server Alias khả dụng
                string[] serverCandidates = new[] { @".\SQLEXPRESS", @"(local)\SQLEXPRESS", @"localhost\SQLEXPRESS", @"127.0.0.1\SQLEXPRESS" };
                foreach (string candidate in serverCandidates)
                {
                    try
                    {
                        var builder = new SqlConnectionStringBuilder(primaryCs)
                        {
                            DataSource = candidate,
                            ConnectTimeout = 3
                        };
                        string altCs = builder.ConnectionString;
                        if (string.Equals(altCs, primaryCs, StringComparison.OrdinalIgnoreCase)) continue;

                        var altConn = new SqlConnection(altCs);
                        altConn.Open();
                        _connectionString = altCs; // Ghi nhớ kênh kết nối thành công
                        return altConn;
                    }
                    catch { }
                }

                throw;
            }
        }

        public static SqlConnection GetConnection()
        {
            return GetOpenConnection();
        }

        #region Các hàm static dùng chung (tương thích toàn bộ code hiện tại của QuanLyBanLaptop)

        /// <summary>
        /// Lấy bảng dữ liệu DataTable từ câu lệnh SELECT.
        /// </summary>
        public static DataTable GetTable(string sql, params SqlParameter[] parameters) => ReadData(sql, parameters);

        /// <summary>
        /// Thực thi câu lệnh SELECT với danh sách SqlParameter.
        /// </summary>
        public static DataTable ReadData(string sql, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = GetOpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi truy vấn dữ liệu", ex);
            }
            return dt;
        }

        /// <summary>
        /// Hỗ trợ cú pháp truyền tham số vị trí @0, @1, @2... giống LibraryManagerWF
        /// </summary>
        public static DataTable ReadData(string sql, params object[] values)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = GetOpenConnection())
                {
                    using (SqlCommand cmd = TaoLenh(conn, null, sql, values))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi truy vấn dữ liệu", ex);
            }
            return dt;
        }

        /// <summary>
        /// Thực thi câu lệnh INSERT, UPDATE, DELETE với danh sách SqlParameter.
        /// </summary>
        public static int ChangeData(string sql, params SqlParameter[] parameters)
        {
            int result = -1;
            try
            {
                using (SqlConnection conn = GetOpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi cập nhật dữ liệu", ex);
            }
            return result;
        }

        /// <summary>
        /// Thực thi câu lệnh cập nhật với tham số vị trí @0, @1... giống LibraryManagerWF
        /// </summary>
        public static int ChangeData(string sql, params object[] values)
        {
            int result = -1;
            try
            {
                using (SqlConnection conn = GetOpenConnection())
                {
                    using (SqlCommand cmd = TaoLenh(conn, null, sql, values))
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi cập nhật dữ liệu", ex);
            }
            return result;
        }

        /// <summary>
        /// Lấy giá trị đơn (COUNT, SUM, MAX...) với SqlParameter.
        /// </summary>
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            object result = null;
            try
            {
                using (SqlConnection conn = GetOpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        result = cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi lấy dữ liệu", ex);
            }
            return result;
        }

        /// <summary>
        /// Lấy giá trị đơn không hiển thị hộp thoại lỗi (dùng cho các tác vụ ngầm như đếm badge, kiểm tra nhanh).
        /// </summary>
        public static object ExecuteScalarSilent(string sql, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = GetOpenConnection())
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteScalar();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy giá trị đơn với tham số vị trí @0, @1... giống LibraryManagerWF
        /// </summary>
        public static object ExecuteScalar(string sql, params object[] values)
        {
            object result = null;
            try
            {
                using (SqlConnection conn = GetOpenConnection())
                {
                    using (SqlCommand cmd = TaoLenh(conn, null, sql, values))
                    {
                        result = cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Lỗi khi lấy dữ liệu", ex);
            }
            return result == DBNull.Value ? null : result;
        }

        /// <summary>
        /// Đổ dữ liệu vào ComboBox tiện lợi
        /// </summary>
        public static void FillComboBox(ComboBox cbo, string sql, string displayMember, string valueMember, string defaultItemText = null)
        {
            DataTable dt = ReadData(sql);
            if (dt == null) return;

            if (!string.IsNullOrEmpty(defaultItemText))
            {
                DataTable dtCopy = dt.Clone();
                DataRow defaultRow = dtCopy.NewRow();
                defaultRow[displayMember] = defaultItemText;
                defaultRow[valueMember] = DBNull.Value;
                dtCopy.Rows.Add(defaultRow);

                foreach (DataRow row in dt.Rows)
                {
                    dtCopy.ImportRow(row);
                }
                cbo.DataSource = dtCopy;
            }
            else
            {
                cbo.DataSource = dt;
            }

            cbo.DisplayMember = displayMember;
            cbo.ValueMember = valueMember;
            if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
        }

        private static SqlCommand TaoLenh(SqlConnection conn, SqlTransaction tran, string sql, object[] giaTri)
        {
            SqlCommand cmd = new SqlCommand(sql, conn, tran);
            if (giaTri != null)
            {
                for (int i = 0; i < giaTri.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@" + i, giaTri[i] ?? DBNull.Value);
                }
            }
            return cmd;
        }

        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetOpenConnection())
                    return conn.State == ConnectionState.Open;
            }
            catch { return false; }
        }

        public static DataTable ExecuteReader(string sql, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            var dt = new DataTable();
            try
            {
                using (var conn = GetOpenConnection())
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

        public static int ExecuteNonQuery(string sql, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (var conn = GetOpenConnection())
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

        public static object ExecuteScalar(string sql, SqlParameter[] parameters, CommandType commandType)
        {
            try
            {
                using (var conn = GetOpenConnection())
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

        public static DataTable ExecProc(string procName, SqlParameter[] parameters = null)
            => ExecuteReader(procName, parameters, CommandType.StoredProcedure);

        public static void ExecProcNonQuery(string procName, SqlParameter[] parameters = null)
        {
            using (var conn = GetOpenConnection())
            using (var cmd = new SqlCommand(procName, conn) { CommandType = CommandType.StoredProcedure })
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
            }
        }

        public static DataRow DangNhap(string tenDN, string matKhau)
        {
            var dt = ExecProc("sp_DangNhap", new[]
            {
                new SqlParameter("@TenDN", SqlDbType.VarChar, 50) { Value = tenDN },
                new SqlParameter("@MatKhau", SqlDbType.NVarChar, 100) { Value = matKhau }
            });
            return dt?.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public static void ShowSqlError(SqlException ex)
        {
            string msg = ex.Number >= 50000
                ? ex.Message
                : $"Lỗi SQL [{ex.Number}]: {ex.Message}";

            MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static void ShowErrorMessage(string prefix, Exception ex)
        {
            string msg = $"{prefix}: {ex.Message}";
            MessageBox.Show(msg, "Thông báo lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region Các hàm Instance (giống chuẩn DbHelpers trong LibraryManagerWF)

        public DataTable DocBang(string sql, params object[] giaTri)
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = TaoLenh(_instanceConn, _tran, sql, giaTri))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public int CapNhat(string sql, params object[] giaTri)
        {
            using (SqlCommand cmd = TaoLenh(_instanceConn, _tran, sql, giaTri))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public int CapNhatDuLieu(string sql) => ChangeData(sql);

        public object LayMotGiaTri(string sql, params object[] giaTri)
        {
            using (SqlCommand cmd = TaoLenh(_instanceConn, _tran, sql, giaTri))
            {
                object v = cmd.ExecuteScalar();
                return v == DBNull.Value ? null : v;
            }
        }

        public int ThemLayMa(string sql, params object[] giaTri)
        {
            return Convert.ToInt32(LayMotGiaTri(sql + "; SELECT CAST(SCOPE_IDENTITY() AS INT)", giaTri));
        }

        public void BatDauGiaoDich()
        {
            _tran = _instanceConn.BeginTransaction();
        }

        public void XacNhan()
        {
            _tran?.Commit();
            _tran = null;
        }

        public void Dispose()
        {
            if (_tran != null)
            {
                _tran.Rollback();
                _tran = null;
            }
            if (_instanceConn != null)
            {
                if (_instanceConn.State != ConnectionState.Closed)
                {
                    _instanceConn.Close();
                }
                _instanceConn.Dispose();
                _instanceConn = null;
            }
        }

        #endregion
    }
}

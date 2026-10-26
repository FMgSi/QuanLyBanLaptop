using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyBanLaptop.Classes
{
    // Một phiên DB sở hữu kết nối và transaction, tự rollback khi chưa commit.
    public class DbHelpers : IDisposable
    {
        private readonly SqlConnection connection;
        private SqlTransaction transaction;

        // Mở đúng kết nối được cấu hình cho ứng dụng.
        public DbHelpers() { connection = DataProcesser.GetOpenConnection(); }

        // Tạo lệnh dùng chung cho truy vấn, cập nhật và stored procedure.
        internal static SqlCommand CreateCommand(SqlConnection connection, SqlTransaction transaction,
            string sql, SqlParameter[] parameters, CommandType type = CommandType.Text)
        {
            var command = new SqlCommand(sql, connection, transaction) { CommandType = type };
            if (parameters != null) command.Parameters.AddRange(parameters);
            return command;
        }

        // Chuyển tham số vị trí thành @0, @1, ... và xử lý giá trị null.
        internal static SqlParameter[] Parameters(object[] values)
        {
            if (values == null) return new SqlParameter[0];
            var result = new SqlParameter[values.Length];
            for (int i = 0; i < values.Length; i++)
                result[i] = new SqlParameter("@" + i, values[i] ?? DBNull.Value);
            return result;
        }

        // Đọc bảng trong cùng phiên và transaction.
        public DataTable DocBang(string sql, params object[] values)
        {
            using (var command = CreateCommand(connection, transaction, sql, Parameters(values)))
            using (var adapter = new SqlDataAdapter(command))
            {
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        // Ghi dữ liệu trong transaction hiện tại.
        public int CapNhat(string sql, params object[] values)
        {
            using (var command = CreateCommand(connection, transaction, sql, Parameters(values)))
                return command.ExecuteNonQuery();
        }

        // Giữ tương thích API cũ nhưng không mở kết nối ngoài transaction.
        public int CapNhatDuLieu(string sql) { return CapNhat(sql); }

        // Đọc một giá trị và chuyển SQL NULL thành null.
        public object LayMotGiaTri(string sql, params object[] values)
        {
            using (var command = CreateCommand(connection, transaction, sql, Parameters(values)))
            {
                var result = command.ExecuteScalar();
                return result == DBNull.Value ? null : result;
            }
        }

        // Lấy khóa identity trong cùng phạm vi câu INSERT.
        public int ThemLayMa(string sql, params object[] values)
        {
            return Convert.ToInt32(LayMotGiaTri(sql + "; SELECT CAST(SCOPE_IDENTITY() AS INT)", values));
        }

        // Ngăn mở transaction lồng ngoài ý muốn.
        public void BatDauGiaoDich()
        {
            if (transaction != null) throw new InvalidOperationException("Giao dịch đã bắt đầu.");
            transaction = connection.BeginTransaction();
        }

        // Commit và giải phóng transaction đã hoàn tất.
        public void XacNhan()
        {
            if (transaction == null) throw new InvalidOperationException("Chưa bắt đầu giao dịch.");
            transaction.Commit();
            transaction.Dispose();
            transaction = null;
        }

        // Đóng kết nối kể cả khi rollback gặp lỗi.
        public void Dispose()
        {
            try { if (transaction != null) transaction.Rollback(); }
            finally
            {
                if (transaction != null) transaction.Dispose();
                transaction = null;
                connection.Dispose();
            }
        }
    }
}

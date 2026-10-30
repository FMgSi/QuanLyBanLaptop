using System;
using System.Data;
using System.Data.SqlClient;
namespace QuanLyBanLaptop.Classes
{
    public interface IDatabaseSession : IDisposable
    {
        DataTable DocBang(string sql, params object[] values);
        int CapNhat(string sql, params object[] values);
        object LayMotGiaTri(string sql, params object[] values);
        void BatDauGiaoDich();
        void XacNhan();
    }
    public class DbHelpers : IDatabaseSession
    {
        private readonly SqlConnection connection = DataProcesser.GetOpenConnection();
        private SqlTransaction transaction;
        internal static SqlCommand CreateCommand(SqlConnection cn, SqlTransaction tx, string sql, SqlParameter[] p, CommandType type = CommandType.Text)
        {
            var cmd = new SqlCommand(sql, cn, tx) { CommandType = type };
            if (p != null) cmd.Parameters.AddRange(p);
            return cmd;
        }
        internal static SqlParameter[] Parameters(object[] values)
        {
            if (values == null) return new SqlParameter[0];
            var res = new SqlParameter[values.Length];
            for (int i = 0; i < values.Length; i++) res[i] = new SqlParameter("@" + i, values[i] ?? DBNull.Value);
            return res;
        }
        public DataTable DocBang(string sql, params object[] values)
        {
            using (var cmd = CreateCommand(connection, transaction, sql, Parameters(values)))
            using (var da = new SqlDataAdapter(cmd)) { var dt = new DataTable(); da.Fill(dt); return dt; }
        }
        public int CapNhat(string sql, params object[] values)
        {
            using (var cmd = CreateCommand(connection, transaction, sql, Parameters(values))) return cmd.ExecuteNonQuery();
        }
        public object LayMotGiaTri(string sql, params object[] values)
        {
            using (var cmd = CreateCommand(connection, transaction, sql, Parameters(values)))
            {
                var r = cmd.ExecuteScalar();
                return r == DBNull.Value ? null : r;
            }
        }
        public void BatDauGiaoDich()
        {
            if (transaction != null) throw new InvalidOperationException("Giao dịch đã bắt đầu.");
            transaction = connection.BeginTransaction();
        }
        public void XacNhan()
        {
            if (transaction == null) throw new InvalidOperationException("Chưa bắt đầu giao dịch.");
            transaction.Commit();
            transaction.Dispose();
            transaction = null;
        }
        public void Dispose()
        {
            try { if (transaction?.Connection != null) transaction.Rollback(); }
            finally { transaction?.Dispose(); transaction = null; connection.Dispose(); }
        }
    }
}

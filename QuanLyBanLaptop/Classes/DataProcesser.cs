using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace Chuong6_LTTQ_LMS.Classes
{
    internal class DataProcesser
    {
        string strConnect = @"Data Source=.;Initial Catalog=QuanLyBanLapTop;Integrated Security=True";
        SqlConnection sqlConnection = null;

        //Ham mo ket noi
        private void KetNoiCSDL()
        {
            sqlConnection = new SqlConnection(strConnect);
            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();
        }
        //Ham dong ket noi
        private void DongKetNoiCSDL()
        {
            if (sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
            sqlConnection.Dispose();
        }
        //Ham thuc thi cau lenh dang select tra ve mot DataTable
        public DataTable DocBang(string sql)
        {
            DataTable dtBang = new DataTable();
            KetNoiCSDL();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, sqlConnection);
            sqlDataAdapter.Fill(dtBang);
            DongKetNoiCSDL();
            return dtBang;
        }
        //Ham thuc thi lenh insert or update or delete
        public void CapNhatDuLieu(string sql)
        {
            KetNoiCSDL();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = sql;
            sqlCommand.ExecuteNonQuery();
            DongKetNoiCSDL();
            sqlCommand.Dispose();
        }

    }
}
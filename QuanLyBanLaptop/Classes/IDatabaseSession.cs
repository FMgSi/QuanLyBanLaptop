using System;
using System.Data;

namespace QuanLyBanLaptop.Classes
{
    // Một service có thể nhận phiên SQL thật hoặc phiên giả khi kiểm thử.
    public interface IDatabaseSession : IDisposable
    {
        DataTable DocBang(string sql, params object[] values);
        object LayMotGiaTri(string sql, params object[] values);
        int CapNhat(string sql, params object[] values);
        void BatDauGiaoDich();
        void XacNhan();
    }
}

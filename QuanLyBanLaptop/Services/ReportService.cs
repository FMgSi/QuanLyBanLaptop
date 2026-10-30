using System;
using System.Data;
using QuanLyBanLaptop.Classes;
namespace QuanLyBanLaptop.Services
{
    public enum RevenueGrouping { Day, Month, Product, Employee }
    public sealed class ReportService
    {
        public DataTable GetRevenue(RevenueGrouping grouping, DateTime from, DateTime to)
        {
            UserSession.RequireAdmin();
            if (from.Date > to.Date) throw new ArgumentException("Khoảng ngày không hợp lệ.");
            string columns, group, order;
            switch (grouping)
            {
                case RevenueGrouping.Day:
                    columns = "CAST(h.NgayLap AS date) AS Ngay";
                    group = "CAST(h.NgayLap AS date)"; order = "Ngay"; break;
                case RevenueGrouping.Month:
                    columns = "CAST(MONTH(h.NgayLap) AS varchar(2))+'/'+CAST(YEAR(h.NgayLap) AS varchar(4)) AS ThangNam";
                    group = "YEAR(h.NgayLap),MONTH(h.NgayLap)"; order = group; break;
                case RevenueGrouping.Product:
                    columns = "l.MaLaptop,l.TenLaptop,l.Loai";
                    group = columns; order = "DoanhThu DESC"; break;
                case RevenueGrouping.Employee:
                    columns = "n.MaNV,n.TenNV,n.ChucVu";
                    group = columns; order = "DoanhThu DESC"; break;
                default: throw new ArgumentOutOfRangeException(nameof(grouping));
            }
            return DataProcesser.ReadData("SELECT " + columns + @",
                COUNT(DISTINCT h.MaHD) AS SoHoaDon,SUM(c.SoLuong) AS SoMayBan,
                SUM(c.ThanhTien) AS DoanhThu,SUM(c.SoLuong*c.GiaVon) AS GiaVon,
                SUM(c.ThanhTien-c.SoLuong*c.GiaVon) AS LoiNhuan
                FROM HOA_DON h JOIN CHI_TIET_HD c ON c.MaHD=h.MaHD
                JOIN LAPTOP l ON l.MaLaptop=c.MaLaptop JOIN NHAN_VIEN n ON n.MaNV=h.MaNV
                WHERE h.NgayLap>=@0 AND h.NgayLap<@1 GROUP BY " + group + " ORDER BY " + order,
                from.Date, to.Date.AddDays(1));
        }
    }
}

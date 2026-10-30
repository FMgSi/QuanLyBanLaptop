using System;
using System.Data;
namespace QuanLyBanLaptop.HoaDon
{
    public class HoaDonPrintData
    {
        public string MaHD { get; set; }
        public DateTime NgayLap { get; set; } = DateTime.Now;
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string HangKhach { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public bool IsTraGop { get; set; }
        public string PhuongThuc { get; set; }
        public int ChuKy { get; set; }
        public double LaiSuat { get; set; }
        public decimal TongTien { get; set; }
        public decimal TienTraTruoc { get; set; }
        public decimal TienVay { get; set; }
        public decimal TienLai { get; set; }
        public decimal TienHangThang { get; set; }
        public decimal KhachDua { get; set; }
        public decimal TienThua { get; set; }
        public DataTable DtChiTiet { get; set; }
    }
}

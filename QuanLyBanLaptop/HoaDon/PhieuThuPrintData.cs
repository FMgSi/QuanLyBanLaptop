using System;

namespace QuanLyBanLaptop.HoaDon
{
    /// <summary>
    /// Dữ liệu in phiếu thu tiền kỳ trả góp kèm mã QR hoặc tiền mặt
    /// </summary>
    public class PhieuThuPrintData
    {
        public string MaHD { get; set; }
        public DateTime NgayThu { get; set; } = DateTime.Now;

        public string TenKH { get; set; }
        public string SDT { get; set; }

        public int KyThu { get; set; }
        public int TongSoKy { get; set; }
        public DateTime NgayDenHan { get; set; }

        public decimal TienGocKy { get; set; }
        public int SoNgayTre { get; set; }
        public decimal TienPhat { get; set; }
        public decimal TongThucThu { get; set; }

        public string PhuongThucThu { get; set; } = "Chuyển khoản ngân hàng (Mã QR VietQR)";
        public decimal TienKhachDua { get; set; }
        public decimal TienThua { get; set; }

        public string TenNV { get; set; }
        public string MaNV { get; set; }
    }
}

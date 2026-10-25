using System;
using System.Data;

namespace QuanLyBanLaptop.HoaDon
{
    /// <summary>
    /// Chứa toàn bộ dữ liệu cần thiết để in hóa đơn bán hàng / hóa đơn trả góp.
    /// </summary>
    public class HoaDonPrintData
    {
        // Thông tin hóa đơn
        public string MaHD { get; set; }
        public DateTime NgayLap { get; set; } = DateTime.Now;

        // Thông tin khách hàng
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string HangKhach { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }

        // Thông tin nhân viên lập / thu ngân
        public string MaNV { get; set; }
        public string TenNV { get; set; }

        // Hình thức thanh toán: Trả thẳng hay Trả góp
        public bool IsTraGop { get; set; }
        public string PhuongThuc { get; set; } // "Tiền mặt", "Chuyển khoản", "Trả góp"
        public int ChuKy { get; set; }         // 0, 3, 6, 9, 12 tháng
        public double LaiSuat { get; set; }     // 2.4, 4.0, 6.0, 8.0 %

        // Số tiền
        public decimal TongTien { get; set; }       // Tổng giá trị máy / hóa đơn
        public decimal TienTraTruoc { get; set; }   // 30% nếu trả góp (số tiền thanh toán ngay)
        public decimal TienVay { get; set; }        // 70% còn lại nợ góp
        public decimal TienLai { get; set; }        // Tiền lãi phát sinh
        public decimal TienHangThang { get; set; }  // Số tiền trả góp mỗi tháng

        // Thanh toán tiền mặt (trả thẳng)
        public decimal KhachDua { get; set; }
        public decimal TienThua { get; set; }

        // Bảng danh sách chi tiết các mặt hàng mua
        public DataTable DtChiTiet { get; set; }
    }
}

using System.Data;
using System.Windows.Forms;

namespace QuanLyBanLaptop.Classes
{
    /// <summary>
    /// Lớp tiện ích dùng chung trong toàn bộ ứng dụng.
    /// </summary>
    internal static class Function
    {
        // ─── Thông tin phiên đăng nhập hiện tại ───────────────────────────
        /// <summary>Mã nhân viên đang đăng nhập.</summary>
        public static string CurrentMaNV { get; set; }

        /// <summary>Tên nhân viên đang đăng nhập.</summary>
        public static string CurrentTenNV { get; set; }

        /// <summary>Chức vụ của nhân viên đang đăng nhập.</summary>
        public static string CurrentChucVu { get; set; }

        /// <summary>Nhân viên hiện tại có phải Quản trị viên không.</summary>
        public static bool IsAdmin =>
            CurrentChucVu == "Quản trị viên";

        // ─── Lưu thông tin đăng nhập từ DataRow ───────────────────────────
        /// <summary>
        /// Gán thông tin phiên đăng nhập từ DataRow trả về bởi sp_DangNhap.
        /// </summary>
        public static void SetCurrentUser(DataRow row)
        {
            CurrentMaNV  = row["MaNV"]?.ToString();
            CurrentTenNV = row["TenNV"]?.ToString();
            CurrentChucVu = row["ChucVu"]?.ToString();
        }

        /// <summary>Xóa thông tin phiên đăng nhập (khi đăng xuất).</summary>
        public static void ClearCurrentUser()
        {
            CurrentMaNV  = null;
            CurrentTenNV = null;
            CurrentChucVu = null;
        }

        // ─── Format tiền tệ ───────────────────────────────────────────────
        /// <summary>
        /// Định dạng số tiền kiểu "1.500.000 ₫".
        /// </summary>
        public static string FormatTien(decimal amount)
            => amount.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + " ₫";

        // ─── Thông báo ────────────────────────────────────────────────────
        public static void ThongBaoLoi(string msg)
            => MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static void ThongBaoThanhCong(string msg)
            => MessageBox.Show(msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static DialogResult XacNhan(string msg)
            => MessageBox.Show(msg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }
}

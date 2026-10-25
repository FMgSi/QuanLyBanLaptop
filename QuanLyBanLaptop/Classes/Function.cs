using System;
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

        /// <summary>
        /// Đọc số tiền thành chữ tiếng Việt (VD: 1.374.000 -> Một triệu ba trăm bảy mươi tư nghìn đồng).
        /// </summary>
        public static string ChuyenSoThanhChu(decimal number)
        {
            if (number == 0) return "Không đồng";
            if (number < 0) return "Âm " + ChuyenSoThanhChu(Math.Abs(number));

            long n = (long)Math.Round(number);
            if (n == 0) return "Không đồng";

            string sNumber = n.ToString();
            int len = sNumber.Length;
            int rem = len % 3;
            if (rem > 0)
                sNumber = sNumber.PadLeft(len + (3 - rem), '0');

            string[] chuSo = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] dvTien = { "", "nghìn", "triệu", "tỷ", "nghìn tỷ", "triệu tỷ" };

            int numGroups = sNumber.Length / 3;
            string result = "";

            for (int i = 0; i < numGroups; i++)
            {
                string group = sNumber.Substring(i * 3, 3);
                int tram = group[0] - '0';
                int chuc = group[1] - '0';
                int donvi = group[2] - '0';

                if (tram == 0 && chuc == 0 && donvi == 0)
                    continue;

                string groupStr = "";
                if (i == 0 && tram == 0)
                {
                    if (chuc == 1)
                        groupStr += "mười ";
                    else if (chuc > 1)
                        groupStr += chuSo[chuc] + " mươi ";

                    if (chuc > 0 && donvi == 1 && chuc != 1)
                        groupStr += "mốt ";
                    else if (chuc > 0 && donvi == 5)
                        groupStr += "lăm ";
                    else if (donvi > 0)
                        groupStr += chuSo[donvi] + " ";
                }
                else
                {
                    groupStr += chuSo[tram] + " trăm ";

                    if (chuc == 0 && donvi != 0)
                        groupStr += "lẻ ";
                    else if (chuc == 1)
                        groupStr += "mười ";
                    else if (chuc > 1)
                        groupStr += chuSo[chuc] + " mươi ";

                    if (chuc > 0 && donvi == 1 && chuc != 1)
                        groupStr += "mốt ";
                    else if (chuc > 0 && donvi == 5)
                        groupStr += "lăm ";
                    else if (donvi > 0)
                        groupStr += chuSo[donvi] + " ";
                }

                int groupLevel = numGroups - 1 - i;
                if (groupLevel >= 0 && groupLevel < dvTien.Length && !string.IsNullOrEmpty(dvTien[groupLevel]))
                    groupStr += dvTien[groupLevel] + " ";

                result += groupStr;
            }

            result = result.Trim();
            if (string.IsNullOrEmpty(result)) return "Không đồng";
            result = char.ToUpper(result[0]) + result.Substring(1) + " đồng";
            while (result.Contains("  ")) result = result.Replace("  ", " ");
            return result;
        }
    }
}

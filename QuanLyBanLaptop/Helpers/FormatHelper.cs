using System;
using System.Globalization;

namespace QuanLyBanLaptop.Helpers
{
    /// <summary>Định dạng / phân tích số tiền và ngày tháng theo chuẩn Việt Nam.</summary>
    public static class FormatHelper
    {
        private static readonly CultureInfo Vn = new CultureInfo("vi-VN");

        /// <summary>1234567 -> "1.234.567".</summary>
        public static string Money(decimal value) => value.ToString("N0", Vn);

        /// <summary>"1.234.567" -> 1234567. Trả về false nếu chuỗi không hợp lệ.</summary>
        public static bool TryParseMoney(string text, out decimal value) =>
            decimal.TryParse(text, NumberStyles.Number, Vn, out value);

        /// <summary>Ngày dạng dd/MM/yyyy.</summary>
        public static string Date(DateTime date) => date.ToString("dd/MM/yyyy", Vn);
    }
}

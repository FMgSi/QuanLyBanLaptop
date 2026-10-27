using System;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.Services
{
    // Công thức trả góp dùng chung cho lịch xem trước, lưu và in hóa đơn.
    public sealed class InstallmentCalculator
    {
        public decimal Rate { get; private set; }
        public decimal DownPayment { get; private set; }
        public decimal Principal { get; private set; }
        public decimal Interest { get; private set; }
        public decimal Monthly { get; private set; }

        // Đọc lãi suất trọn kỳ; cấu hình thiếu/sai phải được xử lý trước khi giao dịch.
        public static decimal RateFor(int months)
        {
            if (months != 3 && months != 6 && months != 9 && months != 12)
                throw new ArgumentException("Kỳ hạn không hợp lệ.");

            decimal rate = new ConfigurationService().GetDecimal("LAI_SUAT_" + months + "_THANG");
            if (rate < 0 || rate > 100) throw new InvalidOperationException("Lãi suất cấu hình không hợp lệ.");
            return rate;
        }

        public static decimal GetDefaultDownPaymentRatio(int? months = null)
        {
            if (months.HasValue && months != 3 && months != 6 && months != 9 && months != 12)
                throw new ArgumentException("Kỳ hạn không hợp lệ.");
            string key = months.HasValue ? "TRA_TRUOC_" + months + "_THANG" : "TRA_TRUOC_TOI_THIEU";
            decimal ratio = new ConfigurationService().GetDecimal(key) / 100m;
            if (ratio < 0 || ratio >= 1) throw new InvalidOperationException("Tỷ lệ trả trước cấu hình không hợp lệ.");
            return ratio;
        }
        // Làm tròn tiền đồng thống nhất với SQL ROUND; kỳ cuối nhận phần dư.
        public static InstallmentCalculator Calculate(decimal total, int months, decimal? rate = null, decimal? downRatio = null)
        {
            if (months <= 0) throw new ArgumentException("Kỳ hạn phải lớn hơn 0.");
            decimal ratio = downRatio ?? GetDefaultDownPaymentRatio(months);
            if (total < 0 || ratio < 0 || ratio >= 1) throw new ArgumentException("Số tiền/tỷ lệ không hợp lệ.");
            decimal interestRate = rate ?? RateFor(months);
            if (months <= 0 || interestRate < 0) throw new ArgumentException("Kỳ hạn/lãi suất không hợp lệ.");
            decimal down = Math.Round(total * ratio, 0, MidpointRounding.AwayFromZero);
            decimal principal = total - down;
            decimal interest = Math.Round(principal * interestRate / 100m, 0, MidpointRounding.AwayFromZero);
            return new InstallmentCalculator { Rate = interestRate, DownPayment = down,
                Principal = principal, Interest = interest, Monthly = Math.Floor((principal + interest) / months) };
        }

        // Đọc điều khoản lịch sử thay vì áp gói hiện tại vào hóa đơn cũ.
        public static InstallmentCalculator Load(string invoiceId, decimal total, int months)
        {
            var table = DataProcesser.ReadData("SELECT LaiSuat,TyLeTraTruoc FROM HOP_DONG_TRA_GOP WHERE MaHD=@0", invoiceId);
            if (table.Rows.Count != 1) throw new InvalidOperationException("Thiếu điều khoản trả góp.");
            return Calculate(total, months, Convert.ToDecimal(table.Rows[0]["LaiSuat"]),
                Convert.ToDecimal(table.Rows[0]["TyLeTraTruoc"]));
        }
    }
}

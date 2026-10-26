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

        // Lãi suất trọn kỳ của các gói hiện tại (ưu tiên đọc từ CAU_HINH, dự phòng giá trị mặc định).
        public static decimal RateFor(int months)
        {
            if (months != 3 && months != 6 && months != 9 && months != 12)
                throw new ArgumentException("Kỳ hạn không hợp lệ.");

            try
            {
                string key = "LAI_SUAT_" + months + "_THANG";
                object val = DataProcesser.ExecuteScalar("SELECT GiaTri FROM CAU_HINH WHERE Khoa = '" + key + "'");
                if (val != null && decimal.TryParse(val.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal configuredRate))
                {
                    return configuredRate;
                }
            }
            catch { }

            switch (months)
            {
                case 3: return 2.4m;
                case 6: return 4m;
                case 9: return 6m;
                case 12: return 8m;
                default: throw new ArgumentException("Kỳ hạn không hợp lệ.");
            }
        }

        // Tỷ lệ trả trước mặc định từ CAU_HINH (mặc định 30%)
        public static decimal GetDefaultDownPaymentRatio()
        {
            try
            {
                object val = DataProcesser.ExecuteScalar("SELECT GiaTri FROM CAU_HINH WHERE Khoa = 'TRA_TRUOC_TOI_THIEU'");
                if (val != null && decimal.TryParse(val.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal pct) && pct > 0 && pct < 100)
                {
                    return pct / 100m;
                }
            }
            catch { }
            return 0.30m;
        }

        // Làm tròn tiền đồng thống nhất với SQL ROUND; kỳ cuối nhận phần dư.
        public static InstallmentCalculator Calculate(decimal total, int months, decimal? rate = null, decimal? downRatio = null)
        {
            decimal ratio = downRatio ?? GetDefaultDownPaymentRatio();
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

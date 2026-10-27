using System;
using System.Data;
using System.Globalization;
using System.Collections.Generic;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.Services
{
    public sealed class ConfigurationService
    {
        private static readonly Dictionary<string, decimal> Discounts = new Dictionary<string, decimal>
        {
            { "GIAM_GIA_DONG", 0 }, { "GIAM_GIA_BAC", 5 }, { "GIAM_GIA_VANG", 15 }, { "GIAM_GIA_VIP", 15 }
        };

        public DataTable GetAll()
        {
            UserSession.RequireAdmin();
            var table = DataProcesser.ReadData("SELECT Khoa,GiaTri,MoTa FROM CAU_HINH ORDER BY Khoa");
            var defaults = new Dictionary<string, decimal>(Discounts);
            foreach (int months in new[] { 3, 6, 9, 12 })
                defaults.Add("TRA_TRUOC_" + months + "_THANG", GetDecimal("TRA_TRUOC_TOI_THIEU"));
            foreach (var item in defaults)
                if (table.Select("Khoa='" + item.Key + "'").Length == 0)
                    table.Rows.Add(item.Key, item.Value.ToString(CultureInfo.InvariantCulture), DBNull.Value);
            return table;
        }

        public decimal GetDecimal(string key)
        {
            var value = DataProcesser.ExecuteScalar("SELECT GiaTri FROM CAU_HINH WHERE Khoa=@0", key);
            // Tương thích DB cũ: chỉ tham số mới có mặc định, không tự ghi khi đọc.
            if (value == null && Discounts.ContainsKey(key)) return Discounts[key];
            if (value == null && IsTermDownPayment(key)) return GetDecimal("TRA_TRUOC_TOI_THIEU");
            decimal number;
            if (value == null || !decimal.TryParse(value.ToString(), NumberStyles.Number,
                CultureInfo.InvariantCulture, out number))
                throw new InvalidOperationException("Thiếu hoặc sai cấu hình: " + key);
            return number;
        }

        private static bool IsTermDownPayment(string key) =>
            key == "TRA_TRUOC_3_THANG" || key == "TRA_TRUOC_6_THANG" ||
            key == "TRA_TRUOC_9_THANG" || key == "TRA_TRUOC_12_THANG";

        public decimal DiscountFor(string membership)
        {
            string key;
            switch ((membership ?? "").Trim().ToUpperInvariant())
            {
                case "BẠC": key = "GIAM_GIA_BAC"; break;
                case "VÀNG": key = "GIAM_GIA_VANG"; break;
                case "VIP": key = "GIAM_GIA_VIP"; break;
                default: key = "GIAM_GIA_DONG"; break;
            }
            decimal discount = GetDecimal(key);
            if (discount < 0 || discount > 100) throw new InvalidOperationException("Cấu hình giảm giá phải từ 0 đến 100%.");
            return discount;
        }

        public void Save(string key, decimal value)
        {
            if (!UserSession.IsAdmin) throw new InvalidOperationException("Chỉ quản trị viên được sửa cấu hình.");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Chưa chọn tham số.");
            bool down = key == "TRA_TRUOC_TOI_THIEU" || IsTermDownPayment(key);
            bool rate = key == "LAI_SUAT_3_THANG" || key == "LAI_SUAT_6_THANG" || key == "LAI_SUAT_9_THANG" || key == "LAI_SUAT_12_THANG";
            bool money = key == "NGUONG_BAC" || key == "NGUONG_VANG" || key == "NGUONG_VIP" || key == "TIEN_PHAT_NGAY";
            if (!down && !rate && !money && !Discounts.ContainsKey(key)) throw new ArgumentException("Tham số không được hỗ trợ.");
            if (value < 0 || (down && value >= 100) || ((rate || Discounts.ContainsKey(key)) && value > 100) ||
                (money && value != decimal.Truncate(value)) || (!money && decimal.Round(value, 2) != value))
                throw new ArgumentException("Giá trị cấu hình không hợp lệ; tỷ lệ trả trước phải nhỏ hơn 100%.");
            using (var db = new DbHelpers())
            {
                db.BatDauGiaoDich();
                db.CapNhat(@"IF EXISTS(SELECT 1 FROM CAU_HINH WITH(UPDLOCK,HOLDLOCK) WHERE Khoa=@1)
                    UPDATE CAU_HINH SET GiaTri=@0 WHERE Khoa=@1;
                    ELSE INSERT CAU_HINH(Khoa,GiaTri) VALUES(@1,@0);", value.ToString(CultureInfo.InvariantCulture), key);
                db.XacNhan();
            }
        }
    }
}

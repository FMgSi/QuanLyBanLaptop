using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
namespace QuanLyBanLaptop.Classes
{
    public static class Function
    {
        public static string CurrentMaNV { get => UserSession.MaNV; set => UserSession.MaNV = value; }
        public static string CurrentTenNV { get => UserSession.TenNV; set => UserSession.TenNV = value; }
        public static string CurrentChucVu { get => UserSession.ChucVu; set => UserSession.ChucVu = value; }
        public static bool IsAdmin => CurrentChucVu == "Quản trị viên" || CurrentChucVu == "Quản lý";
        public static void ClearCurrentUser() => UserSession.Logout();
        public static DataTable DocBang(string sql, params SqlParameter[] p) => DataProcesser.ReadData(sql, p);
        public static DataTable DocBang(string sql, params object[] v) => DataProcesser.ReadData(sql, v);
        public static int CapNhat(string sql, params SqlParameter[] p) => DataProcesser.ChangeData(sql, p);
        public static int CapNhat(string sql, params object[] v) => DataProcesser.ChangeData(sql, v);
        public static string FormatTien(decimal amount) => amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " ₫";
        public static string ChuyenSoThanhChu(decimal number)
        {
            if (number == 0) return "Không đồng";
            if (number < 0) return "Âm " + ChuyenSoThanhChu(Math.Abs(number));
            long n = (long)Math.Round(number);
            if (n == 0) return "Không đồng";
            string s = n.ToString();
            int rem = s.Length % 3;
            if (rem > 0) s = s.PadLeft(s.Length + (3 - rem), '0');
            string[] chuSo = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] dvTien = { "", "nghìn", "triệu", "tỷ", "nghìn tỷ", "triệu tỷ" };
            int groups = s.Length / 3;
            string res = "";
            for (int i = 0; i < groups; i++)
            {
                int tram = s[i * 3] - '0', chuc = s[i * 3 + 1] - '0', donvi = s[i * 3 + 2] - '0';
                if (tram == 0 && chuc == 0 && donvi == 0) continue;
                string g = "";
                if (i > 0 || tram > 0)
                {
                    g += chuSo[tram] + " trăm ";
                    if (chuc == 0 && donvi != 0) g += "lẻ ";
                }
                if (chuc == 1) g += "mười ";
                else if (chuc > 1) g += chuSo[chuc] + " mươi ";
                if (chuc > 0 && donvi == 1 && chuc != 1) g += "mốt ";
                else if (chuc > 0 && donvi == 5) g += "lăm ";
                else if (donvi > 0) g += chuSo[donvi] + " ";
                int lvl = groups - 1 - i;
                if (lvl > 0 && lvl < dvTien.Length) g += dvTien[lvl] + " ";
                res += g;
            }
            res = res.Trim();
            if (string.IsNullOrEmpty(res)) return "Không đồng";
            res = char.ToUpper(res[0]) + res.Substring(1) + " đồng";
            while (res.Contains("  ")) res = res.Replace("  ", " ");
            return res;
        }
        public static string SinhMaTuDong(string tenBang, string tenCot, string tienTo, int doRong = 2)
        {
            string seq;
            switch (tenBang + "." + tenCot)
            {
                case "LAPTOP.MaLaptop": seq = "seq_AppLaptop"; break;
                case "NHA_CUNG_CAP.MaNCC": seq = "seq_AppSupplier"; break;
                case "KHACH_HANG.MaKH": seq = "seq_AppCustomer"; break;
                case "NHAN_VIEN.MaNV": seq = "seq_AppEmployee"; break;
                default: throw new ArgumentException("Bảng sinh mã chưa được hỗ trợ.");
            }
            long next = Convert.ToInt64(DataProcesser.ExecuteScalar("SELECT NEXT VALUE FOR dbo." + seq));
            string id = tienTo + next.ToString("D" + doRong);
            if (id.Length > 10) throw new InvalidOperationException("Mã vượt giới hạn 10 ký tự.");
            return id;
        }
        public static void ThongBaoLoi(string msg, string title = "Lỗi") => MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        public static void ThongBaoThanhCong(string msg, string title = "Thành công") => MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        public static void ThongBaoCanhBao(string msg, string title = "Cảnh báo") => MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        public static DialogResult XacNhan(string msg, string title = "Xác nhận") => MessageBox.Show(msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        public static void DinhDangBang(DataGridView dgv)
        {
            if (dgv == null) return;
            dgv.EnableHeadersVisualStyles = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(229, 231, 235);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(239, 246, 255);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersHeight = 38;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 64, 175);
            dgv.RowTemplate.Height = 36;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 64, 175);
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = dgv.RowHeadersVisible = dgv.AllowUserToAddRows = dgv.AllowUserToDeleteRows = dgv.AllowUserToResizeRows = dgv.AllowUserToOrderColumns = false;
            dgv.AllowUserToResizeColumns = dgv.ReadOnly = true;
            dgv.ScrollBars = ScrollBars.Both;
        }
        public static void DatTieuDeCot(DataGridView dgv, string tenCot, string tieuDe, int doRong = 0)
        {
            if (dgv != null && dgv.Columns.Contains(tenCot))
            {
                dgv.Columns[tenCot].HeaderText = tieuDe;
                if (doRong > 0)
                {
                    dgv.Columns[tenCot].Width = doRong;
                    dgv.Columns[tenCot].MinimumWidth = Math.Max(doRong, 60);
                }
            }
        }
        public static string LayGiaTriCell(DataGridViewRow row, string tenCot, string macDinh = "") =>
            row == null || !row.DataGridView.Columns.Contains(tenCot) ? macDinh : (row.Cells[tenCot]?.Value?.ToString() ?? macDinh);
        public static string SinhMaHoaDonTuDong() =>
            $"HDB_{DateTime.Now:ddMMyyyy}{Convert.ToInt64(DataProcesser.ExecuteScalar("SELECT NEXT VALUE FOR dbo.seq_AppInvoice")):D4}";
    }
}

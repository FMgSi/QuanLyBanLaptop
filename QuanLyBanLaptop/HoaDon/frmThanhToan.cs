using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmThanhToan : AppForm
    {
        [DllImport("user32.dll")] public static extern bool ReleaseCapture();
        [DllImport("user32.dll")] public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1, HTCAPTION = 0x2;
        private readonly string _maHD, _maKH, _tenKH, _hangKhach, _maNV, _tenNV, _diaChi, _sdt;
        private readonly DateTime _ngayLap;
        private readonly decimal _tongTien;
        private readonly DataTable _dtGioHang;
        public string MaHDDaLuu { get; private set; }
        public string PhuongThucChon { get; private set; }
        public int ChuKyChon { get; private set; }
        private const decimal TY_LE_TRA_TRUOC = 0.30m;
        private static readonly (int Ky, double LaiSuat, string HienThi)[] _goiTraGop =
        {
            (3,  2.4, "3 Tháng (Lãi suất 2.4%)"),
            (6,  4.0, "6 Tháng (Lãi suất 4.0%)"),
            (9,  6.0, "9 Tháng (Lãi suất 6.0%)"),
            (12, 8.0, "12 Tháng (Lãi suất 8.0%)")
        };
        public frmThanhToan(string maHD, string maKH, string tenKH, string hangKhach, string maNV, string tenNV, string diaChi, string sdt, DateTime ngayLap, decimal tongTien, DataTable dtGioHang)
        {
            InitializeComponent();
            _maHD = maHD; _maKH = maKH; _tenKH = tenKH; _hangKhach = hangKhach;
            _maNV = maNV; _tenNV = tenNV; _diaChi = diaChi; _sdt = sdt;
            _ngayLap = ngayLap; _tongTien = tongTien; _dtGioHang = dtGioHang;
            MouseEventHandler drag = (s, e) => { if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0); } };
            pnlTitleBar.MouseDown += drag;
            lblIconAndTitle.MouseDown += drag;
            Paint += (s, e) => { using (var pen = new Pen(Color.FromArgb(30, 58, 138), 2)) e.Graphics.DrawRectangle(pen, 1, 1, ClientSize.Width - 2, ClientSize.Height - 2); };
            pnlInvoiceSummary.Paint += (s, e) => { using (var pen = new Pen(Color.FromArgb(191, 219, 254), 1)) e.Graphics.DrawRectangle(pen, 0, 0, pnlInvoiceSummary.Width - 1, pnlInvoiceSummary.Height - 1); };
            foreach (var c in new Control[] { pnlTabThangCard, pnlTabGopCard, dgvLichTraGop })
                c.Paint += (s, e) => { using (var pen = new Pen(Color.FromArgb(203, 213, 225), 1)) e.Graphics.DrawRectangle(pen, 0, 0, c.Width - 1, c.Height - 1); };
            pnlTabBar.Paint += (s, e) => { using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1)) e.Graphics.DrawLine(pen, 0, pnlTabBar.Height - 1, pnlTabBar.Width, pnlTabBar.Height - 1); };
        }
        private void frmThanhToan_Load(object sender, EventArgs e)
        {
            pnlTitleBar.BringToFront();
            pnlFooter.BringToFront();
            lblMaHDValue.Text = _maHD;
            lblTenKhach.Text = string.IsNullOrEmpty(_tenKH) ? "Khách vãng lai" : _tenKH;
            lblHangKhach.Text = "(" + (string.IsNullOrEmpty(_hangKhach) ? "Đồng" : _hangKhach) + ")";
            lblTongTienValue.Text = Function.FormatTien(_tongTien);
            lblMaHDValue.Left = lblIconAndTitle.Right + 4;
            lblHangKhach.Left = lblTenKhach.Right + 6;
            cboPhuongThuc.Items.Clear();
            cboPhuongThuc.Items.AddRange(new object[] { "Chuyển khoản ngân hàng", "Tiền mặt" });
            cboPhuongThuc.SelectedIndex = 0;
            txtTongThanhToan.Text = Function.FormatTien(_tongTien);
            txtKhachDua.Text = _tongTien.ToString("0");
            TinhTienThua();
            decimal downpayment = Math.Round(_tongTien * TY_LE_TRA_TRUOC, 0, MidpointRounding.AwayFromZero);
            txtTraTruoc.Text = Function.FormatTien(downpayment);
            txtSoTienVay.Text = Function.FormatTien(_tongTien - downpayment);
            cboChuKy.Items.Clear();
            foreach (var g in _goiTraGop) cboChuKy.Items.Add(g.HienThi);
            cboChuKy.SelectedIndex = 0;
            ChonTab(1);
        }
        private void btnCloseTitle_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
        private void btnHuy_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
        private void btnTabThang_Click(object sender, EventArgs e) => ChonTab(1);
        private void btnTabGop_Click(object sender, EventArgs e) => ChonTab(2);
        private void ChonTab(int tabIndex)
        {
            ButtonTheme.Apply(btnTabThang);
            ButtonTheme.Apply(btnTabGop);
            var activeBtn = tabIndex == 1 ? btnTabThang : btnTabGop;
            pnlTabIndicator.Location = new Point(activeBtn.Left, pnlTabBar.Height - 4);
            pnlTabIndicator.Width = activeBtn.Width;
            pnlTabThangContent.Visible = tabIndex == 1;
            pnlTabGopContent.Visible = tabIndex != 1;
            if (tabIndex == 1) pnlTabThangContent.BringToFront();
            else { pnlTabGopContent.BringToFront(); TinhLichTraGop(); }
        }
        private void txtKhachDua_TextChanged(object sender, EventArgs e) => TinhTienThua();
        private void TinhTienThua()
        {
            string raw = txtKhachDua.Text.Trim().Replace(",", "").Replace(".", "");
            if (decimal.TryParse(raw, out decimal khachDua))
            {
                decimal thua = khachDua - _tongTien;
                txtTienThua.ForeColor = thua < 0 ? Color.FromArgb(220, 38, 38) : Color.FromArgb(15, 23, 42);
                txtTienThua.Text = thua < 0 ? "Chưa đủ tiền (" + Function.FormatTien(thua) + ")" : Function.FormatTien(thua);
            }
            else txtTienThua.Text = "0 đ";
        }
        private void cboChuKy_SelectedIndexChanged(object sender, EventArgs e) => TinhLichTraGop();
        private void TinhLichTraGop()
        {
            if (cboChuKy.SelectedIndex < 0 || cboChuKy.SelectedIndex >= _goiTraGop.Length) return;
            var goi = _goiTraGop[cboChuKy.SelectedIndex];
            var plan = Services.InstallmentCalculator.Calculate(_tongTien, goi.Ky);
            decimal totalPayable = plan.Principal + plan.Interest, accumulated = 0;
            txtTraTruoc.Text = Function.FormatTien(plan.DownPayment);
            txtSoTienVay.Text = Function.FormatTien(plan.Principal);
            lblTongTienLai.Text = $"Tổng tiền lãi: {Function.FormatTien(plan.Interest)} (Lãi suất {goi.LaiSuat:0.0}%)";
            dgvLichTraGop.Rows.Clear();
            for (int k = 1; k <= goi.Ky; k++)
            {
                decimal soTienKy = k == goi.Ky ? totalPayable - accumulated : plan.Monthly;
                accumulated += soTienKy;
                dgvLichTraGop.Rows.Add("Kỳ " + k, _ngayLap.AddMonths(k).ToString("dd/MM/yyyy"), Function.FormatTien(soTienKy));
            }
        }
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (pnlTabThangContent.Visible) XacNhanTraThang();
            else XacNhanTraGop();
        }
        private void XacNhanTraThang()
        {
            string phuongThucUI = cboPhuongThuc.SelectedItem?.ToString() ?? "Chuyển khoản ngân hàng";
            decimal khachDua = _tongTien;
            if (phuongThucUI.Contains("Tiền mặt"))
            {
                string raw = txtKhachDua.Text.Trim().Replace(",", "").Replace(".", "");
                if (!decimal.TryParse(raw, out khachDua) || khachDua < _tongTien)
                {
                    Function.ThongBaoCanhBao("Tiền khách đưa chưa đủ. Cần thanh toán: " + Function.FormatTien(_tongTien), "Thông báo");
                    txtKhachDua.Focus();
                    return;
                }
            }
            decimal tienThua = Math.Max(0, khachDua - _tongTien);
            if (LuuHoaDonVaoDB(phuongThucUI.Contains("khoản") ? "Chuyển khoản" : "Tiền mặt", 0))
            {
                PhuongThucChon = phuongThucUI; ChuKyChon = 0; MaHDDaLuu = _maHD;
                InHoaDonPreview(false, 0, 0, 0, 0, 0, 0, khachDua, tienThua);
                DialogResult = DialogResult.OK; Close();
            }
        }
        private void XacNhanTraGop()
        {
            if (cboChuKy.SelectedIndex < 0) { Function.ThongBaoCanhBao("Vui lòng chọn chu kỳ trả góp.", "Thông báo"); return; }
            var goi = _goiTraGop[cboChuKy.SelectedIndex];
            var plan = Services.InstallmentCalculator.Calculate(_tongTien, goi.Ky);
            if (LuuHoaDonVaoDB("Trả góp", goi.Ky))
            {
                PhuongThucChon = "Trả góp"; ChuKyChon = goi.Ky; MaHDDaLuu = _maHD;
                InHoaDonPreview(true, goi.Ky, goi.LaiSuat, plan.DownPayment, plan.Principal, plan.Interest, plan.Monthly, 0, 0);
                DialogResult = DialogResult.OK; Close();
            }
        }
        private void InHoaDonPreview(bool isTraGop, int chuKy, double laiSuat, decimal downpayment, decimal loan, decimal interest, decimal monthly, decimal khachDua, decimal tienThua)
        {
            using (var frm = new frmPrintPreview(new HoaDonPrintData
            {
                MaHD = _maHD, NgayLap = _ngayLap, MaKH = _maKH, TenKH = _tenKH, HangKhach = _hangKhach, SDT = _sdt, DiaChi = _diaChi, MaNV = _maNV, TenNV = _tenNV,
                IsTraGop = isTraGop, PhuongThuc = isTraGop ? "Trả góp" : (cboPhuongThuc.SelectedItem?.ToString() ?? "Tiền mặt"),
                ChuKy = chuKy, LaiSuat = laiSuat, TongTien = _tongTien, TienTraTruoc = downpayment, TienVay = loan, TienLai = interest, TienHangThang = monthly,
                KhachDua = khachDua, TienThua = tienThua, DtChiTiet = _dtGioHang
            })) frm.ShowDialog(this);
        }
        private bool LuuHoaDonVaoDB(string phuongThuc, int chuKy)
        {
            try
            {
                new Services.SaleService().Save(new Services.SaleRequest
                {
                    InvoiceId = _maHD, CustomerId = _maKH, EmployeeId = _maNV, Date = _ngayLap, PaymentMethod = phuongThuc, Months = chuKy, Items = _dtGioHang
                });
                return true;
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message, "Không thể lưu hóa đơn"); return false; }
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmThuTienTraGop : AppForm
    {
        [DllImport("user32.dll")] public static extern bool ReleaseCapture();
        [DllImport("user32.dll")] public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1, HTCAPTION = 0x2;
        private readonly string _maHD, _tenKH, _sdt;
        private readonly int _kyThu, _tongSoKy;
        private readonly DateTime _ngayDenHan;
        private readonly decimal _tienGocKy;
        private int _soNgayTre;
        private decimal _tienPhat, _tongPhaiThu;
        public frmThuTienTraGop(string maHD, string tenKH, string sdt, int kyThu, int tongSoKy, DateTime ngayDenHan, decimal tienGocKy)
        {
            InitializeComponent();
            _maHD = maHD; _tenKH = tenKH; _sdt = sdt;
            _kyThu = kyThu; _tongSoKy = tongSoKy; _ngayDenHan = ngayDenHan; _tienGocKy = tienGocKy;
            MouseEventHandler drag = (s, e) => { if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0); } };
            pnlTitle.MouseDown += drag;
            lblTitle.MouseDown += drag;
            Paint += (s, e) => { using (var pen = new Pen(Color.FromArgb(29, 78, 216), 2)) e.Graphics.DrawRectangle(pen, 1, 1, ClientSize.Width - 2, ClientSize.Height - 2); };
            pnlCardInfo.Paint += (s, e) => { using (var pen = new Pen(Color.FromArgb(203, 213, 225), 1)) e.Graphics.DrawRectangle(pen, 0, 0, pnlCardInfo.Width - 1, pnlCardInfo.Height - 1); };
        }
        private void frmThuTienTraGop_Load(object sender, EventArgs e)
        {
            lblMaHoaDon.Text = "Mã hóa đơn: " + _maHD;
            lblKhachHang.Text = "Khách hàng: " + (string.IsNullOrEmpty(_tenKH) ? "Khách vãng lai" : _tenKH);
            lblKyThanhToan.Text = "Kỳ thanh toán: Kỳ thứ " + _kyThu + " / " + _tongSoKy;
            lblNgayDenHan.Text = "Ngày đến hạn: " + _ngayDenHan.ToString("dd/MM/yyyy");
            _soNgayTre = DateTime.Now.Date > _ngayDenHan.Date ? (int)(DateTime.Now.Date - _ngayDenHan.Date).TotalDays : 0;
            _tienPhat = _soNgayTre * new Services.InstallmentService().DailyPenalty();
            _tongPhaiThu = _tienGocKy + _tienPhat;
            txtTienGoc.Text = Function.FormatTien(_tienGocKy);
            txtSoNgayTre.Text = _soNgayTre.ToString();
            txtTienPhat.Text = Function.FormatTien(_tienPhat);
            txtTongPhaiThu.Text = Function.FormatTien(_tongPhaiThu);
            if (_soNgayTre == 0)
            {
                txtSoNgayTre.ForeColor = txtTienPhat.ForeColor = Color.FromArgb(71, 85, 105);
                txtSoNgayTre.BackColor = txtTienPhat.BackColor = Color.FromArgb(248, 250, 252);
            }
            cboPhuongThuc.SelectedIndex = 0;
            UpdatePhuongThucView();
        }
        private void btnClose_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
        private void btnDong_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
        private void cboPhuongThuc_SelectedIndexChanged(object sender, EventArgs e) => UpdatePhuongThucView();
        private void UpdatePhuongThucView()
        {
            bool isTienMat = cboPhuongThuc.SelectedIndex == 1;
            pnlTienMat.Visible = isTienMat;
            Height = isTienMat ? 490 : 440;
            btnXacNhanIn.Location = new Point(145, isTienMat ? 420 : 360);
            btnDong.Location = new Point(430, isTienMat ? 420 : 360);
            if (isTienMat) { txtKhachDua.Text = _tongPhaiThu.ToString("0"); TinhTienThua(); }
            else btnXacNhanIn.Enabled = true;
        }
        private void txtKhachDua_TextChanged(object sender, EventArgs e) => TinhTienThua();
        private void TinhTienThua()
        {
            string raw = txtKhachDua.Text.Trim().Replace(",", "").Replace(".", "");
            if (decimal.TryParse(raw, out decimal khachDua))
            {
                decimal thua = khachDua - _tongPhaiThu;
                lblTienThuaValue.ForeColor = thua < 0 ? Color.FromArgb(220, 38, 38) : Color.FromArgb(15, 23, 42);
                lblTienThuaValue.Text = thua < 0 ? "Chưa đủ (" + Function.FormatTien(thua) + ")" : Function.FormatTien(thua);
                lblCanhBao.Visible = thua < 0;
                btnXacNhanIn.Enabled = thua >= 0;
            }
            else
            {
                lblTienThuaValue.Text = "0 đ";
                lblCanhBao.Visible = true;
                btnXacNhanIn.Enabled = false;
            }
        }
        private void btnXacNhanIn_Click(object sender, EventArgs e)
        {
            bool isTienMat = cboPhuongThuc.SelectedIndex == 1;
            decimal khachDua = _tongPhaiThu, tienThua = 0;
            if (isTienMat)
            {
                string raw = txtKhachDua.Text.Trim().Replace(",", "").Replace(".", "");
                if (!decimal.TryParse(raw, out khachDua) || khachDua < _tongPhaiThu)
                {
                    Function.ThongBaoCanhBao("Số tiền khách đưa chưa đủ để thanh toán kỳ này!", "Cảnh báo");
                    txtKhachDua.Focus();
                    return;
                }
                tienThua = khachDua - _tongPhaiThu;
            }
            try
            {
                DataProcesser.ExecuteNonQuery("dbo.sp_ThanhToanKy", new[]
                {
                    new SqlParameter("@MaHD", SqlDbType.NVarChar, 30) { Value = _maHD },
                    new SqlParameter("@KyThu", SqlDbType.Int) { Value = _kyThu },
                    new SqlParameter("@NgayTra", SqlDbType.Date) { Value = DateTime.Now.Date },
                    new SqlParameter("@TienPhat", SqlDbType.Decimal) { Value = _tienPhat }
                }, CommandType.StoredProcedure);
            }
            catch (Exception ex) { Function.ThongBaoLoi("Lỗi khi lưu thanh toán kỳ vào CSDL: " + ex.Message, "Lỗi"); return; }
            using (var preview = new frmPrintPreview(new PhieuThuPrintData
            {
                MaHD = _maHD, NgayThu = DateTime.Now, TenKH = _tenKH, SDT = _sdt, KyThu = _kyThu, TongSoKy = _tongSoKy,
                NgayDenHan = _ngayDenHan, TienGocKy = _tienGocKy, SoNgayTre = _soNgayTre, TienPhat = _tienPhat, TongThucThu = _tongPhaiThu,
                PhuongThucThu = cboPhuongThuc.SelectedItem?.ToString() ?? "Chuyển khoản ngân hàng (Mã QR VietQR)",
                TienKhachDua = khachDua, TienThua = tienThua, TenNV = Function.CurrentTenNV ?? "Trần Thị Bình", MaNV = Function.CurrentMaNV ?? "NV002"
            })) preview.ShowDialog(this);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

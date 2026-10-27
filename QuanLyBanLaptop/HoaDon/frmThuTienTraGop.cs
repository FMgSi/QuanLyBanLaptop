using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmThuTienTraGop : QuanLyBanLaptop.Classes.AppForm
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private readonly string _maHD;
        private readonly string _tenKH;
        private readonly string _sdt;
        private readonly int _kyThu;
        private readonly int _tongSoKy;
        private readonly DateTime _ngayDenHan;
        private readonly decimal _tienGocKy;

        private int _soNgayTre = 0;
        private decimal _tienPhat = 0;
        private decimal _tongPhaiThu = 0;
        public frmThuTienTraGop(
            string maHD, string tenKH, string sdt,
            int kyThu, int tongSoKy, DateTime ngayDenHan, decimal tienGocKy)
        {
            InitializeComponent();

            _maHD        = maHD;
            _tenKH       = tenKH;
            _sdt         = sdt;
            _kyThu       = kyThu;
            _tongSoKy    = tongSoKy;
            _ngayDenHan  = ngayDenHan;
            _tienGocKy   = tienGocKy;

            // Kéo form bằng title bar
            pnlTitle.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };
            lblTitle.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };

            // Viền form
            this.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(29, 78, 216), 2))
                {
                    e.Graphics.DrawRectangle(pen, 1, 1, this.ClientSize.Width - 2, this.ClientSize.Height - 2);
                }
            };

            // Viền card thông tin
            pnlCardInfo.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(203, 213, 225), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlCardInfo.Width - 1, pnlCardInfo.Height - 1);
                }
            };
        }
        private void frmThuTienTraGop_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin chung
            lblMaHoaDon.Text    = "Mã hóa đơn: " + _maHD;
            lblKhachHang.Text   = "Khách hàng: " + (string.IsNullOrEmpty(_tenKH) ? "Khách vãng lai" : _tenKH);
            lblKyThanhToan.Text = "Kỳ thanh toán: Kỳ thứ " + _kyThu + " / " + _tongSoKy;
            lblNgayDenHan.Text  = "Ngày đến hạn: " + _ngayDenHan.ToString("dd/MM/yyyy");

            // Tính số ngày trễ hạn
            DateTime today = DateTime.Now.Date;
            if (today > _ngayDenHan.Date)
            {
                _soNgayTre = (int)(today - _ngayDenHan.Date).TotalDays;
            }
            else
            {
                _soNgayTre = 0;
            }

            // Lấy mức phạt từ CAU_HINH (mặc định 20.000 đ / ngày)
            decimal mucPhatNgay = new QuanLyBanLaptop.Services.InstallmentService().DailyPenalty();

            _tienPhat = _soNgayTre * mucPhatNgay;
            _tongPhaiThu = _tienGocKy + _tienPhat;

            // Hiển thị số liệu
            txtTienGoc.Text     = Function.FormatTien(_tienGocKy);
            txtSoNgayTre.Text   = _soNgayTre.ToString();
            txtTienPhat.Text    = Function.FormatTien(_tienPhat);
            txtTongPhaiThu.Text = Function.FormatTien(_tongPhaiThu);

            if (_soNgayTre == 0)
            {
                txtSoNgayTre.ForeColor = Color.FromArgb(71, 85, 105);
                txtSoNgayTre.BackColor = Color.FromArgb(248, 250, 252);
                txtTienPhat.ForeColor  = Color.FromArgb(71, 85, 105);
                txtTienPhat.BackColor  = Color.FromArgb(248, 250, 252);
            }

            cboPhuongThuc.SelectedIndex = 0;
            UpdatePhuongThucView();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void btnDong_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void cboPhuongThuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePhuongThucView();
        }
        private void UpdatePhuongThucView()
        {
            bool isTienMat = cboPhuongThuc.SelectedIndex == 1; // Tiền mặt
            pnlTienMat.Visible = isTienMat;

            if (isTienMat)
            {
                this.Height = 490;
                btnXacNhanIn.Location = new Point(145, 420);
                btnDong.Location      = new Point(430, 420);
                txtKhachDua.Text = _tongPhaiThu.ToString("0");
                TinhTienThua();
            }
            else
            {
                this.Height = 440;
                btnXacNhanIn.Location = new Point(145, 360);
                btnDong.Location      = new Point(430, 360);
                btnXacNhanIn.Enabled = true;
            }
        }
        private void txtKhachDua_TextChanged(object sender, EventArgs e)
        {
            TinhTienThua();
        }
        private void TinhTienThua()
        {
            string raw = txtKhachDua.Text.Trim().Replace(",", "").Replace(".", "");
            if (decimal.TryParse(raw, out decimal khachDua))
            {
                decimal thua = khachDua - _tongPhaiThu;
                if (thua < 0)
                {
                    lblTienThuaValue.ForeColor = Color.FromArgb(220, 38, 38);
                    lblTienThuaValue.Text = "Chưa đủ (" + Function.FormatTien(thua) + ")";
                    lblCanhBao.Visible = true;
                    btnXacNhanIn.Enabled = false;
                }
                else
                {
                    lblTienThuaValue.ForeColor = Color.FromArgb(15, 23, 42);
                    lblTienThuaValue.Text = Function.FormatTien(thua);
                    lblCanhBao.Visible = false;
                    btnXacNhanIn.Enabled = true;
                }
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
            decimal khachDua = _tongPhaiThu;
            decimal tienThua = 0;

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

            // Gọi sp_ThanhToanKy để lưu vào CSDL
            try
            {
                var pMaHD = new SqlParameter("@MaHD", SqlDbType.NVarChar, 30) { Value = _maHD };
                var pKy = new SqlParameter("@KyThu", SqlDbType.Int) { Value = _kyThu };
                var pNgay = new SqlParameter("@NgayTra", SqlDbType.Date) { Value = DateTime.Now.Date };
                var pPhat = new SqlParameter("@TienPhat", SqlDbType.Decimal) { Value = _tienPhat };

                DataProcesser.ExecuteNonQuery("dbo.sp_ThanhToanKy",
                    new[] { pMaHD, pKy, pNgay, pPhat },
                    CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Function.ThongBaoLoi("Lỗi khi lưu thanh toán kỳ vào CSDL: " + ex.Message, "Lỗi");
                return;
            }

            // Chuẩn bị dữ liệu in phiếu thu
            var phieuThu = new PhieuThuPrintData
            {
                MaHD          = _maHD,
                NgayThu       = DateTime.Now,
                TenKH         = _tenKH,
                SDT           = _sdt,
                KyThu         = _kyThu,
                TongSoKy      = _tongSoKy,
                NgayDenHan    = _ngayDenHan,
                TienGocKy     = _tienGocKy,
                SoNgayTre     = _soNgayTre,
                TienPhat      = _tienPhat,
                TongThucThu   = _tongPhaiThu,
                PhuongThucThu = cboPhuongThuc.SelectedItem?.ToString() ?? "Chuyển khoản ngân hàng (Mã QR VietQR)",
                TienKhachDua  = khachDua,
                TienThua      = tienThua,
                TenNV         = Function.CurrentTenNV ?? "Trần Thị Bình",
                MaNV          = Function.CurrentMaNV ?? "NV002"
            };

            // Mở cửa sổ xem trước phiếu thu kèm QR Code
            using (var preview = new frmPrintPreview(phieuThu))
            {
                preview.ShowDialog(this);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

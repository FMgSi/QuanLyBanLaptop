using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.DanhMuc;

namespace QuanLyBanLaptop
{
    public partial class frmDangNhap : QuanLyBanLaptop.Classes.AppForm
    {
        public frmDangNhap()
        {
            InitializeComponent();

            picLogo.Image = IconHelper.CreateLaptopLogo(64);

            chkHienMatKhau.CheckedChanged += (s, e) =>
            {
                txtMatKhau.PasswordChar = chkHienMatKhau.Checked ? '\0' : '*';
            };

            btnDangNhap.Click += BtnDangNhap_Click;
            btnThoat.Click += (s, e) => this.Close();

            this.FormClosing += (s, e) =>
            {
                if (!_isLoggingIn && e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult dr = MessageBox.Show(
                        "Bạn có chắc chắn muốn thoát khỏi chương trình?",
                        "Xác nhận thoát",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (dr != DialogResult.Yes)
                    {
                        e.Cancel = true;
                    }
                }
            };

            txtTenDN.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtMatKhau.Focus();
                    e.SuppressKeyPress = true;
                }
            };

            txtMatKhau.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    BtnDangNhap_Click(s, e);
                    e.SuppressKeyPress = true;
                }
            };
        }

        private bool _isLoggingIn = false;
        private void BtnDangNhap_Click(object sender, EventArgs e)
        {
            lblThongBao.Visible = false;

            string tenDN = txtTenDN.Text.Trim();
            string matKhau = txtMatKhau.Text;

            if (string.IsNullOrEmpty(tenDN))
            {
                lblThongBao.Text = "Vui lòng nhập tên đăng nhập!";
                lblThongBao.Visible = true;
                txtTenDN.Focus();
                return;
            }

            if (string.IsNullOrEmpty(matKhau))
            {
                lblThongBao.Text = "Vui lòng nhập mật khẩu!";
                lblThongBao.Visible = true;
                txtMatKhau.Focus();
                return;
            }

            DataRow r;
            try { r = DataProcesser.DangNhap(tenDN, matKhau); }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); return; }
            if (r != null)
            {
                UserSession.MaNV = r["MaNV"].ToString();
                UserSession.TenNV = r["TenNV"].ToString();
                UserSession.ChucVu = r["ChucVu"].ToString();
                UserSession.TenDN = r["TenDN"].ToString();

                _isLoggingIn = true;
                frmSanPham.userName = r["TenDN"].ToString();

                if (this.Modal)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    frmTrangChu trangChu = new frmTrangChu();
                    trangChu.WindowState = FormWindowState.Maximized;
                    trangChu.FormClosed += (s, ev) => Application.Exit();
                    this.Hide();
                    trangChu.Show();
                }
            }
            else
            {
                lblThongBao.Text = "Tên đăng nhập hoặc mật khẩu không chính xác!";
                lblThongBao.Visible = true;
                txtMatKhau.SelectAll();
                txtMatKhau.Focus();
            }
        }
        private void pnlRight_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}

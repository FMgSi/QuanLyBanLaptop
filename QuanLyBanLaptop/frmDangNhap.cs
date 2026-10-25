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
    public partial class frmDangNhap : Form
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
            string matKhau = txtMatKhau.Text.Trim();

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

            // Truy vấn thông tin tài khoản theo tên đăng nhập
            string sql = "SELECT TOP 1 * FROM NHAN_VIEN WHERE TenDN = @u";
            DataTable dt = DataProcesser.ReadData(sql, new SqlParameter("@u", tenDN));

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                string storedPass = r["MatKhau"] != DBNull.Value ? r["MatKhau"].ToString() : "";

                if (!VerifyPassword(matKhau, storedPass))
                {
                    lblThongBao.Text = "Tên đăng nhập hoặc mật khẩu không chính xác!";
                    lblThongBao.Visible = true;
                    txtMatKhau.SelectAll();
                    txtMatKhau.Focus();
                    return;
                }

                // Kiểm tra trạng thái tài khoản (hỗ trợ cả cột DaNghi và DangLamViec)
                if (dt.Columns.Contains("DaNghi") && r["DaNghi"] != DBNull.Value && Convert.ToBoolean(r["DaNghi"]))
                {
                    lblThongBao.Text = "Tài khoản này đã bị khóa hoặc đã nghỉ việc!";
                    lblThongBao.Visible = true;
                    return;
                }
                if (dt.Columns.Contains("DangLamViec") && r["DangLamViec"] != DBNull.Value && !Convert.ToBoolean(r["DangLamViec"]))
                {
                    lblThongBao.Text = "Tài khoản này đã bị khóa hoặc đã nghỉ việc!";
                    lblThongBao.Visible = true;
                    return;
                }

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

        private static bool VerifyPassword(string inputPassword, string storedPassword)
        {
            if (string.IsNullOrEmpty(storedPassword)) return false;
            if (storedPassword == inputPassword) return true; // Plain text
            if (string.Equals(storedPassword, ComputeSha256(inputPassword), StringComparison.OrdinalIgnoreCase)) return true; // Direct SHA256

            // Kiểm tra salt$hash (theo chuẩn LibraryManagerWF hoặc sp_DatMatKhau)
            int idx = storedPassword.IndexOf('$');
            if (idx > 0)
            {
                string salt = storedPassword.Substring(0, idx);
                string hashPart = storedPassword.Substring(idx + 1);

                using (var sha = SHA256.Create())
                {
                    // 1. Thử UTF-8 (LibraryManagerWF)
                    byte[] bytesUtf8 = sha.ComputeHash(Encoding.UTF8.GetBytes(salt + inputPassword));
                    string hashUtf8 = BitConverter.ToString(bytesUtf8).Replace("-", "");
                    if (string.Equals(hashPart, hashUtf8, StringComparison.OrdinalIgnoreCase)) return true;

                    // 2. Thử Unicode / UTF-16 (SQL Server HASHBYTES)
                    byte[] bytesUnicode = sha.ComputeHash(Encoding.Unicode.GetBytes(salt + inputPassword));
                    string hashUnicode = BitConverter.ToString(bytesUnicode).Replace("-", "");
                    if (string.Equals(hashPart, hashUnicode, StringComparison.OrdinalIgnoreCase)) return true;
                }
            }
            return false;
        }

        private static string ComputeSha256(string raw)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private void pnlRight_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}

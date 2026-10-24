using System;
using System.Data;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();

            // Cho phép nhấn Enter ở ô mật khẩu để đăng nhập
            txtMatKhau.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) btnDangNhap_Click(s, e);
            };
            txtTenDangNhap.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) txtMatKhau.Focus();
            };
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenDN   = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text;

            // Validate cơ bản
            if (string.IsNullOrEmpty(tenDN))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return;
            }
            if (string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return;
            }

            try
            {
                DataRow row = DataProcesser.DangNhap(tenDN, matKhau);

                if (row == null)
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.\nVui lòng kiểm tra lại.",
                        "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Clear();
                    txtMatKhau.Focus();
                    return;
                }

                // Lưu thông tin phiên đăng nhập
                Function.SetCurrentUser(row);

                // Mở màn hình chính
                var trangChu = new frmTrangChu();
                trangChu.Show();
                this.Hide();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show($"Lỗi kết nối cơ sở dữ liệu:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDangNhap_Click_1(object sender, EventArgs e)
            => btnDangNhap_Click(sender, e);
    }
}

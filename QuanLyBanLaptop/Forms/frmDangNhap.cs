using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.Helpers;

namespace QuanLyBanLaptop
{
    public partial class frmDangNhap : Form
    {
        private bool _isClosingConfirmed = false;

        public frmDangNhap()
        {
            InitializeComponent();
            picLogo.Image = IconHelper.CreateLaptopLogo(150);
            ActiveControl = txtTenDN;
        }

        // Bật / tắt hiển thị ký tự mật khẩu
        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.PasswordChar = chkHienMatKhau.Checked ? '\0' : '*';
        }

        private async void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenDN = txtTenDN.Text.Trim();
            string matKhau = txtMatKhau.Text;

            if (string.IsNullOrEmpty(tenDN))
            {
                MessageHelper.Warning("Vui lòng nhập tên đăng nhập.");
                txtTenDN.Focus();
                return;
            }

            if (string.IsNullOrEmpty(matKhau))
            {
                MessageHelper.Warning("Vui lòng nhập mật khẩu.");
                txtMatKhau.Focus();
                return;
            }

            // Chuyển sang trạng thái đang xử lý để giao diện mượt mà, không bị freeze
            btnDangNhap.Enabled = false;
            btnThoat.Enabled = false;
            btnDangNhap.Text = "ĐANG XỬ LÝ...";
            Cursor = Cursors.WaitCursor;

            try
            {
                // Thực thi xác thực bất đồng bộ trong background thread
                bool isSuccess = await Task.Run(() => AuthService.DangNhap(tenDN, matKhau));

                if (!isSuccess)
                {
                    MessageHelper.Error("Tên đăng nhập hoặc mật khẩu không chính xác.");
                    txtMatKhau.SelectAll();
                    txtMatKhau.Focus();
                    return;
                }

                // Đăng nhập thành công -> Đánh dấu đóng form và đóng an toàn
                _isClosingConfirmed = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (SqlException ex)
            {
                MessageHelper.Error("Lỗi kết nối cơ sở dữ liệu SQL Server LocalDB:\n" + ex.Message +
                    "\n\nBạn có thể khởi động lại LocalDB bằng lệnh:\nsqllocaldb start MSSQLLocalDB");
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Đã xảy ra lỗi khi đăng nhập:\n" + ex.Message);
            }
            finally
            {
                // Khôi phục trạng thái nút nếu form chưa đóng
                if (!IsDisposed && IsHandleCreated)
                {
                    btnDangNhap.Enabled = true;
                    btnThoat.Enabled = true;
                    btnDangNhap.Text = "ĐĂNG NHẬP";
                    Cursor = Cursors.Default;
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageHelper.Confirm("Bạn có muốn thoát chương trình?"))
            {
                _isClosingConfirmed = true;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void frmDangNhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Nếu thoát bằng cách bấm dấu X trên góc cửa sổ
            if (DialogResult != DialogResult.OK && !_isClosingConfirmed && e.CloseReason == CloseReason.UserClosing)
            {
                if (!MessageHelper.Confirm("Bạn có muốn thoát chương trình?"))
                {
                    e.Cancel = true;
                    return;
                }
                _isClosingConfirmed = true;
            }
        }
    }
}

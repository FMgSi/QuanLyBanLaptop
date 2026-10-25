using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop
{
    public partial class frmMain : Form
    {
        public static frmMain Instance { get; private set; }

        public frmMain()
        {
            Instance = this;
            InitializeComponent();

            DoubleBuffered = true;
            SetDoubleBuffered(pnlNoiDung);
            SetDoubleBuffered(pnlHeader);
            SetDoubleBuffered(sidebarControl1);

            pnlHeader.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(226, 232, 240)))
                {
                    e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
                }
            };

            SetupEvents();

            NavigationManager.InitHost(this, pnlNoiDung, sidebarControl1, lblTrang, lblNguoiDung);
        }

        private static void SetDoubleBuffered(Control control)
        {
            if (control == null) return;
            typeof(Control).InvokeMember(
                "DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void SetupEvents()
        {
            this.Load += FrmMain_Load;
            this.FormClosing += FrmMain_FormClosing;

            btnDangXuat.Click += BtnDangXuat_Click;
            lblNguoiDung.Click += LblNguoiDung_Click;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Tải thông tin người dùng phiên làm việc mặc định nếu chưa đăng nhập
            if (string.IsNullOrEmpty(UserSession.MaNV))
            {
                try
                {
                    DataTable dt = Function.DocBang("SELECT TOP 1 MaNV, TenNV, ChucVu FROM NHAN_VIEN WHERE DaNghi = 0 ORDER BY MaNV");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        UserSession.MaNV = dt.Rows[0]["MaNV"]?.ToString() ?? "NV001";
                        UserSession.TenNV = dt.Rows[0]["TenNV"]?.ToString() ?? "Nguyễn Văn An";
                        UserSession.ChucVu = dt.Rows[0]["ChucVu"]?.ToString() ?? "Quản lý";
                    }
                    else
                    {
                        UserSession.MaNV = "NV001";
                        UserSession.TenNV = "Nguyễn Văn An";
                        UserSession.ChucVu = "Quản lý";
                    }
                }
                catch
                {
                    UserSession.MaNV = "NV001";
                    UserSession.TenNV = "Nguyễn Văn An";
                    UserSession.ChucVu = "Quản trị viên";
                }
            }

            NavigationManager.UpdateUserInfo();
            NavigationManager.Navigate(NavigationMenu.TrangChu);
        }

        private void LblNguoiDung_Click(object sender, EventArgs e)
        {
            // Mở hộp thoại đăng nhập để đổi tài khoản nếu muốn
            using (var login = new frmDangNhap())
            {
                if (login.ShowDialog(this) == DialogResult.OK)
                {
                    NavigationManager.UpdateUserInfo();
                }
            }
        }

        private void BtnDangXuat_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Bạn có muốn đăng xuất tài khoản hiện tại không?",
                "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                UserSession.MaNV = "";
                UserSession.TenNV = "";
                UserSession.ChucVu = "";

                using (var login = new frmDangNhap())
                {
                    if (login.ShowDialog(this) == DialogResult.OK)
                    {
                        NavigationManager.UpdateUserInfo();
                        NavigationManager.Navigate(NavigationMenu.TrangChu);
                    }
                    else
                    {
                        // Nếu hủy đăng nhập, giữ phiên cũ hoặc đóng nếu không còn user
                        if (string.IsNullOrEmpty(UserSession.MaNV))
                        {
                            Application.Exit();
                        }
                    }
                }
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var dr = MessageBox.Show("Bạn có muốn thoát chương trình không?",
                    "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

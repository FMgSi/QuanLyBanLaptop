using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
namespace QuanLyBanLaptop
{
    public partial class frmMain : AppForm
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
                    e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };
            Load += FrmMain_Load;
            FormClosing += FrmMain_FormClosing;
            btnDangXuat.Click += BtnDangXuat_Click;
            lblNguoiDung.Click += LblNguoiDung_Click;
            NavigationManager.InitHost(this, pnlNoiDung, sidebarControl1, lblTrang, lblNguoiDung);
            Disposed += (s, e) => NavigationManager.InitHost(null, null, null);
        }
        private static void SetDoubleBuffered(Control c) =>
            c?.GetType().InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, c, new object[] { true });
        private void FrmMain_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            if (string.IsNullOrWhiteSpace(UserSession.MaNV)) { Close(); return; }
            NavigationManager.UpdateUserInfo();
            NavigationManager.Navigate(NavigationMenu.TrangChu);
        }
        private void LblNguoiDung_Click(object sender, EventArgs e)
        {
            using (var login = new frmDangNhap())
                if (login.ShowDialog(this) == DialogResult.OK)
                {
                    NavigationManager.UpdateUserInfo();
                    NavigationManager.Navigate(NavigationMenu.TrangChu);
                }
        }
        private void BtnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn đăng xuất tài khoản hiện tại không?", "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            UserSession.Logout();
            using (var login = new frmDangNhap())
                if (login.ShowDialog(this) == DialogResult.OK)
                {
                    NavigationManager.UpdateUserInfo();
                    NavigationManager.Navigate(NavigationMenu.TrangChu);
                }
                else if (string.IsNullOrEmpty(UserSession.MaNV)) Application.Exit();
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing &&
                MessageBox.Show("Bạn có muốn thoát chương trình không?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                e.Cancel = true;
        }
    }
}

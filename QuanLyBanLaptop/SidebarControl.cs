using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
namespace QuanLyBanLaptop
{
    public partial class SidebarControl : UserControl
    {
        private NavigationMenu _selectedMenu = NavigationMenu.TrangChu;
        private bool _isCollapsed;
        private int _overdueCount;
        private const int EXPANDED_WIDTH = 230, COLLAPSED_WIDTH = 64, STEP_WIDTH = 25;
        private readonly Button btnCauHinh = new Button();
        [Description("The currently selected menu item on the sidebar")]
        [DefaultValue(NavigationMenu.TrangChu)]
        public NavigationMenu SelectedMenu
        {
            get => _selectedMenu;
            set { _selectedMenu = value; UpdateMenuStyles(); }
        }
        public SidebarControl()
        {
            InitializeComponent();
            btnCauHinh.Name = "btnCauHinh";
            btnCauHinh.Text = "   Cấu Hình";
            btnCauHinh.Tag = NavigationMenu.CauHinh;
            btnCauHinh.Dock = DockStyle.Top;
            btnCauHinh.Height = btnThongKe.Height;
            btnCauHinh.TextAlign = ContentAlignment.MiddleLeft;
            btnCauHinh.Padding = btnThongKe.Padding;
            pnlMenu.Controls.Add(btnCauHinh);
            pnlMenu.Controls.SetChildIndex(btnCauHinh, 0);
            pnlMenu.AutoScroll = true;
            DoubleBuffered = true;
            SetDoubleBuffered(pnlLogo);
            SetDoubleBuffered(pnlMenu);
            ApplyThemeAndIcons();
            SetupEvents();
            UserSession.Changed += RefreshPermissions;
            Disposed += (s, e) => UserSession.Changed -= RefreshPermissions;
            RefreshPermissions(this, EventArgs.Empty);
        }
        private void RefreshPermissions(object sender, EventArgs args) =>
            btnNhanVien.Visible = btnThongKe.Visible = btnCauHinh.Visible =
            btnNhanVien.Enabled = btnThongKe.Enabled = btnCauHinh.Enabled = UserSession.IsAdmin;
        private static void SetDoubleBuffered(Control c) =>
            typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, c, new object[] { true });
        private void ApplyThemeAndIcons()
        {
            BackColor = pnlLogo.BackColor = pnlMenu.BackColor = SidebarTheme.SidebarBack;
            picLogo.Image = IconHelper.CreateLaptopLogo(48);
            btnTrangChu.Image = IconHelper.CreateHomeIcon(20);
            btnQuanLyLaptop.Image = IconHelper.CreateLaptopIcon(20);
            btnBanHang.Image = IconHelper.CreateCartIcon(20);
            btnNhapHang.Image = IconHelper.CreateImportCartIcon(20);
            btnNhanVien.Image = IconHelper.CreateUserIcon(20);
            btnKhachHang.Image = IconHelper.CreateCustomerIcon(20);
            btnTheoDoiTraGop.Image = IconHelper.CreateInstallmentIcon(20);
            btnBaoHanhSuaChua.Image = IconHelper.CreateWrenchIcon(20);
            btnThongKe.Image = IconHelper.CreateChartIcon(20);
            btnCauHinh.Image = IconHelper.CreateSettingsIcon(20);
            btnCauHinh.Cursor = Cursors.Hand;
            btnCauHinh.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCauHinh.ImageAlign = ContentAlignment.MiddleLeft;
            btnTrangChu.Tag = NavigationMenu.TrangChu;
            btnQuanLyLaptop.Tag = NavigationMenu.QuanLyLaptop;
            btnBanHang.Tag = NavigationMenu.BanHang;
            btnNhapHang.Tag = NavigationMenu.NhapHang;
            btnNhanVien.Tag = NavigationMenu.NhanVien;
            btnKhachHang.Tag = NavigationMenu.KhachHang;
            btnTheoDoiTraGop.Tag = NavigationMenu.TheoDoiTraGop;
            btnBaoHanhSuaChua.Tag = NavigationMenu.BaoHanhSuaChua;
            btnThongKe.Tag = NavigationMenu.ThongKe;
            btnCauHinh.Tag = NavigationMenu.CauHinh;
            new ToolTip().SetToolTip(picLogo, "Bấm để thu gọn / mở rộng menu");
            btnTheoDoiTraGop.Paint += (s, e) =>
            {
                if (_overdueCount <= 0 || _isCollapsed) return;
                string txt = _overdueCount > 99 ? "99+" : _overdueCount.ToString();
                int sz = 20, bx = btnTheoDoiTraGop.Width - sz - 16, by = (btnTheoDoiTraGop.Height - sz) / 2;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(Color.FromArgb(239, 68, 68))) e.Graphics.FillEllipse(br, bx, by, sz, sz);
                using (var f = new Font("Segoe UI", 8.2f, FontStyle.Bold))
                using (var tb = new SolidBrush(Color.White))
                {
                    var m = e.Graphics.MeasureString(txt, f);
                    e.Graphics.DrawString(txt, f, tb, bx + (sz - m.Width) / 2f + 0.5f, by + (sz - m.Height) / 2f + 0.5f);
                }
            };
            RefreshOverdueBadge();
            UpdateMenuStyles();
        }
        public void RefreshOverdueBadge()
        {
            try
            {
                object v = DataProcesser.ExecuteScalarSilent("SELECT COUNT(DISTINCT MaHD) FROM LICH_TRA_GOP WHERE NgayTra IS NULL AND NgayDenHan < CAST(GETDATE() AS DATE)");
                _overdueCount = (v != null && v != DBNull.Value) ? Convert.ToInt32(v) : 0;
                btnTheoDoiTraGop.Invalidate();
            }
            catch { }
        }
        private void SetupEvents()
        {
            picLogo.Click += (s, e) => ToggleSidebar();
            foreach (Control c in pnlMenu.Controls)
                if (c is Button btn)
                {
                    btn.Click += (s, e) =>
                    {
                        if (!(s is Button b) || !(b.Tag is NavigationMenu m) || m == _selectedMenu) return;
                        _selectedMenu = m;
                        UpdateMenuStyles();
                        NavigationManager.Navigate(FindForm(), m);
                    };
                    btn.MouseEnter += (s, e) => { if (s is Button b && b.Tag is NavigationMenu m && m != _selectedMenu) b.BackColor = SidebarTheme.SidebarHover; };
                    btn.MouseLeave += (s, e) => { if (s is Button b && b.Tag is NavigationMenu m) b.BackColor = m == _selectedMenu ? SidebarTheme.SidebarActive : SidebarTheme.SidebarBack; };
                }
            tmrCollapse.Interval = 10;
            tmrCollapse.Tick += TmrCollapse_Tick;
        }
        public void UpdateMenuStyles()
        {
            foreach (Control c in pnlMenu.Controls)
                if (c is Button btn && btn.Tag is NavigationMenu menu)
                {
                    bool sel = menu == _selectedMenu;
                    btn.UseVisualStyleBackColor = false;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.BackColor = sel ? SidebarTheme.SidebarActive : SidebarTheme.SidebarBack;
                    btn.ForeColor = SidebarTheme.TextWhite;
                    btn.Font = sel ? SidebarTheme.MenuActiveFont : SidebarTheme.MenuFont;
                    btn.FlatAppearance.MouseDownBackColor = SidebarTheme.SidebarActive;
                    btn.FlatAppearance.MouseOverBackColor = SidebarTheme.SidebarHover;
                }
        }
        public void ToggleSidebar() => tmrCollapse.Start();
        private void TmrCollapse_Tick(object sender, EventArgs e)
        {
            if (_isCollapsed)
            {
                Width += STEP_WIDTH;
                if (Width >= EXPANDED_WIDTH)
                {
                    Width = EXPANDED_WIDTH;
                    _isCollapsed = false;
                    tmrCollapse.Stop();
                    SetButtonTextsVisible(true);
                    picLogo.Location = new Point((EXPANDED_WIDTH - picLogo.Width) / 2, 14);
                }
            }
            else
            {
                SetButtonTextsVisible(false);
                Width -= STEP_WIDTH;
                if (Width <= COLLAPSED_WIDTH)
                {
                    Width = COLLAPSED_WIDTH;
                    _isCollapsed = true;
                    tmrCollapse.Stop();
                    picLogo.Location = new Point((COLLAPSED_WIDTH - picLogo.Width) / 2, 14);
                }
            }
        }
        private void SetButtonTextsVisible(bool v)
        {
            btnTrangChu.Text = v ? "   Trang Chủ" : "";
            btnQuanLyLaptop.Text = v ? "   Quản Lý Laptop" : "";
            btnBanHang.Text = v ? "   Bán Hàng" : "";
            btnNhapHang.Text = v ? "   Nhập Hàng" : "";
            btnNhanVien.Text = v ? "   Nhân Viên" : "";
            btnKhachHang.Text = v ? "   Khách Hàng" : "";
            btnTheoDoiTraGop.Text = v ? "   Theo Dõi Trả Góp" : "";
            btnBaoHanhSuaChua.Text = v ? "   Sửa Chữa Bảo Hành" : "";
            btnThongKe.Text = v ? "   Thống Kê" : "";
            btnCauHinh.Text = v ? "   Cấu Hình" : "";
            int p = v ? 16 : 18;
            foreach (Control c in pnlMenu.Controls)
                if (c is Button b) b.Padding = new Padding(p, 0, 0, 0);
        }
    }
}

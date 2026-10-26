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
        private bool _isCollapsed = false;
        private int _overdueCount = 0;
        private const int EXPANDED_WIDTH = 230;
        private const int COLLAPSED_WIDTH = 64;
        private const int STEP_WIDTH = 25;

        [Description("The currently selected menu item on the sidebar")]
        [DefaultValue(NavigationMenu.TrangChu)]
        public NavigationMenu SelectedMenu
        {
            get => _selectedMenu;
            set
            {
                _selectedMenu = value;
                UpdateMenuStyles();
            }
        }

        // Thực hiện SidebarControl theo trạng thái và dữ liệu hiện tại.
        public SidebarControl()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetDoubleBuffered(pnlLogo);
            SetDoubleBuffered(pnlMenu);

            ApplyThemeAndIcons();
            SetupEvents();
        }

        // Thực hiện SetDoubleBuffered theo trạng thái và dữ liệu hiện tại.
        private static void SetDoubleBuffered(Control control)
        {
            typeof(Control).InvokeMember(
                "DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        // Thực hiện ApplyThemeAndIcons theo trạng thái và dữ liệu hiện tại.
        private void ApplyThemeAndIcons()
        {
            BackColor = SidebarTheme.SidebarBack;
            pnlLogo.BackColor = SidebarTheme.SidebarBack;
            pnlMenu.BackColor = SidebarTheme.SidebarBack;

            // Load vector-drawn crisp icons
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

            // Button tag mapping
            btnTrangChu.Tag = NavigationMenu.TrangChu;
            btnQuanLyLaptop.Tag = NavigationMenu.QuanLyLaptop;
            btnBanHang.Tag = NavigationMenu.BanHang;
            btnNhapHang.Tag = NavigationMenu.NhapHang;
            btnNhanVien.Tag = NavigationMenu.NhanVien;
            btnKhachHang.Tag = NavigationMenu.KhachHang;
            btnTheoDoiTraGop.Tag = NavigationMenu.TheoDoiTraGop;
            btnBaoHanhSuaChua.Tag = NavigationMenu.BaoHanhSuaChua;
            btnThongKe.Tag = NavigationMenu.ThongKe;

            // Common button appearance
            foreach (Control c in pnlMenu.Controls)
            {
                if (c is Button btn)
                {
                    btn.UseVisualStyleBackColor = false;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.ForeColor = SidebarTheme.TextWhite;
                    btn.Font = SidebarTheme.MenuFont;
                    btn.BackColor = SidebarTheme.SidebarBack;
                    btn.FlatAppearance.MouseDownBackColor = SidebarTheme.SidebarActive;
                    btn.FlatAppearance.MouseOverBackColor = SidebarTheme.SidebarHover;
                }
            }

            var tip = new ToolTip();
            tip.SetToolTip(picLogo, "Bấm để thu gọn / mở rộng menu");

            // Vẽ badge đỏ cảnh báo kỳ quá hạn trên nút Theo Dõi Trả Góp
            btnTheoDoiTraGop.Paint += (s, e) =>
            {
                if (_overdueCount > 0 && !_isCollapsed)
                {
                    string badgeText = _overdueCount > 99 ? "99+" : _overdueCount.ToString();
                    int badgeSize = 20;
                    int badgeX = btnTheoDoiTraGop.Width - badgeSize - 16;
                    int badgeY = (btnTheoDoiTraGop.Height - badgeSize) / 2;

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using (var brush = new SolidBrush(Color.FromArgb(239, 68, 68))) // Red
                    {
                        e.Graphics.FillEllipse(brush, badgeX, badgeY, badgeSize, badgeSize);
                    }
                    using (var font = new Font("Segoe UI", 8.2f, FontStyle.Bold))
                    using (var textBrush = new SolidBrush(Color.White))
                    {
                        var sz = e.Graphics.MeasureString(badgeText, font);
                        e.Graphics.DrawString(badgeText, font, textBrush,
                            badgeX + (badgeSize - sz.Width) / 2f + 0.5f,
                            badgeY + (badgeSize - sz.Height) / 2f + 0.5f);
                    }
                }
            };

            RefreshOverdueBadge();
            UpdateMenuStyles();
        }

        // Thực hiện RefreshOverdueBadge theo trạng thái và dữ liệu hiện tại.
        public void RefreshOverdueBadge()
        {
            try
            {
                object countObj = DataProcesser.ExecuteScalarSilent(
                    "SELECT COUNT(DISTINCT MaHD) FROM LICH_TRA_GOP WHERE NgayTra IS NULL AND NgayDenHan < CAST(GETDATE() AS DATE)");
                _overdueCount = (countObj != null && countObj != DBNull.Value) ? Convert.ToInt32(countObj) : 0;
                btnTheoDoiTraGop.Invalidate();
            }
            catch { }
        }

        // Khởi tạo cấu hình và liên kết cho SetupEvents.
        private void SetupEvents()
        {
            picLogo.Click += (s, e) => ToggleSidebar();

            foreach (Control c in pnlMenu.Controls)
            {
                if (c is Button btn)
                {
                    btn.Click += MenuButton_Click;
                    btn.MouseEnter += MenuButton_MouseEnter;
                    btn.MouseLeave += MenuButton_MouseLeave;
                }
            }

            tmrCollapse.Interval = 10;
            tmrCollapse.Tick += TmrCollapse_Tick;
        }

        // Xử lý sự kiện MenuButton_Click của giao diện.
        private void MenuButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is NavigationMenu menu)
            {
                if (menu == _selectedMenu) return;

                _selectedMenu = menu;
                UpdateMenuStyles();

                Form parentForm = FindForm();
                NavigationManager.Navigate(parentForm, menu);
            }
        }

        // Xử lý sự kiện MenuButton_MouseEnter của giao diện.
        private void MenuButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is NavigationMenu menu)
            {
                if (menu != _selectedMenu)
                {
                    btn.BackColor = SidebarTheme.SidebarHover;
                }
            }
        }

        // Xử lý sự kiện MenuButton_MouseLeave của giao diện.
        private void MenuButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is NavigationMenu menu)
            {
                if (menu != _selectedMenu)
                {
                    btn.BackColor = SidebarTheme.SidebarBack;
                }
                else
                {
                    btn.BackColor = SidebarTheme.SidebarActive;
                }
            }
        }

        // Thực hiện UpdateMenuStyles theo trạng thái và dữ liệu hiện tại.
        public void UpdateMenuStyles()
        {
            foreach (Control c in pnlMenu.Controls)
            {
                if (c is Button btn && btn.Tag is NavigationMenu menu)
                {
                    bool isSelected = (menu == _selectedMenu);
                    btn.UseVisualStyleBackColor = false;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.BackColor = isSelected ? SidebarTheme.SidebarActive : SidebarTheme.SidebarBack;
                    btn.ForeColor = SidebarTheme.TextWhite;
                    btn.Font = isSelected ? SidebarTheme.MenuActiveFont : SidebarTheme.MenuFont;
                    btn.FlatAppearance.MouseDownBackColor = SidebarTheme.SidebarActive;
                    btn.FlatAppearance.MouseOverBackColor = SidebarTheme.SidebarHover;
                }
            }
        }

        // Thực hiện ToggleSidebar theo trạng thái và dữ liệu hiện tại.
        public void ToggleSidebar()
        {
            tmrCollapse.Start();
        }

        // Xử lý sự kiện TmrCollapse_Tick của giao diện.
        private void TmrCollapse_Tick(object sender, EventArgs e)
        {
            if (_isCollapsed)
            {
                // Expanding
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
                // Collapsing
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

        // Thực hiện SetButtonTextsVisible theo trạng thái và dữ liệu hiện tại.
        private void SetButtonTextsVisible(bool visible)
        {
            btnTrangChu.Text = visible ? "   Trang Chủ" : "";
            btnQuanLyLaptop.Text = visible ? "   Quản Lý Laptop" : "";
            btnBanHang.Text = visible ? "   Bán Hàng" : "";
            btnNhapHang.Text = visible ? "   Nhập Hàng" : "";
            btnNhanVien.Text = visible ? "   Nhân Viên" : "";
            btnKhachHang.Text = visible ? "   Khách Hàng" : "";
            btnTheoDoiTraGop.Text = visible ? "   Theo Dõi Trả Góp" : "";
            btnBaoHanhSuaChua.Text = visible ? "   Sửa Chữa Bảo Hành" : "";
            btnThongKe.Text = visible ? "   Thống Kê" : "";

            int paddingLeft = visible ? 16 : 18;
            foreach (Control c in pnlMenu.Controls)
            {
                if (c is Button b)
                {
                    b.Padding = new Padding(paddingLeft, 0, 0, 0);
                }
            }
        }
    }
}

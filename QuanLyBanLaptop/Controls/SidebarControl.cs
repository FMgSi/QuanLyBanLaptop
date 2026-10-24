using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.Helpers;

namespace QuanLyBanLaptop
{
    /// <summary>
    /// Thanh menu bên trái. Control này chỉ lo phần giao diện (màu, icon, thu gọn/mở rộng)
    /// và báo ra ngoài bằng sự kiện <see cref="MenuSelected"/>; việc chuyển trang do frmMain đảm nhiệm.
    /// </summary>
    public partial class SidebarControl : UserControl
    {
        private const int EXPANDED_WIDTH = 175;
        private const int COLLAPSED_WIDTH = 56;
        private const int STEP_WIDTH = 30;

        /// <summary>Thông tin của một mục menu: nút, tiêu đề và icon.</summary>
        private class SidebarItem
        {
            public NavigationMenu Menu;
            public Button Button;
            public string Title;
        }

        private SidebarItem[] _items;
        private NavigationMenu _selectedMenu = NavigationMenu.TrangChu;
        private bool _isCollapsed;

        /// <summary>Phát ra khi người dùng bấm vào một mục menu khác mục đang chọn.</summary>
        public event Action<NavigationMenu> MenuSelected;

        [Description("Mục menu đang được chọn trên sidebar")]
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

        public SidebarControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ControlHelper.SetDoubleBuffered(pnlLogo, pnlMenu);

            // Bảng khai báo menu: muốn thêm mục mới chỉ cần thêm 1 dòng ở đây.
            _items = new[]
            {
                new SidebarItem { Menu = NavigationMenu.TrangChu,     Button = btnTrangChu,     Title = "Trang Chủ" },
                new SidebarItem { Menu = NavigationMenu.QuanLyLaptop, Button = btnQuanLyLaptop, Title = "Quản Lý Laptop" },
                new SidebarItem { Menu = NavigationMenu.BanHang,      Button = btnBanHang,      Title = "Bán Hàng" },
                new SidebarItem { Menu = NavigationMenu.NhapHang,     Button = btnNhapHang,     Title = "Nhập Hàng" },
                new SidebarItem { Menu = NavigationMenu.NhanVien,     Button = btnNhanVien,     Title = "Nhân Viên" },
                new SidebarItem { Menu = NavigationMenu.ThongKe,      Button = btnThongKe,      Title = "Thống Kê" },
            };

            ApplyThemeAndIcons();
            SetupEvents();
        }

        private void ApplyThemeAndIcons()
        {
            BackColor = pnlLogo.BackColor = pnlMenu.BackColor = SidebarTheme.SidebarBack;

            picLogo.Image = IconHelper.CreateLaptopLogo(48);
            btnTrangChu.Image = IconHelper.CreateHomeIcon(20);
            btnQuanLyLaptop.Image = IconHelper.CreateLaptopIcon(20);
            btnBanHang.Image = IconHelper.CreateCartIcon(20);
            btnNhapHang.Image = IconHelper.CreateImportCartIcon(20);
            btnNhanVien.Image = IconHelper.CreateUserIcon(20);
            btnThongKe.Image = IconHelper.CreateChartIcon(20);

            foreach (SidebarItem item in _items)
                item.Button.Tag = item.Menu;

            ControlHelper.ForEachButton(pnlMenu, btn =>
            {
                btn.ForeColor = SidebarTheme.TextWhite;
                btn.Font = SidebarTheme.MenuFont;
                btn.BackColor = SidebarTheme.SidebarBack;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseDownBackColor = SidebarTheme.SidebarActive;
                btn.FlatAppearance.MouseOverBackColor = SidebarTheme.SidebarHover;
            });

            new ToolTip().SetToolTip(picLogo, "Bấm để thu gọn / mở rộng menu");
            UpdateMenuStyles();
        }

        private void SetupEvents()
        {
            picLogo.Click += (s, e) => ToggleSidebar();

            ControlHelper.ForEachButton(pnlMenu, btn =>
            {
                btn.Click += MenuButton_Click;
                btn.MouseEnter += (s, e) => SetHover((Button)s, true);
                btn.MouseLeave += (s, e) => SetHover((Button)s, false);
            });

            tmrCollapse.Interval = 10;
            tmrCollapse.Tick += TmrCollapse_Tick;
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            var menu = (NavigationMenu)((Button)sender).Tag;
            if (menu == _selectedMenu) return;

            SelectedMenu = menu;
            MenuSelected?.Invoke(menu);
        }

        /// <summary>Đổi màu khi rê chuột, trừ mục đang được chọn.</summary>
        private void SetHover(Button btn, bool hover)
        {
            if ((NavigationMenu)btn.Tag == _selectedMenu) return;
            btn.BackColor = hover ? SidebarTheme.SidebarHover : SidebarTheme.SidebarBack;
        }

        public void UpdateMenuStyles()
        {
            if (_items == null) return;

            foreach (SidebarItem item in _items)
            {
                bool isSelected = item.Menu == _selectedMenu;
                item.Button.BackColor = isSelected ? SidebarTheme.SidebarActive : SidebarTheme.SidebarBack;
                item.Button.Font = isSelected ? SidebarTheme.MenuActiveFont : SidebarTheme.MenuFont;
            }
        }

        public void ToggleSidebar() => tmrCollapse.Start();

        private void TmrCollapse_Tick(object sender, EventArgs e)
        {
            if (_isCollapsed)
            {
                Width += STEP_WIDTH;
                if (Width >= EXPANDED_WIDTH)
                    FinishAnimation(EXPANDED_WIDTH, false);
            }
            else
            {
                SetButtonTextsVisible(false);
                Width -= STEP_WIDTH;
                if (Width <= COLLAPSED_WIDTH)
                    FinishAnimation(COLLAPSED_WIDTH, true);
            }
        }

        private void FinishAnimation(int finalWidth, bool collapsed)
        {
            Width = finalWidth;
            _isCollapsed = collapsed;
            tmrCollapse.Stop();
            if (!collapsed) SetButtonTextsVisible(true);
            picLogo.Location = new Point((finalWidth - picLogo.Width) / 2, 14);
        }

        private void SetButtonTextsVisible(bool visible)
        {
            foreach (SidebarItem item in _items)
                item.Button.Text = visible ? "   " + item.Title : "";

            int paddingLeft = visible ? 12 : 16;
            ControlHelper.ForEachButton(pnlMenu, b => b.Padding = new Padding(paddingLeft, 0, 0, 0));
        }

        /// <summary>Mở khóa toàn bộ các mục menu cho tất cả người dùng.</summary>
        public void ApplyRolePermissions(bool laQuanTri)
        {
            // Mở khóa toàn bộ tính năng cho mọi người dùng
            btnTrangChu.Visible = true;
            btnQuanLyLaptop.Visible = true;
            btnBanHang.Visible = true;
            btnNhapHang.Visible = true;
            btnNhanVien.Visible = true;
            btnThongKe.Visible = true;
        }
    }
}

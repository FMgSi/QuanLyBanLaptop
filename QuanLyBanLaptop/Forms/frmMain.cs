using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.DanhMuc;
using QuanLyBanLaptop.Helpers;
using QuanLyBanLaptop.HoaDon;
using QuanLyBanLaptop.Thongke;

namespace QuanLyBanLaptop
{
    /// <summary>
    /// Form chính duy nhất: sidebar bên trái + vùng nội dung (pnlContent) chứa các UserControl.
    /// Bấm menu chỉ đổi UserControl trong pnlContent chứ không mở form mới.
    /// </summary>
    public partial class frmMain : Form
    {
        // Mỗi mục menu ứng với một hàm tạo trang. Muốn thêm trang mới chỉ cần thêm 1 dòng.
        private readonly Dictionary<NavigationMenu, Func<UserControl>> _pageFactories;

        // Trang đã mở được giữ lại để không mất dữ liệu đang nhập khi chuyển qua lại.
        private readonly Dictionary<NavigationMenu, UserControl> _pages = new Dictionary<NavigationMenu, UserControl>();

        private UserControl _currentPage;

        public frmMain()
        {
            InitializeComponent();
            ControlHelper.SetDoubleBuffered(this, pnlContent);

            _pageFactories = new Dictionary<NavigationMenu, Func<UserControl>>
            {
                { NavigationMenu.TrangChu,     CreateTrangChu },
                { NavigationMenu.QuanLyLaptop, () => new ucSanPham() },
                { NavigationMenu.BanHang,      () => new ucBanHang() },
                { NavigationMenu.NhapHang,     () => new ucNhapHang() },
                { NavigationMenu.NhanVien,     () => new ucNhanVien() },
                { NavigationMenu.ThongKe,      () => new ucThongKe() },
            };

            sidebarControl1.MenuSelected += ShowPage;
        }

        private UserControl CreateTrangChu()
        {
            var home = new ucTrangChu();
            // Bấm vào thẻ ở trang chủ cũng đổi luôn mục được chọn trên sidebar.
            home.NavigateRequested += menu =>
            {
                sidebarControl1.SelectedMenu = menu;
                ShowPage(menu);
            };
            return home;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin người dùng và vai trò tự động lên thanh tiêu đề
            Text = string.Format("HỆ THỐNG QUẢN LÝ BÁN HÀNG LAPTOP  |  {0}  -  Vai trò: {1}",
                AppSession.TenNV ?? "Người dùng",
                AppSession.ChucVu ?? "Nhân viên");

            sidebarControl1.ApplyRolePermissions(AppSession.LaQuanTri);

            sidebarControl1.SelectedMenu = NavigationMenu.TrangChu;
            ShowPage(NavigationMenu.TrangChu);
        }

        /// <summary>Hiển thị trang tương ứng với mục menu, tạo mới nếu chưa có.</summary>
        public void ShowPage(NavigationMenu menu)
        {
            if (!_pages.TryGetValue(menu, out UserControl page))
            {
                page = _pageFactories[menu]();
                page.Dock = DockStyle.Fill;
                _pages[menu] = page;
            }

            if (page == _currentPage) return;

            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(page);
            pnlContent.ResumeLayout(true);
            _currentPage = page;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !MessageHelper.Confirm("Bạn có muốn thoát chương trình?"))
                e.Cancel = true;
        }
    }
}

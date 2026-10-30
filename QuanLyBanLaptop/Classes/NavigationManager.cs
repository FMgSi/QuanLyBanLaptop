using System.Collections.Generic;
using System.Windows.Forms;
using QuanLyBanLaptop.DanhMuc;
using QuanLyBanLaptop.HoaDon;
using QuanLyBanLaptop.Thongke;
namespace QuanLyBanLaptop.Classes
{
    public enum NavigationMenu
    {
        None = 0, TrangChu, QuanLyLaptop, BanHang, NhapHang, NhanVien, KhachHang, TheoDoiTraGop, BaoHanhSuaChua, ThongKe, CauHinh
    }
    public static class NavigationManager
    {
        private static readonly Dictionary<NavigationMenu, Form> _forms = new Dictionary<NavigationMenu, Form>();
        private static bool _isNavigating;
        private static Form _hostForm;
        private static Panel _containerPanel;
        private static SidebarControl _mainSidebar;
        private static Label _lblTitle, _lblUser;
        public static bool IsHosted => _hostForm != null && !_hostForm.IsDisposed && _containerPanel != null && !_containerPanel.IsDisposed;
        public static void InitHost(Form hostForm, Panel containerPanel, SidebarControl sidebar, Label lblTitle = null, Label lblUser = null)
        {
            _hostForm = hostForm;
            _containerPanel = containerPanel;
            _mainSidebar = sidebar;
            _lblTitle = lblTitle;
            _lblUser = lblUser;
            UpdateUserInfo();
        }
        public static void UpdateUserInfo()
        {
            if (_lblUser == null || _lblUser.IsDisposed) return;
            string name = !string.IsNullOrEmpty(UserSession.TenNV) ? UserSession.TenNV : "Quản trị viên";
            string ma = !string.IsNullOrEmpty(UserSession.MaNV) ? $" ({UserSession.MaNV})" : "";
            string role = !string.IsNullOrEmpty(UserSession.ChucVu) ? $" - {UserSession.ChucVu}" : "";
            _lblUser.Text = $"👤 {name}{ma}{role}";
        }
        public static Form GetForm(NavigationMenu menu)
        {
            if (menu == NavigationMenu.NhanVien || menu == NavigationMenu.ThongKe || menu == NavigationMenu.CauHinh)
                UserSession.RequireAdmin();
            if (_forms.TryGetValue(menu, out Form existing) && existing != null && !existing.IsDisposed)
                return existing;
            Form f;
            switch (menu)
            {
                case NavigationMenu.CauHinh: f = new frmCauHinhTraGop(); break;
                case NavigationMenu.QuanLyLaptop: f = new frmSanPham(); break;
                case NavigationMenu.BanHang: f = new frmBanHang(); break;
                case NavigationMenu.NhanVien: f = new frmNhanVien(); break;
                case NavigationMenu.KhachHang: f = new frmKhachHang(); break;
                case NavigationMenu.TheoDoiTraGop: f = new frmQuanLyTraGop(); break;
                case NavigationMenu.BaoHanhSuaChua: f = new frmBaoHanhSuaChua(); break;
                case NavigationMenu.ThongKe: f = new frmThongKe(); break;
                default: f = new frmTrangChu(); break;
            }
            RegisterForm(menu, f);
            return f;
        }
        public static void RegisterForm(NavigationMenu menu, Form form)
        {
            if (form == null || form.IsDisposed) return;
            _forms[menu] = form;
            form.FormClosing += (s, e) =>
            {
                if (e.CloseReason != CloseReason.UserClosing) return;
                if (IsHosted) { e.Cancel = true; Navigate(NavigationMenu.TrangChu); }
                else if (!_isNavigating) Application.Exit();
            };
        }
        public static void Navigate(NavigationMenu targetMenu) => Navigate(null, targetMenu);
        public static void Navigate(Form currentForm, NavigationMenu targetMenu)
        {
            if (IsHosted)
            {
                Form target = GetForm(targetMenu);
                if (target == null || target.IsDisposed) return;
                foreach (Control c in target.Controls)
                    if (c is SidebarControl sc) sc.Visible = false;
                target.TopLevel = false;
                target.FormBorderStyle = FormBorderStyle.None;
                target.Dock = DockStyle.Fill;
                _containerPanel.SuspendLayout();
                if (!_containerPanel.Controls.Contains(target)) _containerPanel.Controls.Add(target);
                foreach (Control c in _containerPanel.Controls)
                    if (c != target) c.Visible = false;
                target.Visible = true;
                target.BringToFront();
                target.Show();
                _containerPanel.ResumeLayout(true);
                if (_lblTitle != null && !_lblTitle.IsDisposed) _lblTitle.Text = GetMenuTitle(targetMenu);
                if (_mainSidebar != null && !_mainSidebar.IsDisposed)
                {
                    _mainSidebar.SelectedMenu = targetMenu;
                    _mainSidebar.RefreshOverdueBadge();
                }
                return;
            }
            if (currentForm == null || currentForm.IsDisposed) return;
            Form standalone = GetForm(targetMenu);
            if (standalone == currentForm) return;
            _isNavigating = true;
            try
            {
                standalone.StartPosition = FormStartPosition.Manual;
                if (currentForm.WindowState == FormWindowState.Normal)
                {
                    standalone.WindowState = FormWindowState.Normal;
                    standalone.Location = currentForm.Location;
                    standalone.Size = currentForm.Size;
                }
                else standalone.WindowState = currentForm.WindowState;
                standalone.Show();
                standalone.BringToFront();
                standalone.Activate();
                currentForm.Hide();
            }
            finally { _isNavigating = false; }
        }
        public static string GetMenuTitle(NavigationMenu menu)
        {
            switch (menu)
            {
                case NavigationMenu.TrangChu: return "Trang Chủ";
                case NavigationMenu.QuanLyLaptop: return "Quản Lý Laptop";
                case NavigationMenu.BanHang: return "Hóa Đơn Bán Hàng";
                case NavigationMenu.NhapHang: return "Quản Lý Nhập Hàng";
                case NavigationMenu.NhanVien: return "Quản Lý Nhân Viên";
                case NavigationMenu.KhachHang: return "Quản Lý Khách Hàng";
                case NavigationMenu.TheoDoiTraGop: return "Theo Dõi & Thu Tiền Trả Góp";
                case NavigationMenu.BaoHanhSuaChua: return "Quản Lý Tiếp Nhận Bảo Hành & Sửa Chữa";
                case NavigationMenu.ThongKe: return "Báo Cáo & Thống Kê Doanh Thu";
                case NavigationMenu.CauHinh: return "Cấu Hình Tham Số Hệ Thống";
                default: return "Hệ Thống Quản Lý Bán Laptop";
            }
        }
    }
}

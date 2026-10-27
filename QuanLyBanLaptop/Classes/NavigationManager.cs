using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QuanLyBanLaptop.DanhMuc;
using QuanLyBanLaptop.HoaDon;
using QuanLyBanLaptop.Thongke;

namespace QuanLyBanLaptop.Classes
{
    public static class NavigationManager
    {
        private static readonly Dictionary<NavigationMenu, Form> _forms = new Dictionary<NavigationMenu, Form>();
        private static bool _isNavigating = false;

        private static Form _hostForm;
        private static Panel _containerPanel;
        private static SidebarControl _mainSidebar;
        private static Label _lblTitle;
        private static Label _lblUser;

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
            if (_lblUser != null && !_lblUser.IsDisposed)
            {
                string name = !string.IsNullOrEmpty(UserSession.TenNV) ? UserSession.TenNV : "Quản trị viên";
                string ma = !string.IsNullOrEmpty(UserSession.MaNV) ? $" ({UserSession.MaNV})" : "";
                string role = !string.IsNullOrEmpty(UserSession.ChucVu) ? $" - {UserSession.ChucVu}" : "";
                _lblUser.Text = $"👤 {name}{ma}{role}";
            }
        }
        public static Form GetForm(NavigationMenu menu)
        {
            if (menu == NavigationMenu.NhanVien || menu == NavigationMenu.ThongKe || menu == NavigationMenu.CauHinh)
                UserSession.RequireAdmin();
            if (_forms.TryGetValue(menu, out Form existing) && existing != null && !existing.IsDisposed)
            {
                return existing;
            }

            Form newForm;
            switch (menu)
            {
                case NavigationMenu.CauHinh:
                    newForm = new frmCauHinhTraGop();
                    break;
                case NavigationMenu.TrangChu:
                    newForm = new frmTrangChu();
                    break;
                case NavigationMenu.QuanLyLaptop:
                    newForm = new frmSanPham();
                    break;
                case NavigationMenu.BanHang:
                    newForm = new frmBanHang();
                    break;
                case NavigationMenu.NhapHang:
                    newForm = new frmTrangChu();
                    break;
                case NavigationMenu.NhanVien:
                    newForm = new frmNhanVien();
                    break;
                case NavigationMenu.KhachHang:
                    newForm = new frmKhachHang();
                    break;
                case NavigationMenu.TheoDoiTraGop:
                    newForm = new frmQuanLyTraGop();
                    break;
                case NavigationMenu.BaoHanhSuaChua:
                    newForm = new frmBaoHanhSuaChua();
                    break;
                case NavigationMenu.ThongKe:
                    newForm = new frmThongKe();
                    break;
                default:
                    newForm = new frmTrangChu();
                    break;
            }

            // Hook close event: when hosted, closing a child view navigates back to TrangChu smoothly
            newForm.FormClosing += (s, e) =>
            {
                if (IsHosted)
                {
                    if (e.CloseReason == CloseReason.UserClosing)
                    {
                        e.Cancel = true;
                        Navigate(NavigationMenu.TrangChu);
                    }
                }
                else
                {
                    if (!_isNavigating && e.CloseReason == CloseReason.UserClosing)
                    {
                        Application.Exit();
                    }
                }
            };

            _forms[menu] = newForm;
            return newForm;
        }
        public static void RegisterForm(NavigationMenu menu, Form form)
        {
            if (form != null && !form.IsDisposed)
            {
                _forms[menu] = form;
                form.FormClosing += (s, e) =>
                {
                    if (IsHosted)
                    {
                        if (e.CloseReason == CloseReason.UserClosing)
                        {
                            e.Cancel = true;
                            Navigate(NavigationMenu.TrangChu);
                        }
                    }
                    else
                    {
                        if (!_isNavigating && e.CloseReason == CloseReason.UserClosing)
                        {
                            Application.Exit();
                        }
                    }
                };
            }
        }
        public static void Navigate(NavigationMenu targetMenu)
        {
            Navigate(null, targetMenu);
        }
        public static void Navigate(Form currentForm, NavigationMenu targetMenu)
        {
            if (IsHosted)
            {
                Form targetForm = GetForm(targetMenu);
                if (targetForm == null || targetForm.IsDisposed) return;

                // Hide local sidebar inside the child form to prevent duplicate sidebars
                foreach (Control c in targetForm.Controls)
                {
                    if (c is SidebarControl sc)
                    {
                        sc.Visible = false;
                    }
                }

                // Configure child form as embedded control
                targetForm.TopLevel = false;
                targetForm.FormBorderStyle = FormBorderStyle.None;
                targetForm.Dock = DockStyle.Fill;

                _containerPanel.SuspendLayout();
                if (!_containerPanel.Controls.Contains(targetForm))
                {
                    _containerPanel.Controls.Add(targetForm);
                }

                foreach (Control c in _containerPanel.Controls)
                {
                    if (c != targetForm)
                    {
                        c.Visible = false;
                    }
                }

                targetForm.Visible = true;
                targetForm.BringToFront();
                targetForm.Show();
                _containerPanel.ResumeLayout(true);

                if (_lblTitle != null && !_lblTitle.IsDisposed)
                {
                    _lblTitle.Text = GetMenuTitle(targetMenu);
                }

                if (_mainSidebar != null && !_mainSidebar.IsDisposed)
                {
                    _mainSidebar.SelectedMenu = targetMenu;
                    _mainSidebar.RefreshOverdueBadge();
                }

                return;
            }

            // Fallback for standalone window mode
            if (currentForm == null || currentForm.IsDisposed) return;

            Form standaloneTarget = GetForm(targetMenu);
            if (standaloneTarget == currentForm) return;

            _isNavigating = true;
            try
            {
                standaloneTarget.StartPosition = FormStartPosition.Manual;
                if (currentForm.WindowState == FormWindowState.Normal)
                {
                    standaloneTarget.WindowState = FormWindowState.Normal;
                    standaloneTarget.Location = currentForm.Location;
                    standaloneTarget.Size = currentForm.Size;
                }
                else
                {
                    standaloneTarget.WindowState = currentForm.WindowState;
                }

                standaloneTarget.Show();
                standaloneTarget.BringToFront();
                standaloneTarget.Activate();

                currentForm.Hide();
            }
            finally
            {
                _isNavigating = false;
            }
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
                case NavigationMenu.CauHinh: return "Cấu Hình Trả Góp & Hạng Thành Viên";
                default: return "Hệ Thống Quản Lý Bán Laptop";
            }
        }
    }
}

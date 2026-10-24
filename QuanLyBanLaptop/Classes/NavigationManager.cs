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

        public static Form GetForm(NavigationMenu menu)
        {
            if (_forms.TryGetValue(menu, out Form existing) && existing != null && !existing.IsDisposed)
            {
                return existing;
            }

            Form newForm;
            switch (menu)
            {
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
                    newForm = new frmNhapHang();
                    break;
                case NavigationMenu.NhanVien:
                    newForm = new frmNhanVien();
                    break;
                case NavigationMenu.KhachHang:
                    newForm = new frmKhachHang();
                    break;
                case NavigationMenu.ThongKe:
                    newForm = new frmThongKe();
                    break;
                default:
                    newForm = new frmTrangChu();
                    break;
            }

            // Hook close event to exit entire app when user clicks X on any form
            newForm.FormClosing += (s, e) =>
            {
                if (!_isNavigating && e.CloseReason == CloseReason.UserClosing)
                {
                    Application.Exit();
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
                    if (!_isNavigating && e.CloseReason == CloseReason.UserClosing)
                    {
                        Application.Exit();
                    }
                };
            }
        }

        public static void Navigate(Form currentForm, NavigationMenu targetMenu)
        {
            if (currentForm == null || currentForm.IsDisposed) return;

            // Check if currentForm already matches targetMenu
            Form targetForm = GetForm(targetMenu);
            if (targetForm == currentForm) return;

            _isNavigating = true;
            try
            {
                // Synchronize geometry & window state seamlessly
                targetForm.StartPosition = FormStartPosition.Manual;
                if (currentForm.WindowState == FormWindowState.Normal)
                {
                    targetForm.WindowState = FormWindowState.Normal;
                    targetForm.Location = currentForm.Location;
                    targetForm.Size = currentForm.Size;
                }
                else
                {
                    targetForm.WindowState = currentForm.WindowState;
                }

                targetForm.Show();
                targetForm.BringToFront();
                targetForm.Activate();

                currentForm.Hide();
            }
            finally
            {
                _isNavigating = false;
            }
        }
    }
}

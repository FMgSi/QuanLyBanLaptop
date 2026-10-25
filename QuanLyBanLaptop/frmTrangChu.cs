using System;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop
{
    public partial class frmTrangChu : Form
    {
        public frmTrangChu()
        {
            InitializeComponent();

            NavigationManager.RegisterForm(NavigationMenu.TrangChu, this);
            sidebarControl1.SelectedMenu = NavigationMenu.TrangChu;

            WireNavigationCards();
            CapNhatThongTinNguoiDung();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.TrangChu;
                CapNhatThongTinNguoiDung();
            }
        }

        private void CapNhatThongTinNguoiDung()
        {
            if (!string.IsNullOrEmpty(UserSession.TenNV))
            {
                lblSubtitle.Text = $"Xin chào {UserSession.TenNV} ({UserSession.ChucVu})! Chọn chức năng từ danh mục để bắt đầu làm việc.";
            }
        }

        private void WireNavigationCards()
        {
            // Thẻ Quản lý Laptop
            Action navLaptop = () => NavigationManager.Navigate(this, NavigationMenu.QuanLyLaptop);
            pnlCardLaptop.Click += (s, e) => navLaptop();
            lblCardLaptopTitle.Click += (s, e) => navLaptop();
            lblCardLaptopCount.Click += (s, e) => navLaptop();

            // Thẻ Bán Hàng
            Action navBanHang = () => NavigationManager.Navigate(this, NavigationMenu.BanHang);
            pnlCardBanHang.Click += (s, e) => navBanHang();
            lblCardBanHangTitle.Click += (s, e) => navBanHang();
            lblCardBanHangCount.Click += (s, e) => navBanHang();

            // Thẻ Nhập Hàng
            Action navNhapHang = () => NavigationManager.Navigate(this, NavigationMenu.NhapHang);
            pnlCardNhapHang.Click += (s, e) => navNhapHang();
            lblCardNhapHangTitle.Click += (s, e) => navNhapHang();
            lblCardNhapHangCount.Click += (s, e) => navNhapHang();

            // Thẻ Nhân Viên
            Action navNhanVien = () => NavigationManager.Navigate(this, NavigationMenu.NhanVien);
            pnlCardNhanVien.Click += (s, e) => navNhanVien();
            lblCardNhanVienTitle.Click += (s, e) => navNhanVien();
            lblCardNhanVienCount.Click += (s, e) => navNhanVien();
        }
    }
}

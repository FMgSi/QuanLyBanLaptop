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

            // Wire card clicks
            pnlCardLaptop.Click += (s, e) => NavigationManager.Navigate(this, NavigationMenu.QuanLyLaptop);
            lblCardLaptopCount.Click += (s, e) => NavigationManager.Navigate(this, NavigationMenu.QuanLyLaptop);

            pnlCardBanHang.Click += (s, e) => NavigationManager.Navigate(this, NavigationMenu.BanHang);
            lblCardBanHangCount.Click += (s, e) => NavigationManager.Navigate(this, NavigationMenu.BanHang);

            pnlCardNhapHang.Click += (s, e) => NavigationManager.Navigate(this, NavigationMenu.NhapHang);
            lblCardNhapHangCount.Click += (s, e) => NavigationManager.Navigate(this, NavigationMenu.NhapHang);

            pnlCardNhanVien.Click += (s, e) => NavigationManager.Navigate(this, NavigationMenu.NhanVien);
            lblCardNhanVienCount.Click += (s, e) => NavigationManager.Navigate(this, NavigationMenu.NhanVien);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.TrangChu;
            }
        }
    }
}

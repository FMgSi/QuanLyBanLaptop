using System;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
namespace QuanLyBanLaptop
{
    public partial class frmTrangChu : AppForm
    {
        public frmTrangChu()
        {
            InitializeComponent();
            NavigationManager.RegisterForm(NavigationMenu.TrangChu, this);
            sidebarControl1.SelectedMenu = NavigationMenu.TrangChu;
            WireCard(pnlCardLaptop, lblCardLaptopTitle, lblCardLaptopCount, NavigationMenu.QuanLyLaptop);
            WireCard(pnlCardBanHang, lblCardBanHangTitle, lblCardBanHangCount, NavigationMenu.BanHang);
            WireCard(pnlCardNhapHang, lblCardNhapHangTitle, lblCardNhapHangCount, NavigationMenu.NhapHang);
            WireCard(pnlCardNhanVien, lblCardNhanVienTitle, lblCardNhanVienCount, NavigationMenu.NhanVien);
            CapNhatThongTinNguoiDung();
        }
        private void WireCard(Control card, Control title, Control count, NavigationMenu menu)
        {
            EventHandler nav = (s, e) => NavigationManager.Navigate(this, menu);
            card.Click += nav; title.Click += nav; count.Click += nav;
        }
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!Visible) return;
            sidebarControl1.SelectedMenu = NavigationMenu.TrangChu;
            CapNhatThongTinNguoiDung();
        }
        private void CapNhatThongTinNguoiDung()
        {
            if (!string.IsNullOrEmpty(UserSession.TenNV))
                lblSubtitle.Text = $"Xin chào {UserSession.TenNV} ({UserSession.ChucVu})! Chọn chức năng từ danh mục để bắt đầu làm việc.";
            pnlCardNhanVien.Visible = UserSession.IsAdmin;
        }
    }
}

using System;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop
{
    public partial class ucTrangChu : UserControl
    {
        /// <summary>Yêu cầu frmMain chuyển sang trang khác (khi bấm vào thẻ thống kê).</summary>
        public event Action<NavigationMenu> NavigateRequested;

        public ucTrangChu()
        {
            InitializeComponent();

            WireCard(pnlCardLaptop, lblCardLaptopCount, NavigationMenu.QuanLyLaptop);
            WireCard(pnlCardBanHang, lblCardBanHangCount, NavigationMenu.BanHang);
            WireCard(pnlCardNhapHang, lblCardNhapHangCount, NavigationMenu.NhapHang);
            WireCard(pnlCardNhanVien, lblCardNhanVienCount, NavigationMenu.NhanVien);
        }

        private void WireCard(Control card, Control link, NavigationMenu target)
        {
            EventHandler go = (s, e) => NavigateRequested?.Invoke(target);
            card.Click += go;
            link.Click += go;
        }
    }
}

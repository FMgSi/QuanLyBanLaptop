using System;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmSanPham : Form
    {
        public frmSanPham()
        {
            InitializeComponent();

            NavigationManager.RegisterForm(NavigationMenu.QuanLyLaptop, this);
            sidebarControl1.SelectedMenu = NavigationMenu.QuanLyLaptop;

            // Demo row matching the user's screenshot
            dgvLaptop.Rows.Add("Mã", "Tên Laptop", "CPU X30 2G...", "12HI", "Ổ cứng", "290.000", "20");
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.QuanLyLaptop;
            }
        }
    }
}

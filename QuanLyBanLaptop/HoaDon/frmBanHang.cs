using System;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmBanHang : Form
    {
        public frmBanHang()
        {
            InitializeComponent();

            NavigationManager.RegisterForm(NavigationMenu.BanHang, this);
            sidebarControl1.SelectedMenu = NavigationMenu.BanHang;

            // Demo sample data
            cboLaptopBan.Items.AddRange(new object[] { "Dell Inspiron 14 5430", "Asus Vivobook 15 OLED", "Lenovo Yoga Slim 7" });
            if (cboLaptopBan.Items.Count > 0) cboLaptopBan.SelectedIndex = 0;

            txtDonGiaBan.Text = "28.500.000";
            dgvGioHang.Rows.Add("SP01", "Dell Inspiron 14 5430", "28.500.000", "1", "0", "28.500.000");
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.BanHang;
            }
        }
    }
}

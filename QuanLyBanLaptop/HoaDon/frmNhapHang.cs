using System;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmNhapHang : Form
    {
        public frmNhapHang()
        {
            InitializeComponent();

            NavigationManager.RegisterForm(NavigationMenu.NhapHang, this);
            sidebarControl1.SelectedMenu = NavigationMenu.NhapHang;

            // Demo sample data matching screenshot
            cboNhaCungCap.Items.AddRange(new object[] { "Nhà cung cấp Dell", "Nhà cung cấp Asus", "Nhà cung cấp Lenovo" });
            if (cboNhaCungCap.Items.Count > 0) cboNhaCungCap.SelectedIndex = 0;

            cboNhanVien.Items.AddRange(new object[] { "Nguyễn Văn A", "Trần Thị B" });
            if (cboNhanVien.Items.Count > 0) cboNhanVien.SelectedIndex = 0;

            cboLaptop.Items.AddRange(new object[] { "Dell XPS 15 9530", "Asus ROG Zephyrus G14", "ThinkPad X1 Carbon Gen 11" });
            if (cboLaptop.Items.Count > 0) cboLaptop.SelectedIndex = 0;

            dgvDanhSachNhap.Rows.Add("MS01", "Tên Laptop Đang chọn", "1", "50.000", "130.000");
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.NhapHang;
            }
        }
    }
}

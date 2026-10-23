using System;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmNhanVien : Form
    {
        public frmNhanVien()
        {
            InitializeComponent();

            NavigationManager.RegisterForm(NavigationMenu.NhanVien, this);
            sidebarControl1.SelectedMenu = NavigationMenu.NhanVien;

            // Sample demo items
            cboChucVu.Items.AddRange(new object[] { "Quản lý", "Nhân viên bán hàng", "Thủ kho", "Kế toán" });
            if (cboChucVu.Items.Count > 0) cboChucVu.SelectedIndex = 1;

            dgvNhanVien.Rows.Add("NV01", "Nguyễn Văn A", "Nam", "15/05/1998", "0912345678", "Hà Nội", "Quản lý");
            dgvNhanVien.Rows.Add("NV02", "Trần Thị B", "Nữ", "20/10/2000", "0987654321", "Đà Nẵng", "Nhân viên bán hàng");
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.NhanVien;
            }
        }
    }
}

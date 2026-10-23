using System;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.Thongke
{
    public partial class frmThongKe : Form
    {
        public frmThongKe()
        {
            InitializeComponent();

            NavigationManager.RegisterForm(NavigationMenu.ThongKe, this);
            sidebarControl1.SelectedMenu = NavigationMenu.ThongKe;

            // Demo filter & grid items
            cboLoaiThongKe.Items.AddRange(new object[] { "Tất cả", "Theo ngày", "Theo tháng", "Theo quý", "Theo năm" });
            if (cboLoaiThongKe.Items.Count > 0) cboLoaiThongKe.SelectedIndex = 1;

            dtpTuNgay.Value = DateTime.Now.AddDays(-7);
            dtpDenNgay.Value = DateTime.Now;

            dgvThongKe.Rows.Add(DateTime.Now.ToString("dd/MM/yyyy"), "5", "125.000.000", "95.000.000", "30.000.000");
            dgvThongKe.Rows.Add(DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"), "4", "98.000.000", "72.000.000", "26.000.000");
            dgvThongKe.Rows.Add(DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy"), "3", "68.000.000", "50.000.000", "18.000.000");
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.ThongKe;
            }
        }
    }
}

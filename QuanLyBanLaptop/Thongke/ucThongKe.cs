using System;
using System.Windows.Forms;
using QuanLyBanLaptop.Helpers;

namespace QuanLyBanLaptop.Thongke
{
    public partial class ucThongKe : UserControl
    {
        public ucThongKe()
        {
            InitializeComponent();

            // Dữ liệu mẫu
            ControlHelper.FillCombo(cboLoaiThongKe, 1, "Tất cả", "Theo ngày", "Theo tháng", "Theo quý", "Theo năm");

            dtpTuNgay.Value = DateTime.Now.AddDays(-7);
            dtpDenNgay.Value = DateTime.Now;

            dgvThongKe.Rows.Add(FormatHelper.Date(DateTime.Now), "5", "125.000.000", "95.000.000", "30.000.000");
            dgvThongKe.Rows.Add(FormatHelper.Date(DateTime.Now.AddDays(-1)), "4", "98.000.000", "72.000.000", "26.000.000");
            dgvThongKe.Rows.Add(FormatHelper.Date(DateTime.Now.AddDays(-2)), "3", "68.000.000", "50.000.000", "18.000.000");
        }
    }
}

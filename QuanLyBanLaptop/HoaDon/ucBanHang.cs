using System.Windows.Forms;
using QuanLyBanLaptop.Helpers;

namespace QuanLyBanLaptop.HoaDon
{
    public partial class ucBanHang : UserControl
    {
        public ucBanHang()
        {
            InitializeComponent();

            // Dữ liệu mẫu
            ControlHelper.FillCombo(cboLaptopBan, 0,
                "Dell Inspiron 14 5430", "Asus Vivobook 15 OLED", "Lenovo Yoga Slim 7");

            txtDonGiaBan.Text = FormatHelper.Money(28500000);
            dgvGioHang.Rows.Add("SP01", "Dell Inspiron 14 5430", "28.500.000", "1", "0", "28.500.000");
        }
    }
}

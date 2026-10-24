using System.Windows.Forms;
using QuanLyBanLaptop.Helpers;

namespace QuanLyBanLaptop.HoaDon
{
    public partial class ucNhapHang : UserControl
    {
        public ucNhapHang()
        {
            InitializeComponent();

            // Dữ liệu mẫu
            ControlHelper.FillCombo(cboNhaCungCap, 0, "Nhà cung cấp Dell", "Nhà cung cấp Asus", "Nhà cung cấp Lenovo");
            ControlHelper.FillCombo(cboNhanVien, 0, "Nguyễn Văn A", "Trần Thị B");
            ControlHelper.FillCombo(cboLaptop, 0, "Dell XPS 15 9530", "Asus ROG Zephyrus G14", "ThinkPad X1 Carbon Gen 11");

            dgvDanhSachNhap.Rows.Add("MS01", "Tên Laptop đang chọn", "1", "50.000", "130.000");
        }
    }
}

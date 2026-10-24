using System.Windows.Forms;
using QuanLyBanLaptop.Helpers;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class ucNhanVien : UserControl
    {
        public ucNhanVien()
        {
            InitializeComponent();

            // Dữ liệu mẫu
            ControlHelper.FillCombo(cboChucVu, 1, "Quản lý", "Nhân viên bán hàng", "Thủ kho", "Kế toán");

            dgvNhanVien.Rows.Add("NV01", "Nguyễn Văn A", "Nam", "15/05/1998", "0912345678", "Hà Nội", "Quản lý");
            dgvNhanVien.Rows.Add("NV02", "Trần Thị B", "Nữ", "20/10/2000", "0987654321", "Đà Nẵng", "Nhân viên bán hàng");
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmNhapHang : QuanLyBanLaptop.Classes.AppForm
    {
        // Khởi tạo giao diện và đăng ký sự kiện cho form.
        public frmNhapHang()
        {
            InitializeComponent();

            NavigationManager.RegisterForm(NavigationMenu.NhapHang, this);
            sidebarControl1.SelectedMenu = NavigationMenu.NhapHang;
        }

        // Thực hiện OnVisibleChanged theo trạng thái và dữ liệu hiện tại.
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.NhapHang;
                LoadDuLieu();
            }
        }

        // Tải danh sách NCC, nhân viên, laptop từ CSDL.
        private void LoadDuLieu()
        {
            try
            {
                // Nhà cung cấp
                DataTable dtNCC = DataProcesser.ExecuteReader(
                    "SELECT MaNCC, TenNCC FROM NHA_CUNG_CAP ORDER BY TenNCC");
                cboNhaCungCap.DataSource    = dtNCC;
                cboNhaCungCap.DisplayMember = "TenNCC";
                cboNhaCungCap.ValueMember   = "MaNCC";

                // Nhân viên đang làm việc
                DataTable dtNV = DataProcesser.ExecuteReader(
                    "SELECT MaNV, TenNV FROM NHAN_VIEN WHERE DaNghi = 0 ORDER BY TenNV");
                cboNhanVien.DataSource    = dtNV;
                cboNhanVien.DisplayMember = "TenNV";
                cboNhanVien.ValueMember   = "MaNV";

                // Laptop (theo NCC được chọn)
                LoadLaptopTheoNCC();
            }
            catch (SqlException ex)
            {
                DataProcesser.ShowSqlError(ex);
            }
        }

        // Tải dữ liệu phục vụ LoadLaptopTheoNCC.
        private void LoadLaptopTheoNCC()
        {
            if (cboNhaCungCap.SelectedValue == null) return;
            try
            {
                string maNCC = cboNhaCungCap.SelectedValue.ToString();
                DataTable dtLT = DataProcesser.ExecuteReader(
                    "SELECT MaLaptop, TenLaptop FROM LAPTOP WHERE MaNCC = @MaNCC AND NgungKinhDoanh = 0 ORDER BY TenLaptop",
                    new[] { new SqlParameter("@MaNCC", maNCC) });
                cboLaptop.DataSource    = dtLT;
                cboLaptop.DisplayMember = "TenLaptop";
                cboLaptop.ValueMember   = "MaLaptop";
            }
            catch (SqlException ex)
            {
                DataProcesser.ShowSqlError(ex);
            }
        }
    }
}

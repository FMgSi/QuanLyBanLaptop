using System;
using System.Data;
using System.Data.SqlClient;
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
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.QuanLyLaptop;
                LoadDanhSachLaptop();
            }
        }

        /// <summary>Tải danh sách laptop từ CSDL vào DataGridView.</summary>
        private void LoadDanhSachLaptop()
        {
            try
            {
                DataTable dt = DataProcesser.ExecuteReader(
                    @"SELECT l.MaLaptop, l.TenLaptop, l.Loai, l.CPU, l.RAM, l.SSD,
                             l.DonGia, l.SoLuongTon, l.ThoiGianBaoHanh,
                             n.TenNCC, l.NgungKinhDoanh
                      FROM LAPTOP l
                      JOIN NHA_CUNG_CAP n ON n.MaNCC = l.MaNCC
                      ORDER BY l.MaLaptop");

                dgvLaptop.DataSource = dt;

                // Đổi tên cột tiêu đề cho thân thiện
                if (dgvLaptop.Columns.Count > 0)
                {
                    SetHeader("MaLaptop",        "Mã");
                    SetHeader("TenLaptop",        "Tên Laptop");
                    SetHeader("Loai",             "Loại");
                    SetHeader("CPU",              "CPU");
                    SetHeader("RAM",              "RAM");
                    SetHeader("SSD",              "SSD");
                    SetHeader("DonGia",           "Đơn giá");
                    SetHeader("SoLuongTon",       "Tồn kho");
                    SetHeader("ThoiGianBaoHanh",  "BH (tháng)");
                    SetHeader("TenNCC",           "Nhà cung cấp");
                    SetHeader("NgungKinhDoanh",   "Ngừng KD");
                }
            }
            catch (SqlException ex)
            {
                DataProcesser.ShowSqlError(ex);
            }
        }

        private void SetHeader(string colName, string header)
        {
            if (dgvLaptop.Columns.Contains(colName))
                dgvLaptop.Columns[colName].HeaderText = header;
        }
    }
}

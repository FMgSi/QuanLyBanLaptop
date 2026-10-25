using System;
using System.Data;
using System.Data.SqlClient;
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

            // Filter options
            cboLoaiThongKe.Items.AddRange(new object[] { "Theo ngày", "Theo tháng" });
            cboLoaiThongKe.SelectedIndex = 0;

            dtpTuNgay.Value = DateTime.Now.AddDays(-7);
            dtpDenNgay.Value = DateTime.Now;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.ThongKe;
                LoadThongKe();
            }
        }

        /// <summary>Tải báo cáo doanh thu từ CSDL theo khoảng ngày.</summary>
        public void LoadThongKe()
        {
            try
            {
                var tuNgay  = dtpTuNgay.Value.Date;
                var denNgay = dtpDenNgay.Value.Date;

                DataTable dt = DataProcesser.ExecuteReader(
                    "SELECT * FROM dbo.fn_BaoCaoDoanhThu(@Tu, @Den) ORDER BY Ngay DESC",
                    new[]
                    {
                        new SqlParameter("@Tu",  SqlDbType.Date) { Value = tuNgay  },
                        new SqlParameter("@Den", SqlDbType.Date) { Value = denNgay }
                    });

                dgvThongKe.DataSource = dt;

                if (dgvThongKe.Columns.Count > 0)
                {
                    SetHeader("Ngay",       "Ngày");
                    SetHeader("SoHoaDon",   "Số HĐ");
                    SetHeader("SoMayBan",   "Số máy");
                    SetHeader("DoanhThu",   "Doanh thu");
                    SetHeader("GiaVon",     "Giá vốn");
                    SetHeader("LoiNhuan",   "Lợi nhuận");
                }
            }
            catch (SqlException ex)
            {
                DataProcesser.ShowSqlError(ex);
            }
        }

        private void SetHeader(string colName, string header)
        {
            if (dgvThongKe.Columns.Contains(colName))
                dgvThongKe.Columns[colName].HeaderText = header;
        }
    }
}

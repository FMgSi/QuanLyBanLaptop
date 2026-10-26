using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.Thongke
{
    public partial class frmThongKe : QuanLyBanLaptop.Classes.AppForm
    {
        private DataTable duLieuBaoCao;
        private string tieuDeBaoCaoHienTai = "BÁO CÁO DOANH THU & LỢI NHUẬN";
        private int dongDangIn = 0;
        private int trangDangIn = 0;
        private PrintDocument taiLieuIn;

        public frmThongKe()
        {
            InitializeComponent();

            NavigationManager.RegisterForm(NavigationMenu.ThongKe, this);
            sidebarControl1.SelectedMenu = NavigationMenu.ThongKe;

            // Thiết lập tùy chọn bộ lọc thống kê
            cboLoaiThongKe.Items.Clear();
            cboLoaiThongKe.Items.AddRange(new object[] { "Theo ngày", "Theo tháng", "Theo sản phẩm", "Theo nhân viên" });
            cboLoaiThongKe.SelectedIndex = 0;
            cboLoaiThongKe.SelectedIndexChanged += (s, e) => { if (Visible) LoadThongKe(); };

            btnThongKe.Click += (s, e) => LoadThongKe();
            btnInBaoCao.Click += (s, e) => InBaoCao();
            btnXuatExcel.Click += (s, e) => XuatBaoCaoCSV();

            // Mặc định khoảng ngày 90 ngày gần nhất để bao quát dữ liệu mẫu
            dtpTuNgay.Value = DateTime.Now.AddDays(-90);
            dtpDenNgay.Value = DateTime.Now;

            KhoiTaoBieuDo();
            KhoiTaoLuoi();
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

        private void KhoiTaoLuoi()
        {
            dgvThongKe.AllowUserToAddRows = false;
            dgvThongKe.AllowUserToDeleteRows = false;
            dgvThongKe.AllowUserToResizeColumns = false;
            dgvThongKe.AllowUserToResizeRows = false;
            dgvThongKe.AllowUserToOrderColumns = false;
            dgvThongKe.ReadOnly = true;
            dgvThongKe.MultiSelect = false;
            dgvThongKe.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThongKe.RowHeadersVisible = false;
            dgvThongKe.BackgroundColor = Color.White;
            dgvThongKe.BorderStyle = BorderStyle.None;
            dgvThongKe.GridColor = Color.FromArgb(235, 240, 246);
            dgvThongKe.Font = new Font("Segoe UI", 9F);
            dgvThongKe.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvThongKe.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 108, 189);
            dgvThongKe.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvThongKe.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongKe.EnableHeadersVisualStyles = false;
            dgvThongKe.DefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 108, 189);
            dgvThongKe.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvThongKe.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 253);
        }

        private void KhoiTaoBieuDo()
        {
            chartDoanhthu.BackColor = Color.White;
            chartDoanhthu.ChartAreas[0].BackColor = Color.FromArgb(250, 252, 255);
            chartDoanhthu.ChartAreas[0].AxisX.LineColor = Color.FromArgb(180, 200, 220);
            chartDoanhthu.ChartAreas[0].AxisY.LineColor = Color.FromArgb(180, 200, 220);
            chartDoanhthu.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(240, 245, 250);
            chartDoanhthu.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(240, 245, 250);
            chartDoanhthu.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
            chartDoanhthu.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);
            chartDoanhthu.ChartAreas[0].AxisY.LabelStyle.Format = "#,0,, 'tr'";
            chartDoanhthu.ChartAreas[0].AxisY.Title = "Triệu đồng";
            chartDoanhthu.ChartAreas[0].AxisY.TitleFont = new Font("Segoe UI", 8F, FontStyle.Regular);
            chartDoanhthu.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.FromArgb(60, 80, 100);
            chartDoanhthu.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.FromArgb(60, 80, 100);
            chartDoanhthu.Legends[0].Font = new Font("Segoe UI", 8.5F);
        }

        // Tải báo cáo doanh thu từ CSDL theo chế độ và khoảng ngày đã chọn
        public void LoadThongKe()
        {
            var tuNgay = dtpTuNgay.Value.Date;
            var denNgay = dtpDenNgay.Value.Date;

            if (tuNgay > denNgay)
            {
                Function.ThongBaoCanhBao("Từ ngày không được lớn hơn Đến ngày.", "Khoảng ngày không hợp lệ");
                return;
            }

            int mode = cboLoaiThongKe.SelectedIndex;
            string sql;
            SqlParameter[] pars;

            try
            {
                if (mode == 0) // Theo ngày
                {
                    tieuDeBaoCaoHienTai = "BÁO CÁO DOANH THU THEO NGÀY";
                    lblGridTitle.Text = "BẢNG SỐ LIỆU DOANH THU THEO NGÀY";
                    lblChartTitle.Text = "BIỂU ĐỒ DOANH THU && LỢI NHUẬN THEO NGÀY";
                    sql = "SELECT Ngay, SoHoaDon, SoMayBan, DoanhThu, GiaVon, LoiNhuan FROM dbo.fn_BaoCaoDoanhThu(@Tu, @Den) ORDER BY Ngay ASC";
                    pars = new[]
                    {
                        new SqlParameter("@Tu", SqlDbType.Date) { Value = tuNgay },
                        new SqlParameter("@Den", SqlDbType.Date) { Value = denNgay }
                    };
                }
                else if (mode == 1) // Theo tháng
                {
                    tieuDeBaoCaoHienTai = "BÁO CÁO DOANH THU THEO THÁNG";
                    lblGridTitle.Text = "BẢNG SỐ LIỆU DOANH THU THEO THÁNG";
                    lblChartTitle.Text = "BIỂU ĐỒ DOANH THU && LỢI NHUẬN THEO THÁNG";
                    sql = @"SELECT CAST(Thang AS varchar(2)) + '/' + CAST(Nam AS varchar(4)) AS ThangNam, 
                                   SoHoaDon, SoMayBan, DoanhThu, GiaVon, LoiNhuan 
                            FROM dbo.vw_DoanhThuThang 
                            WHERE (Nam * 100 + Thang) BETWEEN (@TuYear * 100 + @TuMonth) AND (@DenYear * 100 + @DenMonth) 
                            ORDER BY Nam ASC, Thang ASC";
                    pars = new[]
                    {
                        new SqlParameter("@TuYear", SqlDbType.Int) { Value = tuNgay.Year },
                        new SqlParameter("@TuMonth", SqlDbType.Int) { Value = tuNgay.Month },
                        new SqlParameter("@DenYear", SqlDbType.Int) { Value = denNgay.Year },
                        new SqlParameter("@DenMonth", SqlDbType.Int) { Value = denNgay.Month }
                    };
                }
                else if (mode == 2) // Theo sản phẩm
                {
                    tieuDeBaoCaoHienTai = "BÁO CÁO DOANH THU THEO SẢN PHẨM LAPTOP";
                    lblGridTitle.Text = "BẢNG XẾP HẠNG DOANH THU THEO SẢN PHẨM";
                    lblChartTitle.Text = "BIỂU ĐỒ DOANH THU THEO DÒNG LAPTOP";
                    sql = @"SELECT l.MaLaptop, l.TenLaptop, l.Loai, 
                                   SUM(ct.SoLuong) AS SoMayBan, 
                                   COUNT(DISTINCT hd.MaHD) AS SoHoaDon, 
                                   SUM(ct.SoLuong * ct.DonGia) AS DoanhThu, 
                                   SUM(ct.SoLuong * ct.GiaVon) AS GiaVon, 
                                   SUM(ct.SoLuong * (ct.DonGia - ct.GiaVon)) AS LoiNhuan 
                            FROM CHI_TIET_HD ct 
                            JOIN HOA_DON hd ON hd.MaHD = ct.MaHD 
                            JOIN LAPTOP l ON l.MaLaptop = ct.MaLaptop 
                            WHERE hd.NgayLap >= @Tu AND hd.NgayLap < DATEADD(DAY, 1, @Den) 
                            GROUP BY l.MaLaptop, l.TenLaptop, l.Loai 
                            ORDER BY DoanhThu DESC";
                    pars = new[]
                    {
                        new SqlParameter("@Tu", SqlDbType.Date) { Value = tuNgay },
                        new SqlParameter("@Den", SqlDbType.Date) { Value = denNgay }
                    };
                }
                else // Theo nhân viên
                {
                    tieuDeBaoCaoHienTai = "HIỆU SUẤT DOANH THU THEO NHÂN VIÊN";
                    lblGridTitle.Text = "BẢNG HIỆU SUẤT DOANH THU THEO NHÂN VIÊN";
                    lblChartTitle.Text = "BIỂU ĐỒ DOANH THU THEO NHÂN VIÊN";
                    sql = @"SELECT nv.MaNV, nv.TenNV, nv.ChucVu, 
                                   COUNT(DISTINCT hd.MaHD) AS SoHoaDon, 
                                   SUM(ct.SoLuong) AS SoMayBan, 
                                   SUM(ct.SoLuong * ct.DonGia) AS DoanhThu, 
                                   SUM(ct.SoLuong * ct.GiaVon) AS GiaVon, 
                                   SUM(ct.SoLuong * (ct.DonGia - ct.GiaVon)) AS LoiNhuan 
                            FROM HOA_DON hd 
                            JOIN CHI_TIET_HD ct ON ct.MaHD = hd.MaHD 
                            JOIN NHAN_VIEN nv ON nv.MaNV = hd.MaNV 
                            WHERE hd.NgayLap >= @Tu AND hd.NgayLap < DATEADD(DAY, 1, @Den) 
                            GROUP BY nv.MaNV, nv.TenNV, nv.ChucVu 
                            ORDER BY DoanhThu DESC";
                    pars = new[]
                    {
                        new SqlParameter("@Tu", SqlDbType.Date) { Value = tuNgay },
                        new SqlParameter("@Den", SqlDbType.Date) { Value = denNgay }
                    };
                }

                duLieuBaoCao = DataProcesser.ExecuteReader(sql, pars);
                HienThiDuLieuLuoi(mode);
                CapNhatTheKPI();
                CapNhatBieuDo(mode);
            }
            catch (SqlException ex)
            {
                DataProcesser.ShowSqlError(ex);
            }
            catch (Exception ex)
            {
                Function.ThongBaoLoi("Lỗi tải dữ liệu thống kê: " + ex.Message, "Lỗi");
            }
        }

        private void HienThiDuLieuLuoi(int mode)
        {
            dgvThongKe.Columns.Clear();
            dgvThongKe.AutoGenerateColumns = true;
            dgvThongKe.DataSource = duLieuBaoCao;
            dgvThongKe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvThongKe.Columns.Count > 0)
            {
                SetHeader("Ngay", "Thời gian / Ngày");
                SetHeader("ThangNam", "Tháng / Năm");
                SetHeader("SoHoaDon", "Số hóa đơn");
                SetHeader("SoMayBan", "Số máy bán ra");
                SetHeader("DoanhThu", "Tổng doanh thu");
                SetHeader("GiaVon", "Tổng tiền nhập");
                SetHeader("LoiNhuan", "Lợi nhuận ước tính");
                SetHeader("MaLaptop", "Mã Laptop");
                SetHeader("TenLaptop", "Tên Laptop");
                SetHeader("Loai", "Phân loại");
                SetHeader("MaNV", "Mã NV");
                SetHeader("TenNV", "Họ tên NV");
                SetHeader("ChucVu", "Chức vụ");

                FormatColumnCurrency("DoanhThu");
                FormatColumnCurrency("GiaVon");
                FormatColumnCurrency("LoiNhuan");

                if (dgvThongKe.Columns.Contains("Ngay"))
                {
                    dgvThongKe.Columns["Ngay"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvThongKe.Columns["Ngay"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvThongKe.Columns.Contains("ThangNam"))
                    dgvThongKe.Columns["ThangNam"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (dgvThongKe.Columns.Contains("SoHoaDon"))
                    dgvThongKe.Columns["SoHoaDon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (dgvThongKe.Columns.Contains("SoMayBan"))
                    dgvThongKe.Columns["SoMayBan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void FormatColumnCurrency(string colName)
        {
            if (dgvThongKe.Columns.Contains(colName))
            {
                dgvThongKe.Columns[colName].DefaultCellStyle.Format = "#,##0 'đ'";
                dgvThongKe.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void SetHeader(string colName, string header)
        {
            if (dgvThongKe.Columns.Contains(colName))
                dgvThongKe.Columns[colName].HeaderText = header;
        }

        private void CapNhatTheKPI()
        {
            decimal tongDoanhThu = 0;
            decimal tongGiaVon = 0;
            decimal tongLoiNhuan = 0;

            if (duLieuBaoCao != null && duLieuBaoCao.Rows.Count > 0)
            {
                foreach (DataRow row in duLieuBaoCao.Rows)
                {
                    if (duLieuBaoCao.Columns.Contains("DoanhThu") && row["DoanhThu"] != DBNull.Value)
                        tongDoanhThu += Convert.ToDecimal(row["DoanhThu"]);
                    if (duLieuBaoCao.Columns.Contains("GiaVon") && row["GiaVon"] != DBNull.Value)
                        tongGiaVon += Convert.ToDecimal(row["GiaVon"]);
                    if (duLieuBaoCao.Columns.Contains("LoiNhuan") && row["LoiNhuan"] != DBNull.Value)
                        tongLoiNhuan += Convert.ToDecimal(row["LoiNhuan"]);
                }
            }

            lblValDoanhThu.Text = tongDoanhThu.ToString("#,##0") + " đ";
            lblValNhap.Text = tongGiaVon.ToString("#,##0") + " đ";
            lblValLoiNhuan.Text = tongLoiNhuan.ToString("#,##0") + " đ";

            if (tongLoiNhuan >= 0)
                lblValLoiNhuan.ForeColor = Color.FromArgb(40, 167, 69);
            else
                lblValLoiNhuan.ForeColor = Color.FromArgb(220, 53, 69);
        }

        private void CapNhatBieuDo(int mode)
        {
            try
            {
                chartDoanhthu.Series.Clear();

                var seriesDoanhThu = new Series("Doanh thu");
                seriesDoanhThu.ChartType = SeriesChartType.Column;
                seriesDoanhThu.Color = Color.FromArgb(15, 108, 189);
                seriesDoanhThu.BorderColor = Color.FromArgb(0, 80, 160);
                seriesDoanhThu.BorderWidth = 1;

                var seriesLoiNhuan = new Series("Lợi nhuận");
                seriesLoiNhuan.ChartType = (mode == 0 || mode == 1) ? SeriesChartType.Line : SeriesChartType.Column;
                seriesLoiNhuan.Color = Color.FromArgb(40, 167, 69);
                seriesLoiNhuan.BorderWidth = 3;
                seriesLoiNhuan.MarkerStyle = MarkerStyle.Circle;
                seriesLoiNhuan.MarkerSize = 6;

                if (duLieuBaoCao != null && duLieuBaoCao.Rows.Count > 0)
                {
                    for (int i = 0; i < duLieuBaoCao.Rows.Count; i++)
                    {
                        DataRow row = duLieuBaoCao.Rows[i];
                        string xVal = "";
                        if (mode == 0 && duLieuBaoCao.Columns.Contains("Ngay"))
                            xVal = Convert.ToDateTime(row["Ngay"]).ToString("dd/MM");
                        else if (mode == 1 && duLieuBaoCao.Columns.Contains("ThangNam"))
                            xVal = Convert.ToString(row["ThangNam"]);
                        else if (mode == 2 && duLieuBaoCao.Columns.Contains("TenLaptop"))
                            xVal = Shorten(Convert.ToString(row["TenLaptop"]), 14);
                        else if (mode == 3 && duLieuBaoCao.Columns.Contains("TenNV"))
                            xVal = Shorten(Convert.ToString(row["TenNV"]), 14);

                        double dtVal = duLieuBaoCao.Columns.Contains("DoanhThu") && row["DoanhThu"] != DBNull.Value
                            ? Convert.ToDouble(row["DoanhThu"]) : 0;
                        double lnVal = duLieuBaoCao.Columns.Contains("LoiNhuan") && row["LoiNhuan"] != DBNull.Value
                            ? Convert.ToDouble(row["LoiNhuan"]) : 0;

                        seriesDoanhThu.Points.AddXY(xVal, dtVal);
                        seriesLoiNhuan.Points.AddXY(xVal, lnVal);
                    }
                }

                chartDoanhthu.Series.Add(seriesDoanhThu);
                chartDoanhthu.Series.Add(seriesLoiNhuan);

                chartDoanhthu.ChartAreas[0].AxisX.Interval = (duLieuBaoCao != null && duLieuBaoCao.Rows.Count > 10) ? 2 : 1;
                chartDoanhthu.ChartAreas[0].AxisX.LabelStyle.Angle = (mode >= 2) ? -25 : 0;
                chartDoanhthu.ChartAreas[0].RecalculateAxesScale();
            }
            catch { }
        }

        private static string Shorten(string value, int maxLen)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLen) return value;
            return value.Substring(0, maxLen - 1) + "…";
        }

        // Xuất báo cáo CSV UTF-8 tương thích Excel có dấu
        private void XuatBaoCaoCSV()
        {
            if (duLieuBaoCao == null || duLieuBaoCao.Rows.Count == 0)
            {
                Function.ThongBaoCanhBao("Chưa có dữ liệu để xuất báo cáo.", "Thông báo");
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Tệp Excel (*.csv)|*.csv|All Files (*.*)|*.*";
                sfd.FileName = "BaoCaoDoanhThu_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";
                sfd.Title = "Xuất báo cáo thống kê ra Excel";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(tieuDeBaoCaoHienTai);
                    sb.AppendLine("Khoảng thời gian:," + dtpTuNgay.Value.ToString("dd/MM/yyyy") + " đến " + dtpDenNgay.Value.ToString("dd/MM/yyyy"));
                    sb.AppendLine("Tổng doanh thu:," + lblValDoanhThu.Text);
                    sb.AppendLine("Tổng giá vốn:," + lblValNhap.Text);
                    sb.AppendLine("Tổng lợi nhuận:," + lblValLoiNhuan.Text);
                    sb.AppendLine();

                    // Tiêu đề cột
                    var headers = duLieuBaoCao.Columns.Cast<DataColumn>().Select(c => "\"" + c.ColumnName.Replace("\"", "\"\"") + "\"");
                    sb.AppendLine(string.Join(",", headers));

                    // Dữ liệu dòng
                    foreach (DataRow row in duLieuBaoCao.Rows)
                    {
                        var fields = row.ItemArray.Select(item =>
                        {
                            if (item == null || item == DBNull.Value) return "\"\"";
                            if (item is DateTime dt) return "\"" + dt.ToString("dd/MM/yyyy") + "\"";
                            return "\"" + item.ToString().Replace("\"", "\"\"") + "\"";
                        });
                        sb.AppendLine(string.Join(",", fields));
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));
                    Function.ThongBaoThanhCong("Xuất báo cáo thành công ra file:\n" + sfd.FileName, "Thành công");
                }
                catch (Exception ex)
                {
                    Function.ThongBaoLoi("Không thể xuất file: " + ex.Message, "Lỗi xuất file");
                }
            }
        }

        // In ấn báo cáo sử dụng PrintDocument và PrintPreviewDialog
        private void InBaoCao()
        {
            if (duLieuBaoCao == null || duLieuBaoCao.Rows.Count == 0)
            {
                Function.ThongBaoCanhBao("Chưa có dữ liệu để in báo cáo.", "Thông báo");
                return;
            }

            try
            {
                taiLieuIn = new PrintDocument();
                taiLieuIn.DocumentName = tieuDeBaoCaoHienTai;
                taiLieuIn.DefaultPageSettings.Landscape = true;
                taiLieuIn.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(40, 40, 40, 40);

                dongDangIn = 0;
                trangDangIn = 0;

                taiLieuIn.PrintPage += TaiLieuIn_PrintPage;

                using (var preview = new PrintPreviewDialog
                {
                    Document = taiLieuIn,
                    Width = 1000,
                    Height = 700,
                    StartPosition = FormStartPosition.CenterParent
                })
                {
                    preview.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                Function.ThongBaoLoi("Không thể mở bản xem trước khi in. Hãy kiểm tra dịch vụ máy in:\n" + ex.Message, "Lỗi in ấn");
            }
            finally
            {
                if (taiLieuIn != null)
                {
                    taiLieuIn.PrintPage -= TaiLieuIn_PrintPage;
                    taiLieuIn.Dispose();
                    taiLieuIn = null;
                }
            }
        }

        private void TaiLieuIn_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = e.MarginBounds;
            float y = bounds.Top;
            trangDangIn++;

            using (var fontTieuDe = new Font("Segoe UI", 14F, FontStyle.Bold))
            using (var fontSub = new Font("Segoe UI", 9F, FontStyle.Regular))
            using (var fontBold = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (var brushText = new SolidBrush(Color.FromArgb(15, 23, 42)))
            using (var penLine = new Pen(Color.FromArgb(203, 213, 225)))
            using (var brushHeader = new SolidBrush(Color.FromArgb(15, 108, 189)))
            using (var brushHeaderText = new SolidBrush(Color.White))
            using (var brushStripe = new SolidBrush(Color.FromArgb(245, 249, 253)))
            {
                // Tiêu đề báo cáo
                g.DrawString(tieuDeBaoCaoHienTai, fontTieuDe, brushText, bounds.Left, y);
                y += 28;

                string timeInfo = string.Format("Từ ngày: {0:dd/MM/yyyy}  Đến ngày: {1:dd/MM/yyyy}  |  Người lập: {2}", 
                    dtpTuNgay.Value, dtpDenNgay.Value, string.IsNullOrEmpty(UserSession.TenNV) ? "Quản trị viên" : UserSession.TenNV);
                g.DrawString(timeInfo, fontSub, brushText, bounds.Left, y);
                y += 22;

                string kpiInfo = string.Format("Tổng doanh thu: {0}   |   Tổng giá vốn: {1}   |   Tổng lợi nhuận: {2}",
                    lblValDoanhThu.Text, lblValNhap.Text, lblValLoiNhuan.Text);
                g.DrawString(kpiInfo, fontBold, brushHeader, bounds.Left, y);
                y += 26;

                g.DrawLine(penLine, bounds.Left, y, bounds.Right, y);
                y += 10;

                // Chuẩn bị vẽ bảng
                int colCount = Math.Min(duLieuBaoCao.Columns.Count, 8);
                float colWidth = bounds.Width / (float)colCount;
                float rowHeight = 24;

                // Vẽ header bảng
                g.FillRectangle(brushHeader, bounds.Left, y, bounds.Width, rowHeight);
                for (int i = 0; i < colCount; i++)
                {
                    string colName = duLieuBaoCao.Columns[i].ColumnName;
                    string headerText = dgvThongKe.Columns.Contains(colName) ? dgvThongKe.Columns[colName].HeaderText : colName;
                    var rect = new RectangleF(bounds.Left + i * colWidth + 4, y + 4, colWidth - 8, rowHeight - 8);
                    g.DrawString(headerText, fontBold, brushHeaderText, rect);
                }
                y += rowHeight;

                // Vẽ dữ liệu dòng
                while (dongDangIn < duLieuBaoCao.Rows.Count)
                {
                    if (y + rowHeight > bounds.Bottom - 30)
                    {
                        e.HasMorePages = true;
                        g.DrawString(string.Format("Trang {0}", trangDangIn), fontSub, brushText, bounds.Right - 60, bounds.Bottom - 20);
                        return;
                    }

                    if (dongDangIn % 2 == 1)
                        g.FillRectangle(brushStripe, bounds.Left, y, bounds.Width, rowHeight);

                    g.DrawLine(penLine, bounds.Left, y + rowHeight, bounds.Right, y + rowHeight);

                    DataRow row = duLieuBaoCao.Rows[dongDangIn];
                    for (int i = 0; i < colCount; i++)
                    {
                        string val = "";
                        object cell = row[i];
                        if (cell != null && cell != DBNull.Value)
                        {
                            if (cell is DateTime dt)
                                val = dt.ToString("dd/MM/yyyy");
                            else if (cell is decimal dec)
                                val = dec.ToString("#,##0") + " đ";
                            else
                                val = cell.ToString();
                        }

                        var rect = new RectangleF(bounds.Left + i * colWidth + 4, y + 4, colWidth - 8, rowHeight - 8);
                        g.DrawString(val, fontSub, brushText, rect);
                    }

                    y += rowHeight;
                    dongDangIn++;
                }

                g.DrawString(string.Format("Trang {0}", trangDangIn), fontSub, brushText, bounds.Right - 60, bounds.Bottom - 20);
                e.HasMorePages = false;
            }
        }
    }
}

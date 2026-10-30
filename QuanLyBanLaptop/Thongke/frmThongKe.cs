using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QuanLyBanLaptop.Classes;
namespace QuanLyBanLaptop.Thongke
{
    public partial class frmThongKe : AppForm
    {
        protected override bool RequiresAdmin => true;
        private DataTable duLieuBaoCao;
        private string tieuDeBaoCaoHienTai = "BÁO CÁO DOANH THU & LỢI NHUẬN";
        private int dongDangIn = 0, trangDangIn = 0;
        private PrintDocument taiLieuIn;
        public frmThongKe()
        {
            InitializeComponent();
            NavigationManager.RegisterForm(NavigationMenu.ThongKe, this);
            sidebarControl1.SelectedMenu = NavigationMenu.ThongKe;
            cboLoaiThongKe.Items.Clear();
            cboLoaiThongKe.Items.AddRange(new object[] { "Theo ngày", "Theo tháng", "Theo sản phẩm", "Theo nhân viên" });
            cboLoaiThongKe.SelectedIndex = 0;
            cboLoaiThongKe.SelectedIndexChanged += (s, e) => { if (Visible) LoadThongKe(); };
            btnThongKe.Click += (s, e) => LoadThongKe();
            btnInBaoCao.Click += (s, e) => InBaoCao();
            btnXuatExcel.Click += (s, e) => XuatBaoCaoCSV();
            dtpTuNgay.Value = DateTime.Now.AddDays(-90);
            dtpDenNgay.Value = DateTime.Now;
            KhoiTaoBieuDo();
            KhoiTaoLuoi();
        }
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!Visible) return;
            sidebarControl1.SelectedMenu = NavigationMenu.ThongKe;
            LoadThongKe();
        }
        private void KhoiTaoLuoi()
        {
            dgvThongKe.AllowUserToAddRows = dgvThongKe.AllowUserToDeleteRows = dgvThongKe.AllowUserToResizeRows = dgvThongKe.AllowUserToOrderColumns = dgvThongKe.MultiSelect = dgvThongKe.RowHeadersVisible = dgvThongKe.EnableHeadersVisualStyles = false;
            dgvThongKe.AllowUserToResizeColumns = dgvThongKe.ReadOnly = true;
            dgvThongKe.ScrollBars = ScrollBars.Both;
            dgvThongKe.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThongKe.BackgroundColor = Color.White;
            dgvThongKe.BorderStyle = BorderStyle.None;
            dgvThongKe.GridColor = Color.FromArgb(229, 231, 235);
            dgvThongKe.Font = new Font("Segoe UI", 9F);
            dgvThongKe.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvThongKe.ColumnHeadersDefaultCellStyle.BackColor = dgvThongKe.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
            dgvThongKe.ColumnHeadersDefaultCellStyle.ForeColor = dgvThongKe.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvThongKe.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvThongKe.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvThongKe.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 64, 175);
            dgvThongKe.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        }
        private void KhoiTaoBieuDo()
        {
            chartDoanhthu.BackColor = Color.White;
            var area = chartDoanhthu.ChartAreas[0];
            area.BackColor = Color.FromArgb(250, 252, 255);
            area.AxisX.LineColor = area.AxisY.LineColor = Color.FromArgb(180, 200, 220);
            area.AxisX.MajorGrid.LineColor = area.AxisY.MajorGrid.LineColor = Color.FromArgb(240, 245, 250);
            area.AxisX.LabelStyle.Font = area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);
            area.AxisY.LabelStyle.Format = "#,0,, 'tr'";
            area.AxisY.Title = "Triệu đồng";
            area.AxisY.TitleFont = new Font("Segoe UI", 8F, FontStyle.Regular);
            area.AxisX.LabelStyle.ForeColor = area.AxisY.LabelStyle.ForeColor = Color.FromArgb(60, 80, 100);
            chartDoanhthu.Legends[0].Font = new Font("Segoe UI", 8.5F);
        }
        public void LoadThongKe()
        {
            var tuNgay = dtpTuNgay.Value.Date;
            var denNgay = dtpDenNgay.Value.Date;
            if (tuNgay > denNgay) { Function.ThongBaoCanhBao("Từ ngày không được lớn hơn Đến ngày.", "Khoảng ngày không hợp lệ"); return; }
            int mode = cboLoaiThongKe.SelectedIndex;
            try
            {
                (tieuDeBaoCaoHienTai, lblGridTitle.Text, lblChartTitle.Text) = mode == 0 ? ("BÁO CÁO DOANH THU THEO NGÀY", "BẢNG SỐ LIỆU DOANH THU THEO NGÀY", "BIỂU ĐỒ DOANH THU && LỢI NHUẬN THEO NGÀY")
                    : mode == 1 ? ("BÁO CÁO DOANH THU THEO THÁNG", "BẢNG SỐ LIỆU DOANH THU THEO THÁNG", "BIỂU ĐỒ DOANH THU && LỢI NHUẬN THEO THÁNG")
                    : mode == 2 ? ("BÁO CÁO DOANH THU THEO SẢN PHẨM LAPTOP", "BẢNG XẾP HẠNG DOANH THU THEO SẢN PHẨM", "BIỂU ĐỒ DOANH THU THEO DÒNG LAPTOP")
                    : ("HIỆU SUẤT DOANH THU THEO NHÂN VIÊN", "BẢNG HIỆU SUẤT DOANH THU THEO NHÂN VIÊN", "BIỂU ĐỒ DOANH THU THEO NHÂN VIÊN");
                duLieuBaoCao = new Services.ReportService().GetRevenue((Services.RevenueGrouping)mode, tuNgay, denNgay);
                HienThiDuLieuLuoi(mode);
                CapNhatTheKPI();
                CapNhatBieuDo(mode);
            }
            catch (SqlException ex) { DataProcesser.ShowSqlError(ex); }
            catch (Exception ex) { Function.ThongBaoLoi("Lỗi tải dữ liệu thống kê: " + ex.Message, "Lỗi"); }
        }
        private void HienThiDuLieuLuoi(int mode)
        {
            dgvThongKe.Columns.Clear();
            dgvThongKe.AutoGenerateColumns = true;
            dgvThongKe.DataSource = duLieuBaoCao;
            dgvThongKe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvThongKe.Columns.Count == 0) return;
            SetHeader("Ngay", "Thời gian / Ngày", 130);
            SetHeader("ThangNam", "Tháng / Năm", 120);
            SetHeader("SoHoaDon", "Số hóa đơn", 100);
            SetHeader("SoMayBan", "Số máy bán ra", 110);
            SetHeader("DoanhThu", "Tổng doanh thu", 140);
            SetHeader("GiaVon", "Tổng tiền nhập", 140);
            SetHeader("LoiNhuan", "Lợi nhuận ước tính", 140);
            SetHeader("MaLaptop", "Mã Laptop", 100);
            SetHeader("TenLaptop", "Tên Laptop", 180);
            SetHeader("Loai", "Phân loại", 110);
            SetHeader("MaNV", "Mã NV", 90);
            SetHeader("TenNV", "Họ tên NV", 160);
            SetHeader("ChucVu", "Chức vụ", 110);
            FormatColumnCurrency("DoanhThu");
            FormatColumnCurrency("GiaVon");
            FormatColumnCurrency("LoiNhuan");
            if (dgvThongKe.Columns.Contains("Ngay"))
            {
                dgvThongKe.Columns["Ngay"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvThongKe.Columns["Ngay"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            foreach (var c in new[] { "ThangNam", "SoHoaDon", "SoMayBan" })
                if (dgvThongKe.Columns.Contains(c)) dgvThongKe.Columns[c].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private void FormatColumnCurrency(string colName)
        {
            if (!dgvThongKe.Columns.Contains(colName)) return;
            dgvThongKe.Columns[colName].DefaultCellStyle.Format = "#,##0 'đ'";
            dgvThongKe.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        private void SetHeader(string colName, string header, int minWidth = 100)
        {
            if (!dgvThongKe.Columns.Contains(colName)) return;
            dgvThongKe.Columns[colName].HeaderText = header;
            dgvThongKe.Columns[colName].MinimumWidth = minWidth;
        }
        private void CapNhatTheKPI()
        {
            decimal tongDoanhThu = 0, tongGiaVon = 0, tongLoiNhuan = 0;
            if (duLieuBaoCao != null)
            {
                foreach (DataRow row in duLieuBaoCao.Rows)
                {
                    if (duLieuBaoCao.Columns.Contains("DoanhThu") && row["DoanhThu"] != DBNull.Value) tongDoanhThu += Convert.ToDecimal(row["DoanhThu"]);
                    if (duLieuBaoCao.Columns.Contains("GiaVon") && row["GiaVon"] != DBNull.Value) tongGiaVon += Convert.ToDecimal(row["GiaVon"]);
                    if (duLieuBaoCao.Columns.Contains("LoiNhuan") && row["LoiNhuan"] != DBNull.Value) tongLoiNhuan += Convert.ToDecimal(row["LoiNhuan"]);
                }
            }
            lblValDoanhThu.Text = tongDoanhThu.ToString("#,##0") + " đ";
            lblValNhap.Text = tongGiaVon.ToString("#,##0") + " đ";
            lblValLoiNhuan.Text = tongLoiNhuan.ToString("#,##0") + " đ";
            lblValLoiNhuan.ForeColor = tongLoiNhuan >= 0 ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
        }
        private void CapNhatBieuDo(int mode)
        {
            try
            {
                chartDoanhthu.Series.Clear();
                var seriesDoanhThu = new Series("Doanh thu") { ChartType = SeriesChartType.Column, Color = Color.FromArgb(15, 108, 189), BorderColor = Color.FromArgb(0, 80, 160), BorderWidth = 1 };
                var seriesLoiNhuan = new Series("Lợi nhuận") { ChartType = (mode == 0 || mode == 1) ? SeriesChartType.Line : SeriesChartType.Column, Color = Color.FromArgb(40, 167, 69), BorderWidth = 3, MarkerStyle = MarkerStyle.Circle, MarkerSize = 6 };
                if (duLieuBaoCao != null)
                {
                    foreach (DataRow row in duLieuBaoCao.Rows)
                    {
                        string xVal = mode == 0 && duLieuBaoCao.Columns.Contains("Ngay") ? Convert.ToDateTime(row["Ngay"]).ToString("dd/MM")
                            : mode == 1 && duLieuBaoCao.Columns.Contains("ThangNam") ? Convert.ToString(row["ThangNam"])
                            : mode == 2 && duLieuBaoCao.Columns.Contains("TenLaptop") ? Shorten(Convert.ToString(row["TenLaptop"]), 14)
                            : mode == 3 && duLieuBaoCao.Columns.Contains("TenNV") ? Shorten(Convert.ToString(row["TenNV"]), 14) : "";
                        double dtVal = duLieuBaoCao.Columns.Contains("DoanhThu") && row["DoanhThu"] != DBNull.Value ? Convert.ToDouble(row["DoanhThu"]) : 0;
                        double lnVal = duLieuBaoCao.Columns.Contains("LoiNhuan") && row["LoiNhuan"] != DBNull.Value ? Convert.ToDouble(row["LoiNhuan"]) : 0;
                        seriesDoanhThu.Points.AddXY(xVal, dtVal);
                        seriesLoiNhuan.Points.AddXY(xVal, lnVal);
                    }
                }
                chartDoanhthu.Series.Add(seriesDoanhThu);
                chartDoanhthu.Series.Add(seriesLoiNhuan);
                chartDoanhthu.ChartAreas[0].AxisX.Interval = (duLieuBaoCao != null && duLieuBaoCao.Rows.Count > 10) ? 2 : 1;
                chartDoanhthu.ChartAreas[0].AxisX.LabelStyle.Angle = mode >= 2 ? -25 : 0;
                chartDoanhthu.ChartAreas[0].RecalculateAxesScale();
            }
            catch { }
        }
        private static string Shorten(string value, int maxLen) => string.IsNullOrEmpty(value) || value.Length <= maxLen ? value : value.Substring(0, maxLen - 1) + "…";
        private void XuatBaoCaoCSV()
        {
            if (duLieuBaoCao == null || duLieuBaoCao.Rows.Count == 0) { Function.ThongBaoCanhBao("Chưa có dữ liệu để xuất báo cáo.", "Thông báo"); return; }
            using (var sfd = new SaveFileDialog { Filter = "Tệp Excel (*.csv)|*.csv|All Files (*.*)|*.*", FileName = "BaoCaoDoanhThu_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv", Title = "Xuất báo cáo thống kê ra Excel" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(tieuDeBaoCaoHienTai);
                    sb.AppendLine($"Khoảng thời gian:,{dtpTuNgay.Value:dd/MM/yyyy} đến {dtpDenNgay.Value:dd/MM/yyyy}");
                    sb.AppendLine("Tổng doanh thu:," + lblValDoanhThu.Text);
                    sb.AppendLine("Tổng giá vốn:," + lblValNhap.Text);
                    sb.AppendLine("Tổng lợi nhuận:," + lblValLoiNhuan.Text);
                    sb.AppendLine();
                    sb.AppendLine(string.Join(",", duLieuBaoCao.Columns.Cast<DataColumn>().Select(c => "\"" + c.ColumnName.Replace("\"", "\"\"") + "\"")));
                    foreach (DataRow row in duLieuBaoCao.Rows)
                    {
                        var fields = row.ItemArray.Select(item => item == null || item == DBNull.Value ? "\"\"" : item is DateTime dt ? "\"" + dt.ToString("dd/MM/yyyy") + "\"" : "\"" + item.ToString().Replace("\"", "\"\"") + "\"");
                        sb.AppendLine(string.Join(",", fields));
                    }
                    File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));
                    Function.ThongBaoThanhCong("Xuất báo cáo thành công ra file:\n" + sfd.FileName, "Thành công");
                }
                catch (Exception ex) { Function.ThongBaoLoi("Không thể xuất file: " + ex.Message, "Lỗi xuất file"); }
            }
        }
        private void InBaoCao()
        {
            if (duLieuBaoCao == null || duLieuBaoCao.Rows.Count == 0) { Function.ThongBaoCanhBao("Chưa có dữ liệu để in báo cáo.", "Thông báo"); return; }
            try
            {
                taiLieuIn = new PrintDocument { DocumentName = tieuDeBaoCaoHienTai };
                taiLieuIn.DefaultPageSettings.Landscape = true;
                taiLieuIn.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(40, 40, 40, 40);
                dongDangIn = trangDangIn = 0;
                taiLieuIn.PrintPage += TaiLieuIn_PrintPage;
                using (var preview = new PrintPreviewDialog { Document = taiLieuIn, Width = 1000, Height = 700, StartPosition = FormStartPosition.CenterParent })
                    preview.ShowDialog(this);
            }
            catch (Exception ex) { Function.ThongBaoLoi("Không thể mở bản xem trước khi in. Hãy kiểm tra dịch vụ máy in:\n" + ex.Message, "Lỗi in ấn"); }
            finally
            {
                if (taiLieuIn != null) { taiLieuIn.PrintPage -= TaiLieuIn_PrintPage; taiLieuIn.Dispose(); taiLieuIn = null; }
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
                g.DrawString(tieuDeBaoCaoHienTai, fontTieuDe, brushText, bounds.Left, y);
                y += 28;
                g.DrawString($"Từ ngày: {dtpTuNgay.Value:dd/MM/yyyy}  Đến ngày: {dtpDenNgay.Value:dd/MM/yyyy}  |  Người lập: {(string.IsNullOrEmpty(UserSession.TenNV) ? "Quản trị viên" : UserSession.TenNV)}", fontSub, brushText, bounds.Left, y);
                y += 22;
                g.DrawString($"Tổng doanh thu: {lblValDoanhThu.Text}   |   Tổng giá vốn: {lblValNhap.Text}   |   Tổng lợi nhuận: {lblValLoiNhuan.Text}", fontBold, brushHeader, bounds.Left, y);
                y += 26;
                g.DrawLine(penLine, bounds.Left, y, bounds.Right, y);
                y += 10;
                int colCount = Math.Min(duLieuBaoCao.Columns.Count, 8);
                float colWidth = bounds.Width / (float)colCount, rowHeight = 24;
                g.FillRectangle(brushHeader, bounds.Left, y, bounds.Width, rowHeight);
                for (int i = 0; i < colCount; i++)
                {
                    string colName = duLieuBaoCao.Columns[i].ColumnName;
                    string headerText = dgvThongKe.Columns.Contains(colName) ? dgvThongKe.Columns[colName].HeaderText : colName;
                    g.DrawString(headerText, fontBold, brushHeaderText, new RectangleF(bounds.Left + i * colWidth + 4, y + 4, colWidth - 8, rowHeight - 8));
                }
                y += rowHeight;
                while (dongDangIn < duLieuBaoCao.Rows.Count)
                {
                    if (y + rowHeight > bounds.Bottom - 30)
                    {
                        e.HasMorePages = true;
                        g.DrawString($"Trang {trangDangIn}", fontSub, brushText, bounds.Right - 60, bounds.Bottom - 20);
                        return;
                    }
                    if (dongDangIn % 2 == 1) g.FillRectangle(brushStripe, bounds.Left, y, bounds.Width, rowHeight);
                    g.DrawLine(penLine, bounds.Left, y + rowHeight, bounds.Right, y + rowHeight);
                    DataRow row = duLieuBaoCao.Rows[dongDangIn];
                    for (int i = 0; i < colCount; i++)
                    {
                        object cell = row[i];
                        string val = cell == null || cell == DBNull.Value ? "" : cell is DateTime dt ? dt.ToString("dd/MM/yyyy") : cell is decimal dec ? dec.ToString("#,##0") + " đ" : cell.ToString();
                        g.DrawString(val, fontSub, brushText, new RectangleF(bounds.Left + i * colWidth + 4, y + 4, colWidth - 8, rowHeight - 8));
                    }
                    y += rowHeight;
                    dongDangIn++;
                }
                g.DrawString($"Trang {trangDangIn}", fontSub, brushText, bounds.Right - 60, bounds.Bottom - 20);
                e.HasMorePages = false;
            }
        }
    }
}

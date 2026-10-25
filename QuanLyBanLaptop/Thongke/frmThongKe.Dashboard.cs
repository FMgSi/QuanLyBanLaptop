using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLyBanLaptop.Thongke
{
    public partial class frmThongKe
    {
        private DataTable duLieuBaoCao = new DataTable();
        private PrintDocument taiLieuIn;
        private int dongDangIn;
        private int trangDangIn;
        private Exception loiInBaoCao;
        private string tieuDeBaoCao = "BÁO CÁO DOANH THU & LỢI NHUẬN";

        private void TaoGiaoDienBaoCaoMoi()
        {
            cboCheDoBaoCao.Items.Clear();
            cboCheDoBaoCao.Items.AddRange(new object[] { "Theo ngày", "Theo tháng", "Theo sản phẩm", "Theo nhân viên" });
            cboCheDoBaoCao.SelectedIndex = 0;
            cboCheDoBaoCao.SelectedIndexChanged += (s, e) => { if (Visible && IsHandleCreated) TaiBaoCaoMoi(); };

            StyleActionButton(btnLayDuLieu, "▤  Thống kê", Color.FromArgb(37, 99, 235));
            StyleActionButton(btnInBaoCao, "▧  In báo cáo", Color.FromArgb(126, 34, 206));
            StyleActionButton(btnXuatExcel, "▣  Excel", Color.FromArgb(22, 163, 74));
            btnInBaoCao.Width = 100;
            btnInBaoCao.Click += (s, e) => MoXemTruocIn();

            dgvBaoCao.DataSource = null;
            dgvBaoCao.AutoGenerateColumns = true;
            dgvBaoCao.ClearSelection();
            dgvBaoCao.CurrentCell = null;
            pnlContent.BackColor = Color.FromArgb(240, 244, 248);
            pnlContent.Padding = new Padding(14, 10, 14, 10);
        }
        private static Panel TaoKhungNoiDung(out Label title, string text)
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(0, 0, 8, 0) };
            title = new Label { Text = text, AutoSize = false };
            panel.Controls.Add(title);
            return panel;
        }

        private static Label TaoGiaTriThe()
        {
            return new Label { Text = "0 đ", Dock = DockStyle.Top, Height = 29, Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42), TextAlign = ContentAlignment.MiddleLeft };
        }

        private static Label TaoMoTaThe()
        {
            return new Label { Text = "Tỷ suất lợi nhuận: 0%", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(5, 150, 105), TextAlign = ContentAlignment.TopLeft };
        }

        private static Panel TaoTheThongKe(string title, Label value, object description, Color accent)
        {
            var card = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(0, 0, 8, 0), Padding = new Padding(12, 8, 8, 6) };
            var stripe = new Panel { Dock = DockStyle.Left, Width = 4, BackColor = accent };
            var content = new Panel { Dock = DockStyle.Fill, Padding = new Padding(9, 0, 0, 0) };
            var label = new Label { Text = title, Dock = DockStyle.Top, Height = 20, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139), TextAlign = ContentAlignment.MiddleLeft };
            value.Dock = DockStyle.Top;
            value.Height = 31;
            value.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            value.ForeColor = accent;
            value.TextAlign = ContentAlignment.MiddleLeft;
            if (description is Label descriptionLabel)
            {
                descriptionLabel.Dock = DockStyle.Fill;
                content.Controls.Add(descriptionLabel);
            }
            else
            {
                content.Controls.Add(new Label { Text = Convert.ToString(description), Dock = DockStyle.Fill, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(100, 116, 139), TextAlign = ContentAlignment.TopLeft });
            }
            content.Controls.Add(value);
            content.Controls.Add(label);
            var badge = new Panel { Dock = DockStyle.Right, Width = 42, BackColor = Color.FromArgb(239, 246, 255), Margin = new Padding(0, 3, 2, 0) };
            string iconText = title.Contains("DOANH THU") ? "₫" : title.Contains("GIÁ VỐN") ? "▤" : title.Contains("LỢI NHUẬN") ? "◈" : "▰";
            badge.Controls.Add(new Label { Text = iconText, Dock = DockStyle.Fill, ForeColor = accent, Font = new Font("Segoe UI", 16F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter });
            card.Controls.Add(badge);
            card.Controls.Add(content);
            card.Controls.Add(stripe);
            return card;
        }

        private static Label TaoLabelTong(Color color)
        {
            return new Label { Text = "0 đ", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, ForeColor = color, Font = new Font("Segoe UI", 12F, FontStyle.Bold) };
        }

        private static void AddFilterControl(TableLayoutPanel panel, string labelText, Control control, int labelCol, int controlCol)
        {
            panel.Controls.Add(new Label { Text = labelText, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(51, 65, 85) }, labelCol, 0);
            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(3, 4, 8, 4);
            panel.Controls.Add(control, controlCol, 0);
        }

        private static void StyleActionButton(Button button, string text, Color color)
        {
            button.Text = text;
            button.Dock = DockStyle.None;
            button.Width = 92;
            button.Height = 31;
            button.Margin = new Padding(4, 0, 0, 0);
            button.BackColor = color;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
        }

        private void TaiBaoCaoMoi()
        {
            DateTime tu = dtpTuNgay.Value.Date;
            DateTime den = dtpDenNgay.Value.Date;
            if (tu > den)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.", "Khoảng ngày không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tuSql = tu.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            string denSql = den.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            string query;
            int mode = cboCheDoBaoCao.SelectedIndex;
            if (mode == 0)
            {
                tieuDeBaoCao = "BÁO CÁO DOANH THU THEO NGÀY";
                query = "SELECT CONVERT(varchar(10), Ngay, 103) AS Ngay, SoHoaDon, SoMayBan, DoanhThu, GiaVon, LoiNhuan FROM dbo.fn_BaoCaoDoanhThu('" + tuSql + "','" + denSql + "') ORDER BY Ngay";
            }
            else if (mode == 1)
            {
                tieuDeBaoCao = "BÁO CÁO DOANH THU THEO THÁNG";
                query = "SELECT CAST(Thang AS varchar(2)) + '/' + CAST(Nam AS varchar(4)) AS ThangNam, SoHoaDon, SoMayBan, DoanhThu, GiaVon, LoiNhuan FROM dbo.vw_DoanhThuThang WHERE (Nam * 100 + Thang) BETWEEN (YEAR('" + tuSql + "') * 100 + MONTH('" + tuSql + "')) AND (YEAR('" + denSql + "') * 100 + MONTH('" + denSql + "')) ORDER BY Nam, Thang";
            }
            else if (mode == 2)
            {
                tieuDeBaoCao = "DOANH THU THEO SẢN PHẨM LAPTOP";
                query = "SELECT l.MaLaptop, l.TenLaptop, l.Loai, SUM(ct.SoLuong) AS SoMayBan, COUNT(DISTINCT hd.MaHD) AS SoHoaDon, SUM(ct.SoLuong * ct.DonGia) AS DoanhThu, SUM(ct.SoLuong * ct.GiaVon) AS GiaVon, SUM(ct.SoLuong * (ct.DonGia - ct.GiaVon)) AS LoiNhuan FROM CHI_TIET_HD ct JOIN HOA_DON hd ON hd.MaHD = ct.MaHD JOIN LAPTOP l ON l.MaLaptop = ct.MaLaptop WHERE hd.NgayLap >= '" + tuSql + "' AND hd.NgayLap < DATEADD(DAY, 1, '" + denSql + "') GROUP BY l.MaLaptop, l.TenLaptop, l.Loai ORDER BY DoanhThu DESC";
            }
            else
            {
                tieuDeBaoCao = "HIỆU SUẤT DOANH THU THEO NHÂN VIÊN";
                query = "SELECT nv.MaNV, nv.TenNV, nv.ChucVu, COUNT(DISTINCT hd.MaHD) AS SoHoaDon, SUM(ct.SoLuong) AS SoMayBan, SUM(ct.SoLuong * ct.DonGia) AS DoanhThu, SUM(ct.SoLuong * ct.GiaVon) AS GiaVon, SUM(ct.SoLuong * (ct.DonGia - ct.GiaVon)) AS LoiNhuan FROM HOA_DON hd JOIN CHI_TIET_HD ct ON ct.MaHD = hd.MaHD JOIN NHAN_VIEN nv ON nv.MaNV = hd.MaNV WHERE hd.NgayLap >= '" + tuSql + "' AND hd.NgayLap < DATEADD(DAY, 1, '" + denSql + "') GROUP BY nv.MaNV, nv.TenNV, nv.ChucVu ORDER BY DoanhThu DESC";
            }

            try
            {
                duLieuBaoCao = dataProcesser.DocBang(query);
                DataTable tong = dataProcesser.DocBang("SELECT ISNULL(SUM(ct.SoLuong * ct.DonGia),0) AS DoanhThu, ISNULL(SUM(ct.SoLuong * ct.GiaVon),0) AS GiaVon, ISNULL(SUM(ct.SoLuong * (ct.DonGia-ct.GiaVon)),0) AS LoiNhuan, COUNT(DISTINCT hd.MaHD) AS SoHoaDon, ISNULL(SUM(ct.SoLuong),0) AS SoMayBan FROM HOA_DON hd JOIN CHI_TIET_HD ct ON ct.MaHD=hd.MaHD WHERE hd.NgayLap >= '" + tuSql + "' AND hd.NgayLap < DATEADD(DAY,1,'" + denSql + "')");
                HienThiTongQuan(tong);
                HienThiBangBaoCao(mode);
                HienThiBieuDoBaoCao(mode);
                lblTrangThaiDB.Text = "● Đã tải dữ liệu từ SQL Server";
                lblTrangThaiDB.ForeColor = Color.FromArgb(0, 140, 80);
            }
            catch (Exception ex)
            {
                lblTrangThaiDB.Text = "● Không thể đọc dữ liệu SQL Server";
                lblTrangThaiDB.ForeColor = Color.FromArgb(190, 50, 50);
                MessageBox.Show("Không thể tải báo cáo. Kiểm tra kết nối CSDL và các view/hàm thống kê.\n\n" + ex.Message, "Lỗi thống kê", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiTongQuan(DataTable tong)
        {
            if (tong == null || tong.Rows.Count == 0) return;
            DataRow row = tong.Rows[0];
            decimal doanhThu = Convert.ToDecimal(row["DoanhThu"]);
            decimal giaVon = Convert.ToDecimal(row["GiaVon"]);
            decimal loiNhuan = Convert.ToDecimal(row["LoiNhuan"]);
            lblDoanhThuValue.Text = doanhThu.ToString("#,0") + " đ";
            lblTongChiPhi.Text = giaVon.ToString("#,0") + " đ";
            lblTongLoiNhuan.Text = loiNhuan.ToString("#,0") + " đ";
            lblTySuat.Text = "Tỷ suất lợi nhuận: " + (doanhThu == 0 ? 0 : loiNhuan * 100m / doanhThu).ToString("0.##") + "%";
            lblTongBan.Text = Convert.ToInt32(row["SoMayBan"]).ToString("#,0") + " máy / " + Convert.ToInt32(row["SoHoaDon"]).ToString("#,0") + " HĐ";
        }

        private void HienThiBangBaoCao(int mode)
        {
            dgvBaoCao.DataSource = duLieuBaoCao;
            dgvBaoCao.ClearSelection();
            dgvBaoCao.CurrentCell = null;
            lblTieuDeBang.Text = mode == 0 ? "BÁO CÁO DOANH THU THEO NGÀY (fn_BaoCaoDoanhThu)" :
                mode == 1 ? "BÁO CÁO DOANH THU THEO THÁNG (vw_DoanhThuThang)" :
                mode == 2 ? "DOANH THU THEO SẢN PHẨM (vw_DoanhThuLaptop)" : "DOANH THU THEO NHÂN VIÊN (vw_DoanhThuNhanVien)";
            lblTieuDeBieuDo.Text = mode == 2 ? "DOANH THU & LỢI NHUẬN THEO SẢN PHẨM" : mode == 3 ? "DOANH THU & LỢI NHUẬN THEO NHÂN VIÊN" : "BIỂU ĐỒ DOANH THU & LỢI NHUẬN";
            foreach (DataGridViewColumn column in dgvBaoCao.Columns)
            {
                column.HeaderText = column.HeaderText.Replace("SoHoaDon", "Số hóa đơn").Replace("SoMayBan", "Số máy bán")
                    .Replace("DoanhThu", "Doanh thu").Replace("GiaVon", "Giá vốn").Replace("LoiNhuan", "Lợi nhuận")
                    .Replace("MaLaptop", "Mã laptop").Replace("TenLaptop", "Tên laptop").Replace("TenNV", "Nhân viên")
                    .Replace("ChucVu", "Chức vụ").Replace("SoHoaDon", "Số hóa đơn").Replace("Ngay", "Ngày")
                    .Replace("ThangNam", "Tháng / Năm").Replace("Nam", "Năm").Replace("Thang", "Tháng").Replace("Loai", "Phân loại").Replace("MaNV", "Mã NV");
                if (column.Name == "DoanhThu" || column.Name == "GiaVon" || column.Name == "LoiNhuan")
                {
                    column.DefaultCellStyle.Format = "#,0 'đ'";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (column.Name == "SoHoaDon" || column.Name == "SoMayBan") column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if ((mode == 1 && (column.Name == "Nam" || column.Name == "Thang")) || (mode == 2 && (column.Name == "SoHoaDon" || column.Name == "GiaVon")) || (mode == 3 && (column.Name == "SoMayBan" || column.Name == "GiaVon"))) column.Visible = false;
            }
            decimal doanhThu = SumColumn("DoanhThu");
            decimal loiNhuan = SumColumn("LoiNhuan");
            lblGridDoanhThu.Text = "Doanh thu: " + doanhThu.ToString("#,0") + " đ";
            lblGridLoiNhuan.Text = "Lợi nhuận: " + loiNhuan.ToString("#,0") + " đ";
        }

        private decimal SumColumn(string columnName)
        {
            if (duLieuBaoCao == null || !duLieuBaoCao.Columns.Contains(columnName)) return 0;
            decimal sum = 0;
            foreach (DataRow row in duLieuBaoCao.Rows) if (row[columnName] != DBNull.Value) sum += Convert.ToDecimal(row[columnName]);
            return sum;
        }

        private void HienThiBieuDoBaoCao(int mode)
        {
            chartDoanhthu.Series.Clear();
            chartDoanhthu.Legends.Clear();
            var legend = new Legend("Chú giải") { Docking = Docking.Bottom, Font = new Font("Segoe UI", 8F) };
            chartDoanhthu.Legends.Add(legend);
            var revenue = new Series("Doanh thu") { ChartType = mode == 2 ? SeriesChartType.Column : SeriesChartType.Line, Color = Color.FromArgb(37, 99, 235), BorderWidth = 3, MarkerStyle = MarkerStyle.Circle, MarkerSize = 6, Legend = legend.Name };
            var profit = new Series("Lợi nhuận") { ChartType = revenue.ChartType, Color = Color.FromArgb(16, 185, 129), BorderWidth = 3, MarkerStyle = MarkerStyle.Circle, MarkerSize = 5, Legend = legend.Name };
            for (int i = 0; i < duLieuBaoCao.Rows.Count; i++)
            {
                DataRow row = duLieuBaoCao.Rows[i];
                string category = mode == 0 ? Convert.ToString(row["Ngay"]) : mode == 1 ? Convert.ToString(row["ThangNam"]) : mode == 2 ? Convert.ToString(row["TenLaptop"]) : Convert.ToString(row["TenNV"]);
                var p1 = revenue.Points.AddXY(category, Convert.ToDouble(row["DoanhThu"]));
                var p2 = profit.Points.AddXY(category, Convert.ToDouble(row["LoiNhuan"]));
                if (mode >= 2) { revenue.Points[p1].AxisLabel = Shorten(category, 16); profit.Points[p2].AxisLabel = Shorten(category, 16); }
            }
            chartDoanhthu.Series.Add(revenue);
            chartDoanhthu.Series.Add(profit);
            chartDoanhthu.ChartAreas[0].AxisY.LabelStyle.Format = "#,0,, 'tr'";
            chartDoanhthu.ChartAreas[0].AxisY.Title = "Triệu đồng";
            chartDoanhthu.ChartAreas[0].AxisY.TitleFont = new Font("Segoe UI", 8F);
            chartDoanhthu.ChartAreas[0].AxisY.Minimum = 0;
            chartDoanhthu.ChartAreas[0].AxisX.LabelStyle.Angle = mode >= 2 ? -25 : 0;
            chartDoanhthu.ChartAreas[0].AxisX.Interval = duLieuBaoCao.Rows.Count > 12 ? Math.Ceiling(duLieuBaoCao.Rows.Count / 12.0) : 1;
            chartDoanhthu.ChartAreas[0].RecalculateAxesScale();
        }

        private static string Shorten(string value, int length)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= length) return value;
            return value.Substring(0, length - 1) + "…";
        }

        private void MoXemTruocIn()
        {
            Form cuaSoChu = LayCuaSoChu();
            try
            {
                taiLieuIn = new PrintDocument();
                taiLieuIn.DocumentName = tieuDeBaoCao;
                taiLieuIn.DefaultPageSettings.Landscape = true;
                taiLieuIn.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(40, 40, 38, 38);
                dongDangIn = 0;
                trangDangIn = 0;
                loiInBaoCao = null;
                taiLieuIn.PrintPage += TaiLieuIn_PrintPage;
                using (var preview = new PrintPreviewDialog
                {
                    Document = taiLieuIn,
                    Width = 1000,
                    Height = 760,
                    StartPosition = FormStartPosition.CenterParent
                })
                {
                    // Form thống kê được nhúng bên trong trang Nhân viên; hộp thoại in
                    // cần gắn với cửa sổ chính để không làm mất focus của ứng dụng.
                    preview.ShowDialog(cuaSoChu);
                }
                if (loiInBaoCao != null)
                {
                    MessageBox.Show("Không thể dựng trang báo cáo để in.\n\n" + loiInBaoCao.Message,
                        "Lỗi dữ liệu in", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở bản xem trước hoặc in báo cáo. Hãy kiểm tra máy in mặc định.\n\n" + ex.Message,
                    "Lỗi in báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (taiLieuIn != null)
                {
                    taiLieuIn.PrintPage -= TaiLieuIn_PrintPage;
                    taiLieuIn.Dispose();
                    taiLieuIn = null;
                }
                if (cuaSoChu != null && !cuaSoChu.IsDisposed)
                    cuaSoChu.Activate();
            }
        }

        private Form LayCuaSoChu()
        {
            Control control = this;
            while (control != null)
            {
                Form form = control as Form;
                if (form != null && form.TopLevel) return form;
                control = control.Parent;
            }
            return FindForm();
        }

        private void TaiLieuIn_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                VeTrangBaoCao(e);
            }
            catch (Exception ex)
            {
                // PrintPage chạy trong vòng lặp thông điệp của PrintPreviewDialog;
                // lỗi tại đây có thể làm debugger cướp focus nếu thoát khỏi event.
                loiInBaoCao = ex;
                e.HasMorePages = false;
                e.Graphics.Clear(Color.White);
                using (var errorFont = new Font("Segoe UI", 11F, FontStyle.Bold))
                using (var errorBrush = new SolidBrush(Color.Firebrick))
                using (var detailFont = new Font("Segoe UI", 9F))
                using (var detailBrush = new SolidBrush(Color.FromArgb(51, 65, 85)))
                {
                    e.Graphics.DrawString("Không thể tạo bản in báo cáo.", errorFont, errorBrush, e.MarginBounds.Left, e.MarginBounds.Top);
                    e.Graphics.DrawString(ex.Message, detailFont, detailBrush,
                        new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top + 32, e.MarginBounds.Width, 80));
                }
            }
        }

        private void VeTrangBaoCao(PrintPageEventArgs e)
        {
            Rectangle bounds = e.MarginBounds;
            float y = bounds.Top;
            trangDangIn++;

            using (var titleFont = new Font("Segoe UI", 15F, FontStyle.Bold))
            using (var bodyFont = new Font("Segoe UI", 8.5F))
            using (var boldFont = new Font("Segoe UI", 8.5F, FontStyle.Bold))
            using (var headerFont = new Font("Segoe UI", 8.5F, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.FromArgb(15, 23, 42)))
            using (var linePen = new Pen(Color.FromArgb(203, 213, 225)))
            using (var headerBrush = new SolidBrush(Color.FromArgb(30, 58, 138)))
            using (var headerTextBrush = new SolidBrush(Color.White))
            using (var stripeBrush = new SolidBrush(Color.FromArgb(241, 245, 249)))
            using (var cellFormat = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap, LineAlignment = StringAlignment.Center })
            {
                e.Graphics.DrawString(tieuDeBaoCao, titleFont, brush, bounds.Left, y);
                y += 27;
                e.Graphics.DrawString("Thời gian: " + dtpTuNgay.Value.ToString("dd/MM/yyyy") + " - " + dtpDenNgay.Value.ToString("dd/MM/yyyy"), bodyFont, brush, bounds.Left, y);
                y += 19;
                e.Graphics.DrawString("Doanh thu: " + lblDoanhThuValue.Text + "     Giá vốn: " + lblTongChiPhi.Text + "     Lợi nhuận: " + lblTongLoiNhuan.Text + "     Sản phẩm / hóa đơn: " + lblTongBan.Text,
                    boldFont, brush, bounds.Left, y);
                y += 23;
                e.Graphics.DrawLine(linePen, bounds.Left, y, bounds.Right, y);
                y += 8;

                var cotIn = dgvBaoCao.Columns.Cast<DataGridViewColumn>()
                    .Where(c => c.Visible && duLieuBaoCao != null && duLieuBaoCao.Columns.Contains(c.Name))
                    .ToList();
                if (cotIn.Count == 0 && duLieuBaoCao != null)
                    cotIn = duLieuBaoCao.Columns.Cast<DataColumn>()
                        .Select(c => (DataGridViewColumn)new DataGridViewTextBoxColumn { Name = c.ColumnName, HeaderText = c.ColumnName })
                        .ToList();

                float tableWidth = bounds.Width;
                float[] weights = cotIn.Select(c =>
                {
                    string name = c.Name;
                    if (name == "TenLaptop" || name == "TenNV") return 1.8f;
                    if (name == "MaLaptop" || name == "MaNV" || name == "Loai" || name == "ChucVu") return 1.0f;
                    if (name == "Ngay" || name == "ThangNam") return 1.1f;
                    if (name == "DoanhThu" || name == "GiaVon" || name == "LoiNhuan") return 1.25f;
                    return 0.85f;
                }).ToArray();
                float weightTotal = weights.Sum();
                float[] widths = weights.Select(w => tableWidth * w / weightTotal).ToArray();
                const float headerHeight = 26f;
                const float rowHeight = 23f;
                float x = bounds.Left;

                for (int i = 0; i < cotIn.Count; i++)
                {
                    RectangleF cell = new RectangleF(x, y, widths[i], headerHeight);
                    e.Graphics.FillRectangle(headerBrush, cell);
                    cell.Inflate(-4, 0);
                    cellFormat.Alignment = StringAlignment.Near;
                    e.Graphics.DrawString(cotIn[i].HeaderText, headerFont, headerTextBrush, cell, cellFormat);
                    x += widths[i];
                }
                y += headerHeight;

                if (duLieuBaoCao == null || duLieuBaoCao.Rows.Count == 0)
                {
                    e.Graphics.DrawString("Không có dữ liệu trong khoảng thời gian đã chọn.", bodyFont, brush, bounds.Left + 4, y + 8);
                }
                else
                {
                    while (dongDangIn < duLieuBaoCao.Rows.Count && y + rowHeight <= bounds.Bottom - 24)
                    {
                        DataRow row = duLieuBaoCao.Rows[dongDangIn];
                        if (dongDangIn % 2 == 1)
                            e.Graphics.FillRectangle(stripeBrush, bounds.Left, y, tableWidth, rowHeight);

                        x = bounds.Left;
                        for (int i = 0; i < cotIn.Count; i++)
                        {
                            string columnName = cotIn[i].Name;
                            object value = row[columnName];
                            string text = FormattaValoreStampa(value, columnName);
                            RectangleF cell = new RectangleF(x + 4, y, widths[i] - 8, rowHeight);
                            bool numero = columnName == "SoHoaDon" || columnName == "SoMayBan" || columnName == "DoanhThu" || columnName == "GiaVon" || columnName == "LoiNhuan";
                            cellFormat.Alignment = numero ? StringAlignment.Far : StringAlignment.Near;
                            e.Graphics.DrawString(text, bodyFont, brush, cell, cellFormat);
                            x += widths[i];
                        }
                        e.Graphics.DrawLine(linePen, bounds.Left, y + rowHeight, bounds.Right, y + rowHeight);
                        y += rowHeight;
                        dongDangIn++;
                    }
                }

                e.Graphics.DrawString("Trang " + trangDangIn, bodyFont, brush,
                    new RectangleF(bounds.Left, bounds.Bottom - 18, bounds.Width, 18),
                    new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
                e.HasMorePages = duLieuBaoCao != null && dongDangIn < duLieuBaoCao.Rows.Count;
            }
        }

        private static string FormattaValoreStampa(object value, string columnName)
        {
            if (value == null || value == DBNull.Value) return string.Empty;
            if (columnName == "DoanhThu" || columnName == "GiaVon" || columnName == "LoiNhuan")
                return Convert.ToDecimal(value).ToString("#,0") + " đ";
            if (columnName == "SoHoaDon" || columnName == "SoMayBan")
                return Convert.ToDecimal(value).ToString("#,0");
            if (value is DateTime date) return date.ToString("dd/MM/yyyy");
            return Convert.ToString(value, CultureInfo.GetCultureInfo("vi-VN"));
        }
    }
}















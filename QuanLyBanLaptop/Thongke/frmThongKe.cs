using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Chuong6_LTTQ_LMS.Classes;

namespace QuanLyBanLaptop.Thongke
{
    public partial class frmThongKe : Form
    {
        private DataProcesser dataProcesser = new DataProcesser();
        private bool isUsingDatabase = false;

        // ─── Dữ liệu mẫu ─────────────────────────────────────────────────────
        private readonly string[] THANG_NHAN = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public frmThongKe()
        {
            InitializeComponent();
        }

        public void ChuanBiHienThiTrongNoiDung()
        {
            pnlSidebar.Visible = false;
            pnlTopBar.Visible = false;
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = Point.Empty;
            pnlContent.Padding = new Padding(14, 10, 14, 10);
            MinimumSize = Size.Empty;
            WindowState = FormWindowState.Normal;
        }

        // ════════════════════════════════════════════════════════════════════════
        //  FORM LOAD
        // ════════════════════════════════════════════════════════════════════════
        private void frmThongKe_Load(object sender, EventArgs e)
        {
            KhoiTaoGiaoDien();
            dtpTuNgay.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpDenNgay.Value = DateTime.Today;
        }

        private void frmThongKe_Shown(object sender, EventArgs e)
        {
            LayDuLieu();
        }

        // ════════════════════════════════════════════════════════════════════════
        //  KHỞI TẠO GIAO DIỆN & ICON
        // ════════════════════════════════════════════════════════════════════════
        private void KhoiTaoGiaoDien()
        {
            // Logo & icon sidebar
            picLogo.Image = TaoLogoLaptop(48);
            btnMenuTrangChu.Image = TaoIconHome(22, Color.FromArgb(200, 215, 230));
            btnMenuSanPham.Image = TaoIconLaptop(22, Color.FromArgb(200, 215, 230));
            btnMenuBanHang.Image = TaoIconGioHang(22, Color.FromArgb(200, 215, 230));
            btnMenuNhapHang.Image = TaoIconNhapHang(22, Color.FromArgb(200, 215, 230));
            btnMenuNhanVien.Image = TaoIconNhanVien(22, Color.FromArgb(200, 215, 230));
            btnMenuThongKe.Image = TaoIconThongKe(22, Color.White);

            // Toolbar
            btnLamMoi.Image = TaoIconLamMoi(18, Color.White);
            btnBack.Image = TaoIconMuiTen(18, Color.White, true);
            btnDangXuat.Image = TaoIconNguon(18, Color.White);

            // Active menu indicator
            btnMenuThongKe.Paint += (s, pe) =>
            {
                using (var brush = new SolidBrush(Color.FromArgb(0, 180, 216)))
                    pe.Graphics.FillRectangle(brush, 0, 0, 5, btnMenuThongKe.Height);
            };

            // Icon cho card summary
            picIconDoanhthu.Image = TaoIconTienTe(36, Color.White);
            picIconDonHang.Image = TaoIconGioHangNho(36, Color.White);
            picIconLaptop.Image = TaoIconLaptopNho(36, Color.White);

            // Cấu hình biểu đồ
            KhoiTaoBieuDo();

            // Cấu hình DataGridView Top 5
            KhoiTaoGridTop5();
        }

        private void KhoiTaoBieuDo()
        {
            chartDoanhthu.ChartAreas[0].BackColor = Color.Transparent;
            chartDoanhthu.ChartAreas[0].AxisX.LineColor = Color.FromArgb(180, 200, 220);
            chartDoanhthu.ChartAreas[0].AxisY.LineColor = Color.FromArgb(180, 200, 220);
            chartDoanhthu.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(240, 245, 250);
            chartDoanhthu.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(240, 245, 250);
            chartDoanhthu.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 8f);
            chartDoanhthu.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Segoe UI", 8f);
            chartDoanhthu.ChartAreas[0].AxisY.LabelStyle.Format = "#,0";
            chartDoanhthu.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.FromArgb(60, 80, 100);
            chartDoanhthu.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.FromArgb(60, 80, 100);
            chartDoanhthu.BackColor = Color.Transparent;
            chartDoanhthu.Legends.Clear();
            chartDoanhthu.Series.Clear();
        }

        private void KhoiTaoGridTop5()
        {
            dgvTop5.AutoGenerateColumns = false;
            dgvTop5.AllowUserToAddRows = false;
            dgvTop5.AllowUserToDeleteRows = false;
            dgvTop5.AllowUserToResizeRows = false;
            dgvTop5.ReadOnly = true;
            dgvTop5.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTop5.MultiSelect = false;
            dgvTop5.RowHeadersVisible = false;
            dgvTop5.BorderStyle = BorderStyle.None;
            dgvTop5.BackgroundColor = Color.White;
            dgvTop5.GridColor = Color.FromArgb(235, 240, 246);
            dgvTop5.Font = new Font("Segoe UI", 9f);
            dgvTop5.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgvTop5.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 108, 189);
            dgvTop5.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTop5.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTop5.EnableHeadersVisualStyles = false;
            dgvTop5.DefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 108, 189);
            dgvTop5.DefaultCellStyle.SelectionForeColor = Color.White;

            // Tô màu các hàng alternating
            dgvTop5.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 253);

            // Columns
            dgvTop5.Columns.Clear();
            var colTen = new DataGridViewTextBoxColumn { Name = "colTen", HeaderText = "Tên máy", DataPropertyName = "TenSanPham", Width = 160, ReadOnly = true };
            var colSL = new DataGridViewTextBoxColumn { Name = "colSL", HeaderText = "Số lượng đã bán", DataPropertyName = "SoLuong", Width = 120, ReadOnly = true };
            var colDT = new DataGridViewTextBoxColumn { Name = "colDT", HeaderText = "Doanh thu", DataPropertyName = "DoanhThu", Width = 120, ReadOnly = true };
            colDT.DefaultCellStyle.Format = "#,0";
            colDT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colSL.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTen.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvTop5.Columns.AddRange(colTen, colSL, colDT);
        }

        // ════════════════════════════════════════════════════════════════════════
        //  LẤY DỮ LIỆU
        // ════════════════════════════════════════════════════════════════════════
        private void LayDuLieu()
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long tongDoanhThu = 0;
            int tongDonHang = 0;
            int tongLaptopBan = 0;
            DataTable dtTop5 = null;
            DataTable dtBieuDo = null;

            // ── Thử kết nối DB ────────────────────────────────────────────────
            try
            {
                string sqlTong = string.Format(
                    "SELECT ISNULL(SUM(ThanhTien),0) AS DoanhThu, COUNT(DISTINCT MaHD) AS SoDon, ISNULL(SUM(SoLuong),0) AS SoLaptop " +
                    "FROM CHI_TIET_HOA_DON hd " +
                    "JOIN HOA_DON h ON hd.MaHD = h.MaHD " +
                    "WHERE CAST(h.NgayLap AS DATE) BETWEEN '{0}' AND '{1}'",
                    tuNgay.ToString("yyyy-MM-dd"), denNgay.ToString("yyyy-MM-dd"));

                DataTable dtTong = dataProcesser.DocBang(sqlTong);
                if (dtTong != null && dtTong.Rows.Count > 0)
                {
                    tongDoanhThu = Convert.ToInt64(dtTong.Rows[0]["DoanhThu"]);
                    tongDonHang = Convert.ToInt32(dtTong.Rows[0]["SoDon"]);
                    tongLaptopBan = Convert.ToInt32(dtTong.Rows[0]["SoLaptop"]);
                }

                // Top 5
                string sqlTop5 = string.Format(
                    "SELECT TOP 5 sp.TenSP AS TenSanPham, SUM(ct.SoLuong) AS SoLuong, SUM(ct.ThanhTien) AS DoanhThu " +
                    "FROM CHI_TIET_HOA_DON ct " +
                    "JOIN HOA_DON h ON ct.MaHD = h.MaHD " +
                    "JOIN SAN_PHAM sp ON ct.MaSP = sp.MaSP " +
                    "WHERE CAST(h.NgayLap AS DATE) BETWEEN '{0}' AND '{1}' " +
                    "GROUP BY sp.TenSP ORDER BY DoanhThu DESC",
                    tuNgay.ToString("yyyy-MM-dd"), denNgay.ToString("yyyy-MM-dd"));
                dtTop5 = dataProcesser.DocBang(sqlTop5);

                // Biểu đồ theo tháng trong năm
                string sqlBieuDo = string.Format(
                    "SELECT MONTH(h.NgayLap) AS Thang, ISNULL(SUM(ct.ThanhTien),0) AS DoanhThu " +
                    "FROM CHI_TIET_HOA_DON ct JOIN HOA_DON h ON ct.MaHD = h.MaHD " +
                    "WHERE YEAR(h.NgayLap) = {0} " +
                    "GROUP BY MONTH(h.NgayLap) ORDER BY Thang",
                    tuNgay.Year);
                dtBieuDo = dataProcesser.DocBang(sqlBieuDo);

                isUsingDatabase = true;
            }
            catch
            {
                isUsingDatabase = false;
            }

            // ── Dữ liệu mẫu nếu không có DB ──────────────────────────────────
            if (!isUsingDatabase)
            {
                var rnd = new Random(42);
                tongDoanhThu = 56000000;
                tongDonHang = 24;
                tongLaptopBan = 38;

                dtTop5 = new DataTable();
                dtTop5.Columns.Add("TenSanPham", typeof(string));
                dtTop5.Columns.Add("SoLuong", typeof(int));
                dtTop5.Columns.Add("DoanhThu", typeof(long));
                dtTop5.Rows.Add("Tên máy 078", 10, 16000000L);
                dtTop5.Rows.Add("Tên máy ST90", 5, 3000000L);
                dtTop5.Rows.Add("Tên máy RTX", 4, 3500000L);
                dtTop5.Rows.Add("Tên máy BTX", 2, 8000000L);
                dtTop5.Rows.Add("Tên máy 83K", 3, 7500000L);

                dtBieuDo = new DataTable();
                dtBieuDo.Columns.Add("Thang", typeof(int));
                dtBieuDo.Columns.Add("DoanhThu", typeof(long));
                long[] mauBieuDo = { 550000, 700000, 850000, 620000, 900000, 780000, 1050000, 920000, 980000, 0, 0, 0 };
                for (int i = 1; i <= 12; i++)
                    dtBieuDo.Rows.Add(i, mauBieuDo[i - 1] + rnd.Next(0, 100000));
            }

            // ── Hiển thị card summary ─────────────────────────────────────────
            lblDoanhThuValue.Text = tongDoanhThu.ToString("#,0") + " VNĐ";
            lblSoDonValue.Text = tongDonHang.ToString();
            lblLaptopBanValue.Text = tongLaptopBan.ToString();

            // ── Hiển thị biểu đồ ─────────────────────────────────────────────
            VeBieuDo(dtBieuDo);

            // ── Hiển thị top 5 ────────────────────────────────────────────────
            HienThiTop5(dtTop5);

            // Trạng thái DB
            lblTrangThaiDB.Text = isUsingDatabase ? "● Đang kết nối Database" : "● Dùng dữ liệu mẫu (Offline)";
            lblTrangThaiDB.ForeColor = isUsingDatabase ? Color.FromArgb(0, 180, 100) : Color.FromArgb(230, 150, 0);
        }

        private void VeBieuDo(DataTable dt)
        {
            try
            {
                // Chờ layout hoàn tất trước khi render
                if (chartDoanhthu.Width <= 0 || chartDoanhthu.Height <= 0)
                    return;

                chartDoanhthu.Series.Clear();

                var seriesBar = new Series("DoanhThu");
                seriesBar.ChartType = SeriesChartType.Column;
                seriesBar.Color = Color.FromArgb(15, 108, 189);
                seriesBar.BorderColor = Color.FromArgb(0, 80, 160);
                seriesBar.BorderWidth = 1;

                var mapDT = new Dictionary<int, long>();
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int thang = Convert.ToInt32(row["Thang"]);
                        long dt2 = Convert.ToInt64(row["DoanhThu"]);
                        mapDT[thang] = dt2;
                    }
                }

                for (int i = 1; i <= 12; i++)
                {
                    long val = mapDT.ContainsKey(i) ? mapDT[i] : 0;
                    var dp = seriesBar.Points.Add((double)val);
                    dp.AxisLabel = THANG_NHAN[i - 1];
                    dp.Color = val > 800000 ? Color.FromArgb(0, 180, 216) : Color.FromArgb(15, 108, 189);
                }

                chartDoanhthu.Series.Add(seriesBar);
                chartDoanhthu.ChartAreas[0].RecalculateAxesScale();
            }
            catch { /* Bỏ qua lỗi render nếu form chưa sẵn sàng */ }
        }

        private void HienThiTop5(DataTable dt)
        {
            dgvTop5.DataSource = dt;

            // Tô màu hàng cao nhất bằng vàng
            if (dgvTop5.Rows.Count > 0)
            {
                dgvTop5.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 200);
                dgvTop5.Rows[0].DefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                dgvTop5.Rows[0].DefaultCellStyle.ForeColor = Color.FromArgb(120, 80, 0);
            }
        }

        // ════════════════════════════════════════════════════════════════════════
        //  SỰ KIỆN BUTTON
        // ════════════════════════════════════════════════════════════════════════
        private void btnLayDuLieu_Click(object sender, EventArgs e)
        {
            LayDuLieu();
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            XuatBaoCaoExcel();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dtpTuNgay.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpDenNgay.Value = DateTime.Today;
            LayDuLieu();
        }

        // ════════════════════════════════════════════════════════════════════════
        //  XUẤT BÁO CÁO EXCEL (CSV UTF-8)
        // ════════════════════════════════════════════════════════════════════════
        private void XuatBaoCaoExcel()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files (*.csv)|*.csv|All Files (*.*)|*.*";
            sfd.FileName = "BaoCaoDoanhThu_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";
            sfd.Title = "Lưu báo cáo Excel";

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("BÁO CÁO THỐNG KÊ DOANH THU");
                sb.AppendLine("Từ ngày: " + dtpTuNgay.Value.ToString("dd/MM/yyyy") + " - Đến ngày: " + dtpDenNgay.Value.ToString("dd/MM/yyyy"));
                sb.AppendLine("Xuất lúc: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                sb.AppendLine();

                // Summary
                sb.AppendLine("=== TỔNG QUAN ===");
                sb.AppendLine("Tổng doanh thu:," + lblDoanhThuValue.Text);
                sb.AppendLine("Số đơn hàng:," + lblSoDonValue.Text);
                sb.AppendLine("Laptop đã bán:," + lblLaptopBanValue.Text);
                sb.AppendLine();

                // Top 5
                sb.AppendLine("=== TOP 5 LAPTOP BÁN CHẠY ===");
                sb.AppendLine("STT,Tên máy,Số lượng đã bán,Doanh thu (VNĐ)");
                if (dgvTop5.DataSource is DataTable dt)
                {
                    int stt = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        sb.AppendLine(string.Format("{0},{1},{2},{3}",
                            stt++, row["TenSanPham"], row["SoLuong"],
                            Convert.ToInt64(row["DoanhThu"]).ToString("#,0")));
                    }
                }

                File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));

                DialogResult dr = MessageBox.Show(
                    "Xuất báo cáo thành công!\nBạn có muốn mở file không?",
                    "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dr == DialogResult.Yes)
                    System.Diagnostics.Process.Start(sfd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════════════════════
        //  ĐIỀU HƯỚNG SIDEBAR
        // ════════════════════════════════════════════════════════════════════════
        private void btnMenuTrangChu_Click(object sender, EventArgs e)
        {
            try { var frm = new frmTrangChu(); frm.Show(); this.Hide(); }
            catch (Exception ex) { MessageBox.Show("Chưa thể mở Trang Chủ: " + ex.Message); }
        }
        private void btnMenuSanPham_Click(object sender, EventArgs e)
        {
            try { var frm = new DanhMuc.frmSanPham(); frm.Show(); this.Hide(); }
            catch (Exception ex) { MessageBox.Show("Chưa thể mở Quản lý Laptop: " + ex.Message); }
        }
        private void btnMenuBanHang_Click(object sender, EventArgs e)
        {
            try { var frm = new HoaDon.frmBanHang(); frm.Show(); this.Hide(); }
            catch (Exception ex) { MessageBox.Show("Chưa thể mở Bán Hàng: " + ex.Message); }
        }
        private void btnMenuNhapHang_Click(object sender, EventArgs e)
        {
            try { var frm = new HoaDon.frmNhapHang(); frm.Show(); this.Hide(); }
            catch (Exception ex) { MessageBox.Show("Chưa thể mở Nhập Hàng: " + ex.Message); }
        }
        private void btnMenuNhanVien_Click(object sender, EventArgs e)
        {
            try { var frm = new DanhMuc.frmNhanVien(); frm.Show(); this.Hide(); }
            catch (Exception ex) { MessageBox.Show("Chưa thể mở Nhân Viên: " + ex.Message); }
        }
        private void btnMenuCauHinhTraGop_Click(object sender, EventArgs e)
        {
            try { var frm = new DanhMuc.frmCauHinhTraGop(); frm.Show(this); this.Hide(); }
            catch (Exception ex) { MessageBox.Show("Chưa thể mở Cấu hình Trả Góp: " + ex.Message); }
        }
        private void btnBack_Click(object sender, EventArgs e) { btnMenuTrangChu_Click(sender, e); }
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try { var frm = new frmDangNhap(); frm.Show(); this.Hide(); }
                catch { this.Close(); }
            }
        }

        // ════════════════════════════════════════════════════════════════════════
        //  VẼ ICON BẰNG GDI+ (tái sử dụng từ frmNhanVien)
        // ════════════════════════════════════════════════════════════════════════
        private Bitmap TaoLogoLaptop(int size)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var path = new GraphicsPath())
                {
                    int r = 10;
                    path.AddArc(2, 2, r, r, 180, 90); path.AddArc(size - 3 - r, 2, r, r, 270, 90);
                    path.AddArc(size - 3 - r, size - 3 - r, r, r, 0, 90); path.AddArc(2, size - 3 - r, r, r, 90, 90);
                    path.CloseFigure();
                    using (var lgb = new LinearGradientBrush(new Point(0, 0), new Point(size, size), Color.FromArgb(15, 108, 189), Color.FromArgb(0, 180, 216)))
                        g.FillPath(lgb, path);
                }
                using (Pen pen = new Pen(Color.White, 2.2f))
                {
                    g.DrawRectangle(pen, 11, 10, 26, 17);
                    g.DrawLine(pen, size / 2 - 2, 27, size / 2 - 2, 32);
                    g.DrawLine(pen, 11, 32, size - 13, 32);
                }
            }
            return bmp;
        }

        private Bitmap TaoIconHome(int size, Color color)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 2f))
                {
                    Point[] roof = { new Point(1, size / 2), new Point(size / 2, 2), new Point(size - 1, size / 2) };
                    g.DrawLines(pen, roof);
                    g.DrawRectangle(pen, 4, size / 2, size - 9, size - size / 2 - 3);
                    g.DrawRectangle(pen, size / 2 - 3, size - 7, 6, 6);
                }
            }
            return bmp;
        }

        private Bitmap TaoIconLaptop(int size, Color color)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 2f))
                {
                    g.DrawRectangle(pen, 3, 4, size - 7, size - 11);
                    g.DrawLine(pen, 1, size - 4, size - 2, size - 4);
                }
            }
            return bmp;
        }

        private Bitmap TaoIconGioHang(int size, Color color)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 2f))
                {
                    g.DrawLine(pen, 1, 3, 4, 3);
                    Point[] cart = { new Point(4, 3), new Point(6, 14), new Point(size - 3, 14), new Point(size - 1, 6), new Point(5, 6) };
                    g.DrawLines(pen, cart);
                    g.DrawEllipse(pen, 6, 15, 3, 3); g.DrawEllipse(pen, size - 7, 15, 3, 3);
                }
            }
            return bmp;
        }

        private Bitmap TaoIconNhapHang(int size, Color color)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 2f))
                {
                    g.DrawRectangle(pen, 2, 8, size - 5, size - 11);
                    g.DrawLine(pen, 7, 8, 7, 4); g.DrawLine(pen, size - 8, 8, size - 8, 4);
                    g.DrawLine(pen, 7, 4, size - 8, 4);
                    g.DrawLine(pen, size / 2, 10, size / 2, size - 5);
                    Point[] arr = { new Point(size / 2 - 3, 13), new Point(size / 2, 10), new Point(size / 2 + 3, 13) };
                    g.DrawLines(pen, arr);
                }
            }
            return bmp;
        }

        private Bitmap TaoIconNhanVien(int size, Color color)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 2f))
                {
                    g.DrawEllipse(pen, size / 2 - 4, 2, 8, 8);
                    g.DrawArc(pen, 2, 12, size - 5, 9, 180, 180);
                }
            }
            return bmp;
        }

        private Bitmap TaoIconThongKe(int size, Color color)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Brush br = new SolidBrush(color))
                {
                    g.FillRectangle(br, 2, size - 7, 4, 6);
                    g.FillRectangle(br, 9, size - 12, 4, 11);
                    g.FillRectangle(br, 16, size - 9, 4, 8);
                }
                using (Pen pen = new Pen(color, 1.5f))
                    g.DrawLine(pen, 1, size - 1, size - 1, size - 1);
            }
            return bmp;
        }

        private Bitmap TaoIconLamMoi(int size, Color color)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 2f))
                {
                    g.DrawArc(pen, 2, 2, size - 5, size - 5, -30, 270);
                    Point[] arr = { new Point(size - 4, 2), new Point(size - 4, 7), new Point(size - 9, 7) };
                    g.DrawLines(pen, arr);
                }
            }
            return bmp;
        }

        private Bitmap TaoIconMuiTen(int size, Color color, bool trai)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 2f))
                {
                    int cx = size / 2, cy = size / 2;
                    if (trai)
                    {
                        g.DrawLine(pen, cx + 3, 3, cx - 3, cy);
                        g.DrawLine(pen, cx - 3, cy, cx + 3, size - 3);
                    }
                    else
                    {
                        g.DrawLine(pen, cx - 3, 3, cx + 3, cy);
                        g.DrawLine(pen, cx + 3, cy, cx - 3, size - 3);
                    }
                }
            }
            return bmp;
        }

        private Bitmap TaoIconNguon(int size, Color color)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 2f))
                {
                    g.DrawArc(pen, 3, 4, size - 7, size - 7, -60, 300);
                    g.DrawLine(pen, size / 2, 1, size / 2, size / 2);
                }
            }
            return bmp;
        }

        // Icon cho card
        private Bitmap TaoIconTienTe(int size, Color color)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 2.5f))
                {
                    g.DrawEllipse(pen, 3, 3, size - 7, size - 7);
                    g.DrawString("₫", new Font("Segoe UI", size * 0.35f, FontStyle.Bold), new SolidBrush(color),
                        new RectangleF(0, 0, size, size),
                        new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
            }
            return bmp;
        }

        private Bitmap TaoIconGioHangNho(int size, Color color)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 2.5f))
                {
                    g.DrawString("🛒", new Font("Segoe UI Emoji", size * 0.4f), new SolidBrush(color),
                        new RectangleF(0, 0, size, size),
                        new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
            }
            return bmp;
        }

        private Bitmap TaoIconLaptopNho(int size, Color color)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 2.5f))
                {
                    int m = 4;
                    g.DrawRectangle(pen, m, m + 2, size - m * 2, size - m * 2 - 8);
                    g.DrawLine(pen, 2, size - m, size - 3, size - m);
                }
            }
            return bmp;
        }
    }
}

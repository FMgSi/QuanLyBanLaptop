using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmQuanLyTraGop : AppForm
    {
        private DataTable _dtMaster;
        private string _selectedMaHD = "", _selectedTenKH = "", _selectedSDT = "";
        private int _selectedChuKy = 0;
        public frmQuanLyTraGop()
        {
            InitializeComponent();
            NavigationManager.RegisterForm(NavigationMenu.TheoDoiTraGop, this);
            sidebarControl1.SelectedMenu = NavigationMenu.TheoDoiTraGop;
            SetupGrids();
            SetupCardBordersAndIcons();
            pnlSplit.IsSplitterFixed = true;
            pnlSplit.SizeChanged += (s, e) => AdjustSplitter();
            Resize += (s, e) => AdjustSplitter();
        }
        private void AdjustSplitter()
        {
            try
            {
                if (pnlSplit.Width > 700)
                {
                    int target = (int)(pnlSplit.Width * 0.58);
                    if (pnlSplit.Width - target < 460) target = pnlSplit.Width - 460;
                    pnlSplit.SplitterDistance = target;
                }
            }
            catch { }
        }
        private void SetupGrids()
        {
            dgvHopDong.AllowUserToResizeColumns = true;
            dgvHopDong.AllowUserToResizeRows = dgvHopDong.AllowUserToOrderColumns = dgvHopDong.AutoGenerateColumns = false;
            dgvHopDong.Columns.Clear();
            dgvHopDong.ScrollBars = ScrollBars.Both;
            dgvHopDong.Columns.AddRange(
                new DataGridViewTextBoxColumn { DataPropertyName = "MaHD", HeaderText = "Mã HĐ", Width = 95, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9F, FontStyle.Bold) } },
                new DataGridViewTextBoxColumn { DataPropertyName = "TenKH", HeaderText = "Khách Hàng", Width = 135, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 9F, FontStyle.Bold) } },
                new DataGridViewTextBoxColumn { DataPropertyName = "SDT", HeaderText = "SĐT", Width = 95, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                new DataGridViewTextBoxColumn { DataPropertyName = "KyHanStr", HeaderText = "Kỳ Hạn", Width = 55, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9F, FontStyle.Bold) } },
                new DataGridViewTextBoxColumn { DataPropertyName = "TongTienStr", HeaderText = "Tổng Tiền", Width = 105, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Font = new Font("Segoe UI", 9F, FontStyle.Bold) } },
                new DataGridViewTextBoxColumn { DataPropertyName = "ConNoStr", HeaderText = "Còn Nợ", Width = 105, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, ForeColor = Color.FromArgb(29, 78, 216), Font = new Font("Segoe UI", 9F, FontStyle.Bold) } },
                new DataGridViewTextBoxColumn { DataPropertyName = "TinhTrangText", HeaderText = "Tình Trạng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 105, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvHopDong.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvHopDong.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 64, 175);
            dgvHopDong.CellPainting += DgvHopDong_CellPainting;
            dgvLichTraGop.AllowUserToResizeColumns = true;
            dgvLichTraGop.AllowUserToResizeRows = dgvLichTraGop.AllowUserToOrderColumns = dgvLichTraGop.AutoGenerateColumns = false;
            dgvLichTraGop.Columns.Clear();
            dgvLichTraGop.ScrollBars = ScrollBars.Both;
            dgvLichTraGop.Columns.AddRange(
                new DataGridViewTextBoxColumn { DataPropertyName = "KyStr", HeaderText = "Kỳ", Width = 50, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9F, FontStyle.Bold) } },
                new DataGridViewTextBoxColumn { DataPropertyName = "HanTraStr", HeaderText = "Hạn Trả", Width = 85, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                new DataGridViewTextBoxColumn { DataPropertyName = "TienKyStr", HeaderText = "Tiền Kỳ", Width = 95, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Font = new Font("Segoe UI", 9F, FontStyle.Bold) } },
                new DataGridViewTextBoxColumn { DataPropertyName = "QuaHanStr", HeaderText = "Quá Hạn", Width = 65, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, ForeColor = Color.FromArgb(71, 85, 105), Font = new Font("Segoe UI", 8.5F, FontStyle.Regular) } },
                new DataGridViewTextBoxColumn { DataPropertyName = "PhatStr", HeaderText = "Phạt", Width = 75, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, ForeColor = Color.FromArgb(71, 85, 105), Font = new Font("Segoe UI", 8.5F, FontStyle.Regular) } },
                new DataGridViewTextBoxColumn { DataPropertyName = "TrangThaiStr", HeaderText = "Trạng Thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 90, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvLichTraGop.CellPainting += DgvLichTraGop_CellPainting;
            dgvLichTraGop.RowPrePaint += DgvLichTraGop_RowPrePaint;
        }
        private void SetupCardBordersAndIcons()
        {
            foreach (var c in new Control[] { pnlCard1, pnlCard2, pnlCard3, pnlCard4, pnlCustomerInfo, pnlFilter })
                PaintCardBorder(c, Color.FromArgb(226, 232, 240));
            picIcon1.Image = CreateKpiIcon(1);
            picIcon2.Image = CreateKpiIcon(2);
            picIcon3.Image = CreateKpiIcon(3);
            picIcon4.Image = CreateKpiIcon(4);
            pnlNotePill.Resize += (s, e) => UpdatePillRegion();
            UpdatePillRegion();
        }
        private void UpdatePillRegion()
        {
            try
            {
                if (pnlNotePill.Width > pnlNotePill.Height && pnlNotePill.Height > 0)
                {
                    using (var path = new GraphicsPath())
                    {
                        path.AddArc(0, 0, pnlNotePill.Height, pnlNotePill.Height, 90, 180);
                        path.AddArc(pnlNotePill.Width - pnlNotePill.Height, 0, pnlNotePill.Height, pnlNotePill.Height, 270, 180);
                        path.CloseFigure();
                        pnlNotePill.Region = new Region(path);
                    }
                }
            }
            catch { }
        }
        private void PaintCardBorder(Control c, Color? borderColor = null)
        {
            Color color = borderColor ?? Color.FromArgb(226, 232, 240);
            c.Paint += (s, e) => { using (var pen = new Pen(color, 1)) e.Graphics.DrawRectangle(pen, 0, 0, c.Width - 1, c.Height - 1); };
        }
        private Bitmap CreateKpiIcon(int type)
        {
            var bmp = new Bitmap(24, 24);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                if (type == 1)
                {
                    using (var p = new Pen(Color.FromArgb(59, 130, 246), 1.6f))
                    {
                        g.DrawRectangle(p, 4, 3, 16, 18);
                        g.DrawLine(p, 7, 8, 17, 8);
                        g.DrawLine(p, 7, 12, 17, 12);
                        g.DrawLine(p, 7, 16, 13, 16);
                    }
                }
                else if (type == 2)
                {
                    using (var p = new Pen(Color.FromArgb(37, 99, 235), 1.6f))
                    {
                        g.DrawLine(p, 12, 3, 12, 20);
                        g.DrawLine(p, 4, 6, 20, 6);
                        g.DrawLine(p, 4, 6, 2, 12);
                        g.DrawLine(p, 20, 6, 22, 12);
                        g.DrawArc(p, 1, 10, 6, 4, 0, 180);
                        g.DrawArc(p, 17, 10, 6, 4, 0, 180);
                    }
                }
                else if (type == 3)
                {
                    using (var p = new Pen(Color.FromArgb(37, 99, 235), 1.6f))
                    using (var b = new SolidBrush(Color.FromArgb(37, 99, 235)))
                    {
                        g.DrawPolygon(p, new[] { new PointF(12, 3), new PointF(3, 20), new PointF(21, 20) });
                        g.DrawLine(p, 12, 8, 12, 14);
                        g.FillEllipse(b, 11, 16, 2, 2);
                    }
                }
                else
                {
                    using (var p = new Pen(Color.FromArgb(37, 99, 235), 1.8f))
                    {
                        g.DrawEllipse(p, 3, 3, 18, 18);
                        g.DrawLine(p, 7, 12, 11, 16);
                        g.DrawLine(p, 11, 16, 17, 8);
                    }
                }
            }
            return bmp;
        }
        private void frmQuanLyTraGop_Load(object sender, EventArgs e)
        {
            cboTrangThai.SelectedIndex = 0;
            AdjustSplitter();
            LoadData();
        }
        public void LoadData()
        {
            LoadKPIs();
            LoadDanhSachHopDong();
            sidebarControl1.RefreshOverdueBadge();
        }
        private void LoadKPIs()
        {
            try
            {
                object c1 = DataProcesser.ExecuteScalar("SELECT COUNT(*) FROM HOA_DON WHERE ChuKyTraGop > 0");
                lblCardVal1.Text = ((c1 != null && c1 != DBNull.Value) ? Convert.ToInt32(c1) : 0) + " đơn";
                object c2 = DataProcesser.ExecuteScalar("SELECT ISNULL(SUM(SoTienPhaiTra), 0) FROM LICH_TRA_GOP WHERE NgayTra IS NULL");
                lblCardVal2.Text = Function.FormatTien((c2 != null && c2 != DBNull.Value) ? Convert.ToDecimal(c2) : 0);
                object c3 = DataProcesser.ExecuteScalar("SELECT COUNT(DISTINCT MaHD) FROM LICH_TRA_GOP WHERE NgayTra IS NULL AND NgayDenHan < CAST(GETDATE() AS DATE)");
                lblCardVal3.Text = ((c3 != null && c3 != DBNull.Value) ? Convert.ToInt32(c3) : 0) + " đơn";
                object c4 = DataProcesser.ExecuteScalar("SELECT COUNT(*) FROM HOA_DON hd WHERE hd.ChuKyTraGop > 0 AND NOT EXISTS (SELECT 1 FROM LICH_TRA_GOP l WHERE l.MaHD = hd.MaHD AND l.NgayTra IS NULL)");
                lblCardVal4.Text = ((c4 != null && c4 != DBNull.Value) ? Convert.ToInt32(c4) : 0) + " đơn";
            }
            catch { }
        }
        private void LoadDanhSachHopDong()
        {
            string keyword = txtTimKiem.Text.Trim();
            int filterMode = cboTrangThai.SelectedIndex;
            try
            {
                DataTable raw = DataProcesser.ExecuteReader(@"SELECT hd.MaHD, kh.TenKH, kh.SDT, hd.ChuKyTraGop, hd.TongTien,
                    ISNULL(SUM(CASE WHEN l.NgayTra IS NULL THEN l.SoTienPhaiTra ELSE 0 END), 0) AS ConNo,
                    COUNT(CASE WHEN l.NgayTra IS NULL AND l.NgayDenHan < CAST(GETDATE() AS DATE) THEN 1 END) AS SoKyTre,
                    COUNT(CASE WHEN l.NgayTra IS NULL THEN 1 END) AS SoKyChuaDong, hd.TinhTrang
                    FROM HOA_DON hd JOIN KHACH_HANG kh ON hd.MaKH = kh.MaKH LEFT JOIN LICH_TRA_GOP l ON hd.MaHD = l.MaHD
                    WHERE hd.ChuKyTraGop > 0 GROUP BY hd.MaHD, kh.TenKH, kh.SDT, hd.ChuKyTraGop, hd.TongTien, hd.TinhTrang ORDER BY hd.MaHD DESC");
                _dtMaster = new DataTable();
                _dtMaster.Columns.AddRange(new[]
                {
                    new DataColumn("MaHD", typeof(string)), new DataColumn("TenKH", typeof(string)), new DataColumn("SDT", typeof(string)),
                    new DataColumn("ChuKyTraGop", typeof(int)), new DataColumn("KyHanStr", typeof(string)), new DataColumn("TongTien", typeof(decimal)),
                    new DataColumn("TongTienStr", typeof(string)), new DataColumn("ConNo", typeof(decimal)), new DataColumn("ConNoStr", typeof(string)),
                    new DataColumn("TinhTrangText", typeof(string)), new DataColumn("SoKyTre", typeof(int)), new DataColumn("SoKyChuaDong", typeof(int))
                });
                foreach (DataRow r in raw.Rows)
                {
                    string maHD = r["MaHD"].ToString(), tenKH = r["TenKH"].ToString(), sdt = r["SDT"].ToString();
                    int chuKy = Convert.ToInt32(r["ChuKyTraGop"]), soTre = Convert.ToInt32(r["SoKyTre"]), soChua = Convert.ToInt32(r["SoKyChuaDong"]);
                    decimal tong = Convert.ToDecimal(r["TongTien"]), conno = Convert.ToDecimal(r["ConNo"]);
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        string kw = keyword.ToLower();
                        if (!maHD.ToLower().Contains(kw) && !tenKH.ToLower().Contains(kw) && !sdt.ToLower().Contains(kw)) continue;
                    }
                    string tinhTrang = soChua == 0 ? "Hoàn tất" : soTre > 0 ? "Có kỳ trễ" : "Đang góp";
                    if ((filterMode == 1 && soTre == 0) || (filterMode == 2 && (soTre > 0 || soChua == 0)) || (filterMode == 3 && soChua > 0)) continue;
                    _dtMaster.Rows.Add(maHD, tenKH, sdt, chuKy, chuKy + " T", tong, Function.FormatTien(tong), conno, Function.FormatTien(conno), tinhTrang, soTre, soChua);
                }
                dgvHopDong.DataSource = _dtMaster;
                if (dgvHopDong.Rows.Count > 0) { dgvHopDong.Rows[0].Selected = true; HienThiChiTietDong(0); }
                else XoaTrangChiTiet();
            }
            catch (Exception ex) { Function.ThongBaoLoi("Lỗi tải danh sách hợp đồng trả góp: " + ex.Message, "Lỗi"); }
        }
        private void dgvHopDong_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHopDong.SelectedRows.Count > 0) HienThiChiTietDong(dgvHopDong.SelectedRows[0].Index);
        }
        private void HienThiChiTietDong(int rowIndex)
        {
            if (_dtMaster == null || rowIndex < 0 || rowIndex >= _dtMaster.Rows.Count) return;
            DataRow r = _dtMaster.Rows[rowIndex];
            _selectedMaHD = r["MaHD"].ToString();
            _selectedTenKH = r["TenKH"].ToString();
            _selectedSDT = r["SDT"].ToString();
            _selectedChuKy = Convert.ToInt32(r["ChuKyTraGop"]);
            decimal conno = Convert.ToDecimal(r["ConNo"]);
            lblDetailTitle.Text = "LỊCH TRẢ GÓP: " + _selectedMaHD;
            lblCustomerName.Text = "Khách: " + _selectedTenKH;
            lblCustomerPhone.Text = "SĐT: " + _selectedSDT;
            string tenLaptop = "Laptop";
            try
            {
                object lt = DataProcesser.ExecuteScalar("SELECT TOP 1 lt.TenLaptop FROM CHI_TIET_HD ct JOIN LAPTOP lt ON ct.MaLaptop=lt.MaLaptop WHERE ct.MaHD=@Ma", new[] { new SqlParameter("@Ma", _selectedMaHD) });
                if (lt != null && lt != DBNull.Value) tenLaptop = lt.ToString();
            }
            catch { }
            lblCustomerLaptop.Text = "Laptop: " + tenLaptop;
            lblCustomerConNo.Text = "Còn nợ: " + Function.FormatTien(conno);
            LoadLichTraGop(_selectedMaHD);
        }
        private void XoaTrangChiTiet()
        {
            _selectedMaHD = "";
            lblDetailTitle.Text = "LỊCH TRẢ GÓP: (Chưa chọn)";
            lblCustomerName.Text = "Khách: -";
            lblCustomerPhone.Text = "SĐT: -";
            lblCustomerLaptop.Text = "Laptop: -";
            lblCustomerConNo.Text = "Còn nợ: 0 đ";
            dgvLichTraGop.DataSource = null;
        }
        private void LoadLichTraGop(string maHD)
        {
            try
            {
                DataTable raw = DataProcesser.ExecuteReader("SELECT KyThu, NgayDenHan, SoTienPhaiTra, NgayTra, SoTienDaTra, ISNULL(TienPhat, 0) AS TienPhat FROM LICH_TRA_GOP WHERE MaHD = @Ma ORDER BY KyThu ASC", new[] { new SqlParameter("@Ma", maHD) });
                var dtDetail = new DataTable();
                dtDetail.Columns.AddRange(new[]
                {
                    new DataColumn("KyThu", typeof(int)), new DataColumn("KyStr", typeof(string)), new DataColumn("NgayDenHan", typeof(DateTime)),
                    new DataColumn("HanTraStr", typeof(string)), new DataColumn("SoTienPhaiTra", typeof(decimal)), new DataColumn("TienKyStr", typeof(string)),
                    new DataColumn("QuaHanStr", typeof(string)), new DataColumn("PhatStr", typeof(string)), new DataColumn("TrangThaiStr", typeof(string)),
                    new DataColumn("IsDaDong", typeof(bool))
                });
                DateTime today = DateTime.Now.Date;
                decimal mucPhatNgay = 20000m;
                try
                {
                    object objPhat = DataProcesser.ExecuteScalar("SELECT GiaTri FROM CAU_HINH WHERE Khoa = 'TIEN_PHAT_NGAY'");
                    if (objPhat != null && decimal.TryParse(objPhat.ToString(), out decimal p)) mucPhatNgay = p;
                }
                catch { }
                foreach (DataRow r in raw.Rows)
                {
                    int ky = Convert.ToInt32(r["KyThu"]);
                    DateTime han = Convert.ToDateTime(r["NgayDenHan"]);
                    decimal phaiTra = Convert.ToDecimal(r["SoTienPhaiTra"]), phatDb = Convert.ToDecimal(r["TienPhat"]);
                    bool daDong = r["NgayTra"] != DBNull.Value;
                    string quaHanStr = "-", phatStr = "-", trangThai = "Chưa đóng";
                    if (daDong)
                    {
                        trangThai = "Đã đóng";
                        if (phatDb > 0) phatStr = Function.FormatTien(phatDb);
                    }
                    else if (today > han.Date)
                    {
                        int days = (int)(today - han.Date).TotalDays;
                        quaHanStr = days + " ngày";
                        phatStr = Function.FormatTien(days * mucPhatNgay);
                        trangThai = "Quá hạn";
                    }
                    dtDetail.Rows.Add(ky, "Kỳ " + ky, han, han.ToString("dd/MM/yyyy"), phaiTra, Function.FormatTien(phaiTra), quaHanStr, phatStr, trangThai, daDong);
                }
                dgvLichTraGop.DataSource = dtDetail;
            }
            catch { }
        }
        private void DgvHopDong_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 6) return;
            e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
            string val = e.Value?.ToString() ?? "";
            Color backPill, textPill, borderPill;
            if (val.Contains("trễ")) { backPill = Color.FromArgb(254, 242, 242); borderPill = Color.FromArgb(254, 202, 202); textPill = Color.FromArgb(185, 28, 28); val = "Có kỳ trễ"; }
            else if (val.Contains("tất")) { backPill = Color.FromArgb(238, 242, 255); borderPill = Color.FromArgb(191, 219, 254); textPill = Color.FromArgb(30, 58, 138); val = "Hoàn tất"; }
            else { backPill = Color.FromArgb(239, 246, 255); borderPill = Color.FromArgb(191, 219, 254); textPill = Color.FromArgb(29, 78, 216); val = "Đang góp"; }
            DrawStatusPill(e.Graphics, e.CellBounds, 96, 24, val, backPill, borderPill, textPill);
            e.Handled = true;
        }
        private void DgvLichTraGop_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvLichTraGop.Rows.Count) return;
            var row = dgvLichTraGop.Rows[e.RowIndex];
            row.DefaultCellStyle.BackColor = Color.White;
            row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            row.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 58, 138);
        }
        private void DgvLichTraGop_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 5) return;
            e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
            string val = e.Value?.ToString() ?? "";
            if (val == "Chưa đóng")
            {
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (var brushText = new SolidBrush(Color.FromArgb(100, 116, 139)))
                using (var font = new Font("Segoe UI", 8.25F, FontStyle.Regular))
                    e.Graphics.DrawString("Chưa đóng", font, brushText, e.CellBounds, sf);
                e.Handled = true;
                return;
            }
            Color backPill, textPill, borderPill;
            if (val == "Đã đóng") { backPill = Color.FromArgb(238, 242, 255); borderPill = Color.FromArgb(191, 219, 254); textPill = Color.FromArgb(30, 58, 138); }
            else if (val == "Quá hạn") { backPill = Color.FromArgb(254, 242, 242); borderPill = Color.FromArgb(254, 202, 202); textPill = Color.FromArgb(185, 28, 28); }
            else { backPill = Color.FromArgb(241, 245, 249); borderPill = Color.FromArgb(203, 213, 225); textPill = Color.FromArgb(100, 116, 139); }
            DrawStatusPill(e.Graphics, e.CellBounds, 82, 22, val, backPill, borderPill, textPill);
            e.Handled = true;
        }
        private static void DrawStatusPill(Graphics g, Rectangle cellBounds, int w, int h, string text, Color back, Color border, Color fore)
        {
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var brushBack = new SolidBrush(back))
            using (var penBorder = new Pen(border, 1))
            using (var brushText = new SolidBrush(fore))
            using (var font = new Font("Segoe UI", 8.25F, FontStyle.Bold))
            {
                var rect = new Rectangle(cellBounds.X + (cellBounds.Width - w) / 2, cellBounds.Y + (cellBounds.Height - h) / 2, w, h);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillRectangle(brushBack, rect);
                g.DrawRectangle(penBorder, rect);
                g.DrawString(text, font, brushText, rect, sf);
            }
        }
        private void btnTimKiem_Click(object sender, EventArgs e) => LoadDanhSachHopDong();
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; LoadDanhSachHopDong(); } }
        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e) => LoadDanhSachHopDong();
        private void btnLamMoi_Click(object sender, EventArgs e) { txtTimKiem.Clear(); cboTrangThai.SelectedIndex = 0; LoadData(); }
        private void btnThuTien_Click(object sender, EventArgs e)
        {
            if (dgvLichTraGop.SelectedRows.Count == 0) { Function.ThongBaoCanhBao("Vui lòng nhấp chọn kỳ trả góp cần thu tiền trên bảng lịch!", "Thông báo"); return; }
            var row = dgvLichTraGop.SelectedRows[0];
            if ((row.Cells[5].Value?.ToString() ?? "").Contains("Đã đóng")) { Function.ThongBaoThanhCong("Kỳ trả góp này đã được đóng trước đó rồi!", "Thông báo"); return; }
            int kyThu = 1;
            DateTime hanTra = DateTime.Today;
            decimal tienKy = 0;
            if (row.DataBoundItem is DataRowView drv)
            {
                kyThu = Convert.ToInt32(drv["KyThu"]);
                hanTra = Convert.ToDateTime(drv["NgayDenHan"]);
                tienKy = Convert.ToDecimal(drv["SoTienPhaiTra"]);
            }
            else
            {
                kyThu = Convert.ToInt32(row.Cells[0].Value.ToString().Replace("Kỳ ", "").Trim());
                DateTime.TryParseExact(row.Cells[1].Value?.ToString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out hanTra);
                decimal.TryParse(row.Cells[2].Value.ToString().Replace(" đ", "").Replace(".", "").Replace(",", "").Trim(), out tienKy);
            }
            using (var dlg = new frmThuTienTraGop(_selectedMaHD, _selectedTenKH, _selectedSDT, kyThu, _selectedChuKy, hanTra, tienKy))
                if (dlg.ShowDialog(this) == DialogResult.OK) LoadData();
        }
        private void btnInLich_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedMaHD)) { Function.ThongBaoCanhBao("Vui lòng chọn một hợp đồng để in lịch trả góp!", "Thông báo"); return; }
            try
            {
                object tObj = DataProcesser.ExecuteScalar("SELECT TongTien FROM HOA_DON WHERE MaHD = @Ma", new[] { new SqlParameter("@Ma", _selectedMaHD) });
                decimal tongTien = (tObj != null && tObj != DBNull.Value) ? Convert.ToDecimal(tObj) : 0;
                DataTable dtItems = DataProcesser.ExecuteReader("SELECT ct.MaLaptop, lt.TenLaptop, ct.SoLuong, ct.DonGia, ct.ThanhTien, ct.ThoiGianBaoHanh FROM CHI_TIET_HD ct JOIN LAPTOP lt ON ct.MaLaptop=lt.MaLaptop WHERE ct.MaHD=@Ma", new[] { new SqlParameter("@Ma", _selectedMaHD) });
                var plan = Services.InstallmentCalculator.Load(_selectedMaHD, tongTien, _selectedChuKy);
                using (var preview = new frmPrintPreview(new HoaDonPrintData
                {
                    MaHD = _selectedMaHD, NgayLap = DateTime.Now, TenKH = _selectedTenKH, SDT = _selectedSDT,
                    TenNV = Function.CurrentTenNV ?? "Trần Thị Bình", MaNV = Function.CurrentMaNV ?? "NV002",
                    IsTraGop = true, PhuongThuc = "Trả góp", ChuKy = _selectedChuKy, LaiSuat = (double)plan.Rate,
                    TongTien = tongTien, TienTraTruoc = plan.DownPayment, TienVay = plan.Principal, TienLai = plan.Interest, TienHangThang = plan.Monthly, DtChiTiet = dtItems
                })) preview.ShowDialog(this);
            }
            catch (Exception ex) { Function.ThongBaoLoi("Lỗi in lịch trả góp: " + ex.Message, "Lỗi"); }
        }
        private void dgvLichTraGop_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}

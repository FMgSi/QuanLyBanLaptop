using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmQuanLyTraGop : Form
    {
        private DataTable _dtMaster;
        private string _selectedMaHD = "";
        private string _selectedTenKH = "";
        private string _selectedSDT = "";
        private int _selectedChuKy = 0;

        public frmQuanLyTraGop()
        {
            InitializeComponent();

            NavigationManager.RegisterForm(NavigationMenu.TheoDoiTraGop, this);
            sidebarControl1.SelectedMenu = NavigationMenu.TheoDoiTraGop;

            SetupGrids();
            SetupCardBordersAndIcons();

            pnlSplit.SizeChanged += (s, e) => AdjustSplitter();
            this.Resize += (s, e) => AdjustSplitter();
        }

        private void AdjustSplitter()
        {
            try
            {
                if (pnlSplit.Width > 700)
                {
                    int detailWidth = 490;
                    int target = pnlSplit.Width - detailWidth;
                    if (target >= 580)
                    {
                        pnlSplit.SplitterDistance = target;
                    }
                    else
                    {
                        pnlSplit.SplitterDistance = (int)(pnlSplit.Width * 0.58);
                    }
                }
            }
            catch { }
        }

        private void SetupGrids()
        {
            // Cấu hình dgvHopDong (Master)
            dgvHopDong.Columns.Clear();
            dgvHopDong.AutoGenerateColumns = false;
            dgvHopDong.ScrollBars = ScrollBars.Vertical;

            dgvHopDong.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaHD",
                HeaderText = "Mã HĐ",
                Width = 95,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                }
            });

            dgvHopDong.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenKH",
                HeaderText = "Khách Hàng",
                Width = 135,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                }
            });

            dgvHopDong.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SDT",
                HeaderText = "SĐT",
                Width = 95,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvHopDong.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "KyHanStr",
                HeaderText = "Kỳ Hạn",
                Width = 55,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                }
            });

            dgvHopDong.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TongTienStr",
                HeaderText = "Tổng Tiền",
                Width = 105,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                }
            });

            dgvHopDong.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ConNoStr",
                HeaderText = "Còn Nợ",
                Width = 105,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.FromArgb(29, 78, 216),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                }
            });

            dgvHopDong.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TinhTrangText",
                HeaderText = "Tình Trạng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 105,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvHopDong.DefaultCellStyle.SelectionBackColor = Color.FromArgb(29, 78, 216);
            dgvHopDong.DefaultCellStyle.SelectionForeColor = Color.White;

            // Vẽ badge màu ở cột Tình Trạng
            dgvHopDong.CellPainting += DgvHopDong_CellPainting;

            // Cấu hình dgvLichTraGop (Detail)
            dgvLichTraGop.Columns.Clear();
            dgvLichTraGop.AutoGenerateColumns = false;
            dgvLichTraGop.ScrollBars = ScrollBars.Vertical;

            dgvLichTraGop.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "KyStr",
                HeaderText = "Kỳ",
                Width = 50,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                }
            });

            dgvLichTraGop.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "HanTraStr",
                HeaderText = "Hạn Trả",
                Width = 85,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvLichTraGop.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TienKyStr",
                HeaderText = "Tiền Kỳ",
                Width = 95,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                }
            });

            dgvLichTraGop.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "QuaHanStr",
                HeaderText = "Quá Hạn",
                Width = 65,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    ForeColor = Color.FromArgb(71, 85, 105),
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Regular)
                }
            });

            dgvLichTraGop.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PhatStr",
                HeaderText = "Phạt",
                Width = 75,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = Color.FromArgb(71, 85, 105),
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Regular)
                }
            });

            dgvLichTraGop.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThaiStr",
                HeaderText = "Trạng Thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvLichTraGop.CellPainting += DgvLichTraGop_CellPainting;
            dgvLichTraGop.RowPrePaint += DgvLichTraGop_RowPrePaint;
        }

        private void SetupCardBordersAndIcons()
        {
            // Viền và bo tròn cho các card KPI
            PaintCardBorder(pnlCard1, Color.FromArgb(226, 232, 240));
            PaintCardBorder(pnlCard2, Color.FromArgb(226, 232, 240));
            PaintCardBorder(pnlCard3, Color.FromArgb(226, 232, 240));
            PaintCardBorder(pnlCard4, Color.FromArgb(226, 232, 240));
            PaintCardBorder(pnlCustomerInfo, Color.FromArgb(226, 232, 240));
            PaintCardBorder(pnlFilter, Color.FromArgb(226, 232, 240));

            // Icon cho KPI
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
            c.Paint += (s, e) =>
            {
                using (var pen = new Pen(color, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, c.Width - 1, c.Height - 1);
                }
            };
        }

        private Bitmap CreateKpiIcon(int type)
        {
            var bmp = new Bitmap(24, 24);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                if (type == 1) // File / Doc
                {
                    using (var p = new Pen(Color.FromArgb(59, 130, 246), 1.6f))
                    {
                        g.DrawRectangle(p, 4, 3, 16, 18);
                        g.DrawLine(p, 7, 8, 17, 8);
                        g.DrawLine(p, 7, 12, 17, 12);
                        g.DrawLine(p, 7, 16, 13, 16);
                    }
                }
                else if (type == 2) // Scale / Balance
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
                else if (type == 3) // Alert triangle
                {
                    using (var p = new Pen(Color.FromArgb(37, 99, 235), 1.6f))
                    using (var b = new SolidBrush(Color.FromArgb(37, 99, 235)))
                    {
                        PointF[] pts = { new PointF(12, 3), new PointF(3, 20), new PointF(21, 20) };
                        g.DrawPolygon(p, pts);
                        g.DrawLine(p, 12, 8, 12, 14);
                        g.FillEllipse(b, 11, 16, 2, 2);
                    }
                }
                else // Check circle
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
                // 1. Tổng số hợp đồng
                object c1 = DataProcesser.ExecuteScalar("SELECT COUNT(*) FROM HOA_DON WHERE ChuKyTraGop > 0");
                int val1 = (c1 != null && c1 != DBNull.Value) ? Convert.ToInt32(c1) : 0;
                lblCardVal1.Text = val1 + " đơn";

                // 2. Tổng tiền còn nợ
                object c2 = DataProcesser.ExecuteScalar(
                    "SELECT ISNULL(SUM(SoTienPhaiTra), 0) FROM LICH_TRA_GOP WHERE NgayTra IS NULL");
                decimal val2 = (c2 != null && c2 != DBNull.Value) ? Convert.ToDecimal(c2) : 0;
                lblCardVal2.Text = Function.FormatTien(val2);

                // 3. Đơn có kỳ quá hạn
                object c3 = DataProcesser.ExecuteScalar(
                    "SELECT COUNT(DISTINCT MaHD) FROM LICH_TRA_GOP WHERE NgayTra IS NULL AND NgayDenHan < CAST(GETDATE() AS DATE)");
                int val3 = (c3 != null && c3 != DBNull.Value) ? Convert.ToInt32(c3) : 0;
                lblCardVal3.Text = val3 + " đơn";

                // 4. Đơn hoàn tất thanh toán
                object c4 = DataProcesser.ExecuteScalar(
                    "SELECT COUNT(*) FROM HOA_DON hd WHERE hd.ChuKyTraGop > 0 " +
                    " AND NOT EXISTS (SELECT 1 FROM LICH_TRA_GOP l WHERE l.MaHD = hd.MaHD AND l.NgayTra IS NULL)");
                int val4 = (c4 != null && c4 != DBNull.Value) ? Convert.ToInt32(c4) : 0;
                lblCardVal4.Text = val4 + " đơn";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi nạp KPI: " + ex.Message);
            }
        }

        private void LoadDanhSachHopDong()
        {
            string keyword = txtTimKiem.Text.Trim();
            int filterMode = cboTrangThai.SelectedIndex; // 0: Tất cả, 1: Quá hạn, 2: Đúng hạn, 3: Hoàn tất

            try
            {
                string sql = @"
SELECT 
    hd.MaHD,
    kh.TenKH,
    kh.SDT,
    hd.ChuKyTraGop,
    hd.TongTien,
    ISNULL(SUM(CASE WHEN l.NgayTra IS NULL THEN l.SoTienPhaiTra ELSE 0 END), 0) AS ConNo,
    COUNT(CASE WHEN l.NgayTra IS NULL AND l.NgayDenHan < CAST(GETDATE() AS DATE) THEN 1 END) AS SoKyTre,
    COUNT(CASE WHEN l.NgayTra IS NULL THEN 1 END) AS SoKyChuaDong,
    hd.TinhTrang
FROM HOA_DON hd
JOIN KHACH_HANG kh ON hd.MaKH = kh.MaKH
LEFT JOIN LICH_TRA_GOP l ON hd.MaHD = l.MaHD
WHERE hd.ChuKyTraGop > 0
GROUP BY hd.MaHD, kh.TenKH, kh.SDT, hd.ChuKyTraGop, hd.TongTien, hd.TinhTrang
ORDER BY hd.MaHD DESC";

                DataTable raw = DataProcesser.ExecuteReader(sql);

                // Tạo bảng hiển thị với cột định dạng
                _dtMaster = new DataTable();
                _dtMaster.Columns.Add("MaHD", typeof(string));
                _dtMaster.Columns.Add("TenKH", typeof(string));
                _dtMaster.Columns.Add("SDT", typeof(string));
                _dtMaster.Columns.Add("ChuKyTraGop", typeof(int));
                _dtMaster.Columns.Add("KyHanStr", typeof(string));
                _dtMaster.Columns.Add("TongTien", typeof(decimal));
                _dtMaster.Columns.Add("TongTienStr", typeof(string));
                _dtMaster.Columns.Add("ConNo", typeof(decimal));
                _dtMaster.Columns.Add("ConNoStr", typeof(string));
                _dtMaster.Columns.Add("TinhTrangText", typeof(string));
                _dtMaster.Columns.Add("SoKyTre", typeof(int));
                _dtMaster.Columns.Add("SoKyChuaDong", typeof(int));

                foreach (DataRow r in raw.Rows)
                {
                    string maHD   = r["MaHD"].ToString();
                    string tenKH  = r["TenKH"].ToString();
                    string sdt    = r["SDT"].ToString();
                    int chuKy     = Convert.ToInt32(r["ChuKyTraGop"]);
                    decimal tong  = Convert.ToDecimal(r["TongTien"]);
                    decimal conno = Convert.ToDecimal(r["ConNo"]);
                    int soTre     = Convert.ToInt32(r["SoKyTre"]);
                    int soChua    = Convert.ToInt32(r["SoKyChuaDong"]);

                    // Lọc theo từ khóa
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        string kw = keyword.ToLower();
                        if (!maHD.ToLower().Contains(kw) && !tenKH.ToLower().Contains(kw) && !sdt.ToLower().Contains(kw))
                            continue;
                    }

                    // Trạng thái hợp đồng
                    string tinhTrang;
                    if (soChua == 0)
                        tinhTrang = "Hoàn tất";
                    else if (soTre > 0)
                        tinhTrang = "Có kỳ trễ";
                    else
                        tinhTrang = "Đang góp";

                    // Lọc theo ComboBox
                    if (filterMode == 1 && soTre == 0) continue; // Chỉ quá hạn
                    if (filterMode == 2 && (soTre > 0 || soChua == 0)) continue; // Đúng hạn
                    if (filterMode == 3 && soChua > 0) continue; // Hoàn tất

                    _dtMaster.Rows.Add(
                        maHD, tenKH, sdt, chuKy,
                        chuKy + " T",
                        tong, Function.FormatTien(tong),
                        conno, Function.FormatTien(conno),
                        tinhTrang, soTre, soChua);
                }

                dgvHopDong.DataSource = _dtMaster;

                if (dgvHopDong.Rows.Count > 0)
                {
                    dgvHopDong.Rows[0].Selected = true;
                    HienThiChiTietDong(0);
                }
                else
                {
                    XoaTrangChiTiet();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hợp đồng trả góp: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvHopDong_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHopDong.SelectedRows.Count > 0)
            {
                int index = dgvHopDong.SelectedRows[0].Index;
                HienThiChiTietDong(index);
            }
        }

        private void HienThiChiTietDong(int rowIndex)
        {
            if (_dtMaster == null || rowIndex < 0 || rowIndex >= _dtMaster.Rows.Count) return;

            DataRow r = _dtMaster.Rows[rowIndex];
            _selectedMaHD  = r["MaHD"].ToString();
            _selectedTenKH = r["TenKH"].ToString();
            _selectedSDT   = r["SDT"].ToString();
            _selectedChuKy = Convert.ToInt32(r["ChuKyTraGop"]);
            decimal conno  = Convert.ToDecimal(r["ConNo"]);

            lblDetailTitle.Text = "LỊCH TRẢ GÓP: " + _selectedMaHD;
            lblCustomerName.Text = "Khách: " + _selectedTenKH;
            lblCustomerPhone.Text = "SĐT: " + _selectedSDT;

            // Lấy tên laptop từ chi tiết đơn hàng
            string tenLaptop = "Laptop";
            try
            {
                object lt = DataProcesser.ExecuteScalar(
                    "SELECT TOP 1 lt.TenLaptop FROM CHI_TIET_HD ct JOIN LAPTOP lt ON ct.MaLaptop=lt.MaLaptop WHERE ct.MaHD=@Ma",
                    new[] { new SqlParameter("@Ma", _selectedMaHD) });
                if (lt != null && lt != DBNull.Value) tenLaptop = lt.ToString();
            }
            catch { }

            lblCustomerLaptop.Text = "Laptop: " + tenLaptop;
            lblCustomerConNo.Text = "Còn nợ: " + Function.FormatTien(conno);

            // Nap bang lich chi tiet
            LoadLichTraGop(_selectedMaHD);
        }

        // xoa trang thong tin chi tiet lich tra gop
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
                string sql = "SELECT KyThu, NgayDenHan, SoTienPhaiTra, NgayTra, SoTienDaTra, ISNULL(TienPhat, 0) AS TienPhat " +
                             "FROM LICH_TRA_GOP WHERE MaHD = @Ma ORDER BY KyThu ASC";

                DataTable raw = DataProcesser.ExecuteReader(sql, new[] { new SqlParameter("@Ma", maHD) });

                DataTable dtDetail = new DataTable();
                dtDetail.Columns.Add("KyThu", typeof(int));
                dtDetail.Columns.Add("KyStr", typeof(string));
                dtDetail.Columns.Add("NgayDenHan", typeof(DateTime));
                dtDetail.Columns.Add("HanTraStr", typeof(string));
                dtDetail.Columns.Add("SoTienPhaiTra", typeof(decimal));
                dtDetail.Columns.Add("TienKyStr", typeof(string));
                dtDetail.Columns.Add("QuaHanStr", typeof(string));
                dtDetail.Columns.Add("PhatStr", typeof(string));
                dtDetail.Columns.Add("TrangThaiStr", typeof(string));
                dtDetail.Columns.Add("IsDaDong", typeof(bool));

                DateTime today = DateTime.Now.Date;

                foreach (DataRow r in raw.Rows)
                {
                    int ky          = Convert.ToInt32(r["KyThu"]);
                    DateTime han    = Convert.ToDateTime(r["NgayDenHan"]);
                    decimal phaiTra = Convert.ToDecimal(r["SoTienPhaiTra"]);
                    bool daDong     = (r["NgayTra"] != DBNull.Value);
                    decimal daTra   = Convert.ToDecimal(r["SoTienDaTra"]);
                    decimal phatDb  = Convert.ToDecimal(r["TienPhat"]);

                    string quaHanStr = "-";
                    string phatStr   = "-";
                    string trangThai = "Chưa đóng";

                    if (daDong)
                    {
                        trangThai = "Đã đóng";
                        if (phatDb > 0) phatStr = Function.FormatTien(phatDb);
                    }
                    else
                    {
                        if (today > han.Date)
                        {
                            int days = (int)(today - han.Date).TotalDays;
                            quaHanStr = days + " ngày";
                            decimal phat = days * 20000m;
                            phatStr   = Function.FormatTien(phat);
                            trangThai = "Quá hạn";
                        }
                    }

                    dtDetail.Rows.Add(
                        ky, "Kỳ " + ky,
                        han, han.ToString("dd/MM/yyyy"),
                        phaiTra, Function.FormatTien(phaiTra),
                        quaHanStr, phatStr, trangThai, daDong);
                }

                dgvLichTraGop.DataSource = dtDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi tải lịch: " + ex.Message);
            }
        }

        // Vẽ pill badge cho Master DataGridView (Tình Trạng)
        private void DgvHopDong_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 6) // Cột Tình Trạng
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                string val = e.Value?.ToString() ?? "";
                Color backPill;
                Color textPill;
                Color borderPill;

                if (val.Contains("trễ"))
                {
                    backPill   = Color.FromArgb(254, 242, 242);
                    borderPill = Color.FromArgb(254, 202, 202);
                    textPill   = Color.FromArgb(185, 28, 28);
                    val = "Có kỳ trễ";
                }
                else if (val.Contains("tất"))
                {
                    backPill   = Color.FromArgb(238, 242, 255);
                    borderPill = Color.FromArgb(191, 219, 254);
                    textPill   = Color.FromArgb(30, 58, 138);
                    val = "Hoàn tất";
                }
                else
                {
                    backPill   = Color.FromArgb(239, 246, 255);
                    borderPill = Color.FromArgb(191, 219, 254);
                    textPill   = Color.FromArgb(29, 78, 216);
                    val = "Đang góp";
                }

                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (var brushBack = new SolidBrush(backPill))
                using (var penBorder = new Pen(borderPill, 1))
                using (var brushText = new SolidBrush(textPill))
                using (var font = new Font("Segoe UI", 8.25F, FontStyle.Bold))
                {
                    int w = 96;
                    int h = 24;
                    int x = e.CellBounds.X + (e.CellBounds.Width - w) / 2;
                    int y = e.CellBounds.Y + (e.CellBounds.Height - h) / 2;
                    var rect = new Rectangle(x, y, w, h);

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillRectangle(brushBack, rect);
                    e.Graphics.DrawRectangle(penBorder, rect);
                    e.Graphics.DrawString(val, font, brushText, rect, sf);
                }

                e.Handled = true;
            }
        }

        private void DgvLichTraGop_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvLichTraGop.Rows.Count)
            {
                var row = dgvLichTraGop.Rows[e.RowIndex];
                row.DefaultCellStyle.BackColor = Color.White;
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
                row.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 58, 138);
            }
        }

        // Vẽ pill badge cho Detail DataGridView (Trạng Thái Kỳ)
        private void DgvLichTraGop_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 5) // Cột Trạng Thái
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                string val = e.Value?.ToString() ?? "";

                if (val == "Chưa đóng")
                {
                    using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    using (var brushText = new SolidBrush(Color.FromArgb(100, 116, 139)))
                    using (var font = new Font("Segoe UI", 8.25F, FontStyle.Regular))
                    {
                        e.Graphics.DrawString("Chưa đóng", font, brushText, e.CellBounds, sf);
                    }
                    e.Handled = true;
                    return;
                }

                Color backPill;
                Color textPill;
                Color borderPill;

                if (val == "Đã đóng")
                {
                    backPill   = Color.FromArgb(238, 242, 255);
                    borderPill = Color.FromArgb(191, 219, 254);
                    textPill   = Color.FromArgb(30, 58, 138);
                    val = "Đã đóng";
                }
                else if (val == "Quá hạn")
                {
                    backPill   = Color.FromArgb(254, 242, 242);
                    borderPill = Color.FromArgb(254, 202, 202);
                    textPill   = Color.FromArgb(185, 28, 28);
                    val = "Quá hạn";
                }
                else
                {
                    backPill   = Color.FromArgb(241, 245, 249);
                    borderPill = Color.FromArgb(203, 213, 225);
                    textPill   = Color.FromArgb(100, 116, 139);
                }

                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (var brushBack = new SolidBrush(backPill))
                using (var penBorder = new Pen(borderPill, 1))
                using (var brushText = new SolidBrush(textPill))
                using (var font = new Font("Segoe UI", 8.25F, FontStyle.Bold))
                {
                    int w = 82;
                    int h = 22;
                    int x = e.CellBounds.X + (e.CellBounds.Width - w) / 2;
                    int y = e.CellBounds.Y + (e.CellBounds.Height - h) / 2;
                    var rect = new Rectangle(x, y, w, h);

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillRectangle(brushBack, rect);
                    e.Graphics.DrawRectangle(penBorder, rect);
                    e.Graphics.DrawString(val, font, brushText, rect, sf);
                }

                e.Handled = true;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e) => LoadDanhSachHopDong();

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoadDanhSachHopDong();
            }
        }

        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e) => LoadDanhSachHopDong();

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboTrangThai.SelectedIndex = 0;
            LoadData();
        }

        // ═══════════════════════════════════════════════════════════════════
        // THU TIỀN KỲ TRẢ GÓP ĐANG CHỌN
        // ═══════════════════════════════════════════════════════════════════
        private void btnThuTien_Click(object sender, EventArgs e)
        {
            if (dgvLichTraGop.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng nhấp chọn kỳ trả góp cần thu tiền trên bảng lịch!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgvLichTraGop.SelectedRows[0];
            string trangThai = row.Cells[5].Value?.ToString() ?? "";

            if (trangThai.Contains("Đã đóng"))
            {
                MessageBox.Show("Kỳ trả góp này đã được đóng trước đó rồi!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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
                string rawTien = row.Cells[2].Value.ToString().Replace(" đ", "").Replace(".", "").Replace(",", "").Trim();
                decimal.TryParse(rawTien, out tienKy);
            }

            using (var dlg = new frmThuTienTraGop(
                _selectedMaHD, _selectedTenKH, _selectedSDT,
                kyThu, _selectedChuKy, hanTra, tienKy))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // Tải lại toàn bộ dữ liệu sau khi thu thành công
                    LoadData();
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // IN TOÀN BỘ LỊCH TRẢ GÓP
        // ═══════════════════════════════════════════════════════════════════
        private void btnInLich_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedMaHD))
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng để in lịch trả góp!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Mở cửa sổ xem trước hóa đơn trả góp
                decimal tongTien = 0;
                object tObj = DataProcesser.ExecuteScalar(
                    "SELECT TongTien FROM HOA_DON WHERE MaHD = @Ma",
                    new[] { new SqlParameter("@Ma", _selectedMaHD) });
                if (tObj != null && tObj != DBNull.Value) tongTien = Convert.ToDecimal(tObj);

                DataTable dtItems = DataProcesser.ExecuteReader(
                    "SELECT ct.MaLaptop, lt.TenLaptop, ct.SoLuong, ct.DonGia, ct.ThanhTien, ct.ThoiGianBaoHanh " +
                    "FROM CHI_TIET_HD ct JOIN LAPTOP lt ON ct.MaLaptop=lt.MaLaptop WHERE ct.MaHD=@Ma",
                    new[] { new SqlParameter("@Ma", _selectedMaHD) });

                double laiSuat = 4.0;
                if (_selectedChuKy == 3) laiSuat = 2.4;
                else if (_selectedChuKy == 6) laiSuat = 4.0;
                else if (_selectedChuKy == 9) laiSuat = 6.0;
                else if (_selectedChuKy == 12) laiSuat = 8.0;

                decimal downpayment = Math.Round(tongTien * 0.30m, 0);
                decimal loan = tongTien - downpayment;
                decimal interest = Math.Round(loan * (decimal)(laiSuat / 100.0), 0);
                decimal monthly = Math.Floor((loan + interest) / Math.Max(1, _selectedChuKy));

                var printData = new HoaDonPrintData
                {
                    MaHD = _selectedMaHD,
                    NgayLap = DateTime.Now,
                    TenKH = _selectedTenKH,
                    SDT = _selectedSDT,
                    TenNV = Function.CurrentTenNV ?? "Trần Thị Bình",
                    MaNV = Function.CurrentMaNV ?? "NV002",
                    IsTraGop = true,
                    PhuongThuc = "Trả góp",
                    ChuKy = _selectedChuKy,
                    LaiSuat = laiSuat,
                    TongTien = tongTien,
                    TienTraTruoc = downpayment,
                    TienVay = loan,
                    TienLai = interest,
                    TienHangThang = monthly,
                    DtChiTiet = dtItems
                };

                using (var preview = new frmPrintPreview(printData))
                {
                    preview.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in lịch trả góp: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvLichTraGop_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

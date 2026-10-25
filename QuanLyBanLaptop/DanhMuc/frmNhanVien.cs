using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Chuong6_LTTQ_LMS.Classes;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmNhanVien : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SendMessageW")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        private DataTable dtNhanVien;
        private DataProcesser dataProcesser = new DataProcesser();
        private bool isUsingDatabase = false;
        private bool isDangThemMoi = false;
        private bool dangHienMatKhau = false;
        private Form trangNoiDungDangMo;

        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            KhoiTaoGiaoDien();
            TaiDuLieu();
        }

        #region Giao diện & Biểu tượng (Icons)

        private void KhoiTaoGiaoDien()
        {
            // Thiết lập ComboBox chức vụ
            if (cboChucVu.Items.Count > 0)
            {
                cboChucVu.SelectedIndex = 0;
            }

            // Vẽ Logo Laptop trên thanh Sidebar
            picLogo.Image = TaoLogoLaptop(48);

            // Vẽ Icon cho thanh Menu Sidebar
            btnMenuTrangChu.Image = TaoIconHome(22, Color.FromArgb(200, 215, 230));
            btnMenuSanPham.Image = TaoIconLaptop(22, Color.FromArgb(200, 215, 230));
            btnMenuBanHang.Image = TaoIconGioHang(22, Color.FromArgb(200, 215, 230));
            btnMenuNhapHang.Image = TaoIconNhapHang(22, Color.FromArgb(200, 215, 230));
            btnMenuNhanVien.Image = TaoIconNhanVien(22, Color.White);
            btnMenuThongKe.Image = TaoIconThongKe(22, Color.FromArgb(200, 215, 230));

            // Vẽ Icon cho Toolbar ở TopBar
            btnLamMoi.Image = TaoIconLamMoi(18, Color.White);
            btnBack.Image = TaoIconMuiTen(18, Color.White, true);
            btnForward.Image = TaoIconMuiTen(18, Color.White, false);
            btnDangXuat.Image = TaoIconNguon(18, Color.White);

            CapNhatMenuDangChon(btnMenuNhanVien);

            // Tùy chỉnh DataGridView
            dgvNhanVien.AutoGenerateColumns = false;
            pnlLeft.Width = 300;
            grpThongTinNV.Width = grpTaiKhoan.Width = 290;
            txtMaNV.Width = txtHoTen.Width = txtSDT.Width = cboChucVu.Width = 194;
            txtSDT.MaxLength = 10;
            txtHoTen.KeyPress += txtHoTen_KeyPress;
            txtSDT.KeyPress += txtSDT_KeyPress;
            txtTenDangNhap.Width = 137;
            txtMatKhau.Width = 137;
            btnHienMatKhau.Image = TaoIconMatKhau(false);
            btnHienMatKhau.ImageAlign = ContentAlignment.MiddleCenter;
            btnHienMatKhau.AccessibleName = "Hiện mật khẩu";
            var tooltipMatKhau = new ToolTip();
            tooltipMatKhau.SetToolTip(txtMatKhau, "Mật khẩu lưu dạng mã hóa không thể xem lại; để trống khi sửa để giữ mật khẩu hiện tại.");
            tooltipMatKhau.SetToolTip(btnHienMatKhau, "Hiện mật khẩu đang nhập");
            SendMessage(txtTimKiem.Handle, 0x1501, (IntPtr)1, "Tìm Kiếm");

            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.RowHeadersVisible = false;
            dgvNhanVien.RowTemplate.Height = 34;
            dgvNhanVien.ColumnHeadersHeight = 38;
            dgvNhanVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvNhanVien.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 249, 252);
            dgvNhanVien.Columns["colMaNV"].FillWeight = 13F;
            dgvNhanVien.Columns["colHoTen"].FillWeight = 27F;
            dgvNhanVien.Columns["colSDT"].FillWeight = 18F;
            dgvNhanVien.Columns["colChucVu"].FillWeight = 19F;
            dgvNhanVien.Columns["colTrangThai"].FillWeight = 23F;
            dgvNhanVien.CellFormatting += dgvNhanVien_CellFormatting;

            pnlButtons.Resize += (s, e) => CanhNutDanhSach();
            CanhNutDanhSach();
        }

        private Bitmap TaoIconMatKhau(bool hien)
        {
            var bitmap = new Bitmap(20, 20);
            using (Graphics g = Graphics.FromImage(bitmap))
            using (var pen = new Pen(Color.FromArgb(75, 85, 99), 1.7f))
            using (var brush = new SolidBrush(Color.FromArgb(75, 85, 99)))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawBezier(pen, 2, 10, 6, 3, 14, 3, 18, 10);
                g.DrawBezier(pen, 2, 10, 6, 17, 14, 17, 18, 10);
                g.FillEllipse(brush, 7, 7, 6, 6);
                if (!hien) g.DrawLine(pen, 3, 17, 17, 3);
            }
            return bitmap;
        }

        private bool MatKhauCoTheLaChuoiMaHoa(string giaTri)
        {
            if (string.IsNullOrWhiteSpace(giaTri)) return false;
            string giaTriThuong = giaTri.Trim();
            return giaTriThuong.Length >= 32 || giaTriThuong.StartsWith("$2", StringComparison.Ordinal) ||
                   giaTriThuong.StartsWith("pbkdf2", StringComparison.OrdinalIgnoreCase) ||
                   giaTriThuong.StartsWith("sha", StringComparison.OrdinalIgnoreCase) || giaTriThuong.Contains(":");
        }
        private void btnHienMatKhau_Click(object sender, EventArgs e)
        {
            dangHienMatKhau = !dangHienMatKhau;
            txtMatKhau.UseSystemPasswordChar = !dangHienMatKhau;
            var iconCu = btnHienMatKhau.Image;
            btnHienMatKhau.Image = TaoIconMatKhau(dangHienMatKhau);
            iconCu?.Dispose();
            btnHienMatKhau.AccessibleName = dangHienMatKhau ? "Ẩn mật khẩu" : "Hiện mật khẩu";
        }
        private void CanhNutDanhSach()
        {
            if (pnlButtons == null || btnThem == null) return;
            Button[] controls = { btnThem, btnSua, btnKhoaTK, btnXoa, btnCapLaiMK };
            int[] widths = { 132, 120, 150, 95, 132 };
            int available = Math.Max(350, pnlButtons.ClientSize.Width);
            int total = widths.Sum();
            double scale = Math.Min(1.0, (double)(available - 24) / total);
            int[] buttonWidths = widths.Select(width => Math.Max(68, (int)Math.Floor(width * scale))).ToArray();
            int remaining = Math.Max(0, available - buttonWidths.Sum());
            int gap = remaining / (controls.Length + 1);
            int extraPixels = remaining % (controls.Length + 1);
            int x = gap + (extraPixels > 0 ? 1 : 0);
            for (int i = 0; i < controls.Length; i++)
            {
                controls[i].Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                controls[i].Width = buttonWidths[i];
                controls[i].Left = x;
                controls[i].Top = 3;
                x += controls[i].Width + gap;
                if (extraPixels > i + 1) x++;
            }
        }

        private void CapNhatMenuDangChon(Button nutDangChon)
        {
            Button[] cacNutMenu = { btnMenuTrangChu, btnMenuSanPham, btnMenuBanHang, btnMenuNhapHang, btnMenuNhanVien, btnMenuCauHinhTraGop, btnMenuThongKe };
            foreach (Button nut in cacNutMenu)
            {
                bool dangChon = nut == nutDangChon;
                nut.BackColor = dangChon ? Color.FromArgb(29, 74, 121) : Color.FromArgb(14, 42, 71);
                nut.ForeColor = dangChon ? Color.White : Color.Gainsboro;
                nut.Font = new Font("Segoe UI", 10.5F, dangChon ? FontStyle.Bold : FontStyle.Regular);
                nut.UseVisualStyleBackColor = false;
            }
        }

        private void HienThiTrangNoiDung(Form trang, string tieuDe, Button nutMenu)
        {
            HienThiTrangNhanVien(true);

            trangNoiDungDangMo = trang;
            trang.TopLevel = false;
            trang.FormBorderStyle = FormBorderStyle.None;
            trang.WindowState = FormWindowState.Normal;
            trang.Dock = DockStyle.Fill;
            trang.Margin = Padding.Empty;

            var trangThongKe = trang as QuanLyBanLaptop.Thongke.frmThongKe;
            if (trangThongKe != null) trangThongKe.ChuanBiHienThiTrongNoiDung();

            pnlLeft.Visible = false;
            pnlRight.Visible = false;
            pnlContent.Controls.Add(trang);
            trang.BringToFront();
            lblTitle.Text = tieuDe;
            Text = tieuDe;
            CapNhatMenuDangChon(nutMenu);
            trang.Show();
        }

        private void HienThiTrangNhanVien(bool dongTrangNoiDung = true)
        {
            if (dongTrangNoiDung && trangNoiDungDangMo != null)
            {
                Form trangCu = trangNoiDungDangMo;
                trangNoiDungDangMo = null;
                pnlContent.Controls.Remove(trangCu);
                trangCu.Close();
                trangCu.Dispose();
            }

            pnlLeft.Visible = true;
            pnlRight.Visible = true;
            pnlContent.PerformLayout();
            lblTitle.Text = "TRANG QUẢN LÝ NHÂN VIÊN & TÀI KHOẢN";
            Text = "TRANG QUẢN LÝ NHÂN VIÊN & TÀI KHOẢN";
            CapNhatMenuDangChon(btnMenuNhanVien);
        }

        private void dgvNhanVien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvNhanVien.Columns[e.ColumnIndex].Name != "colTrangThai") return;
            string trangThai = Convert.ToString(e.Value);
            e.CellStyle.ForeColor = trangThai == "Đang hoạt động" ? Color.FromArgb(34, 120, 80) : Color.FromArgb(180, 55, 55);
            e.CellStyle.Font = dgvNhanVien.DefaultCellStyle.Font;
        }

        private void txtHoTen_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (e.KeyChar < '0' || e.KeyChar > '9'))
                e.Handled = true;
        }

        private bool HoTenHopLe(string hoTen)
        {
            return !string.IsNullOrWhiteSpace(hoTen) && Regex.IsMatch(hoTen.Trim(), @"^[\p{L}]+(?:\s+[\p{L}]+)*$");
        }

        private bool SoDienThoaiHopLe(string sdt)
        {
            return Regex.IsMatch((sdt ?? string.Empty).Trim(), @"^[0-9]{10}$");
        }

        private Bitmap TaoLogoLaptop(int size)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (GraphicsPath path = new GraphicsPath())
                {
                    int r = 10;
                    path.AddArc(2, 2, r, r, 180, 90);
                    path.AddArc(size - 3 - r, 2, r, r, 270, 90);
                    path.AddArc(size - 3 - r, size - 3 - r, r, r, 0, 90);
                    path.AddArc(2, size - 3 - r, r, r, 90, 90);
                    path.CloseFigure();

                    using (LinearGradientBrush lgb = new LinearGradientBrush(new Point(0, 0), new Point(size, size),
                        Color.FromArgb(15, 108, 189), Color.FromArgb(0, 180, 216)))
                    {
                        g.FillPath(lgb, path);
                    }
                }

                using (Pen pen = new Pen(Color.White, 2.2f))
                {
                    g.DrawRectangle(pen, 11, 10, 26, 17);
                    g.DrawLine(pen, 7, 30, 41, 30);
                    using (Pen p2 = new Pen(Color.FromArgb(180, 230, 255), 1.5f))
                    {
                        g.DrawLine(p2, 21, 30, 27, 30);
                    }
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
                    Point[] roof = { new Point(size / 2, 2), new Point(2, 10), new Point(size - 3, 10) };
                    g.DrawLines(pen, roof);
                    g.DrawRectangle(pen, 4, 10, size - 9, size - 12);
                    g.DrawRectangle(pen, size / 2 - 2, size - 7, 4, 5);
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
                using (Pen pen = new Pen(color, 1.8f))
                {
                    g.DrawRectangle(pen, 3, 3, size - 7, size - 11);
                    g.DrawLine(pen, 1, size - 5, size - 2, size - 5);
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
                using (Pen pen = new Pen(color, 1.8f))
                {
                    Point[] cart = {
                        new Point(2, 4),
                        new Point(5, 4),
                        new Point(8, 14),
                        new Point(size - 3, 14),
                        new Point(size - 1, 6),
                        new Point(6, 6)
                    };
                    g.DrawLines(pen, cart);
                    g.FillEllipse(new SolidBrush(color), 7, size - 5, 4, 4);
                    g.FillEllipse(new SolidBrush(color), size - 6, size - 5, 4, 4);
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
                using (Pen pen = new Pen(color, 1.8f))
                {
                    g.DrawRectangle(pen, 3, 7, size - 7, size - 10);
                    g.DrawLine(pen, 3, 10, size - 4, 10);
                    g.DrawLine(pen, size / 2, 1, size / 2, 7);
                    g.DrawLine(pen, size / 2 - 3, 4, size / 2, 7);
                    g.DrawLine(pen, size / 2 + 3, 4, size / 2, 7);
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
                    g.DrawArc(pen, 2, 11, size - 5, 12, 180, 180);
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
                using (Pen pen = new Pen(color, 1.8f))
                {
                    g.DrawLine(pen, 2, 2, 2, size - 3);
                    g.DrawLine(pen, 2, size - 3, size - 2, size - 3);

                    using (SolidBrush b = new SolidBrush(color))
                    {
                        g.FillRectangle(b, 5, size - 8, 3, 5);
                        g.FillRectangle(b, 10, size - 13, 3, 10);
                        g.FillRectangle(b, 15, size - 17, 3, 14);
                    }
                }
            }
            return bmp;
        }

        private Bitmap TaoIconLamMoi(int size, Color color)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 1.8f))
                {
                    g.DrawArc(pen, 2, 2, size - 5, size - 5, 45, 270);
                    Point[] arrow = { new Point(size - 3, size / 2 - 4), new Point(size - 3, size / 2 + 1), new Point(size - 8, size / 2 + 1) };
                    g.DrawLines(pen, arrow);
                }
            }
            return bmp;
        }

        private Bitmap TaoIconMuiTen(int size, Color color, bool isBack)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(color, 2f))
                {
                    if (isBack)
                    {
                        g.DrawLine(pen, 3, size / 2, size - 3, size / 2);
                        g.DrawLine(pen, 3, size / 2, 7, size / 2 - 4);
                        g.DrawLine(pen, 3, size / 2, 7, size / 2 + 4);
                    }
                    else
                    {
                        g.DrawLine(pen, 3, size / 2, size - 3, size / 2);
                        g.DrawLine(pen, size - 3, size / 2, size - 7, size / 2 - 4);
                        g.DrawLine(pen, size - 3, size / 2, size - 7, size / 2 + 4);
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

        #endregion

        #region Xử lý dữ liệu Nhân Viên & CSDL Database

        private void TaiDuLieu()
        {
            try
            {
                // Truy vấn bảng NHAN_VIEN trong CSDL QuanLyBanLaptop
                string sql = "SELECT MaNV, TenNV, TenNV AS HoTen, SDT, ChucVu, TenDN, TenDN AS TenDangNhap, MatKhau, CAST(CASE WHEN ISNULL(DaNghi, 0) = 0 THEN 1 ELSE 0 END AS BIT) AS TrangThai, CASE WHEN ISNULL(DaNghi, 0) = 0 THEN N'Đang hoạt động' ELSE N'Đã nghỉ' END AS TrangThaiText FROM NHAN_VIEN";
                DataTable dt = dataProcesser.DocBang(sql);

                if (dt != null)
                {
                    dtNhanVien = dt;
                    isUsingDatabase = true;
                }
                else
                {
                    TaoDuLieuMau();
                }
            }
            catch
            {
                isUsingDatabase = false;
                TaoDuLieuMau();
            }

            dgvNhanVien.DataSource = dtNhanVien;

            if (dgvNhanVien.Rows.Count > 0)
            {
                dgvNhanVien.Rows[0].Selected = true;
                HienThiThongTinDong(0);
            }
        }

        private void TaoDuLieuMau()
        {
            dtNhanVien = new DataTable();
            dtNhanVien.Columns.Add("MaNV", typeof(string));
            dtNhanVien.Columns.Add("TenNV", typeof(string));
            dtNhanVien.Columns.Add("HoTen", typeof(string));
            dtNhanVien.Columns.Add("SDT", typeof(string));
            dtNhanVien.Columns.Add("ChucVu", typeof(string));
            dtNhanVien.Columns.Add("TrangThai", typeof(bool));
            dtNhanVien.Columns.Add("TrangThaiText", typeof(string));
            dtNhanVien.Columns.Add("TenDN", typeof(string));
            dtNhanVien.Columns.Add("TenDangNhap", typeof(string));
            dtNhanVien.Columns.Add("MatKhau", typeof(string));
            dtNhanVien.Rows.Add("NV001", "Nguyễn Văn An", "Nguyễn Văn An", "0912345678", "Quản trị viên", true, "Đang hoạt động", "admin", "admin", "admin123");
            dtNhanVien.Rows.Add("NV002", "Trần Thị Bình", "Trần Thị Bình", "0987654321", "Quản trị viên", true, "Đang hoạt động", "binh_sale", "binh_sale", "123456");
            dtNhanVien.Rows.Add("NV003", "Lê Hoàng Cường", "Lê Hoàng Cường", "0905556666", "Nhân viên", true, "Đang hoạt động", "cuong_kho", "cuong_kho", "123456");
        }

        private void HienThiThongTinDong(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvNhanVien.Rows.Count)
                return;

            DataGridViewRow row = dgvNhanVien.Rows[rowIndex];

            txtMaNV.Text = row.Cells["colMaNV"].Value?.ToString() ?? "";
            txtHoTen.Text = row.Cells["colHoTen"].Value?.ToString() ?? "";
            txtSDT.Text = row.Cells["colSDT"].Value?.ToString() ?? "";
            cboChucVu.Text = row.Cells["colChucVu"].Value?.ToString() ?? "Nhân viên";

            string maNV = txtMaNV.Text;
            DataRow[] found = dtNhanVien.Select($"MaNV = '{maNV}'");
            if (found.Length > 0)
            {
                txtTenDangNhap.Text = (found[0]["TenDN"] ?? found[0]["TenDangNhap"])?.ToString() ?? "";
                string matKhauDaLuu = found[0]["MatKhau"]?.ToString() ?? "";
                txtMatKhau.Text = isUsingDatabase && MatKhauCoTheLaChuoiMaHoa(matKhauDaLuu) ? "" : matKhauDaLuu;

                bool trangThai = Convert.ToBoolean(found[0]["TrangThai"]);
                CapNhatTrangThaiKhoaGiaoDien(trangThai);
            }
        }

        private void CapNhatTrangThaiKhoaGiaoDien(bool trangThaiHoatDong)
        {
            if (trangThaiHoatDong)
            {
                lblTrangThaiValue.Text = "● Đang hoạt động";
                lblTrangThaiValue.ForeColor = Color.ForestGreen;

                btnKhoaTK.Text = "KHÓA TK";
                btnKhoaTK.BackColor = Color.FromArgb(211, 47, 47);
            }
            else
            {
                lblTrangThaiValue.Text = "● ĐÃ BỊ KHÓA";
                lblTrangThaiValue.ForeColor = Color.Crimson;

                btnKhoaTK.Text = "MỞ KHÓA TK";
                btnKhoaTK.BackColor = Color.FromArgb(46, 125, 50);
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isDangThemMoi = false;
                btnThem.Text = "THÊM NHÂN VIÊN";
                btnThem.BackColor = Color.FromArgb(15, 108, 189);

                HienThiThongTinDong(e.RowIndex);
            }
        }

        private void dgvNhanVien_SelectionChanged(object sender, EventArgs e)
        {
            if (isDangThemMoi) return;

            if (dgvNhanVien.CurrentRow != null && dgvNhanVien.CurrentRow.Index >= 0)
            {
                HienThiThongTinDong(dgvNhanVien.CurrentRow.Index);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (dtNhanVien == null) return;

            string tuKhoa = txtTimKiem.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(tuKhoa))
            {
                dtNhanVien.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                dtNhanVien.DefaultView.RowFilter = $"MaNV LIKE '%{tuKhoa}%' OR HoTen LIKE '%{tuKhoa}%' OR SDT LIKE '%{tuKhoa}%' OR ChucVu LIKE '%{tuKhoa}%'";
            }
        }

        private string TaoMaNVTuDong()
        {
            string prefix = dtNhanVien != null && dtNhanVien.AsEnumerable()
                .Any(row => Convert.ToString(row["MaNV"]).StartsWith("NV", StringComparison.OrdinalIgnoreCase))
                ? "NV"
                : dtNhanVien != null && dtNhanVien.AsEnumerable()
                    .Any(row => Convert.ToString(row["MaNV"]).StartsWith("V", StringComparison.OrdinalIgnoreCase))
                    ? "V"
                    : "NV";

            var maDaDung = new HashSet<int>();
            int maLonNhat = 0;
            if (dtNhanVien != null)
            {
                foreach (DataRow row in dtNhanVien.Rows)
                {
                    Match match = Regex.Match(Convert.ToString(row["MaNV"]), @"^(NV|V)(\d+)$", RegexOptions.IgnoreCase);
                    if (!match.Success || !match.Groups[1].Value.Equals(prefix, StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (int.TryParse(match.Groups[2].Value, out int soMa))
                    {
                        maDaDung.Add(soMa);
                        if (soMa > maLonNhat) maLonNhat = soMa;
                    }
                }
            }

            for (int soMa = 1; soMa <= maLonNhat; soMa++)
            {
                if (!maDaDung.Contains(soMa))
                    return prefix == "NV" ? "NV" + soMa.ToString("D3") : "V" + soMa;
            }

            int maTiepTheo = maLonNhat + 1;
            return prefix == "NV" ? "NV" + maTiepTheo.ToString("D3") : "V" + maTiepTheo;
        }

        #endregion

        #region Các nút chức năng (Thêm, Sửa, Khóa/Mở lại TK, Xóa, Cấp lại MK, Làm mới) - Cập nhật trực tiếp CSDL

        private void btnThem_Click(object sender, EventArgs e)
        {
            // BƯỚC 1: Nếu chưa ở trạng thái thêm mới -> Xóa sạch các ô nhập bên trái để người dùng nhập
            if (!isDangThemMoi)
            {
                isDangThemMoi = true;

                txtMaNV.Text = TaoMaNVTuDong();
                txtHoTen.Clear();
                txtSDT.Clear();
                txtTenDangNhap.Clear();
                txtMatKhau.Clear();

                if (cboChucVu.Items.Count > 1)
                    cboChucVu.SelectedIndex = 1;

                CapNhatTrangThaiKhoaGiaoDien(true);
                dgvNhanVien.ClearSelection();

                btnThem.Text = "LƯU NHÂN VIÊN";
                btnThem.BackColor = Color.FromArgb(46, 125, 50);

                txtHoTen.Focus();
                return;
            }

            // BƯỚC 2: Kiểm tra dữ liệu khi người dùng bấm Lưu
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập Họ tên nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (!HoTenHopLe(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên chỉ được gồm chữ cái và dấu cách, không được nhập số hoặc ký tự đặc biệt.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (!SoDienThoaiHopLe(txtSDT.Text))
            {
                MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên đăng nhập hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return;
            }

            string maMoi = txtMaNV.Text.Trim();
            if (string.IsNullOrWhiteSpace(maMoi))
                maMoi = TaoMaNVTuDong();
            string hoTen = txtHoTen.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string chucVu = cboChucVu.Text;
            string tenDN = txtTenDangNhap.Text.Trim();
            string matKhau = string.IsNullOrWhiteSpace(txtMatKhau.Text) ? "123456" : txtMatKhau.Text.Trim();

            // Kiểm tra trùng tên đăng nhập trong CSDL
            if (isUsingDatabase)
            {
                try
                {
                    DataTable dtKiemTra = dataProcesser.DocBang($"SELECT MaNV FROM NHAN_VIEN WHERE TenDN = '{tenDN}'");
                    if (dtKiemTra != null && dtKiemTra.Rows.Count > 0)
                    {
                        MessageBox.Show($"Tên đăng nhập '{tenDN}' đã tồn tại trong CSDL. Vui lòng chọn tên đăng nhập khác!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTenDangNhap.Focus();
                        return;
                    }
                }
                catch { }
            }

            // Cập nhật CSDL: Thêm mới vào bảng NHAN_VIEN
            if (isUsingDatabase)
            {
                try
                {
                    string sqlInsert = $"INSERT INTO NHAN_VIEN (MaNV, TenNV, SDT, ChucVu, TenDN, MatKhau) " +
                                       $"VALUES ('{maMoi}', N'{hoTen}', '{sdt}', N'{chucVu}', '{tenDN}', '{matKhau}')";
                    dataProcesser.CapNhatDuLieu(sqlInsert);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm vào CSDL: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Tải lại dữ liệu từ CSDL để đảm bảo đồng bộ 100%
            TaiDuLieu();

            MessageBox.Show($"Đã thêm nhân viên {hoTen} (Mã: {maMoi}) vào CSDL thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            isDangThemMoi = false;
            btnThem.Text = "THÊM NHÂN VIÊN";
            btnThem.BackColor = Color.FromArgb(15, 108, 189);

            // Chọn dòng vừa thêm
            for (int i = 0; i < dgvNhanVien.Rows.Count; i++)
            {
                if (dgvNhanVien.Rows[i].Cells["colMaNV"].Value?.ToString() == maMoi)
                {
                    dgvNhanVien.Rows[i].Selected = true;
                    HienThiThongTinDong(i);
                    break;
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (isDangThemMoi)
            {
                MessageBox.Show("Bạn đang ở chế độ thêm nhân viên mới. Hãy bấm [LƯU NHÂN VIÊN] để lưu hoặc bấm vào danh sách để chọn nhân viên cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvNhanVien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNV = txtMaNV.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string chucVu = cboChucVu.Text;
            string tenDN = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                MessageBox.Show("Họ tên không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (!HoTenHopLe(hoTen))
            {
                MessageBox.Show("Họ tên chỉ được gồm chữ cái và dấu cách, không được nhập số hoặc ký tự đặc biệt.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (!SoDienThoaiHopLe(sdt))
            {
                MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            // Cập nhật vào CSDL bảng NHAN_VIEN
            if (isUsingDatabase)
            {
                try
                {
                    string sqlUpdate = $"UPDATE NHAN_VIEN SET TenNV = N'{hoTen}', SDT = '{sdt}', " +
                                       $"ChucVu = N'{chucVu}', TenDN = '{tenDN}'";
                    if (!string.IsNullOrWhiteSpace(matKhau))
                        sqlUpdate += $", MatKhau = '{matKhau}'";
                    sqlUpdate += $" WHERE MaNV = '{maNV}'";
                    dataProcesser.CapNhatDuLieu(sqlUpdate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật vào CSDL: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Cập nhật lại DataTable hiển thị
            DataRow[] found = dtNhanVien.Select($"MaNV = '{maNV}'");
            if (found.Length > 0)
            {
                DataRow r = found[0];
                r["TenNV"] = hoTen;
                r["HoTen"] = hoTen;
                r["SDT"] = sdt;
                r["ChucVu"] = chucVu;
                r["TenDN"] = tenDN;
                r["TenDangNhap"] = tenDN;
                r["MatKhau"] = matKhau;
            }

            dgvNhanVien.Refresh();
            MessageBox.Show($"Cập nhật thông tin nhân viên {hoTen} vào CSDL thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnKhoaTK_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần Khóa/Mở tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNV = txtMaNV.Text.Trim();
            DataRow[] found = dtNhanVien.Select($"MaNV = '{maNV}'");
            if (found.Length == 0) return;

            DataRow row = found[0];
            string hoTen = (row["TenNV"] ?? row["HoTen"]).ToString();
            string tenDN = (row["TenDN"] ?? row["TenDangNhap"]).ToString();
            bool dangHoatDong = Convert.ToBoolean(row["TrangThai"]);

            if (dangHoatDong)
            {
                // KHÓA TÀI KHOẢN
                DialogResult dr = MessageBox.Show(
                    $"Bạn có CHẮC CHẮN muốn KHÓA tài khoản '{tenDN}' của nhân viên '{hoTen}' (Mã: {maNV}) không?\n\n" +
                    $"⚠ Chú ý: Sau khi khóa, tài khoản này sẽ KHÔNG THỂ đăng nhập vào phần mềm cho đến khi được Quản trị viên mở lại!",
                    "Xác nhận khóa tài khoản",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    if (isUsingDatabase)
                    {
                        try
                        {
                            string sql = $"UPDATE NHAN_VIEN SET DaNghi = 1 WHERE MaNV = '{maNV}'";
                            dataProcesser.CapNhatDuLieu(sql);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi cập nhật trạng thái vào CSDL: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    row["TrangThai"] = false;
                    dgvNhanVien.Refresh();
                    CapNhatTrangThaiKhoaGiaoDien(false);

                    MessageBox.Show(
                        $"Đã KHÓA thành công tài khoản '{tenDN}' (Mã: {maNV}) trong CSDL!\nTài khoản này sẽ bị khóa đến khi được bạn mở lại.",
                        "Đã khóa tài khoản",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else
            {
                // MỞ LẠI TÀI KHOẢN
                DialogResult dr = MessageBox.Show(
                    $"Bạn có muốn MỞ LẠI tài khoản '{tenDN}' cho nhân viên '{hoTen}' (Mã: {maNV}) không?\n\n" +
                    $"Tài khoản sẽ được kích hoạt trở lại trong CSDL và có thể đăng nhập bình thường.",
                    "Xác nhận mở lại tài khoản",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    if (isUsingDatabase)
                    {
                        try
                        {
                            string sql = $"UPDATE NHAN_VIEN SET DaNghi = 0 WHERE MaNV = '{maNV}'";
                            dataProcesser.CapNhatDuLieu(sql);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi cập nhật trạng thái vào CSDL: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    row["TrangThai"] = true;
                    dgvNhanVien.Refresh();
                    CapNhatTrangThaiKhoaGiaoDien(true);

                    MessageBox.Show(
                        $"Đã MỞ LẠI thành công tài khoản '{tenDN}' (Mã: {maNV}) trong CSDL!\nNhân viên hiện có thể đăng nhập bình thường.",
                        "Mở lại tài khoản thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNV = txtMaNV.Text.Trim();
            DataRow[] found = dtNhanVien.Select($"MaNV = '{maNV}'");
            if (found.Length == 0) return;

            DataRow row = found[0];
            string hoTen = (row["TenNV"] ?? row["HoTen"]).ToString();

            DialogResult confirm = MessageBox.Show(
                $"Bạn có CHẮC CHẮN muốn XÓA nhân viên '{hoTen}' (Mã: {maNV}) khỏi CSDL không?\n\n" +
                $"💡 Gợi ý: Nếu nhân viên này đã nghỉ việc nhưng từng có hóa đơn bán hàng/phiếu nhập, bạn nên dùng chức năng 'KHÓA TÀI KHOẢN' thay vì xóa hẳn để bảo toàn lịch sử hóa đơn.",
                "Cảnh báo xóa nhân viên",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                if (isUsingDatabase)
                {
                    try
                    {
                        string sqlDelete = $"DELETE FROM NHAN_VIEN WHERE MaNV = '{maNV}'";
                        dataProcesser.CapNhatDuLieu(sqlDelete);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("REFERENCE") || ex.Message.Contains("FOREIGN KEY"))
                        {
                            MessageBox.Show($"Không thể xóa nhân viên '{hoTen}' do đã có Hóa đơn hoặc Phiếu nhập liên kết với nhân viên này trong CSDL!\n\n💡 Vui lòng sử dụng chức năng [KHÓA TÀI KHOẢN] để vô hiệu hóa quyền truy cập.", "Ràng buộc dữ liệu CSDL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Lỗi khi xóa khỏi CSDL: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                row.Delete();
                dtNhanVien.AcceptChanges();
                
                if (dgvNhanVien.Rows.Count > 0)
                {
                    dgvNhanVien.Rows[0].Selected = true;
                    HienThiThongTinDong(0);
                }
                else
                {
                    txtMaNV.Text = TaoMaNVTuDong();
                    txtHoTen.Clear();
                    txtSDT.Clear();
                    txtTenDangNhap.Clear();
                    txtMatKhau.Clear();
                }

                MessageBox.Show($"Đã XÓA nhân viên '{hoTen}' khỏi CSDL thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCapLaiMK_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần cấp lại mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNV = txtMaNV.Text.Trim();
            DataRow[] found = dtNhanVien.Select($"MaNV = '{maNV}'");
            if (found.Length == 0) return;

            DataRow row = found[0];
            string hoTen = (row["TenNV"] ?? row["HoTen"]).ToString();

            DialogResult dr = MessageBox.Show(
                $"Bạn có chắc chắn muốn cấp lại mật khẩu mặc định cho nhân viên '{hoTen}'?\nMật khẩu mới sẽ được cập nhật trong CSDL là: 123456",
                "Xác nhận cấp lại mật khẩu",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                string matKhauMoi = "123456";

                if (isUsingDatabase)
                {
                    try
                    {
                        string sql = $"UPDATE NHAN_VIEN SET MatKhau = '{matKhauMoi}' WHERE MaNV = '{maNV}'";
                        dataProcesser.CapNhatDuLieu(sql);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi cập nhật mật khẩu vào CSDL: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                row["MatKhau"] = matKhauMoi;
                txtMatKhau.Text = matKhauMoi;

                MessageBox.Show($"Đã cấp lại mật khẩu mới cho nhân viên {hoTen} trong CSDL thành công!\nMật khẩu mới: 123456", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            isDangThemMoi = false;
            btnThem.Text = "THÊM NHÂN VIÊN";
            btnThem.BackColor = Color.FromArgb(15, 108, 189);

            TaiDuLieu();
            txtTimKiem.Clear();

            if (dgvNhanVien.Rows.Count > 0)
            {
                dgvNhanVien.Rows[0].Selected = true;
                HienThiThongTinDong(0);
            }
            else
            {
                txtMaNV.Text = TaoMaNVTuDong();
                txtHoTen.Clear();
                txtSDT.Clear();
                txtTenDangNhap.Clear();
                txtMatKhau.Clear();
            }
        }

        #endregion

        #region Điều hướng Menu Sidebar & Toolbar

        private void btnMenuTrangChu_Click(object sender, EventArgs e)
        {
            try
            {
                frmTrangChu frm = new frmTrangChu();
                frm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chưa thể mở Trang Chủ: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnMenuSanPham_Click(object sender, EventArgs e)
        {
            try
            {
                frmSanPham frm = new frmSanPham();
                frm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chưa thể mở Quản lý Laptop: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnMenuBanHang_Click(object sender, EventArgs e)
        {
            try
            {
                HoaDon.frmBanHang frm = new HoaDon.frmBanHang();
                frm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chưa thể mở Bán Hàng: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnMenuNhapHang_Click(object sender, EventArgs e)
        {
            try
            {
                HoaDon.frmNhapHang frm = new HoaDon.frmNhapHang();
                frm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chưa thể mở Nhập Hàng: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnMenuNhanVien_Click(object sender, EventArgs e)
        {
            HienThiTrangNhanVien();
        }

        private void btnMenuThongKe_Click(object sender, EventArgs e)
        {
            try
            {
                Thongke.frmThongKe frm = new Thongke.frmThongKe();
                HienThiTrangNoiDung(frm, "TRANG THỐNG KÊ & BÁO CÁO", btnMenuThongKe);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chưa thể mở Thống Kê: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnMenuCauHinhTraGop_Click(object sender, EventArgs e)
        {
            try
            {
                frmCauHinhTraGop frm = new frmCauHinhTraGop();
                HienThiTrangNoiDung(frm, "CẤU HÌNH TRẢ GÓP & HẠNG THÀNH VIÊN", btnMenuCauHinhTraGop);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chưa thể mở Cấu hình Trả Góp: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            btnMenuTrangChu_Click(sender, e);
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?", "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    frmDangNhap frm = new frmDangNhap();
                    frm.Show();
                    this.Hide();
                }
                catch
                {
                    this.Close();
                }
            }
        }

        #endregion
    }
}

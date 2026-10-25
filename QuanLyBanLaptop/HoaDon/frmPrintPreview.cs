using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmPrintPreview : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        private const int WM_VSCROLL = 0x0115;
        private const int SB_BOTTOM = 7;

        private readonly HoaDonPrintData _data;
        private readonly PhieuThuPrintData _phieuThuData;
        private readonly bool _isPhieuThu = false;
        private readonly PrintDocument _printDoc;

        // Bảng mã vạch Code 39 chuẩn
        private static readonly Dictionary<char, string> Code39Table = new Dictionary<char, string>
        {
            {'0', "000110100"}, {'1', "100100001"}, {'2', "001100001"}, {'3', "101100000"},
            {'4', "000110001"}, {'5', "100110000"}, {'6', "001110000"}, {'7', "000100101"},
            {'8', "100100100"}, {'9', "001100100"}, {'A', "100001001"}, {'B', "001001001"},
            {'C', "101001000"}, {'D', "000011001"}, {'E', "100011000"}, {'F', "001011000"},
            {'G', "000001101"}, {'H', "100001100"}, {'I', "001001100"}, {'J', "000011100"},
            {'K', "100000011"}, {'L', "001000011"}, {'M', "101000010"}, {'N', "000010011"},
            {'O', "100010010"}, {'P', "001010010"}, {'Q', "000000111"}, {'R', "100000110"},
            {'S', "001000110"}, {'T', "000010110"}, {'U', "110000001"}, {'V', "011000001"},
            {'W', "111000000"}, {'X', "010010001"}, {'Y', "110010000"}, {'Z', "011010000"},
            {'-', "010000101"}, {'.', "110000100"}, {' ', "011000100"}, {'*', "010010100"}
        };

        public frmPrintPreview(HoaDonPrintData data)
        {
            InitializeComponent();
            _data = data ?? new HoaDonPrintData();
            _isPhieuThu = false;

            _printDoc = new PrintDocument();
            _printDoc.DocumentName = "HoaDon_" + (_data.MaHD ?? "LaptopStore");
            _printDoc.PrintPage += PrintDoc_PrintPage;

            InitToolbarIcons();
            SetupScrollButton();
        }

        public frmPrintPreview(PhieuThuPrintData phieuThuData)
        {
            InitializeComponent();
            _phieuThuData = phieuThuData ?? new PhieuThuPrintData();
            _isPhieuThu = true;

            _printDoc = new PrintDocument();
            _printDoc.DocumentName = "PhieuThu_" + (_phieuThuData.MaHD ?? "LaptopStore");
            _printDoc.PrintPage += PrintDoc_PrintPagePhieuThu;

            InitToolbarIcons();
            SetupScrollButton();
        }

        private void frmPrintPreview_Load(object sender, EventArgs e)
        {
            if (_isPhieuThu)
            {
                this.Text = "Print preview - Phiếu Thu Trả Góp [frmPrintPreview]";
                int pageHeight = TinhTongChieuCaoPhieuThu();
                _printDoc.DefaultPageSettings.PaperSize = new PaperSize("K80_Receipt", 330, pageHeight);
            }
            else
            {
                this.Text = "Hóa đơn " + (_data?.MaHD ?? "") + " - Xem trước khi in";
                int pageHeight = TinhTongChieuCao();
                _printDoc.DefaultPageSettings.PaperSize = new PaperSize("K80_Receipt", 330, pageHeight);
            }

            _printDoc.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(10, 10, 10, 10);

            // Gắn vào PrintPreviewControl
            printPreviewControl1.Document = _printDoc;
            printPreviewControl1.AutoZoom = false;
            printPreviewControl1.Zoom = 1.0; // Hiển thị 100% kích thước chuẩn
            printPreviewControl1.Rows = 1;
            printPreviewControl1.Columns = 1;
            printPreviewControl1.InvalidatePreview();
        }

        #region Khởi tạo Icon & Giao diện Toolbar
        private void InitToolbarIcons()
        {
            tsBtnPrint.Image      = CreatePrinterIcon(18);
            tsBtnZoomIn.Image     = CreateZoomIcon(18, true);
            tsBtnZoomOut.Image    = CreateZoomIcon(18, false);
            tsBtnOnePage.Image    = CreatePageIcon(18, 1);
            tsBtnTwoPages.Image   = CreatePageIcon(18, 2);
            tsBtnThreePages.Image = CreatePageIcon(18, 3);
            tsBtnFourPages.Image  = CreatePageIcon(18, 4);

            // Style nút Close góc phải giống chuẩn thanh công cụ Windows
            tsBtnClose.Paint += (s, e) =>
            {
                var r = new Rectangle(0, 1, tsBtnClose.Width - 1, tsBtnClose.Height - 3);
                using (var pen = new Pen(Color.FromArgb(156, 163, 175), 1))
                using (var brush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillRectangle(brush, r);
                    e.Graphics.DrawRectangle(pen, r);
                }
                TextRenderer.DrawText(e.Graphics, "Close", tsBtnClose.Font, r,
                    Color.FromArgb(31, 41, 55),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
        }

        private void SetupScrollButton()
        {
            // Nút cuộn tròn màu tối ở góc dưới đáy giữa
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, btnScrollBottom.Width, btnScrollBottom.Height);
                btnScrollBottom.Region = new Region(path);
            }
            btnScrollBottom.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // Vẽ mũi tên chỉ xuống màu trắng
                using (var pen = new Pen(Color.White, 2f))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    float cx = btnScrollBottom.Width / 2f;
                    float cy = btnScrollBottom.Height / 2f;
                    e.Graphics.DrawLine(pen, cx, cy - 6, cx, cy + 6);
                    e.Graphics.DrawLine(pen, cx - 5, cy + 1, cx, cy + 6);
                    e.Graphics.DrawLine(pen, cx + 5, cy + 1, cx, cy + 6);
                }
            };
        }

        private Bitmap CreatePrinterIcon(int sz)
        {
            var bmp = new Bitmap(sz, sz);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(Color.FromArgb(30, 41, 59), 1.4f))
                using (var bDark = new SolidBrush(Color.FromArgb(30, 41, 59)))
                using (var bLight = new SolidBrush(Color.White))
                {
                    // Giấy nạp phía trên
                    g.FillRectangle(bLight, sz * 0.28f, sz * 0.12f, sz * 0.44f, sz * 0.32f);
                    g.DrawRectangle(pen, sz * 0.28f, sz * 0.12f, sz * 0.44f, sz * 0.32f);

                    // Thân máy in
                    g.FillRectangle(bDark, sz * 0.12f, sz * 0.36f, sz * 0.76f, sz * 0.36f);

                    // Tờ giấy in ra phía dưới
                    g.FillRectangle(bLight, sz * 0.25f, sz * 0.55f, sz * 0.50f, sz * 0.35f);
                    g.DrawRectangle(pen, sz * 0.25f, sz * 0.55f, sz * 0.50f, sz * 0.35f);

                    // Đèn tín hiệu
                    g.FillEllipse(Brushes.White, sz * 0.72f, sz * 0.42f, 2, 2);
                }
            }
            return bmp;
        }

        private Bitmap CreateZoomIcon(int sz, bool isPlus)
        {
            var bmp = new Bitmap(sz, sz);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(Color.FromArgb(30, 41, 59), 1.6f))
                {
                    // Vòng kính lúp
                    float dia = sz * 0.55f;
                    g.DrawEllipse(pen, 2, 2, dia, dia);

                    // Cán kính
                    g.DrawLine(pen, dia * 0.85f + 2, dia * 0.85f + 2, sz - 3, sz - 3);

                    // Dấu cộng hoặc trừ
                    float cx = dia / 2f + 2;
                    float cy = dia / 2f + 2;
                    g.DrawLine(pen, cx - 3, cy, cx + 3, cy);
                    if (isPlus)
                    {
                        g.DrawLine(pen, cx, cy - 3, cx, cy + 3);
                    }
                }
            }
            return bmp;
        }

        private Bitmap CreatePageIcon(int sz, int mode)
        {
            var bmp = new Bitmap(sz, sz);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(Color.FromArgb(30, 41, 59), 1.3f))
                using (var brush = new SolidBrush(Color.White))
                {
                    if (mode == 1)
                    {
                        // 1 trang
                        g.FillRectangle(brush, 4, 2, 10, 14);
                        g.DrawRectangle(pen, 4, 2, 10, 14);
                    }
                    else if (mode == 2)
                    {
                        // 2 trang song song
                        g.FillRectangle(brush, 2, 3, 6, 12);
                        g.DrawRectangle(pen, 2, 3, 6, 12);
                        g.FillRectangle(brush, 10, 3, 6, 12);
                        g.DrawRectangle(pen, 10, 3, 6, 12);
                    }
                    else if (mode == 3)
                    {
                        // 3 trang
                        g.FillRectangle(brush, 1, 3, 4, 12);
                        g.DrawRectangle(pen, 1, 3, 4, 12);
                        g.FillRectangle(brush, 7, 3, 4, 12);
                        g.DrawRectangle(pen, 7, 3, 4, 12);
                        g.FillRectangle(brush, 13, 3, 4, 12);
                        g.DrawRectangle(pen, 13, 3, 4, 12);
                    }
                    else
                    {
                        // 4 trang lưới 2x2
                        g.FillRectangle(brush, 2, 2, 6, 6);
                        g.DrawRectangle(pen, 2, 2, 6, 6);
                        g.FillRectangle(brush, 10, 2, 6, 6);
                        g.DrawRectangle(pen, 10, 2, 6, 6);
                        g.FillRectangle(brush, 2, 10, 6, 6);
                        g.DrawRectangle(pen, 2, 10, 6, 6);
                        g.FillRectangle(brush, 10, 10, 6, 6);
                        g.DrawRectangle(pen, 10, 10, 6, 6);
                    }
                }
            }
            return bmp;
        }
        #endregion

        #region Tính toán chiều cao hóa đơn K80
        private int TinhTongChieuCao()
        {
            int h = 30; // Margin top
            h += 90;    // Header (Tiêu đề, Tên cửa hàng, Slogan, Ngày giờ)
            h += 15;    // Đường kẻ gạch
            h += 80;    // Thông tin khách hàng (4 dòng)
            h += 15;    // Đường kẻ gạch
            h += 22;    // Tiêu đề bảng: TÊN SẢN PHẨM / THÀNH TIỀN

            int count = 0;
            if (_data.DtChiTiet != null)
            {
                foreach (DataRow r in _data.DtChiTiet.Rows)
                {
                    if (r.RowState != DataRowState.Deleted) count++;
                }
            }
            if (count == 0) count = 1;
            h += count * 42; // Mỗi sản phẩm chiếm khoảng 42 units (2 dòng)

            h += 15;    // Đường kẻ gạch

            if (_data.IsTraGop)
            {
                h += 120; // 6 dòng chi tiết trả góp
                h += 15;  // Đường kẻ gạch
                h += 24;  // Mỗi tháng trả
                h += 15;  // Đường kẻ gạch
                h += 26;  // Tổng thanh toán
                h += 15;  // Đường kẻ gạch
            }
            else
            {
                h += 50;  // 2 dòng: Hình thức, Tổng giá trị máy
                if (_data.PhuongThuc != null && _data.PhuongThuc.Contains("Tiền mặt"))
                {
                    h += 38; // Khách đưa, Tiền thừa
                }
                h += 22;  // Trạng thái đã thanh toán
                h += 15;  // Đường kẻ gạch
                h += 26;  // Tổng thanh toán
                h += 15;  // Đường kẻ gạch
            }

            h += 30; // Thu ngân
            h += 25; // Lời chúc
            h += 60; // Mã vạch Barcode & Số hóa đơn
            h += 40; // Margin bottom

            return Math.Max(560, h);
        }
        #endregion

        #region Vẽ tờ hóa đơn - Chuẩn khớp thiết kế K80 như hình ảnh mẫu
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float xLeft = 16f;
            float xRight = 314f;
            float xCenter = (xLeft + xRight) / 2f;
            float printableWidth = xRight - xLeft;

            // Font Monospace chuẩn receipt giống ảnh mẫu
            string fontName = "Consolas";
            using (var test = new Font("Consolas", 10))
            {
                if (test.Name != "Consolas") fontName = "Courier New";
            }

            using (var fontTitle      = new Font(fontName, 12f, FontStyle.Bold))
            using (var fontStore      = new Font(fontName, 9.5f, FontStyle.Bold))
            using (var fontSub        = new Font(fontName, 8.5f, FontStyle.Regular))
            using (var fontDate       = new Font(fontName, 8.5f, FontStyle.Regular))
            using (var fontRegular    = new Font(fontName, 8.5f, FontStyle.Regular))
            using (var fontBold       = new Font(fontName, 8.5f, FontStyle.Bold))
            using (var fontSmall      = new Font(fontName, 8f, FontStyle.Regular))
            using (var fontItalic     = new Font(fontName, 7.8f, FontStyle.Italic))
            using (var fontTotal      = new Font(fontName, 9.5f, FontStyle.Bold))
            using (var brushBlack     = new SolidBrush(Color.FromArgb(15, 23, 42)))
            using (var penDash        = new Pen(Color.FromArgb(70, 70, 70), 1.1f) { DashPattern = new float[] { 3, 2 } })
            {
                float y = 20f;

                // ═══════════════════════════════════════════════════════════════
                // 1. HEADER
                // ═══════════════════════════════════════════════════════════════
                string title = _data.IsTraGop ? "HÓA ĐƠN TRẢ GÓP LAPTOP" : "HÓA ĐƠN BÁN HÀNG";
                DrawCenteredText(g, title, fontTitle, brushBlack, xCenter, y);
                y += 24f;

                DrawCenteredText(g, "CỬA HÀNG LAPTOP STORE", fontStore, brushBlack, xCenter, y);
                y += 18f;

                DrawCenteredText(g, "Hệ Thống Bán Lẻ Máy Tính & Phụ Kiện", fontSub, brushBlack, xCenter, y);
                y += 17f;

                DrawCenteredText(g, _data.NgayLap.ToString("dd/MM/yyyy HH:mm"), fontDate, brushBlack, xCenter, y);
                y += 22f;

                // Đường kẻ gạch
                g.DrawLine(penDash, xLeft, y, xRight, y);
                y += 12f;

                // ═══════════════════════════════════════════════════════════════
                // 2. THÔNG TIN KHÁCH HÀNG
                // ═══════════════════════════════════════════════════════════════
                DrawTwoColumn(g, "Khách hàng:", _data.TenKH ?? "Khách vãng lai", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                y += 18f;

                string hang = string.IsNullOrEmpty(_data.HangKhach) ? "Đồng" : _data.HangKhach;
                string maKhHang = (_data.MaKH ?? "") + " (" + hang + ")";
                DrawTwoColumn(g, "Mã KH / Hạng:", maKhHang, fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                y += 18f;

                DrawTwoColumn(g, "Số điện thoại:", _data.SDT ?? "", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                y += 18f;

                DrawTwoColumn(g, "Địa chỉ:", _data.DiaChi ?? "", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                y += 20f;

                // Đường kẻ gạch
                g.DrawLine(penDash, xLeft, y, xRight, y);
                y += 12f;

                // ═══════════════════════════════════════════════════════════════
                // 3. DANH SÁCH SẢN PHẨM (TABLE)
                // ═══════════════════════════════════════════════════════════════
                g.DrawString("TÊN SẢN PHẨM", fontBold, brushBlack, xLeft, y);
                DrawRightText(g, "THÀNH TIỀN", fontBold, brushBlack, xRight, y);
                y += 20f;

                if (_data.DtChiTiet != null && _data.DtChiTiet.Rows.Count > 0)
                {
                    foreach (DataRow r in _data.DtChiTiet.Rows)
                    {
                        if (r.RowState == DataRowState.Deleted) continue;

                        int sl         = Convert.ToInt32(r["SoLuong"]);
                        string maSP    = r["MaLaptop"].ToString();
                        string tenSP   = r["TenLaptop"].ToString();
                        decimal donGia = Convert.ToDecimal(r["DonGia"]);
                        decimal tt     = Convert.ToDecimal(r["ThanhTien"]);
                        int bh         = (r.Table.Columns.Contains("ThoiGianBaoHanh") && r["ThoiGianBaoHanh"] != DBNull.Value)
                                         ? Convert.ToInt32(r["ThoiGianBaoHanh"]) : 12;

                        // Dòng 1: 2x Asus ROG Strix G15 (LT002)
                        string line1 = sl + "x " + tenSP + " (" + maSP + ")";
                        g.DrawString(line1, fontRegular, brushBlack, xLeft, y);
                        y += 17f;

                        // Dòng 2: Đơn giá: 24.500.000đ | BH: 24t         49.000.000đ
                        string line2 = "Đơn giá: " + donGia.ToString("N0") + "đ | BH: " + bh + "t";
                        g.DrawString(line2, fontSmall, brushBlack, xLeft, y);
                        DrawRightText(g, tt.ToString("N0") + "đ", fontRegular, brushBlack, xRight, y);
                        y += 22f;
                    }
                }
                else
                {
                    g.DrawString("Chưa có mặt hàng", fontItalic, brushBlack, xLeft, y);
                    y += 20f;
                }

                // Đường kẻ gạch
                g.DrawLine(penDash, xLeft, y, xRight, y);
                y += 12f;

                // ═══════════════════════════════════════════════════════════════
                // 4. CHI TIẾT THANH TOÁN
                // ═══════════════════════════════════════════════════════════════
                if (_data.IsTraGop)
                {
                    DrawTwoColumn(g, "Hình thức thanh toán:", "Trả góp chu kỳ " + _data.ChuKy + " tháng", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                    y += 18f;

                    DrawTwoColumn(g, "Tổng giá trị máy:", _data.TongTien.ToString("N0") + " đ", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                    y += 18f;

                    DrawTwoColumn(g, "Số tiền trả trước (30%):", _data.TienTraTruoc.ToString("N0") + " đ", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                    y += 18f;

                    DrawTwoColumn(g, "Số tiền vay trả góp:", _data.TienVay.ToString("N0") + " đ", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                    y += 18f;

                    DrawTwoColumn(g, "Lãi suất áp dụng:", _data.LaiSuat.ToString("0.0") + "%", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                    y += 18f;

                    DrawTwoColumn(g, "Tiền lãi phát sinh:", _data.TienLai.ToString("N0") + " đ", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                    y += 20f;

                    g.DrawLine(penDash, xLeft, y, xRight, y);
                    y += 12f;

                    string labelKy = "Mỗi tháng trả (" + _data.ChuKy + " kỳ):";
                    string valKy = _data.TienHangThang.ToString("N0") + " đ/tháng";
                    DrawTwoColumn(g, labelKy, valKy, fontBold, fontBold, brushBlack, xLeft, xRight, y);
                    y += 20f;

                    g.DrawLine(penDash, xLeft, y, xRight, y);
                    y += 14f;

                    // TỔNG THANH TOÁN (Số tiền khách thanh toán ngay tại quầy nhận máy: 30%)
                    DrawTwoColumn(g, "TỔNG THANH TOÁN:", _data.TienTraTruoc.ToString("N0") + " đ", fontTotal, fontTotal, brushBlack, xLeft, xRight, y);
                    y += 22f;

                    g.DrawLine(penDash, xLeft, y, xRight, y);
                    y += 16f;
                }
                else
                {
                    string phuongThucStr = string.IsNullOrEmpty(_data.PhuongThuc) ? "Tiền mặt" : _data.PhuongThuc;
                    DrawTwoColumn(g, "Hình thức thanh toán:", phuongThucStr, fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                    y += 18f;

                    DrawTwoColumn(g, "Tổng giá trị máy:", _data.TongTien.ToString("N0") + " đ", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                    y += 18f;

                    if (phuongThucStr.Contains("Tiền mặt"))
                    {
                        DrawTwoColumn(g, "Số tiền khách đưa:", _data.KhachDua.ToString("N0") + " đ", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                        y += 18f;

                        DrawTwoColumn(g, "Tiền thừa trả khách:", _data.TienThua.ToString("N0") + " đ", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                        y += 18f;
                    }

                    DrawTwoColumn(g, "Trạng thái:", "ĐÃ THANH TOÁN (100%)", fontRegular, fontBold, brushBlack, xLeft, xRight, y);
                    y += 20f;

                    g.DrawLine(penDash, xLeft, y, xRight, y);
                    y += 14f;

                    DrawTwoColumn(g, "TỔNG THANH TOÁN:", _data.TongTien.ToString("N0") + " đ", fontTotal, fontTotal, brushBlack, xLeft, xRight, y);
                    y += 22f;

                    g.DrawLine(penDash, xLeft, y, xRight, y);
                    y += 16f;
                }

                // ═══════════════════════════════════════════════════════════════
                // 5. FOOTER & MÃ VẠCH (BARCODE)
                // ═══════════════════════════════════════════════════════════════
                string thuNgan = "Thu ngân: " + (_data.TenNV ?? "NV") + " (" + (_data.MaNV ?? "") + ")";
                DrawCenteredText(g, thuNgan, fontBold, brushBlack, xCenter, y);
                y += 20f;

                string loichuc = "Cảm ơn quý khách đã tin tưởng và mua hàng tại Laptop Store!";
                DrawCenteredText(g, loichuc, fontItalic, brushBlack, xCenter, y);
                y += 22f;

                // Vẽ Mã vạch Barcode Code 39 chuẩn cho mã hóa đơn
                if (!string.IsNullOrEmpty(_data.MaHD))
                {
                    DrawBarcodeCode39(g, _data.MaHD, xCenter, y, 32f, brushBlack);
                    y += 36f;
                    DrawCenteredText(g, "* " + _data.MaHD + " *", fontSmall, brushBlack, xCenter, y);
                    y += 18f;
                }
            }

            e.HasMorePages = false;
        }

        private void DrawCenteredText(Graphics g, string text, Font font, Brush brush, float centerX, float y)
        {
            var size = g.MeasureString(text, font);
            g.DrawString(text, font, brush, centerX - (size.Width / 2f), y);
        }

        private void DrawRightText(Graphics g, string text, Font font, Brush brush, float rightX, float y)
        {
            var size = g.MeasureString(text, font);
            g.DrawString(text, font, brush, rightX - size.Width, y);
        }

        private void DrawTwoColumn(Graphics g, string col1, string col2, Font f1, Font f2, Brush brush, float leftX, float rightX, float y)
        {
            g.DrawString(col1, f1, brush, leftX, y);
            var size2 = g.MeasureString(col2, f2);
            g.DrawString(col2, f2, brush, rightX - size2.Width, y);
        }

        private void DrawBarcodeCode39(Graphics g, string code, float centerX, float y, float height, Brush brush)
        {
            string formatted = "*" + code.ToUpper().Trim() + "*";
            float narrow = 1.1f;
            float wide = 2.4f;

            // Tính tổng chiều rộng
            float totalWidth = 0;
            foreach (char c in formatted)
            {
                if (!Code39Table.ContainsKey(c)) continue;
                string pattern = Code39Table[c];
                for (int i = 0; i < 9; i++)
                    totalWidth += (pattern[i] == '1' ? wide : narrow);
                totalWidth += narrow; // Khoảng trắng giữa các ký tự
            }

            float startX = centerX - (totalWidth / 2f);

            // Vẽ từng vạch
            foreach (char c in formatted)
            {
                if (!Code39Table.ContainsKey(c)) continue;
                string pattern = Code39Table[c];
                for (int i = 0; i < 9; i++)
                {
                    float barW = (pattern[i] == '1' ? wide : narrow);
                    if (i % 2 == 0) // Vạch đen
                    {
                        g.FillRectangle(brush, startX, y, barW, height);
                    }
                    startX += barW;
                }
                startX += narrow; // Khoảng cách ký tự
            }
        }
        #endregion

        #region Xử lý nút bấm trên ToolStrip
        private void tsBtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dlg = new PrintDialog())
                {
                    dlg.Document = _printDoc;
                    dlg.UseEXDialog = true;
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        _printDoc.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gửi lệnh in: " + ex.Message, "Lỗi in",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsBtnZoomIn_Click(object sender, EventArgs e)
        {
            printPreviewControl1.AutoZoom = false;
            printPreviewControl1.Zoom = Math.Min(printPreviewControl1.Zoom + 0.2, 3.0);
        }

        private void tsBtnZoomOut_Click(object sender, EventArgs e)
        {
            printPreviewControl1.AutoZoom = false;
            printPreviewControl1.Zoom = Math.Max(printPreviewControl1.Zoom - 0.2, 0.3);
        }

        private void tsBtnOnePage_Click(object sender, EventArgs e)
        {
            printPreviewControl1.AutoZoom = false;
            printPreviewControl1.Rows = 1;
            printPreviewControl1.Columns = 1;
            printPreviewControl1.Zoom = 1.0;
        }

        private void tsBtnTwoPages_Click(object sender, EventArgs e)
        {
            printPreviewControl1.AutoZoom = true;
            printPreviewControl1.Rows = 1;
            printPreviewControl1.Columns = 2;
        }

        private void tsBtnThreePages_Click(object sender, EventArgs e)
        {
            printPreviewControl1.AutoZoom = true;
            printPreviewControl1.Rows = 1;
            printPreviewControl1.Columns = 3;
        }

        private void tsBtnFourPages_Click(object sender, EventArgs e)
        {
            printPreviewControl1.AutoZoom = true;
            printPreviewControl1.Rows = 2;
            printPreviewControl1.Columns = 2;
        }

        private void tsBtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnScrollBottom_Click(object sender, EventArgs e)
        {
            try
            {
                SendMessage(printPreviewControl1.Handle, WM_VSCROLL, (IntPtr)SB_BOTTOM, IntPtr.Zero);
            }
            catch { }
        }
        #endregion

        #region Tính toán & Vẽ Phiếu Thu Trả Góp K80
        private int TinhTongChieuCaoPhieuThu()
        {
            int h = 30; // Margin top
            h += 90;    // Header (PHIẾU THU TIỀN TRẢ GÓP, CỬA HÀNG LAPTOP STORE, Mã đơn hàng, Ngày giờ)
            h += 15;    // Gạch ngang
            h += 80;    // Thông tin khách hàng, SĐT, Kỳ thanh toán, Ngày đến hạn
            h += 15;    // Gạch ngang
            h += 75;    // Tiền gốc kỳ, Số ngày trễ, Tiền phạt
            h += 15;    // Gạch ngang
            h += 30;    // TỔNG THU
            h += 15;    // Gạch ngang

            if (_phieuThuData.PhuongThucThu != null && _phieuThuData.PhuongThucThu.Contains("Tiền mặt"))
            {
                h += 65; // Tiền khách đưa, Tiền thừa trả lại
            }
            else
            {
                h += 24;  // MÃ QUÉT CHUYỂN KHOẢN (VIETQR)
                h += 290; // Ảnh QR VietQR
                h += 38;  // Chú thích quét mã QR
            }

            h += 15; // Gạch ngang
            h += 30; // Thu ngân
            h += 25; // Lời chúc
            h += 35; // Margin bottom

            return Math.Max(560, h);
        }

        private void PrintDoc_PrintPagePhieuThu(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float xLeft = 16f;
            float xRight = 314f;
            float xCenter = (xLeft + xRight) / 2f;

            string fontName = "Consolas";
            using (var test = new Font("Consolas", 10))
            {
                if (test.Name != "Consolas") fontName = "Courier New";
            }

            using (var fontTitle   = new Font(fontName, 12f, FontStyle.Bold))
            using (var fontStore   = new Font(fontName, 9.5f, FontStyle.Bold))
            using (var fontSub     = new Font(fontName, 8.5f, FontStyle.Regular))
            using (var fontDate    = new Font(fontName, 8.5f, FontStyle.Regular))
            using (var fontRegular = new Font(fontName, 8.5f, FontStyle.Regular))
            using (var fontBold    = new Font(fontName, 8.5f, FontStyle.Bold))
            using (var fontItalic  = new Font(fontName, 7.8f, FontStyle.Italic))
            using (var fontTotal   = new Font(fontName, 10.5f, FontStyle.Bold))
            using (var brushBlack  = new SolidBrush(Color.FromArgb(15, 23, 42)))
            using (var brushRed    = new SolidBrush(Color.FromArgb(220, 38, 38)))
            using (var penDash     = new Pen(Color.FromArgb(70, 70, 70), 1.1f) { DashPattern = new float[] { 3, 2 } })
            {
                float y = 20f;

                // 1. HEADER
                DrawCenteredText(g, "PHIẾU THU TIỀN TRẢ GÓP", fontTitle, brushBlack, xCenter, y);
                y += 24f;

                DrawCenteredText(g, "CỬA HÀNG LAPTOP STORE", fontStore, brushBlack, xCenter, y);
                y += 18f;

                DrawCenteredText(g, "Mã đơn hàng: " + (_phieuThuData.MaHD ?? ""), fontSub, brushBlack, xCenter, y);
                y += 17f;

                DrawCenteredText(g, _phieuThuData.NgayThu.ToString("dd/MM/yyyy HH:mm"), fontDate, brushBlack, xCenter, y);
                y += 22f;

                // Gạch ngang
                g.DrawLine(penDash, xLeft, y, xRight, y);
                y += 12f;

                // 2. THÔNG TIN KHÁCH HÀNG & KỲ
                DrawTwoColumn(g, "Khách hàng:", _phieuThuData.TenKH ?? "Khách vãng lai", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                y += 18f;

                DrawTwoColumn(g, "Số điện thoại:", _phieuThuData.SDT ?? "", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                y += 18f;

                string kyText = "Kỳ thứ " + _phieuThuData.KyThu + " / " + _phieuThuData.TongSoKy;
                DrawTwoColumn(g, "Kỳ thanh toán:", kyText, fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                y += 18f;

                DrawTwoColumn(g, "Ngày đến hạn:", _phieuThuData.NgayDenHan.ToString("dd/MM/yyyy"), fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                y += 20f;

                // Gạch ngang
                g.DrawLine(penDash, xLeft, y, xRight, y);
                y += 12f;

                // 3. KÊ KHAI TÀI CHÍNH
                DrawTwoColumn(g, "Tiền gốc kỳ này:", _phieuThuData.TienGocKy.ToString("N0") + " đ", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                y += 18f;

                Brush delayBrush = _phieuThuData.SoNgayTre > 0 ? brushRed : brushBlack;
                string delayText = _phieuThuData.SoNgayTre > 0 ? (_phieuThuData.SoNgayTre + " ngày") : "Đúng hạn (0 ngày)";
                DrawTwoColumn(g, "Số ngày trễ hạn:", delayText, fontRegular, fontRegular, delayBrush, xLeft, xRight, y);
                y += 18f;

                string penaltyText = _phieuThuData.TienPhat.ToString("N0") + " đ";
                DrawTwoColumn(g, "Tiền phạt trễ (20k/ngày):", penaltyText, fontRegular, fontRegular, delayBrush, xLeft, xRight, y);
                y += 20f;

                // Gạch ngang
                g.DrawLine(penDash, xLeft, y, xRight, y);
                y += 14f;

                // 4. TỔNG THU
                DrawTwoColumn(g, "TỔNG THU:", _phieuThuData.TongThucThu.ToString("N0") + " đ", fontTotal, fontTotal, brushBlack, xLeft, xRight, y);
                y += 22f;

                g.DrawLine(penDash, xLeft, y, xRight, y);
                y += 16f;

                // 5. PHƯƠNG THỨC & MÃ QR
                if (_phieuThuData.PhuongThucThu != null && _phieuThuData.PhuongThucThu.Contains("Tiền mặt"))
                {
                    DrawTwoColumn(g, "Hình thức thu:", "Tiền mặt", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                    y += 18f;
                    DrawTwoColumn(g, "Tiền khách đưa:", _phieuThuData.TienKhachDua.ToString("N0") + " đ", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                    y += 18f;
                    DrawTwoColumn(g, "Tiền thừa trả lại:", _phieuThuData.TienThua.ToString("N0") + " đ", fontRegular, fontRegular, brushBlack, xLeft, xRight, y);
                    y += 20f;
                    g.DrawLine(penDash, xLeft, y, xRight, y);
                    y += 16f;
                }
                else
                {
                    DrawCenteredText(g, "MÃ QUÉT CHUYỂN KHOẢN (VIETQR)", fontBold, brushBlack, xCenter, y);
                    y += 18f;

                    // Vẽ ảnh VietQR của khách hàng đã tải lên
                    string qrPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "vietqr_user.png");
                    if (!System.IO.File.Exists(qrPath))
                    {
                        qrPath = System.IO.Path.Combine(Application.StartupPath, "Resources", "vietqr_user.png");
                    }
                    if (!System.IO.File.Exists(qrPath))
                    {
                        qrPath = @"C:\Users\ADMIN\Downloads\QuanLyBanLaptop-master (1)\QuanLyBanLaptop-master\QuanLyBanLaptop\Resources\vietqr_user.png";
                    }

                    if (System.IO.File.Exists(qrPath))
                    {
                        try
                        {
                            using (var img = Image.FromFile(qrPath))
                            {
                                float qrW = 190f;
                                float qrH = qrW * ((float)img.Height / img.Width);
                                float qrX = xCenter - (qrW / 2f);
                                g.DrawImage(img, qrX, y, qrW, qrH);
                                y += qrH + 8f;
                            }
                        }
                        catch
                        {
                            y += 20f;
                        }
                    }
                    else
                    {
                        y += 20f;
                    }

                    DrawCenteredText(g, "Quét mã QR bằng App Ngân hàng hoặc Ví điện tử để chuyển", fontItalic, brushBlack, xCenter, y);
                    y += 14f;
                    DrawCenteredText(g, "khoản tự động chính xác số tiền", fontItalic, brushBlack, xCenter, y);
                    y += 20f;

                    g.DrawLine(penDash, xLeft, y, xRight, y);
                    y += 16f;
                }

                // 6. FOOTER
                string thuNgan = "Thu ngân: " + (_phieuThuData.TenNV ?? "Trần Thị Bình") + " (" + (_phieuThuData.MaNV ?? "NV002") + ")";
                DrawCenteredText(g, thuNgan, fontBold, brushBlack, xCenter, y);
                y += 18f;

                DrawCenteredText(g, "Cảm ơn quý khách & Hẹn gặp lại!", fontItalic, brushBlack, xCenter, y);
                y += 20f;
            }

            e.HasMorePages = false;
        }
        #endregion
    }
}

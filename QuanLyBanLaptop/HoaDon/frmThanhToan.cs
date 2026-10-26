using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.HoaDon
{
    // Form Thanh Toán Hóa Đơn & Trả Góp - Thiết kế chuẩn theo giao diện người dùng yêu cầu.
    public partial class frmThanhToan : QuanLyBanLaptop.Classes.AppForm
    {
        // Cho phép kéo di chuyển form không viền
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        // Dữ liệu đầu vào từ frmBanHang
        private readonly string _maHD;
        private readonly string _maKH;
        private readonly string _tenKH;
        private readonly string _hangKhach;
        private readonly string _maNV;
        private readonly string _tenNV;
        private readonly string _diaChi;
        private readonly string _sdt;
        private readonly DateTime _ngayLap;
        private readonly decimal _tongTien;
        private readonly DataTable _dtGioHang;

        // Kết quả trả về
        public string MaHDDaLuu { get; private set; }
        public string PhuongThucChon { get; private set; }
        public int ChuKyChon { get; private set; }

        private const decimal TY_LE_TRA_TRUOC = 0.30m; // 30% tối thiểu

        // Cấu hình các gói trả góp: (tháng, lãi suất %, hiển thị)
        private static readonly (int Ky, double LaiSuat, string HienThi)[] _goiTraGop = new[]
        {
            (3,  2.4, "3 Tháng (Lãi suất 2.4%)"),
            (6,  4.0, "6 Tháng (Lãi suất 4.0%)"),
            (9,  6.0, "9 Tháng (Lãi suất 6.0%)"),
            (12, 8.0, "12 Tháng (Lãi suất 8.0%)")
        };

        // Khởi tạo giao diện và đăng ký sự kiện cho form.
        public frmThanhToan(
            string maHD, string maKH, string tenKH, string hangKhach,
            string maNV, string tenNV, string diaChi, string sdt,
            DateTime ngayLap, decimal tongTien, DataTable dtGioHang)
        {
            InitializeComponent();
            _maHD      = maHD;
            _maKH      = maKH;
            _tenKH     = tenKH;
            _hangKhach = hangKhach;
            _maNV      = maNV;
            _tenNV     = tenNV;
            _diaChi    = diaChi;
            _sdt       = sdt;
            _ngayLap   = ngayLap;
            _tongTien  = tongTien;
            _dtGioHang = dtGioHang;

            // Kéo form bằng thanh tiêu đề
            pnlTitleBar.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };
            lblIconAndTitle.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };

            // Vẽ viền ngoài cho Form
            this.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(30, 58, 138), 2))
                {
                    e.Graphics.DrawRectangle(pen, 1, 1, this.ClientSize.Width - 2, this.ClientSize.Height - 2);
                }
            };

            // Vẽ viền cho khung tóm tắt thông tin hóa đơn
            pnlInvoiceSummary.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(191, 219, 254), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlInvoiceSummary.Width - 1, pnlInvoiceSummary.Height - 1);
                }
            };

            // Vẽ viền cho card Tab 1
            pnlTabThangCard.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(203, 213, 225), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlTabThangCard.Width - 1, pnlTabThangCard.Height - 1);
                }
            };

            // Vẽ viền cho card Tab 2
            pnlTabGopCard.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(203, 213, 225), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlTabGopCard.Width - 1, pnlTabGopCard.Height - 1);
                }
            };

            // Vẽ viền cho DataGridView
            dgvLichTraGop.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(203, 213, 225), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, dgvLichTraGop.Width - 1, dgvLichTraGop.Height - 1);
                }
            };

            // Vẽ đường gạch dưới Tab Bar
            pnlTabBar.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                {
                    e.Graphics.DrawLine(pen, 0, pnlTabBar.Height - 1, pnlTabBar.Width, pnlTabBar.Height - 1);
                }
            };
        }

        // Khởi tạo giao diện và đăng ký sự kiện cho form.
        private void frmThanhToan_Load(object sender, EventArgs e)
        {
            pnlTitleBar.BringToFront();
            pnlFooter.BringToFront();

            // Hiển thị thông tin hóa đơn
            lblMaHDValue.Text     = _maHD;
            lblTenKhach.Text      = string.IsNullOrEmpty(_tenKH) ? "Khách vãng lai" : _tenKH;
            lblHangKhach.Text     = "(" + (string.IsNullOrEmpty(_hangKhach) ? "Đồng" : _hangKhach) + ")";
            lblTongTienValue.Text = Function.FormatTien(_tongTien);

            lblMaHDValue.Left = lblIconAndTitle.Right + 4;
            lblHangKhach.Left = lblTenKhach.Right + 6;

            // Tab 1: Khởi tạo giá trị
            cboPhuongThuc.Items.Clear();
            cboPhuongThuc.Items.Add("Chuyển khoản ngân hàng");
            cboPhuongThuc.Items.Add("Tiền mặt");
            cboPhuongThuc.SelectedIndex = 0;

            txtTongThanhToan.Text = Function.FormatTien(_tongTien);
            txtKhachDua.Text      = _tongTien.ToString("0");
            TinhTienThua();

            // Tab 2: Khởi tạo giá trị
            decimal downpayment = Math.Round(_tongTien * TY_LE_TRA_TRUOC, 0, MidpointRounding.AwayFromZero);
            decimal loan        = _tongTien - downpayment;

            txtTraTruoc.Text  = Function.FormatTien(downpayment);
            txtSoTienVay.Text = Function.FormatTien(loan);

            cboChuKy.Items.Clear();
            foreach (var g in _goiTraGop)
                cboChuKy.Items.Add(g.HienThi);
            cboChuKy.SelectedIndex = 0; // Mặc định 3 tháng như ảnh mẫu

            // Hiển thị Tab 1 ban đầu
            ChonTab(1);
        }

        // Đóng form
        private void btnCloseTitle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Xử lý sự kiện btnHuy_Click của giao diện.
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Chuyển đổi Tab Trả Thẳng / Trả Góp
        private void btnTabThang_Click(object sender, EventArgs e) => ChonTab(1);
        // Xử lý sự kiện btnTabGop_Click của giao diện.
        private void btnTabGop_Click(object sender, EventArgs e)   => ChonTab(2);

        // Thực hiện ChonTab theo trạng thái và dữ liệu hiện tại.
        private void ChonTab(int tabIndex)
        {
            if (tabIndex == 1)
            {
                // Tab 1 Active
                ButtonTheme.Apply(btnTabThang);
                
                ButtonTheme.Apply(btnTabGop);
                

                pnlTabIndicator.Location = new Point(btnTabThang.Left, pnlTabBar.Height - 4);
                pnlTabIndicator.Width    = btnTabThang.Width;

                pnlTabThangContent.Visible = true;
                pnlTabGopContent.Visible   = false;
                pnlTabThangContent.BringToFront();
            }
            else
            {
                // Tab 2 Active
                ButtonTheme.Apply(btnTabGop);
                
                ButtonTheme.Apply(btnTabThang);
                

                pnlTabIndicator.Location = new Point(btnTabGop.Left, pnlTabBar.Height - 4);
                pnlTabIndicator.Width    = btnTabGop.Width;

                pnlTabThangContent.Visible = false;
                pnlTabGopContent.Visible   = true;
                pnlTabGopContent.BringToFront();

                TinhLichTraGop();
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // TAB 1: TÍNH TIỀN THỪA TRẢ KHÁCH
        // ═══════════════════════════════════════════════════════════════════
        private void txtKhachDua_TextChanged(object sender, EventArgs e) => TinhTienThua();

        // Thực hiện TinhTienThua theo trạng thái và dữ liệu hiện tại.
        private void TinhTienThua()
        {
            string raw = txtKhachDua.Text.Trim().Replace(",", "").Replace(".", "");
            if (decimal.TryParse(raw, out decimal khachDua))
            {
                decimal thua = khachDua - _tongTien;
                if (thua < 0)
                {
                    txtTienThua.ForeColor = Color.FromArgb(220, 38, 38);
                    txtTienThua.Text = "Chưa đủ tiền (" + Function.FormatTien(thua) + ")";
                }
                else
                {
                    txtTienThua.ForeColor = Color.FromArgb(15, 23, 42);
                    txtTienThua.Text = Function.FormatTien(thua);
                }
            }
            else
            {
                txtTienThua.Text = "0 đ";
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // TAB 2: TÍNH LỊCH TRẢ GÓP & LÃI SUẤT
        // ═══════════════════════════════════════════════════════════════════
        private void cboChuKy_SelectedIndexChanged(object sender, EventArgs e) => TinhLichTraGop();

        // Thực hiện TinhLichTraGop theo trạng thái và dữ liệu hiện tại.
        private void TinhLichTraGop()
        {
            if (cboChuKy.SelectedIndex < 0 || cboChuKy.SelectedIndex >= _goiTraGop.Length) return;

            var goi = _goiTraGop[cboChuKy.SelectedIndex];
            int kyHan = goi.Ky;
            decimal laiSuat = (decimal)(goi.LaiSuat / 100.0);

            var plan = Services.InstallmentCalculator.Calculate(_tongTien, kyHan);
            decimal downpayment = plan.DownPayment, loan = plan.Principal;
            decimal totalInterest = plan.Interest, totalPayable = loan + totalInterest;
            decimal baseMonthly = plan.Monthly;
            txtTraTruoc.Text = Function.FormatTien(downpayment);
            txtSoTienVay.Text = Function.FormatTien(loan);

            lblTongTienLai.Text = "Tổng tiền lãi: " + Function.FormatTien(totalInterest)
                                + " (Lãi suất " + goi.LaiSuat.ToString("0.0") + "%)";

            dgvLichTraGop.Rows.Clear();
            decimal accumulated = 0;

            for (int k = 1; k <= kyHan; k++)
            {
                decimal soTienKy = baseMonthly;
                if (k == kyHan)
                {
                    soTienKy = totalPayable - accumulated;
                }
                accumulated += soTienKy;

                DateTime hanTra = _ngayLap.AddMonths(k);
                dgvLichTraGop.Rows.Add(
                    "Kỳ " + k,
                    hanTra.ToString("dd/MM/yyyy"),
                    Function.FormatTien(soTienKy));
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // XÁC NHẬN THANH TOÁN & MỞ PRINT PREVIEW DIALOG
        // ═══════════════════════════════════════════════════════════════════
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (pnlTabThangContent.Visible)
            {
                XacNhanTraThang();
            }
            else
            {
                XacNhanTraGop();
            }
        }

        // Thực hiện XacNhanTraThang theo trạng thái và dữ liệu hiện tại.
        private void XacNhanTraThang()
        {
            string phuongThucUI = cboPhuongThuc.SelectedItem?.ToString() ?? "Chuyển khoản ngân hàng";

            decimal khachDua = _tongTien;
            if (phuongThucUI.Contains("Tiền mặt"))
            {
                string raw = txtKhachDua.Text.Trim().Replace(",", "").Replace(".", "");
                if (!decimal.TryParse(raw, out khachDua) || khachDua < _tongTien)
                {
                    Function.ThongBaoCanhBao(
                        "Tiền khách đưa chưa đủ. Cần thanh toán: " + Function.FormatTien(_tongTien),
                        "Thông báo");
                    txtKhachDua.Focus();
                    return;
                }
            }
            decimal tienThua = Math.Max(0, khachDua - _tongTien);

            // Map chính xác theo CHECK constraint: CK_HD_PhuongThuc (N'Tiền mặt', N'Chuyển khoản', N'Trả góp')
            string phuongThucDB = phuongThucUI.Contains("khoản") ? "Chuyển khoản" : "Tiền mặt";

            // Ghi vào CSDL SQL Server trong 1 Transaction
            if (LuuHoaDonVaoDB(phuongThucDB, 0, "Đã thanh toán"))
            {
                PhuongThucChon = phuongThucUI;
                ChuKyChon = 0;
                MaHDDaLuu = _maHD;

                // Mở cửa sổ xem trước khi in hóa đơn (Print Preview Dialog)
                InHoaDonPreview(false, 0, 0, 0, 0, 0, 0, khachDua, tienThua);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        // Thực hiện XacNhanTraGop theo trạng thái và dữ liệu hiện tại.
        private void XacNhanTraGop()
        {
            if (cboChuKy.SelectedIndex < 0)
            {
                Function.ThongBaoCanhBao("Vui lòng chọn chu kỳ trả góp.", "Thông báo");
                return;
            }

            var goi = _goiTraGop[cboChuKy.SelectedIndex];
            int kyHan = goi.Ky;
            double laiSuatPct = goi.LaiSuat;

            var plan = Services.InstallmentCalculator.Calculate(_tongTien, kyHan);
            decimal downpayment = plan.DownPayment, loan = plan.Principal;
            decimal totalInterest = plan.Interest, baseMonthly = plan.Monthly;

            if (LuuHoaDonVaoDB("Trả góp", kyHan, "Đang trả góp"))
            {
                PhuongThucChon = "Trả góp";
                ChuKyChon = kyHan;
                MaHDDaLuu = _maHD;

                // Mở cửa sổ xem trước khi in hóa đơn (Print Preview Dialog)
                InHoaDonPreview(true, kyHan, laiSuatPct, downpayment, loan, totalInterest, baseMonthly, 0, 0);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        // Thực hiện InHoaDonPreview theo trạng thái và dữ liệu hiện tại.
        private void InHoaDonPreview(bool isTraGop, int chuKy, double laiSuat,
            decimal downpayment, decimal loan, decimal interest, decimal monthly,
            decimal khachDua, decimal tienThua)
        {
            var printData = new HoaDonPrintData
            {
                MaHD = _maHD,
                NgayLap = _ngayLap,
                MaKH = _maKH,
                TenKH = _tenKH,
                HangKhach = _hangKhach,
                SDT = _sdt,
                DiaChi = _diaChi,
                MaNV = _maNV,
                TenNV = _tenNV,
                IsTraGop = isTraGop,
                PhuongThuc = isTraGop ? "Trả góp" : (cboPhuongThuc.SelectedItem?.ToString() ?? "Tiền mặt"),
                ChuKy = chuKy,
                LaiSuat = laiSuat,
                TongTien = _tongTien,
                TienTraTruoc = downpayment,
                TienVay = loan,
                TienLai = interest,
                TienHangThang = monthly,
                KhachDua = khachDua,
                TienThua = tienThua,
                DtChiTiet = _dtGioHang
            };

            using (var frm = new frmPrintPreview(printData))
            {
                frm.ShowDialog(this);
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // LƯU CSDL SQL SERVER VỚI 1 TRANSACTION
        // ═══════════════════════════════════════════════════════════════════
        // Giao diện chỉ gửi dữ liệu; service chịu trách nhiệm toàn bộ transaction.
        private bool LuuHoaDonVaoDB(string phuongThuc, int chuKy, string tinhTrang)
        {
            try
            {
                new Services.SaleService().Save(new Services.SaleRequest
                {
                    InvoiceId = _maHD, CustomerId = _maKH, EmployeeId = _maNV,
                    Date = _ngayLap, PaymentMethod = phuongThuc, Months = chuKy, Items = _dtGioHang
                });
                return true;
            }
            catch (Exception ex)
            {
                Function.ThongBaoLoi(ex.Message, "Không thể lưu hóa đơn");
                return false;
            }
        }

        // Xuất hóa đơn ra file
        private void InHoaDon()
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Tệp HTML (*.html)|*.html|Tệp Excel (*.xls)|*.xls";
                sfd.FileName = "HoaDon_" + _maHD + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".html";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine("<!DOCTYPE html><html><head><meta charset='utf-8'><title>Hóa Đơn " + _maHD + "</title>");
                        sb.AppendLine("<style>body{font-family:Segoe UI,sans-serif;padding:24px;color:#333} table{width:100%;border-collapse:collapse;margin-top:16px;} th,td{border:1px solid #ddd;padding:8px;font-size:13px;} th{background:#1e3a8a;color:white;}</style>");
                        sb.AppendLine("</head><body>");
                        sb.AppendLine("<h2 style='color:#b91c1c;text-align:center;'>HÓA ĐƠN BÁN HÀNG</h2>");
                        sb.AppendLine("<p><b>Mã HĐ:</b> " + _maHD + " &nbsp;|&nbsp; <b>Ngày lập:</b> " + _ngayLap.ToString("dd/MM/yyyy HH:mm") + "</p>");
                        sb.AppendLine("<p><b>Khách hàng:</b> " + _tenKH + " (" + _hangKhach + ") &nbsp;|&nbsp; <b>SĐT:</b> " + _sdt + "</p>");
                        sb.AppendLine("<p><b>Địa chỉ:</b> " + _diaChi + "</p>");
                        sb.AppendLine("<p><b>Hình thức:</b> " + PhuongThucChon + (ChuKyChon > 0 ? " (" + ChuKyChon + " tháng)" : "") + "</p>");
                        sb.AppendLine("<table>");
                        sb.AppendLine("<tr><th>STT</th><th>Mã SP</th><th>Tên sản phẩm</th><th>Đơn giá</th><th>SL</th><th>Giảm giá</th><th>Thành tiền</th><th>BH</th></tr>");

                        int stt = 1;
                        foreach (DataRow r in _dtGioHang.Rows)
                        {
                            if (r.RowState == DataRowState.Deleted) continue;
                            string bh = (r.Table.Columns.Contains("ThoiGianBaoHanh") && r["ThoiGianBaoHanh"] != DBNull.Value)
                                        ? r["ThoiGianBaoHanh"].ToString() + " th" : "12 th";
                            sb.AppendLine("<tr><td align='center'>" + (stt++) + "</td>"
                                + "<td>" + r["MaLaptop"] + "</td>"
                                + "<td>" + r["TenLaptop"] + "</td>"
                                + "<td align='right'>" + Convert.ToDecimal(r["DonGia"]).ToString("N0") + "</td>"
                                + "<td align='center'>" + r["SoLuong"] + "</td>"
                                + "<td align='center'>" + r["GiamGia"] + "</td>"
                                + "<td align='right'>" + Convert.ToDecimal(r["ThanhTien"]).ToString("N0") + "</td>"
                                + "<td align='center'>" + bh + "</td></tr>");
                        }
                        sb.AppendLine("<tr><td colspan='6' align='right'><b>TỔNG CỘNG:</b></td><td align='right'><b style='color:#be123c'>" + _tongTien.ToString("N0") + " đ</b></td><td></td></tr>");
                        sb.AppendLine("</table>");
                        sb.AppendLine("<p><i>Bằng chữ: " + Function.ChuyenSoThanhChu(_tongTien) + "</i></p>");
                        sb.AppendLine("</body></html>");

                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        if (MessageBox.Show("Xuất hóa đơn thành công:\n" + sfd.FileName + "\n\nBạn có muốn mở xem ngay không?",
                            "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(sfd.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Function.ThongBaoLoi("Lỗi xuất hóa đơn: " + ex.Message, "Lỗi");
                    }
                }
            }
        }

        // Xử lý sự kiện lblHangKhach_Click của giao diện.
        private void lblHangKhach_Click(object sender, EventArgs e)
        {

        }
    }
}

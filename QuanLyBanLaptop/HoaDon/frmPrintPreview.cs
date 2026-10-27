using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmPrintPreview : QuanLyBanLaptop.Classes.AppForm
    {
        private readonly HoaDonPrintData _data;
        private readonly PhieuThuPrintData _phieuThu;
        private readonly PrintDocument _printDoc = new PrintDocument();

        // 1. Nhận dữ liệu hóa đơn bán hàng
        public frmPrintPreview(HoaDonPrintData data)
        {
            InitializeComponent();
            _data = data ?? new HoaDonPrintData();
            _printDoc.PrintPage += InHoaDon;
        }

        // 2. Nhận dữ liệu phiếu thu trả góp
        public frmPrintPreview(PhieuThuPrintData phieuThu)
        {
            InitializeComponent();
            _phieuThu = phieuThu ?? new PhieuThuPrintData();
            _printDoc.PrintPage += InPhieuThu;
        }
        private void frmPrintPreview_Load(object sender, EventArgs e)
        {
            // Chiều cao tự co giãn theo số lượng sản phẩm
            int h = _data != null ? 520 + (_data.DtChiTiet?.Rows.Count ?? 0) * 45 : 550;
            _printDoc.DefaultPageSettings.PaperSize = new PaperSize("K80", 330, h);

            printPreviewControl1.Document = _printDoc;
            printPreviewControl1.Zoom = 1.0;
        }

        // Nút In hóa đơn
        private void tsBtnPrint_Click(object sender, EventArgs e)
        {
            using (PrintDialog dlg = new PrintDialog { Document = _printDoc, UseEXDialog = true })
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _printDoc.Print();
                }
            }
        }

        // Nút Đóng
        private void tsBtnClose_Click(object sender, EventArgs e) => this.Close();

        // ==================== VẼ HÓA ĐƠN BÁN HÀNG ====================
        private void InHoaDon(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            var sfCenter = new StringFormat { Alignment = StringAlignment.Center };
            var sfRight = new StringFormat { Alignment = StringAlignment.Far };

            using (Font fTitle = new Font("Consolas", 12, FontStyle.Bold))
            using (Font fBold = new Font("Consolas", 9, FontStyle.Bold))
            using (Font fNormal = new Font("Consolas", 8.5f))
            {
                float y = 20;

                // 1. Tiêu đề cửa hàng
                string tieuDe = _data.IsTraGop ? "HÓA ĐƠN TRẢ GÓP" : "HÓA ĐƠN BÁN HÀNG";
                g.DrawString(tieuDe, fTitle, Brushes.Black, 165, y, sfCenter); y += 22;
                g.DrawString("CỬA HÀNG LAPTOP STORE", fBold, Brushes.Black, 165, y, sfCenter); y += 18;
                g.DrawString(_data.NgayLap.ToString("dd/MM/yyyy HH:mm"), fNormal, Brushes.Black, 165, y, sfCenter); y += 22;
                g.DrawLine(Pens.Gray, 15, y, 315, y); y += 10;

                // 2. Thông tin khách hàng
                DrawRow(g, "Khách hàng:", _data.TenKH ?? "Khách lẻ", fNormal, y, sfRight); y += 18;
                DrawRow(g, "Số điện thoại:", _data.SDT ?? "", fNormal, y, sfRight); y += 18;
                DrawRow(g, "Địa chỉ:", _data.DiaChi ?? "", fNormal, y, sfRight); y += 20;
                g.DrawLine(Pens.Gray, 15, y, 315, y); y += 10;

                // 3. Danh sách sản phẩm mua
                DrawRow(g, "TÊN SẢN PHẨM", "THÀNH TIỀN", fBold, y, sfRight); y += 20;
                if (_data.DtChiTiet != null)
                {
                    foreach (DataRow r in _data.DtChiTiet.Rows)
                    {
                        if (r.RowState == DataRowState.Deleted) continue;
                        g.DrawString($"{r["SoLuong"]}x {r["TenLaptop"]}", fNormal, Brushes.Black, 15, y); y += 16;
                        DrawRow(g, $"  Đơn giá: {Convert.ToDecimal(r["DonGia"]):N0}đ", $"{Convert.ToDecimal(r["ThanhTien"]):N0}đ", fNormal, y, sfRight); y += 20;
                    }
                }
                g.DrawLine(Pens.Gray, 15, y, 315, y); y += 10;

                // 4. Chi tiết thanh toán
                string hinhThuc = _data.IsTraGop ? $"Trả góp ({_data.ChuKy} tháng)" : (_data.PhuongThuc ?? "Tiền mặt");
                DrawRow(g, "Hình thức:", hinhThuc, fNormal, y, sfRight); y += 18;

                if (!_data.IsTraGop && (_data.PhuongThuc ?? "").Contains("Tiền mặt"))
                {
                    DrawRow(g, "Tiền khách đưa:", $"{_data.KhachDua:N0} đ", fNormal, y, sfRight); y += 18;
                    DrawRow(g, "Tiền thừa:", $"{_data.TienThua:N0} đ", fNormal, y, sfRight); y += 18;
                }

                decimal tongThanhToan = _data.IsTraGop ? _data.TienTraTruoc : _data.TongTien;
                DrawRow(g, "TỔNG THANH TOÁN:", $"{tongThanhToan:N0} đ", fTitle, y, sfRight); y += 25;
                g.DrawLine(Pens.Gray, 15, y, 315, y); y += 15;

                // 5. Chữ ký thu ngân & Mã hóa đơn
                g.DrawString($"Thu ngân: {_data.TenNV ?? "Admin"}", fNormal, Brushes.Black, 165, y, sfCenter); y += 18;
                g.DrawString("Cảm ơn quý khách và hẹn gặp lại!", fNormal, Brushes.Black, 165, y, sfCenter); y += 22;
                g.DrawString($"* {_data.MaHD} *", fBold, Brushes.Black, 165, y, sfCenter);
            }
        }

        // ==================== VẼ PHIẾU THU TRẢ GÓP ====================
        private void InPhieuThu(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            var sfCenter = new StringFormat { Alignment = StringAlignment.Center };
            var sfRight = new StringFormat { Alignment = StringAlignment.Far };

            using (Font fTitle = new Font("Consolas", 12, FontStyle.Bold))
            using (Font fBold = new Font("Consolas", 9, FontStyle.Bold))
            using (Font fNormal = new Font("Consolas", 8.5f))
            {
                float y = 20;

                // Tiêu đề
                g.DrawString("PHIẾU THU TIỀN TRẢ GÓP", fTitle, Brushes.Black, 165, y, sfCenter); y += 22;
                g.DrawString($"Mã đơn: {_phieuThu.MaHD}", fBold, Brushes.Black, 165, y, sfCenter); y += 18;
                g.DrawString(_phieuThu.NgayThu.ToString("dd/MM/yyyy HH:mm"), fNormal, Brushes.Black, 165, y, sfCenter); y += 22;
                g.DrawLine(Pens.Gray, 15, y, 315, y); y += 10;

                // Khách hàng & Kỳ trả góp
                DrawRow(g, "Khách hàng:", _phieuThu.TenKH ?? "", fNormal, y, sfRight); y += 18;
                DrawRow(g, "Kỳ thanh toán:", $"Kỳ {_phieuThu.KyThu} / {_phieuThu.TongSoKy}", fNormal, y, sfRight); y += 18;
                DrawRow(g, "Tiền gốc kỳ:", $"{_phieuThu.TienGocKy:N0} đ", fNormal, y, sfRight); y += 18;
                if (_phieuThu.TienPhat > 0)
                {
                    DrawRow(g, "Tiền phạt trễ:", $"{_phieuThu.TienPhat:N0} đ", fNormal, y, sfRight); y += 18;
                }
                g.DrawLine(Pens.Gray, 15, y, 315, y); y += 10;

                // Tổng thu
                DrawRow(g, "TỔNG THU:", $"{_phieuThu.TongThucThu:N0} đ", fTitle, y, sfRight); y += 25;
                g.DrawLine(Pens.Gray, 15, y, 315, y); y += 15;

                // Lời cảm ơn
                g.DrawString($"Thu ngân: {_phieuThu.TenNV ?? "Admin"}", fNormal, Brushes.Black, 165, y, sfCenter); y += 18;
                g.DrawString("Cảm ơn quý khách!", fNormal, Brushes.Black, 165, y, sfCenter);
            }
        }

        // Hàm phụ: vẽ 1 dòng có cột trái và cột phải
        private void DrawRow(Graphics g, string left, string right, Font f, float y, StringFormat sfRight)
        {
            g.DrawString(left, f, Brushes.Black, 15, y);
            g.DrawString(right, f, Brushes.Black, 315, y, sfRight);
        }
    }
}

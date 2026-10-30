using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using QuanLyBanLaptop.Classes;
namespace QuanLyBanLaptop.Services
{
    public sealed class SaleRequest
    {
        public string InvoiceId { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string PaymentMethod { get; set; }
        public int Months { get; set; }
        public DataTable Items { get; set; }
    }
    public sealed class SaleService
    {
        private readonly Func<IDatabaseSession> openSession;
        public SaleService() : this(() => new DbHelpers()) { }
        public SaleService(Func<IDatabaseSession> openSession)
        {
            this.openSession = openSession ?? throw new ArgumentNullException(nameof(openSession));
        }
        private static List<DataRow> Validate(SaleRequest sale, bool validatePayment = true)
        {
            if (sale == null || sale.Items == null) throw new ArgumentException("Thiếu giỏ hàng.");
            var rows = sale.Items.Copy().AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .OrderBy(r => Convert.ToString(r["MaLaptop"]), StringComparer.Ordinal).ToList();
            if (rows.Count == 0) throw new ArgumentException("Hóa đơn phải có ít nhất một sản phẩm.");
            if (string.IsNullOrWhiteSpace(sale.InvoiceId) || string.IsNullOrWhiteSpace(sale.CustomerId) || string.IsNullOrWhiteSpace(sale.EmployeeId))
                throw new ArgumentException("Thiếu mã hóa đơn, khách hàng hoặc nhân viên.");
            if (rows.Select(r => Convert.ToString(r["MaLaptop"]).Trim()).Distinct(StringComparer.OrdinalIgnoreCase).Count() != rows.Count)
                throw new ArgumentException("Sản phẩm bị trùng trong giỏ hàng.");
            if (validatePayment && !new[] { "Tiền mặt", "Chuyển khoản", "Trả góp" }.Contains(sale.PaymentMethod))
                throw new ArgumentException("Phương thức thanh toán không hợp lệ.");
            if (sale.PaymentMethod == "Trả góp" && !new[] { 3, 6, 9, 12 }.Contains(sale.Months))
                throw new ArgumentException("Kỳ hạn trả góp không hợp lệ.");
            if (sale.PaymentMethod != "Trả góp" && sale.Months != 0)
                throw new ArgumentException("Thanh toán ngay không có kỳ hạn trả góp.");
            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(Convert.ToString(row["MaLaptop"])) ||
                    Convert.ToInt32(row["SoLuong"]) <= 0 || Convert.ToDecimal(row["DonGia"]) <= 0 ||
                    Convert.ToDecimal(row["GiamGia"]) < 0 || Convert.ToDecimal(row["GiamGia"]) > 100)
                    throw new ArgumentException("Mã hàng, số lượng, giá bán hoặc giảm giá không hợp lệ.");
            }
            return rows;
        }
        public void Save(SaleRequest sale)
        {
            var rows = Validate(sale);
            foreach (string column in new[] { "OriginalCost", "OriginalWarranty" })
                if (rows[0].Table.Columns.Contains(column)) rows[0].Table.Columns.Remove(column);
            using (var db = openSession())
            {
                db.BatDauGiaoDich();
                if (Convert.ToInt32(db.LayMotGiaTri("SELECT COUNT(*) FROM NHAN_VIEN WHERE MaNV=@0 AND DaNghi=0", sale.EmployeeId)) != 1)
                    throw new InvalidOperationException("Nhân viên không còn hoạt động.");
                if (Convert.ToInt32(db.LayMotGiaTri("SELECT COUNT(*) FROM KHACH_HANG WHERE MaKH=@0 AND DaKhoa=0", sale.CustomerId)) != 1)
                    throw new InvalidOperationException("Khách hàng không tồn tại hoặc bị khóa.");
                db.CapNhat(@"INSERT HOA_DON(MaHD,MaKH,MaNV,NgayLap,PhuongThucThanhToan,ChuKyTraGop,TinhTrang)
                    VALUES(@0,@1,@2,@3,@4,@5,@6)", sale.InvoiceId, sale.CustomerId, sale.EmployeeId,
                    sale.Date, sale.PaymentMethod, sale.Months,
                    sale.PaymentMethod == "Trả góp" ? "Đang trả góp" : "Đã thanh toán");
                foreach (var row in rows) SaveLine(db, sale, row);
                if (sale.PaymentMethod == "Trả góp")
                {
                    db.CapNhat(@"INSERT HOP_DONG_TRA_GOP(MaHD,TyLeTraTruoc,LaiSuat)
                        SELECT @0,COALESCE(
                            (SELECT CONVERT(decimal(9,4),GiaTri) FROM CAU_HINH WHERE Khoa=@1),
                            (SELECT CONVERT(decimal(9,4),GiaTri) FROM CAU_HINH WHERE Khoa='TRA_TRUOC_TOI_THIEU'))/100,
                            (SELECT CONVERT(decimal(5,2),GiaTri) FROM CAU_HINH WHERE Khoa=@2)",
                        sale.InvoiceId, "TRA_TRUOC_" + sale.Months + "_THANG", "LAI_SUAT_" + sale.Months + "_THANG");
                    db.CapNhat("EXEC dbo.sp_TaoLichTraGop @MaHD=@0", sale.InvoiceId);
                }
                db.XacNhan();
            }
        }
        private string LockEditable(IDatabaseSession db, string id)
        {
            var invoice = db.DocBang("SELECT MaKH,PhuongThucThanhToan FROM HOA_DON WITH(UPDLOCK,HOLDLOCK) WHERE MaHD=@0", id);
            if (invoice.Rows.Count != 1) throw new InvalidOperationException("Không tìm thấy hóa đơn.");
            if (Convert.ToString(invoice.Rows[0]["PhuongThucThanhToan"]) == "Trả góp")
                throw new InvalidOperationException("Hóa đơn trả góp phải được xử lý qua nghiệp vụ điều chỉnh công nợ.");
            if (Convert.ToInt32(db.LayMotGiaTri(@"SELECT COUNT(*) FROM BAO_HANH_SUA_CHUA b
                JOIN LAPTOP_SERIAL s ON s.SerialNo=b.SerialNo WHERE s.MaHD=@0", id)) > 0)
                throw new InvalidOperationException("Hóa đơn có lịch sử bảo hành, không thể xóa hoặc ghi đè.");
            return Convert.ToString(invoice.Rows[0]["MaKH"]);
        }
        private void ReleaseLines(IDatabaseSession db, string id, string productId = null)
        {
            db.CapNhat(@"UPDATE LAPTOP_SERIAL SET TrangThai=N'Trong kho',MaHD=NULL,NgayHetBH=NULL
                WHERE MaHD=@0 AND (@1 IS NULL OR MaLaptop=@1)", id, productId);
            db.CapNhat("DELETE CHI_TIET_HD WHERE MaHD=@0 AND (@1 IS NULL OR MaLaptop=@1)", id, productId);
        }
        public void Update(SaleRequest sale)
        {
            var rows = Validate(sale, false);
            using (var db = openSession())
            {
                db.BatDauGiaoDich();
                LockEditable(db, sale.InvoiceId);
                var originals = db.DocBang("SELECT MaLaptop,GiaVon,ThoiGianBaoHanh FROM CHI_TIET_HD WHERE MaHD=@0", sale.InvoiceId);
                var items = rows[0].Table;
                if (!items.Columns.Contains("OriginalCost")) items.Columns.Add("OriginalCost", typeof(decimal));
                if (!items.Columns.Contains("OriginalWarranty")) items.Columns.Add("OriginalWarranty", typeof(int));
                foreach (var row in rows)
                {
                    var old = originals.AsEnumerable().FirstOrDefault(r => Convert.ToString(r["MaLaptop"]) == Convert.ToString(row["MaLaptop"]));
                    row["OriginalCost"] = old == null ? DBNull.Value : old["GiaVon"];
                    row["OriginalWarranty"] = old == null ? DBNull.Value : old["ThoiGianBaoHanh"];
                }
                ReleaseLines(db, sale.InvoiceId);
                db.CapNhat("UPDATE HOA_DON SET MaKH=@0,MaNV=@1,NgayLap=@2 WHERE MaHD=@3",
                    sale.CustomerId, sale.EmployeeId, sale.Date, sale.InvoiceId);
                foreach (var row in rows) SaveLine(db, sale, row);
                db.XacNhan();
            }
        }
        public void Delete(string id)
        {
            using (var db = openSession())
            {
                db.BatDauGiaoDich();
                LockEditable(db, id);
                ReleaseLines(db, id);
                db.CapNhat("DELETE HOA_DON WHERE MaHD=@0", id);
                db.XacNhan();
            }
        }
        public void RemoveLine(string id, string productId)
        {
            using (var db = openSession())
            {
                db.BatDauGiaoDich();
                LockEditable(db, id);
                if (Convert.ToInt32(db.LayMotGiaTri("SELECT COUNT(*) FROM CHI_TIET_HD WHERE MaHD=@0", id)) <= 1)
                    throw new InvalidOperationException("Hãy dùng Xóa hóa đơn khi trả sản phẩm cuối cùng.");
                ReleaseLines(db, id, productId);
                db.XacNhan();
            }
        }
        private void SaveLine(IDatabaseSession db, SaleRequest sale, DataRow row)
        {
            string id = Convert.ToString(row["MaLaptop"]);
            int quantity = Convert.ToInt32(row["SoLuong"]);
            decimal price = Convert.ToDecimal(row["DonGia"]);
            decimal discount = Convert.ToDecimal(row["GiamGia"]);
            if (quantity <= 0 || price < 0 || discount < 0 || discount > 100)
                throw new ArgumentException("Số lượng, giá bán hoặc giảm giá không hợp lệ.");
            var product = db.DocBang(@"SELECT ThoiGianBaoHanh FROM LAPTOP WITH(UPDLOCK,HOLDLOCK)
                WHERE MaLaptop=@0 AND NgungKinhDoanh=0 AND SoLuongTon>=@1", id, quantity);
            if (product.Rows.Count != 1) throw new InvalidOperationException("Sản phẩm " + id + " không đủ tồn kho hoặc đã khóa.");
            int warranty = row.Table.Columns.Contains("OriginalWarranty") && row["OriginalWarranty"] != DBNull.Value
                ? Convert.ToInt32(row["OriginalWarranty"]) : Convert.ToInt32(product.Rows[0]["ThoiGianBaoHanh"]);
            decimal cost = row.Table.Columns.Contains("OriginalCost") && row["OriginalCost"] != DBNull.Value
                ? Convert.ToDecimal(row["OriginalCost"])
                : Convert.ToDecimal(db.LayMotGiaTri(@"SELECT ISNULL(
                    (SELECT SUM(SoLuong*DonGiaNhap)/NULLIF(SUM(SoLuong),0) FROM CHI_TIET_PN WHERE MaLaptop=@0),
                    ISNULL((SELECT DonGiaNhap FROM LAPTOP_DATA WHERE MaLaptop=@0),0))", id) ?? 0);
            db.CapNhat(@"INSERT CHI_TIET_HD(MaHD,MaLaptop,SoLuong,DonGia,GiaVon,ThoiGianBaoHanh,GiamGia)
                VALUES(@0,@1,@2,@3,@6,@4,@5)",
                sale.InvoiceId, id, quantity, price, warranty, discount, cost);
            int assigned = Convert.ToInt32(db.LayMotGiaTri(
                "SELECT COUNT(*) FROM LAPTOP_SERIAL WHERE MaHD=@0 AND MaLaptop=@1",
                sale.InvoiceId, id));
            if (assigned != quantity)
                throw new InvalidOperationException("Serial không khớp số lượng bán của " + id + ". Hãy đối soát.");
        }
    }
}

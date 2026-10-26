using System;
using System.Data;
using System.Linq;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.Services
{
    // Dữ liệu bán hàng tách khỏi form để có thể gọi và kiểm thử độc lập.
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

    // Ghi hóa đơn, tồn kho và serial trong một transaction.
    public sealed class SaleService
    {
        // Kiểm tra dữ liệu đầu vào trước khi mở transaction.
        public void Save(SaleRequest sale)
        {
            if (sale == null || sale.Items == null) throw new ArgumentException("Thiếu giỏ hàng.");
            var rows = sale.Items.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .OrderBy(r => Convert.ToString(r["MaLaptop"]), StringComparer.Ordinal).ToList();
            if (rows.Count == 0) throw new ArgumentException("Hóa đơn phải có ít nhất một sản phẩm.");
            if (rows.Select(r => Convert.ToString(r["MaLaptop"])).Distinct().Count() != rows.Count)
                throw new ArgumentException("Sản phẩm bị trùng trong giỏ hàng.");
            if (sale.PaymentMethod == "Trả góp" && !new[] { 3, 6, 9, 12 }.Contains(sale.Months))
                throw new ArgumentException("Kỳ hạn trả góp không hợp lệ.");

            using (var db = new DbHelpers())
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
                    db.CapNhat("INSERT HOP_DONG_TRA_GOP(MaHD,TyLeTraTruoc,LaiSuat) VALUES(@0,@1,@2)",
                        sale.InvoiceId, InstallmentCalculator.GetDefaultDownPaymentRatio(), InstallmentCalculator.RateFor(sale.Months));
                    db.CapNhat("EXEC dbo.sp_TaoLichTraGop @MaHD=@0", sale.InvoiceId);
                }
                db.CapNhat("EXEC dbo.sp_CapNhatHangThanhVien @MaKH=@0", sale.CustomerId);
                db.XacNhan();
            }
        }

        // Khóa hóa đơn và chặn sửa lịch sử đã có trả góp hoặc bảo hành.
        private string LockEditable(DbHelpers db, string id)
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

        // Hoàn serial trước khi xóa chi tiết để giữ toàn vẹn khóa ngoại.
        private void ReleaseLines(DbHelpers db, string id, string productId = null)
        {
            db.CapNhat(@"UPDATE LAPTOP_SERIAL SET TrangThai=N'Trong kho',MaHD=NULL,NgayHetBH=NULL
                WHERE MaHD=@0 AND (@1 IS NULL OR MaLaptop=@1)", id, productId);
            db.CapNhat("DELETE CHI_TIET_HD WHERE MaHD=@0 AND (@1 IS NULL OR MaLaptop=@1)", id, productId);
        }

        // Cập nhật hóa đơn tiền mặt/chuyển khoản bằng cùng logic bán từng sản phẩm.
        public void Update(SaleRequest sale)
        {
            var rows = sale.Items.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .OrderBy(r => Convert.ToString(r["MaLaptop"]), StringComparer.Ordinal).ToList();
            if (rows.Count == 0) throw new ArgumentException("Hóa đơn phải có sản phẩm.");
            using (var db = new DbHelpers())
            {
                db.BatDauGiaoDich();
                LockEditable(db, sale.InvoiceId);
                var originals = db.DocBang("SELECT MaLaptop,GiaVon,ThoiGianBaoHanh FROM CHI_TIET_HD WHERE MaHD=@0", sale.InvoiceId);
                if (!sale.Items.Columns.Contains("OriginalCost")) sale.Items.Columns.Add("OriginalCost", typeof(decimal));
                if (!sale.Items.Columns.Contains("OriginalWarranty")) sale.Items.Columns.Add("OriginalWarranty", typeof(int));
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

        // Xóa hóa đơn không có công nợ/bảo hành và hoàn serial trong cùng giao dịch.
        public void Delete(string id)
        {
            using (var db = new DbHelpers())
            {
                db.BatDauGiaoDich();
                LockEditable(db, id);
                ReleaseLines(db, id);
                db.CapNhat("DELETE HOA_DON WHERE MaHD=@0", id);
                db.XacNhan();
            }
        }

        // Trả một dòng hàng, giữ ít nhất một dòng trên hóa đơn.
        public void RemoveLine(string id, string productId)
        {
            using (var db = new DbHelpers())
            {
                db.BatDauGiaoDich();
                LockEditable(db, id);
                if (Convert.ToInt32(db.LayMotGiaTri("SELECT COUNT(*) FROM CHI_TIET_HD WHERE MaHD=@0", id)) <= 1)
                    throw new InvalidOperationException("Hãy dùng Xóa hóa đơn khi trả sản phẩm cuối cùng.");
                ReleaseLines(db, id, productId);
                db.XacNhan();
            }
        }

        // Khóa hàng theo thứ tự mã; chỉ ghi thành công khi đủ cả tồn kho và serial.
        private void SaveLine(DbHelpers db, SaleRequest sale, DataRow row)
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
                ? Convert.ToDecimal(row["OriginalCost"]) : Convert.ToDecimal(db.LayMotGiaTri("SELECT dbo.fn_GiaVonTrungBinh(@0)", id));
            db.CapNhat(@"INSERT CHI_TIET_HD(MaHD,MaLaptop,SoLuong,DonGia,GiaVon,ThoiGianBaoHanh,GiamGia)
                VALUES(@0,@1,@2,@3,@6,@4,@5)",
                sale.InvoiceId, id, quantity, price, warranty, discount, cost);
            int changed = Convert.ToInt32(db.LayMotGiaTri(@";WITH selected AS (
                    SELECT TOP (@0) * FROM LAPTOP_SERIAL WITH(UPDLOCK,HOLDLOCK)
                    WHERE MaLaptop=@1 AND TrangThai=N'Trong kho' ORDER BY SerialNo)
                UPDATE selected SET TrangThai=N'Đã bán',MaHD=@2,NgayHetBH=DATEADD(MONTH,@3,@4); SELECT @@ROWCOUNT",
                quantity, id, sale.InvoiceId, warranty, sale.Date.Date));
            if (changed != quantity) throw new InvalidOperationException("Serial không khớp tồn kho của " + id + ". Hãy đối soát.");
            db.CapNhat("UPDATE LAPTOP SET SoLuongTon=SoLuongTon-@0 WHERE MaLaptop=@1", quantity, id);
        }
    }
}

using System;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.Models;
namespace QuanLyBanLaptop.Services
{
    public sealed class ProductService
    {
        public void Save(Product product, bool isNew)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.SupplierId))
                throw new ArgumentException("Vui lòng nhập tên sản phẩm và chọn nhà cung cấp.");
            if (product.OpeningQuantity < 0 || product.OpeningQuantity > 10000 ||
                product.PurchasePrice < 0 || product.SalePrice <= product.PurchasePrice || product.WarrantyMonths < 0)
                throw new ArgumentException("Số lượng phải từ 0 đến 10.000; giá bán phải lớn hơn giá nhập không âm.");
            using (var db = new DbHelpers())
            {
                db.BatDauGiaoDich();
                object[] values = { product.Id, product.Name, product.Category, product.Cpu,
                    product.Ram, product.Ssd, product.PurchasePrice, product.SalePrice,
                    product.WarrantyMonths, product.SupplierId, product.Notes };
                if (isNew)
                {
                    db.CapNhat(@"INSERT LAPTOP(MaLaptop,TenLaptop,Loai,CPU,RAM,SSD,DonGiaNhap,DonGia,
                        ThoiGianBaoHanh,MaNCC,GhiChu)
                        VALUES(@0,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10)", values);
                    if (product.OpeningQuantity > 0)
                        db.CapNhat(@"DECLARE @items dbo.TVP_NhapHang, @id varchar(10);
                            INSERT @items VALUES(@0,@1,@2);
                            EXEC dbo.sp_NhapHang @MaNCC=@3,@MaNV=@4,@Items=@items,@MaPN=@id OUTPUT",
                            product.Id, product.OpeningQuantity, product.PurchasePrice,
                            product.SupplierId, UserSession.MaNV);
                }
                else
                {
                    if (db.LayMotGiaTri("SELECT MaLaptop FROM LAPTOP WITH(UPDLOCK,HOLDLOCK) WHERE MaLaptop=@0", product.Id) == null)
                        throw new InvalidOperationException("Sản phẩm không còn tồn tại.");
                    db.CapNhat(@"UPDATE LAPTOP SET TenLaptop=@1,Loai=@2,CPU=@3,RAM=@4,SSD=@5,
                        DonGiaNhap=@6,DonGia=@7,ThoiGianBaoHanh=@8,MaNCC=@9,GhiChu=@10 WHERE MaLaptop=@0", values);
                }
                db.XacNhan();
            }
        }
    }
}

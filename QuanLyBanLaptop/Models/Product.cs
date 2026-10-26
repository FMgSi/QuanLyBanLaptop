namespace QuanLyBanLaptop.Models
{
    // Dữ liệu sản phẩm dùng chung cho thêm mới và cập nhật.
    public sealed class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string SupplierId { get; set; }
        public string Cpu { get; set; }
        public string Ram { get; set; }
        public string Ssd { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int OpeningQuantity { get; set; }
        public int WarrantyMonths { get; set; }
        public string Notes { get; set; }
    }
}

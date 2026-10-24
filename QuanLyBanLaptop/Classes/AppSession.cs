namespace QuanLyBanLaptop.Classes
{
    /// <summary>Thông tin người dùng đang đăng nhập, dùng được ở mọi form.</summary>
    public static class AppSession
    {
        public static string MaNV { get; private set; }
        public static string TenNV { get; private set; }
        public static string ChucVu { get; private set; }

        public static bool LaQuanTri => ChucVu == ChucVuQuanTri;

        public const string ChucVuQuanTri = "Quản trị viên";
        public const string ChucVuNhanVien = "Nhân viên";

        public static void Set(string maNV, string tenNV, string chucVu)
        {
            MaNV = maNV;
            TenNV = tenNV;
            ChucVu = chucVu;
        }

        public static void Clear() => Set(null, null, null);
    }
}

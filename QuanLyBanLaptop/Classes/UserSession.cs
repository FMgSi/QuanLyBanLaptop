namespace QuanLyBanLaptop.Classes
{
    public static class UserSession
    {
        public static string MaNV { get; set; } = "NV001";
        public static string TenNV { get; set; } = "Nguyễn Văn An";
        public static string ChucVu { get; set; } = "Quản trị viên";
        public static string TenDN { get; set; } = "admin";

        public static bool IsAdmin => ChucVu == "Quản trị viên";

        public static void Logout()
        {
            MaNV = null;
            TenNV = null;
            ChucVu = null;
            TenDN = null;
        }
    }
}

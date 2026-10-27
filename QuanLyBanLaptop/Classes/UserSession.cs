namespace QuanLyBanLaptop.Classes
{
    public static class UserSession
    {
        public static string MaNV { get; set; }
        public static string TenNV { get; set; }
        private static string role;
        public static event System.EventHandler Changed;
        public static string ChucVu
        {
            get => role;
            set { role = value; Changed?.Invoke(null, System.EventArgs.Empty); }
        }
        public static string TenDN { get; set; }

        public static bool IsAdmin => !string.IsNullOrEmpty(ChucVu) && 
            (string.Equals(ChucVu.Trim(), "Quản trị viên", System.StringComparison.OrdinalIgnoreCase) || 
             string.Equals(ChucVu.Trim(), "Admin", System.StringComparison.OrdinalIgnoreCase));
        public static void RequireAdmin()
        {
            if (!IsAdmin) throw new System.InvalidOperationException("Chỉ quản trị viên được truy cập chức năng này.");
        }
        public static void Logout()
        {
            MaNV = null;
            TenNV = null;
            ChucVu = null;
            TenDN = null;
        }
    }
}

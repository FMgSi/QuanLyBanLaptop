using System.Drawing;

namespace QuanLyBanLaptop.Classes
{
    public static class SidebarTheme
    {
        // Dark navy blue matching LibraryManagerWF
        public static readonly Color Sidebar = Color.FromArgb(31, 42, 68);
        public static readonly Color SidebarHover = Color.FromArgb(48, 64, 100);
        public static readonly Color SidebarChon = Color.FromArgb(45, 108, 223);
        public static readonly Color SidebarActive = Color.FromArgb(45, 108, 223);
        public static readonly Color SidebarBack = Color.FromArgb(31, 42, 68);

        // Text & icon colors
        public static readonly Color TextYellow = Color.Yellow;
        public static readonly Color TextWhite = Color.White;
        public static readonly Color TextLogout = Color.FromArgb(255, 180, 180);
        public static readonly Color TextDimmed = Color.FromArgb(200, 215, 235);

        // Fonts
        public static readonly Font MenuFont = new Font("Segoe UI", 9.75F, FontStyle.Regular);
        public static readonly Font MenuActiveFont = new Font("Segoe UI", 9.75F, FontStyle.Bold);
        public static readonly Font LogoutFont = new Font("Segoe UI", 9.75F, FontStyle.Regular);
    }
}

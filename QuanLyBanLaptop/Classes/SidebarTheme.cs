using System.Drawing;

namespace QuanLyBanLaptop.Classes
{
    public static class SidebarTheme
    {
        // Dark navy blue matching the screenshot and reference repo
        public static readonly Color SidebarBack = Color.FromArgb(11, 36, 71);
        public static readonly Color HeaderBack = Color.FromArgb(8, 28, 56);
        public static readonly Color SidebarHover = Color.FromArgb(24, 60, 105);
        public static readonly Color SidebarActive = Color.FromArgb(32, 101, 185);

        // Text & icon colors
        public static readonly Color TextWhite = Color.White;
        public static readonly Color TextDimmed = Color.FromArgb(200, 215, 235);

        // Fonts
        public static readonly Font MenuFont = new Font("Segoe UI", 10.25f, FontStyle.Regular);
        public static readonly Font MenuActiveFont = new Font("Segoe UI", 10.25f, FontStyle.Bold);
        public static readonly Font HeaderFont = new Font("Segoe UI", 10f, FontStyle.Bold);
    }
}

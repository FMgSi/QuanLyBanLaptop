using System.Windows.Forms;

namespace QuanLyBanLaptop.Helpers
{
    /// <summary>Hộp thoại thông báo dùng chung, thống nhất tiêu đề và biểu tượng.</summary>
    public static class MessageHelper
    {
        private const string AppTitle = "Quản Lý Bán Laptop";

        public static void Info(string message) =>
            MessageBox.Show(message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void Warning(string message) =>
            MessageBox.Show(message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static void Error(string message) =>
            MessageBox.Show(message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static bool Confirm(string message) =>
            MessageBox.Show(message, AppTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }
}

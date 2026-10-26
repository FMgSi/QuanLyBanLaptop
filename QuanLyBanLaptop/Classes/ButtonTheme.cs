using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyBanLaptop.Classes
{
    // Định dạng nút dùng chung theo thanh thao tác của frmSanPham.
    public static class ButtonTheme
    {
        private static readonly Font ButtonFont = new Font("Segoe UI", 9F);

        // Giữ kích thước bố cục, chuẩn hóa nền trắng, chữ và viền mảnh.
        public static void Apply(Button button)
        {
            if (button.Tag is NavigationMenu) return;
            Control p = button.Parent;
            while (p != null)
            {
                if (p is SidebarControl) return;
                p = p.Parent;
            }

            button.UseVisualStyleBackColor = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Color.FromArgb(206, 212, 218);
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(241, 245, 249);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(226, 232, 240);
            button.BackColor = Color.White;
            button.ForeColor = Color.FromArgb(33, 37, 41);
            button.Font = ButtonFont;
            button.Image = null;
            button.BackgroundImage = null;
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.Cursor = Cursors.Hand;
        }

        // Duyệt các nút lồng trong panel, tab và user control.
        public static void ApplyAll(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                if (child is SidebarControl) continue;
                if (child is Button button) Apply(button);
                ApplyAll(child);
            }
        }
    }

    // Form cơ sở áp dụng giao diện nút sau khi các form con tải xong.
    public class AppForm : Form
    {
        // Áp dụng sau sự kiện Load để ghi đè định dạng cũ.
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ButtonTheme.ApplyAll(this);
        }
    }
}

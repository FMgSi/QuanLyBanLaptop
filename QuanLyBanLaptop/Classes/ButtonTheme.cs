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

        // Cập nhật trạng thái Enabled và visual tương ứng cho nút
        public static void SetButtonState(Button btn, bool enabled, Color? activeForeColor = null)
        {
            if (btn == null) return;
            btn.Enabled = enabled;
            if (enabled)
            {
                btn.BackColor = Color.White;
                btn.ForeColor = activeForeColor ?? Color.FromArgb(30, 41, 59);
                btn.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
                btn.Cursor = Cursors.Hand;
            }
            else
            {
                btn.BackColor = Color.FromArgb(248, 250, 252);
                btn.ForeColor = Color.FromArgb(148, 163, 184);
                btn.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
                btn.Cursor = Cursors.Default;
            }
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
        protected virtual bool RequiresAdmin => false;
        public AppForm()
        {
            UserSession.Changed += OnSessionChanged;
            Disposed += (sender, args) => UserSession.Changed -= OnSessionChanged;
        }
        private void OnSessionChanged(object sender, EventArgs args)
        {
            if (RequiresAdmin && !UserSession.IsAdmin) Hide();
        }
        protected override void SetVisibleCore(bool value)
        {
            if (value && RequiresAdmin && !DesignMode) UserSession.RequireAdmin();
            base.SetVisibleCore(value);
        }
        // Áp dụng sau sự kiện Load để ghi đè định dạng cũ.
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ButtonTheme.ApplyAll(this);
        }
    }
}

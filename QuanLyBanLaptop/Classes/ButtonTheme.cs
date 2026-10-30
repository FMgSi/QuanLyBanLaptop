using System;
using System.Drawing;
using System.Windows.Forms;
namespace QuanLyBanLaptop.Classes
{
    public static class ButtonTheme
    {
        private static readonly Font ButtonFont = new Font("Segoe UI", 9F);
        public static void Apply(Button button)
        {
            if (button.Tag is NavigationMenu) return;
            for (Control p = button.Parent; p != null; p = p.Parent)
                if (p is SidebarControl) return;
            button.UseVisualStyleBackColor = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Color.FromArgb(206, 212, 218);
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(241, 245, 249);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(226, 232, 240);
            button.BackColor = Color.White;
            button.ForeColor = Color.FromArgb(33, 37, 41);
            button.Font = ButtonFont;
            button.Image = button.BackgroundImage = null;
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.Cursor = Cursors.Hand;
        }
        public static void SetButtonState(Button btn, bool enabled, Color? activeForeColor = null)
        {
            if (btn == null) return;
            btn.Enabled = enabled;
            btn.BackColor = enabled ? Color.White : Color.FromArgb(248, 250, 252);
            btn.ForeColor = enabled ? (activeForeColor ?? Color.FromArgb(30, 41, 59)) : Color.FromArgb(148, 163, 184);
            btn.FlatAppearance.BorderColor = enabled ? Color.FromArgb(203, 213, 225) : Color.FromArgb(226, 232, 240);
            btn.Cursor = enabled ? Cursors.Hand : Cursors.Default;
        }
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
    public class AppForm : Form
    {
        protected virtual bool RequiresAdmin => false;
        public AppForm()
        {
            UserSession.Changed += OnSessionChanged;
            Disposed += (s, e) => UserSession.Changed -= OnSessionChanged;
        }
        private void OnSessionChanged(object s, EventArgs e) { if (RequiresAdmin && !UserSession.IsAdmin) Hide(); }
        protected override void SetVisibleCore(bool value)
        {
            if (value && RequiresAdmin && !DesignMode) UserSession.RequireAdmin();
            base.SetVisibleCore(value);
        }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); ButtonTheme.ApplyAll(this); }
    }
}

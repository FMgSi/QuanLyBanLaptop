using System.Reflection;
using System.Windows.Forms;

namespace QuanLyBanLaptop.Helpers
{
    /// <summary>Các hàm tiện ích dùng chung cho Control / ComboBox / DataGridView.</summary>
    public static class ControlHelper
    {
        /// <summary>Bật DoubleBuffered (thuộc tính protected) để giảm giật khi vẽ lại.</summary>
        public static void SetDoubleBuffered(params Control[] controls)
        {
            foreach (Control control in controls)
            {
                typeof(Control).InvokeMember(
                    "DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }
        }

        /// <summary>Nạp danh sách vào ComboBox và chọn sẵn một dòng (nếu index hợp lệ).</summary>
        public static void FillCombo(ComboBox combo, int selectedIndex, params object[] items)
        {
            combo.Items.Clear();
            combo.Items.AddRange(items);
            if (selectedIndex >= 0 && selectedIndex < combo.Items.Count)
                combo.SelectedIndex = selectedIndex;
        }

        /// <summary>Đặt tất cả Button con trực tiếp của container vào một hành động chung.</summary>
        public static void ForEachButton(Control container, System.Action<Button> action)
        {
            foreach (Control c in container.Controls)
            {
                if (c is Button btn) action(btn);
            }
        }
    }
}

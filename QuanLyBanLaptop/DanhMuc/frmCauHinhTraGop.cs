using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmCauHinhTraGop : Form
    {
        private const string ConnectionString = @"Data Source=.;Initial Catalog=QuanLyBanLapTop;Integrated Security=True";
        private readonly Dictionary<string, ThamSo> danhSachThamSo = TaoDanhSachThamSo();
        private readonly DataTable bangCauHinh = new DataTable();
        private ComboBox cboThamSo;
        private TextBox txtKhoa;
        private TextBox txtGiaTri;
        private TextBox txtMoTa;
        private DataGridView dgvCauHinh;
        private Button btnSua;
        private Button btnLuu;
        private Button btnHuy;
        private bool dangSua;

        public frmCauHinhTraGop()
        {
            InitializeComponent();
            KhoiTaoGiaoDien();
            Shown += (s, e) => TaiDuLieu();
            FormClosed += (s, e) =>
            {
                if (Owner != null) Owner.Show();
            };
        }

        private void KhoiTaoGiaoDien()
        {
            Text = "Cấu hình Trả Góp & Hạng Thành Viên";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(920, 620);
            Size = new Size(1100, 740);
            WindowState = FormWindowState.Maximized;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 9.5F);

            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(24), ColumnCount = 1, RowCount = 4 };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 210));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 126));
            Controls.Add(root);

            var header = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(34, 64, 139) };
            header.Controls.Add(new Label { Text = "▦  Cấu hình Trả Góp & Hạng Thành Viên (Bảng CAU_HINH)", Dock = DockStyle.Left, Width = 690, ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(14, 0, 0, 0) });
            header.Controls.Add(new Label { Text = "Quản trị viên", Dock = DockStyle.Right, Width = 160, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter });
            root.Controls.Add(header, 0, 0);

            var info = new GroupBox { Text = "Thông tin tham số", Dock = DockStyle.Fill, Padding = new Padding(14) };
            var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 3, Padding = new Padding(0, 4, 0, 0) };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125)); grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90)); grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 48)); grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            cboThamSo = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cboThamSo.Items.Add("-- Chọn nhanh tham số cấu hình hệ thống --");
            foreach (var item in danhSachThamSo.Values) cboThamSo.Items.Add(item);
            cboThamSo.SelectedIndex = 0;
            cboThamSo.SelectedIndexChanged += cboThamSo_SelectedIndexChanged;
            grid.Controls.Add(TaoNhan("Chọn tham số:"), 0, 0); grid.Controls.Add(cboThamSo, 1, 0); grid.SetColumnSpan(cboThamSo, 3);
            txtKhoa = TaoTextBox(); txtKhoa.ReadOnly = true;
            txtGiaTri = TaoTextBox(); txtGiaTri.TextAlign = HorizontalAlignment.Right;
            grid.Controls.Add(TaoNhan("Mã tham số:"), 0, 1); grid.Controls.Add(txtKhoa, 1, 1);
            grid.Controls.Add(TaoNhan("Giá trị:"), 2, 1); grid.Controls.Add(txtGiaTri, 3, 1);
            txtMoTa = TaoTextBox(); txtMoTa.ReadOnly = true;
            grid.Controls.Add(TaoNhan("Mô tả / Quy định:"), 0, 2); grid.Controls.Add(txtMoTa, 1, 2); grid.SetColumnSpan(txtMoTa, 3);
            info.Controls.Add(grid); root.Controls.Add(info, 0, 1);

            var details = new GroupBox { Text = "Thông tin quy định chi tiết", Dock = DockStyle.Fill, Padding = new Padding(12) };
            dgvCauHinh = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, MultiSelect = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, RowHeadersVisible = false, BackgroundColor = Color.White, BorderStyle = BorderStyle.FixedSingle, EnableHeadersVisualStyles = false };
            dgvCauHinh.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 64, 139); dgvCauHinh.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCauHinh.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold); dgvCauHinh.ColumnHeadersHeight = 38; dgvCauHinh.RowTemplate.Height = 36;
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "MoTa", HeaderText = "Quy định / Mô tả", FillWeight = 48 });
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "Khoa", HeaderText = "Mã tham số (Khoa)", FillWeight = 24 });
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiaTri", HeaderText = "Giá trị", FillWeight = 13, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvCauHinh.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonVi", HeaderText = "Đơn vị", FillWeight = 15 });
            dgvCauHinh.SelectionChanged += dgvCauHinh_SelectionChanged;
            details.Controls.Add(dgvCauHinh); root.Controls.Add(details, 0, 2);

            var actions = new GroupBox { Text = "Thao tác", Dock = DockStyle.Fill, Padding = new Padding(12) };
            var footer = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = Padding.Empty };
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            footer.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            footer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            footer.Controls.Add(new Label { Text = "* Dữ liệu được đọc và cập nhật trực tiếp trong SQL Server ('CAU_HINH').", Dock = DockStyle.Fill, ForeColor = Color.FromArgb(85, 105, 140), Font = new Font("Segoe UI", 9, FontStyle.Italic), TextAlign = ContentAlignment.MiddleLeft }, 0, 0);

            var buttons = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 1, Padding = new Padding(0, 4, 0, 0) };
            for (int i = 0; i < 4; i++) buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            buttons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            var btnLamMoi = TaoNut("↻  Làm mới", Color.FromArgb(232, 237, 244)); btnLamMoi.Click += (s, e) => TaiDuLieu();
            btnSua = TaoNut("▣  Chỉnh sửa", Color.FromArgb(220, 232, 250)); btnSua.Click += btnSua_Click;
            btnLuu = TaoNut("▣  Lưu cấu hình", Color.FromArgb(218, 239, 226)); btnLuu.Click += btnLuu_Click;
            btnHuy = TaoNut("⊘  Hủy bỏ", Color.FromArgb(249, 225, 228)); btnHuy.Click += btnHuy_Click;
            Button[] actionButtons = { btnLamMoi, btnSua, btnLuu, btnHuy };
            for (int i = 0; i < actionButtons.Length; i++)
            {
                actionButtons[i].Dock = DockStyle.Fill;
                actionButtons[i].Height = 48;
                actionButtons[i].Margin = new Padding(8, 2, 8, 4);
                buttons.Controls.Add(actionButtons[i], i, 0);
            }
            footer.Controls.Add(buttons, 0, 1);
            actions.Controls.Add(footer);
            root.Controls.Add(actions, 0, 3);
            CapNhatTrangThaiSua(false);
        }

        private static Label TaoNhan(string text) { return new Label { Text = text, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.FromArgb(45, 58, 78) }; }
        private static TextBox TaoTextBox() { return new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10), Margin = new Padding(3, 6, 12, 6) }; }
        private static Button TaoNut(string text, Color color) { return new Button { Text = text, Width = 160, Height = 48, BackColor = color, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold), Margin = new Padding(8, 2, 8, 4), Cursor = Cursors.Hand, UseVisualStyleBackColor = false }; }

        private void TaiDuLieu()
        {
            try
            {
            using (var connection = new SqlConnection(ConnectionString))
                using (var command = new SqlCommand("SELECT Khoa, GiaTri, MoTa FROM CAU_HINH ORDER BY Khoa", connection))
                using (var adapter = new SqlDataAdapter(command))
                {
                    bangCauHinh.Clear(); adapter.Fill(bangCauHinh);
                }
                dgvCauHinh.Rows.Clear();
                foreach (DataRow row in bangCauHinh.Rows)
                {
                    string khoa = Convert.ToString(row["Khoa"]);
                    ThamSo item;
                    string moTa = Convert.ToString(row["MoTa"]);
                    if (!danhSachThamSo.TryGetValue(khoa, out item)) item = new ThamSo(khoa, string.IsNullOrWhiteSpace(moTa) ? khoa : moTa, "");
                    if (!string.IsNullOrWhiteSpace(moTa)) item = new ThamSo(khoa, moTa, item.DonVi);
                    dgvCauHinh.Rows.Add(item.MoTa, khoa, Convert.ToString(row["GiaTri"]), item.DonVi);
                }
                if (dgvCauHinh.Rows.Count > 0) dgvCauHinh.Rows[0].Selected = true;
                HienThiDongDangChon();
                if (dgvCauHinh.Rows.Count == 0) MessageBox.Show("Bảng CAU_HINH chưa có tham số nào.", "Cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể đọc cấu hình từ bảng CAU_HINH. Kiểm tra kết nối SQL Server và các cột Khoa, GiaTri, MoTa.\n\n" + ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCauHinh_SelectionChanged(object sender, EventArgs e) { if (!dangSua) HienThiDongDangChon(); }

        private void HienThiDongDangChon()
        {
            if (dgvCauHinh == null || dgvCauHinh.CurrentRow == null) return;
            string khoa = Convert.ToString(dgvCauHinh.CurrentRow.Cells["Khoa"].Value);
            if (string.IsNullOrEmpty(khoa)) return;
            txtKhoa.Text = khoa;
            txtGiaTri.Text = Convert.ToString(dgvCauHinh.CurrentRow.Cells["GiaTri"].Value);
            txtMoTa.Text = Convert.ToString(dgvCauHinh.CurrentRow.Cells["MoTa"].Value);
            int index = cboThamSo.Items.Cast<object>().Select((item, i) => new { item, i }).Where(x => x.item is ThamSo && ((ThamSo)x.item).Khoa == khoa).Select(x => x.i).DefaultIfEmpty(0).First();
            cboThamSo.SelectedIndex = index;
        }

        private void cboThamSo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dangSua || !(cboThamSo.SelectedItem is ThamSo)) return;
            string khoa = ((ThamSo)cboThamSo.SelectedItem).Khoa;
            foreach (DataGridViewRow row in dgvCauHinh.Rows)
                if (Convert.ToString(row.Cells["Khoa"].Value) == khoa) { row.Selected = true; dgvCauHinh.CurrentCell = row.Cells["Khoa"]; return; }
            txtKhoa.Text = khoa; txtGiaTri.Clear(); txtMoTa.Text = ((ThamSo)cboThamSo.SelectedItem).MoTa;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKhoa.Text)) { MessageBox.Show("Hãy chọn một tham số đang có trong bảng CAU_HINH.", "Chưa chọn tham số", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (string.IsNullOrWhiteSpace(txtGiaTri.Text)) { MessageBox.Show("Tham số này chưa có trong bảng CAU_HINH nên không thể chỉnh sửa.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            CapNhatTrangThaiSua(true);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            decimal value;
            if (!decimal.TryParse(txtGiaTri.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out value) && !decimal.TryParse(txtGiaTri.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out value))
            { MessageBox.Show("Giá trị phải là số hợp lệ.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtGiaTri.Focus(); return; }
            ThamSo item; danhSachThamSo.TryGetValue(txtKhoa.Text, out item);
            if (item != null && (item.Khoa == "TRA_TRUOC_TOI_THIEU" || item.Khoa.StartsWith("LAI_SUAT_")) && (value < 0 || value > 100))
            { MessageBox.Show("Tỷ lệ phần trăm phải nằm trong khoảng từ 0 đến 100.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                using (var command = new SqlCommand("UPDATE CAU_HINH SET GiaTri = @GiaTri WHERE Khoa = @Khoa", connection))
                {
                    command.Parameters.Add("@GiaTri", SqlDbType.Decimal).Value = value;
                    command.Parameters["@GiaTri"].Precision = 18; command.Parameters["@GiaTri"].Scale = 4;
                    command.Parameters.Add("@Khoa", SqlDbType.NVarChar, 100).Value = txtKhoa.Text;
                    connection.Open();
                    if (command.ExecuteNonQuery() == 0) { MessageBox.Show("Không tìm thấy tham số trong CAU_HINH; chưa có thay đổi nào được lưu.", "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                }
                CapNhatTrangThaiSua(false); TaiDuLieu();
                MessageBox.Show("Đã lưu cấu hình thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Không thể lưu cấu hình: " + ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnHuy_Click(object sender, EventArgs e) { CapNhatTrangThaiSua(false); HienThiDongDangChon(); }

        private void CapNhatTrangThaiSua(bool sua)
        {
            dangSua = sua;
            txtGiaTri.ReadOnly = !sua;
            txtGiaTri.BackColor = sua ? Color.White : SystemColors.Control;
            btnSua.Enabled = !sua; btnLuu.Enabled = sua; btnHuy.Enabled = sua;
            cboThamSo.Enabled = !sua; dgvCauHinh.Enabled = !sua;
        }

        private static Dictionary<string, ThamSo> TaoDanhSachThamSo()
        {
            var items = new[] {
                new ThamSo("TRA_TRUOC_TOI_THIEU", "Tỷ lệ trả trước tối thiểu", "% tổng giá trị"),
                new ThamSo("LAI_SUAT_3_THANG", "Lãi suất kỳ hạn 3 tháng", "%"),
                new ThamSo("LAI_SUAT_6_THANG", "Lãi suất kỳ hạn 6 tháng", "%"),
                new ThamSo("LAI_SUAT_9_THANG", "Lãi suất kỳ hạn 9 tháng", "%"),
                new ThamSo("LAI_SUAT_12_THANG", "Lãi suất kỳ hạn 12 tháng", "%"),
                new ThamSo("NGUONG_BAC", "Tổng chi tiêu tối thiểu lên hạng Bạc", "đồng"),
                new ThamSo("NGUONG_VANG", "Tổng chi tiêu tối thiểu lên hạng Vàng", "đồng"),
                new ThamSo("NGUONG_VIP", "Tổng chi tiêu tối thiểu lên hạng VIP", "đồng")
            };
            return items.ToDictionary(x => x.Khoa, x => x);
        }

        private sealed class ThamSo
        {
            public string Khoa { get; private set; }
            public string MoTa { get; private set; }
            public string DonVi { get; private set; }
            public ThamSo(string khoa, string moTa, string donVi) { Khoa = khoa; MoTa = moTa; DonVi = donVi; }
            public override string ToString() { return MoTa + "  (" + Khoa + ")"; }
        }
    }
}

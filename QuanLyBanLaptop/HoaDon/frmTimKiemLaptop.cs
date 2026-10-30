using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmTimKiemLaptop : AppForm
    {
        private const string PlaceholderText = "Nhập tên máy, hãng, chip xử lý...";
        private DataTable _dtSource;
        private DataView _dv;
        private readonly bool repairMode;
        public string SelectedMaLaptop { get; private set; }
        public string SelectedTenLaptop { get; private set; }
        public decimal SelectedDonGia { get; private set; }
        public int SelectedBaoHanh { get; private set; }
        public int SelectedSoLuongTon { get; private set; }
        public frmTimKiemLaptop() => InitializeComponent();
        public frmTimKiemLaptop(bool forRepair) : this()
        {
            repairMode = forRepair;
            if (!repairMode) return;
            Text = "Tìm máy cần sửa chữa";
            lblTitle.Text = "TÌM KIẾM LAPTOP";
            lblCapNhat.Visible = false;
            var outside = new Button
            {
                Name = "btnMayNgoaiDanhMuc", Text = "Máy ngoài danh mục...",
                Location = new Point(230, 8), Size = new Size(190, 32),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            outside.Click += (sender, args) => ChooseOutsideDevice();
            pnlFooter.Controls.Add(outside);
        }
        private void ChooseOutsideDevice()
        {
            using (var dialog = new Form { Text = "Máy ngoài danh mục", Width = 550, Height = 180, StartPosition = FormStartPosition.CenterParent })
            using (var label = new Label { Text = "Nhập hãng / model máy (có thể ghi thêm serial):", Dock = DockStyle.Top, Height = 30 })
            using (var name = new TextBox { Name = "txtMayNgoai", MaxLength = 200, Dock = DockStyle.Top })
            using (var choose = new Button { Text = "Chọn máy", Dock = DockStyle.Bottom, Height = 36 })
            {
                choose.Click += (sender, args) =>
                {
                    if (string.IsNullOrWhiteSpace(name.Text)) { Function.ThongBaoCanhBao("Vui lòng nhập tên máy cần sửa."); return; }
                    dialog.DialogResult = DialogResult.OK;
                };
                dialog.Controls.AddRange(new Control[] { name, label, choose });
                dialog.AcceptButton = choose;
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                SelectedMaLaptop = null;
                SelectedTenLaptop = name.Text.Trim();
                SelectedDonGia = 0;
                SelectedBaoHanh = 0;
                SelectedSoLuongTon = 0;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        private void frmTimKiemLaptop_Load(object sender, EventArgs e) { LoadData(); txtTimKiem.Focus(); }
        private void LoadData()
        {
            try
            {
                string query = @"SELECT l.MaLaptop, l.TenLaptop,
                    CASE WHEN l.TenLaptop LIKE 'Acer%' OR n.TenNCC LIKE '%Acer%' THEN 'Acer'
                         WHEN l.TenLaptop LIKE 'Asus%' OR n.TenNCC LIKE '%Asus%' THEN 'Asus'
                         WHEN l.TenLaptop LIKE 'Dell%' OR n.TenNCC LIKE '%Dell%' THEN 'Dell'
                         WHEN l.TenLaptop LIKE 'HP%' OR n.TenNCC LIKE '%HP%' THEN 'HP'
                         WHEN l.TenLaptop LIKE 'Lenovo%' OR n.TenNCC LIKE '%Lenovo%' THEN 'Lenovo'
                         WHEN l.TenLaptop LIKE 'MacBook%' OR l.TenLaptop LIKE 'Apple%' OR n.TenNCC LIKE '%Apple%' THEN 'Apple'
                         ELSE ISNULL(l.Loai, N'Khác') END AS Hang,
                    ISNULL(l.CPU, '') + CASE WHEN ISNULL(l.RAM, '') <> '' THEN ' / ' + l.RAM ELSE '' END +
                    CASE WHEN ISNULL(l.SSD, '') <> '' THEN ' / ' + l.SSD + (CASE WHEN l.SSD NOT LIKE '%SSD%' THEN ' SSD' ELSE '' END) ELSE '' END AS CauHinhChinh,
                    l.DonGia, l.SoLuongTon, l.ThoiGianBaoHanh, l.CPU
                    FROM LAPTOP l LEFT JOIN NHA_CUNG_CAP n ON l.MaNCC = n.MaNCC WHERE l.NgungKinhDoanh = 0 ORDER BY l.TenLaptop";
                if (repairMode) query = query.Replace("WHERE l.NgungKinhDoanh = 0", "");
                _dtSource = DataProcesser.ReadData(query);
                if (!_dtSource.Columns.Contains("STT")) _dtSource.Columns.Add("STT", typeof(int));
                if (!_dtSource.Columns.Contains("GiaBanFormatted")) _dtSource.Columns.Add("GiaBanFormatted", typeof(string));
                _dv = new DataView(_dtSource);
                BindData();
            }
            catch (Exception ex) { Function.ThongBaoLoi("Lỗi tải danh sách laptop: " + ex.Message, "Lỗi"); }
        }
        private void BindData()
        {
            if (_dv == null) return;
            int stt = 1;
            foreach (DataRowView rowView in _dv)
            {
                rowView["STT"] = stt++;
                rowView["GiaBanFormatted"] = string.Format("{0:N0} đ", rowView["DonGia"] != DBNull.Value ? Convert.ToDecimal(rowView["DonGia"]) : 0);
            }
            dgvLaptop.AutoGenerateColumns = false;
            colSTT.DataPropertyName = "STT";
            colTenLaptop.DataPropertyName = "TenLaptop";
            colHang.DataPropertyName = "Hang";
            colCauHinh.DataPropertyName = "CauHinhChinh";
            colGiaBan.DataPropertyName = "GiaBanFormatted";
            colMaLaptop.DataPropertyName = "MaLaptop";
            colSoLuongTon.DataPropertyName = "SoLuongTon";
            colThoiGianBaoHanh.DataPropertyName = "ThoiGianBaoHanh";
            dgvLaptop.DataSource = _dv;
            lblHienThi.Text = $"Hiển thị: {_dv.Count} sản phẩm";
        }
        private void FilterData()
        {
            if (_dtSource == null || _dv == null) return;
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword) || keyword == PlaceholderText) _dv.RowFilter = "";
            else
            {
                string safe = keyword.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]");
                _dv.RowFilter = $"TenLaptop LIKE '%{safe}%' OR Hang LIKE '%{safe}%' OR CauHinhChinh LIKE '%{safe}%' OR MaLaptop LIKE '%{safe}%' OR CPU LIKE '%{safe}%'";
            }
            BindData();
        }
        private void ChonSanPhamHienTai()
        {
            if (dgvLaptop.CurrentRow != null && dgvLaptop.CurrentRow.Index >= 0)
            {
                var row = dgvLaptop.CurrentRow;
                var drv = row.DataBoundItem as DataRowView;
                SelectedMaLaptop = row.Cells["colMaLaptop"].Value?.ToString() ?? "";
                SelectedTenLaptop = row.Cells["colTenLaptop"].Value?.ToString() ?? "";
                SelectedDonGia = drv != null && drv["DonGia"] != DBNull.Value ? Convert.ToDecimal(drv["DonGia"]) : 0m;
                int.TryParse(row.Cells["colThoiGianBaoHanh"].Value?.ToString(), out int bh);
                SelectedBaoHanh = bh > 0 ? bh : 12;
                int.TryParse(row.Cells["colSoLuongTon"].Value?.ToString(), out int slt);
                SelectedSoLuongTon = slt;
                DialogResult = DialogResult.OK;
                Close();
            }
            else Function.ThongBaoThanhCong("Vui lòng chọn một sản phẩm trong danh sách.", "Thông báo");
        }
        private void btnTimKiem_Click(object sender, EventArgs e) => FilterData();
        private void txtTimKiem_TextChanged(object sender, EventArgs e) { if (txtTimKiem.Text != PlaceholderText) FilterData(); }
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; FilterData(); if (dgvLaptop.Rows.Count > 0) dgvLaptop.Focus(); }
            else if (e.KeyCode == Keys.Down && dgvLaptop.Rows.Count > 0) { dgvLaptop.Focus(); e.SuppressKeyPress = true; }
        }
        private void txtTimKiem_Enter(object sender, EventArgs e) { if (txtTimKiem.Text == PlaceholderText) { txtTimKiem.Text = ""; txtTimKiem.ForeColor = Color.FromArgb(15, 23, 42); } }
        private void txtTimKiem_Leave(object sender, EventArgs e) { if (string.IsNullOrWhiteSpace(txtTimKiem.Text)) { txtTimKiem.Text = PlaceholderText; txtTimKiem.ForeColor = Color.FromArgb(148, 163, 184); } }
        private void dgvLaptop_CellDoubleClick(object sender, DataGridViewCellEventArgs e) { if (e.RowIndex >= 0) ChonSanPhamHienTai(); }
        private void dgvLaptop_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; ChonSanPhamHienTai(); } }
        private void btnChon_Click(object sender, EventArgs e) => ChonSanPhamHienTai();
        private void btnDong_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
    }
}

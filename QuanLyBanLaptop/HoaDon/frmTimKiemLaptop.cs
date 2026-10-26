using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmTimKiemLaptop : QuanLyBanLaptop.Classes.AppForm
    {
        private const string PlaceholderText = "Nhập tên máy, hãng, chip xử lý...";
        private DataTable _dtSource;
        private DataView _dv;

        public string SelectedMaLaptop { get; private set; }
        public string SelectedTenLaptop { get; private set; }
        public decimal SelectedDonGia { get; private set; }
        public int SelectedBaoHanh { get; private set; }
        public int SelectedSoLuongTon { get; private set; }

        // Khởi tạo giao diện và đăng ký sự kiện cho form.
        public frmTimKiemLaptop()
        {
            InitializeComponent();
        }

        // Khởi tạo giao diện và đăng ký sự kiện cho form.
        private void frmTimKiemLaptop_Load(object sender, EventArgs e)
        {
            LoadData();
            txtTimKiem.Focus();
        }

        // Tải dữ liệu phục vụ LoadData.
        private void LoadData()
        {
            try
            {
                string query = @"
SELECT 
    l.MaLaptop,
    l.TenLaptop,
    CASE 
        WHEN l.TenLaptop LIKE 'Acer%' THEN 'Acer'
        WHEN l.TenLaptop LIKE 'Asus%' THEN 'Asus'
        WHEN l.TenLaptop LIKE 'Dell%' THEN 'Dell'
        WHEN l.TenLaptop LIKE 'HP%' THEN 'HP'
        WHEN l.TenLaptop LIKE 'Lenovo%' THEN 'Lenovo'
        WHEN l.TenLaptop LIKE 'MacBook%' OR l.TenLaptop LIKE 'Apple%' THEN 'Apple'
        WHEN n.TenNCC LIKE '%Acer%' THEN 'Acer'
        WHEN n.TenNCC LIKE '%Asus%' THEN 'Asus'
        WHEN n.TenNCC LIKE '%Dell%' THEN 'Dell'
        WHEN n.TenNCC LIKE '%HP%' THEN 'HP'
        WHEN n.TenNCC LIKE '%Lenovo%' THEN 'Lenovo'
        WHEN n.TenNCC LIKE '%Apple%' THEN 'Apple'
        ELSE ISNULL(l.Loai, N'Khác')
    END AS Hang,
    ISNULL(l.CPU, '') + 
    CASE WHEN l.RAM IS NOT NULL AND l.RAM <> '' THEN ' / ' + l.RAM ELSE '' END + 
    CASE WHEN l.SSD IS NOT NULL AND l.SSD <> '' THEN ' / ' + l.SSD + (CASE WHEN l.SSD NOT LIKE '%SSD%' THEN ' SSD' ELSE '' END) ELSE '' END AS CauHinhChinh,
    l.DonGia,
    l.SoLuongTon,
    l.ThoiGianBaoHanh,
    l.CPU
FROM LAPTOP l
LEFT JOIN NHA_CUNG_CAP n ON l.MaNCC = n.MaNCC
WHERE l.NgungKinhDoanh = 0
ORDER BY l.TenLaptop";

                _dtSource = DataProcesser.ReadData(query);
                if (!_dtSource.Columns.Contains("STT"))
                {
                    _dtSource.Columns.Add("STT", typeof(int));
                }
                if (!_dtSource.Columns.Contains("GiaBanFormatted"))
                {
                    _dtSource.Columns.Add("GiaBanFormatted", typeof(string));
                }

                _dv = new DataView(_dtSource);
                BindData();
            }
            catch (Exception ex)
            {
                Function.ThongBaoLoi("Lỗi tải danh sách laptop: " + ex.Message, "Lỗi");
            }
        }

        // Thực hiện BindData theo trạng thái và dữ liệu hiện tại.
        private void BindData()
        {
            if (_dv == null) return;

            // Đánh lại số thứ tự STT và định dạng giá bán
            int stt = 1;
            foreach (DataRowView rowView in _dv)
            {
                rowView["STT"] = stt++;
                decimal gia = rowView["DonGia"] != DBNull.Value ? Convert.ToDecimal(rowView["DonGia"]) : 0;
                rowView["GiaBanFormatted"] = string.Format("{0:N0} đ", gia);
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

        // Thực hiện FilterData theo trạng thái và dữ liệu hiện tại.
        private void FilterData()
        {
            if (_dtSource == null || _dv == null) return;

            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword) || keyword == PlaceholderText)
            {
                _dv.RowFilter = "";
            }
            else
            {
                string safe = keyword.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]");
                _dv.RowFilter = string.Format(
                    "TenLaptop LIKE '%{0}%' OR Hang LIKE '%{0}%' OR CauHinhChinh LIKE '%{0}%' OR MaLaptop LIKE '%{0}%' OR CPU LIKE '%{0}%'",
                    safe);
            }

            BindData();
        }

        // Thực hiện ChonSanPhamHienTai theo trạng thái và dữ liệu hiện tại.
        private void ChonSanPhamHienTai()
        {
            if (dgvLaptop.CurrentRow != null && dgvLaptop.CurrentRow.Index >= 0)
            {
                var row = dgvLaptop.CurrentRow;
                SelectedMaLaptop = row.Cells["colMaLaptop"].Value?.ToString() ?? "";
                SelectedTenLaptop = row.Cells["colTenLaptop"].Value?.ToString() ?? "";

                decimal.TryParse(row.Cells["colGiaBan"].Value?.ToString().Replace(" đ", "").Replace(",", "").Replace(".", ""), out decimal dg);
                SelectedDonGia = dg;

                int.TryParse(row.Cells["colThoiGianBaoHanh"].Value?.ToString(), out int bh);
                SelectedBaoHanh = bh > 0 ? bh : 12;

                int.TryParse(row.Cells["colSoLuongTon"].Value?.ToString(), out int slt);
                SelectedSoLuongTon = slt;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                Function.ThongBaoThanhCong("Vui lòng chọn một sản phẩm trong danh sách.", "Thông báo");
            }
        }

        // Xử lý sự kiện btnTimKiem_Click của giao diện.
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            FilterData();
        }

        // Xử lý sự kiện txtTimKiem_TextChanged của giao diện.
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtTimKiem.Text != PlaceholderText)
            {
                FilterData();
            }
        }

        // Xử lý sự kiện txtTimKiem_KeyDown của giao diện.
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                FilterData();
                if (dgvLaptop.Rows.Count > 0)
                {
                    dgvLaptop.Focus();
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (dgvLaptop.Rows.Count > 0)
                {
                    dgvLaptop.Focus();
                    e.SuppressKeyPress = true;
                }
            }
        }

        // Xử lý sự kiện txtTimKiem_Enter của giao diện.
        private void txtTimKiem_Enter(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == PlaceholderText)
            {
                txtTimKiem.Text = "";
                txtTimKiem.ForeColor = Color.FromArgb(15, 23, 42);
            }
        }

        // Xử lý sự kiện txtTimKiem_Leave của giao diện.
        private void txtTimKiem_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                txtTimKiem.Text = PlaceholderText;
                txtTimKiem.ForeColor = Color.FromArgb(148, 163, 184);
            }
        }

        // Xử lý sự kiện dgvLaptop_CellDoubleClick của giao diện.
        private void dgvLaptop_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ChonSanPhamHienTai();
            }
        }

        // Xử lý sự kiện dgvLaptop_KeyDown của giao diện.
        private void dgvLaptop_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ChonSanPhamHienTai();
            }
        }

        // Xử lý sự kiện btnChon_Click của giao diện.
        private void btnChon_Click(object sender, EventArgs e)
        {
            ChonSanPhamHienTai();
        }

        // Xử lý sự kiện btnDong_Click của giao diện.
        private void btnDong_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

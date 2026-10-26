using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmTimKiemKhachHang : QuanLyBanLaptop.Classes.AppForm
    {
        private const string PlaceholderText = "Nhập mã, tên hoặc số điện thoại khách hàng...";
        private DataTable _dtSource;
        private DataView _dv;

        public string SelectedMaKH { get; private set; }
        public string SelectedTenKH { get; private set; }
        public string SelectedSdt { get; private set; }
        public string SelectedDiaChi { get; private set; }

        public frmTimKiemKhachHang()
        {
            InitializeComponent();
        }

        private void frmTimKiemKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
            txtTimKiem.Focus();
        }

        public void LoadData(string tuKhoa = null)
        {
            try
            {
                string sql = @"
                    SELECT 
                        MaKH, 
                        TenKH, 
                        ISNULL(Sdt, '') AS Sdt, 
                        ISNULL(DiaChi, '') AS DiaChi, 
                        ISNULL(HangThanhVien, N'Đồng') AS HangThanhVien,
                        ISNULL(GioiTinh, N'Nam') AS GioiTinh
                    FROM dbo.KHACH_HANG
                    WHERE DaKhoa = 0 OR DaKhoa IS NULL
                    ORDER BY MaKH DESC";

                _dtSource = Function.DocBang(sql);
                if (_dtSource == null || _dtSource.Rows.Count == 0)
                {
                    // Dữ liệu mẫu dự phòng nếu DB chưa kết nối
                    _dtSource = new DataTable();
                    _dtSource.Columns.Add("MaKH", typeof(string));
                    _dtSource.Columns.Add("TenKH", typeof(string));
                    _dtSource.Columns.Add("Sdt", typeof(string));
                    _dtSource.Columns.Add("DiaChi", typeof(string));
                    _dtSource.Columns.Add("HangThanhVien", typeof(string));
                    _dtSource.Columns.Add("GioiTinh", typeof(string));

                    _dtSource.Rows.Add("KH001", "Trần Anh Tuấn", "0901234567", "Hà Nội", "Vàng", "Nam");
                    _dtSource.Rows.Add("KH002", "Hoàng Minh Đức", "0912345678", "Hải Phòng", "Bạc", "Nam");
                    _dtSource.Rows.Add("KH003", "Nguyễn Thảo My", "0923456789", "Đà Nẵng", "Đồng", "Nữ");
                    _dtSource.Rows.Add("KH004", "Lê Quang Hải", "0934567890", "Hà Nội", "VIP", "Nam");
                    _dtSource.Rows.Add("KH005", "Phạm Minh Đăng", "0945678901", "Bắc Ninh", "Vàng", "Nam");
                    _dtSource.Rows.Add("KH006", "Đinh Công Trình", "0956789012", "Hà Nam", "Đồng", "Nam");
                    _dtSource.Rows.Add("KH007", "Bùi Thị Mai", "0967890123", "Hưng Yên", "Bạc", "Nữ");
                    _dtSource.Rows.Add("KH008", "Ngô Văn Long", "0978901234", "Vĩnh Phúc", "Đồng", "Nam");
                }

                _dv = new DataView(_dtSource);
                dgvKhachHang.DataSource = _dv;
                ConfigureGridColumns();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                Function.ThongBaoLoi("Lỗi tải danh sách khách hàng: " + ex.Message);
            }
        }

        private void ConfigureGridColumns()
        {
            if (dgvKhachHang.Columns.Count == 0) return;

            Function.DatTieuDeCot(dgvKhachHang, "MaKH", "Mã KH", 100);
            Function.DatTieuDeCot(dgvKhachHang, "TenKH", "Họ và tên khách hàng", 190);
            Function.DatTieuDeCot(dgvKhachHang, "Sdt", "Số điện thoại", 125);
            Function.DatTieuDeCot(dgvKhachHang, "DiaChi", "Địa chỉ", 200);
            Function.DatTieuDeCot(dgvKhachHang, "HangThanhVien", "Hạng TV", 105);
            Function.DatTieuDeCot(dgvKhachHang, "GioiTinh", "Giới tính", 85);
        }

        private void ApplyFilter()
        {
            if (_dv == null) return;

            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword) || keyword == PlaceholderText)
            {
                _dv.RowFilter = string.Empty;
            }
            else
            {
                string safe = keyword.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]");
                _dv.RowFilter = $"MaKH LIKE '%{safe}%' OR TenKH LIKE '%{safe}%' OR Sdt LIKE '%{safe}%' OR DiaChi LIKE '%{safe}%'";
            }

            lblThongKe.Text = $"Tổng số: {_dv.Count} khách hàng";
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtTimKiem.Text != PlaceholderText)
            {
                ApplyFilter();
            }
        }

        private void txtTimKiem_Enter(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == PlaceholderText)
            {
                txtTimKiem.Text = "";
                txtTimKiem.ForeColor = Color.FromArgb(30, 41, 59);
            }
        }

        private void txtTimKiem_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                txtTimKiem.Text = PlaceholderText;
                txtTimKiem.ForeColor = Color.FromArgb(148, 163, 184);
                ApplyFilter();
            }
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ApplyFilter();
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (dgvKhachHang.Rows.Count > 0)
                {
                    dgvKhachHang.Focus();
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnChon_Click(object sender, EventArgs e)
        {
            ChonKhachHangHienTai();
        }

        private void dgvKhachHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ChonKhachHangHienTai();
            }
        }

        private void dgvKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                ChonKhachHangHienTai();
            }
        }

        private void ChonKhachHangHienTai()
        {
            if (dgvKhachHang.CurrentRow == null)
            {
                Function.ThongBaoCanhBao("Vui lòng chọn một khách hàng trong danh sách!");
                return;
            }

            DataRowView rowView = dgvKhachHang.CurrentRow.DataBoundItem as DataRowView;
            if (rowView != null)
            {
                SelectedMaKH = rowView["MaKH"]?.ToString();
                SelectedTenKH = rowView["TenKH"]?.ToString();
                SelectedSdt = rowView["Sdt"]?.ToString();
                SelectedDiaChi = rowView["DiaChi"]?.ToString();

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        // Thêm nhanh khách hàng mới ngay trên hộp thoại
        private void btnThemNhanh_Click(object sender, EventArgs e)
        {
            using (var dlg = new Form())
            {
                dlg.Text = "Thêm Khách Hàng Mới";
                dlg.Size = new Size(460, 310);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.BackColor = Color.FromArgb(248, 250, 252);
                dlg.Font = new Font("Segoe UI", 9F);

                var lblHeader = new Label
                {
                    Text = "THÊM NHANH KHÁCH HÀNG MỚI",
                    Dock = DockStyle.Top,
                    Height = 40,
                    BackColor = Color.FromArgb(17, 35, 64),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                dlg.Controls.Add(lblHeader);

                // Tự sinh mã khách hàng mới
                string newMaKH = "KH001";
                try
                {
                    newMaKH = Function.SinhMaTuDong("KHACH_HANG", "MaKH", "KH", 2);
                }
                catch
                {
                    newMaKH = $"KH{(_dtSource != null ? _dtSource.Rows.Count + 1 : 1):D3}";
                }

                int y = 55;
                var lblMa = new Label { Text = "Mã KH:", Location = new Point(25, y + 3), Size = new Size(90, 20), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                var txtMa = new TextBox { Text = newMaKH, Location = new Point(125, y), Size = new Size(290, 26), ReadOnly = true, BackColor = Color.FromArgb(241, 245, 249) };
                dlg.Controls.Add(lblMa);
                dlg.Controls.Add(txtMa);

                y += 38;
                var lblTen = new Label { Text = "Họ và tên *:", Location = new Point(25, y + 3), Size = new Size(90, 20), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
                var txtTen = new TextBox { Location = new Point(125, y), Size = new Size(290, 26) };
                dlg.Controls.Add(lblTen);
                dlg.Controls.Add(txtTen);

                y += 38;
                var lblSdt = new Label { Text = "Số điện thoại:", Location = new Point(25, y + 3), Size = new Size(90, 20) };
                var txtSdt = new TextBox { Location = new Point(125, y), Size = new Size(290, 26) };
                dlg.Controls.Add(lblSdt);
                dlg.Controls.Add(txtSdt);

                y += 38;
                var lblDiaChi = new Label { Text = "Địa chỉ:", Location = new Point(25, y + 3), Size = new Size(90, 20) };
                var txtDiaChi = new TextBox { Location = new Point(125, y), Size = new Size(290, 26) };
                dlg.Controls.Add(lblDiaChi);
                dlg.Controls.Add(txtDiaChi);

                y += 48;
                var btnLuu = new Button
                {
                    Text = "Lưu & Chọn luôn",
                    Location = new Point(125, y),
                    Size = new Size(150, 32),
                    BackColor = Color.FromArgb(21, 128, 61),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnLuu.FlatAppearance.BorderSize = 0;

                var btnHuy = new Button
                {
                    Text = "Hủy bỏ",
                    Location = new Point(285, y),
                    Size = new Size(130, 32),
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(51, 65, 85),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnHuy.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
                btnHuy.Click += (s2, e2) => dlg.Close();

                btnLuu.Click += (s2, e2) =>
                {
                    string ten = txtTen.Text.Trim();
                    if (string.IsNullOrEmpty(ten))
                    {
                        Function.ThongBaoCanhBao("Vui lòng nhập Họ và tên khách hàng!");
                        txtTen.Focus();
                        return;
                    }

                    string sdt = txtSdt.Text.Trim();
                    string diaChi = txtDiaChi.Text.Trim();

                    try
                    {
                        string insertSql = @"
                            INSERT INTO dbo.KHACH_HANG (MaKH, TenKH, Sdt, DiaChi, HangThanhVien, GioiTinh, DaKhoa)
                            VALUES (@MaKH, @TenKH, @Sdt, @DiaChi, N'Đồng', N'Nam', 0)";

                        Function.CapNhat(insertSql,
                            new SqlParameter("@MaKH", newMaKH),
                            new SqlParameter("@TenKH", ten),
                            new SqlParameter("@Sdt", string.IsNullOrEmpty(sdt) ? (object)DBNull.Value : sdt),
                            new SqlParameter("@DiaChi", string.IsNullOrEmpty(diaChi) ? (object)DBNull.Value : diaChi));
                    }
                    catch { }

                    // Thêm vào DataTable nguồn
                    if (_dtSource != null)
                    {
                        var newRow = _dtSource.NewRow();
                        newRow["MaKH"] = newMaKH;
                        newRow["TenKH"] = ten;
                        newRow["Sdt"] = sdt;
                        newRow["DiaChi"] = diaChi;
                        newRow["HangThanhVien"] = "Đồng";
                        newRow["GioiTinh"] = "Nam";
                        _dtSource.Rows.InsertAt(newRow, 0);
                    }

                    SelectedMaKH = newMaKH;
                    SelectedTenKH = ten;
                    SelectedSdt = sdt;
                    SelectedDiaChi = diaChi;

                    dlg.DialogResult = DialogResult.OK;
                    dlg.Close();

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                };

                dlg.Controls.Add(btnLuu);
                dlg.Controls.Add(btnHuy);

                dlg.ShowDialog(this);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

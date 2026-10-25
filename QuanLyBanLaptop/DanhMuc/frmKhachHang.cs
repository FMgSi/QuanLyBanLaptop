using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmKhachHang : Form
    {
        // Trạng thái form
        private enum FormMode { View, Add, Edit, Delete }
        private FormMode _mode = FormMode.View;
        private string _selectedMaKH = null;
        private bool _currentSelectedIsLocked = false;

        // Bảng màu chuẩn
        private static readonly Color ColorDisabled = Color.FromArgb(148, 163, 184); // Xám khi không dùng được (giống Lưu và Hủy)
        private static readonly Color ColorGreen    = Color.FromArgb(34, 197, 94);   // Xanh lá (Thêm, Lưu)
        private static readonly Color ColorBlue     = Color.FromArgb(37, 99, 235);   // Xanh dương (Sửa)
        private static readonly Color ColorRed      = Color.FromArgb(239, 68, 68);   // Đỏ (Xóa, Hủy)
        private static readonly Color ColorAmber    = Color.FromArgb(217, 119, 6);   // Cam (Khóa)
        private static readonly Color ColorEmerald  = Color.FromArgb(16, 185, 129);  // Xanh ngọc (Mở khóa)

        public frmKhachHang()
        {
            InitializeComponent();
            NavigationManager.RegisterForm(NavigationMenu.KhachHang, this);
            sidebarControl1.SelectedMenu = NavigationMenu.KhachHang;

            // Khởi tạo ComboBox giới tính
            cboGioiTinh.Items.Clear();
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ" });
            cboGioiTinh.SelectedIndex = 0;

            // Khởi tạo ComboBox hạng thành viên
            cboHangThanhVien.Items.Clear();
            cboHangThanhVien.Items.AddRange(new object[] { "Đồng", "Bạc", "Vàng", "VIP" });
            cboHangThanhVien.SelectedIndex = 0;

            SetFormMode(FormMode.View);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.KhachHang;
                LoadDanhSachKhachHang();
            }
        }

        // ─── Tải danh sách ───────────────────────────────────────────────
        private void LoadDanhSachKhachHang(string tuKhoa = null)
        {
            try
            {
                DataTable dt = DataProcesser.ExecProc("sp_TimKiemKhachHang", new[]
                {
                    new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 100)
                    { Value = string.IsNullOrEmpty(tuKhoa) ? (object)DBNull.Value : tuKhoa }
                });

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Tự động sắp xếp chuẩn theo thứ tự số mã KH: 001 -> 002 -> 003...
                    var sortedRows = dt.AsEnumerable()
                        .OrderBy(r => GetMaKHSortKey(r.Field<string>("MaKH")))
                        .ToList();
                    dt = sortedRows.CopyToDataTable();
                }

                dgvKhachHang.DataSource = dt;
                SetGridHeaders();
            }
            catch (SqlException ex) { DataProcesser.ShowSqlError(ex); }
        }

        // doi tieu de cot datagridview
        private void SetGridHeaders()
        {
            if (dgvKhachHang.Columns.Count == 0) return;
            Function.DatTieuDeCot(dgvKhachHang, "MaKH",          "Mã khách hàng", 110);
            Function.DatTieuDeCot(dgvKhachHang, "TenKH",         "Họ tên",         160);
            Function.DatTieuDeCot(dgvKhachHang, "GioiTinh",      "Giới tính",       85);
            Function.DatTieuDeCot(dgvKhachHang, "HangThanhVien", "Hạng thành viên", 120);
            Function.DatTieuDeCot(dgvKhachHang, "NgaySinh",      "Ngày sinh",       100);
            Function.DatTieuDeCot(dgvKhachHang, "Sdt",           "Số điện thoại",   115);
            Function.DatTieuDeCot(dgvKhachHang, "DiaChi",        "Địa chỉ",         200);
            Function.DatTieuDeCot(dgvKhachHang, "TrangThai",     "Trạng thái",      100);
        }

        // thiet lap che do form khach hang
        private void SetFormMode(FormMode mode)
        {
            _mode = mode;
            bool isView = (mode == FormMode.View);
            bool isEditing = (mode == FormMode.Add || mode == FormMode.Edit);
            bool isDelete = (mode == FormMode.Delete);
            bool hasSelected = (_selectedMaKH != null);

            txtMaKH.ReadOnly     = true;
            txtHoTen.ReadOnly    = !isEditing;
            cboGioiTinh.Enabled  =  isEditing;
            dtpNgaySinh.Enabled  =  isEditing;
            txtSDT.ReadOnly      = !isEditing;
            txtDiaChi.ReadOnly   = !isEditing;
            cboHangThanhVien.Enabled = isEditing;

            txtTimKiem.Enabled = isView;
            btnTimKiem.Enabled = isView;
            btnLamMoi.Enabled  = isView;

            if (isView)
            {
                btnThem.Visible = true;
                btnSua.Visible  = true;
                btnXoa.Visible  = true;
                btnKhoa.Visible = true;

                btnThem.Enabled = true;
                btnSua.Enabled  = hasSelected;
                btnXoa.Enabled  = hasSelected;
                btnKhoa.Enabled = hasSelected;

                StyleButton(btnThem, btnThem.Enabled ? ColorGreen : ColorDisabled, Color.White);
                StyleButton(btnSua,  btnSua.Enabled  ? ColorBlue  : ColorDisabled, Color.White);
                StyleButton(btnXoa,  btnXoa.Enabled  ? ColorRed   : ColorDisabled, Color.White);

                Color activeKhoaColor = _currentSelectedIsLocked ? ColorEmerald : ColorAmber;
                StyleButton(btnKhoa, btnKhoa.Enabled ? activeKhoaColor : ColorDisabled, Color.White);

                btnLuu.Visible = false;
                btnHuy.Visible = false;
            }
            else
            {
                btnThem.Visible = false;
                btnSua.Visible  = false;
                btnXoa.Visible  = false;
                btnKhoa.Visible = false;

                btnLuu.Location = new Point(10, 8);
                btnHuy.Location = new Point(150, 8);
                btnLuu.Size     = new Size(130, 38);
                btnHuy.Size     = new Size(130, 38);

                btnLuu.Text = "💾  Lưu";
                btnHuy.Text = "✕  Hủy";

                btnLuu.Visible = true;
                btnHuy.Visible = true;
                btnLuu.Enabled = true;
                btnHuy.Enabled = true;

                StyleButton(btnLuu, ColorGreen, Color.White);
                StyleButton(btnHuy, ColorRed, Color.White);
            }
        }

        private void StyleButton(Button btn, Color back, Color fore)
        {
            btn.BackColor = back;
            btn.ForeColor = fore;
        }

        // do du lieu tu dong vao o nhap
        private void FillFormFromGrid(DataGridViewRow row)
        {
            if (row == null) return;
            txtMaKH.Text   = Function.LayGiaTriCell(row, "MaKH");
            txtHoTen.Text  = Function.LayGiaTriCell(row, "TenKH");
            txtSDT.Text    = Function.LayGiaTriCell(row, "Sdt");
            txtDiaChi.Text = Function.LayGiaTriCell(row, "DiaChi");

            string gt = Function.LayGiaTriCell(row, "GioiTinh", "Nam");
            cboGioiTinh.SelectedItem = gt;

            string hang = Function.LayGiaTriCell(row, "HangThanhVien", "Đồng");
            cboHangThanhVien.SelectedItem = hang;

            string ngaySinh = Function.LayGiaTriCell(row, "NgaySinh");
            if (!string.IsNullOrEmpty(ngaySinh) &&
                (DateTime.TryParseExact(ngaySinh, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime ns)
                 || DateTime.TryParse(ngaySinh, out ns)))
            {
                dtpNgaySinh.Value = ns;
            }
            else
            {
                dtpNgaySinh.Value = DateTime.Today.AddYears(-20);
            }

            string tt = Function.LayGiaTriCell(row, "TrangThai");
            _currentSelectedIsLocked = (tt == "Đã khóa");
            btnKhoa.Text = _currentSelectedIsLocked ? "🔓  Mở khóa khách hàng" : "🔒  Khóa khách hàng";
        }

        // xoa trang thong tin va sinh ma khach hang tu dong KHxx
        private void ClearForm()
        {
            txtMaKH.Text         = Function.SinhMaTuDong("KHACH_HANG", "MaKH", "KH", 2);
            txtHoTen.Text        = "";
            txtSDT.Text          = "";
            txtDiaChi.Text       = "";
            cboGioiTinh.SelectedIndex      = 0;
            cboHangThanhVien.SelectedIndex = 0;
            dtpNgaySinh.Value    = DateTime.Today.AddYears(-20);
            _currentSelectedIsLocked = false;
            btnKhoa.Text         = "🔒  Khóa khách hàng";
        }

        // tim kiem khach hang
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDanhSachKhachHang(txtTimKiem.Text.Trim());
        }

        // lam moi danh sach va form
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            LoadDanhSachKhachHang();
            _selectedMaKH = null;
            ClearForm();
            SetFormMode(FormMode.View);
        }

        // them khach hang moi tu dong sinh ma KHxx
        private void btnThem_Click(object sender, EventArgs e)
        {
            _selectedMaKH = null;
            ClearForm();
            SetFormMode(FormMode.Add);
            txtHoTen.Focus();
        }

        // ─── Nút Sửa ─────────────────────────────────────────────────────
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedMaKH == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SetFormMode(FormMode.Edit);
            txtHoTen.Focus();
        }

        // ─── Nút Lưu ─────────────────────────────────────────────────────
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mode == FormMode.Add)
                {
                    if (!ValidateInput()) return;

                    // Thêm mới: Mã KH do nhân viên tự nhập
                    string maKH = txtMaKH.Text.Trim().ToUpper();
                    DataProcesser.ExecProcNonQuery("sp_ThemKhachHang", new[]
                    {
                        new SqlParameter("@MaKH",          maKH),
                        new SqlParameter("@TenKH",         txtHoTen.Text.Trim()),
                        new SqlParameter("@GioiTinh",       cboGioiTinh.SelectedItem.ToString()),
                        new SqlParameter("@NgaySinh",       dtpNgaySinh.Value.Date),
                        new SqlParameter("@Sdt",            txtSDT.Text.Trim()),
                        new SqlParameter("@DiaChi",         txtDiaChi.Text.Trim()),
                        new SqlParameter("@HangThanhVien",  cboHangThanhVien.SelectedItem.ToString()),
                    });

                    MessageBox.Show($"Thêm khách hàng \"{maKH}\" thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_mode == FormMode.Edit)
                {
                    if (!ValidateInput()) return;

                    // Cập nhật
                    DataProcesser.ExecProcNonQuery("sp_CapNhatKhachHang", new[]
                    {
                        new SqlParameter("@MaKH",          _selectedMaKH),
                        new SqlParameter("@TenKH",         txtHoTen.Text.Trim()),
                        new SqlParameter("@GioiTinh",       cboGioiTinh.SelectedItem.ToString()),
                        new SqlParameter("@NgaySinh",       dtpNgaySinh.Value.Date),
                        new SqlParameter("@Sdt",            txtSDT.Text.Trim()),
                        new SqlParameter("@DiaChi",         txtDiaChi.Text.Trim()),
                        new SqlParameter("@HangThanhVien",  cboHangThanhVien.SelectedItem.ToString()),
                    });
                    MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_mode == FormMode.Delete)
                {
                    // Xác nhận xóa khách hàng
                    string tenKH = txtHoTen.Text;
                    var confirm = MessageBox.Show(
                        $"Bạn có chắc muốn XÓA khách hàng \"{tenKH}\" ({_selectedMaKH})?\n\nLưu ý: Khách hàng đã có hóa đơn sẽ không xóa được.",
                        "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm != DialogResult.Yes) return;

                    DataProcesser.ExecProcNonQuery("sp_XoaKhachHang", new[]
                    {
                        new SqlParameter("@MaKH", _selectedMaKH)
                    });
                    MessageBox.Show("Xóa khách hàng thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadDanhSachKhachHang(txtTimKiem.Text.Trim());
                _selectedMaKH = null;
                ClearForm();
                SetFormMode(FormMode.View);
            }
            catch (SqlException ex) { DataProcesser.ShowSqlError(ex); }
        }

        // ─── Nút Xóa ─────────────────────────────────────────────────────
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedMaKH == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Chuyển sang chế độ xóa: ẩn 4 nút, hiện [Xác nhận xóa] và [Hủy] ở vị trí Thêm và Sửa
            SetFormMode(FormMode.Delete);
        }

        // ─── Nút Khóa / Mở khóa khách hàng ──────────────────────────────
        private void btnKhoa_Click(object sender, EventArgs e)
        {
            if (_selectedMaKH == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần khóa / mở khóa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow row = dgvKhachHang.CurrentRow;
            if (row == null) return;
            string tt = row.Cells["TrangThai"]?.Value?.ToString() ?? "";
            bool daKhoa = (tt == "Đã khóa");
            string action = daKhoa ? "MỞ KHÓA" : "KHÓA";

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn {action} khách hàng \"{txtHoTen.Text}\" ({_selectedMaKH})?",
                $"Xác nhận {action.ToLower()}", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                DataProcesser.ExecProcNonQuery("sp_KhoaKhachHang", new[]
                {
                    new SqlParameter("@MaKH",   _selectedMaKH),
                    new SqlParameter("@DaKhoa", !daKhoa)
                });

                string msg = daKhoa ? "Đã mở khóa khách hàng thành công!" : "Đã khóa khách hàng thành công!";
                MessageBox.Show(msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDanhSachKhachHang(txtTimKiem.Text.Trim());
                _selectedMaKH = null;
                ClearForm();
                SetFormMode(FormMode.View);
            }
            catch (SqlException ex) { DataProcesser.ShowSqlError(ex); }
        }

        // ─── Nút Hủy ─────────────────────────────────────────────────────
        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (_mode == FormMode.Edit || _mode == FormMode.Delete)
            {
                // Khi hủy Sửa hoặc Xóa: khôi phục lại dữ liệu của dòng đang chọn
                if (dgvKhachHang.CurrentRow != null)
                {
                    FillFormFromGrid(dgvKhachHang.CurrentRow);
                }
                SetFormMode(FormMode.View);
            }
            else
            {
                _selectedMaKH = null;
                ClearForm();
                SetFormMode(FormMode.View);
            }
        }

        // ─── Chọn dòng trong DataGridView ────────────────────────────────
        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];
            _selectedMaKH = row.Cells["MaKH"]?.Value?.ToString();
            FillFormFromGrid(row);
            if (_mode == FormMode.View)
                SetFormMode(FormMode.View); // refresh button states
        }

        // ─── Tìm kiếm khi gõ Enter ────────────────────────────────────────
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoadDanhSachKhachHang(txtTimKiem.Text.Trim());
        }

        // ─── Chỉ cho phép nhập số ở ô Số điện thoại ─────────────────────
        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép các phím điều khiển (như Backspace, Delete) và các chữ số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // ─── Tách số thứ tự từ mã KH để tự động sắp xếp (001 -> 002 -> 003...) ───

        private static int GetMaKHSortKey(string maKH)
        {
            if (string.IsNullOrEmpty(maKH)) return int.MaxValue;
            var match = Regex.Match(maKH, @"\d+");
            if (match.Success && int.TryParse(match.Value, out int num))
                return num;
            return int.MaxValue;
        }

        // ─── Validate đầu vào ────────────────────────────────────────────
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
            {
                MessageBox.Show("Vui lòng nhập mã khách hàng.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKH.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên khách hàng.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                string sdt = txtSDT.Text.Trim();
                if (!Regex.IsMatch(sdt, @"^[0-9]+$"))
                {
                    MessageBox.Show("Số điện thoại chỉ được chứa các chữ số.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    return false;
                }
            }

            return true;
        }
    }
}

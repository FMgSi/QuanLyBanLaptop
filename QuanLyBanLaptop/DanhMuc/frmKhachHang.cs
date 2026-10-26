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
    public partial class frmKhachHang : QuanLyBanLaptop.Classes.AppForm
    {
        // Trạng thái thao tác form theo logic chuẩn
        private enum FormMode { Normal, Adding, Selected, Editing }
        private FormMode _mode = FormMode.Normal;
        private string _selectedMaKH = null;

        // Khởi tạo giao diện và đăng ký sự kiện cho form.
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

            SetFormMode(FormMode.Normal);
        }

        // Thực hiện OnVisibleChanged theo trạng thái và dữ liệu hiện tại.
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.KhachHang;
                LoadDanhSachKhachHang();
                _selectedMaKH = null;
                ClearForm();
                dgvKhachHang.ClearSelection();
                SetFormMode(FormMode.Normal);
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
                dgvKhachHang.ClearSelection();
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

        // Thiết lập chế độ thao tác form khách hàng theo chuẩn ButtonTheme
        private void SetFormMode(FormMode mode)
        {
            _mode = mode;
            bool isEditing = (mode == FormMode.Adding || mode == FormMode.Editing);

            txtMaKH.ReadOnly     = true;
            txtHoTen.ReadOnly    = !isEditing;
            cboGioiTinh.Enabled  =  isEditing;
            dtpNgaySinh.Enabled  =  isEditing;
            txtSDT.ReadOnly      = !isEditing;
            txtDiaChi.ReadOnly   = !isEditing;
            cboHangThanhVien.Enabled = false;

            txtTimKiem.Enabled = !isEditing;
            btnTimKiem.Enabled = !isEditing;
            btnLamMoi.Enabled  = !isEditing;

            switch (mode)
            {
                case FormMode.Normal:
                    // 1. Khi mở bảng lên: chỉ nút Thêm và Thoát bấm được
                    ButtonTheme.SetButtonState(btnThem, true);
                    ButtonTheme.SetButtonState(btnSua, false);
                    ButtonTheme.SetButtonState(btnXoa, false);
                    ButtonTheme.SetButtonState(btnLuu, false);
                    ButtonTheme.SetButtonState(btnHuy, false);
                    ButtonTheme.SetButtonState(btnThoat, true);
                    break;

                case FormMode.Adding:
                    // 2. Khi bấm vào nút Thêm: nút Hủy, Lưu sẽ bấm được
                    ButtonTheme.SetButtonState(btnThem, false);
                    ButtonTheme.SetButtonState(btnSua, false);
                    ButtonTheme.SetButtonState(btnXoa, false);
                    ButtonTheme.SetButtonState(btnLuu, true);
                    ButtonTheme.SetButtonState(btnHuy, true);
                    ButtonTheme.SetButtonState(btnThoat, true);
                    break;

                case FormMode.Selected:
                    // 3. Nếu bấm vào dòng trong DataGridView: nút Hủy, Sửa (và Xóa) sẽ bấm được
                    ButtonTheme.SetButtonState(btnThem, false);
                    ButtonTheme.SetButtonState(btnSua, true);
                    ButtonTheme.SetButtonState(btnXoa, true);
                    ButtonTheme.SetButtonState(btnLuu, false);
                    ButtonTheme.SetButtonState(btnHuy, true);
                    ButtonTheme.SetButtonState(btnThoat, true);
                    break;

                case FormMode.Editing:
                    // 4. Bấm vào nút Sửa: nút Hủy, Lưu sẽ bấm được
                    ButtonTheme.SetButtonState(btnThem, false);
                    ButtonTheme.SetButtonState(btnSua, false);
                    ButtonTheme.SetButtonState(btnXoa, false);
                    ButtonTheme.SetButtonState(btnLuu, true);
                    ButtonTheme.SetButtonState(btnHuy, true);
                    ButtonTheme.SetButtonState(btnThoat, true);
                    break;
            }
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
        }

        // xoa trang thong tin
        private void ClearForm()
        {
            txtMaKH.Text         = "";
            txtHoTen.Text        = "";
            txtSDT.Text          = "";
            txtDiaChi.Text       = "";
            cboGioiTinh.SelectedIndex      = 0;
            cboHangThanhVien.SelectedIndex = 0;
            dtpNgaySinh.Value    = DateTime.Today.AddYears(-20);
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
            dgvKhachHang.ClearSelection();
            SetFormMode(FormMode.Normal);
        }

        // them khach hang moi tu dong sinh ma KHxx
        private void btnThem_Click(object sender, EventArgs e)
        {
            _selectedMaKH = null;
            ClearForm();
            txtMaKH.Text = Function.SinhMaTuDong("KHACH_HANG", "MaKH", "KH", 2);
            dgvKhachHang.ClearSelection();
            SetFormMode(FormMode.Adding);
            txtHoTen.Focus();
        }

        // ─── Nút Sửa ─────────────────────────────────────────────────────
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedMaKH == null)
            {
                Function.ThongBaoCanhBao("Vui lòng chọn khách hàng trong bảng cần sửa.", "Thông báo");
                return;
            }
            SetFormMode(FormMode.Editing);
            txtHoTen.Focus();
        }

        // ─── Nút Lưu ─────────────────────────────────────────────────────
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mode == FormMode.Adding)
                {
                    if (!ValidateInput()) return;

                    // Thêm mới: Mã KH do nhân viên tự nhập hoặc sinh tự động
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

                    Function.ThongBaoThanhCong($"Thêm khách hàng \"{maKH}\" thành công!", "Thành công");
                }
                else if (_mode == FormMode.Editing)
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
                    Function.ThongBaoThanhCong("Cập nhật thông tin khách hàng thành công!", "Thành công");
                }
                else
                {
                    return;
                }

                LoadDanhSachKhachHang(txtTimKiem.Text.Trim());
                _selectedMaKH = null;
                ClearForm();
                dgvKhachHang.ClearSelection();
                SetFormMode(FormMode.Normal);
            }
            catch (SqlException ex) { DataProcesser.ShowSqlError(ex); }
        }

        // ─── Nút Xóa ─────────────────────────────────────────────────────
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedMaKH == null)
            {
                Function.ThongBaoCanhBao("Vui lòng chọn khách hàng cần xóa.", "Thông báo");
                return;
            }

            string tenKH = txtHoTen.Text;
            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn XÓA khách hàng \"{tenKH}\" ({_selectedMaKH})?\n\nLưu ý: Khách hàng đã có hóa đơn sẽ không xóa được.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                DataProcesser.ExecProcNonQuery("sp_XoaKhachHang", new[]
                {
                    new SqlParameter("@MaKH", _selectedMaKH)
                });
                Function.ThongBaoThanhCong("Xóa khách hàng thành công!", "Thành công");
                LoadDanhSachKhachHang(txtTimKiem.Text.Trim());
                _selectedMaKH = null;
                ClearForm();
                dgvKhachHang.ClearSelection();
                SetFormMode(FormMode.Normal);
            }
            catch (SqlException ex) { DataProcesser.ShowSqlError(ex); }
        }

        // ─── Nút Hủy ─────────────────────────────────────────────────────
        private void btnHuy_Click(object sender, EventArgs e)
        {
            _selectedMaKH = null;
            ClearForm();
            dgvKhachHang.ClearSelection();
            SetFormMode(FormMode.Normal);
        }

        // ─── Nút Thoát ───────────────────────────────────────────────────
        private void btnThoat_Click(object sender, EventArgs e)
        {
            NavigationManager.Navigate(NavigationMenu.TrangChu);
        }

        // ─── Chọn dòng trong DataGridView ────────────────────────────────
        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];
            _selectedMaKH = row.Cells["MaKH"]?.Value?.ToString();
            FillFormFromGrid(row);
            SetFormMode(FormMode.Selected);
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

        // Thực hiện GetMaKHSortKey theo trạng thái và dữ liệu hiện tại.
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
                Function.ThongBaoCanhBao("Vui lòng nhập mã khách hàng.", "Cảnh báo");
                txtMaKH.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                Function.ThongBaoCanhBao("Vui lòng nhập họ tên khách hàng.", "Cảnh báo");
                txtHoTen.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                string sdt = txtSDT.Text.Trim();
                if (!Regex.IsMatch(sdt, @"^[0-9]+$"))
                {
                    Function.ThongBaoCanhBao("Số điện thoại chỉ được chứa các chữ số.", "Cảnh báo");
                    txtSDT.Focus();
                    return false;
                }
            }

            return true;
        }
    }
}

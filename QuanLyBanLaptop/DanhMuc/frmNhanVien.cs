using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.Services;
using QuanLyBanLaptop.Models;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmNhanVien : QuanLyBanLaptop.Classes.AppForm
    {
        protected override bool RequiresAdmin => true;
        private enum FormMode { Normal, Adding, Selected, Editing }
        private FormMode _mode = FormMode.Normal;
        private string _selectedMaNV = null;
        public frmNhanVien()
        {
            InitializeComponent();
            NavigationManager.RegisterForm(NavigationMenu.NhanVien, this);
            sidebarControl1.SelectedMenu = NavigationMenu.NhanVien;

            // Chức vụ
            cboChucVu.Items.Clear();
            cboChucVu.Items.AddRange(new object[] { "Quản trị viên", "Nhân viên" });
            cboChucVu.SelectedIndex = 1;

            SetFormMode(FormMode.Normal);
        }
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.NhanVien;
                LoadDanhSachNhanVien();
                _selectedMaNV = null;
                ClearForm();
                dgvNhanVien.ClearSelection();
                SetFormMode(FormMode.Normal);
            }
        }

        // ─── Tải danh sách ──────────────────────────────────────────────────
        private void LoadDanhSachNhanVien(string tuKhoa = null)
        {
            try
            {
                DataTable dt = new EmployeeService().Search(tuKhoa);
                dgvNhanVien.DataSource = dt;
                SetGridHeaders();
                dgvNhanVien.ClearSelection();
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }

        // doi tieu de cot datagridview
        private void SetGridHeaders()
        {
            if (dgvNhanVien.Columns.Count == 0) return;
            Function.DatTieuDeCot(dgvNhanVien, "MaNV",      "Mã NV", 95);
            Function.DatTieuDeCot(dgvNhanVien, "TenNV",     "Họ tên", 180);
            Function.DatTieuDeCot(dgvNhanVien, "GioiTinh",  "Giới tính", 85);
            Function.DatTieuDeCot(dgvNhanVien, "NgaySinh",  "Ngày sinh", 110);
            Function.DatTieuDeCot(dgvNhanVien, "SDT",       "SĐT", 120);
            Function.DatTieuDeCot(dgvNhanVien, "DiaChi",    "Địa chỉ", 220);
            Function.DatTieuDeCot(dgvNhanVien, "ChucVu",    "Chức vụ", 120);
            Function.DatTieuDeCot(dgvNhanVien, "TenDN",     "Tên đăng nhập", 130);
            Function.DatTieuDeCot(dgvNhanVien, "MatKhau",   "Mật khẩu", 120);
            Function.DatTieuDeCot(dgvNhanVien, "TrangThai", "Trạng thái", 125);
            dgvNhanVien.AllowUserToResizeColumns = true;
            dgvNhanVien.ScrollBars = ScrollBars.Both;
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        }

        // Thiết lập chế độ thao tác form nhân viên theo chuẩn ButtonTheme
        private void SetFormMode(FormMode mode)
        {
            _mode = mode;
            bool editing = (mode == FormMode.Adding || mode == FormMode.Editing);

            txtMaNV.ReadOnly    = true;
            txtTenNV.ReadOnly   = !editing;
            radNam.Enabled      =  editing;
            radNu.Enabled       =  editing;
            dtpNgaySinh.Enabled =  editing;
            txtSDT.ReadOnly     = !editing;
            txtDiaChi.ReadOnly  = !editing;
            cboChucVu.Enabled   =  editing;
            txtTenDN.ReadOnly   = !editing;
            txtMatKhau.ReadOnly = !editing;

            txtTimKiem.Enabled = !editing;
            btnTimKiem.Enabled = !editing;
            btnLamMoi.Enabled  = !editing;

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
            txtMaNV.Text    = Function.LayGiaTriCell(row, "MaNV");
            txtTenNV.Text   = Function.LayGiaTriCell(row, "TenNV");
            txtSDT.Text     = Function.LayGiaTriCell(row, "SDT");
            txtDiaChi.Text  = Function.LayGiaTriCell(row, "DiaChi");
            txtTenDN.Text   = Function.LayGiaTriCell(row, "TenDN");
            txtMatKhau.Text = Function.LayGiaTriCell(row, "MatKhau");

            string gt = Function.LayGiaTriCell(row, "GioiTinh", "Nam");
            radNam.Checked = (gt == "Nam");
            radNu.Checked  = (gt == "Nữ");

            string cv = Function.LayGiaTriCell(row, "ChucVu", "Nhân viên");
            cboChucVu.SelectedItem = cv;

            string ns = Function.LayGiaTriCell(row, "NgaySinh");
            if (!string.IsNullOrEmpty(ns) && DateTime.TryParse(ns, out DateTime d))
                dtpNgaySinh.Value = d;
            else
                dtpNgaySinh.Value = DateTime.Today.AddYears(-25);
        }

        // tao ma nhan vien tu dong tang theo dinh dang NVxx
        private string TaoMaNVTuDong()
        {
            return Function.SinhMaTuDong("NHAN_VIEN", "MaNV", "NV", 2);
        }
        private void ClearForm()
        {
            txtMaNV.Text    = "";
            txtTenNV.Text   = "";
            txtSDT.Text     = "";
            txtDiaChi.Text  = "";
            txtTenDN.Text   = "";
            txtMatKhau.Text = "";
            radNam.Checked  = true;
            cboChucVu.SelectedIndex = 1;
            dtpNgaySinh.Value = DateTime.Today.AddYears(-25);
        }

        // ─── Events ──────────────────────────────────────────────────────────
        private void btnTimKiem_Click(object sender, EventArgs e)
            => LoadDanhSachNhanVien(txtTimKiem.Text.Trim());
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            LoadDanhSachNhanVien();
            _selectedMaNV = null;
            ClearForm();
            dgvNhanVien.ClearSelection();
            SetFormMode(FormMode.Normal);
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            _selectedMaNV = null;
            ClearForm();
            txtMaNV.Text = TaoMaNVTuDong();
            dgvNhanVien.ClearSelection();
            SetFormMode(FormMode.Adding);
            txtTenNV.Focus();
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedMaNV == null)
            {
                Function.ThongBaoCanhBao("Vui lòng chọn nhân viên trong bảng cần sửa.", "Thông báo");
                return;
            }
            SetFormMode(FormMode.Editing);
            txtTenNV.Focus();
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (_mode != FormMode.Adding && _mode != FormMode.Editing) return;
            try
            {
                string id = new EmployeeService().Save(new Employee
                {
                    Id = _mode == FormMode.Adding ? txtMaNV.Text.Trim() : _selectedMaNV,
                    Name = txtTenNV.Text.Trim(), Gender = radNam.Checked ? "Nam" : "Nữ",
                    BirthDate = dtpNgaySinh.Value.Date, Phone = txtSDT.Text.Trim(),
                    Address = txtDiaChi.Text.Trim(), Role = Convert.ToString(cboChucVu.SelectedItem),
                    Username = txtTenDN.Text.Trim(), Password = txtMatKhau.Text
                }, _mode == FormMode.Adding);
                Function.ThongBaoThanhCong("Đã lưu nhân viên " + id + ".");
                LoadDanhSachNhanVien(txtTimKiem.Text.Trim());
                _selectedMaNV = null;
                ClearForm();
                dgvNhanVien.ClearSelection();
                SetFormMode(FormMode.Normal);
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedMaNV == null)
            {
                Function.ThongBaoCanhBao("Vui lòng chọn nhân viên trong bảng cần xóa.", "Thông báo");
                return;
            }
            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn XÓA nhân viên \"{txtTenNV.Text}\" ({_selectedMaNV})?\n" +
                "Nhân viên đã có hóa đơn / phiếu nhập sẽ không xóa được.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;
            try
            {
                new EmployeeService().Delete(_selectedMaNV);
                Function.ThongBaoThanhCong("Xóa nhân viên thành công!", "Thành công");
                LoadDanhSachNhanVien(txtTimKiem.Text.Trim());
                _selectedMaNV = null;
                ClearForm();
                dgvNhanVien.ClearSelection();
                SetFormMode(FormMode.Normal);
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }
        private void btnHuy_Click(object sender, EventArgs e)
        {
            _selectedMaNV = null;
            ClearForm();
            dgvNhanVien.ClearSelection();
            SetFormMode(FormMode.Normal);
        }

        // Xử lý sự kiện btnThoat_Click của giao diện: Điều hướng về Trang Chủ
        private void btnThoat_Click(object sender, EventArgs e)
        {
            NavigationManager.Navigate(NavigationMenu.TrangChu);
        }
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
            _selectedMaNV = row.Cells["MaNV"]?.Value?.ToString();
            FillFormFromGrid(row);
            SetFormMode(FormMode.Selected);
        }
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoadDanhSachNhanVien(txtTimKiem.Text.Trim());
        }
    }
}

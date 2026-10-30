using System;
using System.Data;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.Services;
using QuanLyBanLaptop.Models;
namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmNhanVien : AppForm
    {
        protected override bool RequiresAdmin => true;
        private enum FormMode { Normal, Adding, Selected, Editing }
        private FormMode _mode = FormMode.Normal;
        private string _selectedMaNV;
        public frmNhanVien()
        {
            InitializeComponent();
            NavigationManager.RegisterForm(NavigationMenu.NhanVien, this);
            sidebarControl1.SelectedMenu = NavigationMenu.NhanVien;
            cboChucVu.Items.Clear();
            cboChucVu.Items.AddRange(new object[] { "Quản trị viên", "Nhân viên" });
            cboChucVu.SelectedIndex = 1;
            SetFormMode(FormMode.Normal);
        }
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!Visible) return;
            sidebarControl1.SelectedMenu = NavigationMenu.NhanVien;
            LoadDanhSachNhanVien();
            ResetSelection();
        }
        private void ResetSelection()
        {
            _selectedMaNV = null;
            ClearForm();
            dgvNhanVien.ClearSelection();
            SetFormMode(FormMode.Normal);
        }
        private void LoadDanhSachNhanVien(string tuKhoa = null)
        {
            try
            {
                dgvNhanVien.DataSource = new EmployeeService().Search(tuKhoa);
                SetGridHeaders();
                dgvNhanVien.ClearSelection();
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }
        private void SetGridHeaders()
        {
            if (dgvNhanVien.Columns.Count == 0) return;
            Function.DatTieuDeCot(dgvNhanVien, "MaNV", "Mã NV", 95);
            Function.DatTieuDeCot(dgvNhanVien, "TenNV", "Họ tên", 180);
            Function.DatTieuDeCot(dgvNhanVien, "GioiTinh", "Giới tính", 85);
            Function.DatTieuDeCot(dgvNhanVien, "NgaySinh", "Ngày sinh", 110);
            Function.DatTieuDeCot(dgvNhanVien, "SDT", "SĐT", 120);
            Function.DatTieuDeCot(dgvNhanVien, "DiaChi", "Địa chỉ", 220);
            Function.DatTieuDeCot(dgvNhanVien, "ChucVu", "Chức vụ", 120);
            Function.DatTieuDeCot(dgvNhanVien, "TenDN", "Tên đăng nhập", 130);
            Function.DatTieuDeCot(dgvNhanVien, "MatKhau", "Mật khẩu", 120);
            Function.DatTieuDeCot(dgvNhanVien, "TrangThai", "Trạng thái", 125);
            dgvNhanVien.AllowUserToResizeColumns = true;
            dgvNhanVien.ScrollBars = ScrollBars.Both;
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        }
        private void SetFormMode(FormMode mode)
        {
            _mode = mode;
            bool ed = mode == FormMode.Adding || mode == FormMode.Editing;
            txtMaNV.ReadOnly = true;
            txtTenNV.ReadOnly = txtSDT.ReadOnly = txtDiaChi.ReadOnly = txtTenDN.ReadOnly = txtMatKhau.ReadOnly = !ed;
            radNam.Enabled = radNu.Enabled = dtpNgaySinh.Enabled = cboChucVu.Enabled = ed;
            txtTimKiem.Enabled = btnTimKiem.Enabled = btnLamMoi.Enabled = !ed;
            ButtonTheme.SetButtonState(btnThem, mode == FormMode.Normal);
            ButtonTheme.SetButtonState(btnSua, mode == FormMode.Selected);
            ButtonTheme.SetButtonState(btnXoa, mode == FormMode.Selected);
            ButtonTheme.SetButtonState(btnLuu, ed);
            ButtonTheme.SetButtonState(btnHuy, mode != FormMode.Normal);
            ButtonTheme.SetButtonState(btnThoat, true);
        }
        private void FillFormFromGrid(DataGridViewRow row)
        {
            if (row == null) return;
            txtMaNV.Text = Function.LayGiaTriCell(row, "MaNV");
            txtTenNV.Text = Function.LayGiaTriCell(row, "TenNV");
            txtSDT.Text = Function.LayGiaTriCell(row, "SDT");
            txtDiaChi.Text = Function.LayGiaTriCell(row, "DiaChi");
            txtTenDN.Text = Function.LayGiaTriCell(row, "TenDN");
            txtMatKhau.Text = Function.LayGiaTriCell(row, "MatKhau");
            string gt = Function.LayGiaTriCell(row, "GioiTinh", "Nam");
            radNam.Checked = gt == "Nam";
            radNu.Checked = gt == "Nữ";
            cboChucVu.SelectedItem = Function.LayGiaTriCell(row, "ChucVu", "Nhân viên");
            string ns = Function.LayGiaTriCell(row, "NgaySinh");
            dtpNgaySinh.Value = !string.IsNullOrEmpty(ns) && DateTime.TryParse(ns, out DateTime d) ? d : DateTime.Today.AddYears(-25);
        }
        private void ClearForm()
        {
            txtMaNV.Text = txtTenNV.Text = txtSDT.Text = txtDiaChi.Text = txtTenDN.Text = txtMatKhau.Text = "";
            radNam.Checked = true;
            cboChucVu.SelectedIndex = 1;
            dtpNgaySinh.Value = DateTime.Today.AddYears(-25);
        }
        private void btnTimKiem_Click(object sender, EventArgs e) => LoadDanhSachNhanVien(txtTimKiem.Text.Trim());
        private void btnLamMoi_Click(object sender, EventArgs e) { txtTimKiem.Clear(); LoadDanhSachNhanVien(); ResetSelection(); }
        private void btnThem_Click(object sender, EventArgs e)
        {
            _selectedMaNV = null;
            ClearForm();
            txtMaNV.Text = Function.SinhMaTuDong("NHAN_VIEN", "MaNV", "NV", 2);
            dgvNhanVien.ClearSelection();
            SetFormMode(FormMode.Adding);
            txtTenNV.Focus();
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedMaNV == null) { Function.ThongBaoCanhBao("Vui lòng chọn nhân viên trong bảng cần sửa.", "Thông báo"); return; }
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
                ResetSelection();
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedMaNV == null) { Function.ThongBaoCanhBao("Vui lòng chọn nhân viên trong bảng cần xóa.", "Thông báo"); return; }
            if (MessageBox.Show($"Bạn có chắc muốn XÓA nhân viên \"{txtTenNV.Text}\" ({_selectedMaNV})?\nNhân viên đã có hóa đơn / phiếu nhập sẽ không xóa được.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                new EmployeeService().Delete(_selectedMaNV);
                Function.ThongBaoThanhCong("Xóa nhân viên thành công!", "Thành công");
                LoadDanhSachNhanVien(txtTimKiem.Text.Trim());
                ResetSelection();
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }
        private void btnHuy_Click(object sender, EventArgs e) => ResetSelection();
        private void btnThoat_Click(object sender, EventArgs e) => NavigationManager.Navigate(NavigationMenu.TrangChu);
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
            _selectedMaNV = row.Cells["MaNV"]?.Value?.ToString();
            FillFormFromGrid(row);
            SetFormMode(FormMode.Selected);
        }
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) LoadDanhSachNhanVien(txtTimKiem.Text.Trim()); }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.Services;
using QuanLyBanLaptop.Models;
namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmKhachHang : AppForm
    {
        private enum FormMode { Normal, Adding, Selected, Editing }
        private FormMode _mode = FormMode.Normal;
        private string _selectedMaKH;
        public frmKhachHang()
        {
            InitializeComponent();
            NavigationManager.RegisterForm(NavigationMenu.KhachHang, this);
            sidebarControl1.SelectedMenu = NavigationMenu.KhachHang;
            cboGioiTinh.Items.Clear();
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ" });
            cboGioiTinh.SelectedIndex = 0;
            cboHangThanhVien.Items.Clear();
            cboHangThanhVien.Items.AddRange(new object[] { "Đồng", "Bạc", "Vàng", "VIP" });
            cboHangThanhVien.SelectedIndex = 0;
            SetFormMode(FormMode.Normal);
        }
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!Visible) return;
            sidebarControl1.SelectedMenu = NavigationMenu.KhachHang;
            LoadDanhSachKhachHang();
            ResetSelection();
        }
        private void ResetSelection()
        {
            _selectedMaKH = null;
            ClearForm();
            dgvKhachHang.ClearSelection();
            SetFormMode(FormMode.Normal);
        }
        private void LoadDanhSachKhachHang(string tuKhoa = null)
        {
            try
            {
                DataTable dt = new CustomerService().Search(tuKhoa);
                if (dt != null && dt.Rows.Count > 0)
                    dt = dt.AsEnumerable().OrderBy(r => GetMaKHSortKey(r.Field<string>("MaKH"))).CopyToDataTable();
                dgvKhachHang.DataSource = dt;
                SetGridHeaders();
                dgvKhachHang.ClearSelection();
            }
            catch (SqlException ex) { DataProcesser.ShowSqlError(ex); }
        }
        private void SetGridHeaders()
        {
            if (dgvKhachHang.Columns.Count == 0) return;
            Function.DatTieuDeCot(dgvKhachHang, "MaKH", "Mã khách hàng", 115);
            Function.DatTieuDeCot(dgvKhachHang, "TenKH", "Họ tên", 180);
            Function.DatTieuDeCot(dgvKhachHang, "GioiTinh", "Giới tính", 85);
            Function.DatTieuDeCot(dgvKhachHang, "HangThanhVien", "Hạng thành viên", 130);
            Function.DatTieuDeCot(dgvKhachHang, "NgaySinh", "Ngày sinh", 110);
            Function.DatTieuDeCot(dgvKhachHang, "Sdt", "Số điện thoại", 120);
            Function.DatTieuDeCot(dgvKhachHang, "DiaChi", "Địa chỉ", 220);
            Function.DatTieuDeCot(dgvKhachHang, "TrangThai", "Trạng thái", 115);
            dgvKhachHang.AllowUserToResizeColumns = true;
            dgvKhachHang.ScrollBars = ScrollBars.Both;
            dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        }
        private void SetFormMode(FormMode mode)
        {
            _mode = mode;
            bool ed = mode == FormMode.Adding || mode == FormMode.Editing;
            txtMaKH.ReadOnly = true;
            txtHoTen.ReadOnly = txtSDT.ReadOnly = txtDiaChi.ReadOnly = !ed;
            cboGioiTinh.Enabled = dtpNgaySinh.Enabled = ed;
            cboHangThanhVien.Enabled = false;
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
            txtMaKH.Text = Function.LayGiaTriCell(row, "MaKH");
            txtHoTen.Text = Function.LayGiaTriCell(row, "TenKH");
            txtSDT.Text = Function.LayGiaTriCell(row, "Sdt");
            txtDiaChi.Text = Function.LayGiaTriCell(row, "DiaChi");
            cboGioiTinh.SelectedItem = Function.LayGiaTriCell(row, "GioiTinh", "Nam");
            cboHangThanhVien.SelectedItem = Function.LayGiaTriCell(row, "HangThanhVien", "Đồng");
            string ns = Function.LayGiaTriCell(row, "NgaySinh");
            dtpNgaySinh.Value = !string.IsNullOrEmpty(ns) &&
                (DateTime.TryParseExact(ns, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d) || DateTime.TryParse(ns, out d))
                ? d : DateTime.Today.AddYears(-20);
        }
        private void ClearForm()
        {
            txtMaKH.Text = txtHoTen.Text = txtSDT.Text = txtDiaChi.Text = "";
            cboGioiTinh.SelectedIndex = cboHangThanhVien.SelectedIndex = 0;
            dtpNgaySinh.Value = DateTime.Today.AddYears(-20);
        }
        private void btnTimKiem_Click(object sender, EventArgs e) => LoadDanhSachKhachHang(txtTimKiem.Text.Trim());
        private void btnLamMoi_Click(object sender, EventArgs e) { txtTimKiem.Clear(); LoadDanhSachKhachHang(); ResetSelection(); }
        private void btnThem_Click(object sender, EventArgs e)
        {
            _selectedMaKH = null;
            ClearForm();
            txtMaKH.Text = Function.SinhMaTuDong("KHACH_HANG", "MaKH", "KH", 2);
            dgvKhachHang.ClearSelection();
            SetFormMode(FormMode.Adding);
            txtHoTen.Focus();
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedMaKH == null) { Function.ThongBaoCanhBao("Vui lòng chọn khách hàng trong bảng cần sửa.", "Thông báo"); return; }
            SetFormMode(FormMode.Editing);
            txtHoTen.Focus();
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if ((_mode != FormMode.Adding && _mode != FormMode.Editing) || !ValidateInput()) return;
            try
            {
                new CustomerService().Save(new Customer
                {
                    Id = _mode == FormMode.Adding ? txtMaKH.Text.Trim().ToUpper() : _selectedMaKH,
                    Name = txtHoTen.Text.Trim(), Gender = Convert.ToString(cboGioiTinh.SelectedItem),
                    BirthDate = dtpNgaySinh.Value.Date, Phone = txtSDT.Text.Trim(),
                    Address = txtDiaChi.Text.Trim(), Membership = Convert.ToString(cboHangThanhVien.SelectedItem)
                }, _mode == FormMode.Adding);
                Function.ThongBaoThanhCong("Đã lưu khách hàng thành công.");
                LoadDanhSachKhachHang(txtTimKiem.Text.Trim());
                ResetSelection();
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedMaKH == null) { Function.ThongBaoCanhBao("Vui lòng chọn khách hàng cần xóa.", "Thông báo"); return; }
            if (MessageBox.Show($"Bạn có chắc muốn XÓA khách hàng \"{txtHoTen.Text}\" ({_selectedMaKH})?\n\nLưu ý: Khách hàng đã có hóa đơn sẽ không xóa được.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                new CustomerService().Delete(_selectedMaKH);
                Function.ThongBaoThanhCong("Xóa khách hàng thành công!", "Thành công");
                LoadDanhSachKhachHang(txtTimKiem.Text.Trim());
                ResetSelection();
            }
            catch (SqlException ex) { DataProcesser.ShowSqlError(ex); }
        }
        private void btnHuy_Click(object sender, EventArgs e) => ResetSelection();
        private void btnThoat_Click(object sender, EventArgs e) => NavigationManager.Navigate(NavigationMenu.TrangChu);
        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];
            _selectedMaKH = row.Cells["MaKH"]?.Value?.ToString();
            FillFormFromGrid(row);
            SetFormMode(FormMode.Selected);
        }
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) LoadDanhSachKhachHang(txtTimKiem.Text.Trim()); }
        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e) { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; }
        private static int GetMaKHSortKey(string maKH) =>
            !string.IsNullOrEmpty(maKH) && Regex.Match(maKH, @"\d+") is Match m && m.Success && int.TryParse(m.Value, out int n) ? n : int.MaxValue;
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text)) { Function.ThongBaoCanhBao("Vui lòng nhập mã khách hàng.", "Cảnh báo"); txtMaKH.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(txtHoTen.Text)) { Function.ThongBaoCanhBao("Vui lòng nhập họ tên khách hàng.", "Cảnh báo"); txtHoTen.Focus(); return false; }
            if (!string.IsNullOrWhiteSpace(txtSDT.Text) && !Regex.IsMatch(txtSDT.Text.Trim(), @"^[0-9]+$"))
            {
                Function.ThongBaoCanhBao("Số điện thoại chỉ được chứa các chữ số.", "Cảnh báo");
                txtSDT.Focus();
                return false;
            }
            return true;
        }
    }
}

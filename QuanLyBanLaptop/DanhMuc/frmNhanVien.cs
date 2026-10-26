using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmNhanVien : QuanLyBanLaptop.Classes.AppForm
    {
        private enum FormMode { View, Add, Edit }
        private FormMode _mode = FormMode.View;
        private string _selectedMaNV = null;

        // Khởi tạo giao diện và đăng ký sự kiện cho form.
        public frmNhanVien()
        {
            InitializeComponent();
            NavigationManager.RegisterForm(NavigationMenu.NhanVien, this);
            sidebarControl1.SelectedMenu = NavigationMenu.NhanVien;

            // Chức vụ
            cboChucVu.Items.Clear();
            cboChucVu.Items.AddRange(new object[] { "Quản trị viên", "Nhân viên" });
            cboChucVu.SelectedIndex = 1;

            SetFormMode(FormMode.View);
        }

        // Thực hiện OnVisibleChanged theo trạng thái và dữ liệu hiện tại.
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.NhanVien;
                LoadDanhSachNhanVien();
            }
        }

        // ─── Tải danh sách ──────────────────────────────────────────────────
        private void LoadDanhSachNhanVien(string tuKhoa = null)
        {
            try
            {
                DataTable dt = DataProcesser.ExecProc("sp_TimKiemNhanVien", new[]
                {
                    new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 100)
                    { Value = string.IsNullOrEmpty(tuKhoa) ? (object)DBNull.Value : tuKhoa }
                });
                dgvNhanVien.DataSource = dt;
                SetGridHeaders();
            }
            catch (SqlException ex) { DataProcesser.ShowSqlError(ex); }
        }

        // doi tieu de cot datagridview
        private void SetGridHeaders()
        {
            if (dgvNhanVien.Columns.Count == 0) return;
            Function.DatTieuDeCot(dgvNhanVien, "MaNV",      "Mã NV");
            Function.DatTieuDeCot(dgvNhanVien, "TenNV",     "Họ tên");
            Function.DatTieuDeCot(dgvNhanVien, "GioiTinh",  "Giới tính");
            Function.DatTieuDeCot(dgvNhanVien, "NgaySinh",  "Ngày sinh");
            Function.DatTieuDeCot(dgvNhanVien, "SDT",       "SĐT");
            Function.DatTieuDeCot(dgvNhanVien, "DiaChi",    "Địa chỉ");
            Function.DatTieuDeCot(dgvNhanVien, "ChucVu",    "Chức vụ");
            Function.DatTieuDeCot(dgvNhanVien, "TenDN",     "Tên đăng nhập");
            Function.DatTieuDeCot(dgvNhanVien, "TrangThai", "Trạng thái");
        }

        // thiet lap che do form nhan vien
        private void SetFormMode(FormMode mode)
        {
            _mode = mode;
            bool editing = (mode == FormMode.Add || mode == FormMode.Edit);
            bool hasSelected = (_selectedMaNV != null);

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

            btnThem.Enabled  = !editing;
            btnSua.Enabled   = !editing && hasSelected;
            btnXoa.Enabled   = !editing && hasSelected;
            btnLuu.Enabled   = editing;
            btnHuy.Enabled   = editing;
            btnThoat.Enabled = true;
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
            txtMatKhau.Clear();

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

        // Thực hiện ClearForm theo trạng thái và dữ liệu hiện tại.
        private void ClearForm()
        {
            txtMaNV.Text    = TaoMaNVTuDong();
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

        // Xử lý sự kiện btnLamMoi_Click của giao diện.
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            LoadDanhSachNhanVien();
            _selectedMaNV = null;
            ClearForm();
            SetFormMode(FormMode.View);
        }

        // Xử lý sự kiện btnThem_Click của giao diện.
        private void btnThem_Click(object sender, EventArgs e)
        {
            _selectedMaNV = null;
            ClearForm();
            SetFormMode(FormMode.Add);
            txtTenNV.Focus();
        }

        // Xử lý sự kiện btnSua_Click của giao diện.
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedMaNV == null)
            {
                Function.ThongBaoThanhCong("Vui lòng chọn nhân viên cần sửa.", "Thông báo");
                return;
            }
            SetFormMode(FormMode.Edit);
            txtTenNV.Focus();
        }

        // Xử lý sự kiện btnLuu_Click của giao diện.
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenNV.Text))
            {
                Function.ThongBaoCanhBao("Vui lòng nhập họ tên nhân viên.", "Cảnh báo");
                txtTenNV.Focus();
                return;
            }

            try
            {
                string gioiTinh = radNam.Checked ? "Nam" : "Nữ";

                if (_mode == FormMode.Add)
                {
                    if (string.IsNullOrWhiteSpace(txtTenDN.Text))
                    {
                        Function.ThongBaoCanhBao("Vui lòng nhập tên đăng nhập.", "Cảnh báo");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                    {
                        Function.ThongBaoCanhBao("Vui lòng nhập mật khẩu (ít nhất 6 ký tự).", "Cảnh báo");
                        return;
                    }

                    var maNVParam = new SqlParameter("@MaNV", SqlDbType.VarChar, 10)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = string.IsNullOrWhiteSpace(txtMaNV.Text) ? (object)DBNull.Value : txtMaNV.Text.Trim()
                    };

                    using (var conn = DataProcesser.GetConnection())
                    using (var cmd = new SqlCommand("sp_ThemNhanVien", conn)
                    { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(maNVParam);
                        cmd.Parameters.AddWithValue("@TenNV",    txtTenNV.Text.Trim());
                        cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                        cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value.Date);
                        cmd.Parameters.AddWithValue("@SDT",      txtSDT.Text.Trim());
                        cmd.Parameters.AddWithValue("@DiaChi",   txtDiaChi.Text.Trim());
                        cmd.Parameters.AddWithValue("@ChucVu",   cboChucVu.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@TenDN",    txtTenDN.Text.Trim());
                        cmd.Parameters.AddWithValue("@MatKhau",  txtMatKhau.Text);
                        cmd.ExecuteNonQuery();
                    }

                    string newMa = maNVParam.Value?.ToString();
                    if (string.IsNullOrEmpty(newMa)) newMa = txtMaNV.Text;
                    Function.ThongBaoThanhCong($"Thêm nhân viên thành công! Mã: {newMa}", "Thành công");
                }
                else if (_mode == FormMode.Edit)
                {
                    DataProcesser.ExecProcNonQuery("sp_CapNhatNhanVien", new[]
                    {
                        new SqlParameter("@MaNV",     _selectedMaNV),
                        new SqlParameter("@TenNV",    txtTenNV.Text.Trim()),
                        new SqlParameter("@GioiTinh", gioiTinh),
                        new SqlParameter("@NgaySinh", dtpNgaySinh.Value.Date),
                        new SqlParameter("@SDT",      txtSDT.Text.Trim()),
                        new SqlParameter("@DiaChi",   txtDiaChi.Text.Trim()),
                        new SqlParameter("@ChucVu",   cboChucVu.SelectedItem.ToString()),
                        new SqlParameter("@TenDN",    txtTenDN.Text.Trim()),
                        new SqlParameter("@MatKhau",  string.IsNullOrWhiteSpace(txtMatKhau.Text) ? (object)DBNull.Value : txtMatKhau.Text.Trim()),
                    });
                    Function.ThongBaoThanhCong("Cập nhật nhân viên thành công!", "Thành công");
                }

                LoadDanhSachNhanVien(txtTimKiem.Text.Trim());
                _selectedMaNV = null;
                ClearForm();
                SetFormMode(FormMode.View);
            }
            catch (SqlException ex) { DataProcesser.ShowSqlError(ex); }
        }

        // Xử lý sự kiện btnXoa_Click của giao diện.
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedMaNV == null)
            {
                Function.ThongBaoThanhCong("Vui lòng chọn nhân viên cần xóa.", "Thông báo");
                return;
            }
            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn XÓA nhân viên \"{txtTenNV.Text}\" ({_selectedMaNV})?\n" +
                "Nhân viên đã có hóa đơn / phiếu nhập sẽ không xóa được.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;
            try
            {
                DataProcesser.ExecProcNonQuery("sp_XoaNhanVien", new[]
                { new SqlParameter("@MaNV", _selectedMaNV) });
                Function.ThongBaoThanhCong("Xóa nhân viên thành công!", "Thành công");
                LoadDanhSachNhanVien(txtTimKiem.Text.Trim());
                _selectedMaNV = null;
                ClearForm();
                SetFormMode(FormMode.View);
            }
            catch (SqlException ex) { DataProcesser.ShowSqlError(ex); }
        }

        // Xử lý sự kiện btnHuy_Click của giao diện.
        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (_selectedMaNV != null && dgvNhanVien.CurrentRow != null)
            {
                FillFormFromGrid(dgvNhanVien.CurrentRow);
            }
            else
            {
                _selectedMaNV = null;
                ClearForm();
            }
            SetFormMode(FormMode.View);
        }

        // Xử lý sự kiện btnThoat_Click của giao diện: Điều hướng về Trang Chủ
        private void btnThoat_Click(object sender, EventArgs e)
        {
            NavigationManager.Navigate(NavigationMenu.TrangChu);
        }

        // Xử lý sự kiện dgvNhanVien_CellClick của giao diện.
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
            _selectedMaNV = row.Cells["MaNV"]?.Value?.ToString();
            FillFormFromGrid(row);
            if (_mode == FormMode.View)
                SetFormMode(FormMode.View);
        }

        // Xử lý sự kiện txtTimKiem_KeyDown của giao diện.
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoadDanhSachNhanVien(txtTimKiem.Text.Trim());
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmSanPham : QuanLyBanLaptop.Classes.AppForm
    {
        // Biến toàn cục static nhận UserName từ frmDangNhap theo yêu cầu đề bài
        public static string userName = "";

        private enum ActiveTab
        {
            TatCaMay,
            LoaiMay,
            NhaCungCap
        }

        private ActiveTab _currentTab = ActiveTab.TatCaMay;
        private readonly Color _colorNavy = Color.FromArgb(27, 42, 74);
        private readonly Color _colorWhite = Color.White;

        // Khởi tạo giao diện và đăng ký sự kiện cho form.
        public frmSanPham()
        {
            InitializeComponent();

            NavigationManager.RegisterForm(NavigationMenu.QuanLyLaptop, this);
            sidebarControl1.SelectedMenu = NavigationMenu.QuanLyLaptop;

            dgvLaptop.AutoGenerateColumns = false;
            dgvLoaiMay.AutoGenerateColumns = false;
            dgvNCC.AutoGenerateColumns = false;

            this.Load += FrmSanPham_Load;

            SetupTabEvents();
            SetupLaptopEvents();
            SetupLoaiMayEvents();
            SetupNCCEvents();
            SetFormMode(FormMode.Normal);
        }

        // Xử lý sự kiện FrmSanPham_Load của giao diện.
        private void FrmSanPham_Load(object sender, EventArgs e)
        {


            // Kiểm tra trạng thái đăng nhập: Nếu chưa đăng nhập thì hiển thị frmDangNhap dạng hộp thoại ShowDialog()
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(UserSession.TenDN))
            {
                using (frmDangNhap frmDN = new frmDangNhap())
                {
                    frmDN.ShowDialog();
                }
            }

            // Tải dữ liệu ban đầu
            LoadComboboxesLaptop();
            LoadDataLaptop();
            LoadDataLoaiMay();
            LoadDataNCC();
            LamMoiLaptopInputs();
            dgvLaptop.ClearSelection();
            SetFormMode(FormMode.Normal);

            SwitchTab(ActiveTab.TatCaMay);
        }

        // Thực hiện OnVisibleChanged theo trạng thái và dữ liệu hiện tại.
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.QuanLyLaptop;
                LoadComboboxesLaptop();
                LoadDataLaptop();
                LamMoiLaptopInputs();
                dgvLaptop.ClearSelection();
                SetFormMode(FormMode.Normal);
            }
        }



        #region TAB SWITCHING

        // Khởi tạo cấu hình và liên kết cho SetupTabEvents.
        private void SetupTabEvents()
        {
            btnTabTatCaMay.Click += (s, e) => SwitchTab(ActiveTab.TatCaMay);
            btnTabLoaiMay.Click += (s, e) => SwitchTab(ActiveTab.LoaiMay);
            btnTabNhaCungCap.Click += (s, e) => SwitchTab(ActiveTab.NhaCungCap);
        }

        // Thực hiện SwitchTab theo trạng thái và dữ liệu hiện tại.
        private void SwitchTab(ActiveTab tab)
        {
            _currentTab = tab;

            pnlTabTatCaMay.Visible = (tab == ActiveTab.TatCaMay);
            pnlTabLoaiMay.Visible = (tab == ActiveTab.LoaiMay);
            pnlTabNhaCungCap.Visible = (tab == ActiveTab.NhaCungCap);

            SetTabButtonStyle(btnTabTatCaMay, tab == ActiveTab.TatCaMay);
            SetTabButtonStyle(btnTabLoaiMay, tab == ActiveTab.LoaiMay);
            SetTabButtonStyle(btnTabNhaCungCap, tab == ActiveTab.NhaCungCap);

            if (tab == ActiveTab.TatCaMay)
            {
                LoadComboboxesLaptop();
                LoadDataLaptop();
                LamMoiLaptopInputs();
                dgvLaptop.ClearSelection();
                SetFormMode(FormMode.Normal);
            }
            else if (tab == ActiveTab.LoaiMay)
            {
                LoadDataLoaiMay();
            }
            else if (tab == ActiveTab.NhaCungCap)
            {
                LoadDataNCC();
            }
        }

        // Đánh dấu tab hiện tại bằng viền, giữ cùng nền và phông chữ.
        private void SetTabButtonStyle(Button btn, bool isActive)
        {
            ButtonTheme.Apply(btn);
            btn.FlatAppearance.BorderColor = isActive ? _colorNavy : Color.FromArgb(206, 212, 218);
        }

        #endregion

        #region TAB 1: TẤT CẢ CÁC MÁY (LAPTOP / SẢN PHẨM)

        private enum FormMode
        {
            Normal,
            Adding,
            Selected,
            Editing
        }

        private FormMode _mode = FormMode.Normal;

        // Khởi tạo cấu hình và liên kết cho SetupLaptopEvents.
        private void SetupLaptopEvents()
        {
            dgvLaptop.CellClick += DgvLaptop_CellClick;
            dgvLaptop.CellFormatting += DgvLaptop_CellFormatting;

            // Ràng buộc nhập số nguyên cho Số Lượng, Đơn giá Nhập, Đơn giá Bán theo yêu cầu đề bài
            txtSoLuong.KeyPress += TxtChiNhapSoNguyen_KeyPress;
            txtDonGiaNhap.KeyPress += TxtChiNhapSoNguyen_KeyPress;
            txtDonGiaBan.KeyPress += TxtChiNhapSoNguyen_KeyPress;

            // Tự động tìm kiếm tức thì khi gõ từ khóa
            txtTimKiem.TextChanged += (s, e) => LoadDataLaptop();
            btnTimKiem.Click += (s, e) => LoadDataLaptop();
            btnLamMoiLoc.Click += (s, e) =>
            {
                txtTimKiem.Clear();
                LoadDataLaptop();
                LamMoiLaptopInputs();
                dgvLaptop.ClearSelection();
                SetFormMode(FormMode.Normal);
                txtTimKiem.Focus();
            };

            // Các nút chức năng
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            btnHuy.Click += BtnHuy_Click;
            btnThoat.Click += BtnThoat_Click;
        }

        // Thiết lập trạng thái chế độ thao tác form Laptop theo ButtonTheme
        private void SetFormMode(FormMode mode)
        {
            _mode = mode;
            bool isEditing = (mode == FormMode.Adding || mode == FormMode.Editing);

            txtMaLaptop.ReadOnly = true;
            txtTenLaptop.ReadOnly = !isEditing;
            cboLoaiLaptop.Enabled = isEditing;
            cboNhaCungCap.Enabled = isEditing;
            txtCPU.ReadOnly = !isEditing;
            cboRAM.Enabled = isEditing;
            cboSSD.Enabled = isEditing;
            txtSoLuong.ReadOnly = (mode != FormMode.Adding);
            txtDonGiaNhap.ReadOnly = !isEditing;
            txtDonGiaBan.ReadOnly = !isEditing;
            txtGhiChu.ReadOnly = !isEditing;
            numBaoHanh.Enabled = isEditing;

            txtTimKiem.Enabled = !isEditing;
            btnTimKiem.Enabled = !isEditing;
            btnLamMoiLoc.Enabled = !isEditing;

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

        // Xử lý sự kiện TxtChiNhapSoNguyen_KeyPress của giao diện.
        private void TxtChiNhapSoNguyen_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Các TextBox Số Lượng, Đơn giá Nhập, Đơn giá bán chỉ cho phép người dùng nhập số nguyên
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Tải dữ liệu phục vụ LoadComboboxesLaptop.
        private void LoadComboboxesLaptop()
        {
            // Đổ danh sách loại máy vào form chỉnh sửa
            DataTable dtLoai = DataProcesser.ReadData("SELECT TenLoai FROM LOAI_MAY ORDER BY TenLoai");
            if (dtLoai == null || dtLoai.Rows.Count == 0)
            {
                // Mặc định có sẵn các phân loại
                DataProcesser.ChangeData("INSERT INTO LOAI_MAY (TenLoai, MoTa) VALUES (N'Gaming', N'Máy chơi game hiệu năng cao'), (N'Văn phòng', N'Máy mỏng nhẹ dùng văn phòng'), (N'Đồ họa', N'Máy trạm đồ họa'), (N'Sinh viên', N'Máy học tập giá phổ thông')");
                dtLoai = DataProcesser.ReadData("SELECT TenLoai FROM LOAI_MAY ORDER BY TenLoai");
            }
            cboLoaiLaptop.DataSource = dtLoai;
            cboLoaiLaptop.DisplayMember = "TenLoai";
            cboLoaiLaptop.ValueMember = "TenLoai";

            // Đổ danh sách nhà cung cấp vào form chỉnh sửa
            DataProcesser.FillComboBox(cboNhaCungCap, "SELECT MaNCC, TenNCC FROM NHA_CUNG_CAP ORDER BY TenNCC", "TenNCC", "MaNCC");

            if (cboRAM.Items.Count > 0 && cboRAM.SelectedIndex < 0)
                cboRAM.SelectedIndex = 1; // 8GB

            if (cboSSD.Items.Count > 0 && cboSSD.SelectedIndex < 0)
                cboSSD.SelectedIndex = 1; // 512GB
        }

        // Tải dữ liệu phục vụ LoadDataLaptop.
        private void LoadDataLaptop()
        {
            string sql = @"
                SELECT l.MaLaptop, l.TenLaptop, ISNULL(l.Loai, N'Khác') AS Loai, 
                       n.TenNCC, l.CPU, l.RAM, l.SSD, 
                       ISNULL(l.DonGiaNhap, 0) AS DonGiaNhap,
                       l.DonGia, 
                       l.SoLuongTon, 
                       l.ThoiGianBaoHanh, l.MaNCC, l.NgungKinhDoanh,
                       ISNULL(l.Anh, N'') AS Anh,
                       ISNULL(l.GhiChu, N'') AS GhiChu,
                       CASE WHEN l.NgungKinhDoanh = 1 THEN N'Ngừng KD' ELSE N'Hoạt động' END AS TrangThaiHienThi
                FROM LAPTOP l
                LEFT JOIN NHA_CUNG_CAP n ON l.MaNCC = n.MaNCC
                WHERE (1=1)";

            var parameters = new System.Collections.Generic.List<SqlParameter>();

            // Tìm kiếm toàn diện theo ô từ khóa: tự động lọc theo mã, tên, loại máy, cấu hình, NCC, trạng thái
            string keyword = txtTimKiem.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                sql += @" AND (
                    l.MaLaptop LIKE @kw 
                    OR l.TenLaptop LIKE @kw 
                    OR l.Loai LIKE @kw 
                    OR l.CPU LIKE @kw 
                    OR l.RAM LIKE @kw 
                    OR l.SSD LIKE @kw 
                    OR n.TenNCC LIKE @kw
                    OR (CASE WHEN l.NgungKinhDoanh = 1 THEN N'Ngừng KD' ELSE N'Hoạt động' END) LIKE @kw
                )";
                parameters.Add(new SqlParameter("@kw", "%" + keyword + "%"));
            }

            sql += " ORDER BY l.MaLaptop ASC";

            DataTable dt = DataProcesser.ReadData(sql, parameters.ToArray());
            dgvLaptop.DataSource = dt;
            dgvLaptop.ClearSelection();
        }

        // Xử lý sự kiện DgvLaptop_CellFormatting của giao diện.
        private void DgvLaptop_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            // Format cột Đơn giá bán và Đơn giá nhập
            if (dgvLaptop.Columns[e.ColumnIndex].Name == "colDonGia" || dgvLaptop.Columns[e.ColumnIndex].Name == "colDonGiaNhap")
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal gia))
                {
                    e.Value = string.Format("{0:N0} đ", gia);
                    e.FormattingApplied = true;
                }
            }

            // Format cột Bảo hành
            if (dgvLaptop.Columns[e.ColumnIndex].Name == "colBaoHanh")
            {
                if (int.TryParse(e.Value.ToString(), out int bh))
                {
                    e.Value = bh + " tháng";
                    e.FormattingApplied = true;
                }
            }

            // Đổi màu cột trạng thái
            if (dgvLaptop.Columns[e.ColumnIndex].Name == "colTrangThai")
            {
                string tt = e.Value.ToString();
                if (tt == "Ngừng KD")
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
            }
        }

        // Thực hiện GetCellString theo trạng thái và dữ liệu hiện tại.
        private string GetCellString(DataGridViewRow row, string colName)
        {
            if (row == null) return "";
            if (dgvLaptop.Columns.Contains(colName) && row.Cells[colName] != null)
            {
                return row.Cells[colName].Value?.ToString() ?? "";
            }
            if (row.DataBoundItem is DataRowView drv)
            {
                string dataProp = dgvLaptop.Columns.Contains(colName) ? dgvLaptop.Columns[colName].DataPropertyName : colName;
                if (!string.IsNullOrEmpty(dataProp) && drv.Row.Table.Columns.Contains(dataProp))
                {
                    return drv[dataProp]?.ToString() ?? "";
                }
            }
            return "";
        }

        // Đổ dữ liệu từ một dòng GridView lên các control thông tin laptop
        private void FillLaptopFromRow(DataGridViewRow row)
        {
            if (row == null) return;
            txtMaLaptop.Text = GetCellString(row, "colMaLaptop");
            txtTenLaptop.Text = GetCellString(row, "colTenLaptop");
            cboLoaiLaptop.Text = GetCellString(row, "colLoai");
            cboNhaCungCap.Text = GetCellString(row, "colTenNCC");

            txtCPU.Text = GetCellString(row, "colCPU");
            cboRAM.Text = GetCellString(row, "colRAM");
            cboSSD.Text = GetCellString(row, "colSSD");

            // Hiển thị số lượng, đơn giá nhập, đơn giá bán, ghi chú
            txtSoLuong.Text = GetCellString(row, "colSoLuongTon");
            txtSoLuong.ReadOnly = true;
            txtDonGiaNhap.Text = GetCellString(row, "colDonGiaNhap");
            if (string.IsNullOrEmpty(txtDonGiaNhap.Text)) txtDonGiaNhap.Text = "0";
            txtDonGiaBan.Text = GetCellString(row, "colDonGia");
            txtGhiChu.Text = GetCellString(row, "colGhiChu");

            string bhStr = GetCellString(row, "colBaoHanh");
            if (int.TryParse(bhStr, out int baoHanh))
                numBaoHanh.Value = Math.Min(numBaoHanh.Maximum, Math.Max(numBaoHanh.Minimum, baoHanh));
        }

        // Xử lý sự kiện DgvLaptop_CellClick của giao diện.
        private void DgvLaptop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvLaptop.Rows.Count) return;
            FillLaptopFromRow(dgvLaptop.Rows[e.RowIndex]);
            SetFormMode(FormMode.Selected);
        }

        // Bấm nút Thêm: Chuyển sang Adding mode, xóa trắng các trường, sinh mã mới, focus ô nhập tên
        private void BtnThem_Click(object sender, EventArgs e)
        {
            LamMoiLaptopInputs();
            txtMaLaptop.Text = Function.SinhMaTuDong("LAPTOP", "MaLaptop", "MH", 2);
            dgvLaptop.ClearSelection();
            SetFormMode(FormMode.Adding);
            txtTenLaptop.Focus();
        }

        // Bấm nút Sửa: Chuyển sang Editing mode, mở khóa các trường thông tin cho phép sửa
        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvLaptop.CurrentRow == null || dgvLaptop.SelectedRows.Count == 0)
            {
                Function.ThongBaoCanhBao("Vui lòng chọn sản phẩm trong bảng cần sửa!");
                return;
            }
            SetFormMode(FormMode.Editing);
            txtTenLaptop.Focus();
        }

        // Bấm nút Lưu: Lưu sản phẩm (Adding hoặc Editing)
        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (_mode == FormMode.Adding || _mode == FormMode.Editing)
            {
                SaveProduct(_mode == FormMode.Adding);
            }
        }

        // Bấm nút Hủy: Hủy bỏ thêm/sửa, trở về trạng thái ban đầu
        private void BtnHuy_Click(object sender, EventArgs e)
        {
            LamMoiLaptopInputs();
            dgvLaptop.ClearSelection();
            SetFormMode(FormMode.Normal);
        }

        // Đọc và kiểm tra dữ liệu chung, gọi service rồi tải lại danh sách.
        private void SaveProduct(bool isNew)
        {
            if (string.IsNullOrWhiteSpace(txtTenLaptop.Text))
            {
                Function.ThongBaoCanhBao("Vui lòng nhập tên laptop!");
                txtTenLaptop.Focus();
                return;
            }

            if (!int.TryParse(txtSoLuong.Text, out int quantity) ||
                !decimal.TryParse(txtDonGiaNhap.Text, out decimal purchasePrice) ||
                !decimal.TryParse(txtDonGiaBan.Text, out decimal salePrice))
            {
                Function.ThongBaoCanhBao("Số lượng và đơn giá phải là số hợp lệ.");
                return;
            }
            try
            {
                new Services.ProductService().Save(new Models.Product
                {
                    Id = txtMaLaptop.Text.Trim(), Name = txtTenLaptop.Text.Trim(),
                    Category = cboLoaiLaptop.Text.Trim(),
                    SupplierId = Convert.ToString(cboNhaCungCap.SelectedValue),
                    Cpu = txtCPU.Text.Trim(), Ram = cboRAM.Text, Ssd = cboSSD.Text,
                    PurchasePrice = purchasePrice, SalePrice = salePrice, OpeningQuantity = quantity,
                    WarrantyMonths = (int)numBaoHanh.Value, Notes = txtGhiChu.Text.Trim()
                }, isNew);
                LoadDataLaptop();
                LamMoiLaptopInputs();
                dgvLaptop.ClearSelection();
                SetFormMode(FormMode.Normal);
                Function.ThongBaoThanhCong("Đã lưu sản phẩm thành công!");
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }

        // Xử lý sự kiện BtnXoa_Click của giao diện.
        private void BtnXoa_Click(object sender, EventArgs e)
        {
            string ma = txtMaLaptop.Text.Trim();
            // Kiểm tra xem người dùng đã chọn hàng để xóa chưa
            if (string.IsNullOrEmpty(ma))
            {
                Function.ThongBaoCanhBao("Bạn chưa chọn hàng để xóa!", "Thông báo");
                return;
            }

            // Nếu chọn rồi sẽ hỏi người dùng có muốn xóa không, nếu người dùng chọn Yes sẽ xóa hàng đó khỏi bảng tblHang
            DialogResult dr = MessageBox.Show("Bạn có muốn xóa sản phẩm " + ma + " không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                // Kiểm tra xem máy đã từng có hóa đơn hoặc phiếu nhập chưa
                string checkSql = "SELECT (SELECT COUNT(*) FROM CHI_TIET_HD WHERE MaLaptop = @ma) + (SELECT COUNT(*) FROM CHI_TIET_PN WHERE MaLaptop = @ma)";
                object count = DataProcesser.ExecuteScalar(checkSql, new SqlParameter("@ma", ma));
                if (count != null && Convert.ToInt32(count) > 0)
                {
                    DialogResult drSoft = MessageBox.Show(
                        "Laptop này đã có dữ liệu giao dịch hóa đơn/phiếu nhập nên không thể xóa vật lý.\nBạn có muốn chuyển sang trạng thái 'Ngừng kinh doanh' không?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (drSoft == DialogResult.Yes)
                    {
                        DataProcesser.ChangeData("UPDATE LAPTOP SET NgungKinhDoanh = 1 WHERE MaLaptop = @ma", new SqlParameter("@ma", ma));
                        Function.ThongBaoThanhCong("Đã chuyển trạng thái sang 'Ngừng kinh doanh'!", "Thông báo");
                        LoadDataLaptop();
                        LamMoiLaptopInputs();
                        dgvLaptop.ClearSelection();
                        SetFormMode(FormMode.Normal);
                    }
                    return;
                }

                int rows = DataProcesser.ChangeData("DELETE FROM LAPTOP WHERE MaLaptop = @ma", new SqlParameter("@ma", ma));
                if (rows > 0)
                {
                    Function.ThongBaoThanhCong("Đã xóa sản phẩm thành công!", "Thành công");
                    // Sau đó load lại dữ liệu lên GridView
                    LoadDataLaptop();
                    LamMoiLaptopInputs();
                    dgvLaptop.ClearSelection();
                    SetFormMode(FormMode.Normal);
                }
            }
        }

        // Xử lý sự kiện BtnThoat_Click của giao diện: Điều hướng về Trang Chủ
        private void BtnThoat_Click(object sender, EventArgs e)
        {
            NavigationManager.Navigate(NavigationMenu.TrangChu);
        }

        // làm mới các ô nhập liệu
        private void LamMoiLaptopInputs()
        {
            txtMaLaptop.Text = "";
            txtMaLaptop.ReadOnly = true;
            txtTenLaptop.Clear();
            txtSoLuong.Text = "0";
            txtSoLuong.ReadOnly = true;
            txtDonGiaNhap.Clear();
            txtDonGiaBan.Clear();
            txtGhiChu.Clear();
            txtCPU.Clear();

            if (cboLoaiLaptop.Items.Count > 0) cboLoaiLaptop.SelectedIndex = 0;
            if (cboNhaCungCap.Items.Count > 0) cboNhaCungCap.SelectedIndex = 0;
            if (cboRAM.Items.Count > 0) cboRAM.SelectedIndex = 1;
            if (cboSSD.Items.Count > 0) cboSSD.SelectedIndex = 1;
            numBaoHanh.Value = 12;
        }

        #endregion

        #region TAB 2: ĐIỀU CHỈNH LOẠI MÁY

        // Khởi tạo cấu hình và liên kết cho SetupLoaiMayEvents.
        private void SetupLoaiMayEvents()
        {
            dgvLoaiMay.CellClick += DgvLoaiMay_CellClick;

            btnThemLoai.Click += (s, e) =>
            {
                string ten = txtTenLoaiMay.Text.Trim();
                if (string.IsNullOrEmpty(ten))
                {
                    Function.ThongBaoCanhBao("Vui lòng nhập tên loại máy!", "Thông báo");
                    txtTenLoaiMay.Focus();
                    return;
                }

                object check = DataProcesser.ExecuteScalar("SELECT COUNT(*) FROM LOAI_MAY WHERE TenLoai = @ten", new SqlParameter("@ten", ten));
                if (check != null && Convert.ToInt32(check) > 0)
                {
                    Function.ThongBaoCanhBao("Loại máy này đã tồn tại!", "Cảnh báo");
                    return;
                }

                DataProcesser.ChangeData("INSERT INTO LOAI_MAY (TenLoai, MoTa) VALUES (@ten, @mota)",
                    new SqlParameter("@ten", ten),
                    new SqlParameter("@mota", txtMoTaLoaiMay.Text.Trim()));

                Function.ThongBaoThanhCong("Thêm loại máy thành công!", "Thông báo");
                LoadDataLoaiMay();
                LoadComboboxesLaptop();
            };

            btnSuaLoai.Click += (s, e) =>
            {
                string ten = txtTenLoaiMay.Text.Trim();
                if (string.IsNullOrEmpty(ten))
                {
                    Function.ThongBaoCanhBao("Vui lòng chọn loại máy cần sửa!", "Thông báo");
                    return;
                }

                DataProcesser.ChangeData("UPDATE LOAI_MAY SET MoTa = @mota WHERE TenLoai = @ten",
                    new SqlParameter("@ten", ten),
                    new SqlParameter("@mota", txtMoTaLoaiMay.Text.Trim()));

                Function.ThongBaoThanhCong("Cập nhật loại máy thành công!", "Thông báo");
                LoadDataLoaiMay();
            };

            btnXoaLoai.Click += (s, e) =>
            {
                string ten = txtTenLoaiMay.Text.Trim();
                if (string.IsNullOrEmpty(ten))
                {
                    Function.ThongBaoCanhBao("Vui lòng chọn loại máy cần xóa!", "Thông báo");
                    return;
                }

                object count = DataProcesser.ExecuteScalar("SELECT COUNT(*) FROM LAPTOP WHERE Loai = @ten", new SqlParameter("@ten", ten));
                if (count != null && Convert.ToInt32(count) > 0)
                {
                    Function.ThongBaoCanhBao("Có " + count + " laptop đang thuộc loại máy này, không thể xóa trực tiếp!", "Cảnh báo");
                    return;
                }

                DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa loại máy '" + ten + "'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    DataProcesser.ChangeData("DELETE FROM LOAI_MAY WHERE TenLoai = @ten", new SqlParameter("@ten", ten));
                    Function.ThongBaoThanhCong("Đã xóa loại máy thành công!", "Thông báo");
                    LoadDataLoaiMay();
                    LoadComboboxesLaptop();
                    txtTenLoaiMay.Clear();
                    txtMoTaLoaiMay.Clear();
                }
            };

            btnLamMoiLoai.Click += (s, e) =>
            {
                txtTenLoaiMay.Clear();
                txtMoTaLoaiMay.Clear();
                LoadDataLoaiMay();
            };
        }

        // Tải dữ liệu phục vụ LoadDataLoaiMay.
        private void LoadDataLoaiMay()
        {
            string sql = @"
                SELECT lm.TenLoai, lm.MoTa, COUNT(l.MaLaptop) AS SoLuongMay
                FROM LOAI_MAY lm
                LEFT JOIN LAPTOP l ON lm.TenLoai = l.Loai
                GROUP BY lm.TenLoai, lm.MoTa
                ORDER BY lm.TenLoai ASC";

            DataTable dt = DataProcesser.ReadData(sql);
            dgvLoaiMay.DataSource = dt;
        }

        // Xử lý sự kiện DgvLoaiMay_CellClick của giao diện.
        private void DgvLoaiMay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvLoaiMay.Rows.Count) return;
            DataGridViewRow row = dgvLoaiMay.Rows[e.RowIndex];
            txtTenLoaiMay.Text = row.Cells["colLoaiTen"].Value?.ToString();
            txtMoTaLoaiMay.Text = row.Cells["colLoaiMoTa"].Value?.ToString();
        }

        #endregion

        #region TAB 3: NHÀ CUNG CẤP

        // Khởi tạo cấu hình và liên kết cho SetupNCCEvents.
        private void SetupNCCEvents()
        {
            dgvNCC.CellClick += DgvNCC_CellClick;

            btnThemNCC.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtTenNCC.Text))
                {
                    Function.ThongBaoCanhBao("Vui lòng nhập tên nhà cung cấp!", "Thông báo");
                    txtTenNCC.Focus();
                    return;
                }

                string ma = txtMaNCC.Text.Trim();
                if (string.IsNullOrEmpty(ma))
                {
                    ma = Function.SinhMaTuDong("NHA_CUNG_CAP", "MaNCC", "NCC", 2);
                }

                object check = DataProcesser.ExecuteScalar("SELECT COUNT(*) FROM NHA_CUNG_CAP WHERE MaNCC = @ma", new SqlParameter("@ma", ma));
                if (check != null && Convert.ToInt32(check) > 0)
                {
                    ma = Function.SinhMaTuDong("NHA_CUNG_CAP", "MaNCC", "NCC", 2);
                }

                string sql = "INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, Sdt, DiaChi) VALUES (@ma, @ten, @sdt, @diachi)";
                int rows = DataProcesser.ChangeData(sql,
                    new SqlParameter("@ma", ma),
                    new SqlParameter("@ten", txtTenNCC.Text.Trim()),
                    new SqlParameter("@sdt", txtSdtNCC.Text.Trim()),
                    new SqlParameter("@diachi", txtDiaChiNCC.Text.Trim()));

                if (rows > 0)
                {
                    Function.ThongBaoThanhCong("Thêm nhà cung cấp thành công! Mã: " + ma, "Thông báo");
                    LoadDataNCC();
                    LoadComboboxesLaptop();
                }
            };

            btnSuaNCC.Click += (s, e) =>
            {
                string ma = txtMaNCC.Text.Trim();
                if (string.IsNullOrEmpty(ma))
                {
                    Function.ThongBaoCanhBao("Vui lòng chọn nhà cung cấp cần sửa từ bảng!", "Thông báo");
                    return;
                }

                string sql = "UPDATE NHA_CUNG_CAP SET TenNCC = @ten, Sdt = @sdt, DiaChi = @diachi WHERE MaNCC = @ma";
                int rows = DataProcesser.ChangeData(sql,
                    new SqlParameter("@ma", ma),
                    new SqlParameter("@ten", txtTenNCC.Text.Trim()),
                    new SqlParameter("@sdt", txtSdtNCC.Text.Trim()),
                    new SqlParameter("@diachi", txtDiaChiNCC.Text.Trim()));

                if (rows > 0)
                {
                    Function.ThongBaoThanhCong("Cập nhật thông tin NCC thành công!", "Thông báo");
                    LoadDataNCC();
                    LoadComboboxesLaptop();
                }
            };

            btnXoaNCC.Click += (s, e) =>
            {
                string ma = txtMaNCC.Text.Trim();
                if (string.IsNullOrEmpty(ma))
                {
                    Function.ThongBaoCanhBao("Vui lòng chọn NCC cần xóa!", "Thông báo");
                    return;
                }

                object count = DataProcesser.ExecuteScalar("SELECT COUNT(*) FROM LAPTOP WHERE MaNCC = @ma", new SqlParameter("@ma", ma));
                if (count != null && Convert.ToInt32(count) > 0)
                {
                    Function.ThongBaoCanhBao("Có " + count + " dòng laptop thuộc nhà cung cấp này, không thể xóa!", "Cảnh báo");
                    return;
                }

                DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa NCC " + ma + " không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    DataProcesser.ChangeData("DELETE FROM NHA_CUNG_CAP WHERE MaNCC = @ma", new SqlParameter("@ma", ma));
                    Function.ThongBaoThanhCong("Đã xóa nhà cung cấp!", "Thông báo");
                    LoadDataNCC();
                    LoadComboboxesLaptop();
                    txtMaNCC.Clear();
                    txtTenNCC.Clear();
                    txtSdtNCC.Clear();
                    txtDiaChiNCC.Clear();
                }
            };

            btnLamMoiNCC.Click += (s, e) =>
            {
                txtMaNCC.Text = Function.SinhMaTuDong("NHA_CUNG_CAP", "MaNCC", "NCC", 2);
                txtTenNCC.Clear();
                txtSdtNCC.Clear();
                txtDiaChiNCC.Clear();
                LoadDataNCC();
            };
        }

        // Tải dữ liệu phục vụ LoadDataNCC.
        private void LoadDataNCC()
        {
            string sql = @"
                SELECT n.MaNCC, n.TenNCC, n.Sdt, n.DiaChi, COUNT(l.MaLaptop) AS SoDongMay
                FROM NHA_CUNG_CAP n
                LEFT JOIN LAPTOP l ON n.MaNCC = l.MaNCC
                GROUP BY n.MaNCC, n.TenNCC, n.Sdt, n.DiaChi
                ORDER BY n.MaNCC ASC";

            DataTable dt = DataProcesser.ReadData(sql);
            dgvNCC.DataSource = dt;

            txtMaNCC.Text = Function.SinhMaTuDong("NHA_CUNG_CAP", "MaNCC", "NCC", 2);
        }

        // Xử lý sự kiện DgvNCC_CellClick của giao diện.
        private void DgvNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvNCC.Rows.Count) return;
            DataGridViewRow row = dgvNCC.Rows[e.RowIndex];
            txtMaNCC.Text = row.Cells["colNCCMa"].Value?.ToString();
            txtTenNCC.Text = row.Cells["colNCCTen"].Value?.ToString();
            txtSdtNCC.Text = row.Cells["colNCCSdt"].Value?.ToString();
            txtDiaChiNCC.Text = row.Cells["colNCCDiaChi"].Value?.ToString();
        }

        #endregion
    }
}

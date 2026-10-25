using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmSanPham : Form
    {
        /// <summary>
        /// Biến toàn cục static nhận UserName từ frmDangNhap theo yêu cầu đề bài
        /// </summary>
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

        public frmSanPham()
        {
            InitializeComponent();

            NavigationManager.RegisterForm(NavigationMenu.QuanLyLaptop, this);
            sidebarControl1.SelectedMenu = NavigationMenu.QuanLyLaptop;

            dgvLaptop.AutoGenerateColumns = false;
            dgvLoaiMay.AutoGenerateColumns = false;
            dgvNCC.AutoGenerateColumns = false;
            this.WindowState = FormWindowState.Maximized;

            this.Load += FrmSanPham_Load;

            SetupTabEvents();
            SetupLaptopEvents();
            SetupLoaiMayEvents();
            SetupNCCEvents();
        }

        private void FrmSanPham_Load(object sender, EventArgs e)
        {
            // Tự động bổ sung các cột DonGiaNhap, Anh, GhiChu vào DB nếu chưa có
            EnsureLaptopColumnsExist();

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

            SwitchTab(ActiveTab.TatCaMay);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.QuanLyLaptop;
                LoadComboboxesLaptop();
                LoadDataLaptop();
            }
        }

        private void EnsureLaptopColumnsExist()
        {
            try
            {
                string sql = @"
                    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('LAPTOP') AND name = 'DonGiaNhap')
                    BEGIN
                        ALTER TABLE LAPTOP ADD DonGiaNhap DECIMAL(18,0) NULL DEFAULT 0;
                    END
                    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('LAPTOP') AND name = 'Anh')
                    BEGIN
                        ALTER TABLE LAPTOP ADD Anh NVARCHAR(255) NULL;
                    END
                    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('LAPTOP') AND name = 'GhiChu')
                    BEGIN
                        ALTER TABLE LAPTOP ADD GhiChu NVARCHAR(500) NULL;
                    END
                ";
                DataProcesser.ChangeData(sql);
            }
            catch { }
        }

        #region TAB SWITCHING

        private void SetupTabEvents()
        {
            btnTabTatCaMay.Click += (s, e) => SwitchTab(ActiveTab.TatCaMay);
            btnTabLoaiMay.Click += (s, e) => SwitchTab(ActiveTab.LoaiMay);
            btnTabNhaCungCap.Click += (s, e) => SwitchTab(ActiveTab.NhaCungCap);
        }

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

        private void SetTabButtonStyle(Button btn, bool isActive)
        {
            if (isActive)
            {
                btn.BackColor = _colorNavy;
                btn.ForeColor = _colorWhite;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font("Segoe UI", 9.25f, FontStyle.Bold);
            }
            else
            {
                btn.BackColor = _colorWhite;
                btn.ForeColor = _colorNavy;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = _colorNavy;
                btn.Font = new Font("Segoe UI", 9.25f, FontStyle.Regular);
            }
        }

        #endregion

        #region TAB 1: TẤT CẢ CÁC MÁY (LAPTOP / SẢN PHẨM)

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
                txtTimKiem.Focus();
            };

            // Các nút chức năng
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnKhoa.Click += BtnKhoa_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => LamMoiLaptopInputs();
            btnThoat.Click += BtnThoat_Click;
        }

        private void TxtChiNhapSoNguyen_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Các TextBox Số Lượng, Đơn giá Nhập, Đơn giá bán chỉ cho phép người dùng nhập số nguyên
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

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
        }

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

        private void DgvLaptop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvLaptop.Rows.Count) return;

            DataGridViewRow row = dgvLaptop.Rows[e.RowIndex];
            txtMaLaptop.Text = GetCellString(row, "colMaLaptop");
            txtTenLaptop.Text = GetCellString(row, "colTenLaptop");
            cboLoaiLaptop.Text = GetCellString(row, "colLoai");
            cboNhaCungCap.Text = GetCellString(row, "colTenNCC");

            txtCPU.Text = GetCellString(row, "colCPU");
            cboRAM.Text = GetCellString(row, "colRAM");
            cboSSD.Text = GetCellString(row, "colSSD");

            // Hiển thị số lượng, đơn giá nhập, đơn giá bán, ghi chú
            txtSoLuong.Text = GetCellString(row, "colSoLuongTon");
            txtDonGiaNhap.Text = GetCellString(row, "colDonGiaNhap");
            if (string.IsNullOrEmpty(txtDonGiaNhap.Text)) txtDonGiaNhap.Text = "0";
            txtDonGiaBan.Text = GetCellString(row, "colDonGia");
            txtGhiChu.Text = GetCellString(row, "colGhiChu");

            string bhStr = GetCellString(row, "colBaoHanh");
            if (int.TryParse(bhStr, out int baoHanh))
                numBaoHanh.Value = Math.Min(numBaoHanh.Maximum, Math.Max(numBaoHanh.Minimum, baoHanh));

            string trangThai = GetCellString(row, "colTrangThai");
            btnKhoa.Text = (trangThai == "Ngừng KD") ? "Mở khóa" : "Khóa máy";
        }

        // them laptop moi
        private void BtnThem_Click(object sender, EventArgs e)
        {
            // Tu dong sinh ma MHxx neu de trong hoac ma da ton tai
            object chkTonTai = null;
            if (!string.IsNullOrWhiteSpace(txtMaLaptop.Text))
            {
                chkTonTai = DataProcesser.ExecuteScalar(
                    "SELECT 1 FROM LAPTOP WHERE MaLaptop = @ma",
                    new SqlParameter("@ma", txtMaLaptop.Text.Trim()));
            }

            if (string.IsNullOrWhiteSpace(txtMaLaptop.Text) || (chkTonTai != null && chkTonTai != DBNull.Value))
            {
                txtMaLaptop.Text = Function.SinhMaTuDong("LAPTOP", "MaLaptop", "MH", 2);
            }
            string ma = txtMaLaptop.Text.Trim();
            string ten = txtTenLaptop.Text.Trim();
            string soLuongStr = txtSoLuong.Text.Trim();
            string giaNhapStr = txtDonGiaNhap.Text.Trim();
            string giaBanStr = txtDonGiaBan.Text.Trim();

            if (string.IsNullOrWhiteSpace(ten))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenLaptop.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(soLuongStr))
            {
                MessageBox.Show("Vui lòng nhập số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(giaNhapStr))
            {
                MessageBox.Show("Vui lòng nhập đơn giá nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaNhap.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(giaBanStr))
            {
                MessageBox.Show("Vui lòng nhập đơn giá bán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaBan.Focus();
                return;
            }

            if (!int.TryParse(soLuongStr, out int soLuong) || soLuong < 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên không âm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }

            if (!decimal.TryParse(giaNhapStr, out decimal donGiaNhap) || donGiaNhap < 0)
            {
                MessageBox.Show("Đơn giá nhập phải là số hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaNhap.Focus();
                return;
            }

            if (!decimal.TryParse(giaBanStr, out decimal donGiaBan) || donGiaBan < 0)
            {
                MessageBox.Show("Đơn giá bán phải là số hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaBan.Focus();
                return;
            }

            // 2. Đơn giá bán phải lớn hơn đơn giá nhập
            if (donGiaBan <= donGiaNhap)
            {
                MessageBox.Show("Đơn giá bán phải lớn hơn đơn giá nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaBan.Focus();
                return;
            }

            // 3. Kiểm tra mã sản phẩm không được trùng
            object check = DataProcesser.ExecuteScalar("SELECT COUNT(*) FROM LAPTOP WHERE MaLaptop = @ma", new SqlParameter("@ma", ma));
            if (check != null && Convert.ToInt32(check) > 0)
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại, vui lòng chọn mã khác!", "Cảnh báo trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaLaptop.Focus();
                return;
            }

            string loai = cboLoaiLaptop.Text.Trim();
            string ncc = cboNhaCungCap.SelectedValue != null ? cboNhaCungCap.SelectedValue.ToString() : (cboNhaCungCap.Items.Count > 0 ? "NCC01" : "");
            string cpu = string.IsNullOrWhiteSpace(txtCPU.Text) ? "Core i5" : txtCPU.Text.Trim();
            string ram = string.IsNullOrWhiteSpace(cboRAM.Text) ? "8GB" : cboRAM.Text.Trim();
            string ssd = string.IsNullOrWhiteSpace(cboSSD.Text) ? "512GB" : cboSSD.Text.Trim();
            int bh = (int)numBaoHanh.Value;
            string anh = "";
            string ghiChu = txtGhiChu.Text.Trim();

            // 4. Thêm vào bảng LAPTOP (Ảnh chỉ lưu tên)
            string sql = @"
                INSERT INTO LAPTOP (MaLaptop, TenLaptop, Loai, CPU, RAM, SSD, DonGiaNhap, DonGia, SoLuongTon, ThoiGianBaoHanh, MaNCC, Anh, GhiChu, NgungKinhDoanh)
                VALUES (@ma, @ten, @loai, @cpu, @ram, @ssd, @giaNhap, @giaBan, @ton, @bh, @ncc, @anh, @ghichu, 0)";

            int rows = DataProcesser.ChangeData(sql,
                new SqlParameter("@ma", ma),
                new SqlParameter("@ten", ten),
                new SqlParameter("@loai", loai),
                new SqlParameter("@cpu", cpu),
                new SqlParameter("@ram", ram),
                new SqlParameter("@ssd", ssd),
                new SqlParameter("@giaNhap", donGiaNhap),
                new SqlParameter("@giaBan", donGiaBan),
                new SqlParameter("@ton", soLuong),
                new SqlParameter("@bh", bh),
                new SqlParameter("@ncc", ncc),
                new SqlParameter("@anh", anh),
                new SqlParameter("@ghichu", ghiChu));

            if (rows > 0)
            {
                MessageBox.Show("Thêm sản phẩm thành công! Mã: " + ma, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataLaptop();
                // 5. Sau khi thêm sẽ xóa trắng các TextBox nhập liệu trên form
                LamMoiLaptopInputs();
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            string ma = txtMaLaptop.Text.Trim();
            if (string.IsNullOrEmpty(ma))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ten = txtTenLaptop.Text.Trim();
            string soLuongStr = txtSoLuong.Text.Trim();
            string giaNhapStr = txtDonGiaNhap.Text.Trim();
            string giaBanStr = txtDonGiaBan.Text.Trim();

            if (string.IsNullOrWhiteSpace(ten) || string.IsNullOrWhiteSpace(soLuongStr) ||
                string.IsNullOrWhiteSpace(giaNhapStr) || string.IsNullOrWhiteSpace(giaBanStr))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(soLuongStr, out int soLuong) || soLuong < 0 ||
                !decimal.TryParse(giaNhapStr, out decimal donGiaNhap) || donGiaNhap < 0 ||
                !decimal.TryParse(giaBanStr, out decimal donGiaBan) || donGiaBan < 0)
            {
                MessageBox.Show("Số lượng, đơn giá nhập và đơn giá bán phải là số hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (donGiaBan <= donGiaNhap)
            {
                MessageBox.Show("Đơn giá bán phải lớn hơn đơn giá nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaBan.Focus();
                return;
            }

            string loai = cboLoaiLaptop.Text.Trim();
            string ncc = cboNhaCungCap.SelectedValue?.ToString() ?? "";
            string cpu = string.IsNullOrWhiteSpace(txtCPU.Text) ? "Core i5" : txtCPU.Text.Trim();
            string ram = string.IsNullOrWhiteSpace(cboRAM.Text) ? "8GB" : cboRAM.Text.Trim();
            string ssd = string.IsNullOrWhiteSpace(cboSSD.Text) ? "512GB" : cboSSD.Text.Trim();
            int bh = (int)numBaoHanh.Value;
            string anh = "";
            string ghiChu = txtGhiChu.Text.Trim();

            string sql = @"
                UPDATE LAPTOP 
                SET TenLaptop = @ten, Loai = @loai, CPU = @cpu, RAM = @ram, SSD = @ssd, 
                    DonGiaNhap = @giaNhap, DonGia = @giaBan, SoLuongTon = @ton, ThoiGianBaoHanh = @bh, 
                    MaNCC = @ncc, Anh = @anh, GhiChu = @ghichu
                WHERE MaLaptop = @ma";

            int rows = DataProcesser.ChangeData(sql,
                new SqlParameter("@ma", ma),
                new SqlParameter("@ten", ten),
                new SqlParameter("@loai", loai),
                new SqlParameter("@cpu", cpu),
                new SqlParameter("@ram", ram),
                new SqlParameter("@ssd", ssd),
                new SqlParameter("@giaNhap", donGiaNhap),
                new SqlParameter("@giaBan", donGiaBan),
                new SqlParameter("@ton", soLuong),
                new SqlParameter("@bh", bh),
                new SqlParameter("@ncc", ncc),
                new SqlParameter("@anh", anh),
                new SqlParameter("@ghichu", ghiChu));

            if (rows > 0)
            {
                MessageBox.Show("Cập nhật thông tin sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataLaptop();
            }
        }

        private void BtnKhoa_Click(object sender, EventArgs e)
        {
            string ma = txtMaLaptop.Text.Trim();
            if (string.IsNullOrEmpty(ma))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần khóa / mở khóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = "UPDATE LAPTOP SET NgungKinhDoanh = CASE WHEN NgungKinhDoanh = 1 THEN 0 ELSE 1 END WHERE MaLaptop = @ma";
            int rows = DataProcesser.ChangeData(sql, new SqlParameter("@ma", ma));
            if (rows > 0)
            {
                MessageBox.Show("Đã cập nhật trạng thái kinh doanh của máy " + ma, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataLaptop();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            string ma = txtMaLaptop.Text.Trim();
            // Kiểm tra xem người dùng đã chọn hàng để xóa chưa
            if (string.IsNullOrEmpty(ma))
            {
                MessageBox.Show("Bạn chưa chọn hàng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        MessageBox.Show("Đã chuyển trạng thái sang 'Ngừng kinh doanh'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataLaptop();
                        LamMoiLaptopInputs();
                    }
                    return;
                }

                int rows = DataProcesser.ChangeData("DELETE FROM LAPTOP WHERE MaLaptop = @ma", new SqlParameter("@ma", ma));
                if (rows > 0)
                {
                    MessageBox.Show("Đã xóa sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Sau đó load lại dữ liệu lên GridView
                    LoadDataLaptop();
                    LamMoiLaptopInputs();
                }
            }
        }

        private void BtnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có muốn thoát khỏi danh mục sản phẩm không?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // lam moi cac o nhap lieu va sinh ma hang MHxx tu dong
        private void LamMoiLaptopInputs()
        {
            txtMaLaptop.Text = Function.SinhMaTuDong("LAPTOP", "MaLaptop", "MH", 2);
            txtMaLaptop.ReadOnly = true;
            txtTenLaptop.Clear();
            txtSoLuong.Clear();
            txtDonGiaNhap.Clear();
            txtDonGiaBan.Clear();
            txtGhiChu.Clear();
            txtCPU.Clear();

            if (cboLoaiLaptop.Items.Count > 0) cboLoaiLaptop.SelectedIndex = 0;
            if (cboNhaCungCap.Items.Count > 0) cboNhaCungCap.SelectedIndex = 0;
            if (cboRAM.Items.Count > 0) cboRAM.SelectedIndex = 1;
            if (cboSSD.Items.Count > 0) cboSSD.SelectedIndex = 1;
            numBaoHanh.Value = 12;
            btnKhoa.Text = "Khóa máy";
            txtTenLaptop.Focus();
        }

        #endregion

        #region TAB 2: ĐIỀU CHỈNH LOẠI MÁY

        private void SetupLoaiMayEvents()
        {
            dgvLoaiMay.CellClick += DgvLoaiMay_CellClick;

            btnThemLoai.Click += (s, e) =>
            {
                string ten = txtTenLoaiMay.Text.Trim();
                if (string.IsNullOrEmpty(ten))
                {
                    MessageBox.Show("Vui lòng nhập tên loại máy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenLoaiMay.Focus();
                    return;
                }

                object check = DataProcesser.ExecuteScalar("SELECT COUNT(*) FROM LOAI_MAY WHERE TenLoai = @ten", new SqlParameter("@ten", ten));
                if (check != null && Convert.ToInt32(check) > 0)
                {
                    MessageBox.Show("Loại máy này đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataProcesser.ChangeData("INSERT INTO LOAI_MAY (TenLoai, MoTa) VALUES (@ten, @mota)",
                    new SqlParameter("@ten", ten),
                    new SqlParameter("@mota", txtMoTaLoaiMay.Text.Trim()));

                MessageBox.Show("Thêm loại máy thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataLoaiMay();
                LoadComboboxesLaptop();
            };

            btnSuaLoai.Click += (s, e) =>
            {
                string ten = txtTenLoaiMay.Text.Trim();
                if (string.IsNullOrEmpty(ten))
                {
                    MessageBox.Show("Vui lòng chọn loại máy cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataProcesser.ChangeData("UPDATE LOAI_MAY SET MoTa = @mota WHERE TenLoai = @ten",
                    new SqlParameter("@ten", ten),
                    new SqlParameter("@mota", txtMoTaLoaiMay.Text.Trim()));

                MessageBox.Show("Cập nhật loại máy thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataLoaiMay();
            };

            btnXoaLoai.Click += (s, e) =>
            {
                string ten = txtTenLoaiMay.Text.Trim();
                if (string.IsNullOrEmpty(ten))
                {
                    MessageBox.Show("Vui lòng chọn loại máy cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                object count = DataProcesser.ExecuteScalar("SELECT COUNT(*) FROM LAPTOP WHERE Loai = @ten", new SqlParameter("@ten", ten));
                if (count != null && Convert.ToInt32(count) > 0)
                {
                    MessageBox.Show("Có " + count + " laptop đang thuộc loại máy này, không thể xóa trực tiếp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa loại máy '" + ten + "'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    DataProcesser.ChangeData("DELETE FROM LOAI_MAY WHERE TenLoai = @ten", new SqlParameter("@ten", ten));
                    MessageBox.Show("Đã xóa loại máy thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void DgvLoaiMay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvLoaiMay.Rows.Count) return;
            DataGridViewRow row = dgvLoaiMay.Rows[e.RowIndex];
            txtTenLoaiMay.Text = row.Cells["colLoaiTen"].Value?.ToString();
            txtMoTaLoaiMay.Text = row.Cells["colLoaiMoTa"].Value?.ToString();
        }

        #endregion

        #region TAB 3: NHÀ CUNG CẤP

        private void SetupNCCEvents()
        {
            dgvNCC.CellClick += DgvNCC_CellClick;

            btnThemNCC.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtTenNCC.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Thêm nhà cung cấp thành công! Mã: " + ma, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataNCC();
                    LoadComboboxesLaptop();
                }
            };

            btnSuaNCC.Click += (s, e) =>
            {
                string ma = txtMaNCC.Text.Trim();
                if (string.IsNullOrEmpty(ma))
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp cần sửa từ bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Cập nhật thông tin NCC thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataNCC();
                    LoadComboboxesLaptop();
                }
            };

            btnXoaNCC.Click += (s, e) =>
            {
                string ma = txtMaNCC.Text.Trim();
                if (string.IsNullOrEmpty(ma))
                {
                    MessageBox.Show("Vui lòng chọn NCC cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                object count = DataProcesser.ExecuteScalar("SELECT COUNT(*) FROM LAPTOP WHERE MaNCC = @ma", new SqlParameter("@ma", ma));
                if (count != null && Convert.ToInt32(count) > 0)
                {
                    MessageBox.Show("Có " + count + " dòng laptop thuộc nhà cung cấp này, không thể xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa NCC " + ma + " không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    DataProcesser.ChangeData("DELETE FROM NHA_CUNG_CAP WHERE MaNCC = @ma", new SqlParameter("@ma", ma));
                    MessageBox.Show("Đã xóa nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

namespace QuanLyBanLaptop.DanhMuc
{
    partial class frmNhanVien
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.sidebarControl1    = new QuanLyBanLaptop.SidebarControl();
            this.pnlMain            = new System.Windows.Forms.Panel();
            this.pnlSearch          = new System.Windows.Forms.Panel();
            this.lblTimKiem         = new System.Windows.Forms.Label();
            this.txtTimKiem         = new System.Windows.Forms.TextBox();
            this.btnTimKiem         = new System.Windows.Forms.Button();
            this.btnLamMoi          = new System.Windows.Forms.Button();
            this.grpThongTinNV      = new System.Windows.Forms.GroupBox();
            this.lblMaNV            = new System.Windows.Forms.Label();
            this.txtMaNV            = new System.Windows.Forms.TextBox();
            this.lblTenNV           = new System.Windows.Forms.Label();
            this.txtTenNV           = new System.Windows.Forms.TextBox();
            this.lblGioiTinh        = new System.Windows.Forms.Label();
            this.radNam             = new System.Windows.Forms.RadioButton();
            this.radNu              = new System.Windows.Forms.RadioButton();
            this.lblNgaySinh        = new System.Windows.Forms.Label();
            this.dtpNgaySinh        = new System.Windows.Forms.DateTimePicker();
            this.lblSDT             = new System.Windows.Forms.Label();
            this.txtSDT             = new System.Windows.Forms.TextBox();
            this.lblDiaChi          = new System.Windows.Forms.Label();
            this.txtDiaChi          = new System.Windows.Forms.TextBox();
            this.lblChucVu          = new System.Windows.Forms.Label();
            this.cboChucVu          = new System.Windows.Forms.ComboBox();
            this.lblTenDN           = new System.Windows.Forms.Label();
            this.txtTenDN           = new System.Windows.Forms.TextBox();
            this.lblMatKhau         = new System.Windows.Forms.Label();
            this.txtMatKhau         = new System.Windows.Forms.TextBox();
            this.pnlButtons         = new System.Windows.Forms.Panel();
            this.btnThem            = new System.Windows.Forms.Button();
            this.btnSua             = new System.Windows.Forms.Button();
            this.btnXoa             = new System.Windows.Forms.Button();
            this.btnLuu             = new System.Windows.Forms.Button();
            this.btnHuy             = new System.Windows.Forms.Button();
            this.btnThoat           = new System.Windows.Forms.Button();
            this.dgvNhanVien        = new System.Windows.Forms.DataGridView();
            this.colMaNV            = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenNV           = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGioiTinh        = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgaySinh        = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSDT             = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiaChi          = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChucVu          = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenDN           = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai       = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatKhau         = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.pnlMain.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.grpThongTinNV.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // sidebarControl1
            this.sidebarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarControl1.Location = new System.Drawing.Point(0, 0);
            this.sidebarControl1.Name = "sidebarControl1";
            this.sidebarControl1.SelectedMenu = QuanLyBanLaptop.Classes.NavigationMenu.NhanVien;
            this.sidebarControl1.Size = new System.Drawing.Size(230, 700);
            this.sidebarControl1.TabIndex = 0;

            // pnlMain
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(240, 242, 247);
            this.pnlMain.Controls.Add(this.dgvNhanVien);
            this.pnlMain.Controls.Add(this.pnlButtons);
            this.pnlMain.Controls.Add(this.grpThongTinNV);
            this.pnlMain.Controls.Add(this.pnlSearch);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(230, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(14, 12, 14, 12);
            this.pnlMain.Size = new System.Drawing.Size(1009, 700);
            this.pnlMain.TabIndex = 1;

            // pnlSearch
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.Controls.Add(this.lblTimKiem);
            this.pnlSearch.Controls.Add(this.txtTimKiem);
            this.pnlSearch.Controls.Add(this.btnTimKiem);
            this.pnlSearch.Controls.Add(this.btnLamMoi);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(14, 12);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlSearch.Size = new System.Drawing.Size(981, 52);
            this.pnlSearch.TabIndex = 0;

            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTimKiem.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblTimKiem.Location = new System.Drawing.Point(12, 16);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Text = "Từ khóa tìm kiếm:";

            this.txtTimKiem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTimKiem.Location = new System.Drawing.Point(135, 12);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(420, 27);
            this.txtTimKiem.TabIndex = 1;
            this.txtTimKiem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimKiem_KeyDown);

            this.btnTimKiem.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnTimKiem.FlatAppearance.BorderSize = 0;
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Location = new System.Drawing.Point(565, 10);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(110, 32);
            this.btnTimKiem.Text = "🔍  Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            this.btnTimKiem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);

            this.btnLamMoi.BackColor = System.Drawing.Color.White;
            this.btnLamMoi.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(209, 213, 219);
            this.btnLamMoi.FlatAppearance.BorderSize = 1;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLamMoi.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnLamMoi.Location = new System.Drawing.Point(685, 10);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(110, 32);
            this.btnLamMoi.Text = "↺  Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);

            // grpThongTinNV
            this.grpThongTinNV.BackColor = System.Drawing.Color.White;
            this.grpThongTinNV.Controls.Add(this.lblMaNV);
            this.grpThongTinNV.Controls.Add(this.txtMaNV);
            this.grpThongTinNV.Controls.Add(this.lblTenNV);
            this.grpThongTinNV.Controls.Add(this.txtTenNV);
            this.grpThongTinNV.Controls.Add(this.lblGioiTinh);
            this.grpThongTinNV.Controls.Add(this.radNam);
            this.grpThongTinNV.Controls.Add(this.radNu);
            this.grpThongTinNV.Controls.Add(this.lblNgaySinh);
            this.grpThongTinNV.Controls.Add(this.dtpNgaySinh);
            this.grpThongTinNV.Controls.Add(this.lblSDT);
            this.grpThongTinNV.Controls.Add(this.txtSDT);
            this.grpThongTinNV.Controls.Add(this.lblDiaChi);
            this.grpThongTinNV.Controls.Add(this.txtDiaChi);
            this.grpThongTinNV.Controls.Add(this.lblChucVu);
            this.grpThongTinNV.Controls.Add(this.cboChucVu);
            this.grpThongTinNV.Controls.Add(this.lblTenDN);
            this.grpThongTinNV.Controls.Add(this.txtTenDN);
            this.grpThongTinNV.Controls.Add(this.lblMatKhau);
            this.grpThongTinNV.Controls.Add(this.txtMatKhau);
            this.grpThongTinNV.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpThongTinNV.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpThongTinNV.ForeColor = System.Drawing.Color.FromArgb(30, 58, 138);
            this.grpThongTinNV.Location = new System.Drawing.Point(14, 64);
            this.grpThongTinNV.Name = "grpThongTinNV";
            this.grpThongTinNV.Padding = new System.Windows.Forms.Padding(12, 6, 12, 10);
            this.grpThongTinNV.Size = new System.Drawing.Size(981, 215);
            this.grpThongTinNV.TabIndex = 1;
            this.grpThongTinNV.TabStop = false;
            this.grpThongTinNV.Text = "Thông tin nhân viên";

            int fH = 28;
            System.Drawing.Font fFont = new System.Drawing.Font("Segoe UI", 9.5F);
            System.Drawing.Font lFont = new System.Drawing.Font("Segoe UI", 9.5F);

            // Row 1: MaNV (trai) | NgaySinh (phai)
            this.lblMaNV.AutoSize = true; this.lblMaNV.Font = lFont; this.lblMaNV.ForeColor = System.Drawing.Color.FromArgb(55,65,81);
            this.lblMaNV.Location = new System.Drawing.Point(14, 26); this.lblMaNV.Name = "lblMaNV"; this.lblMaNV.Text = "Mã NV:";
            this.txtMaNV.BackColor = System.Drawing.Color.FromArgb(249,250,251); this.txtMaNV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMaNV.Font = fFont; this.txtMaNV.Location = new System.Drawing.Point(120, 23); this.txtMaNV.Name = "txtMaNV"; this.txtMaNV.ReadOnly = true; this.txtMaNV.Size = new System.Drawing.Size(280, fH);

            this.lblNgaySinh.AutoSize = true; this.lblNgaySinh.Font = lFont; this.lblNgaySinh.ForeColor = System.Drawing.Color.FromArgb(55,65,81);
            this.lblNgaySinh.Location = new System.Drawing.Point(480, 26); this.lblNgaySinh.Name = "lblNgaySinh"; this.lblNgaySinh.Text = "Ngày sinh:";
            this.dtpNgaySinh.CustomFormat = "dd/MM/yyyy"; this.dtpNgaySinh.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgaySinh.Font = fFont; this.dtpNgaySinh.Location = new System.Drawing.Point(580, 23); this.dtpNgaySinh.Name = "dtpNgaySinh"; this.dtpNgaySinh.Size = new System.Drawing.Size(280, fH);

            // Row 2: TenNV (trai) | GioiTinh (phai)
            this.lblTenNV.AutoSize = true; this.lblTenNV.Font = lFont; this.lblTenNV.ForeColor = System.Drawing.Color.FromArgb(55,65,81);
            this.lblTenNV.Location = new System.Drawing.Point(14, 61); this.lblTenNV.Name = "lblTenNV"; this.lblTenNV.Text = "Họ tên:";
            this.txtTenNV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.txtTenNV.Font = fFont;
            this.txtTenNV.Location = new System.Drawing.Point(120, 58); this.txtTenNV.Name = "txtTenNV"; this.txtTenNV.Size = new System.Drawing.Size(280, fH);

            this.lblGioiTinh.AutoSize = true; this.lblGioiTinh.Font = lFont; this.lblGioiTinh.ForeColor = System.Drawing.Color.FromArgb(55,65,81);
            this.lblGioiTinh.Location = new System.Drawing.Point(480, 61); this.lblGioiTinh.Name = "lblGioiTinh"; this.lblGioiTinh.Text = "Giới tính:";
            this.radNam.AutoSize = true; this.radNam.Checked = true; this.radNam.Font = fFont;
            this.radNam.Location = new System.Drawing.Point(580, 60); this.radNam.Name = "radNam"; this.radNam.Text = "Nam";
            this.radNu.AutoSize = true; this.radNu.Font = fFont;
            this.radNu.Location = new System.Drawing.Point(660, 60); this.radNu.Name = "radNu"; this.radNu.Text = "Nữ";

            // Row 3: SDT (trai) | ChucVu (phai)
            this.lblSDT.AutoSize = true; this.lblSDT.Font = lFont; this.lblSDT.ForeColor = System.Drawing.Color.FromArgb(55,65,81);
            this.lblSDT.Location = new System.Drawing.Point(14, 96); this.lblSDT.Name = "lblSDT"; this.lblSDT.Text = "SĐT:";
            this.txtSDT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.txtSDT.Font = fFont;
            this.txtSDT.Location = new System.Drawing.Point(120, 93); this.txtSDT.Name = "txtSDT"; this.txtSDT.Size = new System.Drawing.Size(280, fH);

            this.lblChucVu.AutoSize = true; this.lblChucVu.Font = lFont; this.lblChucVu.ForeColor = System.Drawing.Color.FromArgb(55,65,81);
            this.lblChucVu.Location = new System.Drawing.Point(480, 96); this.lblChucVu.Name = "lblChucVu"; this.lblChucVu.Text = "Chức vụ:";
            this.cboChucVu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; this.cboChucVu.Font = fFont; this.cboChucVu.FormattingEnabled = true;
            this.cboChucVu.Location = new System.Drawing.Point(580, 93); this.cboChucVu.Name = "cboChucVu"; this.cboChucVu.Size = new System.Drawing.Size(280, fH);

            // Row 4: TenDN (trai) | MatKhau (phai)
            this.lblTenDN.AutoSize = true; this.lblTenDN.Font = lFont; this.lblTenDN.ForeColor = System.Drawing.Color.FromArgb(55,65,81);
            this.lblTenDN.Location = new System.Drawing.Point(14, 131); this.lblTenDN.Name = "lblTenDN"; this.lblTenDN.Text = "Tên đăng nhập:";
            this.txtTenDN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.txtTenDN.Font = fFont;
            this.txtTenDN.Location = new System.Drawing.Point(120, 128); this.txtTenDN.Name = "txtTenDN"; this.txtTenDN.Size = new System.Drawing.Size(280, fH);

            this.lblMatKhau.AutoSize = true; this.lblMatKhau.Font = lFont; this.lblMatKhau.ForeColor = System.Drawing.Color.FromArgb(55,65,81);
            this.lblMatKhau.Location = new System.Drawing.Point(480, 131); this.lblMatKhau.Name = "lblMatKhau"; this.lblMatKhau.Text = "Mật khẩu:";
            this.txtMatKhau.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.txtMatKhau.Font = fFont;
            this.txtMatKhau.Location = new System.Drawing.Point(580, 128); this.txtMatKhau.Name = "txtMatKhau"; this.txtMatKhau.Size = new System.Drawing.Size(280, fH);

            // Row 5: DiaChi (trai sang phai)
            this.lblDiaChi.AutoSize = true; this.lblDiaChi.Font = lFont; this.lblDiaChi.ForeColor = System.Drawing.Color.FromArgb(55,65,81);
            this.lblDiaChi.Location = new System.Drawing.Point(14, 168); this.lblDiaChi.Name = "lblDiaChi"; this.lblDiaChi.Text = "Địa chỉ:";
            this.txtDiaChi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.txtDiaChi.Font = fFont;
            this.txtDiaChi.Location = new System.Drawing.Point(120, 165); this.txtDiaChi.Name = "txtDiaChi"; this.txtDiaChi.Size = new System.Drawing.Size(740, fH);

            // pnlButtons
            this.pnlButtons.BackColor = System.Drawing.Color.White;
            this.pnlButtons.Controls.Add(this.btnThem);
            this.pnlButtons.Controls.Add(this.btnSua);
            this.pnlButtons.Controls.Add(this.btnXoa);
            this.pnlButtons.Controls.Add(this.btnLuu);
            this.pnlButtons.Controls.Add(this.btnHuy);
            this.pnlButtons.Controls.Add(this.btnThoat);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(14, 648);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlButtons.Size = new System.Drawing.Size(981, 54);
            this.pnlButtons.TabIndex = 3;

            System.Drawing.Font btnFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            int bW = 80; int bH = 32;

            this.btnThem.BackColor = System.Drawing.Color.White; this.btnThem.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225); this.btnThem.FlatAppearance.BorderSize = 1; this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = btnFont; this.btnThem.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59); this.btnThem.Location = new System.Drawing.Point(10, 10);
            this.btnThem.Name = "btnThem"; this.btnThem.Size = new System.Drawing.Size(bW, bH); this.btnThem.Text = "Thêm"; this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Cursor = System.Windows.Forms.Cursors.Hand; this.btnThem.Click += new System.EventHandler(this.btnThem_Click);

            this.btnSua.BackColor = System.Drawing.Color.White; this.btnSua.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225); this.btnSua.FlatAppearance.BorderSize = 1; this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.Font = btnFont; this.btnSua.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59); this.btnSua.Location = new System.Drawing.Point(100, 10);
            this.btnSua.Name = "btnSua"; this.btnSua.Size = new System.Drawing.Size(bW, bH); this.btnSua.Text = "Sửa"; this.btnSua.UseVisualStyleBackColor = false;
            this.btnSua.Cursor = System.Windows.Forms.Cursors.Hand; this.btnSua.Click += new System.EventHandler(this.btnSua_Click);

            this.btnXoa.BackColor = System.Drawing.Color.White; this.btnXoa.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225); this.btnXoa.FlatAppearance.BorderSize = 1; this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Font = btnFont; this.btnXoa.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59); this.btnXoa.Location = new System.Drawing.Point(190, 10);
            this.btnXoa.Name = "btnXoa"; this.btnXoa.Size = new System.Drawing.Size(bW, bH); this.btnXoa.Text = "Xóa"; this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Cursor = System.Windows.Forms.Cursors.Hand; this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);

            this.btnLuu.BackColor = System.Drawing.Color.White; this.btnLuu.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225); this.btnLuu.FlatAppearance.BorderSize = 1; this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = btnFont; this.btnLuu.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59); this.btnLuu.Location = new System.Drawing.Point(280, 10);
            this.btnLuu.Name = "btnLuu"; this.btnLuu.Size = new System.Drawing.Size(bW, bH); this.btnLuu.Text = "Lưu"; this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand; this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);

            this.btnHuy.BackColor = System.Drawing.Color.White; this.btnHuy.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225); this.btnHuy.FlatAppearance.BorderSize = 1; this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = btnFont; this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59); this.btnHuy.Location = new System.Drawing.Point(370, 10);
            this.btnHuy.Name = "btnHuy"; this.btnHuy.Size = new System.Drawing.Size(bW, bH); this.btnHuy.Text = "Hủy"; this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand; this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);

            this.btnThoat.BackColor = System.Drawing.Color.White; this.btnThoat.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225); this.btnThoat.FlatAppearance.BorderSize = 1; this.btnThoat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThoat.Font = btnFont; this.btnThoat.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59); this.btnThoat.Location = new System.Drawing.Point(460, 10);
            this.btnThoat.Name = "btnThoat"; this.btnThoat.Size = new System.Drawing.Size(bW, bH); this.btnThoat.Text = "Thoát"; this.btnThoat.UseVisualStyleBackColor = false;
            this.btnThoat.Cursor = System.Windows.Forms.Cursors.Hand; this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);

            // dgvNhanVien
            this.dgvNhanVien.AllowUserToAddRows = false; this.dgvNhanVien.AllowUserToDeleteRows = false;
            this.dgvNhanVien.AllowUserToResizeRows = false;
            this.dgvNhanVien.AllowUserToResizeColumns = false;
            this.dgvNhanVien.AllowUserToOrderColumns = false;
            this.dgvNhanVien.AutoGenerateColumns = false;
            this.dgvNhanVien.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNhanVien.BackgroundColor = System.Drawing.Color.White; this.dgvNhanVien.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvNhanVien.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(239,246,255);
            this.dgvNhanVien.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.dgvNhanVien.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(30,58,138);
            this.dgvNhanVien.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(239,246,255);
            this.dgvNhanVien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNhanVien.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvNhanVien.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(219,234,254);
            this.dgvNhanVien.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(30,64,175);
            this.dgvNhanVien.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colMaNV, this.colTenNV, this.colGioiTinh, this.colNgaySinh, this.colSDT, this.colDiaChi, this.colChucVu, this.colTenDN, this.colTrangThai, this.colMatKhau });
            this.dgvNhanVien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNhanVien.GridColor = System.Drawing.Color.FromArgb(229,231,235);
            this.dgvNhanVien.Location = new System.Drawing.Point(14, 284);
            this.dgvNhanVien.Name = "dgvNhanVien";
            this.dgvNhanVien.ReadOnly = true;
            this.dgvNhanVien.RowHeadersVisible = false;
            this.dgvNhanVien.RowTemplate.Height = 36;
            this.dgvNhanVien.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNhanVien.Size = new System.Drawing.Size(981, 364);
            this.dgvNhanVien.TabIndex = 2;
            this.dgvNhanVien.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNhanVien_CellClick);

            this.colMaNV.DataPropertyName = "MaNV"; this.colMaNV.HeaderText = "Mã NV"; this.colMaNV.Name = "MaNV"; this.colMaNV.ReadOnly = true; this.colMaNV.Width = 90;
            this.colTenNV.DataPropertyName = "TenNV"; this.colTenNV.HeaderText = "Họ tên"; this.colTenNV.Name = "TenNV"; this.colTenNV.ReadOnly = true; this.colTenNV.Width = 180;
            this.colGioiTinh.DataPropertyName = "GioiTinh"; this.colGioiTinh.HeaderText = "Giới tính"; this.colGioiTinh.Name = "GioiTinh"; this.colGioiTinh.ReadOnly = true; this.colGioiTinh.Width = 80;
            this.colNgaySinh.DataPropertyName = "NgaySinh"; this.colNgaySinh.HeaderText = "Ngày sinh"; this.colNgaySinh.Name = "NgaySinh"; this.colNgaySinh.ReadOnly = true; this.colNgaySinh.Width = 100;
            this.colSDT.DataPropertyName = "SDT"; this.colSDT.HeaderText = "SĐT"; this.colSDT.Name = "SDT"; this.colSDT.ReadOnly = true; this.colSDT.Width = 110;
            this.colDiaChi.DataPropertyName = "DiaChi"; this.colDiaChi.HeaderText = "Địa chỉ"; this.colDiaChi.Name = "DiaChi"; this.colDiaChi.ReadOnly = true; this.colDiaChi.Width = 200;
            this.colChucVu.DataPropertyName = "ChucVu"; this.colChucVu.HeaderText = "Chức vụ"; this.colChucVu.Name = "ChucVu"; this.colChucVu.ReadOnly = true; this.colChucVu.Width = 110;
            this.colTenDN.DataPropertyName = "TenDN"; this.colTenDN.HeaderText = "Tên đăng nhập"; this.colTenDN.Name = "TenDN"; this.colTenDN.ReadOnly = true; this.colTenDN.Width = 120;
            this.colTrangThai.DataPropertyName = "TrangThai"; this.colTrangThai.HeaderText = "Trạng thái"; this.colTrangThai.Name = "TrangThai"; this.colTrangThai.ReadOnly = true; this.colTrangThai.Width = 100;
            this.colMatKhau.DataPropertyName = "MatKhau"; this.colMatKhau.HeaderText = "Mật khẩu"; this.colMatKhau.Name = "MatKhau"; this.colMatKhau.ReadOnly = true; this.colMatKhau.Visible = false;

            // frmNhanVien
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 247);
            this.ClientSize = new System.Drawing.Size(1184, 700);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.sidebarControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.MinimumSize = new System.Drawing.Size(1100, 640);
            this.Name = "frmNhanVien";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Nhân Viên";
            this.pnlMain.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.grpThongTinNV.ResumeLayout(false);
            this.grpThongTinNV.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SidebarControl sidebarControl1;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.GroupBox grpThongTinNV;
        private System.Windows.Forms.Label lblMaNV;
        private System.Windows.Forms.TextBox txtMaNV;
        private System.Windows.Forms.Label lblTenNV;
        private System.Windows.Forms.TextBox txtTenNV;
        private System.Windows.Forms.Label lblGioiTinh;
        private System.Windows.Forms.RadioButton radNam;
        private System.Windows.Forms.RadioButton radNu;
        private System.Windows.Forms.Label lblNgaySinh;
        private System.Windows.Forms.DateTimePicker dtpNgaySinh;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblChucVu;
        private System.Windows.Forms.ComboBox cboChucVu;
        private System.Windows.Forms.Label lblTenDN;
        private System.Windows.Forms.TextBox txtTenDN;
        private System.Windows.Forms.Label lblMatKhau;
        private System.Windows.Forms.TextBox txtMatKhau;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.DataGridView dgvNhanVien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaNV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenNV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGioiTinh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgaySinh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSDT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiaChi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChucVu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenDN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatKhau;
    }
}

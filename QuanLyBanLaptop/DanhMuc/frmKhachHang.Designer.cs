namespace QuanLyBanLaptop.DanhMuc
{
    partial class frmKhachHang
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
            this.sidebarControl1     = new QuanLyBanLaptop.SidebarControl();
            this.pnlMain             = new System.Windows.Forms.Panel();
            this.pnlSearch           = new System.Windows.Forms.Panel();
            this.lblTimKiem          = new System.Windows.Forms.Label();
            this.txtTimKiem          = new System.Windows.Forms.TextBox();
            this.btnTimKiem          = new System.Windows.Forms.Button();
            this.btnLamMoi           = new System.Windows.Forms.Button();
            this.grpThongTin         = new System.Windows.Forms.GroupBox();
            this.lblMaKH             = new System.Windows.Forms.Label();
            this.txtMaKH             = new System.Windows.Forms.TextBox();
            this.lblNgaySinh         = new System.Windows.Forms.Label();
            this.dtpNgaySinh         = new System.Windows.Forms.DateTimePicker();
            this.lblHoTen            = new System.Windows.Forms.Label();
            this.txtHoTen            = new System.Windows.Forms.TextBox();
            this.lblSDT              = new System.Windows.Forms.Label();
            this.txtSDT              = new System.Windows.Forms.TextBox();
            this.lblGioiTinh         = new System.Windows.Forms.Label();
            this.cboGioiTinh         = new System.Windows.Forms.ComboBox();
            this.lblDiaChi           = new System.Windows.Forms.Label();
            this.txtDiaChi           = new System.Windows.Forms.TextBox();
            this.lblHang             = new System.Windows.Forms.Label();
            this.cboHangThanhVien    = new System.Windows.Forms.ComboBox();
            this.dgvKhachHang        = new System.Windows.Forms.DataGridView();
            this.pnlButtons          = new System.Windows.Forms.Panel();
            this.btnThem             = new System.Windows.Forms.Button();
            this.btnSua              = new System.Windows.Forms.Button();
            this.btnXoa              = new System.Windows.Forms.Button();
            this.btnLuu              = new System.Windows.Forms.Button();
            this.btnHuy              = new System.Windows.Forms.Button();
            this.btnThoat            = new System.Windows.Forms.Button();

            this.pnlMain.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.grpThongTin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhachHang)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // ── sidebarControl1 ──────────────────────────────────────────
            this.sidebarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarControl1.Location = new System.Drawing.Point(0, 0);
            this.sidebarControl1.Name = "sidebarControl1";
            this.sidebarControl1.SelectedMenu = QuanLyBanLaptop.Classes.NavigationMenu.KhachHang;
            this.sidebarControl1.Size = new System.Drawing.Size(230, 700);
            this.sidebarControl1.TabIndex = 0;

            // ── pnlMain ───────────────────────────────────────────────────
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(240, 242, 247);
            this.pnlMain.Controls.Add(this.dgvKhachHang);
            this.pnlMain.Controls.Add(this.pnlButtons);
            this.pnlMain.Controls.Add(this.grpThongTin);
            this.pnlMain.Controls.Add(this.pnlSearch);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(230, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(14, 12, 14, 12);
            this.pnlMain.Size = new System.Drawing.Size(1009, 700);
            this.pnlMain.TabIndex = 1;

            // ── pnlSearch ────────────────────────────────────────────────
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
            // Add rounded border via Paint event (done in code-behind would require owner draw)

            // lblTimKiem
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTimKiem.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblTimKiem.Location = new System.Drawing.Point(12, 16);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(115, 21);
            this.lblTimKiem.TabIndex = 0;
            this.lblTimKiem.Text = "Từ khóa tìm kiếm:";

            // txtTimKiem
            this.txtTimKiem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTimKiem.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.txtTimKiem.Location = new System.Drawing.Point(135, 12);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(430, 27);
            this.txtTimKiem.TabIndex = 1;
            this.txtTimKiem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimKiem_KeyDown);

            // btnTimKiem
            this.btnTimKiem.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnTimKiem.FlatAppearance.BorderSize = 0;
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Location = new System.Drawing.Point(575, 10);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(110, 32);
            this.btnTimKiem.TabIndex = 2;
            this.btnTimKiem.Text = "🔍  Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            this.btnTimKiem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);

            // btnLamMoi
            this.btnLamMoi.BackColor = System.Drawing.Color.White;
            this.btnLamMoi.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(209, 213, 219);
            this.btnLamMoi.FlatAppearance.BorderSize = 1;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLamMoi.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnLamMoi.Location = new System.Drawing.Point(697, 10);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(110, 32);
            this.btnLamMoi.TabIndex = 3;
            this.btnLamMoi.Text = "↺  Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);

            // ── grpThongTin ──────────────────────────────────────────────
            this.grpThongTin.BackColor = System.Drawing.Color.White;
            this.grpThongTin.Controls.Add(this.lblMaKH);
            this.grpThongTin.Controls.Add(this.txtMaKH);
            this.grpThongTin.Controls.Add(this.lblNgaySinh);
            this.grpThongTin.Controls.Add(this.dtpNgaySinh);
            this.grpThongTin.Controls.Add(this.lblHoTen);
            this.grpThongTin.Controls.Add(this.txtHoTen);
            this.grpThongTin.Controls.Add(this.lblSDT);
            this.grpThongTin.Controls.Add(this.txtSDT);
            this.grpThongTin.Controls.Add(this.lblGioiTinh);
            this.grpThongTin.Controls.Add(this.cboGioiTinh);
            this.grpThongTin.Controls.Add(this.lblDiaChi);
            this.grpThongTin.Controls.Add(this.txtDiaChi);
            this.grpThongTin.Controls.Add(this.lblHang);
            this.grpThongTin.Controls.Add(this.cboHangThanhVien);
            this.grpThongTin.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpThongTin.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpThongTin.ForeColor = System.Drawing.Color.FromArgb(30, 58, 138);
            this.grpThongTin.Location = new System.Drawing.Point(14, 64);
            this.grpThongTin.Name = "grpThongTin";
            this.grpThongTin.Padding = new System.Windows.Forms.Padding(12, 6, 12, 10);
            this.grpThongTin.Size = new System.Drawing.Size(981, 175);
            this.grpThongTin.TabIndex = 1;
            this.grpThongTin.TabStop = false;
            this.grpThongTin.Text = "Thông tin khách hàng";

            int fH = 28;
            System.Drawing.Font fFont = new System.Drawing.Font("Segoe UI", 9.5F);
            System.Drawing.Font lFont = new System.Drawing.Font("Segoe UI", 9.5F);

            // Row 1: MaKH (trai) | NgaySinh (phai)
            this.lblMaKH.AutoSize = true; this.lblMaKH.Font = lFont; this.lblMaKH.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblMaKH.Location = new System.Drawing.Point(14, 26); this.lblMaKH.Name = "lblMaKH"; this.lblMaKH.Text = "Mã khách hàng:";
            this.txtMaKH.BackColor = System.Drawing.Color.FromArgb(249, 250, 251); this.txtMaKH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMaKH.Font = fFont; this.txtMaKH.Location = new System.Drawing.Point(120, 23); this.txtMaKH.Name = "txtMaKH"; this.txtMaKH.ReadOnly = true; this.txtMaKH.Size = new System.Drawing.Size(280, fH);

            this.lblNgaySinh.AutoSize = true; this.lblNgaySinh.Font = lFont; this.lblNgaySinh.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblNgaySinh.Location = new System.Drawing.Point(480, 26); this.lblNgaySinh.Name = "lblNgaySinh"; this.lblNgaySinh.Text = "Ngày sinh:";
            this.dtpNgaySinh.CustomFormat = "dd/MM/yyyy"; this.dtpNgaySinh.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgaySinh.Font = fFont; this.dtpNgaySinh.Location = new System.Drawing.Point(580, 23); this.dtpNgaySinh.Name = "dtpNgaySinh"; this.dtpNgaySinh.Size = new System.Drawing.Size(280, fH);

            // Row 2: HoTen (trai) | SDT (phai)
            this.lblHoTen.AutoSize = true; this.lblHoTen.Font = lFont; this.lblHoTen.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblHoTen.Location = new System.Drawing.Point(14, 61); this.lblHoTen.Name = "lblHoTen"; this.lblHoTen.Text = "Họ tên:";
            this.txtHoTen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.txtHoTen.Font = fFont;
            this.txtHoTen.Location = new System.Drawing.Point(120, 58); this.txtHoTen.Name = "txtHoTen"; this.txtHoTen.Size = new System.Drawing.Size(280, fH);

            this.lblSDT.AutoSize = true; this.lblSDT.Font = lFont; this.lblSDT.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblSDT.Location = new System.Drawing.Point(480, 61); this.lblSDT.Name = "lblSDT"; this.lblSDT.Text = "Số điện thoại:";
            this.txtSDT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.txtSDT.Font = fFont;
            this.txtSDT.Location = new System.Drawing.Point(580, 58); this.txtSDT.MaxLength = 11; this.txtSDT.Name = "txtSDT"; this.txtSDT.Size = new System.Drawing.Size(280, fH);
            this.txtSDT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSDT_KeyPress);

            // Row 3: GioiTinh (trai) | HangThanhVien (phai)
            this.lblGioiTinh.AutoSize = true; this.lblGioiTinh.Font = lFont; this.lblGioiTinh.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblGioiTinh.Location = new System.Drawing.Point(14, 96); this.lblGioiTinh.Name = "lblGioiTinh"; this.lblGioiTinh.Text = "Giới tính:";
            this.cboGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; this.cboGioiTinh.Font = fFont; this.cboGioiTinh.FormattingEnabled = true;
            this.cboGioiTinh.Location = new System.Drawing.Point(120, 93); this.cboGioiTinh.Name = "cboGioiTinh"; this.cboGioiTinh.Size = new System.Drawing.Size(280, fH);

            this.lblHang.AutoSize = true; this.lblHang.Font = lFont; this.lblHang.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblHang.Location = new System.Drawing.Point(480, 96); this.lblHang.Name = "lblHang"; this.lblHang.Text = "Hạng thành viên:";
            this.cboHangThanhVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; this.cboHangThanhVien.Font = fFont; this.cboHangThanhVien.FormattingEnabled = true;
            this.cboHangThanhVien.Location = new System.Drawing.Point(580, 93); this.cboHangThanhVien.Name = "cboHangThanhVien"; this.cboHangThanhVien.Size = new System.Drawing.Size(280, fH);

            // Row 4: DiaChi (trai sang phai)
            this.lblDiaChi.AutoSize = true; this.lblDiaChi.Font = lFont; this.lblDiaChi.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblDiaChi.Location = new System.Drawing.Point(14, 131); this.lblDiaChi.Name = "lblDiaChi"; this.lblDiaChi.Text = "Địa chỉ:";
            this.txtDiaChi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle; this.txtDiaChi.Font = fFont;
            this.txtDiaChi.Location = new System.Drawing.Point(120, 128); this.txtDiaChi.Name = "txtDiaChi"; this.txtDiaChi.Size = new System.Drawing.Size(740, fH);

            // ── pnlButtons ───────────────────────────────────────────────
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

            System.Drawing.Font btnFont = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            int bW = 100; int bH = 38;

            // btnThem
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnThem.FlatAppearance.BorderSize = 0;
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = btnFont;
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(10, 8);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(bW, bH);
            this.btnThem.TabIndex = 0;
            this.btnThem.Text = "+  Thêm";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);

            // btnSua
            this.btnSua.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnSua.FlatAppearance.BorderSize = 0;
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.Font = btnFont;
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.Location = new System.Drawing.Point(120, 8);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(bW, bH);
            this.btnSua.TabIndex = 1;
            this.btnSua.Text = "✏  Sửa";
            this.btnSua.UseVisualStyleBackColor = false;
            this.btnSua.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);

            // btnXoa
            this.btnXoa.BackColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnXoa.FlatAppearance.BorderSize = 0;
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Font = btnFont;
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(230, 8);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(bW, bH);
            this.btnXoa.TabIndex = 2;
            this.btnXoa.Text = "🗑  Xóa";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);

            // btnLuu
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = btnFont;
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(340, 8);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(bW, bH);
            this.btnLuu.TabIndex = 3;
            this.btnLuu.Text = "💾  Lưu";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.Visible = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);

            // btnHuy
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.btnHuy.FlatAppearance.BorderSize = 0;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = btnFont;
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(450, 8);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(bW, bH);
            this.btnHuy.TabIndex = 4;
            this.btnHuy.Text = "✕  Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.Visible = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);

            // btnThoat
            this.btnThoat.BackColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.btnThoat.FlatAppearance.BorderSize = 0;
            this.btnThoat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThoat.Font = btnFont;
            this.btnThoat.ForeColor = System.Drawing.Color.White;
            this.btnThoat.Location = new System.Drawing.Point(560, 8);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(bW, bH);
            this.btnThoat.TabIndex = 5;
            this.btnThoat.Text = "🚪  Thoát";
            this.btnThoat.UseVisualStyleBackColor = false;
            this.btnThoat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThoat.Visible = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);

            // ── dgvKhachHang ─────────────────────────────────────────────
            this.dgvKhachHang.AllowUserToAddRows = false;
            this.dgvKhachHang.AllowUserToDeleteRows = false;
            this.dgvKhachHang.AllowUserToResizeRows = false;
            this.dgvKhachHang.AllowUserToResizeColumns = false;
            this.dgvKhachHang.AllowUserToOrderColumns = false;
            this.dgvKhachHang.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvKhachHang.BackgroundColor = System.Drawing.Color.White;
            this.dgvKhachHang.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvKhachHang.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.dgvKhachHang.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.dgvKhachHang.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(30, 58, 138);
            this.dgvKhachHang.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.dgvKhachHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKhachHang.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvKhachHang.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(219, 234, 254);
            this.dgvKhachHang.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(30, 64, 175);
            this.dgvKhachHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKhachHang.GridColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.dgvKhachHang.Location = new System.Drawing.Point(14, 244);
            this.dgvKhachHang.Name = "dgvKhachHang";
            this.dgvKhachHang.ReadOnly = true;
            this.dgvKhachHang.RowHeadersVisible = false;
            this.dgvKhachHang.RowTemplate.Height = 36;
            this.dgvKhachHang.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvKhachHang.Size = new System.Drawing.Size(981, 404);
            this.dgvKhachHang.TabIndex = 2;
            this.dgvKhachHang.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKhachHang_CellClick);

            // ── frmKhachHang ─────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 247);
            this.ClientSize = new System.Drawing.Size(1184, 700);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.sidebarControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.MinimumSize = new System.Drawing.Size(1100, 640);
            this.Name = "frmKhachHang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý khách hàng";

            this.pnlMain.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.grpThongTin.ResumeLayout(false);
            this.grpThongTin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhachHang)).EndInit();
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
        private System.Windows.Forms.GroupBox grpThongTin;
        private System.Windows.Forms.Label lblMaKH;
        private System.Windows.Forms.TextBox txtMaKH;
        private System.Windows.Forms.Label lblNgaySinh;
        private System.Windows.Forms.DateTimePicker dtpNgaySinh;
        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Label lblGioiTinh;
        private System.Windows.Forms.ComboBox cboGioiTinh;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblHang;
        private System.Windows.Forms.ComboBox cboHangThanhVien;
        private System.Windows.Forms.DataGridView dgvKhachHang;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnThoat;
    }
}

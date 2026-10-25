namespace QuanLyBanLaptop.HoaDon
{
    partial class frmNhapHang
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.sidebarControl1 = new QuanLyBanLaptop.SidebarControl();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnXacNhanNhapHang = new System.Windows.Forms.Button();
            this.lblTongTienSo = new System.Windows.Forms.Label();
            this.lblTongTienNhap = new System.Windows.Forms.Label();
            this.dgvDanhSachNhap = new System.Windows.Forms.DataGridView();
            this.colMa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGiaNhap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDanhSachSp = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnXoaKhoiPhieu = new System.Windows.Forms.Button();
            this.btnThemVaoPhieu = new System.Windows.Forms.Button();
            this.pnlTopForms = new System.Windows.Forms.Panel();
            this.grpChonLaptop = new System.Windows.Forms.GroupBox();
            this.txtDonGiaNhap = new System.Windows.Forms.TextBox();
            this.lblDonGiaNhap = new System.Windows.Forms.Label();
            this.txtDonSoLuong = new System.Windows.Forms.TextBox();
            this.lblDonSoLuong = new System.Windows.Forms.Label();
            this.btnBrowseLaptop = new System.Windows.Forms.Button();
            this.cboLaptop = new System.Windows.Forms.ComboBox();
            this.grpThongTinPhieu = new System.Windows.Forms.GroupBox();
            this.cboNhanVien = new System.Windows.Forms.ComboBox();
            this.lblNhanVien = new System.Windows.Forms.Label();
            this.dtpNgayNhap = new System.Windows.Forms.DateTimePicker();
            this.lblNgayNhap = new System.Windows.Forms.Label();
            this.cboNhaCungCap = new System.Windows.Forms.ComboBox();
            this.lblNhaCungCap = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachNhap)).BeginInit();
            this.pnlActions.SuspendLayout();
            this.pnlTopForms.SuspendLayout();
            this.grpChonLaptop.SuspendLayout();
            this.grpThongTinPhieu.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidebarControl1
            // 
            this.sidebarControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.sidebarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarControl1.Location = new System.Drawing.Point(0, 0);
            this.sidebarControl1.Name = "sidebarControl1";
            this.sidebarControl1.SelectedMenu = QuanLyBanLaptop.Classes.NavigationMenu.NhapHang;
            this.sidebarControl1.Size = new System.Drawing.Size(230, 661);
            this.sidebarControl1.TabIndex = 0;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.pnlMain.Controls.Add(this.pnlBottom);
            this.pnlMain.Controls.Add(this.dgvDanhSachNhap);
            this.pnlMain.Controls.Add(this.lblDanhSachSp);
            this.pnlMain.Controls.Add(this.pnlActions);
            this.pnlMain.Controls.Add(this.pnlTopForms);
            this.pnlMain.Controls.Add(this.pnlHeader);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(230, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(16);
            this.pnlMain.Size = new System.Drawing.Size(959, 661);
            this.pnlMain.TabIndex = 1;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnXacNhanNhapHang);
            this.pnlBottom.Controls.Add(this.lblTongTienSo);
            this.pnlBottom.Controls.Add(this.lblTongTienNhap);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(16, 595);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(927, 50);
            this.pnlBottom.TabIndex = 5;
            // 
            // btnXacNhanNhapHang
            // 
            this.btnXacNhanNhapHang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXacNhanNhapHang.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnXacNhanNhapHang.FlatAppearance.BorderSize = 0;
            this.btnXacNhanNhapHang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXacNhanNhapHang.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXacNhanNhapHang.ForeColor = System.Drawing.Color.White;
            this.btnXacNhanNhapHang.Location = new System.Drawing.Point(747, 10);
            this.btnXacNhanNhapHang.Name = "btnXacNhanNhapHang";
            this.btnXacNhanNhapHang.Size = new System.Drawing.Size(180, 36);
            this.btnXacNhanNhapHang.TabIndex = 2;
            this.btnXacNhanNhapHang.Text = "XÁC NHẬN NHẬP HÀNG";
            this.btnXacNhanNhapHang.UseVisualStyleBackColor = false;
            // 
            // lblTongTienSo
            // 
            this.lblTongTienSo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTongTienSo.AutoSize = true;
            this.lblTongTienSo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongTienSo.ForeColor = System.Drawing.Color.Black;
            this.lblTongTienSo.Location = new System.Drawing.Point(650, 18);
            this.lblTongTienSo.Name = "lblTongTienSo";
            this.lblTongTienSo.Size = new System.Drawing.Size(83, 25);
            this.lblTongTienSo.TabIndex = 1;
            this.lblTongTienSo.Text = "130.000";
            // 
            // lblTongTienNhap
            // 
            this.lblTongTienNhap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTongTienNhap.AutoSize = true;
            this.lblTongTienNhap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongTienNhap.Location = new System.Drawing.Point(530, 18);
            this.lblTongTienNhap.Name = "lblTongTienNhap";
            this.lblTongTienNhap.Size = new System.Drawing.Size(142, 23);
            this.lblTongTienNhap.TabIndex = 0;
            this.lblTongTienNhap.Text = "Tổng tiền nhập :";
            // 
            // dgvDanhSachNhap
            // 
            this.dgvDanhSachNhap.AllowUserToAddRows = false;
            this.dgvDanhSachNhap.BackgroundColor = System.Drawing.Color.Silver;
            this.dgvDanhSachNhap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDanhSachNhap.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMa,
            this.colTen,
            this.colSoLuong,
            this.colDonGiaNhap,
            this.colThanhTien});
            this.dgvDanhSachNhap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDanhSachNhap.Location = new System.Drawing.Point(16, 320);
            this.dgvDanhSachNhap.Name = "dgvDanhSachNhap";
            this.dgvDanhSachNhap.ReadOnly = true;
            this.dgvDanhSachNhap.RowHeadersWidth = 30;
            this.dgvDanhSachNhap.Size = new System.Drawing.Size(927, 325);
            this.dgvDanhSachNhap.TabIndex = 4;
            // 
            // colMa
            // 
            this.colMa.HeaderText = "Mã";
            this.colMa.MinimumWidth = 6;
            this.colMa.Name = "colMa";
            this.colMa.ReadOnly = true;
            this.colMa.Width = 90;
            // 
            // colTen
            // 
            this.colTen.HeaderText = "Tên";
            this.colTen.MinimumWidth = 6;
            this.colTen.Name = "colTen";
            this.colTen.ReadOnly = true;
            this.colTen.Width = 220;
            // 
            // colSoLuong
            // 
            this.colSoLuong.HeaderText = "Số lượng";
            this.colSoLuong.MinimumWidth = 6;
            this.colSoLuong.Name = "colSoLuong";
            this.colSoLuong.ReadOnly = true;
            // 
            // colDonGiaNhap
            // 
            this.colDonGiaNhap.HeaderText = "Đơn giá nhập";
            this.colDonGiaNhap.MinimumWidth = 6;
            this.colDonGiaNhap.Name = "colDonGiaNhap";
            this.colDonGiaNhap.ReadOnly = true;
            this.colDonGiaNhap.Width = 130;
            // 
            // colThanhTien
            // 
            this.colThanhTien.HeaderText = "Thành tiền";
            this.colThanhTien.MinimumWidth = 6;
            this.colThanhTien.Name = "colThanhTien";
            this.colThanhTien.ReadOnly = true;
            this.colThanhTien.Width = 130;
            // 
            // lblDanhSachSp
            // 
            this.lblDanhSachSp.AutoSize = true;
            this.lblDanhSachSp.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDanhSachSp.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDanhSachSp.Location = new System.Drawing.Point(16, 285);
            this.lblDanhSachSp.Name = "lblDanhSachSp";
            this.lblDanhSachSp.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.lblDanhSachSp.Size = new System.Drawing.Size(235, 35);
            this.lblDanhSachSp.TabIndex = 3;
            this.lblDanhSachSp.Text = "Danh sách sản phẩm nhập";
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnXoaKhoiPhieu);
            this.pnlActions.Controls.Add(this.btnThemVaoPhieu);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActions.Location = new System.Drawing.Point(16, 237);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(927, 48);
            this.pnlActions.TabIndex = 2;
            // 
            // btnXoaKhoiPhieu
            // 
            this.btnXoaKhoiPhieu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(83)))), ((int)(((byte)(79)))));
            this.btnXoaKhoiPhieu.FlatAppearance.BorderSize = 0;
            this.btnXoaKhoiPhieu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaKhoiPhieu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoaKhoiPhieu.ForeColor = System.Drawing.Color.White;
            this.btnXoaKhoiPhieu.Location = new System.Drawing.Point(595, 7);
            this.btnXoaKhoiPhieu.Name = "btnXoaKhoiPhieu";
            this.btnXoaKhoiPhieu.Size = new System.Drawing.Size(140, 32);
            this.btnXoaKhoiPhieu.TabIndex = 1;
            this.btnXoaKhoiPhieu.Text = "Xóa khỏi phiếu";
            this.btnXoaKhoiPhieu.UseVisualStyleBackColor = false;
            // 
            // btnThemVaoPhieu
            // 
            this.btnThemVaoPhieu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(117)))), ((int)(((byte)(216)))));
            this.btnThemVaoPhieu.FlatAppearance.BorderSize = 0;
            this.btnThemVaoPhieu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemVaoPhieu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemVaoPhieu.ForeColor = System.Drawing.Color.White;
            this.btnThemVaoPhieu.Location = new System.Drawing.Point(445, 7);
            this.btnThemVaoPhieu.Name = "btnThemVaoPhieu";
            this.btnThemVaoPhieu.Size = new System.Drawing.Size(140, 32);
            this.btnThemVaoPhieu.TabIndex = 0;
            this.btnThemVaoPhieu.Text = "Thêm vào phiếu";
            this.btnThemVaoPhieu.UseVisualStyleBackColor = false;
            // 
            // pnlTopForms
            // 
            this.pnlTopForms.Controls.Add(this.grpChonLaptop);
            this.pnlTopForms.Controls.Add(this.grpThongTinPhieu);
            this.pnlTopForms.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopForms.Location = new System.Drawing.Point(16, 56);
            this.pnlTopForms.Name = "pnlTopForms";
            this.pnlTopForms.Size = new System.Drawing.Size(927, 181);
            this.pnlTopForms.TabIndex = 1;
            // 
            // grpChonLaptop
            // 
            this.grpChonLaptop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpChonLaptop.Controls.Add(this.txtDonGiaNhap);
            this.grpChonLaptop.Controls.Add(this.lblDonGiaNhap);
            this.grpChonLaptop.Controls.Add(this.txtDonSoLuong);
            this.grpChonLaptop.Controls.Add(this.lblDonSoLuong);
            this.grpChonLaptop.Controls.Add(this.btnBrowseLaptop);
            this.grpChonLaptop.Controls.Add(this.cboLaptop);
            this.grpChonLaptop.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpChonLaptop.Location = new System.Drawing.Point(365, 8);
            this.grpChonLaptop.Name = "grpChonLaptop";
            this.grpChonLaptop.Size = new System.Drawing.Size(540, 160);
            this.grpChonLaptop.TabIndex = 1;
            this.grpChonLaptop.TabStop = false;
            this.grpChonLaptop.Text = "Chọn Laptop && Nhập số lượng/đơn giá nhập";
            // 
            // txtDonGiaNhap
            // 
            this.txtDonGiaNhap.Location = new System.Drawing.Point(130, 110);
            this.txtDonGiaNhap.Name = "txtDonGiaNhap";
            this.txtDonGiaNhap.Size = new System.Drawing.Size(180, 29);
            this.txtDonGiaNhap.TabIndex = 5;
            // 
            // lblDonGiaNhap
            // 
            this.lblDonGiaNhap.AutoSize = true;
            this.lblDonGiaNhap.Location = new System.Drawing.Point(15, 113);
            this.lblDonGiaNhap.Name = "lblDonGiaNhap";
            this.lblDonGiaNhap.Size = new System.Drawing.Size(114, 23);
            this.lblDonGiaNhap.TabIndex = 4;
            this.lblDonGiaNhap.Text = "Đơn giá nhập";
            // 
            // txtDonSoLuong
            // 
            this.txtDonSoLuong.Location = new System.Drawing.Point(130, 72);
            this.txtDonSoLuong.Name = "txtDonSoLuong";
            this.txtDonSoLuong.Size = new System.Drawing.Size(180, 29);
            this.txtDonSoLuong.TabIndex = 3;
            this.txtDonSoLuong.Text = "0";
            // 
            // lblDonSoLuong
            // 
            this.lblDonSoLuong.AutoSize = true;
            this.lblDonSoLuong.Location = new System.Drawing.Point(15, 75);
            this.lblDonSoLuong.Name = "lblDonSoLuong";
            this.lblDonSoLuong.Size = new System.Drawing.Size(113, 23);
            this.lblDonSoLuong.TabIndex = 2;
            this.lblDonSoLuong.Text = "Đơn số lượng";
            // 
            // btnBrowseLaptop
            // 
            this.btnBrowseLaptop.Location = new System.Drawing.Point(320, 31);
            this.btnBrowseLaptop.Name = "btnBrowseLaptop";
            this.btnBrowseLaptop.Size = new System.Drawing.Size(40, 26);
            this.btnBrowseLaptop.TabIndex = 1;
            this.btnBrowseLaptop.Text = "...";
            this.btnBrowseLaptop.UseVisualStyleBackColor = true;
            // 
            // cboLaptop
            // 
            this.cboLaptop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLaptop.FormattingEnabled = true;
            this.cboLaptop.Location = new System.Drawing.Point(18, 32);
            this.cboLaptop.Name = "cboLaptop";
            this.cboLaptop.Size = new System.Drawing.Size(292, 29);
            this.cboLaptop.TabIndex = 0;
            // 
            // grpThongTinPhieu
            // 
            this.grpThongTinPhieu.Controls.Add(this.cboNhanVien);
            this.grpThongTinPhieu.Controls.Add(this.lblNhanVien);
            this.grpThongTinPhieu.Controls.Add(this.dtpNgayNhap);
            this.grpThongTinPhieu.Controls.Add(this.lblNgayNhap);
            this.grpThongTinPhieu.Controls.Add(this.cboNhaCungCap);
            this.grpThongTinPhieu.Controls.Add(this.lblNhaCungCap);
            this.grpThongTinPhieu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpThongTinPhieu.Location = new System.Drawing.Point(0, 8);
            this.grpThongTinPhieu.Name = "grpThongTinPhieu";
            this.grpThongTinPhieu.Size = new System.Drawing.Size(350, 160);
            this.grpThongTinPhieu.TabIndex = 0;
            this.grpThongTinPhieu.TabStop = false;
            this.grpThongTinPhieu.Text = "Thông tin phiếu nhập";
            // 
            // cboNhanVien
            // 
            this.cboNhanVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNhanVien.FormattingEnabled = true;
            this.cboNhanVien.Location = new System.Drawing.Point(120, 110);
            this.cboNhanVien.Name = "cboNhanVien";
            this.cboNhanVien.Size = new System.Drawing.Size(210, 29);
            this.cboNhanVien.TabIndex = 5;
            // 
            // lblNhanVien
            // 
            this.lblNhanVien.AutoSize = true;
            this.lblNhanVien.Location = new System.Drawing.Point(15, 113);
            this.lblNhanVien.Name = "lblNhanVien";
            this.lblNhanVien.Size = new System.Drawing.Size(88, 23);
            this.lblNhanVien.TabIndex = 4;
            this.lblNhanVien.Text = "Nhân viên";
            // 
            // dtpNgayNhap
            // 
            this.dtpNgayNhap.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayNhap.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayNhap.Location = new System.Drawing.Point(120, 72);
            this.dtpNgayNhap.Name = "dtpNgayNhap";
            this.dtpNgayNhap.Size = new System.Drawing.Size(210, 29);
            this.dtpNgayNhap.TabIndex = 3;
            // 
            // lblNgayNhap
            // 
            this.lblNgayNhap.AutoSize = true;
            this.lblNgayNhap.Location = new System.Drawing.Point(15, 75);
            this.lblNgayNhap.Name = "lblNgayNhap";
            this.lblNgayNhap.Size = new System.Drawing.Size(94, 23);
            this.lblNgayNhap.TabIndex = 2;
            this.lblNgayNhap.Text = "Ngày nhập";
            // 
            // cboNhaCungCap
            // 
            this.cboNhaCungCap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNhaCungCap.FormattingEnabled = true;
            this.cboNhaCungCap.Location = new System.Drawing.Point(120, 32);
            this.cboNhaCungCap.Name = "cboNhaCungCap";
            this.cboNhaCungCap.Size = new System.Drawing.Size(210, 29);
            this.cboNhaCungCap.TabIndex = 1;
            // 
            // lblNhaCungCap
            // 
            this.lblNhaCungCap.AutoSize = true;
            this.lblNhaCungCap.Location = new System.Drawing.Point(15, 35);
            this.lblNhaCungCap.Name = "lblNhaCungCap";
            this.lblNhaCungCap.Size = new System.Drawing.Size(117, 23);
            this.lblNhaCungCap.TabIndex = 0;
            this.lblNhaCungCap.Text = "Nhà cung cấp";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(16, 16);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(927, 40);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.lblHeaderTitle.Location = new System.Drawing.Point(3, 7);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(211, 32);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "Trang Nhập hàng";
            // 
            // frmNhapHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 661);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.sidebarControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmNhapHang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TRANG NHẬP HÀNG";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachNhap)).EndInit();
            this.pnlActions.ResumeLayout(false);
            this.pnlTopForms.ResumeLayout(false);
            this.grpChonLaptop.ResumeLayout(false);
            this.grpChonLaptop.PerformLayout();
            this.grpThongTinPhieu.ResumeLayout(false);
            this.grpThongTinPhieu.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SidebarControl sidebarControl1;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Panel pnlTopForms;
        private System.Windows.Forms.GroupBox grpThongTinPhieu;
        private System.Windows.Forms.ComboBox cboNhaCungCap;
        private System.Windows.Forms.Label lblNhaCungCap;
        private System.Windows.Forms.DateTimePicker dtpNgayNhap;
        private System.Windows.Forms.Label lblNgayNhap;
        private System.Windows.Forms.ComboBox cboNhanVien;
        private System.Windows.Forms.Label lblNhanVien;
        private System.Windows.Forms.GroupBox grpChonLaptop;
        private System.Windows.Forms.ComboBox cboLaptop;
        private System.Windows.Forms.Button btnBrowseLaptop;
        private System.Windows.Forms.Label lblDonSoLuong;
        private System.Windows.Forms.TextBox txtDonSoLuong;
        private System.Windows.Forms.Label lblDonGiaNhap;
        private System.Windows.Forms.TextBox txtDonGiaNhap;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnThemVaoPhieu;
        private System.Windows.Forms.Button btnXoaKhoiPhieu;
        private System.Windows.Forms.Label lblDanhSachSp;
        private System.Windows.Forms.DataGridView dgvDanhSachNhap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMa;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGiaNhap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhTien;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblTongTienNhap;
        private System.Windows.Forms.Label lblTongTienSo;
        private System.Windows.Forms.Button btnXacNhanNhapHang;
    }
}
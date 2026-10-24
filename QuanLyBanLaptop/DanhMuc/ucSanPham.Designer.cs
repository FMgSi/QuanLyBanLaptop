namespace QuanLyBanLaptop.DanhMuc
{
    partial class ucSanPham
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.dgvLaptop = new System.Windows.Forms.DataGridView();
            this.colMaLaptop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenLaptop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNhaCungCap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCPU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRAM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSSD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuongTon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.cboTieuChiTimKiem = new System.Windows.Forms.ComboBox();
            this.lblTimKiem = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnThem = new System.Windows.Forms.Button();
            this.grpThongTin = new System.Windows.Forms.GroupBox();
            this.btnQuanLyNCC = new System.Windows.Forms.Button();
            this.btnQuanLyLoai = new System.Windows.Forms.Button();
            this.cboLoai = new System.Windows.Forms.ComboBox();
            this.lblLoai = new System.Windows.Forms.Label();
            this.cboNhaCungCap = new System.Windows.Forms.ComboBox();
            this.lblNhaCungCap = new System.Windows.Forms.Label();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.txtSSD = new System.Windows.Forms.TextBox();
            this.lblSSD = new System.Windows.Forms.Label();
            this.txtRAM = new System.Windows.Forms.TextBox();
            this.lblRAM = new System.Windows.Forms.Label();
            this.txtCPU = new System.Windows.Forms.TextBox();
            this.lblCPU = new System.Windows.Forms.Label();
            this.txtTenLaptop = new System.Windows.Forms.TextBox();
            this.lblTenLaptop = new System.Windows.Forms.Label();
            this.txtMa = new System.Windows.Forms.TextBox();
            this.lblMa = new System.Windows.Forms.Label();
            this.pnlTopHeader = new System.Windows.Forms.Panel();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLaptop)).BeginInit();
            this.pnlActions.SuspendLayout();
            this.grpThongTin.SuspendLayout();
            this.pnlTopHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.pnlMain.Controls.Add(this.dgvLaptop);
            this.pnlMain.Controls.Add(this.pnlActions);
            this.pnlMain.Controls.Add(this.grpThongTin);
            this.pnlMain.Controls.Add(this.pnlTopHeader);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(16);
            this.pnlMain.Size = new System.Drawing.Size(959, 661);
            this.pnlMain.TabIndex = 0;
            // 
            // dgvLaptop
            // 
            this.dgvLaptop.AllowUserToAddRows = false;
            this.dgvLaptop.AllowUserToDeleteRows = false;
            this.dgvLaptop.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLaptop.BackgroundColor = System.Drawing.Color.White;
            this.dgvLaptop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLaptop.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLaptop.ColumnHeadersHeight = 35;
            this.dgvLaptop.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaLaptop,
            this.colTenLaptop,
            this.colLoai,
            this.colNhaCungCap,
            this.colCPU,
            this.colRAM,
            this.colSSD,
            this.colDonGia,
            this.colSoLuongTon});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(101)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLaptop.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLaptop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLaptop.EnableHeadersVisualStyles = false;
            this.dgvLaptop.Location = new System.Drawing.Point(16, 260);
            this.dgvLaptop.MultiSelect = false;
            this.dgvLaptop.Name = "dgvLaptop";
            this.dgvLaptop.ReadOnly = true;
            this.dgvLaptop.RowHeadersWidth = 30;
            this.dgvLaptop.RowTemplate.Height = 28;
            this.dgvLaptop.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLaptop.Size = new System.Drawing.Size(927, 385);
            this.dgvLaptop.TabIndex = 3;
            this.dgvLaptop.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLaptop_CellClick);
            // 
            // colMaLaptop
            // 
            this.colMaLaptop.FillWeight = 65F;
            this.colMaLaptop.HeaderText = "Mã Laptop";
            this.colMaLaptop.Name = "colMaLaptop";
            this.colMaLaptop.ReadOnly = true;
            // 
            // colTenLaptop
            // 
            this.colTenLaptop.FillWeight = 150F;
            this.colTenLaptop.HeaderText = "Tên Laptop";
            this.colTenLaptop.Name = "colTenLaptop";
            this.colTenLaptop.ReadOnly = true;
            // 
            // colLoai
            // 
            this.colLoai.FillWeight = 75F;
            this.colLoai.HeaderText = "Phân Loại";
            this.colLoai.Name = "colLoai";
            this.colLoai.ReadOnly = true;
            // 
            // colNhaCungCap
            // 
            this.colNhaCungCap.FillWeight = 120F;
            this.colNhaCungCap.HeaderText = "Nhà Cung Cấp";
            this.colNhaCungCap.Name = "colNhaCungCap";
            this.colNhaCungCap.ReadOnly = true;
            // 
            // colCPU
            // 
            this.colCPU.FillWeight = 90F;
            this.colCPU.HeaderText = "CPU";
            this.colCPU.Name = "colCPU";
            this.colCPU.ReadOnly = true;
            // 
            // colRAM
            // 
            this.colRAM.FillWeight = 60F;
            this.colRAM.HeaderText = "RAM";
            this.colRAM.Name = "colRAM";
            this.colRAM.ReadOnly = true;
            // 
            // colSSD
            // 
            this.colSSD.FillWeight = 60F;
            this.colSSD.HeaderText = "Ổ Cứng";
            this.colSSD.Name = "colSSD";
            this.colSSD.ReadOnly = true;
            // 
            // colDonGia
            // 
            this.colDonGia.FillWeight = 85F;
            this.colDonGia.HeaderText = "Đơn Giá (VNĐ)";
            this.colDonGia.Name = "colDonGia";
            this.colDonGia.ReadOnly = true;
            // 
            // colSoLuongTon
            // 
            this.colSoLuongTon.FillWeight = 60F;
            this.colSoLuongTon.HeaderText = "Tồn Kho";
            this.colSoLuongTon.Name = "colSoLuongTon";
            this.colSoLuongTon.ReadOnly = true;
            // 
            // pnlActions
            // 
            this.pnlActions.BackColor = System.Drawing.Color.White;
            this.pnlActions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlActions.Controls.Add(this.cboTieuChiTimKiem);
            this.pnlActions.Controls.Add(this.lblTimKiem);
            this.pnlActions.Controls.Add(this.txtTimKiem);
            this.pnlActions.Controls.Add(this.btnLamMoi);
            this.pnlActions.Controls.Add(this.btnXoa);
            this.pnlActions.Controls.Add(this.btnSua);
            this.pnlActions.Controls.Add(this.btnThem);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActions.Location = new System.Drawing.Point(16, 204);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(927, 56);
            this.pnlActions.TabIndex = 2;
            // 
            // cboTieuChiTimKiem
            // 
            this.cboTieuChiTimKiem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTieuChiTimKiem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTieuChiTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboTieuChiTimKiem.FormattingEnabled = true;
            this.cboTieuChiTimKiem.Location = new System.Drawing.Point(542, 14);
            this.cboTieuChiTimKiem.Name = "cboTieuChiTimKiem";
            this.cboTieuChiTimKiem.Size = new System.Drawing.Size(125, 24);
            this.cboTieuChiTimKiem.TabIndex = 6;
            this.cboTieuChiTimKiem.SelectedIndexChanged += new System.EventHandler(this.cboTieuChiTimKiem_SelectedIndexChanged);
            // 
            // lblTimKiem
            // 
            this.lblTimKiem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.lblTimKiem.Location = new System.Drawing.Point(470, 18);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(68, 17);
            this.lblTimKiem.TabIndex = 5;
            this.lblTimKiem.Text = "Tìm kiếm:";
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTimKiem.Location = new System.Drawing.Point(674, 14);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(236, 25);
            this.txtTimKiem.TabIndex = 4;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnLamMoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLamMoi.FlatAppearance.BorderSize = 0;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(366, 10);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(95, 34);
            this.btnLamMoi.TabIndex = 3;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnXoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoa.FlatAppearance.BorderSize = 0;
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(248, 10);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(95, 34);
            this.btnXoa.TabIndex = 2;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnSua
            // 
            this.btnSua.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(159)))), ((int)(((byte)(28)))));
            this.btnSua.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSua.FlatAppearance.BorderSize = 0;
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.Location = new System.Drawing.Point(130, 10);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(95, 34);
            this.btnSua.TabIndex = 1;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = false;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnThem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThem.FlatAppearance.BorderSize = 0;
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(12, 10);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(95, 34);
            this.btnThem.TabIndex = 0;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // grpThongTin
            // 
            this.grpThongTin.BackColor = System.Drawing.Color.White;
            this.grpThongTin.Controls.Add(this.btnQuanLyNCC);
            this.grpThongTin.Controls.Add(this.btnQuanLyLoai);
            this.grpThongTin.Controls.Add(this.cboLoai);
            this.grpThongTin.Controls.Add(this.lblLoai);
            this.grpThongTin.Controls.Add(this.cboNhaCungCap);
            this.grpThongTin.Controls.Add(this.lblNhaCungCap);
            this.grpThongTin.Controls.Add(this.txtDonGia);
            this.grpThongTin.Controls.Add(this.lblDonGia);
            this.grpThongTin.Controls.Add(this.txtSoLuong);
            this.grpThongTin.Controls.Add(this.lblSoLuong);
            this.grpThongTin.Controls.Add(this.txtSSD);
            this.grpThongTin.Controls.Add(this.lblSSD);
            this.grpThongTin.Controls.Add(this.txtRAM);
            this.grpThongTin.Controls.Add(this.lblRAM);
            this.grpThongTin.Controls.Add(this.txtCPU);
            this.grpThongTin.Controls.Add(this.lblCPU);
            this.grpThongTin.Controls.Add(this.txtTenLaptop);
            this.grpThongTin.Controls.Add(this.lblTenLaptop);
            this.grpThongTin.Controls.Add(this.txtMa);
            this.grpThongTin.Controls.Add(this.lblMa);
            this.grpThongTin.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpThongTin.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpThongTin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.grpThongTin.Location = new System.Drawing.Point(16, 56);
            this.grpThongTin.Name = "grpThongTin";
            this.grpThongTin.Size = new System.Drawing.Size(927, 148);
            this.grpThongTin.TabIndex = 1;
            this.grpThongTin.TabStop = false;
            this.grpThongTin.Text = "THÔNG TIN SẢN PHẨM";
            // 
            // btnQuanLyNCC
            // 
            this.btnQuanLyNCC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuanLyNCC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.btnQuanLyNCC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuanLyNCC.FlatAppearance.BorderSize = 0;
            this.btnQuanLyNCC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuanLyNCC.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnQuanLyNCC.ForeColor = System.Drawing.Color.White;
            this.btnQuanLyNCC.Location = new System.Drawing.Point(879, 104);
            this.btnQuanLyNCC.Name = "btnQuanLyNCC";
            this.btnQuanLyNCC.Size = new System.Drawing.Size(32, 27);
            this.btnQuanLyNCC.TabIndex = 19;
            this.btnQuanLyNCC.Text = "...";
            this.btnQuanLyNCC.UseVisualStyleBackColor = false;
            this.btnQuanLyNCC.Click += new System.EventHandler(this.btnQuanLyNCC_Click);
            // 
            // btnQuanLyLoai
            // 
            this.btnQuanLyLoai.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.btnQuanLyLoai.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuanLyLoai.FlatAppearance.BorderSize = 0;
            this.btnQuanLyLoai.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuanLyLoai.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnQuanLyLoai.ForeColor = System.Drawing.Color.White;
            this.btnQuanLyLoai.Location = new System.Drawing.Point(248, 104);
            this.btnQuanLyLoai.Name = "btnQuanLyLoai";
            this.btnQuanLyLoai.Size = new System.Drawing.Size(32, 27);
            this.btnQuanLyLoai.TabIndex = 18;
            this.btnQuanLyLoai.Text = "...";
            this.btnQuanLyLoai.UseVisualStyleBackColor = false;
            this.btnQuanLyLoai.Click += new System.EventHandler(this.btnQuanLyLoai_Click);
            // 
            // cboLoai
            // 
            this.cboLoai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoai.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cboLoai.FormattingEnabled = true;
            this.cboLoai.Location = new System.Drawing.Point(95, 105);
            this.cboLoai.Name = "cboLoai";
            this.cboLoai.Size = new System.Drawing.Size(147, 25);
            this.cboLoai.TabIndex = 5;
            // 
            // lblLoai
            // 
            this.lblLoai.AutoSize = true;
            this.lblLoai.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblLoai.ForeColor = System.Drawing.Color.Black;
            this.lblLoai.Location = new System.Drawing.Point(12, 109);
            this.lblLoai.Name = "lblLoai";
            this.lblLoai.Size = new System.Drawing.Size(65, 17);
            this.lblLoai.TabIndex = 4;
            this.lblLoai.Text = "Phân loại:";
            // 
            // cboNhaCungCap
            // 
            this.cboNhaCungCap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboNhaCungCap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNhaCungCap.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cboNhaCungCap.FormattingEnabled = true;
            this.cboNhaCungCap.Location = new System.Drawing.Point(690, 105);
            this.cboNhaCungCap.Name = "cboNhaCungCap";
            this.cboNhaCungCap.Size = new System.Drawing.Size(183, 25);
            this.cboNhaCungCap.TabIndex = 17;
            // 
            // lblNhaCungCap
            // 
            this.lblNhaCungCap.AutoSize = true;
            this.lblNhaCungCap.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblNhaCungCap.ForeColor = System.Drawing.Color.Black;
            this.lblNhaCungCap.Location = new System.Drawing.Point(595, 109);
            this.lblNhaCungCap.Name = "lblNhaCungCap";
            this.lblNhaCungCap.Size = new System.Drawing.Size(92, 17);
            this.lblNhaCungCap.TabIndex = 16;
            this.lblNhaCungCap.Text = "Nhà cung cấp:";
            // 
            // txtDonGia
            // 
            this.txtDonGia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDonGia.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtDonGia.Location = new System.Drawing.Point(690, 66);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.Size = new System.Drawing.Size(220, 25);
            this.txtDonGia.TabIndex = 15;
            this.txtDonGia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOnlyNumber_KeyPress);
            // 
            // lblDonGia
            // 
            this.lblDonGia.AutoSize = true;
            this.lblDonGia.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblDonGia.ForeColor = System.Drawing.Color.Black;
            this.lblDonGia.Location = new System.Drawing.Point(595, 70);
            this.lblDonGia.Name = "lblDonGia";
            this.lblDonGia.Size = new System.Drawing.Size(57, 17);
            this.lblDonGia.TabIndex = 14;
            this.lblDonGia.Text = "Đơn giá:";
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSoLuong.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtSoLuong.Location = new System.Drawing.Point(690, 27);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(220, 25);
            this.txtSoLuong.TabIndex = 13;
            this.txtSoLuong.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOnlyNumber_KeyPress);
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblSoLuong.ForeColor = System.Drawing.Color.Black;
            this.lblSoLuong.Location = new System.Drawing.Point(595, 31);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(64, 17);
            this.lblSoLuong.TabIndex = 12;
            this.lblSoLuong.Text = "Số lượng:";
            // 
            // txtSSD
            // 
            this.txtSSD.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtSSD.Location = new System.Drawing.Point(375, 105);
            this.txtSSD.Name = "txtSSD";
            this.txtSSD.Size = new System.Drawing.Size(185, 25);
            this.txtSSD.TabIndex = 11;
            // 
            // lblSSD
            // 
            this.lblSSD.AutoSize = true;
            this.lblSSD.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblSSD.ForeColor = System.Drawing.Color.Black;
            this.lblSSD.Location = new System.Drawing.Point(308, 109);
            this.lblSSD.Name = "lblSSD";
            this.lblSSD.Size = new System.Drawing.Size(53, 17);
            this.lblSSD.TabIndex = 10;
            this.lblSSD.Text = "Ổ cứng:";
            // 
            // txtRAM
            // 
            this.txtRAM.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtRAM.Location = new System.Drawing.Point(375, 66);
            this.txtRAM.Name = "txtRAM";
            this.txtRAM.Size = new System.Drawing.Size(185, 25);
            this.txtRAM.TabIndex = 9;
            // 
            // lblRAM
            // 
            this.lblRAM.AutoSize = true;
            this.lblRAM.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblRAM.ForeColor = System.Drawing.Color.Black;
            this.lblRAM.Location = new System.Drawing.Point(308, 70);
            this.lblRAM.Name = "lblRAM";
            this.lblRAM.Size = new System.Drawing.Size(39, 17);
            this.lblRAM.TabIndex = 8;
            this.lblRAM.Text = "RAM:";
            // 
            // txtCPU
            // 
            this.txtCPU.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtCPU.Location = new System.Drawing.Point(375, 27);
            this.txtCPU.Name = "txtCPU";
            this.txtCPU.Size = new System.Drawing.Size(185, 25);
            this.txtCPU.TabIndex = 7;
            // 
            // lblCPU
            // 
            this.lblCPU.AutoSize = true;
            this.lblCPU.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblCPU.ForeColor = System.Drawing.Color.Black;
            this.lblCPU.Location = new System.Drawing.Point(308, 31);
            this.lblCPU.Name = "lblCPU";
            this.lblCPU.Size = new System.Drawing.Size(35, 17);
            this.lblCPU.TabIndex = 6;
            this.lblCPU.Text = "CPU:";
            // 
            // txtTenLaptop
            // 
            this.txtTenLaptop.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtTenLaptop.Location = new System.Drawing.Point(95, 66);
            this.txtTenLaptop.Name = "txtTenLaptop";
            this.txtTenLaptop.Size = new System.Drawing.Size(185, 25);
            this.txtTenLaptop.TabIndex = 3;
            // 
            // lblTenLaptop
            // 
            this.lblTenLaptop.AutoSize = true;
            this.lblTenLaptop.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblTenLaptop.ForeColor = System.Drawing.Color.Black;
            this.lblTenLaptop.Location = new System.Drawing.Point(12, 70);
            this.lblTenLaptop.Name = "lblTenLaptop";
            this.lblTenLaptop.Size = new System.Drawing.Size(74, 17);
            this.lblTenLaptop.TabIndex = 2;
            this.lblTenLaptop.Text = "Tên Laptop:";
            // 
            // txtMa
            // 
            this.txtMa.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtMa.Location = new System.Drawing.Point(95, 27);
            this.txtMa.MaxLength = 10;
            this.txtMa.Name = "txtMa";
            this.txtMa.Size = new System.Drawing.Size(185, 25);
            this.txtMa.TabIndex = 1;
            // 
            // lblMa
            // 
            this.lblMa.AutoSize = true;
            this.lblMa.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblMa.ForeColor = System.Drawing.Color.Black;
            this.lblMa.Location = new System.Drawing.Point(12, 31);
            this.lblMa.Name = "lblMa";
            this.lblMa.Size = new System.Drawing.Size(73, 17);
            this.lblMa.TabIndex = 0;
            this.lblMa.Text = "Mã Laptop:";
            // 
            // pnlTopHeader
            // 
            this.pnlTopHeader.BackColor = System.Drawing.Color.White;
            this.pnlTopHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTopHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlTopHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopHeader.Location = new System.Drawing.Point(16, 16);
            this.pnlTopHeader.Name = "pnlTopHeader";
            this.pnlTopHeader.Size = new System.Drawing.Size(927, 40);
            this.pnlTopHeader.TabIndex = 0;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.lblHeaderTitle.Location = new System.Drawing.Point(8, 7);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(264, 25);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "DANH MỤC HÀNG HÓA LAPTOP";
            // 
            // ucSanPham
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ucSanPham";
            this.Size = new System.Drawing.Size(959, 661);
            this.Load += new System.EventHandler(this.ucSanPham_Load);
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLaptop)).EndInit();
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.grpThongTin.ResumeLayout(false);
            this.grpThongTin.PerformLayout();
            this.pnlTopHeader.ResumeLayout(false);
            this.pnlTopHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlTopHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.GroupBox grpThongTin;
        private System.Windows.Forms.TextBox txtTenLaptop;
        private System.Windows.Forms.Label lblTenLaptop;
        private System.Windows.Forms.TextBox txtMa;
        private System.Windows.Forms.Label lblMa;
        private System.Windows.Forms.TextBox txtSSD;
        private System.Windows.Forms.Label lblSSD;
        private System.Windows.Forms.TextBox txtRAM;
        private System.Windows.Forms.Label lblRAM;
        private System.Windows.Forms.TextBox txtCPU;
        private System.Windows.Forms.Label lblCPU;
        private System.Windows.Forms.TextBox txtDonGia;
        private System.Windows.Forms.Label lblDonGia;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.Label lblNhaCungCap;
        private System.Windows.Forms.ComboBox cboNhaCungCap;
        private System.Windows.Forms.ComboBox cboLoai;
        private System.Windows.Forms.Label lblLoai;
        private System.Windows.Forms.Button btnQuanLyLoai;
        private System.Windows.Forms.Button btnQuanLyNCC;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.ComboBox cboTieuChiTimKiem;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.DataGridView dgvLaptop;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaLaptop;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenLaptop;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNhaCungCap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCPU;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRAM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSSD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuongTon;
    }
}
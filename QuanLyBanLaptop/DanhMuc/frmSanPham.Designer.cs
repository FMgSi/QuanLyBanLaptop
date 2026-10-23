namespace QuanLyBanLaptop.DanhMuc
{
    partial class frmSanPham
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
            this.dgvLaptop = new System.Windows.Forms.DataGridView();
            this.colMaLaptop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenLaptop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCPU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRAM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOCung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuongTon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpThongTin = new System.Windows.Forms.GroupBox();
            this.btnXoaAnh = new System.Windows.Forms.Button();
            this.btnChonAnh = new System.Windows.Forms.Button();
            this.picAnhLaptop = new System.Windows.Forms.PictureBox();
            this.txtNhaCungCap = new System.Windows.Forms.TextBox();
            this.lblNhaCungCap = new System.Windows.Forms.Label();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.txtRAM = new System.Windows.Forms.TextBox();
            this.lblRAM = new System.Windows.Forms.Label();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.txtSSD = new System.Windows.Forms.TextBox();
            this.lblSSD = new System.Windows.Forms.Label();
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
            this.grpThongTin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAnhLaptop)).BeginInit();
            this.pnlTopHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidebarControl1
            // 
            this.sidebarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarControl1.Location = new System.Drawing.Point(0, 0);
            this.sidebarControl1.Name = "sidebarControl1";
            this.sidebarControl1.SelectedMenu = QuanLyBanLaptop.Classes.NavigationMenu.QuanLyLaptop;
            this.sidebarControl1.Size = new System.Drawing.Size(175, 661);
            this.sidebarControl1.TabIndex = 0;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.pnlMain.Controls.Add(this.dgvLaptop);
            this.pnlMain.Controls.Add(this.grpThongTin);
            this.pnlMain.Controls.Add(this.pnlTopHeader);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(175, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(16);
            this.pnlMain.Size = new System.Drawing.Size(959, 661);
            this.pnlMain.TabIndex = 1;
            // 
            // dgvLaptop
            // 
            this.dgvLaptop.AllowUserToAddRows = false;
            this.dgvLaptop.BackgroundColor = System.Drawing.Color.Silver;
            this.dgvLaptop.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLaptop.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaLaptop,
            this.colTenLaptop,
            this.colCPU,
            this.colRAM,
            this.colOCung,
            this.colDonGia,
            this.colSoLuongTon});
            this.dgvLaptop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLaptop.Location = new System.Drawing.Point(16, 266);
            this.dgvLaptop.Name = "dgvLaptop";
            this.dgvLaptop.ReadOnly = true;
            this.dgvLaptop.RowHeadersWidth = 30;
            this.dgvLaptop.Size = new System.Drawing.Size(927, 379);
            this.dgvLaptop.TabIndex = 2;
            // 
            // colMaLaptop
            // 
            this.colMaLaptop.HeaderText = "Mã Laptop";
            this.colMaLaptop.Name = "colMaLaptop";
            this.colMaLaptop.ReadOnly = true;
            this.colMaLaptop.Width = 110;
            // 
            // colTenLaptop
            // 
            this.colTenLaptop.HeaderText = "Tên Laptop";
            this.colTenLaptop.Name = "colTenLaptop";
            this.colTenLaptop.ReadOnly = true;
            this.colTenLaptop.Width = 200;
            // 
            // colCPU
            // 
            this.colCPU.HeaderText = "CPU";
            this.colCPU.Name = "colCPU";
            this.colCPU.ReadOnly = true;
            this.colCPU.Width = 140;
            // 
            // colRAM
            // 
            this.colRAM.HeaderText = "RAM";
            this.colRAM.Name = "colRAM";
            this.colRAM.ReadOnly = true;
            this.colRAM.Width = 90;
            // 
            // colOCung
            // 
            this.colOCung.HeaderText = "Ổ cứng";
            this.colOCung.Name = "colOCung";
            this.colOCung.ReadOnly = true;
            this.colOCung.Width = 110;
            // 
            // colDonGia
            // 
            this.colDonGia.HeaderText = "Đơn giá";
            this.colDonGia.Name = "colDonGia";
            this.colDonGia.ReadOnly = true;
            this.colDonGia.Width = 110;
            // 
            // colSoLuongTon
            // 
            this.colSoLuongTon.HeaderText = "Số lượng tồn";
            this.colSoLuongTon.Name = "colSoLuongTon";
            this.colSoLuongTon.ReadOnly = true;
            this.colSoLuongTon.Width = 110;
            // 
            // grpThongTin
            // 
            this.grpThongTin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.grpThongTin.Controls.Add(this.btnXoaAnh);
            this.grpThongTin.Controls.Add(this.btnChonAnh);
            this.grpThongTin.Controls.Add(this.picAnhLaptop);
            this.grpThongTin.Controls.Add(this.txtNhaCungCap);
            this.grpThongTin.Controls.Add(this.lblNhaCungCap);
            this.grpThongTin.Controls.Add(this.txtSoLuong);
            this.grpThongTin.Controls.Add(this.lblSoLuong);
            this.grpThongTin.Controls.Add(this.txtRAM);
            this.grpThongTin.Controls.Add(this.lblRAM);
            this.grpThongTin.Controls.Add(this.txtDonGia);
            this.grpThongTin.Controls.Add(this.lblDonGia);
            this.grpThongTin.Controls.Add(this.txtSSD);
            this.grpThongTin.Controls.Add(this.lblSSD);
            this.grpThongTin.Controls.Add(this.txtCPU);
            this.grpThongTin.Controls.Add(this.lblCPU);
            this.grpThongTin.Controls.Add(this.txtTenLaptop);
            this.grpThongTin.Controls.Add(this.lblTenLaptop);
            this.grpThongTin.Controls.Add(this.txtMa);
            this.grpThongTin.Controls.Add(this.lblMa);
            this.grpThongTin.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpThongTin.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpThongTin.Location = new System.Drawing.Point(16, 56);
            this.grpThongTin.Name = "grpThongTin";
            this.grpThongTin.Size = new System.Drawing.Size(927, 210);
            this.grpThongTin.TabIndex = 1;
            this.grpThongTin.TabStop = false;
            this.grpThongTin.Text = "Thông tin laptop";
            // 
            // btnXoaAnh
            // 
            this.btnXoaAnh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXoaAnh.Location = new System.Drawing.Point(837, 168);
            this.btnXoaAnh.Name = "btnXoaAnh";
            this.btnXoaAnh.Size = new System.Drawing.Size(75, 28);
            this.btnXoaAnh.TabIndex = 18;
            this.btnXoaAnh.Text = "Xóa ảnh";
            this.btnXoaAnh.UseVisualStyleBackColor = true;
            // 
            // btnChonAnh
            // 
            this.btnChonAnh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChonAnh.Location = new System.Drawing.Point(744, 168);
            this.btnChonAnh.Name = "btnChonAnh";
            this.btnChonAnh.Size = new System.Drawing.Size(87, 28);
            this.btnChonAnh.TabIndex = 17;
            this.btnChonAnh.Text = "Chọn ảnh...";
            this.btnChonAnh.UseVisualStyleBackColor = true;
            // 
            // picAnhLaptop
            // 
            this.picAnhLaptop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picAnhLaptop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.picAnhLaptop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAnhLaptop.Location = new System.Drawing.Point(744, 25);
            this.picAnhLaptop.Name = "picAnhLaptop";
            this.picAnhLaptop.Size = new System.Drawing.Size(168, 137);
            this.picAnhLaptop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAnhLaptop.TabIndex = 16;
            this.picAnhLaptop.TabStop = false;
            // 
            // txtNhaCungCap
            // 
            this.txtNhaCungCap.Location = new System.Drawing.Point(475, 166);
            this.txtNhaCungCap.Name = "txtNhaCungCap";
            this.txtNhaCungCap.Size = new System.Drawing.Size(236, 25);
            this.txtNhaCungCap.TabIndex = 15;
            // 
            // lblNhaCungCap
            // 
            this.lblNhaCungCap.AutoSize = true;
            this.lblNhaCungCap.Location = new System.Drawing.Point(380, 169);
            this.lblNhaCungCap.Name = "lblNhaCungCap";
            this.lblNhaCungCap.Size = new System.Drawing.Size(89, 17);
            this.lblNhaCungCap.TabIndex = 14;
            this.lblNhaCungCap.Text = "Nhà cung cấp";
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Location = new System.Drawing.Point(475, 131);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(236, 25);
            this.txtSoLuong.TabIndex = 13;
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Location = new System.Drawing.Point(380, 134);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(61, 17);
            this.lblSoLuong.TabIndex = 12;
            this.lblSoLuong.Text = "Số lượng";
            // 
            // txtRAM
            // 
            this.txtRAM.Location = new System.Drawing.Point(475, 96);
            this.txtRAM.Name = "txtRAM";
            this.txtRAM.Size = new System.Drawing.Size(236, 25);
            this.txtRAM.TabIndex = 11;
            // 
            // lblRAM
            // 
            this.lblRAM.AutoSize = true;
            this.lblRAM.Location = new System.Drawing.Point(380, 99);
            this.lblRAM.Name = "lblRAM";
            this.lblRAM.Size = new System.Drawing.Size(35, 17);
            this.lblRAM.TabIndex = 10;
            this.lblRAM.Text = "RAM";
            // 
            // txtDonGia
            // 
            this.txtDonGia.Location = new System.Drawing.Point(105, 166);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.Size = new System.Drawing.Size(248, 25);
            this.txtDonGia.TabIndex = 9;
            // 
            // lblDonGia
            // 
            this.lblDonGia.AutoSize = true;
            this.lblDonGia.Location = new System.Drawing.Point(20, 169);
            this.lblDonGia.Name = "lblDonGia";
            this.lblDonGia.Size = new System.Drawing.Size(53, 17);
            this.lblDonGia.TabIndex = 8;
            this.lblDonGia.Text = "Đơn giá";
            // 
            // txtSSD
            // 
            this.txtSSD.Location = new System.Drawing.Point(105, 131);
            this.txtSSD.Name = "txtSSD";
            this.txtSSD.Size = new System.Drawing.Size(248, 25);
            this.txtSSD.TabIndex = 7;
            // 
            // lblSSD
            // 
            this.lblSSD.AutoSize = true;
            this.lblSSD.Location = new System.Drawing.Point(20, 134);
            this.lblSSD.Name = "lblSSD";
            this.lblSSD.Size = new System.Drawing.Size(32, 17);
            this.lblSSD.TabIndex = 6;
            this.lblSSD.Text = "SSD";
            // 
            // txtCPU
            // 
            this.txtCPU.Location = new System.Drawing.Point(105, 96);
            this.txtCPU.Name = "txtCPU";
            this.txtCPU.Size = new System.Drawing.Size(248, 25);
            this.txtCPU.TabIndex = 5;
            // 
            // lblCPU
            // 
            this.lblCPU.AutoSize = true;
            this.lblCPU.Location = new System.Drawing.Point(20, 99);
            this.lblCPU.Name = "lblCPU";
            this.lblCPU.Size = new System.Drawing.Size(32, 17);
            this.lblCPU.TabIndex = 4;
            this.lblCPU.Text = "CPU";
            // 
            // txtTenLaptop
            // 
            this.txtTenLaptop.Location = new System.Drawing.Point(105, 61);
            this.txtTenLaptop.Name = "txtTenLaptop";
            this.txtTenLaptop.Size = new System.Drawing.Size(606, 25);
            this.txtTenLaptop.TabIndex = 3;
            // 
            // lblTenLaptop
            // 
            this.lblTenLaptop.AutoSize = true;
            this.lblTenLaptop.Location = new System.Drawing.Point(20, 64);
            this.lblTenLaptop.Name = "lblTenLaptop";
            this.lblTenLaptop.Size = new System.Drawing.Size(71, 17);
            this.lblTenLaptop.TabIndex = 2;
            this.lblTenLaptop.Text = "Tên laptop";
            // 
            // txtMa
            // 
            this.txtMa.Location = new System.Drawing.Point(105, 26);
            this.txtMa.Name = "txtMa";
            this.txtMa.Size = new System.Drawing.Size(248, 25);
            this.txtMa.TabIndex = 1;
            // 
            // lblMa
            // 
            this.lblMa.AutoSize = true;
            this.lblMa.Location = new System.Drawing.Point(20, 29);
            this.lblMa.Name = "lblMa";
            this.lblMa.Size = new System.Drawing.Size(27, 17);
            this.lblMa.TabIndex = 0;
            this.lblMa.Text = "Mã";
            // 
            // pnlTopHeader
            // 
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
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.lblHeaderTitle.Location = new System.Drawing.Point(3, 7);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(223, 25);
            this.lblHeaderTitle.TabIndex = 0;
            this.lblHeaderTitle.Text = "TRANG QUẢN LÝ LAPTOP";
            // 
            // frmSanPham
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 661);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.sidebarControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmSanPham";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TRANG QUẢN LÝ LAPTOP";
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLaptop)).EndInit();
            this.grpThongTin.ResumeLayout(false);
            this.grpThongTin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAnhLaptop)).EndInit();
            this.pnlTopHeader.ResumeLayout(false);
            this.pnlTopHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SidebarControl sidebarControl1;
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
        private System.Windows.Forms.TextBox txtCPU;
        private System.Windows.Forms.Label lblCPU;
        private System.Windows.Forms.TextBox txtDonGia;
        private System.Windows.Forms.Label lblDonGia;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.TextBox txtRAM;
        private System.Windows.Forms.Label lblRAM;
        private System.Windows.Forms.TextBox txtNhaCungCap;
        private System.Windows.Forms.Label lblNhaCungCap;
        private System.Windows.Forms.Button btnXoaAnh;
        private System.Windows.Forms.Button btnChonAnh;
        private System.Windows.Forms.PictureBox picAnhLaptop;
        private System.Windows.Forms.DataGridView dgvLaptop;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaLaptop;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenLaptop;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCPU;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRAM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOCung;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuongTon;
    }
}
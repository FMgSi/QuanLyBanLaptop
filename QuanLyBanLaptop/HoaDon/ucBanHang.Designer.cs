namespace QuanLyBanLaptop.HoaDon
{
    partial class ucBanHang
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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.lblTongTienSo = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.dgvGioHang = new System.Windows.Forms.DataGridView();
            this.colMa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenLaptop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiamGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDanhSach = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnXoaKhoiGio = new System.Windows.Forms.Button();
            this.btnThemVaoGio = new System.Windows.Forms.Button();
            this.grpChonSp = new System.Windows.Forms.GroupBox();
            this.txtGiamGia = new System.Windows.Forms.TextBox();
            this.lblGiamGia = new System.Windows.Forms.Label();
            this.txtSoLuongBan = new System.Windows.Forms.TextBox();
            this.lblSoLuongBan = new System.Windows.Forms.Label();
            this.txtDonGiaBan = new System.Windows.Forms.TextBox();
            this.lblDonGiaBan = new System.Windows.Forms.Label();
            this.cboLaptopBan = new System.Windows.Forms.ComboBox();
            this.lblChonLaptop = new System.Windows.Forms.Label();
            this.grpThongTinKhach = new System.Windows.Forms.GroupBox();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.lblSDT = new System.Windows.Forms.Label();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtTenKhach = new System.Windows.Forms.TextBox();
            this.lblTenKhach = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGioHang)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.grpChonSp.SuspendLayout();
            this.grpThongTinKhach.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.pnlMain.Controls.Add(this.pnlBottom);
            this.pnlMain.Controls.Add(this.dgvGioHang);
            this.pnlMain.Controls.Add(this.lblDanhSach);
            this.pnlMain.Controls.Add(this.pnlButtons);
            this.pnlMain.Controls.Add(this.grpChonSp);
            this.pnlMain.Controls.Add(this.grpThongTinKhach);
            this.pnlMain.Controls.Add(this.pnlHeader);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(16);
            this.pnlMain.Size = new System.Drawing.Size(1134, 661);
            this.pnlMain.TabIndex = 1;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnThanhToan);
            this.pnlBottom.Controls.Add(this.lblTongTienSo);
            this.pnlBottom.Controls.Add(this.lblTongTien);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(16, 595);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1102, 50);
            this.pnlBottom.TabIndex = 6;
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThanhToan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnThanhToan.FlatAppearance.BorderSize = 0;
            this.btnThanhToan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThanhToan.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThanhToan.ForeColor = System.Drawing.Color.White;
            this.btnThanhToan.Location = new System.Drawing.Point(922, 8);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(180, 36);
            this.btnThanhToan.TabIndex = 2;
            this.btnThanhToan.Text = "XÁC NHẬN THANH TOÁN";
            this.btnThanhToan.UseVisualStyleBackColor = false;
            // 
            // lblTongTienSo
            // 
            this.lblTongTienSo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTongTienSo.AutoSize = true;
            this.lblTongTienSo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongTienSo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(83)))), ((int)(((byte)(79)))));
            this.lblTongTienSo.Location = new System.Drawing.Point(795, 15);
            this.lblTongTienSo.Name = "lblTongTienSo";
            this.lblTongTienSo.Size = new System.Drawing.Size(161, 38);
            this.lblTongTienSo.TabIndex = 1;
            this.lblTongTienSo.Text = "28.500.000";
            // 
            // lblTongTien
            // 
            this.lblTongTien.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongTien.Location = new System.Drawing.Point(700, 16);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(142, 35);
            this.lblTongTien.TabIndex = 0;
            this.lblTongTien.Text = "Tổng tiền :";
            // 
            // dgvGioHang
            // 
            this.dgvGioHang.AllowUserToAddRows = false;
            this.dgvGioHang.BackgroundColor = System.Drawing.Color.Silver;
            this.dgvGioHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGioHang.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMa,
            this.colTenLaptop,
            this.colDonGia,
            this.colSoLuong,
            this.colGiamGia,
            this.colThanhTien});
            this.dgvGioHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGioHang.Location = new System.Drawing.Point(16, 335);
            this.dgvGioHang.Name = "dgvGioHang";
            this.dgvGioHang.ReadOnly = true;
            this.dgvGioHang.RowHeadersWidth = 30;
            this.dgvGioHang.Size = new System.Drawing.Size(1102, 310);
            this.dgvGioHang.TabIndex = 5;
            // 
            // colMa
            // 
            this.colMa.HeaderText = "Mã SP";
            this.colMa.MinimumWidth = 9;
            this.colMa.Name = "colMa";
            this.colMa.ReadOnly = true;
            this.colMa.Width = 90;
            // 
            // colTenLaptop
            // 
            this.colTenLaptop.HeaderText = "Tên Laptop";
            this.colTenLaptop.MinimumWidth = 9;
            this.colTenLaptop.Name = "colTenLaptop";
            this.colTenLaptop.ReadOnly = true;
            this.colTenLaptop.Width = 240;
            // 
            // colDonGia
            // 
            this.colDonGia.HeaderText = "Đơn giá";
            this.colDonGia.MinimumWidth = 9;
            this.colDonGia.Name = "colDonGia";
            this.colDonGia.ReadOnly = true;
            this.colDonGia.Width = 120;
            // 
            // colSoLuong
            // 
            this.colSoLuong.HeaderText = "Số lượng";
            this.colSoLuong.MinimumWidth = 9;
            this.colSoLuong.Name = "colSoLuong";
            this.colSoLuong.ReadOnly = true;
            this.colSoLuong.Width = 90;
            // 
            // colGiamGia
            // 
            this.colGiamGia.HeaderText = "Giảm giá (%)";
            this.colGiamGia.MinimumWidth = 9;
            this.colGiamGia.Name = "colGiamGia";
            this.colGiamGia.ReadOnly = true;
            this.colGiamGia.Width = 110;
            // 
            // colThanhTien
            // 
            this.colThanhTien.HeaderText = "Thành tiền";
            this.colThanhTien.MinimumWidth = 9;
            this.colThanhTien.Name = "colThanhTien";
            this.colThanhTien.ReadOnly = true;
            this.colThanhTien.Width = 130;
            // 
            // lblDanhSach
            // 
            this.lblDanhSach.AutoSize = true;
            this.lblDanhSach.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDanhSach.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDanhSach.Location = new System.Drawing.Point(16, 290);
            this.lblDanhSach.Name = "lblDanhSach";
            this.lblDanhSach.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.lblDanhSach.Size = new System.Drawing.Size(243, 45);
            this.lblDanhSach.TabIndex = 4;
            this.lblDanhSach.Text = "Danh sách giỏ hàng";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnXoaKhoiGio);
            this.pnlButtons.Controls.Add(this.btnThemVaoGio);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButtons.Location = new System.Drawing.Point(16, 246);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(1102, 44);
            this.pnlButtons.TabIndex = 3;
            // 
            // btnXoaKhoiGio
            // 
            this.btnXoaKhoiGio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(83)))), ((int)(((byte)(79)))));
            this.btnXoaKhoiGio.FlatAppearance.BorderSize = 0;
            this.btnXoaKhoiGio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaKhoiGio.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoaKhoiGio.ForeColor = System.Drawing.Color.White;
            this.btnXoaKhoiGio.Location = new System.Drawing.Point(620, 6);
            this.btnXoaKhoiGio.Name = "btnXoaKhoiGio";
            this.btnXoaKhoiGio.Size = new System.Drawing.Size(140, 32);
            this.btnXoaKhoiGio.TabIndex = 1;
            this.btnXoaKhoiGio.Text = "Xóa khỏi giỏ";
            this.btnXoaKhoiGio.UseVisualStyleBackColor = false;
            // 
            // btnThemVaoGio
            // 
            this.btnThemVaoGio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(117)))), ((int)(((byte)(216)))));
            this.btnThemVaoGio.FlatAppearance.BorderSize = 0;
            this.btnThemVaoGio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemVaoGio.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemVaoGio.ForeColor = System.Drawing.Color.White;
            this.btnThemVaoGio.Location = new System.Drawing.Point(470, 6);
            this.btnThemVaoGio.Name = "btnThemVaoGio";
            this.btnThemVaoGio.Size = new System.Drawing.Size(140, 32);
            this.btnThemVaoGio.TabIndex = 0;
            this.btnThemVaoGio.Text = "Thêm vào giỏ";
            this.btnThemVaoGio.UseVisualStyleBackColor = false;
            // 
            // grpChonSp
            // 
            this.grpChonSp.Controls.Add(this.txtGiamGia);
            this.grpChonSp.Controls.Add(this.lblGiamGia);
            this.grpChonSp.Controls.Add(this.txtSoLuongBan);
            this.grpChonSp.Controls.Add(this.lblSoLuongBan);
            this.grpChonSp.Controls.Add(this.txtDonGiaBan);
            this.grpChonSp.Controls.Add(this.lblDonGiaBan);
            this.grpChonSp.Controls.Add(this.cboLaptopBan);
            this.grpChonSp.Controls.Add(this.lblChonLaptop);
            this.grpChonSp.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpChonSp.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpChonSp.Location = new System.Drawing.Point(16, 151);
            this.grpChonSp.Name = "grpChonSp";
            this.grpChonSp.Size = new System.Drawing.Size(1102, 95);
            this.grpChonSp.TabIndex = 2;
            this.grpChonSp.TabStop = false;
            this.grpChonSp.Text = "Chọn Laptop bán";
            // 
            // txtGiamGia
            // 
            this.txtGiamGia.Location = new System.Drawing.Point(780, 35);
            this.txtGiamGia.Name = "txtGiamGia";
            this.txtGiamGia.Size = new System.Drawing.Size(80, 38);
            this.txtGiamGia.TabIndex = 7;
            this.txtGiamGia.Text = "0";
            // 
            // lblGiamGia
            // 
            this.lblGiamGia.AutoSize = true;
            this.lblGiamGia.Location = new System.Drawing.Point(715, 38);
            this.lblGiamGia.Name = "lblGiamGia";
            this.lblGiamGia.Size = new System.Drawing.Size(107, 31);
            this.lblGiamGia.TabIndex = 6;
            this.lblGiamGia.Text = "Giảm (%)";
            // 
            // txtSoLuongBan
            // 
            this.txtSoLuongBan.Location = new System.Drawing.Point(620, 35);
            this.txtSoLuongBan.Name = "txtSoLuongBan";
            this.txtSoLuongBan.Size = new System.Drawing.Size(80, 38);
            this.txtSoLuongBan.TabIndex = 5;
            this.txtSoLuongBan.Text = "1";
            // 
            // lblSoLuongBan
            // 
            this.lblSoLuongBan.AutoSize = true;
            this.lblSoLuongBan.Location = new System.Drawing.Point(555, 38);
            this.lblSoLuongBan.Name = "lblSoLuongBan";
            this.lblSoLuongBan.Size = new System.Drawing.Size(106, 31);
            this.lblSoLuongBan.TabIndex = 4;
            this.lblSoLuongBan.Text = "Số lượng";
            // 
            // txtDonGiaBan
            // 
            this.txtDonGiaBan.Location = new System.Drawing.Point(400, 35);
            this.txtDonGiaBan.Name = "txtDonGiaBan";
            this.txtDonGiaBan.ReadOnly = true;
            this.txtDonGiaBan.Size = new System.Drawing.Size(140, 38);
            this.txtDonGiaBan.TabIndex = 3;
            // 
            // lblDonGiaBan
            // 
            this.lblDonGiaBan.AutoSize = true;
            this.lblDonGiaBan.Location = new System.Drawing.Point(340, 38);
            this.lblDonGiaBan.Name = "lblDonGiaBan";
            this.lblDonGiaBan.Size = new System.Drawing.Size(95, 31);
            this.lblDonGiaBan.TabIndex = 2;
            this.lblDonGiaBan.Text = "Đơn giá";
            // 
            // cboLaptopBan
            // 
            this.cboLaptopBan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLaptopBan.FormattingEnabled = true;
            this.cboLaptopBan.Location = new System.Drawing.Point(95, 35);
            this.cboLaptopBan.Name = "cboLaptopBan";
            this.cboLaptopBan.Size = new System.Drawing.Size(230, 39);
            this.cboLaptopBan.TabIndex = 1;
            // 
            // lblChonLaptop
            // 
            this.lblChonLaptop.AutoSize = true;
            this.lblChonLaptop.Location = new System.Drawing.Point(15, 38);
            this.lblChonLaptop.Name = "lblChonLaptop";
            this.lblChonLaptop.Size = new System.Drawing.Size(86, 31);
            this.lblChonLaptop.TabIndex = 0;
            this.lblChonLaptop.Text = "Laptop";
            // 
            // grpThongTinKhach
            // 
            this.grpThongTinKhach.Controls.Add(this.txtSDT);
            this.grpThongTinKhach.Controls.Add(this.lblSDT);
            this.grpThongTinKhach.Controls.Add(this.txtDiaChi);
            this.grpThongTinKhach.Controls.Add(this.lblDiaChi);
            this.grpThongTinKhach.Controls.Add(this.txtTenKhach);
            this.grpThongTinKhach.Controls.Add(this.lblTenKhach);
            this.grpThongTinKhach.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpThongTinKhach.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpThongTinKhach.Location = new System.Drawing.Point(16, 56);
            this.grpThongTinKhach.Name = "grpThongTinKhach";
            this.grpThongTinKhach.Size = new System.Drawing.Size(1102, 95);
            this.grpThongTinKhach.TabIndex = 1;
            this.grpThongTinKhach.TabStop = false;
            this.grpThongTinKhach.Text = "Thông tin khách hàng";
            // 
            // txtSDT
            // 
            this.txtSDT.Location = new System.Drawing.Point(400, 35);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(150, 38);
            this.txtSDT.TabIndex = 3;
            // 
            // lblSDT
            // 
            this.lblSDT.AutoSize = true;
            this.lblSDT.Location = new System.Drawing.Point(350, 38);
            this.lblSDT.Name = "lblSDT";
            this.lblSDT.Size = new System.Drawing.Size(54, 31);
            this.lblSDT.TabIndex = 2;
            this.lblSDT.Text = "SĐT";
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.Location = new System.Drawing.Point(640, 35);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Size = new System.Drawing.Size(260, 38);
            this.txtDiaChi.TabIndex = 5;
            // 
            // lblDiaChi
            // 
            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Location = new System.Drawing.Point(580, 38);
            this.lblDiaChi.Name = "lblDiaChi";
            this.lblDiaChi.Size = new System.Drawing.Size(84, 31);
            this.lblDiaChi.TabIndex = 4;
            this.lblDiaChi.Text = "Địa chỉ";
            // 
            // txtTenKhach
            // 
            this.txtTenKhach.Location = new System.Drawing.Point(95, 35);
            this.txtTenKhach.Name = "txtTenKhach";
            this.txtTenKhach.Size = new System.Drawing.Size(230, 38);
            this.txtTenKhach.TabIndex = 1;
            // 
            // lblTenKhach
            // 
            this.lblTenKhach.AutoSize = true;
            this.lblTenKhach.Location = new System.Drawing.Point(15, 38);
            this.lblTenKhach.Name = "lblTenKhach";
            this.lblTenKhach.Size = new System.Drawing.Size(115, 31);
            this.lblTenKhach.TabIndex = 0;
            this.lblTenKhach.Text = "Tên khách";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(16, 16);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1102, 40);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.lblTitle.Location = new System.Drawing.Point(3, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(329, 46);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "TRANG BÁN HÀNG";
            // 
            // ucBanHang
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1134, 661);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ucBanHang";
            this.Text = "TRANG BÁN HÀNG";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGioHang)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.grpChonSp.ResumeLayout(false);
            this.grpChonSp.PerformLayout();
            this.grpThongTinKhach.ResumeLayout(false);
            this.grpThongTinKhach.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpThongTinKhach;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtTenKhach;
        private System.Windows.Forms.Label lblTenKhach;
        private System.Windows.Forms.GroupBox grpChonSp;
        private System.Windows.Forms.ComboBox cboLaptopBan;
        private System.Windows.Forms.Label lblChonLaptop;
        private System.Windows.Forms.TextBox txtDonGiaBan;
        private System.Windows.Forms.Label lblDonGiaBan;
        private System.Windows.Forms.TextBox txtSoLuongBan;
        private System.Windows.Forms.Label lblSoLuongBan;
        private System.Windows.Forms.TextBox txtGiamGia;
        private System.Windows.Forms.Label lblGiamGia;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnThemVaoGio;
        private System.Windows.Forms.Button btnXoaKhoiGio;
        private System.Windows.Forms.Label lblDanhSach;
        private System.Windows.Forms.DataGridView dgvGioHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMa;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenLaptop;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiamGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhTien;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Label lblTongTienSo;
        private System.Windows.Forms.Button btnThanhToan;
    }
}
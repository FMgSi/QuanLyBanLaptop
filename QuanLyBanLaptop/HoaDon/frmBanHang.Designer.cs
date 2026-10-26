namespace QuanLyBanLaptop.HoaDon
{
    partial class frmBanHang
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlSearchBottom = new System.Windows.Forms.Panel();
            this.lblMaHDTim = new System.Windows.Forms.Label();
            this.cboMaHDTim = new System.Windows.Forms.ComboBox();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.pnlButtonsRow = new System.Windows.Forms.Panel();
            this.btnThemHD = new System.Windows.Forms.Button();
            this.btnSuaHD = new System.Windows.Forms.Button();
            this.btnXoaHD = new System.Windows.Forms.Button();
            this.btnLuuHD = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnHuyHD = new System.Windows.Forms.Button();
            this.btnInHD = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();
            this.lblBangChu = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.txtTongTien = new System.Windows.Forms.TextBox();
            this.lblHuongDanXoa = new System.Windows.Forms.Label();
            this.dgvGioHang = new System.Windows.Forms.DataGridView();
            this.colMaHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiamGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThoiGianBaoHanh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpThongTinHang = new System.Windows.Forms.GroupBox();
            this.lblMaHang = new System.Windows.Forms.Label();
            this.cboMaHang = new System.Windows.Forms.ComboBox();
            this.btnTimKiemSP = new System.Windows.Forms.Button();
            this.lblTenHang = new System.Windows.Forms.Label();
            this.txtTenHang = new System.Windows.Forms.TextBox();
            this.lblBaoHanh = new System.Windows.Forms.Label();
            this.lblThang = new System.Windows.Forms.Label();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.txtBaoHanh = new System.Windows.Forms.TextBox();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.lblGiamGia = new System.Windows.Forms.Label();
            this.txtGiamGia = new System.Windows.Forms.TextBox();
            this.lblThanhTien = new System.Windows.Forms.Label();
            this.txtThanhTien = new System.Windows.Forms.TextBox();
            this.grpThongTinChung = new System.Windows.Forms.GroupBox();
            this.lblMaHD = new System.Windows.Forms.Label();
            this.txtMaHD = new System.Windows.Forms.TextBox();
            this.lblNgayBan = new System.Windows.Forms.Label();
            this.dtpNgayBan = new System.Windows.Forms.DateTimePicker();
            this.lblMaNV = new System.Windows.Forms.Label();
            this.txtMaNV = new System.Windows.Forms.TextBox();
            this.btnChonNV = new System.Windows.Forms.Button();
            this.lblTenNV = new System.Windows.Forms.Label();
            this.txtTenNV = new System.Windows.Forms.TextBox();
            this.lblMaKH = new System.Windows.Forms.Label();
            this.cboMaKH = new System.Windows.Forms.ComboBox();
            this.btnChonKH = new System.Windows.Forms.Button();
            this.lblTenKhach = new System.Windows.Forms.Label();
            this.txtTenKhach = new System.Windows.Forms.TextBox();
            this.lblHangKhach = new System.Windows.Forms.Label();
            this.txtHangKhach = new System.Windows.Forms.TextBox();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.lblSDT = new System.Windows.Forms.Label();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.lblPhanTram = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.sidebarControl1 = new QuanLyBanLaptop.SidebarControl();
            this.pnlMain.SuspendLayout();
            this.pnlSearchBottom.SuspendLayout();
            this.pnlButtonsRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGioHang)).BeginInit();
            this.grpThongTinHang.SuspendLayout();
            this.grpThongTinChung.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.pnlMain.Controls.Add(this.pnlSearchBottom);
            this.pnlMain.Controls.Add(this.pnlButtonsRow);
            this.pnlMain.Controls.Add(this.lblBangChu);
            this.pnlMain.Controls.Add(this.lblTongTien);
            this.pnlMain.Controls.Add(this.txtTongTien);
            this.pnlMain.Controls.Add(this.lblHuongDanXoa);
            this.pnlMain.Controls.Add(this.dgvGioHang);
            this.pnlMain.Controls.Add(this.grpThongTinHang);
            this.pnlMain.Controls.Add(this.grpThongTinChung);
            this.pnlMain.Controls.Add(this.lblTitle);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(230, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(16, 8, 16, 16);
            this.pnlMain.Size = new System.Drawing.Size(1628, 755);
            this.pnlMain.TabIndex = 1;
            // 
            // pnlSearchBottom
            // 
            this.pnlSearchBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSearchBottom.Controls.Add(this.lblMaHDTim);
            this.pnlSearchBottom.Controls.Add(this.cboMaHDTim);
            this.pnlSearchBottom.Controls.Add(this.btnTimKiem);
            this.pnlSearchBottom.Location = new System.Drawing.Point(16, 675);
            this.pnlSearchBottom.Name = "pnlSearchBottom";
            this.pnlSearchBottom.Size = new System.Drawing.Size(1583, 45);
            this.pnlSearchBottom.TabIndex = 9;
            // 
            // lblMaHDTim
            // 
            this.lblMaHDTim.AutoSize = true;
            this.lblMaHDTim.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaHDTim.ForeColor = System.Drawing.Color.Black;
            this.lblMaHDTim.Location = new System.Drawing.Point(16, 12);
            this.lblMaHDTim.Name = "lblMaHDTim";
            this.lblMaHDTim.Size = new System.Drawing.Size(107, 23);
            this.lblMaHDTim.TabIndex = 0;
            this.lblMaHDTim.Text = "Mã hóa đơn:";
            // 
            // cboMaHDTim
            // 
            this.cboMaHDTim.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboMaHDTim.FormattingEnabled = true;
            this.cboMaHDTim.Location = new System.Drawing.Point(115, 10);
            this.cboMaHDTim.Name = "cboMaHDTim";
            this.cboMaHDTim.Size = new System.Drawing.Size(200, 29);
            this.cboMaHDTim.TabIndex = 1;
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(240)))), ((int)(((byte)(250)))));
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnTimKiem.ForeColor = System.Drawing.Color.Black;
            this.btnTimKiem.Location = new System.Drawing.Point(325, 9);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(100, 27);
            this.btnTimKiem.TabIndex = 2;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            // 
            // pnlButtonsRow
            // 
            this.pnlButtonsRow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButtonsRow.Controls.Add(this.btnThemHD);
            this.pnlButtonsRow.Controls.Add(this.btnSuaHD);
            this.pnlButtonsRow.Controls.Add(this.btnXoaHD);
            this.pnlButtonsRow.Controls.Add(this.btnLuuHD);
            this.pnlButtonsRow.Controls.Add(this.btnHuy);
            this.pnlButtonsRow.Controls.Add(this.btnHuyHD);
            this.pnlButtonsRow.Controls.Add(this.btnInHD);
            this.pnlButtonsRow.Controls.Add(this.btnDong);
            this.pnlButtonsRow.Location = new System.Drawing.Point(16, 622);
            this.pnlButtonsRow.Name = "pnlButtonsRow";
            this.pnlButtonsRow.Size = new System.Drawing.Size(1583, 42);
            this.pnlButtonsRow.TabIndex = 8;
            // 
            // btnThemHD
            // 
            this.btnThemHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.btnThemHD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemHD.FlatAppearance.BorderSize = 0;
            this.btnThemHD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemHD.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnThemHD.ForeColor = System.Drawing.Color.White;
            this.btnThemHD.Location = new System.Drawing.Point(10, 3);
            this.btnThemHD.Name = "btnThemHD";
            this.btnThemHD.Size = new System.Drawing.Size(115, 36);
            this.btnThemHD.TabIndex = 0;
            this.btnThemHD.Text = "Thêm hóa đơn";
            this.btnThemHD.UseVisualStyleBackColor = false;
            // 
            // btnSuaHD
            // 
            this.btnSuaHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnSuaHD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSuaHD.FlatAppearance.BorderSize = 0;
            this.btnSuaHD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSuaHD.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSuaHD.ForeColor = System.Drawing.Color.White;
            this.btnSuaHD.Location = new System.Drawing.Point(0, 3);
            this.btnSuaHD.Name = "btnSuaHD";
            this.btnSuaHD.Size = new System.Drawing.Size(100, 36);
            this.btnSuaHD.TabIndex = 1;
            this.btnSuaHD.Text = "Sửa";
            this.btnSuaHD.UseVisualStyleBackColor = false;
            this.btnSuaHD.Visible = false;
            // 
            // btnXoaHD
            // 
            this.btnXoaHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnXoaHD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoaHD.FlatAppearance.BorderSize = 0;
            this.btnXoaHD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaHD.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnXoaHD.ForeColor = System.Drawing.Color.White;
            this.btnXoaHD.Location = new System.Drawing.Point(0, 3);
            this.btnXoaHD.Name = "btnXoaHD";
            this.btnXoaHD.Size = new System.Drawing.Size(100, 36);
            this.btnXoaHD.TabIndex = 2;
            this.btnXoaHD.Text = "Xóa";
            this.btnXoaHD.UseVisualStyleBackColor = false;
            this.btnXoaHD.Visible = false;
            // 
            // btnLuuHD
            // 
            this.btnLuuHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnLuuHD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuuHD.FlatAppearance.BorderSize = 0;
            this.btnLuuHD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuuHD.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLuuHD.ForeColor = System.Drawing.Color.White;
            this.btnLuuHD.Location = new System.Drawing.Point(135, 3);
            this.btnLuuHD.Name = "btnLuuHD";
            this.btnLuuHD.Size = new System.Drawing.Size(80, 36);
            this.btnLuuHD.TabIndex = 3;
            this.btnLuuHD.Text = "Lưu";
            this.btnLuuHD.UseVisualStyleBackColor = false;
            this.btnLuuHD.Visible = true;
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.FlatAppearance.BorderSize = 0;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(225, 3);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(80, 36);
            this.btnHuy.TabIndex = 4;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Visible = true;
            // 
            // btnHuyHD
            // 
            this.btnHuyHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnHuyHD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuyHD.FlatAppearance.BorderSize = 0;
            this.btnHuyHD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuyHD.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnHuyHD.ForeColor = System.Drawing.Color.White;
            this.btnHuyHD.Location = new System.Drawing.Point(315, 3);
            this.btnHuyHD.Name = "btnHuyHD";
            this.btnHuyHD.Size = new System.Drawing.Size(115, 36);
            this.btnHuyHD.TabIndex = 5;
            this.btnHuyHD.Text = "Hủy hóa đơn";
            this.btnHuyHD.UseVisualStyleBackColor = false;
            this.btnHuyHD.Visible = true;
            // 
            // btnInHD
            // 
            this.btnInHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.btnInHD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInHD.FlatAppearance.BorderSize = 0;
            this.btnInHD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInHD.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnInHD.ForeColor = System.Drawing.Color.White;
            this.btnInHD.Location = new System.Drawing.Point(440, 3);
            this.btnInHD.Name = "btnInHD";
            this.btnInHD.Size = new System.Drawing.Size(105, 36);
            this.btnInHD.TabIndex = 6;
            this.btnInHD.Text = "In hóa đơn";
            this.btnInHD.UseVisualStyleBackColor = false;
            this.btnInHD.Visible = false;
            // 
            // btnDong
            // 
            this.btnDong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnDong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDong.FlatAppearance.BorderSize = 0;
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDong.ForeColor = System.Drawing.Color.White;
            this.btnDong.Location = new System.Drawing.Point(440, 3);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(85, 36);
            this.btnDong.TabIndex = 7;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = false;
            // 
            // lblBangChu
            // 
            this.lblBangChu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBangChu.AutoSize = true;
            this.lblBangChu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBangChu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(205)))));
            this.lblBangChu.Location = new System.Drawing.Point(16, 595);
            this.lblBangChu.Name = "lblBangChu";
            this.lblBangChu.Size = new System.Drawing.Size(166, 21);
            this.lblBangChu.TabIndex = 7;
            this.lblBangChu.Text = "Bằng chữ: Không đồng";
            // 
            // lblTongTien
            // 
            this.lblTongTien.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
            this.lblTongTien.Location = new System.Drawing.Point(1293, 569);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(87, 23);
            this.lblTongTien.TabIndex = 5;
            this.lblTongTien.Text = "Tổng tiền:";
            // 
            // txtTongTien
            // 
            this.txtTongTien.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTongTien.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.txtTongTien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtTongTien.ForeColor = System.Drawing.Color.Black;
            this.txtTongTien.Location = new System.Drawing.Point(1370, 566);
            this.txtTongTien.Name = "txtTongTien";
            this.txtTongTien.ReadOnly = true;
            this.txtTongTien.Size = new System.Drawing.Size(229, 30);
            this.txtTongTien.TabIndex = 6;
            this.txtTongTien.Text = "0";
            // 
            // lblHuongDanXoa
            // 
            this.lblHuongDanXoa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblHuongDanXoa.AutoSize = true;
            this.lblHuongDanXoa.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHuongDanXoa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblHuongDanXoa.Location = new System.Drawing.Point(16, 570);
            this.lblHuongDanXoa.Name = "lblHuongDanXoa";
            this.lblHuongDanXoa.Size = new System.Drawing.Size(259, 21);
            this.lblHuongDanXoa.TabIndex = 4;
            this.lblHuongDanXoa.Text = "Kích đúp vào một dòng hàng để xóa";
            // 
            // dgvGioHang
            // 
            this.dgvGioHang.AllowUserToAddRows = false;
            this.dgvGioHang.AllowUserToDeleteRows = false;
            this.dgvGioHang.AllowUserToResizeRows = false;
            this.dgvGioHang.AllowUserToResizeColumns = false;
            this.dgvGioHang.AllowUserToOrderColumns = false;
            this.dgvGioHang.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGioHang.BackgroundColor = System.Drawing.Color.White;
            this.dgvGioHang.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(245)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGioHang.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGioHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGioHang.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaHang,
            this.colTenHang,
            this.colDonGia,
            this.colSoLuong,
            this.colGiamGia,
            this.colThanhTien,
            this.colThoiGianBaoHanh});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGioHang.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGioHang.EnableHeadersVisualStyles = false;
            this.dgvGioHang.Location = new System.Drawing.Point(16, 340);
            this.dgvGioHang.Name = "dgvGioHang";
            this.dgvGioHang.ReadOnly = true;
            this.dgvGioHang.RowHeadersWidth = 30;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvGioHang.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvGioHang.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGioHang.Size = new System.Drawing.Size(1583, 220);
            this.dgvGioHang.TabIndex = 3;
            // 
            // colMaHang
            // 
            this.colMaHang.DataPropertyName = "MaLaptop";
            this.colMaHang.HeaderText = "Mã hàng";
            this.colMaHang.MinimumWidth = 6;
            this.colMaHang.Name = "colMaHang";
            this.colMaHang.ReadOnly = true;
            this.colMaHang.Width = 90;
            // 
            // colTenHang
            // 
            this.colTenHang.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTenHang.DataPropertyName = "TenLaptop";
            this.colTenHang.HeaderText = "Tên hàng";
            this.colTenHang.MinimumWidth = 6;
            this.colTenHang.Name = "colTenHang";
            this.colTenHang.ReadOnly = true;
            // 
            // colDonGia
            // 
            this.colDonGia.DataPropertyName = "DonGia";
            this.colDonGia.HeaderText = "Đơn giá";
            this.colDonGia.MinimumWidth = 6;
            this.colDonGia.Name = "colDonGia";
            this.colDonGia.ReadOnly = true;
            this.colDonGia.Width = 115;
            // 
            // colSoLuong
            // 
            this.colSoLuong.DataPropertyName = "SoLuong";
            this.colSoLuong.HeaderText = "Số lượng";
            this.colSoLuong.MinimumWidth = 6;
            this.colSoLuong.Name = "colSoLuong";
            this.colSoLuong.ReadOnly = true;
            this.colSoLuong.Width = 75;
            // 
            // colGiamGia
            // 
            this.colGiamGia.DataPropertyName = "GiamGia";
            this.colGiamGia.HeaderText = "Giảm giá %";
            this.colGiamGia.MinimumWidth = 6;
            this.colGiamGia.Name = "colGiamGia";
            this.colGiamGia.ReadOnly = true;
            this.colGiamGia.Width = 85;
            // 
            // colThanhTien
            // 
            this.colThanhTien.DataPropertyName = "ThanhTien";
            this.colThanhTien.HeaderText = "Thành tiền";
            this.colThanhTien.MinimumWidth = 6;
            this.colThanhTien.Name = "colThanhTien";
            this.colThanhTien.ReadOnly = true;
            this.colThanhTien.Width = 125;
            // 
            // colThoiGianBaoHanh
            // 
            this.colThoiGianBaoHanh.DataPropertyName = "ThoiGianBaoHanh";
            this.colThoiGianBaoHanh.HeaderText = "Bảo hành (tháng)";
            this.colThoiGianBaoHanh.MinimumWidth = 6;
            this.colThoiGianBaoHanh.Name = "colThoiGianBaoHanh";
            this.colThoiGianBaoHanh.ReadOnly = true;
            this.colThoiGianBaoHanh.Width = 120;
            // 
            // grpThongTinHang
            // 
            this.grpThongTinHang.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpThongTinHang.BackColor = System.Drawing.Color.White;
            this.grpThongTinHang.Controls.Add(this.lblMaHang);
            this.grpThongTinHang.Controls.Add(this.cboMaHang);
            this.grpThongTinHang.Controls.Add(this.btnTimKiemSP);
            this.grpThongTinHang.Controls.Add(this.lblTenHang);
            this.grpThongTinHang.Controls.Add(this.txtTenHang);
            this.grpThongTinHang.Controls.Add(this.lblDonGia);
            this.grpThongTinHang.Controls.Add(this.txtDonGia);
            this.grpThongTinHang.Controls.Add(this.lblSoLuong);
            this.grpThongTinHang.Controls.Add(this.txtSoLuong);
            this.grpThongTinHang.Controls.Add(this.lblGiamGia);
            this.grpThongTinHang.Controls.Add(this.txtGiamGia);
            this.grpThongTinHang.Controls.Add(this.lblPhanTram);
            this.grpThongTinHang.Controls.Add(this.lblBaoHanh);
            this.grpThongTinHang.Controls.Add(this.txtBaoHanh);
            this.grpThongTinHang.Controls.Add(this.lblThang);
            this.grpThongTinHang.Controls.Add(this.lblThanhTien);
            this.grpThongTinHang.Controls.Add(this.txtThanhTien);
            this.grpThongTinHang.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpThongTinHang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.grpThongTinHang.Location = new System.Drawing.Point(16, 238);
            this.grpThongTinHang.Name = "grpThongTinHang";
            this.grpThongTinHang.Size = new System.Drawing.Size(1583, 96);
            this.grpThongTinHang.TabIndex = 2;
            this.grpThongTinHang.TabStop = false;
            this.grpThongTinHang.Text = "Thông tin các mặt hàng";
            // 
            // lblMaHang
            // 
            this.lblMaHang.AutoSize = true;
            this.lblMaHang.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMaHang.ForeColor = System.Drawing.Color.Black;
            this.lblMaHang.Location = new System.Drawing.Point(-200, -200);
            this.lblMaHang.Name = "lblMaHang";
            this.lblMaHang.Size = new System.Drawing.Size(82, 23);
            this.lblMaHang.TabIndex = 0;
            this.lblMaHang.Text = "Mã hàng:";
            this.lblMaHang.Visible = false;
            // 
            // cboMaHang
            // 
            this.cboMaHang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaHang.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboMaHang.FormattingEnabled = true;
            this.cboMaHang.Location = new System.Drawing.Point(-200, -200);
            this.cboMaHang.Name = "cboMaHang";
            this.cboMaHang.Size = new System.Drawing.Size(10, 29);
            this.cboMaHang.TabIndex = 1;
            this.cboMaHang.Visible = false;
            // 
            // btnTimKiemSP
            // 
            this.btnTimKiemSP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.btnTimKiemSP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimKiemSP.FlatAppearance.BorderSize = 0;
            this.btnTimKiemSP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiemSP.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTimKiemSP.ForeColor = System.Drawing.Color.White;
            this.btnTimKiemSP.Location = new System.Drawing.Point(20, 21);
            this.btnTimKiemSP.Name = "btnTimKiemSP";
            this.btnTimKiemSP.Size = new System.Drawing.Size(190, 31);
            this.btnTimKiemSP.TabIndex = 2;
            this.btnTimKiemSP.Text = "🔍 Tìm máy";
            this.btnTimKiemSP.UseVisualStyleBackColor = false;
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSoLuong.ForeColor = System.Drawing.Color.Black;
            this.lblSoLuong.Location = new System.Drawing.Point(20, 58);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(82, 23);
            this.lblSoLuong.TabIndex = 3;
            this.lblSoLuong.Text = "Số lượng:";
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSoLuong.Location = new System.Drawing.Point(95, 55);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(115, 29);
            this.txtSoLuong.TabIndex = 4;
            this.txtSoLuong.Text = "1";
            // 
            // lblTenHang
            // 
            this.lblTenHang.AutoSize = true;
            this.lblTenHang.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTenHang.ForeColor = System.Drawing.Color.Black;
            this.lblTenHang.Location = new System.Drawing.Point(335, 25);
            this.lblTenHang.Name = "lblTenHang";
            this.lblTenHang.Size = new System.Drawing.Size(84, 23);
            this.lblTenHang.TabIndex = 5;
            this.lblTenHang.Text = "Tên hàng:";
            // 
            // txtTenHang
            // 
            this.txtTenHang.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtTenHang.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTenHang.Location = new System.Drawing.Point(415, 22);
            this.txtTenHang.Name = "txtTenHang";
            this.txtTenHang.ReadOnly = true;
            this.txtTenHang.Size = new System.Drawing.Size(315, 29);
            this.txtTenHang.TabIndex = 6;
            // 
            // lblGiamGia
            // 
            this.lblGiamGia.AutoSize = true;
            this.lblGiamGia.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblGiamGia.ForeColor = System.Drawing.Color.Black;
            this.lblGiamGia.Location = new System.Drawing.Point(335, 58);
            this.lblGiamGia.Name = "lblGiamGia";
            this.lblGiamGia.Size = new System.Drawing.Size(82, 23);
            this.lblGiamGia.TabIndex = 7;
            this.lblGiamGia.Text = "Giảm giá:";
            // 
            // txtGiamGia
            // 
            this.txtGiamGia.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtGiamGia.Location = new System.Drawing.Point(415, 55);
            this.txtGiamGia.Name = "txtGiamGia";
            this.txtGiamGia.Size = new System.Drawing.Size(55, 29);
            this.txtGiamGia.TabIndex = 8;
            this.txtGiamGia.Text = "0";
            // 
            // lblPhanTram
            // 
            this.lblPhanTram.AutoSize = true;
            this.lblPhanTram.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPhanTram.ForeColor = System.Drawing.Color.Black;
            this.lblPhanTram.Location = new System.Drawing.Point(473, 58);
            this.lblPhanTram.Name = "lblPhanTram";
            this.lblPhanTram.Size = new System.Drawing.Size(25, 23);
            this.lblPhanTram.TabIndex = 9;
            this.lblPhanTram.Text = "%";
            // 
            // lblBaoHanh
            // 
            this.lblBaoHanh.AutoSize = true;
            this.lblBaoHanh.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblBaoHanh.ForeColor = System.Drawing.Color.Black;
            this.lblBaoHanh.Location = new System.Drawing.Point(520, 58);
            this.lblBaoHanh.Name = "lblBaoHanh";
            this.lblBaoHanh.Size = new System.Drawing.Size(87, 23);
            this.lblBaoHanh.TabIndex = 10;
            this.lblBaoHanh.Text = "Bảo hành:";
            this.lblBaoHanh.Click += new System.EventHandler(this.lblBaoHanh_Click);
            // 
            // txtBaoHanh
            // 
            this.txtBaoHanh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtBaoHanh.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtBaoHanh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
            this.txtBaoHanh.Location = new System.Drawing.Point(595, 55);
            this.txtBaoHanh.Name = "txtBaoHanh";
            this.txtBaoHanh.ReadOnly = true;
            this.txtBaoHanh.Size = new System.Drawing.Size(55, 29);
            this.txtBaoHanh.TabIndex = 11;
            this.txtBaoHanh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblThang
            // 
            this.lblThang.AutoSize = true;
            this.lblThang.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblThang.ForeColor = System.Drawing.Color.Black;
            this.lblThang.Location = new System.Drawing.Point(655, 58);
            this.lblThang.Name = "lblThang";
            this.lblThang.Size = new System.Drawing.Size(55, 23);
            this.lblThang.TabIndex = 12;
            this.lblThang.Text = "tháng";
            // 
            // lblDonGia
            // 
            this.lblDonGia.AutoSize = true;
            this.lblDonGia.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDonGia.ForeColor = System.Drawing.Color.Black;
            this.lblDonGia.Location = new System.Drawing.Point(755, 25);
            this.lblDonGia.Name = "lblDonGia";
            this.lblDonGia.Size = new System.Drawing.Size(74, 23);
            this.lblDonGia.TabIndex = 13;
            this.lblDonGia.Text = "Đơn giá:";
            // 
            // txtDonGia
            // 
            this.txtDonGia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtDonGia.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDonGia.Location = new System.Drawing.Point(835, 22);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.ReadOnly = true;
            this.txtDonGia.Size = new System.Drawing.Size(145, 29);
            this.txtDonGia.TabIndex = 14;
            // 
            // lblThanhTien
            // 
            this.lblThanhTien.AutoSize = true;
            this.lblThanhTien.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblThanhTien.ForeColor = System.Drawing.Color.Black;
            this.lblThanhTien.Location = new System.Drawing.Point(755, 58);
            this.lblThanhTien.Name = "lblThanhTien";
            this.lblThanhTien.Size = new System.Drawing.Size(96, 23);
            this.lblThanhTien.TabIndex = 15;
            this.lblThanhTien.Text = "Thành tiền:";
            // 
            // txtThanhTien
            // 
            this.txtThanhTien.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtThanhTien.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtThanhTien.Location = new System.Drawing.Point(835, 55);
            this.txtThanhTien.Name = "txtThanhTien";
            this.txtThanhTien.ReadOnly = true;
            this.txtThanhTien.Size = new System.Drawing.Size(145, 29);
            this.txtThanhTien.TabIndex = 16;
            // 
            // grpThongTinChung
            // 
            this.grpThongTinChung.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpThongTinChung.BackColor = System.Drawing.Color.White;
            this.grpThongTinChung.Controls.Add(this.lblMaHD);
            this.grpThongTinChung.Controls.Add(this.txtMaHD);
            this.grpThongTinChung.Controls.Add(this.lblNgayBan);
            this.grpThongTinChung.Controls.Add(this.dtpNgayBan);
            this.grpThongTinChung.Controls.Add(this.lblMaNV);
            this.grpThongTinChung.Controls.Add(this.txtMaNV);
            this.grpThongTinChung.Controls.Add(this.btnChonNV);
            this.grpThongTinChung.Controls.Add(this.lblTenNV);
            this.grpThongTinChung.Controls.Add(this.txtTenNV);
            this.grpThongTinChung.Controls.Add(this.lblMaKH);
            this.grpThongTinChung.Controls.Add(this.cboMaKH);
            this.grpThongTinChung.Controls.Add(this.btnChonKH);
            this.grpThongTinChung.Controls.Add(this.lblTenKhach);
            this.grpThongTinChung.Controls.Add(this.txtTenKhach);
            this.grpThongTinChung.Controls.Add(this.lblHangKhach);
            this.grpThongTinChung.Controls.Add(this.txtHangKhach);
            this.grpThongTinChung.Controls.Add(this.lblDiaChi);
            this.grpThongTinChung.Controls.Add(this.txtDiaChi);
            this.grpThongTinChung.Controls.Add(this.lblSDT);
            this.grpThongTinChung.Controls.Add(this.txtSDT);
            this.grpThongTinChung.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpThongTinChung.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.grpThongTinChung.Location = new System.Drawing.Point(16, 45);
            this.grpThongTinChung.Name = "grpThongTinChung";
            this.grpThongTinChung.Size = new System.Drawing.Size(1583, 185);
            this.grpThongTinChung.TabIndex = 1;
            this.grpThongTinChung.TabStop = false;
            this.grpThongTinChung.Text = "Thông tin chung";
            // 
            // lblMaHD
            // 
            this.lblMaHD.AutoSize = true;
            this.lblMaHD.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMaHD.ForeColor = System.Drawing.Color.Black;
            this.lblMaHD.Location = new System.Drawing.Point(20, 25);
            this.lblMaHD.Name = "lblMaHD";
            this.lblMaHD.Size = new System.Drawing.Size(107, 23);
            this.lblMaHD.TabIndex = 0;
            this.lblMaHD.Text = "Mã hóa đơn:";
            // 
            // txtMaHD
            // 
            this.txtMaHD.BackColor = System.Drawing.Color.White;
            this.txtMaHD.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtMaHD.Location = new System.Drawing.Point(120, 22);
            this.txtMaHD.Name = "txtMaHD";
            this.txtMaHD.ReadOnly = true;
            this.txtMaHD.Size = new System.Drawing.Size(260, 29);
            this.txtMaHD.TabIndex = 1;
            // 
            // lblNgayBan
            // 
            this.lblNgayBan.AutoSize = true;
            this.lblNgayBan.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblNgayBan.ForeColor = System.Drawing.Color.Black;
            this.lblNgayBan.Location = new System.Drawing.Point(20, 58);
            this.lblNgayBan.Name = "lblNgayBan";
            this.lblNgayBan.Size = new System.Drawing.Size(88, 23);
            this.lblNgayBan.TabIndex = 2;
            this.lblNgayBan.Text = "Ngày bán:";
            // 
            // dtpNgayBan
            // 
            this.dtpNgayBan.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayBan.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpNgayBan.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayBan.Location = new System.Drawing.Point(120, 55);
            this.dtpNgayBan.Name = "dtpNgayBan";
            this.dtpNgayBan.Size = new System.Drawing.Size(260, 29);
            this.dtpNgayBan.TabIndex = 3;
            // 
            // lblMaNV
            // 
            this.lblMaNV.AutoSize = true;
            this.lblMaNV.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMaNV.ForeColor = System.Drawing.Color.Black;
            this.lblMaNV.Location = new System.Drawing.Point(20, 91);
            this.lblMaNV.Name = "lblMaNV";
            this.lblMaNV.Size = new System.Drawing.Size(118, 23);
            this.lblMaNV.TabIndex = 4;
            this.lblMaNV.Text = "Mã nhân viên:";
            // 
            // txtMaNV
            // 
            this.txtMaNV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtMaNV.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtMaNV.Location = new System.Drawing.Point(120, 88);
            this.txtMaNV.Name = "txtMaNV";
            this.txtMaNV.ReadOnly = true;
            this.txtMaNV.Size = new System.Drawing.Size(220, 29);
            this.txtMaNV.TabIndex = 5;
            // 
            // btnChonNV
            // 
            this.btnChonNV.BackColor = System.Drawing.Color.LightGray;
            this.btnChonNV.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnChonNV.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnChonNV.Location = new System.Drawing.Point(345, 88);
            this.btnChonNV.Name = "btnChonNV";
            this.btnChonNV.Size = new System.Drawing.Size(35, 27);
            this.btnChonNV.TabIndex = 6;
            this.btnChonNV.Text = "...";
            this.btnChonNV.UseVisualStyleBackColor = false;
            // 
            // lblTenNV
            // 
            this.lblTenNV.AutoSize = true;
            this.lblTenNV.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTenNV.ForeColor = System.Drawing.Color.Black;
            this.lblTenNV.Location = new System.Drawing.Point(20, 124);
            this.lblTenNV.Name = "lblTenNV";
            this.lblTenNV.Size = new System.Drawing.Size(120, 23);
            this.lblTenNV.TabIndex = 7;
            this.lblTenNV.Text = "Tên nhân viên:";
            // 
            // txtTenNV
            // 
            this.txtTenNV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtTenNV.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTenNV.Location = new System.Drawing.Point(120, 121);
            this.txtTenNV.Name = "txtTenNV";
            this.txtTenNV.ReadOnly = true;
            this.txtTenNV.Size = new System.Drawing.Size(260, 29);
            this.txtTenNV.TabIndex = 8;
            // 
            // lblMaKH
            // 
            this.lblMaKH.AutoSize = true;
            this.lblMaKH.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMaKH.ForeColor = System.Drawing.Color.Black;
            this.lblMaKH.Location = new System.Drawing.Point(420, 25);
            this.lblMaKH.Name = "lblMaKH";
            this.lblMaKH.Size = new System.Drawing.Size(132, 23);
            this.lblMaKH.TabIndex = 9;
            this.lblMaKH.Text = "Mã khách hàng:";
            // 
            // cboMaKH
            // 
            this.cboMaKH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboMaKH.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboMaKH.FormattingEnabled = true;
            this.cboMaKH.Location = new System.Drawing.Point(540, 22);
            this.cboMaKH.Name = "cboMaKH";
            this.cboMaKH.Size = new System.Drawing.Size(260, 29);
            this.cboMaKH.TabIndex = 10;
            // 
            // btnChonKH
            // 
            this.btnChonKH.BackColor = System.Drawing.Color.LightGray;
            this.btnChonKH.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnChonKH.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnChonKH.Location = new System.Drawing.Point(805, 22);
            this.btnChonKH.Name = "btnChonKH";
            this.btnChonKH.Size = new System.Drawing.Size(35, 27);
            this.btnChonKH.TabIndex = 11;
            this.btnChonKH.Text = "...";
            this.btnChonKH.UseVisualStyleBackColor = false;
            // 
            // lblTenKhach
            // 
            this.lblTenKhach.AutoSize = true;
            this.lblTenKhach.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTenKhach.ForeColor = System.Drawing.Color.Black;
            this.lblTenKhach.Location = new System.Drawing.Point(420, 58);
            this.lblTenKhach.Name = "lblTenKhach";
            this.lblTenKhach.Size = new System.Drawing.Size(134, 23);
            this.lblTenKhach.TabIndex = 12;
            this.lblTenKhach.Text = "Tên khách hàng:";
            // 
            // txtTenKhach
            // 
            this.txtTenKhach.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtTenKhach.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTenKhach.Location = new System.Drawing.Point(540, 55);
            this.txtTenKhach.Name = "txtTenKhach";
            this.txtTenKhach.ReadOnly = true;
            this.txtTenKhach.Size = new System.Drawing.Size(300, 29);
            this.txtTenKhach.TabIndex = 13;
            // 
            // lblHangKhach
            // 
            this.lblHangKhach.AutoSize = true;
            this.lblHangKhach.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblHangKhach.ForeColor = System.Drawing.Color.Black;
            this.lblHangKhach.Location = new System.Drawing.Point(420, 91);
            this.lblHangKhach.Name = "lblHangKhach";
            this.lblHangKhach.Size = new System.Drawing.Size(105, 23);
            this.lblHangKhach.TabIndex = 14;
            this.lblHangKhach.Text = "Hạng khách:";
            // 
            // txtHangKhach
            // 
            this.txtHangKhach.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtHangKhach.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtHangKhach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtHangKhach.Location = new System.Drawing.Point(540, 88);
            this.txtHangKhach.Name = "txtHangKhach";
            this.txtHangKhach.ReadOnly = true;
            this.txtHangKhach.Size = new System.Drawing.Size(300, 29);
            this.txtHangKhach.TabIndex = 15;
            // 
            // lblDiaChi
            // 
            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDiaChi.ForeColor = System.Drawing.Color.Black;
            this.lblDiaChi.Location = new System.Drawing.Point(420, 124);
            this.lblDiaChi.Name = "lblDiaChi";
            this.lblDiaChi.Size = new System.Drawing.Size(66, 23);
            this.lblDiaChi.TabIndex = 16;
            this.lblDiaChi.Text = "Địa chỉ:";
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtDiaChi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDiaChi.Location = new System.Drawing.Point(540, 121);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.ReadOnly = true;
            this.txtDiaChi.Size = new System.Drawing.Size(300, 29);
            this.txtDiaChi.TabIndex = 17;
            // 
            // lblSDT
            // 
            this.lblSDT.AutoSize = true;
            this.lblSDT.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSDT.ForeColor = System.Drawing.Color.Black;
            this.lblSDT.Location = new System.Drawing.Point(420, 157);
            this.lblSDT.Name = "lblSDT";
            this.lblSDT.Size = new System.Drawing.Size(93, 23);
            this.lblSDT.TabIndex = 18;
            this.lblSDT.Text = "Điện thoại:";
            // 
            // txtSDT
            // 
            this.txtSDT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSDT.Location = new System.Drawing.Point(540, 154);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.ReadOnly = true;
            this.txtSDT.Size = new System.Drawing.Size(300, 29);
            this.txtSDT.TabIndex = 19;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.lblTitle.Location = new System.Drawing.Point(16, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1583, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "HÓA ĐƠN BÁN HÀNG";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sidebarControl1
            // 
            this.sidebarControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.sidebarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarControl1.Location = new System.Drawing.Point(0, 0);
            this.sidebarControl1.Name = "sidebarControl1";
            this.sidebarControl1.SelectedMenu = QuanLyBanLaptop.Classes.NavigationMenu.BanHang;
            this.sidebarControl1.Size = new System.Drawing.Size(230, 755);
            this.sidebarControl1.TabIndex = 0;
            // 
            // frmBanHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1803, 755);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.sidebarControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(1000, 700);
            this.Name = "frmBanHang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmHDBan";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlSearchBottom.ResumeLayout(false);
            this.pnlSearchBottom.PerformLayout();
            this.pnlButtonsRow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGioHang)).EndInit();
            this.grpThongTinHang.ResumeLayout(false);
            this.grpThongTinHang.PerformLayout();
            this.grpThongTinChung.ResumeLayout(false);
            this.grpThongTinChung.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SidebarControl sidebarControl1;

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpThongTinChung;
        private System.Windows.Forms.Label lblMaHD;
        private System.Windows.Forms.TextBox txtMaHD;
        private System.Windows.Forms.Label lblNgayBan;
        private System.Windows.Forms.DateTimePicker dtpNgayBan;
        private System.Windows.Forms.Label lblMaNV;
        private System.Windows.Forms.TextBox txtMaNV;
        private System.Windows.Forms.Button btnChonNV;
        private System.Windows.Forms.Label lblTenNV;
        private System.Windows.Forms.TextBox txtTenNV;
        private System.Windows.Forms.Label lblMaKH;
        private System.Windows.Forms.ComboBox cboMaKH;
        private System.Windows.Forms.Button btnChonKH;
        private System.Windows.Forms.Label lblTenKhach;
        private System.Windows.Forms.TextBox txtTenKhach;
        private System.Windows.Forms.Label lblHangKhach;
        private System.Windows.Forms.TextBox txtHangKhach;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.GroupBox grpThongTinHang;
        private System.Windows.Forms.Label lblMaHang;
        private System.Windows.Forms.ComboBox cboMaHang;
        private System.Windows.Forms.Button btnTimKiemSP;
        private System.Windows.Forms.Label lblTenHang;
        private System.Windows.Forms.TextBox txtTenHang;
        private System.Windows.Forms.Label lblBaoHanh;
        private System.Windows.Forms.TextBox txtBaoHanh;
        private System.Windows.Forms.Label lblThang;
        private System.Windows.Forms.Label lblDonGia;
        private System.Windows.Forms.TextBox txtDonGia;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Label lblGiamGia;
        private System.Windows.Forms.TextBox txtGiamGia;
        private System.Windows.Forms.Label lblPhanTram;
        private System.Windows.Forms.Label lblThanhTien;
        private System.Windows.Forms.TextBox txtThanhTien;
        private System.Windows.Forms.DataGridView dgvGioHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiamGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhTien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThoiGianBaoHanh;
        private System.Windows.Forms.Label lblHuongDanXoa;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.TextBox txtTongTien;
        private System.Windows.Forms.Label lblBangChu;
        private System.Windows.Forms.Panel pnlButtonsRow;
        private System.Windows.Forms.Button btnThemHD;
        private System.Windows.Forms.Button btnSuaHD;
        private System.Windows.Forms.Button btnXoaHD;
        private System.Windows.Forms.Button btnLuuHD;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnHuyHD;
        private System.Windows.Forms.Button btnInHD;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Panel pnlSearchBottom;
        private System.Windows.Forms.Label lblMaHDTim;
        private System.Windows.Forms.ComboBox cboMaHDTim;
        private System.Windows.Forms.Button btnTimKiem;
    }
}
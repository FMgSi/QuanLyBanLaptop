namespace QuanLyBanLaptop.DanhMuc
{
    partial class frmCauHinhTraGop
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel rootLayout;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.GroupBox grpThongTin;
        private System.Windows.Forms.TableLayoutPanel infoLayout;
        private System.Windows.Forms.Label lblChonThamSo;
        private System.Windows.Forms.ComboBox cboThamSo;
        private System.Windows.Forms.Label lblMaThamSo;
        private System.Windows.Forms.TextBox txtKhoa;
        private System.Windows.Forms.Label lblGiaTri;
        private System.Windows.Forms.TextBox txtGiaTri;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.TextBox txtMoTa;
        private System.Windows.Forms.GroupBox grpChiTiet;
        private System.Windows.Forms.DataGridView dgvCauHinh;
        private System.Windows.Forms.GroupBox grpThaoTac;
        private System.Windows.Forms.TableLayoutPanel footerLayout;
        private System.Windows.Forms.Label lblGhiChu;
        private System.Windows.Forms.TableLayoutPanel buttonLayout;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            this.rootLayout = new System.Windows.Forms.TableLayoutPanel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.grpThongTin = new System.Windows.Forms.GroupBox();
            this.infoLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblChonThamSo = new System.Windows.Forms.Label();
            this.cboThamSo = new System.Windows.Forms.ComboBox();
            this.lblMaThamSo = new System.Windows.Forms.Label();
            this.lblGiaTri = new System.Windows.Forms.Label();
            this.txtGiaTri = new System.Windows.Forms.TextBox();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.txtKhoa = new System.Windows.Forms.TextBox();
            this.grpChiTiet = new System.Windows.Forms.GroupBox();
            this.dgvCauHinh = new System.Windows.Forms.DataGridView();
            this.MoTa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Khoa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GiaTri = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DonVi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpThaoTac = new System.Windows.Forms.GroupBox();
            this.footerLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.buttonLayout = new System.Windows.Forms.TableLayoutPanel();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.rootLayout.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.grpThongTin.SuspendLayout();
            this.infoLayout.SuspendLayout();
            this.grpChiTiet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCauHinh)).BeginInit();
            this.grpThaoTac.SuspendLayout();
            this.footerLayout.SuspendLayout();
            this.buttonLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // rootLayout
            // 
            this.rootLayout.ColumnCount = 1;
            this.rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.Controls.Add(this.headerPanel, 0, 0);
            this.rootLayout.Controls.Add(this.grpThongTin, 0, 1);
            this.rootLayout.Controls.Add(this.grpChiTiet, 0, 2);
            this.rootLayout.Controls.Add(this.grpThaoTac, 0, 3);
            this.rootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootLayout.Location = new System.Drawing.Point(0, 0);
            this.rootLayout.Name = "rootLayout";
            this.rootLayout.Padding = new System.Windows.Forms.Padding(24);
            this.rootLayout.RowCount = 4;
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 199F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.rootLayout.Size = new System.Drawing.Size(1100, 740);
            this.rootLayout.TabIndex = 0;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(64)))), ((int)(((byte)(139)))));
            this.headerPanel.Controls.Add(this.lblRole);
            this.headerPanel.Controls.Add(this.lblHeader);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerPanel.Location = new System.Drawing.Point(27, 27);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1046, 42);
            this.headerPanel.TabIndex = 0;
            // 
            // lblRole
            // 
            this.lblRole.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRole.ForeColor = System.Drawing.Color.White;
            this.lblRole.Location = new System.Drawing.Point(886, 0);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(160, 42);
            this.lblRole.TabIndex = 0;
            this.lblRole.Text = "Quản trị viên";
            this.lblRole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHeader
            // 
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.lblHeader.Size = new System.Drawing.Size(690, 42);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "▦  Cấu hình Trả Góp & Hạng Thành Viên (Bảng CAU_HINH)";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpThongTin
            // 
            this.grpThongTin.Controls.Add(this.infoLayout);
            this.grpThongTin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpThongTin.Location = new System.Drawing.Point(27, 75);
            this.grpThongTin.Name = "grpThongTin";
            this.grpThongTin.Padding = new System.Windows.Forms.Padding(14);
            this.grpThongTin.Size = new System.Drawing.Size(1046, 204);
            this.grpThongTin.TabIndex = 1;
            this.grpThongTin.TabStop = false;
            this.grpThongTin.Text = "Thông tin tham số";
            // 
            // infoLayout
            // 
            this.infoLayout.ColumnCount = 4;
            this.infoLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.infoLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.infoLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.infoLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.infoLayout.Controls.Add(this.lblChonThamSo, 0, 0);
            this.infoLayout.Controls.Add(this.cboThamSo, 1, 0);
            this.infoLayout.Controls.Add(this.lblMaThamSo, 0, 1);
            this.infoLayout.Controls.Add(this.lblGiaTri, 2, 1);
            this.infoLayout.Controls.Add(this.txtGiaTri, 3, 1);
            this.infoLayout.Controls.Add(this.lblMoTa, 0, 2);
            this.infoLayout.Controls.Add(this.txtMoTa, 1, 2);
            this.infoLayout.Controls.Add(this.txtKhoa, 1, 1);
            this.infoLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoLayout.Location = new System.Drawing.Point(14, 31);
            this.infoLayout.Name = "infoLayout";
            this.infoLayout.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.infoLayout.RowCount = 3;
            this.infoLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.infoLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.infoLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.infoLayout.Size = new System.Drawing.Size(1018, 159);
            this.infoLayout.TabIndex = 0;
            // 
            // lblChonThamSo
            // 
            this.lblChonThamSo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChonThamSo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(78)))));
            this.lblChonThamSo.Location = new System.Drawing.Point(3, 4);
            this.lblChonThamSo.Name = "lblChonThamSo";
            this.lblChonThamSo.Size = new System.Drawing.Size(119, 48);
            this.lblChonThamSo.TabIndex = 0;
            this.lblChonThamSo.Text = "Chọn kì hạn:";
            this.lblChonThamSo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboThamSo
            // 
            this.infoLayout.SetColumnSpan(this.cboThamSo, 3);
            this.cboThamSo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboThamSo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboThamSo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboThamSo.Items.AddRange(new object[] {
            "-- Chọn nhanh tham số cấu hình hệ thống --"});
            this.cboThamSo.Location = new System.Drawing.Point(128, 10);
            this.cboThamSo.Margin = new System.Windows.Forms.Padding(3, 11, 12, 12);
            this.cboThamSo.Name = "cboThamSo";
            this.cboThamSo.Size = new System.Drawing.Size(878, 25);
            this.cboThamSo.TabIndex = 1;
            // 
            // lblMaThamSo
            // 
            this.lblMaThamSo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMaThamSo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(78)))));
            this.lblMaThamSo.Location = new System.Drawing.Point(3, 52);
            this.lblMaThamSo.Name = "lblMaThamSo";
            this.lblMaThamSo.Size = new System.Drawing.Size(119, 48);
            this.lblMaThamSo.TabIndex = 2;
            this.lblMaThamSo.Text = "Mã tham số:";
            this.lblMaThamSo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGiaTri
            // 
            this.lblGiaTri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGiaTri.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(78)))));
            this.lblGiaTri.Location = new System.Drawing.Point(529, 52);
            this.lblGiaTri.Name = "lblGiaTri";
            this.lblGiaTri.Size = new System.Drawing.Size(84, 48);
            this.lblGiaTri.TabIndex = 4;
            this.lblGiaTri.Text = "Giá trị:";
            this.lblGiaTri.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtGiaTri
            // 
            this.txtGiaTri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGiaTri.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtGiaTri.Location = new System.Drawing.Point(619, 58);
            this.txtGiaTri.Margin = new System.Windows.Forms.Padding(3, 11, 12, 12);
            this.txtGiaTri.Name = "txtGiaTri";
            this.txtGiaTri.ReadOnly = true;
            this.txtGiaTri.Size = new System.Drawing.Size(387, 25);
            this.txtGiaTri.TabIndex = 5;
            this.txtGiaTri.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblMoTa
            // 
            this.lblMoTa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMoTa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(78)))));
            this.lblMoTa.Location = new System.Drawing.Point(3, 100);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new System.Drawing.Size(119, 59);
            this.lblMoTa.TabIndex = 6;
            this.lblMoTa.Text = "Mô tả / Quy định:";
            this.lblMoTa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtMoTa
            // 
            this.infoLayout.SetColumnSpan(this.txtMoTa, 3);
            this.txtMoTa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMoTa.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMoTa.Location = new System.Drawing.Point(128, 106);
            this.txtMoTa.Margin = new System.Windows.Forms.Padding(3, 11, 12, 12);
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.ReadOnly = true;
            this.txtMoTa.Size = new System.Drawing.Size(878, 25);
            this.txtMoTa.TabIndex = 7;
            // 
            // txtKhoa
            // 
            this.txtKhoa.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtKhoa.Location = new System.Drawing.Point(128, 58);
            this.txtKhoa.Margin = new System.Windows.Forms.Padding(3, 11, 12, 12);
            this.txtKhoa.Name = "txtKhoa";
            this.txtKhoa.ReadOnly = true;
            this.txtKhoa.Size = new System.Drawing.Size(386, 25);
            this.txtKhoa.TabIndex = 3;
            // 
            // grpChiTiet
            // 
            this.grpChiTiet.Controls.Add(this.dgvCauHinh);
            this.grpChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpChiTiet.Location = new System.Drawing.Point(27, 285);
            this.grpChiTiet.Name = "grpChiTiet";
            this.grpChiTiet.Padding = new System.Windows.Forms.Padding(12);
            this.grpChiTiet.Size = new System.Drawing.Size(1046, 302);
            this.grpChiTiet.TabIndex = 2;
            this.grpChiTiet.TabStop = false;
            this.grpChiTiet.Text = "Thông tin quy định chi tiết";
            // 
            // dgvCauHinh
            // 
            this.dgvCauHinh.AllowUserToAddRows = false;
            this.dgvCauHinh.AllowUserToDeleteRows = false;
            this.dgvCauHinh.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCauHinh.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(64)))), ((int)(((byte)(139)))));
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCauHinh.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.dgvCauHinh.ColumnHeadersHeight = 38;
            this.dgvCauHinh.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MoTa,
            this.Khoa,
            this.GiaTri,
            this.DonVi});
            this.dgvCauHinh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCauHinh.EnableHeadersVisualStyles = false;
            this.dgvCauHinh.Location = new System.Drawing.Point(12, 29);
            this.dgvCauHinh.MultiSelect = false;
            this.dgvCauHinh.Name = "dgvCauHinh";
            this.dgvCauHinh.ReadOnly = true;
            this.dgvCauHinh.RowHeadersVisible = false;
            this.dgvCauHinh.RowTemplate.Height = 36;
            this.dgvCauHinh.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCauHinh.Size = new System.Drawing.Size(1022, 261);
            this.dgvCauHinh.TabIndex = 0;
            // 
            // MoTa
            // 
            this.MoTa.FillWeight = 48F;
            this.MoTa.HeaderText = "Quy định / Mô tả";
            this.MoTa.Name = "MoTa";
            this.MoTa.ReadOnly = true;
            // 
            // Khoa
            // 
            this.Khoa.FillWeight = 24F;
            this.Khoa.HeaderText = "Mã tham số (Khoa)";
            this.Khoa.Name = "Khoa";
            this.Khoa.ReadOnly = true;
            // 
            // GiaTri
            // 
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.GiaTri.DefaultCellStyle = dataGridViewCellStyle20;
            this.GiaTri.FillWeight = 13F;
            this.GiaTri.HeaderText = "Giá trị";
            this.GiaTri.Name = "GiaTri";
            this.GiaTri.ReadOnly = true;
            // 
            // DonVi
            // 
            this.DonVi.FillWeight = 15F;
            this.DonVi.HeaderText = "Đơn vị";
            this.DonVi.Name = "DonVi";
            this.DonVi.ReadOnly = true;
            // 
            // grpThaoTac
            // 
            this.grpThaoTac.Controls.Add(this.footerLayout);
            this.grpThaoTac.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpThaoTac.Location = new System.Drawing.Point(27, 593);
            this.grpThaoTac.Name = "grpThaoTac";
            this.grpThaoTac.Padding = new System.Windows.Forms.Padding(12);
            this.grpThaoTac.Size = new System.Drawing.Size(1046, 120);
            this.grpThaoTac.TabIndex = 3;
            this.grpThaoTac.TabStop = false;
            this.grpThaoTac.Text = "Thao tác";
            // 
            // footerLayout
            // 
            this.footerLayout.ColumnCount = 1;
            this.footerLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.footerLayout.Controls.Add(this.lblGhiChu, 0, 0);
            this.footerLayout.Controls.Add(this.buttonLayout, 0, 1);
            this.footerLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.footerLayout.Location = new System.Drawing.Point(12, 29);
            this.footerLayout.Name = "footerLayout";
            this.footerLayout.RowCount = 2;
            this.footerLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.footerLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.footerLayout.Size = new System.Drawing.Size(1022, 79);
            this.footerLayout.TabIndex = 0;
            // 
            // lblGhiChu
            // 
            this.lblGhiChu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblGhiChu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(105)))), ((int)(((byte)(140)))));
            this.lblGhiChu.Location = new System.Drawing.Point(3, 0);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Size = new System.Drawing.Size(1016, 26);
            this.lblGhiChu.TabIndex = 0;
            this.lblGhiChu.Text = "* Dữ liệu được đọc và cập nhật trực tiếp trong SQL Server (\'CAU_HINH\').";
            this.lblGhiChu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonLayout
            // 
            this.buttonLayout.ColumnCount = 4;
            this.buttonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.buttonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.buttonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.buttonLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.buttonLayout.Controls.Add(this.btnLamMoi, 0, 0);
            this.buttonLayout.Controls.Add(this.btnSua, 1, 0);
            this.buttonLayout.Controls.Add(this.btnLuu, 2, 0);
            this.buttonLayout.Controls.Add(this.btnHuy, 3, 0);
            this.buttonLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLayout.Location = new System.Drawing.Point(3, 29);
            this.buttonLayout.Name = "buttonLayout";
            this.buttonLayout.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.buttonLayout.RowCount = 1;
            this.buttonLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.buttonLayout.Size = new System.Drawing.Size(1016, 47);
            this.buttonLayout.TabIndex = 1;
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(237)))), ((int)(((byte)(244)))));
            this.btnLamMoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLamMoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnLamMoi.Location = new System.Drawing.Point(8, 6);
            this.btnLamMoi.Margin = new System.Windows.Forms.Padding(8, 2, 8, 4);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(238, 37);
            this.btnLamMoi.TabIndex = 0;
            this.btnLamMoi.Text = "↻  Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            // 
            // btnSua
            // 
            this.btnSua.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.btnSua.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSua.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnSua.Location = new System.Drawing.Point(262, 6);
            this.btnSua.Margin = new System.Windows.Forms.Padding(8, 2, 8, 4);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(238, 37);
            this.btnSua.TabIndex = 1;
            this.btnSua.Text = "▣  Chỉnh sửa";
            this.btnSua.UseVisualStyleBackColor = false;
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(239)))), ((int)(((byte)(226)))));
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnLuu.Location = new System.Drawing.Point(516, 6);
            this.btnLuu.Margin = new System.Windows.Forms.Padding(8, 2, 8, 4);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(238, 37);
            this.btnLuu.TabIndex = 2;
            this.btnLuu.Text = "▣  Lưu cấu hình";
            this.btnLuu.UseVisualStyleBackColor = false;
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(225)))), ((int)(((byte)(228)))));
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnHuy.Location = new System.Drawing.Point(770, 6);
            this.btnHuy.Margin = new System.Windows.Forms.Padding(8, 2, 8, 4);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(238, 37);
            this.btnHuy.TabIndex = 3;
            this.btnHuy.Text = "⊘  Hủy bỏ";
            this.btnHuy.UseVisualStyleBackColor = false;
            // 
            // frmCauHinhTraGop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1100, 740);
            this.Controls.Add(this.rootLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.MinimumSize = new System.Drawing.Size(920, 620);
            this.Name = "frmCauHinhTraGop";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cấu hình Trả Góp & Hạng Thành Viên";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.rootLayout.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.grpThongTin.ResumeLayout(false);
            this.infoLayout.ResumeLayout(false);
            this.infoLayout.PerformLayout();
            this.grpChiTiet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCauHinh)).EndInit();
            this.grpThaoTac.ResumeLayout(false);
            this.footerLayout.ResumeLayout(false);
            this.buttonLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.DataGridViewTextBoxColumn MoTa;
        private System.Windows.Forms.DataGridViewTextBoxColumn Khoa;
        private System.Windows.Forms.DataGridViewTextBoxColumn GiaTri;
        private System.Windows.Forms.DataGridViewTextBoxColumn DonVi;
    }
}






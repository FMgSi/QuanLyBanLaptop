namespace QuanLyBanLaptop.HoaDon
{
    partial class frmTimKiemLaptop
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.pnlCard = new System.Windows.Forms.Panel();
            this.dgvLaptop = new System.Windows.Forms.DataGridView();
            this.colSTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenLaptop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCauHinh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiaBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaLaptop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuongTon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThoiGianBaoHanh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnChon = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();
            this.lblCapNhat = new System.Windows.Forms.Label();
            this.lblHienThi = new System.Windows.Forms.Label();
            this.pnlSearch.SuspendLayout();
            this.pnlCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLaptop)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTitle.Location = new System.Drawing.Point(24, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(836, 42);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "TÌM KIẾM LAPTOP";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.btnTimKiem);
            this.pnlSearch.Controls.Add(this.txtTimKiem);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(24, 62);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(0, 8, 0, 14);
            this.pnlSearch.Size = new System.Drawing.Size(836, 58);
            this.pnlSearch.TabIndex = 1;
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTimKiem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.btnTimKiem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimKiem.FlatAppearance.BorderSize = 0;
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Location = new System.Drawing.Point(716, 9);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(120, 36);
            this.btnTimKiem.TabIndex = 1;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTimKiem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.txtTimKiem.Location = new System.Drawing.Point(0, 13);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(706, 27);
            this.txtTimKiem.TabIndex = 0;
            this.txtTimKiem.Text = "Nhập tên máy, hãng, chip xử lý...";
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);
            this.txtTimKiem.Enter += new System.EventHandler(this.txtTimKiem_Enter);
            this.txtTimKiem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimKiem_KeyDown);
            this.txtTimKiem.Leave += new System.EventHandler(this.txtTimKiem_Leave);
            // 
            // pnlCard
            // 
            this.pnlCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCard.Controls.Add(this.dgvLaptop);
            this.pnlCard.Controls.Add(this.pnlFooter);
            this.pnlCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCard.Location = new System.Drawing.Point(24, 120);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.Size = new System.Drawing.Size(836, 440);
            this.pnlCard.TabIndex = 2;
            // 
            // dgvLaptop
            // 
            this.dgvLaptop.AllowUserToAddRows = false;
            this.dgvLaptop.AllowUserToDeleteRows = false;
            this.dgvLaptop.AllowUserToResizeRows = false;
            this.dgvLaptop.AllowUserToResizeColumns = true;
            this.dgvLaptop.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.dgvLaptop.BackgroundColor = System.Drawing.Color.White;
            this.dgvLaptop.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLaptop.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLaptop.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLaptop.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLaptop.ColumnHeadersHeight = 38;
            this.dgvLaptop.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLaptop.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSTT,
            this.colTenLaptop,
            this.colHang,
            this.colCauHinh,
            this.colGiaBan,
            this.colMaLaptop,
            this.colSoLuongTon,
            this.colThoiGianBaoHanh});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLaptop.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLaptop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLaptop.EnableHeadersVisualStyles = false;
            this.dgvLaptop.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.dgvLaptop.Location = new System.Drawing.Point(0, 0);
            this.dgvLaptop.MultiSelect = false;
            this.dgvLaptop.Name = "dgvLaptop";
            this.dgvLaptop.ReadOnly = true;
            this.dgvLaptop.RowHeadersVisible = false;
            this.dgvLaptop.RowTemplate.Height = 40;
            this.dgvLaptop.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLaptop.Size = new System.Drawing.Size(834, 388);
            this.dgvLaptop.TabIndex = 0;
            this.dgvLaptop.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLaptop_CellDoubleClick);
            this.dgvLaptop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvLaptop_KeyDown);
            // 
            // colSTT
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colSTT.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSTT.HeaderText = "STT"; this.colSTT.MinimumWidth = 50;
            this.colSTT.Name = "colSTT";
            this.colSTT.ReadOnly = true;
            this.colSTT.Width = 55;
            // 
            // colTenLaptop
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.colTenLaptop.DefaultCellStyle = dataGridViewCellStyle4;
            this.colTenLaptop.HeaderText = "TÊN LAPTOP";
            this.colTenLaptop.Name = "colTenLaptop";
            this.colTenLaptop.ReadOnly = true;
            this.colTenLaptop.Width = 220;
            // 
            // colHang
            // 
            this.colHang.HeaderText = "HÃNG";
            this.colHang.Name = "colHang";
            this.colHang.ReadOnly = true;
            this.colHang.Width = 95;
            // 
            // colCauHinh
            // 
            this.colCauHinh.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCauHinh.HeaderText = "CẤU HÌNH CHÍNH";
            this.colCauHinh.Name = "colCauHinh";
            this.colCauHinh.ReadOnly = true;
            // 
            // colGiaBan
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.colGiaBan.DefaultCellStyle = dataGridViewCellStyle5;
            this.colGiaBan.HeaderText = "GIÁ BÁN";
            this.colGiaBan.Name = "colGiaBan";
            this.colGiaBan.ReadOnly = true;
            this.colGiaBan.Width = 145;
            // 
            // colMaLaptop
            // 
            this.colMaLaptop.HeaderText = "MaLaptop";
            this.colMaLaptop.Name = "colMaLaptop";
            this.colMaLaptop.ReadOnly = true;
            this.colMaLaptop.Visible = false;
            // 
            // colSoLuongTon
            // 
            this.colSoLuongTon.HeaderText = "SoLuongTon";
            this.colSoLuongTon.Name = "colSoLuongTon";
            this.colSoLuongTon.ReadOnly = true;
            this.colSoLuongTon.Visible = false;
            // 
            // colThoiGianBaoHanh
            // 
            this.colThoiGianBaoHanh.HeaderText = "BaoHanh";
            this.colThoiGianBaoHanh.Name = "colThoiGianBaoHanh";
            this.colThoiGianBaoHanh.ReadOnly = true;
            this.colThoiGianBaoHanh.Visible = false;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.pnlFooter.Controls.Add(this.btnChon);
            this.pnlFooter.Controls.Add(this.btnDong);
            this.pnlFooter.Controls.Add(this.lblCapNhat);
            this.pnlFooter.Controls.Add(this.lblHienThi);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 388);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.pnlFooter.Size = new System.Drawing.Size(834, 50);
            this.pnlFooter.TabIndex = 1;
            // 
            // btnChon
            // 
            this.btnChon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.btnChon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChon.FlatAppearance.BorderSize = 0;
            this.btnChon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChon.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Bold);
            this.btnChon.ForeColor = System.Drawing.Color.White;
            this.btnChon.Location = new System.Drawing.Point(626, 8);
            this.btnChon.Name = "btnChon";
            this.btnChon.Size = new System.Drawing.Size(100, 32);
            this.btnChon.TabIndex = 2;
            this.btnChon.Text = "✓ Chọn máy";
            this.btnChon.UseVisualStyleBackColor = false;
            this.btnChon.Click += new System.EventHandler(this.btnChon_Click);
            // 
            // btnDong
            // 
            this.btnDong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnDong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDong.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDong.FlatAppearance.BorderSize = 0;
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.btnDong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnDong.Location = new System.Drawing.Point(734, 8);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(86, 32);
            this.btnDong.TabIndex = 3;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = false;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // lblCapNhat
            // 
            this.lblCapNhat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCapNhat.AutoSize = true;
            this.lblCapNhat.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lblCapNhat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCapNhat.Location = new System.Drawing.Point(510, 17);
            this.lblCapNhat.Name = "lblCapNhat";
            this.lblCapNhat.Size = new System.Drawing.Size(102, 15);
            this.lblCapNhat.TabIndex = 1;
            this.lblCapNhat.Text = "Cập nhật mới nhất";
            // 
            // lblHienThi
            // 
            this.lblHienThi.AutoSize = true;
            this.lblHienThi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHienThi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblHienThi.Location = new System.Drawing.Point(16, 17);
            this.lblHienThi.Name = "lblHienThi";
            this.lblHienThi.Size = new System.Drawing.Size(107, 15);
            this.lblHienThi.TabIndex = 0;
            this.lblHienThi.Text = "Hiển thị: 0 sản phẩm";
            // 
            // frmTimKiemLaptop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnDong;
            this.ClientSize = new System.Drawing.Size(884, 584);
            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(760, 500);
            this.Name = "frmTimKiemLaptop";
            this.Padding = new System.Windows.Forms.Padding(24, 20, 24, 24);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tìm kiếm laptop";
            this.Load += new System.EventHandler(this.frmTimKiemLaptop_Load);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLaptop)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Panel pnlCard;
        private System.Windows.Forms.DataGridView dgvLaptop;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblCapNhat;
        private System.Windows.Forms.Label lblHienThi;
        private System.Windows.Forms.Button btnChon;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSTT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenLaptop;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCauHinh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGiaBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaLaptop;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuongTon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThoiGianBaoHanh;
    }
}

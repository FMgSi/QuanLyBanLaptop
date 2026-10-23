namespace QuanLyBanLaptop
{
    partial class frmTrangChu
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
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlCardLaptop = new System.Windows.Forms.Panel();
            this.lblCardLaptopCount = new System.Windows.Forms.Label();
            this.lblCardLaptopTitle = new System.Windows.Forms.Label();
            this.pnlCardBanHang = new System.Windows.Forms.Panel();
            this.lblCardBanHangCount = new System.Windows.Forms.Label();
            this.lblCardBanHangTitle = new System.Windows.Forms.Label();
            this.pnlCardNhapHang = new System.Windows.Forms.Panel();
            this.lblCardNhapHangCount = new System.Windows.Forms.Label();
            this.lblCardNhapHangTitle = new System.Windows.Forms.Label();
            this.pnlCardNhanVien = new System.Windows.Forms.Panel();
            this.lblCardNhanVienCount = new System.Windows.Forms.Label();
            this.lblCardNhanVienTitle = new System.Windows.Forms.Label();
            this.pnlContent.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlCards.SuspendLayout();
            this.pnlCardLaptop.SuspendLayout();
            this.pnlCardBanHang.SuspendLayout();
            this.pnlCardNhapHang.SuspendLayout();
            this.pnlCardNhanVien.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidebarControl1
            // 
            this.sidebarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarControl1.Location = new System.Drawing.Point(0, 0);
            this.sidebarControl1.Name = "sidebarControl1";
            this.sidebarControl1.SelectedMenu = QuanLyBanLaptop.Classes.NavigationMenu.TrangChu;
            this.sidebarControl1.Size = new System.Drawing.Size(175, 661);
            this.sidebarControl1.TabIndex = 0;
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.pnlContent.Controls.Add(this.pnlCards);
            this.pnlContent.Controls.Add(this.pnlHeader);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(175, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(24);
            this.pnlContent.Size = new System.Drawing.Size(959, 661);
            this.pnlContent.TabIndex = 1;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(24, 24);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20);
            this.pnlHeader.Size = new System.Drawing.Size(911, 80);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(462, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "HỆ THỐNG QUẢN LÝ BÁN LAPTOP CHÍNH HÃNG";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(115)))), ((int)(((byte)(130)))));
            this.lblSubtitle.Location = new System.Drawing.Point(18, 46);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(438, 19);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Chào mừng bạn trở lại! Chọn danh mục từ thanh bên trái để thao tác.";
            // 
            // pnlCards
            // 
            this.pnlCards.AutoScroll = true;
            this.pnlCards.Controls.Add(this.pnlCardLaptop);
            this.pnlCards.Controls.Add(this.pnlCardBanHang);
            this.pnlCards.Controls.Add(this.pnlCardNhapHang);
            this.pnlCards.Controls.Add(this.pnlCardNhanVien);
            this.pnlCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCards.Location = new System.Drawing.Point(24, 104);
            this.pnlCards.Name = "pnlCards";
            this.pnlCards.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.pnlCards.Size = new System.Drawing.Size(911, 533);
            this.pnlCards.TabIndex = 1;
            // 
            // pnlCardLaptop
            // 
            this.pnlCardLaptop.BackColor = System.Drawing.Color.White;
            this.pnlCardLaptop.Controls.Add(this.lblCardLaptopCount);
            this.pnlCardLaptop.Controls.Add(this.lblCardLaptopTitle);
            this.pnlCardLaptop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardLaptop.Location = new System.Drawing.Point(0, 20);
            this.pnlCardLaptop.Margin = new System.Windows.Forms.Padding(0, 0, 20, 20);
            this.pnlCardLaptop.Name = "pnlCardLaptop";
            this.pnlCardLaptop.Padding = new System.Windows.Forms.Padding(16);
            this.pnlCardLaptop.Size = new System.Drawing.Size(205, 110);
            this.pnlCardLaptop.TabIndex = 0;
            // 
            // lblCardLaptopTitle
            // 
            this.lblCardLaptopTitle.AutoSize = true;
            this.lblCardLaptopTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardLaptopTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblCardLaptopTitle.Location = new System.Drawing.Point(14, 16);
            this.lblCardLaptopTitle.Name = "lblCardLaptopTitle";
            this.lblCardLaptopTitle.Size = new System.Drawing.Size(107, 19);
            this.lblCardLaptopTitle.TabIndex = 0;
            this.lblCardLaptopTitle.Text = "Sản phẩm laptop";
            // 
            // lblCardLaptopCount
            // 
            this.lblCardLaptopCount.AutoSize = true;
            this.lblCardLaptopCount.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardLaptopCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(101)))), ((int)(((byte)(185)))));
            this.lblCardLaptopCount.Location = new System.Drawing.Point(12, 45);
            this.lblCardLaptopCount.Name = "lblCardLaptopCount";
            this.lblCardLaptopCount.Size = new System.Drawing.Size(140, 32);
            this.lblCardLaptopCount.TabIndex = 1;
            this.lblCardLaptopCount.Text = "Quản lý >";
            // 
            // pnlCardBanHang
            // 
            this.pnlCardBanHang.BackColor = System.Drawing.Color.White;
            this.pnlCardBanHang.Controls.Add(this.lblCardBanHangCount);
            this.pnlCardBanHang.Controls.Add(this.lblCardBanHangTitle);
            this.pnlCardBanHang.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardBanHang.Location = new System.Drawing.Point(225, 20);
            this.pnlCardBanHang.Margin = new System.Windows.Forms.Padding(0, 0, 20, 20);
            this.pnlCardBanHang.Name = "pnlCardBanHang";
            this.pnlCardBanHang.Padding = new System.Windows.Forms.Padding(16);
            this.pnlCardBanHang.Size = new System.Drawing.Size(205, 110);
            this.pnlCardBanHang.TabIndex = 1;
            // 
            // lblCardBanHangTitle
            // 
            this.lblCardBanHangTitle.AutoSize = true;
            this.lblCardBanHangTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardBanHangTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblCardBanHangTitle.Location = new System.Drawing.Point(14, 16);
            this.lblCardBanHangTitle.Name = "lblCardBanHangTitle";
            this.lblCardBanHangTitle.Size = new System.Drawing.Size(66, 19);
            this.lblCardBanHangTitle.TabIndex = 0;
            this.lblCardBanHangTitle.Text = "Bán hàng";
            // 
            // lblCardBanHangCount
            // 
            this.lblCardBanHangCount.AutoSize = true;
            this.lblCardBanHangCount.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardBanHangCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblCardBanHangCount.Location = new System.Drawing.Point(12, 45);
            this.lblCardBanHangCount.Name = "lblCardBanHangCount";
            this.lblCardBanHangCount.Size = new System.Drawing.Size(130, 32);
            this.lblCardBanHangCount.TabIndex = 1;
            this.lblCardBanHangCount.Text = "Tạo đơn >";
            // 
            // pnlCardNhapHang
            // 
            this.pnlCardNhapHang.BackColor = System.Drawing.Color.White;
            this.pnlCardNhapHang.Controls.Add(this.lblCardNhapHangCount);
            this.pnlCardNhapHang.Controls.Add(this.lblCardNhapHangTitle);
            this.pnlCardNhapHang.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardNhapHang.Location = new System.Drawing.Point(450, 20);
            this.pnlCardNhapHang.Margin = new System.Windows.Forms.Padding(0, 0, 20, 20);
            this.pnlCardNhapHang.Name = "pnlCardNhapHang";
            this.pnlCardNhapHang.Padding = new System.Windows.Forms.Padding(16);
            this.pnlCardNhapHang.Size = new System.Drawing.Size(205, 110);
            this.pnlCardNhapHang.TabIndex = 2;
            // 
            // lblCardNhapHangTitle
            // 
            this.lblCardNhapHangTitle.AutoSize = true;
            this.lblCardNhapHangTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNhapHangTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblCardNhapHangTitle.Location = new System.Drawing.Point(14, 16);
            this.lblCardNhapHangTitle.Name = "lblCardNhapHangTitle";
            this.lblCardNhapHangTitle.Size = new System.Drawing.Size(76, 19);
            this.lblCardNhapHangTitle.TabIndex = 0;
            this.lblCardNhapHangTitle.Text = "Nhập hàng";
            // 
            // lblCardNhapHangCount
            // 
            this.lblCardNhapHangCount.AutoSize = true;
            this.lblCardNhapHangCount.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNhapHangCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            this.lblCardNhapHangCount.Location = new System.Drawing.Point(12, 45);
            this.lblCardNhapHangCount.Name = "lblCardNhapHangCount";
            this.lblCardNhapHangCount.Size = new System.Drawing.Size(147, 32);
            this.lblCardNhapHangCount.TabIndex = 1;
            this.lblCardNhapHangCount.Text = "Nhập kho >";
            // 
            // pnlCardNhanVien
            // 
            this.pnlCardNhanVien.BackColor = System.Drawing.Color.White;
            this.pnlCardNhanVien.Controls.Add(this.lblCardNhanVienCount);
            this.pnlCardNhanVien.Controls.Add(this.lblCardNhanVienTitle);
            this.pnlCardNhanVien.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCardNhanVien.Location = new System.Drawing.Point(675, 20);
            this.pnlCardNhanVien.Margin = new System.Windows.Forms.Padding(0, 0, 20, 20);
            this.pnlCardNhanVien.Name = "pnlCardNhanVien";
            this.pnlCardNhanVien.Padding = new System.Windows.Forms.Padding(16);
            this.pnlCardNhanVien.Size = new System.Drawing.Size(205, 110);
            this.pnlCardNhanVien.TabIndex = 3;
            // 
            // lblCardNhanVienTitle
            // 
            this.lblCardNhanVienTitle.AutoSize = true;
            this.lblCardNhanVienTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNhanVienTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblCardNhanVienTitle.Location = new System.Drawing.Point(14, 16);
            this.lblCardNhanVienTitle.Name = "lblCardNhanVienTitle";
            this.lblCardNhanVienTitle.Size = new System.Drawing.Size(71, 19);
            this.lblCardNhanVienTitle.TabIndex = 0;
            this.lblCardNhanVienTitle.Text = "Nhân viên";
            // 
            // lblCardNhanVienCount
            // 
            this.lblCardNhanVienCount.AutoSize = true;
            this.lblCardNhanVienCount.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCardNhanVienCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(66)))), ((int)(((byte)(193)))));
            this.lblCardNhanVienCount.Location = new System.Drawing.Point(12, 45);
            this.lblCardNhanVienCount.Name = "lblCardNhanVienCount";
            this.lblCardNhanVienCount.Size = new System.Drawing.Size(147, 32);
            this.lblCardNhanVienCount.TabIndex = 1;
            this.lblCardNhanVienCount.Text = "Danh sách >";
            // 
            // frmTrangChu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 661);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.sidebarControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmTrangChu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TRANG CHỦ - QUẢN LÝ BÁN LAPTOP";
            this.pnlContent.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlCards.ResumeLayout(false);
            this.pnlCardLaptop.ResumeLayout(false);
            this.pnlCardLaptop.PerformLayout();
            this.pnlCardBanHang.ResumeLayout(false);
            this.pnlCardBanHang.PerformLayout();
            this.pnlCardNhapHang.ResumeLayout(false);
            this.pnlCardNhapHang.PerformLayout();
            this.pnlCardNhanVien.ResumeLayout(false);
            this.pnlCardNhanVien.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SidebarControl sidebarControl1;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.FlowLayoutPanel pnlCards;
        private System.Windows.Forms.Panel pnlCardLaptop;
        private System.Windows.Forms.Label lblCardLaptopCount;
        private System.Windows.Forms.Label lblCardLaptopTitle;
        private System.Windows.Forms.Panel pnlCardBanHang;
        private System.Windows.Forms.Label lblCardBanHangCount;
        private System.Windows.Forms.Label lblCardBanHangTitle;
        private System.Windows.Forms.Panel pnlCardNhapHang;
        private System.Windows.Forms.Label lblCardNhapHangCount;
        private System.Windows.Forms.Label lblCardNhapHangTitle;
        private System.Windows.Forms.Panel pnlCardNhanVien;
        private System.Windows.Forms.Label lblCardNhanVienCount;
        private System.Windows.Forms.Label lblCardNhanVienTitle;
    }
}
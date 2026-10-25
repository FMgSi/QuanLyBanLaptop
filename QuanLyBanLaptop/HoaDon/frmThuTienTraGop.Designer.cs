namespace QuanLyBanLaptop.HoaDon
{
    partial class frmThuTienTraGop
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
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlCardInfo = new System.Windows.Forms.Panel();
            this.lblNgayDenHan = new System.Windows.Forms.Label();
            this.lblKyThanhToan = new System.Windows.Forms.Label();
            this.lblKhachHang = new System.Windows.Forms.Label();
            this.lblMaHoaDon = new System.Windows.Forms.Label();
            this.lblTienGocTitle = new System.Windows.Forms.Label();
            this.txtTienGoc = new System.Windows.Forms.TextBox();
            this.lblSoNgayTreTitle = new System.Windows.Forms.Label();
            this.txtSoNgayTre = new System.Windows.Forms.TextBox();
            this.lblNgayUnit = new System.Windows.Forms.Label();
            this.lblTienPhatTitle = new System.Windows.Forms.Label();
            this.txtTienPhat = new System.Windows.Forms.TextBox();
            this.lblTongPhaiThuTitle = new System.Windows.Forms.Label();
            this.txtTongPhaiThu = new System.Windows.Forms.TextBox();
            this.lblPhuongThucTitle = new System.Windows.Forms.Label();
            this.cboPhuongThuc = new System.Windows.Forms.ComboBox();
            this.pnlTienMat = new System.Windows.Forms.Panel();
            this.lblCanhBao = new System.Windows.Forms.Label();
            this.lblTienThuaValue = new System.Windows.Forms.Label();
            this.lblTienThuaTitle = new System.Windows.Forms.Label();
            this.txtKhachDua = new System.Windows.Forms.TextBox();
            this.lblKhachDuaTitle = new System.Windows.Forms.Label();
            this.btnXacNhanIn = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();
            this.pnlTitle.SuspendLayout();
            this.pnlCardInfo.SuspendLayout();
            this.pnlTienMat.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTitle
            // 
            this.pnlTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(78)))), ((int)(((byte)(216)))));
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Controls.Add(this.btnClose);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(547, 42);
            this.pnlTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(369, 23);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "💳 Thu Tiền Kỳ Trả Góp (frmThuTienTraGop)";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(507, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(32, 32);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "✕";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlCardInfo
            // 
            this.pnlCardInfo.BackColor = System.Drawing.Color.White;
            this.pnlCardInfo.Controls.Add(this.lblNgayDenHan);
            this.pnlCardInfo.Controls.Add(this.lblKyThanhToan);
            this.pnlCardInfo.Controls.Add(this.lblKhachHang);
            this.pnlCardInfo.Controls.Add(this.lblMaHoaDon);
            this.pnlCardInfo.Location = new System.Drawing.Point(24, 56);
            this.pnlCardInfo.Name = "pnlCardInfo";
            this.pnlCardInfo.Size = new System.Drawing.Size(512, 70);
            this.pnlCardInfo.TabIndex = 1;
            // 
            // lblNgayDenHan
            // 
            this.lblNgayDenHan.AutoSize = true;
            this.lblNgayDenHan.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblNgayDenHan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblNgayDenHan.Location = new System.Drawing.Point(270, 40);
            this.lblNgayDenHan.Name = "lblNgayDenHan";
            this.lblNgayDenHan.Size = new System.Drawing.Size(198, 21);
            this.lblNgayDenHan.TabIndex = 3;
            this.lblNgayDenHan.Text = "Ngày đến hạn: 14/08/2026";
            // 
            // lblKyThanhToan
            // 
            this.lblKyThanhToan.AutoSize = true;
            this.lblKyThanhToan.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblKyThanhToan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblKyThanhToan.Location = new System.Drawing.Point(14, 40);
            this.lblKyThanhToan.Name = "lblKyThanhToan";
            this.lblKyThanhToan.Size = new System.Drawing.Size(191, 21);
            this.lblKyThanhToan.TabIndex = 2;
            this.lblKyThanhToan.Text = "Kỳ thanh toán: Kỳ thứ 1 / 3";
            // 
            // lblKhachHang
            // 
            this.lblKhachHang.AutoSize = true;
            this.lblKhachHang.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblKhachHang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblKhachHang.Location = new System.Drawing.Point(270, 12);
            this.lblKhachHang.Name = "lblKhachHang";
            this.lblKhachHang.Size = new System.Drawing.Size(190, 21);
            this.lblKhachHang.TabIndex = 1;
            this.lblKhachHang.Text = "Khách hàng: Vũ Quốc Anh";
            // 
            // lblMaHoaDon
            // 
            this.lblMaHoaDon.AutoSize = true;
            this.lblMaHoaDon.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMaHoaDon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblMaHoaDon.Location = new System.Drawing.Point(14, 12);
            this.lblMaHoaDon.Name = "lblMaHoaDon";
            this.lblMaHoaDon.Size = new System.Drawing.Size(150, 21);
            this.lblMaHoaDon.TabIndex = 0;
            this.lblMaHoaDon.Text = "Mã hóa đơn: HD004";
            // 
            // lblTienGocTitle
            // 
            this.lblTienGocTitle.AutoSize = true;
            this.lblTienGocTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTienGocTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTienGocTitle.Location = new System.Drawing.Point(24, 142);
            this.lblTienGocTitle.Name = "lblTienGocTitle";
            this.lblTienGocTitle.Size = new System.Drawing.Size(120, 21);
            this.lblTienGocTitle.TabIndex = 2;
            this.lblTienGocTitle.Text = "Tiền gốc kỳ này:";
            // 
            // txtTienGoc
            // 
            this.txtTienGoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtTienGoc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtTienGoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtTienGoc.Location = new System.Drawing.Point(220, 138);
            this.txtTienGoc.Name = "txtTienGoc";
            this.txtTienGoc.ReadOnly = true;
            this.txtTienGoc.Size = new System.Drawing.Size(316, 30);
            this.txtTienGoc.TabIndex = 3;
            this.txtTienGoc.Text = "3.920.000 đ";
            this.txtTienGoc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblSoNgayTreTitle
            // 
            this.lblSoNgayTreTitle.AutoSize = true;
            this.lblSoNgayTreTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSoNgayTreTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblSoNgayTreTitle.Location = new System.Drawing.Point(24, 178);
            this.lblSoNgayTreTitle.Name = "lblSoNgayTreTitle";
            this.lblSoNgayTreTitle.Size = new System.Drawing.Size(178, 21);
            this.lblSoNgayTreTitle.TabIndex = 4;
            this.lblSoNgayTreTitle.Text = "Số ngày quá hạn (trễ):";
            // 
            // txtSoNgayTre
            // 
            this.txtSoNgayTre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.txtSoNgayTre.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtSoNgayTre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.txtSoNgayTre.Location = new System.Drawing.Point(290, 174);
            this.txtSoNgayTre.Name = "txtSoNgayTre";
            this.txtSoNgayTre.ReadOnly = true;
            this.txtSoNgayTre.Size = new System.Drawing.Size(70, 30);
            this.txtSoNgayTre.TabIndex = 5;
            this.txtSoNgayTre.Text = "12";
            this.txtSoNgayTre.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblNgayUnit
            // 
            this.lblNgayUnit.AutoSize = true;
            this.lblNgayUnit.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblNgayUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblNgayUnit.Location = new System.Drawing.Point(368, 178);
            this.lblNgayUnit.Name = "lblNgayUnit";
            this.lblNgayUnit.Size = new System.Drawing.Size(44, 21);
            this.lblNgayUnit.TabIndex = 6;
            this.lblNgayUnit.Text = "ngày";
            // 
            // lblTienPhatTitle
            // 
            this.lblTienPhatTitle.AutoSize = true;
            this.lblTienPhatTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTienPhatTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblTienPhatTitle.Location = new System.Drawing.Point(24, 214);
            this.lblTienPhatTitle.Name = "lblTienPhatTitle";
            this.lblTienPhatTitle.Size = new System.Drawing.Size(232, 21);
            this.lblTienPhatTitle.TabIndex = 7;
            this.lblTienPhatTitle.Text = "Tiền phạt trễ hạn (20k/ngày):";
            // 
            // txtTienPhat
            // 
            this.txtTienPhat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.txtTienPhat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtTienPhat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.txtTienPhat.Location = new System.Drawing.Point(220, 210);
            this.txtTienPhat.Name = "txtTienPhat";
            this.txtTienPhat.ReadOnly = true;
            this.txtTienPhat.Size = new System.Drawing.Size(316, 30);
            this.txtTienPhat.TabIndex = 8;
            this.txtTienPhat.Text = "240.000 đ";
            this.txtTienPhat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTongPhaiThuTitle
            // 
            this.lblTongPhaiThuTitle.AutoSize = true;
            this.lblTongPhaiThuTitle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblTongPhaiThuTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTongPhaiThuTitle.Location = new System.Drawing.Point(24, 252);
            this.lblTongPhaiThuTitle.Name = "lblTongPhaiThuTitle";
            this.lblTongPhaiThuTitle.Size = new System.Drawing.Size(207, 25);
            this.lblTongPhaiThuTitle.TabIndex = 9;
            this.lblTongPhaiThuTitle.Text = "TỔNG TIỀN PHẢI THU:";
            // 
            // txtTongPhaiThu
            // 
            this.txtTongPhaiThu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(253)))), ((int)(((byte)(245)))));
            this.txtTongPhaiThu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtTongPhaiThu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(150)))), ((int)(((byte)(105)))));
            this.txtTongPhaiThu.Location = new System.Drawing.Point(220, 248);
            this.txtTongPhaiThu.Name = "txtTongPhaiThu";
            this.txtTongPhaiThu.ReadOnly = true;
            this.txtTongPhaiThu.Size = new System.Drawing.Size(316, 34);
            this.txtTongPhaiThu.TabIndex = 10;
            this.txtTongPhaiThu.Text = "4.160.000 đ";
            this.txtTongPhaiThu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblPhuongThucTitle
            // 
            this.lblPhuongThucTitle.AutoSize = true;
            this.lblPhuongThucTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblPhuongThucTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblPhuongThucTitle.Location = new System.Drawing.Point(24, 294);
            this.lblPhuongThucTitle.Name = "lblPhuongThucTitle";
            this.lblPhuongThucTitle.Size = new System.Drawing.Size(129, 21);
            this.lblPhuongThucTitle.TabIndex = 11;
            this.lblPhuongThucTitle.Text = "Phương thức thu:";
            // 
            // cboPhuongThuc
            // 
            this.cboPhuongThuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPhuongThuc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboPhuongThuc.FormattingEnabled = true;
            this.cboPhuongThuc.Items.AddRange(new object[] {
            "Chuyển khoản ngân hàng (Mã QR VietQR)",
            "Tiền mặt"});
            this.cboPhuongThuc.Location = new System.Drawing.Point(220, 290);
            this.cboPhuongThuc.Name = "cboPhuongThuc";
            this.cboPhuongThuc.Size = new System.Drawing.Size(316, 29);
            this.cboPhuongThuc.TabIndex = 12;
            this.cboPhuongThuc.SelectedIndexChanged += new System.EventHandler(this.cboPhuongThuc_SelectedIndexChanged);
            // 
            // pnlTienMat
            // 
            this.pnlTienMat.Controls.Add(this.lblCanhBao);
            this.pnlTienMat.Controls.Add(this.lblTienThuaValue);
            this.pnlTienMat.Controls.Add(this.lblTienThuaTitle);
            this.pnlTienMat.Controls.Add(this.txtKhachDua);
            this.pnlTienMat.Controls.Add(this.lblKhachDuaTitle);
            this.pnlTienMat.Location = new System.Drawing.Point(24, 325);
            this.pnlTienMat.Name = "pnlTienMat";
            this.pnlTienMat.Size = new System.Drawing.Size(512, 65);
            this.pnlTienMat.TabIndex = 13;
            this.pnlTienMat.Visible = false;
            // 
            // lblCanhBao
            // 
            this.lblCanhBao.AutoSize = true;
            this.lblCanhBao.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.lblCanhBao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblCanhBao.Location = new System.Drawing.Point(196, 45);
            this.lblCanhBao.Name = "lblCanhBao";
            this.lblCanhBao.Size = new System.Drawing.Size(278, 19);
            this.lblCanhBao.TabIndex = 4;
            this.lblCanhBao.Text = "* Số tiền khách đưa chưa đủ để thanh toán";
            this.lblCanhBao.Visible = false;
            // 
            // lblTienThuaValue
            // 
            this.lblTienThuaValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTienThuaValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTienThuaValue.Location = new System.Drawing.Point(340, 24);
            this.lblTienThuaValue.Name = "lblTienThuaValue";
            this.lblTienThuaValue.Size = new System.Drawing.Size(168, 19);
            this.lblTienThuaValue.TabIndex = 3;
            this.lblTienThuaValue.Text = "0 đ";
            this.lblTienThuaValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTienThuaTitle
            // 
            this.lblTienThuaTitle.AutoSize = true;
            this.lblTienThuaTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTienThuaTitle.Location = new System.Drawing.Point(340, 4);
            this.lblTienThuaTitle.Name = "lblTienThuaTitle";
            this.lblTienThuaTitle.Size = new System.Drawing.Size(120, 21);
            this.lblTienThuaTitle.TabIndex = 2;
            this.lblTienThuaTitle.Text = "Tiền thừa trả lại:";
            // 
            // txtKhachDua
            // 
            this.txtKhachDua.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtKhachDua.Location = new System.Drawing.Point(196, 18);
            this.txtKhachDua.Name = "txtKhachDua";
            this.txtKhachDua.Size = new System.Drawing.Size(130, 30);
            this.txtKhachDua.TabIndex = 1;
            this.txtKhachDua.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtKhachDua.TextChanged += new System.EventHandler(this.txtKhachDua_TextChanged);
            // 
            // lblKhachDuaTitle
            // 
            this.lblKhachDuaTitle.AutoSize = true;
            this.lblKhachDuaTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblKhachDuaTitle.Location = new System.Drawing.Point(0, 21);
            this.lblKhachDuaTitle.Name = "lblKhachDuaTitle";
            this.lblKhachDuaTitle.Size = new System.Drawing.Size(117, 21);
            this.lblKhachDuaTitle.TabIndex = 0;
            this.lblKhachDuaTitle.Text = "Tiền khách đưa:";
            // 
            // btnXacNhanIn
            // 
            this.btnXacNhanIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(58)))), ((int)(((byte)(237)))));
            this.btnXacNhanIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXacNhanIn.FlatAppearance.BorderSize = 0;
            this.btnXacNhanIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXacNhanIn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXacNhanIn.ForeColor = System.Drawing.Color.White;
            this.btnXacNhanIn.Location = new System.Drawing.Point(145, 404);
            this.btnXacNhanIn.Name = "btnXacNhanIn";
            this.btnXacNhanIn.Size = new System.Drawing.Size(275, 42);
            this.btnXacNhanIn.TabIndex = 14;
            this.btnXacNhanIn.Text = "🖨️ Xác Nhận && In Phiếu Thu (Có QR)";
            this.btnXacNhanIn.UseVisualStyleBackColor = false;
            this.btnXacNhanIn.Click += new System.EventHandler(this.btnXacNhanIn_Click);
            // 
            // btnDong
            // 
            this.btnDong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnDong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDong.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnDong.Location = new System.Drawing.Point(430, 404);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(106, 42);
            this.btnDong.TabIndex = 15;
            this.btnDong.Text = "✕ Đóng";
            this.btnDong.UseVisualStyleBackColor = false;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // frmThuTienTraGop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(547, 469);
            this.Controls.Add(this.btnDong);
            this.Controls.Add(this.btnXacNhanIn);
            this.Controls.Add(this.pnlTienMat);
            this.Controls.Add(this.cboPhuongThuc);
            this.Controls.Add(this.lblPhuongThucTitle);
            this.Controls.Add(this.txtTongPhaiThu);
            this.Controls.Add(this.lblTongPhaiThuTitle);
            this.Controls.Add(this.txtTienPhat);
            this.Controls.Add(this.lblTienPhatTitle);
            this.Controls.Add(this.lblNgayUnit);
            this.Controls.Add(this.txtSoNgayTre);
            this.Controls.Add(this.lblSoNgayTreTitle);
            this.Controls.Add(this.txtTienGoc);
            this.Controls.Add(this.lblTienGocTitle);
            this.Controls.Add(this.pnlCardInfo);
            this.Controls.Add(this.pnlTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmThuTienTraGop";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thu Tiền Kỳ Trả Góp";
            this.Load += new System.EventHandler(this.frmThuTienTraGop_Load);
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlCardInfo.ResumeLayout(false);
            this.pnlCardInfo.PerformLayout();
            this.pnlTienMat.ResumeLayout(false);
            this.pnlTienMat.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlCardInfo;
        private System.Windows.Forms.Label lblNgayDenHan;
        private System.Windows.Forms.Label lblKyThanhToan;
        private System.Windows.Forms.Label lblKhachHang;
        private System.Windows.Forms.Label lblMaHoaDon;
        private System.Windows.Forms.Label lblTienGocTitle;
        private System.Windows.Forms.TextBox txtTienGoc;
        private System.Windows.Forms.Label lblSoNgayTreTitle;
        private System.Windows.Forms.TextBox txtSoNgayTre;
        private System.Windows.Forms.Label lblNgayUnit;
        private System.Windows.Forms.Label lblTienPhatTitle;
        private System.Windows.Forms.TextBox txtTienPhat;
        private System.Windows.Forms.Label lblTongPhaiThuTitle;
        private System.Windows.Forms.TextBox txtTongPhaiThu;
        private System.Windows.Forms.Label lblPhuongThucTitle;
        private System.Windows.Forms.ComboBox cboPhuongThuc;
        private System.Windows.Forms.Panel pnlTienMat;
        private System.Windows.Forms.Label lblCanhBao;
        private System.Windows.Forms.Label lblTienThuaValue;
        private System.Windows.Forms.Label lblTienThuaTitle;
        private System.Windows.Forms.TextBox txtKhachDua;
        private System.Windows.Forms.Label lblKhachDuaTitle;
        private System.Windows.Forms.Button btnXacNhanIn;
        private System.Windows.Forms.Button btnDong;
    }
}

using System.Drawing;
using System.Windows.Forms;

namespace QuanLyBanLaptop.HoaDon
{
    partial class frmThanhToan
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // Panels layout chính
        private Panel pnlTitleBar;
        private Label lblIconAndTitle;
        private Label lblMaHDValue;
        private Button btnCloseTitle;

        // Invoice summary card
        private Panel pnlInvoiceSummary;
        private Label lblKhachHangTitle;
        private Label lblTenKhach;
        private Label lblHangKhach;
        private Label lblTongTienTitle;
        private Label lblTongTienValue;

        // Custom Tab Buttons
        private Panel pnlTabBar;
        private Button btnTabThang;
        private Button btnTabGop;
        private Panel pnlTabIndicator;

        // Tab 1: Trả Thẳng Panel
        private Panel pnlTabThangContent;
        private Panel pnlTabThangCard;
        private Label lblPhuongThuc;
        private ComboBox cboPhuongThuc;
        private Label lblTongThanhToan;
        private TextBox txtTongThanhToan;
        private Label lblKhachDua;
        private TextBox txtKhachDua;
        private Label lblTienThua;
        private TextBox txtTienThua;

        // Tab 2: Trả Góp Panel
        private Panel pnlTabGopContent;
        private Panel pnlTabGopCard;
        private Label lblTraTruoc;
        private TextBox txtTraTruoc;
        private Label lblSoTienVay;
        private TextBox txtSoTienVay;
        private Label lblChuKy;
        private ComboBox cboChuKy;
        private Label lblScheduleHeader;
        private Label lblTongTienLai;
        private DataGridView dgvLichTraGop;
        private DataGridViewTextBoxColumn colKy;
        private DataGridViewTextBoxColumn colHan;
        private DataGridViewTextBoxColumn colSoTien;

        // Footer Bar
        private Panel pnlFooter;
        private Button btnXacNhan;
        private Button btnHuy;

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dgvHeaderStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvRowStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvColSoTienStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvColKyStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvColHanStyle = new System.Windows.Forms.DataGridViewCellStyle();

            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.lblIconAndTitle = new System.Windows.Forms.Label();
            this.lblMaHDValue = new System.Windows.Forms.Label();
            this.btnCloseTitle = new System.Windows.Forms.Button();

            this.pnlInvoiceSummary = new System.Windows.Forms.Panel();
            this.lblKhachHangTitle = new System.Windows.Forms.Label();
            this.lblTenKhach = new System.Windows.Forms.Label();
            this.lblHangKhach = new System.Windows.Forms.Label();
            this.lblTongTienTitle = new System.Windows.Forms.Label();
            this.lblTongTienValue = new System.Windows.Forms.Label();

            this.pnlTabBar = new System.Windows.Forms.Panel();
            this.btnTabThang = new System.Windows.Forms.Button();
            this.btnTabGop = new System.Windows.Forms.Button();
            this.pnlTabIndicator = new System.Windows.Forms.Panel();

            // Tab 1
            this.pnlTabThangContent = new System.Windows.Forms.Panel();
            this.pnlTabThangCard = new System.Windows.Forms.Panel();
            this.lblPhuongThuc = new System.Windows.Forms.Label();
            this.cboPhuongThuc = new System.Windows.Forms.ComboBox();
            this.lblTongThanhToan = new System.Windows.Forms.Label();
            this.txtTongThanhToan = new System.Windows.Forms.TextBox();
            this.lblKhachDua = new System.Windows.Forms.Label();
            this.txtKhachDua = new System.Windows.Forms.TextBox();
            this.lblTienThua = new System.Windows.Forms.Label();
            this.txtTienThua = new System.Windows.Forms.TextBox();

            // Tab 2
            this.pnlTabGopContent = new System.Windows.Forms.Panel();
            this.pnlTabGopCard = new System.Windows.Forms.Panel();
            this.lblTraTruoc = new System.Windows.Forms.Label();
            this.txtTraTruoc = new System.Windows.Forms.TextBox();
            this.lblSoTienVay = new System.Windows.Forms.Label();
            this.txtSoTienVay = new System.Windows.Forms.TextBox();
            this.lblChuKy = new System.Windows.Forms.Label();
            this.cboChuKy = new System.Windows.Forms.ComboBox();
            this.lblScheduleHeader = new System.Windows.Forms.Label();
            this.lblTongTienLai = new System.Windows.Forms.Label();
            this.dgvLichTraGop = new System.Windows.Forms.DataGridView();
            this.colKy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoTien = new System.Windows.Forms.DataGridViewTextBoxColumn();

            // Footer
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnXacNhan = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();

            this.pnlTitleBar.SuspendLayout();
            this.pnlInvoiceSummary.SuspendLayout();
            this.pnlTabBar.SuspendLayout();
            this.pnlTabThangContent.SuspendLayout();
            this.pnlTabThangCard.SuspendLayout();
            this.pnlTabGopContent.SuspendLayout();
            this.pnlTabGopCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichTraGop)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlTitleBar (Thanh tiêu đề màu xanh)
            // 
            this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(30, 58, 138); // #1E3A8A
            this.pnlTitleBar.Controls.Add(this.lblIconAndTitle);
            this.pnlTitleBar.Controls.Add(this.lblMaHDValue);
            this.pnlTitleBar.Controls.Add(this.btnCloseTitle);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Height = 44;
            this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Size = new System.Drawing.Size(866, 44);

            // lblIconAndTitle
            this.lblIconAndTitle.AutoSize = true;
            this.lblIconAndTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblIconAndTitle.ForeColor = System.Drawing.Color.White;
            this.lblIconAndTitle.Location = new System.Drawing.Point(14, 11);
            this.lblIconAndTitle.Name = "lblIconAndTitle";
            this.lblIconAndTitle.Text = "📟 Thanh toán hóa đơn - Mã: ";

            // lblMaHDValue
            this.lblMaHDValue.AutoSize = true;
            this.lblMaHDValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMaHDValue.ForeColor = System.Drawing.Color.FromArgb(250, 204, 21); // #FACC15
            this.lblMaHDValue.Location = new System.Drawing.Point(232, 11);
            this.lblMaHDValue.Name = "lblMaHDValue";
            this.lblMaHDValue.Text = "HD003";

            // btnCloseTitle
            this.btnCloseTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCloseTitle.FlatAppearance.BorderSize = 0;
            this.btnCloseTitle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnCloseTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCloseTitle.ForeColor = System.Drawing.Color.White;
            this.btnCloseTitle.Location = new System.Drawing.Point(822, 5);
            this.btnCloseTitle.Name = "btnCloseTitle";
            this.btnCloseTitle.Size = new System.Drawing.Size(36, 34);
            this.btnCloseTitle.Text = "✕";
            this.btnCloseTitle.UseVisualStyleBackColor = true;
            this.btnCloseTitle.Click += new System.EventHandler(this.btnCloseTitle_Click);

            // 
            // pnlInvoiceSummary (Khung xanh tóm tắt thông tin hóa đơn)
            // 
            this.pnlInvoiceSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlInvoiceSummary.BackColor = System.Drawing.Color.FromArgb(239, 246, 255); // #EFF6FF
            this.pnlInvoiceSummary.Controls.Add(this.lblKhachHangTitle);
            this.pnlInvoiceSummary.Controls.Add(this.lblTenKhach);
            this.pnlInvoiceSummary.Controls.Add(this.lblHangKhach);
            this.pnlInvoiceSummary.Controls.Add(this.lblTongTienTitle);
            this.pnlInvoiceSummary.Controls.Add(this.lblTongTienValue);
            this.pnlInvoiceSummary.Location = new System.Drawing.Point(18, 56);
            this.pnlInvoiceSummary.Name = "pnlInvoiceSummary";
            this.pnlInvoiceSummary.Size = new System.Drawing.Size(830, 50);

            // lblKhachHangTitle
            this.lblKhachHangTitle.AutoSize = true;
            this.lblKhachHangTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblKhachHangTitle.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblKhachHangTitle.Location = new System.Drawing.Point(16, 16);
            this.lblKhachHangTitle.Text = "Khách hàng:";

            // lblTenKhach
            this.lblTenKhach.AutoSize = true;
            this.lblTenKhach.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTenKhach.ForeColor = System.Drawing.Color.FromArgb(30, 58, 138); // #1E3A8A
            this.lblTenKhach.Location = new System.Drawing.Point(100, 16);
            this.lblTenKhach.Text = "Hoàng Bá Trung";

            // lblHangKhach
            this.lblHangKhach.AutoSize = true;
            this.lblHangKhach.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblHangKhach.ForeColor = System.Drawing.Color.FromArgb(225, 29, 72); // #E11D48
            this.lblHangKhach.Location = new System.Drawing.Point(215, 16);
            this.lblHangKhach.Text = "(Vàng)";

            // lblTongTienTitle
            this.lblTongTienTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTongTienTitle.AutoSize = true;
            this.lblTongTienTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTongTienTitle.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblTongTienTitle.Location = new System.Drawing.Point(530, 16);
            this.lblTongTienTitle.Text = "Tổng tiền hóa đơn:";

            // lblTongTienValue
            this.lblTongTienValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTongTienValue.AutoSize = true;
            this.lblTongTienValue.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblTongTienValue.ForeColor = System.Drawing.Color.FromArgb(190, 18, 60); // #BE123C
            this.lblTongTienValue.Location = new System.Drawing.Point(656, 14);
            this.lblTongTienValue.Text = "49.000.000 đ";

            // 
            // pnlTabBar (Thanh Tab lựa chọn)
            // 
            this.pnlTabBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTabBar.BackColor = System.Drawing.Color.Transparent;
            this.pnlTabBar.Controls.Add(this.btnTabThang);
            this.pnlTabBar.Controls.Add(this.btnTabGop);
            this.pnlTabBar.Controls.Add(this.pnlTabIndicator);
            this.pnlTabBar.Location = new System.Drawing.Point(18, 114);
            this.pnlTabBar.Name = "pnlTabBar";
            this.pnlTabBar.Size = new System.Drawing.Size(830, 38);

            // btnTabThang
            this.btnTabThang.BackColor = System.Drawing.Color.White;
            this.btnTabThang.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTabThang.FlatAppearance.BorderSize = 0;
            this.btnTabThang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTabThang.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnTabThang.ForeColor = System.Drawing.Color.FromArgb(29, 78, 216); // #1D4ED8
            this.btnTabThang.Location = new System.Drawing.Point(0, 0);
            this.btnTabThang.Name = "btnTabThang";
            this.btnTabThang.Size = new System.Drawing.Size(220, 35);
            this.btnTabThang.Text = "💵  1. Trả Thẳng (100%)";
            this.btnTabThang.UseVisualStyleBackColor = false;
            this.btnTabThang.Click += new System.EventHandler(this.btnTabThang_Click);

            // btnTabGop
            this.btnTabGop.BackColor = System.Drawing.Color.Transparent;
            this.btnTabGop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTabGop.FlatAppearance.BorderSize = 0;
            this.btnTabGop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTabGop.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnTabGop.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105); // #475569
            this.btnTabGop.Location = new System.Drawing.Point(225, 0);
            this.btnTabGop.Name = "btnTabGop";
            this.btnTabGop.Size = new System.Drawing.Size(260, 35);
            this.btnTabGop.Text = "🤝  2. Trả Góp (30% + Kỳ hạn)";
            this.btnTabGop.UseVisualStyleBackColor = false;
            this.btnTabGop.Click += new System.EventHandler(this.btnTabGop_Click);

            // pnlTabIndicator (Vạch xanh dưới Tab đang chọn)
            this.pnlTabIndicator.BackColor = System.Drawing.Color.FromArgb(37, 99, 235); // #2563EB
            this.pnlTabIndicator.Location = new System.Drawing.Point(0, 34);
            this.pnlTabIndicator.Name = "pnlTabIndicator";
            this.pnlTabIndicator.Size = new System.Drawing.Size(220, 4);

            // 
            // pnlTabThangContent (NỘI DUNG TAB 1: TRẢ THẲNG)
            // 
            this.pnlTabThangContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTabThangContent.BackColor = System.Drawing.Color.Transparent;
            this.pnlTabThangContent.Controls.Add(this.pnlTabThangCard);
            this.pnlTabThangContent.Location = new System.Drawing.Point(18, 158);
            this.pnlTabThangContent.Name = "pnlTabThangContent";
            this.pnlTabThangContent.Size = new System.Drawing.Size(830, 305);

            // pnlTabThangCard
            this.pnlTabThangCard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTabThangCard.BackColor = System.Drawing.Color.White;
            this.pnlTabThangCard.Controls.Add(this.lblPhuongThuc);
            this.pnlTabThangCard.Controls.Add(this.cboPhuongThuc);
            this.pnlTabThangCard.Controls.Add(this.lblTongThanhToan);
            this.pnlTabThangCard.Controls.Add(this.txtTongThanhToan);
            this.pnlTabThangCard.Controls.Add(this.lblKhachDua);
            this.pnlTabThangCard.Controls.Add(this.txtKhachDua);
            this.pnlTabThangCard.Controls.Add(this.lblTienThua);
            this.pnlTabThangCard.Controls.Add(this.txtTienThua);
            this.pnlTabThangCard.Location = new System.Drawing.Point(0, 4);
            this.pnlTabThangCard.Name = "pnlTabThangCard";
            this.pnlTabThangCard.Size = new System.Drawing.Size(830, 185);

            // Row 1 - Left: Phương thức thanh toán
            this.lblPhuongThuc.AutoSize = true;
            this.lblPhuongThuc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblPhuongThuc.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblPhuongThuc.Location = new System.Drawing.Point(18, 18);
            this.lblPhuongThuc.Text = "Phương thức thanh toán:";

            this.cboPhuongThuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPhuongThuc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cboPhuongThuc.Location = new System.Drawing.Point(20, 42);
            this.cboPhuongThuc.Name = "cboPhuongThuc";
            this.cboPhuongThuc.Size = new System.Drawing.Size(370, 29);

            // Row 1 - Right: Tổng tiền cần thanh toán
            this.lblTongThanhToan.AutoSize = true;
            this.lblTongThanhToan.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTongThanhToan.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblTongThanhToan.Location = new System.Drawing.Point(430, 18);
            this.lblTongThanhToan.Text = "Tổng tiền cần thanh toán:";

            this.txtTongThanhToan.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.txtTongThanhToan.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtTongThanhToan.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtTongThanhToan.Location = new System.Drawing.Point(432, 42);
            this.txtTongThanhToan.Name = "txtTongThanhToan";
            this.txtTongThanhToan.ReadOnly = true;
            this.txtTongThanhToan.Size = new System.Drawing.Size(380, 29);
            this.txtTongThanhToan.Text = "49.000.000 đ";

            // Row 2 - Left: Tiền khách đưa
            this.lblKhachDua.AutoSize = true;
            this.lblKhachDua.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblKhachDua.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblKhachDua.Location = new System.Drawing.Point(18, 98);
            this.lblKhachDua.Text = "Tiền khách đưa (VNĐ):";

            this.txtKhachDua.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtKhachDua.ForeColor = System.Drawing.Color.FromArgb(29, 78, 216); // #1D4ED8
            this.txtKhachDua.Location = new System.Drawing.Point(20, 122);
            this.txtKhachDua.Name = "txtKhachDua";
            this.txtKhachDua.Size = new System.Drawing.Size(370, 29);
            this.txtKhachDua.Text = "50000000";
            this.txtKhachDua.TextChanged += new System.EventHandler(this.txtKhachDua_TextChanged);

            // Row 2 - Right: Tiền thừa trả khách
            this.lblTienThua.AutoSize = true;
            this.lblTienThua.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTienThua.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblTienThua.Location = new System.Drawing.Point(430, 98);
            this.lblTienThua.Text = "Tiền thừa trả khách (VNĐ):";

            this.txtTienThua.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.txtTienThua.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtTienThua.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtTienThua.Location = new System.Drawing.Point(432, 122);
            this.txtTienThua.Name = "txtTienThua";
            this.txtTienThua.ReadOnly = true;
            this.txtTienThua.Size = new System.Drawing.Size(380, 29);
            this.txtTienThua.Text = "1.000.000 đ";

            // 
            // pnlTabGopContent (NỘI DUNG TAB 2: TRẢ GÓP)
            // 
            this.pnlTabGopContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTabGopContent.BackColor = System.Drawing.Color.Transparent;
            this.pnlTabGopContent.Controls.Add(this.pnlTabGopCard);
            this.pnlTabGopContent.Controls.Add(this.lblScheduleHeader);
            this.pnlTabGopContent.Controls.Add(this.lblTongTienLai);
            this.pnlTabGopContent.Controls.Add(this.dgvLichTraGop);
            this.pnlTabGopContent.Location = new System.Drawing.Point(18, 158);
            this.pnlTabGopContent.Name = "pnlTabGopContent";
            this.pnlTabGopContent.Size = new System.Drawing.Size(830, 305);
            this.pnlTabGopContent.Visible = false;

            // pnlTabGopCard (Khung 3 cột phía trên)
            this.pnlTabGopCard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTabGopCard.BackColor = System.Drawing.Color.White;
            this.pnlTabGopCard.Controls.Add(this.lblTraTruoc);
            this.pnlTabGopCard.Controls.Add(this.txtTraTruoc);
            this.pnlTabGopCard.Controls.Add(this.lblSoTienVay);
            this.pnlTabGopCard.Controls.Add(this.txtSoTienVay);
            this.pnlTabGopCard.Controls.Add(this.lblChuKy);
            this.pnlTabGopCard.Controls.Add(this.cboChuKy);
            this.pnlTabGopCard.Location = new System.Drawing.Point(0, 4);
            this.pnlTabGopCard.Name = "pnlTabGopCard";
            this.pnlTabGopCard.Size = new System.Drawing.Size(830, 80);

            // Col 1: Trả trước tối thiểu
            this.lblTraTruoc.AutoSize = true;
            this.lblTraTruoc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTraTruoc.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblTraTruoc.Location = new System.Drawing.Point(16, 12);
            this.lblTraTruoc.Text = "Trả trước tối thiểu (30%):";

            this.txtTraTruoc.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.txtTraTruoc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtTraTruoc.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtTraTruoc.Location = new System.Drawing.Point(18, 36);
            this.txtTraTruoc.Name = "txtTraTruoc";
            this.txtTraTruoc.ReadOnly = true;
            this.txtTraTruoc.Size = new System.Drawing.Size(245, 29);
            this.txtTraTruoc.Text = "14.700.000 đ";

            // Col 2: Số tiền vay còn lại
            this.lblSoTienVay.AutoSize = true;
            this.lblSoTienVay.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSoTienVay.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblSoTienVay.Location = new System.Drawing.Point(286, 12);
            this.lblSoTienVay.Text = "Số tiền vay còn lại:";

            this.txtSoTienVay.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.txtSoTienVay.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtSoTienVay.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtSoTienVay.Location = new System.Drawing.Point(288, 36);
            this.txtSoTienVay.Name = "txtSoTienVay";
            this.txtSoTienVay.ReadOnly = true;
            this.txtSoTienVay.Size = new System.Drawing.Size(245, 29);
            this.txtSoTienVay.Text = "34.300.000 đ";

            // Col 3: Chọn chu kỳ trả góp
            this.lblChuKy.AutoSize = true;
            this.lblChuKy.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblChuKy.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.lblChuKy.Location = new System.Drawing.Point(556, 12);
            this.lblChuKy.Text = "Chọn chu kỳ trả góp:";

            this.cboChuKy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChuKy.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.cboChuKy.ForeColor = System.Drawing.Color.FromArgb(3, 105, 161); // #0369A1
            this.cboChuKy.Location = new System.Drawing.Point(558, 36);
            this.cboChuKy.Name = "cboChuKy";
            this.cboChuKy.Size = new System.Drawing.Size(255, 29);
            this.cboChuKy.SelectedIndexChanged += new System.EventHandler(this.cboChuKy_SelectedIndexChanged);

            // lblScheduleHeader
            this.lblScheduleHeader.AutoSize = true;
            this.lblScheduleHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblScheduleHeader.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblScheduleHeader.Location = new System.Drawing.Point(2, 96);
            this.lblScheduleHeader.Text = "📅  Lịch trả góp dự kiến từng kỳ:";

            // lblTongTienLai
            this.lblTongTienLai.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTongTienLai.AutoSize = true;
            this.lblTongTienLai.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTongTienLai.ForeColor = System.Drawing.Color.FromArgb(4, 120, 87); // #047857
            this.lblTongTienLai.Location = new System.Drawing.Point(540, 96);
            this.lblTongTienLai.Text = "Tổng tiền lãi: 823.200 đ (Lãi suất 2.4%)";

            // dgvLichTraGop (Bảng lịch trả góp)
            this.dgvLichTraGop.AllowUserToAddRows = false;
            this.dgvLichTraGop.AllowUserToDeleteRows = false;
            this.dgvLichTraGop.AllowUserToResizeRows = false;
            this.dgvLichTraGop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLichTraGop.BackgroundColor = System.Drawing.Color.White;
            this.dgvLichTraGop.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLichTraGop.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLichTraGop.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dgvHeaderStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvHeaderStyle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252); // #F8FAFC
            dgvHeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgvHeaderStyle.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            dgvHeaderStyle.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            dgvHeaderStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLichTraGop.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvLichTraGop.ColumnHeadersHeight = 34;
            this.dgvLichTraGop.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLichTraGop.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colKy,
                this.colHan,
                this.colSoTien
            });
            dgvRowStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvRowStyle.BackColor = System.Drawing.Color.White;
            dgvRowStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dgvRowStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            dgvRowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(239, 246, 255);
            dgvRowStyle.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            dgvRowStyle.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.dgvLichTraGop.DefaultCellStyle = dgvRowStyle;
            this.dgvLichTraGop.EnableHeadersVisualStyles = false;
            this.dgvLichTraGop.GridColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.dgvLichTraGop.Location = new System.Drawing.Point(0, 120);
            this.dgvLichTraGop.MultiSelect = false;
            this.dgvLichTraGop.Name = "dgvLichTraGop";
            this.dgvLichTraGop.ReadOnly = true;
            this.dgvLichTraGop.RowHeadersVisible = false;
            this.dgvLichTraGop.RowTemplate.Height = 34;
            this.dgvLichTraGop.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLichTraGop.Size = new System.Drawing.Size(830, 175);

            // colKy
            dgvColKyStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvColKyStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.colKy.DefaultCellStyle = dgvColKyStyle;
            this.colKy.HeaderText = "Kỳ thứ";
            this.colKy.Name = "colKy";
            this.colKy.ReadOnly = true;
            this.colKy.Width = 180;

            // colHan
            dgvColHanStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colHan.DefaultCellStyle = dgvColHanStyle;
            this.colHan.HeaderText = "Hạn thanh toán";
            this.colHan.Name = "colHan";
            this.colHan.ReadOnly = true;
            this.colHan.Width = 300;

            // colSoTien
            dgvColSoTienStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dgvColSoTienStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            dgvColSoTienStyle.ForeColor = System.Drawing.Color.FromArgb(4, 120, 87); // #047857
            this.colSoTien.DefaultCellStyle = dgvColSoTienStyle;
            this.colSoTien.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSoTien.HeaderText = "Số tiền phải trả";
            this.colSoTien.Name = "colSoTien";
            this.colSoTien.ReadOnly = true;

            // 
            // pnlFooter (THANH NÚT BẤM DƯỚI ĐÁY - LUÔN HIỆN RÕ)
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(238, 242, 246);
            this.pnlFooter.Controls.Add(this.btnXacNhan);
            this.pnlFooter.Controls.Add(this.btnHuy);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 60;
            this.pnlFooter.Location = new System.Drawing.Point(0, 475);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(866, 60);

            // btnXacNhan ("✓  Xác nhận thanh toán & In hóa đơn")
            this.btnXacNhan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXacNhan.BackColor = System.Drawing.Color.FromArgb(22, 163, 74); // #16A34A Green
            this.btnXacNhan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXacNhan.FlatAppearance.BorderSize = 0;
            this.btnXacNhan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXacNhan.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnXacNhan.ForeColor = System.Drawing.Color.White;
            this.btnXacNhan.Location = new System.Drawing.Point(435, 10);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.Size = new System.Drawing.Size(285, 40);
            this.btnXacNhan.Text = "✓  Thanh toán && In hóa đơn";
            this.btnXacNhan.UseVisualStyleBackColor = false;
            this.btnXacNhan.Click += new System.EventHandler(this.btnXacNhan_Click);

            // btnHuy ("✕  Hủy (Quay về)" - Đóng form quay về trang Bán hàng)
            this.btnHuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.btnHuy.Location = new System.Drawing.Point(725, 10);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(125, 40);
            this.btnHuy.Text = "✕  Hủy (Quay về)";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);

            // 
            // frmThanhToan (Form Setup)
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.ClientSize = new System.Drawing.Size(866, 535);
            this.Controls.Add(this.pnlTabThangContent);
            this.Controls.Add(this.pnlTabGopContent);
            this.Controls.Add(this.pnlTabBar);
            this.Controls.Add(this.pnlInvoiceSummary);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlTitleBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmThanhToan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thanh toán hóa đơn";
            this.Load += new System.EventHandler(this.frmThanhToan_Load);

            this.pnlTitleBar.ResumeLayout(false);
            this.pnlTitleBar.PerformLayout();
            this.pnlInvoiceSummary.ResumeLayout(false);
            this.pnlInvoiceSummary.PerformLayout();
            this.pnlTabBar.ResumeLayout(false);
            this.pnlTabThangContent.ResumeLayout(false);
            this.pnlTabThangCard.ResumeLayout(false);
            this.pnlTabThangCard.PerformLayout();
            this.pnlTabGopContent.ResumeLayout(false);
            this.pnlTabGopContent.PerformLayout();
            this.pnlTabGopCard.ResumeLayout(false);
            this.pnlTabGopCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLichTraGop)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}

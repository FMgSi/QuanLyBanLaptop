using System.Drawing;
using System.Windows.Forms;
namespace QuanLyBanLaptop
{
    partial class frmMain
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.sidebarControl1 = new QuanLyBanLaptop.SidebarControl();
            this.pnlHeader = new Panel();
            this.lblTrang = new Label();
            this.lblNguoiDung = new Label();
            this.btnDangXuat = new Button();
            this.pnlNoiDung = new Panel();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            this.sidebarControl1.BackColor = Color.FromArgb(31, 42, 68);
            this.sidebarControl1.Dock = DockStyle.Left;
            this.sidebarControl1.Location = new Point(0, 0);
            this.sidebarControl1.Name = "sidebarControl1";
            this.sidebarControl1.SelectedMenu = QuanLyBanLaptop.Classes.NavigationMenu.TrangChu;
            this.sidebarControl1.Size = new Size(230, 800);
            this.sidebarControl1.TabIndex = 0;
            this.pnlHeader.BackColor = Color.White;
            this.pnlHeader.Controls.Add(this.btnDangXuat);
            this.pnlHeader.Controls.Add(this.lblNguoiDung);
            this.pnlHeader.Controls.Add(this.lblTrang);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Location = new Point(230, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new Size(1130, 56);
            this.pnlHeader.TabIndex = 1;
            this.lblTrang.AutoSize = true;
            this.lblTrang.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblTrang.ForeColor = Color.FromArgb(11, 36, 71);
            this.lblTrang.Location = new Point(18, 14);
            this.lblTrang.Name = "lblTrang";
            this.lblTrang.Size = new Size(107, 31);
            this.lblTrang.TabIndex = 0;
            this.lblTrang.Text = "Trang Chủ";
            this.lblNguoiDung.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.lblNguoiDung.Cursor = Cursors.Hand;
            this.lblNguoiDung.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lblNguoiDung.ForeColor = Color.FromArgb(71, 85, 105);
            this.lblNguoiDung.Location = new Point(620, 16);
            this.lblNguoiDung.Name = "lblNguoiDung";
            this.lblNguoiDung.Size = new Size(380, 25);
            this.lblNguoiDung.TabIndex = 1;
            this.lblNguoiDung.Text = "👤 Nguyễn Văn An (NV001) - Quản trị viên";
            this.lblNguoiDung.TextAlign = ContentAlignment.MiddleRight;
            this.btnDangXuat.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnDangXuat.BackColor = Color.FromArgb(254, 242, 242);
            this.btnDangXuat.Cursor = Cursors.Hand;
            this.btnDangXuat.FlatAppearance.BorderColor = Color.FromArgb(254, 202, 202);
            this.btnDangXuat.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 226, 226);
            this.btnDangXuat.FlatAppearance.MouseOverBackColor = Color.FromArgb(254, 202, 202);
            this.btnDangXuat.FlatStyle = FlatStyle.Flat;
            this.btnDangXuat.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnDangXuat.ForeColor = Color.FromArgb(220, 38, 38);
            this.btnDangXuat.Location = new Point(1015, 12);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Size = new Size(95, 32);
            this.btnDangXuat.TabIndex = 2;
            this.btnDangXuat.Text = "Đăng xuất";
            this.btnDangXuat.UseVisualStyleBackColor = false;
            this.pnlNoiDung.BackColor = Color.FromArgb(245, 247, 250);
            this.pnlNoiDung.Dock = DockStyle.Fill;
            this.pnlNoiDung.Location = new Point(230, 56);
            this.pnlNoiDung.Name = "pnlNoiDung";
            this.pnlNoiDung.Size = new Size(1130, 744);
            this.pnlNoiDung.TabIndex = 2;
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1360, 800);
            this.Controls.Add(this.pnlNoiDung);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.sidebarControl1);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = true;
            this.MinimumSize = new Size(1100, 700);
            this.Name = "frmMain";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Quản Lý Bán Laptop";
            this.WindowState = FormWindowState.Maximized;
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
        }
        private SidebarControl sidebarControl1;
        private Panel pnlHeader;
        private Label lblTrang;
        private Label lblNguoiDung;
        private Button btnDangXuat;
        private Panel pnlNoiDung;
    }
}

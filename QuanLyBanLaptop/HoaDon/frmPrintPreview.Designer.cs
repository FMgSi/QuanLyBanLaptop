using System.Drawing;
using System.Windows.Forms;
namespace QuanLyBanLaptop.HoaDon
{
    partial class frmPrintPreview
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
            this.toolStrip1 = new ToolStrip();
            this.tsBtnPrint = new ToolStripButton();
            this.tsBtnClose = new ToolStripButton();
            this.printPreviewControl1 = new PrintPreviewControl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            this.toolStrip1.BackColor = Color.FromArgb(243, 244, 246);
            this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] {
            this.tsBtnPrint,
            this.tsBtnClose});
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new Padding(10, 5, 10, 5);
            this.toolStrip1.RenderMode = ToolStripRenderMode.System;
            this.toolStrip1.Size = new Size(600, 36);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            this.tsBtnPrint.BackColor = Color.FromArgb(37, 99, 235);
            this.tsBtnPrint.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsBtnPrint.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.tsBtnPrint.ForeColor = Color.White;
            this.tsBtnPrint.Margin = new Padding(0, 1, 6, 1);
            this.tsBtnPrint.Name = "tsBtnPrint";
            this.tsBtnPrint.Padding = new Padding(12, 2, 12, 2);
            this.tsBtnPrint.Size = new Size(107, 24);
            this.tsBtnPrint.Text = "🖨 In hóa đơn";
            this.tsBtnPrint.Click += new System.EventHandler(this.tsBtnPrint_Click);
            this.tsBtnClose.Alignment = ToolStripItemAlignment.Right;
            this.tsBtnClose.BackColor = Color.FromArgb(229, 231, 235);
            this.tsBtnClose.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsBtnClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.tsBtnClose.ForeColor = Color.FromArgb(55, 65, 81);
            this.tsBtnClose.Margin = new Padding(0, 1, 0, 1);
            this.tsBtnClose.Name = "tsBtnClose";
            this.tsBtnClose.Padding = new Padding(12, 2, 12, 2);
            this.tsBtnClose.Size = new Size(69, 24);
            this.tsBtnClose.Text = "✖ Đóng";
            this.tsBtnClose.Click += new System.EventHandler(this.tsBtnClose_Click);
            this.printPreviewControl1.AutoZoom = false;
            this.printPreviewControl1.BackColor = Color.FromArgb(100, 116, 139);
            this.printPreviewControl1.Dock = DockStyle.Fill;
            this.printPreviewControl1.Location = new Point(0, 36);
            this.printPreviewControl1.Name = "printPreviewControl1";
            this.printPreviewControl1.Size = new Size(600, 684);
            this.printPreviewControl1.TabIndex = 1;
            this.printPreviewControl1.Zoom = 1D;
            this.AutoScaleDimensions = new SizeF(8F, 19F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(100, 116, 139);
            this.ClientSize = new Size(600, 720);
            this.Controls.Add(this.printPreviewControl1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.MinimumSize = new Size(450, 500);
            this.Name = "frmPrintPreview";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Hóa đơn bán hàng";
            this.Load += new System.EventHandler(this.frmPrintPreview_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private ToolStrip toolStrip1;
        private ToolStripButton tsBtnPrint;
        private ToolStripButton tsBtnClose;
        private PrintPreviewControl printPreviewControl1;
    }
}

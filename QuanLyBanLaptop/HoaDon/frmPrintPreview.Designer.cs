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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsBtnPrint = new System.Windows.Forms.ToolStripButton();
            this.tsSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tsBtnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.tsSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnOnePage = new System.Windows.Forms.ToolStripButton();
            this.tsBtnTwoPages = new System.Windows.Forms.ToolStripButton();
            this.tsBtnThreePages = new System.Windows.Forms.ToolStripButton();
            this.tsBtnFourPages = new System.Windows.Forms.ToolStripButton();
            this.tsSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsLblPage = new System.Windows.Forms.ToolStripLabel();
            this.tsTxtPage = new System.Windows.Forms.ToolStripTextBox();
            this.tsBtnClose = new System.Windows.Forms.ToolStripButton();
            this.printPreviewControl1 = new System.Windows.Forms.PrintPreviewControl();
            this.pnlCanvasContainer = new System.Windows.Forms.Panel();
            this.btnScrollBottom = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.pnlCanvasContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnPrint,
            this.tsSep1,
            this.tsBtnZoomIn,
            this.tsBtnZoomOut,
            this.tsSep2,
            this.tsBtnOnePage,
            this.tsBtnTwoPages,
            this.tsBtnThreePages,
            this.tsBtnFourPages,
            this.tsSep3,
            this.tsLblPage,
            this.tsTxtPage,
            this.tsBtnClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(860, 35);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsBtnPrint
            // 
            this.tsBtnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnPrint.Margin = new System.Windows.Forms.Padding(2, 1, 2, 2);
            this.tsBtnPrint.Name = "tsBtnPrint";
            this.tsBtnPrint.Size = new System.Drawing.Size(23, 24);
            this.tsBtnPrint.Text = "In hóa đơn";
            this.tsBtnPrint.ToolTipText = "In hóa đơn (Print)";
            this.tsBtnPrint.Click += new System.EventHandler(this.tsBtnPrint_Click);
            // 
            // tsSep1
            // 
            this.tsSep1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tsSep1.Name = "tsSep1";
            this.tsSep1.Size = new System.Drawing.Size(6, 27);
            // 
            // tsBtnZoomIn
            // 
            this.tsBtnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnZoomIn.Margin = new System.Windows.Forms.Padding(2, 1, 2, 2);
            this.tsBtnZoomIn.Name = "tsBtnZoomIn";
            this.tsBtnZoomIn.Size = new System.Drawing.Size(23, 24);
            this.tsBtnZoomIn.Text = "Phóng to";
            this.tsBtnZoomIn.ToolTipText = "Phóng to (Zoom In)";
            this.tsBtnZoomIn.Click += new System.EventHandler(this.tsBtnZoomIn_Click);
            // 
            // tsBtnZoomOut
            // 
            this.tsBtnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnZoomOut.Margin = new System.Windows.Forms.Padding(2, 1, 2, 2);
            this.tsBtnZoomOut.Name = "tsBtnZoomOut";
            this.tsBtnZoomOut.Size = new System.Drawing.Size(23, 24);
            this.tsBtnZoomOut.Text = "Thu nhỏ";
            this.tsBtnZoomOut.ToolTipText = "Thu nhỏ (Zoom Out)";
            this.tsBtnZoomOut.Click += new System.EventHandler(this.tsBtnZoomOut_Click);
            // 
            // tsSep2
            // 
            this.tsSep2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tsSep2.Name = "tsSep2";
            this.tsSep2.Size = new System.Drawing.Size(6, 27);
            // 
            // tsBtnOnePage
            // 
            this.tsBtnOnePage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnOnePage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnOnePage.Margin = new System.Windows.Forms.Padding(2, 1, 2, 2);
            this.tsBtnOnePage.Name = "tsBtnOnePage";
            this.tsBtnOnePage.Size = new System.Drawing.Size(23, 24);
            this.tsBtnOnePage.Text = "Xem 1 trang (100%)";
            this.tsBtnOnePage.ToolTipText = "Xem 1 trang (100%)";
            this.tsBtnOnePage.Click += new System.EventHandler(this.tsBtnOnePage_Click);
            // 
            // tsBtnTwoPages
            // 
            this.tsBtnTwoPages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnTwoPages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnTwoPages.Margin = new System.Windows.Forms.Padding(2, 1, 2, 2);
            this.tsBtnTwoPages.Name = "tsBtnTwoPages";
            this.tsBtnTwoPages.Size = new System.Drawing.Size(23, 24);
            this.tsBtnTwoPages.Text = "Xem 2 trang";
            this.tsBtnTwoPages.ToolTipText = "Xem 2 trang";
            this.tsBtnTwoPages.Click += new System.EventHandler(this.tsBtnTwoPages_Click);
            // 
            // tsBtnThreePages
            // 
            this.tsBtnThreePages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnThreePages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnThreePages.Margin = new System.Windows.Forms.Padding(2, 1, 2, 2);
            this.tsBtnThreePages.Name = "tsBtnThreePages";
            this.tsBtnThreePages.Size = new System.Drawing.Size(23, 24);
            this.tsBtnThreePages.Text = "Xem 3 trang";
            this.tsBtnThreePages.ToolTipText = "Xem 3 trang";
            this.tsBtnThreePages.Click += new System.EventHandler(this.tsBtnThreePages_Click);
            // 
            // tsBtnFourPages
            // 
            this.tsBtnFourPages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnFourPages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnFourPages.Margin = new System.Windows.Forms.Padding(2, 1, 2, 2);
            this.tsBtnFourPages.Name = "tsBtnFourPages";
            this.tsBtnFourPages.Size = new System.Drawing.Size(23, 24);
            this.tsBtnFourPages.Text = "Xem lưới 4 trang";
            this.tsBtnFourPages.ToolTipText = "Xem lưới 4 trang";
            this.tsBtnFourPages.Click += new System.EventHandler(this.tsBtnFourPages_Click);
            // 
            // tsSep3
            // 
            this.tsSep3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tsSep3.Name = "tsSep3";
            this.tsSep3.Size = new System.Drawing.Size(6, 27);
            // 
            // tsLblPage
            // 
            this.tsLblPage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tsLblPage.Margin = new System.Windows.Forms.Padding(2, 1, 2, 2);
            this.tsLblPage.Name = "tsLblPage";
            this.tsLblPage.Size = new System.Drawing.Size(33, 24);
            this.tsLblPage.Text = "Page";
            // 
            // tsTxtPage
            // 
            this.tsTxtPage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tsTxtPage.Margin = new System.Windows.Forms.Padding(2, 1, 4, 2);
            this.tsTxtPage.Name = "tsTxtPage";
            this.tsTxtPage.ReadOnly = true;
            this.tsTxtPage.Size = new System.Drawing.Size(35, 24);
            this.tsTxtPage.Text = "1";
            this.tsTxtPage.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tsBtnClose
            // 
            this.tsBtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsBtnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsBtnClose.Margin = new System.Windows.Forms.Padding(6, 1, 4, 2);
            this.tsBtnClose.Name = "tsBtnClose";
            this.tsBtnClose.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.tsBtnClose.Size = new System.Drawing.Size(60, 24);
            this.tsBtnClose.Text = "Close";
            this.tsBtnClose.ToolTipText = "Đóng cửa sổ xem trước";
            this.tsBtnClose.Click += new System.EventHandler(this.tsBtnClose_Click);
            // 
            // printPreviewControl1
            // 
            this.printPreviewControl1.AutoZoom = false;
            this.printPreviewControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(103)))), ((int)(((byte)(115)))));
            this.printPreviewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printPreviewControl1.Location = new System.Drawing.Point(0, 0);
            this.printPreviewControl1.Name = "printPreviewControl1";
            this.printPreviewControl1.Size = new System.Drawing.Size(860, 685);
            this.printPreviewControl1.TabIndex = 1;
            this.printPreviewControl1.Zoom = 1D;
            // 
            // pnlCanvasContainer
            // 
            this.pnlCanvasContainer.Controls.Add(this.btnScrollBottom);
            this.pnlCanvasContainer.Controls.Add(this.printPreviewControl1);
            this.pnlCanvasContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanvasContainer.Location = new System.Drawing.Point(0, 35);
            this.pnlCanvasContainer.Name = "pnlCanvasContainer";
            this.pnlCanvasContainer.Size = new System.Drawing.Size(860, 685);
            this.pnlCanvasContainer.TabIndex = 2;
            // 
            // btnScrollBottom
            // 
            this.btnScrollBottom.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnScrollBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnScrollBottom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnScrollBottom.FlatAppearance.BorderSize = 0;
            this.btnScrollBottom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScrollBottom.ForeColor = System.Drawing.Color.White;
            this.btnScrollBottom.Location = new System.Drawing.Point(412, 637);
            this.btnScrollBottom.Name = "btnScrollBottom";
            this.btnScrollBottom.Size = new System.Drawing.Size(36, 36);
            this.btnScrollBottom.TabIndex = 2;
            this.btnScrollBottom.UseVisualStyleBackColor = false;
            this.btnScrollBottom.Click += new System.EventHandler(this.btnScrollBottom_Click);
            // 
            // frmPrintPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(103)))), ((int)(((byte)(115)))));
            this.ClientSize = new System.Drawing.Size(860, 720);
            this.Controls.Add(this.pnlCanvasContainer);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "frmPrintPreview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Xem trước khi in hóa đơn";
            this.Load += new System.EventHandler(this.frmPrintPreview_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlCanvasContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsBtnPrint;
        private System.Windows.Forms.ToolStripSeparator tsSep1;
        private System.Windows.Forms.ToolStripButton tsBtnZoomIn;
        private System.Windows.Forms.ToolStripButton tsBtnZoomOut;
        private System.Windows.Forms.ToolStripSeparator tsSep2;
        private System.Windows.Forms.ToolStripButton tsBtnOnePage;
        private System.Windows.Forms.ToolStripButton tsBtnTwoPages;
        private System.Windows.Forms.ToolStripButton tsBtnThreePages;
        private System.Windows.Forms.ToolStripButton tsBtnFourPages;
        private System.Windows.Forms.ToolStripSeparator tsSep3;
        private System.Windows.Forms.ToolStripLabel tsLblPage;
        private System.Windows.Forms.ToolStripTextBox tsTxtPage;
        private System.Windows.Forms.ToolStripButton tsBtnClose;
        private System.Windows.Forms.PrintPreviewControl printPreviewControl1;
        private System.Windows.Forms.Panel pnlCanvasContainer;
        private System.Windows.Forms.Button btnScrollBottom;
    }
}

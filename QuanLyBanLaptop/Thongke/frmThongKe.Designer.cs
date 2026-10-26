namespace QuanLyBanLaptop.Thongke
{
    partial class frmThongKe
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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.tlpDataAndChart = new System.Windows.Forms.TableLayoutPanel();
            this.pnlGridBox = new System.Windows.Forms.Panel();
            this.lblGridTitle = new System.Windows.Forms.Label();
            this.dgvThongKe = new System.Windows.Forms.DataGridView();
            this.pnlChartBox = new System.Windows.Forms.Panel();
            this.lblChartTitle = new System.Windows.Forms.Label();
            this.chartDoanhthu = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.colThoiGian = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuongBan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDoanhThu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTienNhap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoiNhuan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlKpiCards = new System.Windows.Forms.Panel();
            this.pnlKpiLoiNhuan = new System.Windows.Forms.Panel();
            this.lblValLoiNhuan = new System.Windows.Forms.Label();
            this.lblTitleLoiNhuan = new System.Windows.Forms.Label();
            this.pnlKpiNhap = new System.Windows.Forms.Panel();
            this.lblValNhap = new System.Windows.Forms.Label();
            this.lblTitleNhap = new System.Windows.Forms.Label();
            this.pnlKpiDoanhThu = new System.Windows.Forms.Panel();
            this.lblValDoanhThu = new System.Windows.Forms.Label();
            this.lblTitleDoanhThu = new System.Windows.Forms.Label();
            this.grpBoLoc = new System.Windows.Forms.GroupBox();
            this.btnInBaoCao = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.btnThongKe = new System.Windows.Forms.Button();
            this.cboLoaiThongKe = new System.Windows.Forms.ComboBox();
            this.lblLoai = new System.Windows.Forms.Label();
            this.dtpDenNgay = new System.Windows.Forms.DateTimePicker();
            this.lblDenNgay = new System.Windows.Forms.Label();
            this.dtpTuNgay = new System.Windows.Forms.DateTimePicker();
            this.lblTuNgay = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pnlMain.SuspendLayout();
            this.tlpDataAndChart.SuspendLayout();
            this.pnlGridBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongKe)).BeginInit();
            this.pnlChartBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDoanhthu)).BeginInit();
            this.pnlKpiCards.SuspendLayout();
            this.pnlKpiLoiNhuan.SuspendLayout();
            this.pnlKpiNhap.SuspendLayout();
            this.pnlKpiDoanhThu.SuspendLayout();
            this.grpBoLoc.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidebarControl1
            // 
            this.sidebarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarControl1.Location = new System.Drawing.Point(0, 0);
            this.sidebarControl1.Name = "sidebarControl1";
            this.sidebarControl1.SelectedMenu = QuanLyBanLaptop.Classes.NavigationMenu.ThongKe;
            this.sidebarControl1.Size = new System.Drawing.Size(230, 661);
            this.sidebarControl1.TabIndex = 0;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.pnlMain.Controls.Add(this.tlpDataAndChart);
            this.pnlMain.Controls.Add(this.pnlKpiCards);
            this.pnlMain.Controls.Add(this.grpBoLoc);
            this.pnlMain.Controls.Add(this.pnlHeader);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(230, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(16);
            this.pnlMain.Size = new System.Drawing.Size(959, 661);
            this.pnlMain.TabIndex = 1;
            // 
            // tlpDataAndChart
            // 
            this.tlpDataAndChart.ColumnCount = 2;
            this.tlpDataAndChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlpDataAndChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpDataAndChart.Controls.Add(this.pnlGridBox, 0, 0);
            this.tlpDataAndChart.Controls.Add(this.pnlChartBox, 1, 0);
            this.tlpDataAndChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDataAndChart.Location = new System.Drawing.Point(16, 236);
            this.tlpDataAndChart.Name = "tlpDataAndChart";
            this.tlpDataAndChart.RowCount = 1;
            this.tlpDataAndChart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDataAndChart.Size = new System.Drawing.Size(927, 409);
            this.tlpDataAndChart.TabIndex = 3;
            // 
            // pnlGridBox
            // 
            this.pnlGridBox.BackColor = System.Drawing.Color.White;
            this.pnlGridBox.Controls.Add(this.dgvThongKe);
            this.pnlGridBox.Controls.Add(this.lblGridTitle);
            this.pnlGridBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridBox.Location = new System.Drawing.Point(0, 0);
            this.pnlGridBox.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.pnlGridBox.Name = "pnlGridBox";
            this.pnlGridBox.Padding = new System.Windows.Forms.Padding(8);
            this.pnlGridBox.Size = new System.Drawing.Size(503, 409);
            this.pnlGridBox.TabIndex = 0;
            // 
            // lblGridTitle
            // 
            this.lblGridTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGridTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblGridTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(108)))), ((int)(((byte)(189)))));
            this.lblGridTitle.Location = new System.Drawing.Point(8, 8);
            this.lblGridTitle.Name = "lblGridTitle";
            this.lblGridTitle.Size = new System.Drawing.Size(487, 26);
            this.lblGridTitle.TabIndex = 0;
            this.lblGridTitle.Text = "BẢNG SỐ LIỆU CHI TIẾT";
            this.lblGridTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvThongKe
            // 
            this.dgvThongKe.AllowUserToAddRows = false;
            this.dgvThongKe.AllowUserToDeleteRows = false;
            this.dgvThongKe.AllowUserToResizeColumns = false;
            this.dgvThongKe.AllowUserToResizeRows = false;
            this.dgvThongKe.AllowUserToOrderColumns = false;
            this.dgvThongKe.BackgroundColor = System.Drawing.Color.White;
            this.dgvThongKe.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvThongKe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThongKe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvThongKe.Location = new System.Drawing.Point(8, 34);
            this.dgvThongKe.Name = "dgvThongKe";
            this.dgvThongKe.ReadOnly = true;
            this.dgvThongKe.RowHeadersWidth = 30;
            this.dgvThongKe.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThongKe.Size = new System.Drawing.Size(487, 367);
            this.dgvThongKe.TabIndex = 1;
            // 
            // pnlChartBox
            // 
            this.pnlChartBox.BackColor = System.Drawing.Color.White;
            this.pnlChartBox.Controls.Add(this.chartDoanhthu);
            this.pnlChartBox.Controls.Add(this.lblChartTitle);
            this.pnlChartBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChartBox.Location = new System.Drawing.Point(515, 0);
            this.pnlChartBox.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.pnlChartBox.Name = "pnlChartBox";
            this.pnlChartBox.Padding = new System.Windows.Forms.Padding(8);
            this.pnlChartBox.Size = new System.Drawing.Size(412, 409);
            this.pnlChartBox.TabIndex = 1;
            // 
            // lblChartTitle
            // 
            this.lblChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChartTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblChartTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(108)))), ((int)(((byte)(189)))));
            this.lblChartTitle.Location = new System.Drawing.Point(8, 8);
            this.lblChartTitle.Name = "lblChartTitle";
            this.lblChartTitle.Size = new System.Drawing.Size(396, 26);
            this.lblChartTitle.TabIndex = 0;
            this.lblChartTitle.Text = "BIỂU ĐỒ TRỰC QUAN DOANH THU && LỢI NHUẬN";
            this.lblChartTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chartDoanhthu
            // 
            chartArea1.Name = "ChartArea1";
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            this.chartDoanhthu.ChartAreas.Add(chartArea1);
            this.chartDoanhthu.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.chartDoanhthu.Legends.Add(legend1);
            this.chartDoanhthu.Location = new System.Drawing.Point(8, 34);
            this.chartDoanhthu.Name = "chartDoanhthu";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Doanh thu";
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(108)))), ((int)(((byte)(189)))));
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Lợi nhuận";
            series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.chartDoanhthu.Series.Add(series1);
            this.chartDoanhthu.Series.Add(series2);
            this.chartDoanhthu.Size = new System.Drawing.Size(396, 367);
            this.chartDoanhthu.TabIndex = 1;
            this.chartDoanhthu.Text = "chartDoanhthu";
            // 
            // colThoiGian
            // 
            this.colThoiGian.HeaderText = "Thời gian / Ngày";
            this.colThoiGian.Name = "colThoiGian";
            this.colThoiGian.ReadOnly = true;
            this.colThoiGian.Width = 160;
            // 
            // colSoLuongBan
            // 
            this.colSoLuongBan.HeaderText = "Số máy bán ra";
            this.colSoLuongBan.Name = "colSoLuongBan";
            this.colSoLuongBan.ReadOnly = true;
            this.colSoLuongBan.Width = 140;
            // 
            // colDoanhThu
            // 
            this.colDoanhThu.HeaderText = "Tổng doanh thu";
            this.colDoanhThu.Name = "colDoanhThu";
            this.colDoanhThu.ReadOnly = true;
            this.colDoanhThu.Width = 180;
            // 
            // colTienNhap
            // 
            this.colTienNhap.HeaderText = "Tổng tiền nhập";
            this.colTienNhap.Name = "colTienNhap";
            this.colTienNhap.ReadOnly = true;
            this.colTienNhap.Width = 180;
            // 
            // colLoiNhuan
            // 
            this.colLoiNhuan.HeaderText = "Lợi nhuận ước tính";
            this.colLoiNhuan.Name = "colLoiNhuan";
            this.colLoiNhuan.ReadOnly = true;
            this.colLoiNhuan.Width = 180;
            // 
            // pnlKpiCards
            // 
            this.pnlKpiCards.Controls.Add(this.pnlKpiLoiNhuan);
            this.pnlKpiCards.Controls.Add(this.pnlKpiNhap);
            this.pnlKpiCards.Controls.Add(this.pnlKpiDoanhThu);
            this.pnlKpiCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKpiCards.Location = new System.Drawing.Point(16, 140);
            this.pnlKpiCards.Name = "pnlKpiCards";
            this.pnlKpiCards.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.pnlKpiCards.Size = new System.Drawing.Size(927, 96);
            this.pnlKpiCards.TabIndex = 2;
            // 
            // pnlKpiLoiNhuan
            // 
            this.pnlKpiLoiNhuan.BackColor = System.Drawing.Color.White;
            this.pnlKpiLoiNhuan.Controls.Add(this.lblValLoiNhuan);
            this.pnlKpiLoiNhuan.Controls.Add(this.lblTitleLoiNhuan);
            this.pnlKpiLoiNhuan.Location = new System.Drawing.Point(590, 8);
            this.pnlKpiLoiNhuan.Name = "pnlKpiLoiNhuan";
            this.pnlKpiLoiNhuan.Padding = new System.Windows.Forms.Padding(16);
            this.pnlKpiLoiNhuan.Size = new System.Drawing.Size(265, 80);
            this.pnlKpiLoiNhuan.TabIndex = 2;
            // 
            // lblValLoiNhuan
            // 
            this.lblValLoiNhuan.AutoSize = true;
            this.lblValLoiNhuan.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValLoiNhuan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblValLoiNhuan.Location = new System.Drawing.Point(12, 38);
            this.lblValLoiNhuan.Name = "lblValLoiNhuan";
            this.lblValLoiNhuan.Size = new System.Drawing.Size(147, 25);
            this.lblValLoiNhuan.TabIndex = 1;
            this.lblValLoiNhuan.Text = "+85.200.000 đ";
            // 
            // lblTitleLoiNhuan
            // 
            this.lblTitleLoiNhuan.AutoSize = true;
            this.lblTitleLoiNhuan.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleLoiNhuan.ForeColor = System.Drawing.Color.Gray;
            this.lblTitleLoiNhuan.Location = new System.Drawing.Point(12, 14);
            this.lblTitleLoiNhuan.Name = "lblTitleLoiNhuan";
            this.lblTitleLoiNhuan.Size = new System.Drawing.Size(117, 17);
            this.lblTitleLoiNhuan.TabIndex = 0;
            this.lblTitleLoiNhuan.Text = "Lợi nhuận ước tính";
            // 
            // pnlKpiNhap
            // 
            this.pnlKpiNhap.BackColor = System.Drawing.Color.White;
            this.pnlKpiNhap.Controls.Add(this.lblValNhap);
            this.pnlKpiNhap.Controls.Add(this.lblTitleNhap);
            this.pnlKpiNhap.Location = new System.Drawing.Point(295, 8);
            this.pnlKpiNhap.Name = "pnlKpiNhap";
            this.pnlKpiNhap.Padding = new System.Windows.Forms.Padding(16);
            this.pnlKpiNhap.Size = new System.Drawing.Size(265, 80);
            this.pnlKpiNhap.TabIndex = 1;
            // 
            // lblValNhap
            // 
            this.lblValNhap.AutoSize = true;
            this.lblValNhap.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValNhap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            this.lblValNhap.Location = new System.Drawing.Point(12, 38);
            this.lblValNhap.Name = "lblValNhap";
            this.lblValNhap.Size = new System.Drawing.Size(142, 25);
            this.lblValNhap.TabIndex = 1;
            this.lblValNhap.Text = "240.000.000 đ";
            // 
            // lblTitleNhap
            // 
            this.lblTitleNhap.AutoSize = true;
            this.lblTitleNhap.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleNhap.ForeColor = System.Drawing.Color.Gray;
            this.lblTitleNhap.Location = new System.Drawing.Point(12, 14);
            this.lblTitleNhap.Name = "lblTitleNhap";
            this.lblTitleNhap.Size = new System.Drawing.Size(124, 17);
            this.lblTitleNhap.TabIndex = 0;
            this.lblTitleNhap.Text = "Tổng chi nhập hàng";
            // 
            // pnlKpiDoanhThu
            // 
            this.pnlKpiDoanhThu.BackColor = System.Drawing.Color.White;
            this.pnlKpiDoanhThu.Controls.Add(this.lblValDoanhThu);
            this.pnlKpiDoanhThu.Controls.Add(this.lblTitleDoanhThu);
            this.pnlKpiDoanhThu.Location = new System.Drawing.Point(0, 8);
            this.pnlKpiDoanhThu.Name = "pnlKpiDoanhThu";
            this.pnlKpiDoanhThu.Padding = new System.Windows.Forms.Padding(16);
            this.pnlKpiDoanhThu.Size = new System.Drawing.Size(265, 80);
            this.pnlKpiDoanhThu.TabIndex = 0;
            // 
            // lblValDoanhThu
            // 
            this.lblValDoanhThu.AutoSize = true;
            this.lblValDoanhThu.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValDoanhThu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(101)))), ((int)(((byte)(185)))));
            this.lblValDoanhThu.Location = new System.Drawing.Point(12, 38);
            this.lblValDoanhThu.Name = "lblValDoanhThu";
            this.lblValDoanhThu.Size = new System.Drawing.Size(142, 25);
            this.lblValDoanhThu.TabIndex = 1;
            this.lblValDoanhThu.Text = "325.200.000 đ";
            // 
            // lblTitleDoanhThu
            // 
            this.lblTitleDoanhThu.AutoSize = true;
            this.lblTitleDoanhThu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleDoanhThu.ForeColor = System.Drawing.Color.Gray;
            this.lblTitleDoanhThu.Location = new System.Drawing.Point(12, 14);
            this.lblTitleDoanhThu.Name = "lblTitleDoanhThu";
            this.lblTitleDoanhThu.Size = new System.Drawing.Size(126, 17);
            this.lblTitleDoanhThu.TabIndex = 0;
            this.lblTitleDoanhThu.Text = "Tổng doanh thu bán";
            // 
            // grpBoLoc
            // 
            this.grpBoLoc.Controls.Add(this.btnXuatExcel);
            this.grpBoLoc.Controls.Add(this.btnInBaoCao);
            this.grpBoLoc.Controls.Add(this.btnThongKe);
            this.grpBoLoc.Controls.Add(this.cboLoaiThongKe);
            this.grpBoLoc.Controls.Add(this.lblLoai);
            this.grpBoLoc.Controls.Add(this.dtpDenNgay);
            this.grpBoLoc.Controls.Add(this.lblDenNgay);
            this.grpBoLoc.Controls.Add(this.dtpTuNgay);
            this.grpBoLoc.Controls.Add(this.lblTuNgay);
            this.grpBoLoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpBoLoc.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoLoc.Location = new System.Drawing.Point(16, 56);
            this.grpBoLoc.Name = "grpBoLoc";
            this.grpBoLoc.Size = new System.Drawing.Size(927, 84);
            this.grpBoLoc.TabIndex = 1;
            this.grpBoLoc.TabStop = false;
            this.grpBoLoc.Text = "Bộ lọc thống kê";
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnXuatExcel.FlatAppearance.BorderSize = 0;
            this.btnXuatExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXuatExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnXuatExcel.ForeColor = System.Drawing.Color.White;
            this.btnXuatExcel.Location = new System.Drawing.Point(765, 31);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(95, 30);
            this.btnXuatExcel.TabIndex = 8;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = false;
            // 
            // btnInBaoCao
            // 
            this.btnInBaoCao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(34)))), ((int)(((byte)(206)))));
            this.btnInBaoCao.FlatAppearance.BorderSize = 0;
            this.btnInBaoCao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInBaoCao.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnInBaoCao.ForeColor = System.Drawing.Color.White;
            this.btnInBaoCao.Location = new System.Drawing.Point(865, 31);
            this.btnInBaoCao.Name = "btnInBaoCao";
            this.btnInBaoCao.Size = new System.Drawing.Size(90, 30);
            this.btnInBaoCao.TabIndex = 7;
            this.btnInBaoCao.Text = "In báo cáo";
            this.btnInBaoCao.UseVisualStyleBackColor = false;
            this.btnInBaoCao.Visible = false;
            // 
            // btnThongKe
            // 
            this.btnThongKe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(108)))), ((int)(((byte)(189)))));
            this.btnThongKe.FlatAppearance.BorderSize = 0;
            this.btnThongKe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThongKe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnThongKe.ForeColor = System.Drawing.Color.White;
            this.btnThongKe.Location = new System.Drawing.Point(665, 31);
            this.btnThongKe.Name = "btnThongKe";
            this.btnThongKe.Size = new System.Drawing.Size(95, 30);
            this.btnThongKe.TabIndex = 6;
            this.btnThongKe.Text = "Thống kê";
            this.btnThongKe.UseVisualStyleBackColor = false;
            // 
            // cboLoaiThongKe
            // 
            this.cboLoaiThongKe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiThongKe.FormattingEnabled = true;
            this.cboLoaiThongKe.Location = new System.Drawing.Point(525, 34);
            this.cboLoaiThongKe.Name = "cboLoaiThongKe";
            this.cboLoaiThongKe.Size = new System.Drawing.Size(140, 25);
            this.cboLoaiThongKe.TabIndex = 5;
            // 
            // lblLoai
            // 
            this.lblLoai.AutoSize = true;
            this.lblLoai.Location = new System.Drawing.Point(485, 37);
            this.lblLoai.Name = "lblLoai";
            this.lblLoai.Size = new System.Drawing.Size(32, 17);
            this.lblLoai.TabIndex = 4;
            this.lblLoai.Text = "Loại";
            // 
            // dtpDenNgay
            // 
            this.dtpDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpDenNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDenNgay.Location = new System.Drawing.Point(315, 34);
            this.dtpDenNgay.Name = "dtpDenNgay";
            this.dtpDenNgay.Size = new System.Drawing.Size(150, 25);
            this.dtpDenNgay.TabIndex = 3;
            // 
            // lblDenNgay
            // 
            this.lblDenNgay.AutoSize = true;
            this.lblDenNgay.Location = new System.Drawing.Point(245, 37);
            this.lblDenNgay.Name = "lblDenNgay";
            this.lblDenNgay.Size = new System.Drawing.Size(64, 17);
            this.lblDenNgay.TabIndex = 2;
            this.lblDenNgay.Text = "Đến ngày";
            // 
            // dtpTuNgay
            // 
            this.dtpTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpTuNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTuNgay.Location = new System.Drawing.Point(75, 34);
            this.dtpTuNgay.Name = "dtpTuNgay";
            this.dtpTuNgay.Size = new System.Drawing.Size(150, 25);
            this.dtpTuNgay.TabIndex = 1;
            // 
            // lblTuNgay
            // 
            this.lblTuNgay.AutoSize = true;
            this.lblTuNgay.Location = new System.Drawing.Point(15, 37);
            this.lblTuNgay.Name = "lblTuNgay";
            this.lblTuNgay.Size = new System.Drawing.Size(55, 17);
            this.lblTuNgay.TabIndex = 0;
            this.lblTuNgay.Text = "Từ ngày";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(16, 16);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(927, 40);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(36)))), ((int)(((byte)(71)))));
            this.lblTitle.Location = new System.Drawing.Point(3, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(370, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "TRANG THỐNG KÊ DOANH THU BÁN HÀNG";
            // 
            // frmThongKe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 661);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.sidebarControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmThongKe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TRANG THỐNG KÊ";
            this.pnlMain.ResumeLayout(false);
            this.tlpDataAndChart.ResumeLayout(false);
            this.pnlGridBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongKe)).EndInit();
            this.pnlChartBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartDoanhthu)).EndInit();
            this.pnlKpiCards.ResumeLayout(false);
            this.pnlKpiLoiNhuan.ResumeLayout(false);
            this.pnlKpiLoiNhuan.PerformLayout();
            this.pnlKpiNhap.ResumeLayout(false);
            this.pnlKpiNhap.PerformLayout();
            this.pnlKpiDoanhThu.ResumeLayout(false);
            this.pnlKpiDoanhThu.PerformLayout();
            this.grpBoLoc.ResumeLayout(false);
            this.grpBoLoc.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SidebarControl sidebarControl1;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpBoLoc;
        private System.Windows.Forms.DateTimePicker dtpTuNgay;
        private System.Windows.Forms.Label lblTuNgay;
        private System.Windows.Forms.DateTimePicker dtpDenNgay;
        private System.Windows.Forms.Label lblDenNgay;
        private System.Windows.Forms.ComboBox cboLoaiThongKe;
        private System.Windows.Forms.Label lblLoai;
        private System.Windows.Forms.Button btnThongKe;
        private System.Windows.Forms.Button btnInBaoCao;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Panel pnlKpiCards;
        private System.Windows.Forms.Panel pnlKpiDoanhThu;
        private System.Windows.Forms.Label lblTitleDoanhThu;
        private System.Windows.Forms.Label lblValDoanhThu;
        private System.Windows.Forms.Panel pnlKpiNhap;
        private System.Windows.Forms.Label lblValNhap;
        private System.Windows.Forms.Label lblTitleNhap;
        private System.Windows.Forms.Panel pnlKpiLoiNhuan;
        private System.Windows.Forms.Label lblValLoiNhuan;
        private System.Windows.Forms.Label lblTitleLoiNhuan;
        private System.Windows.Forms.TableLayoutPanel tlpDataAndChart;
        private System.Windows.Forms.Panel pnlGridBox;
        private System.Windows.Forms.Label lblGridTitle;
        private System.Windows.Forms.DataGridView dgvThongKe;
        private System.Windows.Forms.Panel pnlChartBox;
        private System.Windows.Forms.Label lblChartTitle;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDoanhthu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThoiGian;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuongBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDoanhThu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTienNhap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoiNhuan;
    }
}
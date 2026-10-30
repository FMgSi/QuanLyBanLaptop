using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.Services;
namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmBaoHanhSuaChua : AppForm
    {
        public class TicketItem
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public string EmpId { get; set; }
            public string EmpName { get; set; }
            public string CustId { get; set; }
            public string CustName { get; set; }
            public string Device { get; set; }
            public string Problem { get; set; }
        }
        public class RepairServiceItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Symptom { get; set; }
            public string Price { get; set; }
            public string Time { get; set; }
            public string Warranty { get; set; }
        }
        public class WarrantyServiceItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Condition { get; set; }
            public string Fee { get; set; }
            public string Time { get; set; }
        }
        private readonly WarrantyService warrantyService = new WarrantyService();
        private string selectedSerial;
        private List<TicketItem> allTickets = new List<TicketItem>();
        private List<RepairServiceItem> repairServicesCatalog = new List<RepairServiceItem>();
        private List<WarrantyServiceItem> warrantyServicesCatalog = new List<WarrantyServiceItem>();
        private string currentTab = "all", selectedTicketId, selectedServiceId;
        private enum ActionState { Normal, Adding, Selected, Editing }
        private ActionState currentActionState = ActionState.Normal;
        public frmBaoHanhSuaChua()
        {
            InitializeComponent();
            dgvBaoHanh.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgvBaoHanh.ColumnHeadersDefaultCellStyle.BackColor;
            dgvBaoHanh.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgvBaoHanh.ColumnHeadersDefaultCellStyle.ForeColor;
            dgvBaoHanh.SelectionChanged += (s, e) => UpdateExcelButtonState();
            NavigationManager.RegisterForm(NavigationMenu.BaoHanhSuaChua, this);
            sidebarControl1.SelectedMenu = NavigationMenu.BaoHanhSuaChua;
            InitCatalogData();
            LoadTickets();
            SwitchTab("all");
            UpdateUserInfoDisplay();
            txtMaPhieu.Text = SinhMaPhieuMoi();
            pnlHeaderTop.Visible = !NavigationManager.IsHosted;
            SetActionState(ActionState.Normal);
        }
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!Visible) return;
            sidebarControl1.SelectedMenu = NavigationMenu.BaoHanhSuaChua;
            UpdateUserInfoDisplay();
            LoadTickets();
            RenderGrid();
            if (string.IsNullOrWhiteSpace(txtMaPhieu.Text)) txtMaPhieu.Text = SinhMaPhieuMoi();
            pnlHeaderTop.Visible = !NavigationManager.IsHosted;
            SetActionState(ActionState.Normal);
        }
        private void UpdateUserInfoDisplay()
        {
            string name = UserSession.TenNV ?? "", id = UserSession.MaNV ?? "";
            string role = !string.IsNullOrEmpty(UserSession.ChucVu) ? UserSession.ChucVu : "Quản trị viên";
            lblHeaderUserInfo.Text = $"👤 {name} ({id}) - {role}";
            txtMaNV.Text = id;
            txtTenNV.Text = name;
        }
        private string SinhMaPhieuMoi() => "(Tự động)";
        private void btnChonKhachHang_Click(object sender, EventArgs e)
        {
            using (var frm = new frmTimKiemKhachHang())
            {
                if (frm.ShowDialog(this) != DialogResult.OK || string.IsNullOrEmpty(frm.SelectedMaKH)) return;
                selectedSerial = null;
                txtTenMay.Clear();
                txtMaKH.Text = frm.SelectedMaKH;
                txtTenKH.Text = frm.SelectedTenKH;
                lblFooterNote.Text = $"Đã chọn khách hàng: {frm.SelectedTenKH} ({frm.SelectedMaKH})";
                txtTenMay.Focus();
            }
        }
        private void btnChonSanPham_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKH.Text)) { Function.ThongBaoCanhBao("Hãy chọn khách hàng trước khi chọn serial máy."); return; }
            if (currentTab == "repair")
            {
                using (var picker = new HoaDon.frmTimKiemLaptop(true))
                {
                    if (picker.ShowDialog(this) == DialogResult.OK)
                    {
                        selectedSerial = null;
                        txtTenMay.Text = picker.SelectedTenLaptop;
                        lblFooterNote.Text = "Máy sửa chữa: " + picker.SelectedTenLaptop + " (không yêu cầu mua tại cửa hàng).";
                    }
                }
                return;
            }
            try
            {
                var devices = warrantyService.GetCustomerDevices(txtMaKH.Text.Trim(), currentTab == "warranty");
                if (devices.Rows.Count == 0)
                {
                    selectedSerial = null;
                    txtTenMay.Clear();
                    Function.ThongBaoCanhBao(currentTab == "warranty"
                        ? "Khách hàng không có máy còn hạn bảo hành có thể tiếp nhận."
                        : "Khách hàng không có máy đã mua có thể tiếp nhận sửa chữa.");
                    return;
                }
                using (var dialog = new Form { Text = "Chọn serial máy của khách hàng", Width = 750, Height = 400, StartPosition = FormStartPosition.CenterParent })
                using (var grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AllowUserToAddRows = false, MultiSelect = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, DataSource = devices })
                using (var choose = new Button { Text = "Chọn máy", Dock = DockStyle.Bottom, Height = 36 })
                {
                    choose.Click += (buttonSender, args) =>
                    {
                        if (grid.CurrentRow == null) return;
                        selectedSerial = Convert.ToString(grid.CurrentRow.Cells["SerialNo"].Value);
                        txtTenMay.Text = Convert.ToString(grid.CurrentRow.Cells["TenLaptop"].Value);
                        lblFooterNote.Text = "Serial: " + selectedSerial;
                        dialog.DialogResult = DialogResult.OK;
                    };
                    dialog.Controls.Add(grid);
                    grid.Columns["SerialNo"].HeaderText = "Số serial";
                    grid.Columns["TenLaptop"].HeaderText = "Tên máy đã mua";
                    grid.Columns["NgayHetBH"].HeaderText = "Hạn bảo hành";
                    grid.Columns["NgayHetBH"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dialog.Controls.Add(choose);
                    dialog.ShowDialog(this);
                }
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }
        private void InitCatalogData()
        {
            repairServicesCatalog = new List<RepairServiceItem>
            {
                new RepairServiceItem { Id = "DV-SC01", Name = "Vệ sinh laptop & Tra keo tản nhiệt gốm MX4", Symptom = "Máy nóng rực, quạt rú to, tự ngắt khi tải nặng", Price = "150.000 đ", Time = "30 - 45 phút", Warranty = "1 tháng" },
                new RepairServiceItem { Id = "DV-SC02", Name = "Thay màn hình Laptop Full HD IPS 144Hz", Symptom = "Sọc màn hình, nhấp nháy chập chờn, nứt vỡ LCD", Price = "1.650.000 đ", Time = "45 phút", Warranty = "6 tháng" },
                new RepairServiceItem { Id = "DV-SC03", Name = "Sửa nguồn bo mạch chủ (Mainboard) chạm IC", Symptom = "Mất nguồn hoàn toàn, cắm sạc đèn không sáng", Price = "650.000 đ", Time = "1 - 2 ngày", Warranty = "3 tháng" },
                new RepairServiceItem { Id = "DV-SC04", Name = "Thay cụm bàn phím laptop chuẩn hãng", Symptom = "Liệt phím, kẹt phím, tự động nhảy ký tự liên tục", Price = "450.000 đ", Time = "40 phút", Warranty = "12 tháng" },
                new RepairServiceItem { Id = "DV-SC05", Name = "Thay pin laptop zin dung lượng cao", Symptom = "Pin chai phồng, pin ảo dùng 15 phút sập nguồn", Price = "780.000 đ", Time = "30 phút", Warranty = "6 tháng" },
                new RepairServiceItem { Id = "DV-SC06", Name = "Hàn phục hồi chân ốc & bản lề gãy vỡ", Symptom = "Vỏ bung cọt kẹt khi gập mở, hở mép viền", Price = "350.000 đ", Time = "2 - 3 giờ", Warranty = "3 tháng" },
                new RepairServiceItem { Id = "DV-SC07", Name = "Nâng cấp ổ cứng SSD NVMe PCIe 512GB", Symptom = "Máy khởi động rất chậm, ổ HDD cũ bị Bad Sector", Price = "890.000 đ", Time = "30 phút", Warranty = "36 tháng" },
                new RepairServiceItem { Id = "DV-SC08", Name = "Đóng lại / Thay thế Chipset VGA đồ họa rời", Symptom = "Rác hình, màn hình xanh BSOD khi bật game", Price = "1.250.000 đ", Time = "2 - 3 ngày", Warranty = "3 tháng" }
            };
            warrantyServicesCatalog = new List<WarrantyServiceItem>
            {
                new WarrantyServiceItem { Id = "DV-BH01", Name = "Bảo hành đổi mới Pin linh kiện chính hãng", Condition = "Pin chai vượt mức 30% trong thời hạn bảo hành hãng", Fee = "0 đ (Bảo hành)", Time = "1 - 2 ngày" },
                new WarrantyServiceItem { Id = "DV-BH02", Name = "Bảo hành lỗi điểm chết & sọc màn hình LCD", Condition = "Xuất hiện trên 3 điểm chết hoặc sọc chỉ màn hình", Fee = "0 đ (Bảo hành)", Time = "3 - 5 ngày" },
                new WarrantyServiceItem { Id = "DV-BH03", Name = "Bảo hành bàn phím & Touchpad theo tiêu chuẩn hãng", Condition = "Liệt cụm phím, chập chuột cảm ứng do lỗi linh kiện", Fee = "0 đ (Bảo hành)", Time = "2 - 4 ngày" },
                new WarrantyServiceItem { Id = "DV-BH04", Name = "Bảo hành loa trong & cụm Micro đàm thoại", Condition = "Rè loa, mất tín hiệu mic thu âm không do ẩm nước", Fee = "0 đ (Bảo hành)", Time = "1 - 2 ngày" },
                new WarrantyServiceItem { Id = "DV-BH05", Name = "Bảo hành Bo mạch chủ (Mainboard)", Condition = "Lỗi nguồn chip, không nhận SSD/RAM theo tem bảo hành", Fee = "0 đ (Bảo hành)", Time = "5 - 7 ngày" },
                new WarrantyServiceItem { Id = "DV-BH06", Name = "Bảo hành sạc Adapter theo máy", Condition = "Không sạc vào điện, tiếp xúc chập chờn nguyên tem", Fee = "0 đ (Bảo hành)", Time = "Đổi mới ngay" }
            };
        }
        private void LoadTickets()
        {
            allTickets.Clear();
            try
            {
                foreach (DataRow row in warrantyService.GetTickets().Rows)
                    allTickets.Add(new TicketItem
                    {
                        Id = Convert.ToString(row["MaPhieuBH"]),
                        Type = Convert.ToString(row["LoaiDichVu"]) == "Bảo hành" ? "warranty" : "repair",
                        EmpId = Convert.ToString(row["MaNV"]), EmpName = Convert.ToString(row["TenNV"]),
                        CustId = Convert.ToString(row["MaKH"]), CustName = Convert.ToString(row["TenKH"]),
                        Device = Convert.ToString(row["TenLaptop"]), Problem = Convert.ToString(row["MoTaLoi"])
                    });
            }
            catch (Exception ex) { Function.ThongBaoLoi("Không tải được phiếu bảo hành: " + ex.Message); }
        }
        private void SwitchTab(string tab)
        {
            currentTab = tab;
            pnlFormContainer.Visible = tab != "all";
            HandleClearForm();
            SetActionState(ActionState.Normal);
            selectedTicketId = selectedServiceId = null;
            lblCurrentSelected.Text = "Đang chọn: Chưa chọn";
            Color activeBg = Color.FromArgb(17, 35, 64), inactiveBg = Color.White;
            Color activeFg = Color.White, inactiveFg = Color.FromArgb(51, 65, 85);
            foreach (var (btn, key) in new[] { (btnTabAll, "all"), (btnTabRepair, "repair"), (btnTabWarranty, "warranty") })
            {
                bool active = tab == key;
                btn.BackColor = active ? activeBg : inactiveBg;
                btn.ForeColor = active ? activeFg : inactiveFg;
                btn.Font = new Font("Segoe UI", 9F, active ? FontStyle.Bold : FontStyle.Regular);
            }
            if (tab == "all")
            {
                lblFormHeader.Text = "TỔNG HỢP THÔNG TIN PHIẾU TIẾP NHẬN";
                lblTableTitle.Text = "DANH SÁCH TẤT CẢ PHIẾU DỊCH VỤ SỬA CHỮA & BẢO HÀNH";
                lblTableHint.Text = "Bấm vào dòng để xem thông tin phiếu";
                lblFooterNote.Text = "Bấm vào một dòng trong bảng để chọn nhanh";
            }
            else if (tab == "repair")
            {
                lblFormHeader.Text = "THÔNG TIN TIẾP NHẬN SỬA CHỮA";
                lblTableTitle.Text = "BẢNG CÁC DỊCH VỤ SỬA MÁY (BẤM VÀO DÒNG ĐỂ CHỌN DỊCH VỤ VÀO PHIẾU)";
                lblTableHint.Text = "Bấm trực tiếp dòng dịch vụ để nạp nhanh vào mục Lỗi / Dịch vụ";
                lblFooterNote.Text = "Bấm vào bất kỳ dịch vụ nào để chọn vào phiếu tiếp nhận";
            }
            else
            {
                lblFormHeader.Text = "THÔNG TIN TIẾP NHẬN BẢO HÀNH";
                lblTableTitle.Text = "BẢNG CÁC DỊCH VỤ & HẠNG MỤC BẢO HÀNH (BẤM VÀO DÒNG ĐỂ CHỌN)";
                lblTableHint.Text = "Bấm trực tiếp hạng mục bảo hành để nạp nhanh vào mục Lỗi / Dịch vụ";
                lblFooterNote.Text = "Bấm vào bất kỳ hạng mục nào để chọn vào phiếu tiếp nhận";
            }
            RenderGrid();
        }
        private void btnTabAll_Click(object sender, EventArgs e) => SwitchTab("all");
        private void btnTabRepair_Click(object sender, EventArgs e) => SwitchTab("repair");
        private void btnTabWarranty_Click(object sender, EventArgs e) => SwitchTab("warranty");
        private void RenderGrid()
        {
            dgvBaoHanh.Columns.Clear();
            dgvBaoHanh.AutoGenerateColumns = false;
            dgvBaoHanh.AllowUserToResizeColumns = true;
            dgvBaoHanh.ScrollBars = ScrollBars.Both;
            if (currentTab == "all")
            {
                lblRecordCount.Text = $"Tổng số: {allTickets.Count} phiếu";
                dgvBaoHanh.Columns.AddRange(
                    new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "Mã phiếu", Width = 95, MinimumWidth = 85 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "TypeDisplay", HeaderText = "Phân loại", Width = 110, MinimumWidth = 95 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "EmpId", HeaderText = "Mã NV", Width = 85, MinimumWidth = 80 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "EmpName", HeaderText = "Tên nhân viên", Width = 150, MinimumWidth = 130 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "CustId", HeaderText = "Mã KH", Width = 85, MinimumWidth = 80 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "CustName", HeaderText = "Tên khách hàng", Width = 150, MinimumWidth = 130 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "Device", HeaderText = "Tên máy", Width = 170, MinimumWidth = 140 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "Problem", HeaderText = "Lỗi / Dịch vụ tiếp nhận", Width = 250, MinimumWidth = 200, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                dgvBaoHanh.DataSource = allTickets.Select(t => new { t.Id, TypeDisplay = t.Type == "repair" ? "Sửa chữa" : "Bảo hành", t.EmpId, t.EmpName, t.CustId, t.CustName, t.Device, t.Problem }).ToList();
                for (int i = 0; i < dgvBaoHanh.Rows.Count; i++)
                {
                    string pl = dgvBaoHanh.Rows[i].Cells[1].Value?.ToString() ?? "";
                    dgvBaoHanh.Rows[i].Cells[1].Style.ForeColor = pl == "Sửa chữa" ? Color.FromArgb(29, 78, 216) : Color.FromArgb(126, 34, 206);
                    dgvBaoHanh.Rows[i].Cells[1].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }
            else if (currentTab == "repair")
            {
                lblRecordCount.Text = $"Tổng số: {repairServicesCatalog.Count} dịch vụ có sẵn";
                dgvBaoHanh.Columns.AddRange(
                    new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "Mã DV", Width = 95, MinimumWidth = 85 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Tên dịch vụ sửa máy", Width = 280, MinimumWidth = 180 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "Symptom", HeaderText = "Triệu chứng lỗi thường gặp", Width = 260, MinimumWidth = 180, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill },
                    new DataGridViewTextBoxColumn { DataPropertyName = "Price", HeaderText = "Giá tham khảo", Width = 140, MinimumWidth = 120 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "Time", HeaderText = "Thời gian", Width = 130, MinimumWidth = 110 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "Warranty", HeaderText = "Bảo hành", Width = 120, MinimumWidth = 100 });
                dgvBaoHanh.DataSource = repairServicesCatalog.ToList();
                for (int i = 0; i < dgvBaoHanh.Rows.Count; i++)
                {
                    dgvBaoHanh.Rows[i].Cells[0].Style.ForeColor = Color.FromArgb(17, 35, 64);
                    dgvBaoHanh.Rows[i].Cells[0].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    dgvBaoHanh.Rows[i].Cells[3].Style.ForeColor = Color.FromArgb(21, 128, 61);
                    dgvBaoHanh.Rows[i].Cells[3].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }
            else if (currentTab == "warranty")
            {
                lblRecordCount.Text = $"Tổng số: {warrantyServicesCatalog.Count} hạng mục bảo hành";
                dgvBaoHanh.Columns.AddRange(
                    new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "Mã BH", Width = 95, MinimumWidth = 85 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Hạng mục bảo hành", Width = 300, MinimumWidth = 200 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "Condition", HeaderText = "Điều kiện tiếp nhận bảo hành", Width = 280, MinimumWidth = 200, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill },
                    new DataGridViewTextBoxColumn { DataPropertyName = "Fee", HeaderText = "Chi phí", Width = 150, MinimumWidth = 120 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "Time", HeaderText = "Thời gian xử lý", Width = 150, MinimumWidth = 120 });
                dgvBaoHanh.DataSource = warrantyServicesCatalog.ToList();
                for (int i = 0; i < dgvBaoHanh.Rows.Count; i++)
                {
                    dgvBaoHanh.Rows[i].Cells[0].Style.ForeColor = Color.FromArgb(126, 34, 206);
                    dgvBaoHanh.Rows[i].Cells[0].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    dgvBaoHanh.Rows[i].Cells[3].Style.ForeColor = Color.FromArgb(21, 128, 61);
                    dgvBaoHanh.Rows[i].Cells[3].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }
            string targetId = currentTab == "all" ? selectedTicketId : selectedServiceId;
            bool hasSelection = false;
            if (!string.IsNullOrEmpty(targetId))
            {
                for (int i = 0; i < dgvBaoHanh.Rows.Count; i++)
                {
                    if (dgvBaoHanh.Rows[i].Cells[0].Value?.ToString() == targetId)
                    {
                        dgvBaoHanh.Rows[i].Selected = true;
                        try { dgvBaoHanh.FirstDisplayedScrollingRowIndex = i; } catch { }
                        hasSelection = true;
                        break;
                    }
                }
            }
            if (!hasSelection) dgvBaoHanh.ClearSelection();
            UpdateExcelButtonState();
        }
        private void dgvBaoHanh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvBaoHanh.Rows.Count) return;
            string id = dgvBaoHanh.Rows[e.RowIndex].Cells[0].Value?.ToString();
            if (currentTab == "all") SelectTicketRow(id);
            else SelectServiceRow(id, currentTab);
        }
        private void dgvBaoHanh_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvBaoHanh.Rows.Count) return;
            if (dgvBaoHanh.Columns[e.ColumnIndex] is DataGridViewButtonColumn && (currentTab == "repair" || currentTab == "warranty"))
                SelectServiceRow(dgvBaoHanh.Rows[e.RowIndex].Cells[0].Value?.ToString(), currentTab);
        }
        private void SetActionState(ActionState state)
        {
            bool viewOnly = currentTab == "all";
            if (viewOnly) state = ActionState.Normal;
            currentActionState = state;
            bool editing = state == ActionState.Adding || state == ActionState.Editing;
            bool selected = state == ActionState.Selected;
            foreach (var button in new[] { btnThem, btnSua, btnXoa, btnLuu, btnHuy }) button.Visible = !viewOnly;
            ButtonTheme.SetButtonState(btnThem, !viewOnly && state == ActionState.Normal);
            ButtonTheme.SetButtonState(btnSua, selected);
            ButtonTheme.SetButtonState(btnXoa, selected && UserSession.IsAdmin, Color.FromArgb(185, 28, 28));
            ButtonTheme.SetButtonState(btnLuu, editing, Color.FromArgb(21, 128, 61));
            ButtonTheme.SetButtonState(btnHuy, state != ActionState.Normal);
            ButtonTheme.SetButtonState(btnThoat, true);
            UpdateExcelButtonState();
            txtTenKH.ReadOnly = txtTenMay.ReadOnly = txtMaPhieu.ReadOnly = txtMaNV.ReadOnly = txtTenNV.ReadOnly = txtMaKH.ReadOnly = true;
            txtLoiDichVu.ReadOnly = !editing;
            btnChonKhachHang.Enabled = btnChonSanPham.Enabled = state == ActionState.Adding;
        }
        private void UpdateExcelButtonState()
        {
            bool hasRow = dgvBaoHanh.SelectedRows.Count == 1 || (dgvBaoHanh.CurrentRow != null && dgvBaoHanh.CurrentRow.Index >= 0 && dgvBaoHanh.CurrentRow.Selected);
            ButtonTheme.SetButtonState(btnXuatExcel, hasRow, Color.FromArgb(21, 128, 61));
        }
        private void SelectTicketRow(string id)
        {
            var item = allTickets.FirstOrDefault(x => x.Id == id);
            if (item == null) return;
            selectedTicketId = item.Id;
            selectedServiceId = null;
            lblCurrentSelected.Text = $"Đang chọn: Phiếu {item.Id}";
            lblFooterNote.Text = $"Đã nạp thông tin phiếu [{item.Id}]. Bấm 'Sửa' để cập nhật hoặc 'Hủy' để bỏ chọn.";
            txtMaPhieu.Text = item.Id;
            txtMaNV.Text = item.EmpId;
            txtTenNV.Text = item.EmpName;
            txtMaKH.Text = item.CustId;
            txtTenKH.Text = item.CustName;
            txtTenMay.Text = item.Device;
            txtLoiDichVu.Text = item.Problem;
            SetActionState(ActionState.Selected);
        }
        private void SelectServiceRow(string id, string type)
        {
            selectedServiceId = id;
            selectedTicketId = null;
            lblCurrentSelected.Text = $"Đang chọn: Dịch vụ {id}";
            if (string.IsNullOrWhiteSpace(txtMaNV.Text)) { txtMaNV.Text = UserSession.MaNV ?? ""; txtTenNV.Text = UserSession.TenNV ?? ""; }
            if (string.IsNullOrWhiteSpace(txtMaPhieu.Text) || !txtMaPhieu.Text.StartsWith("MP_")) txtMaPhieu.Text = SinhMaPhieuMoi();
            if (type == "repair")
            {
                var s = repairServicesCatalog.FirstOrDefault(x => x.Id == id);
                if (s != null) { txtLoiDichVu.Text = $"{s.Name} - [Lỗi: {s.Symptom}]"; lblFooterNote.Text = $"Đã chọn: {s.Name}. Bấm [🔍 Khách hàng] để chọn khách rồi bấm 'Lưu'!"; }
            }
            else
            {
                var w = warrantyServicesCatalog.FirstOrDefault(x => x.Id == id);
                if (w != null) { txtLoiDichVu.Text = $"{w.Name} - [ĐK: {w.Condition}]"; lblFooterNote.Text = $"Đã chọn: {w.Name}. Bấm [🔍 Khách hàng] để chọn khách rồi bấm 'Lưu'!"; }
            }
            SetActionState(ActionState.Adding);
            if (string.IsNullOrEmpty(txtMaKH.Text)) btnChonKhachHang.Focus();
            else if (string.IsNullOrEmpty(txtTenMay.Text)) btnChonSanPham.Focus();
            else txtLoiDichVu.Focus();
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (currentTab == "all") return;
            selectedSerial = selectedTicketId = selectedServiceId = null;
            SetActionState(ActionState.Adding);
            lblCurrentSelected.Text = "Đang chọn: Thêm mới";
            lblFooterNote.Text = "Đang thêm mới phiếu. Nhập thông tin rồi bấm 'Lưu' hoặc 'Hủy' để quay lại.";
            txtMaPhieu.Text = SinhMaPhieuMoi();
            txtMaNV.Text = UserSession.MaNV ?? "";
            txtTenNV.Text = UserSession.TenNV ?? "";
            txtMaKH.Text = txtTenKH.Text = txtTenMay.Text = txtLoiDichVu.Text = "";
            dgvBaoHanh.ClearSelection();
            UpdateExcelButtonState();
            btnChonKhachHang.Focus();
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (currentTab == "all") return;
            if (string.IsNullOrEmpty(selectedTicketId)) { Function.ThongBaoCanhBao("Vui lòng chọn 1 phiếu trong bảng để chỉnh sửa!"); return; }
            SetActionState(ActionState.Editing);
            lblFooterNote.Text = $"Đang chỉnh sửa phiếu [{selectedTicketId}]. Sửa xong bấm 'Lưu' hoặc 'Hủy' để quay lại.";
            txtLoiDichVu.Focus();
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (currentTab == "all") return;
            if (currentActionState == ActionState.Adding) ThucHienLuuThem();
            else if (currentActionState == ActionState.Editing) ThucHienLuuSua();
            else Function.ThongBaoCanhBao("Vui lòng bấm 'Thêm' hoặc 'Sửa' trước khi Lưu!");
        }
        private void ThucHienLuuThem()
        {
            try
            {
                string id = warrantyService.Create(txtMaKH.Text.Trim(), selectedSerial, currentTab == "warranty" ? "Bảo hành" : "Sửa chữa", txtLoiDichVu.Text.Trim(), currentTab == "repair" ? txtTenMay.Text.Trim() : null);
                LoadTickets();
                SwitchTab("all");
                HandleClearForm();
                SetActionState(ActionState.Normal);
                Function.ThongBaoThanhCong("Đã lưu phiếu " + id + ".");
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }
        private void ThucHienLuuSua()
        {
            try
            {
                warrantyService.UpdateProblem(selectedTicketId, txtLoiDichVu.Text.Trim());
                LoadTickets();
                RenderGrid();
                HandleClearForm();
                SetActionState(ActionState.Normal);
                Function.ThongBaoThanhCong("Đã cập nhật mô tả lỗi.");
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (currentTab == "all" || string.IsNullOrEmpty(selectedTicketId)) return;
            if (Function.XacNhan("Xóa phiếu nhập sai " + selectedTicketId + "?") != DialogResult.Yes) return;
            try
            {
                warrantyService.Delete(selectedTicketId);
                LoadTickets();
                RenderGrid();
                HandleClearForm();
                SetActionState(ActionState.Normal);
                Function.ThongBaoThanhCong("Đã xóa phiếu.");
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }
        private void btnHuy_Click(object sender, EventArgs e) { HandleClearForm(); SetActionState(ActionState.Normal); }
        private void HandleClearForm()
        {
            selectedSerial = selectedTicketId = selectedServiceId = null;
            lblCurrentSelected.Text = "Đang chọn: Chưa chọn";
            txtMaPhieu.Text = SinhMaPhieuMoi();
            txtMaNV.Text = UserSession.MaNV ?? "";
            txtTenNV.Text = UserSession.TenNV ?? "";
            txtMaKH.Text = txtTenKH.Text = txtTenMay.Text = txtLoiDichVu.Text = "";
            lblFooterNote.Text = currentTab == "all" ? "Bấm vào một dòng trong bảng để chọn nhanh"
                : currentTab == "repair" ? "Bấm vào bất kỳ dịch vụ nào để chọn vào phiếu tiếp nhận"
                : "Bấm vào bất kỳ hạng mục nào để chọn vào phiếu tiếp nhận";
            dgvBaoHanh.ClearSelection();
            UpdateExcelButtonState();
        }
        private void btnInPhieu_Click(object sender, EventArgs e)
        {
            string ticketId = !string.IsNullOrEmpty(txtMaPhieu.Text.Trim()) ? txtMaPhieu.Text.Trim() : selectedTicketId;
            if (string.IsNullOrEmpty(ticketId)) { Function.ThongBaoCanhBao("Vui lòng chọn hoặc nhập thông tin phiếu cần in!"); return; }
            using (var printDoc = new PrintDocument())
            using (var previewDialog = new PrintPreviewDialog { Document = printDoc, Width = 900, Height = 700, StartPosition = FormStartPosition.CenterScreen, Text = $"Xem trước phiếu tiếp nhận [{ticketId}]" })
            {
                printDoc.PrintPage += PrintDoc_PrintPage;
                previewDialog.ShowDialog(this);
            }
        }
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int x = 60, y = 50, w = e.MarginBounds.Width + 40;
            using (var titleFont = new Font("Segoe UI", 16, FontStyle.Bold))
            using (var headerFont = new Font("Segoe UI", 11, FontStyle.Bold))
            using (var normalFont = new Font("Segoe UI", 9.5f, FontStyle.Regular))
            using (var boldFont = new Font("Segoe UI", 9.5f, FontStyle.Bold))
            using (var italicFont = new Font("Segoe UI", 9f, FontStyle.Italic))
            using (var pen = new Pen(Color.FromArgb(17, 35, 64), 2))
            using (var lightPen = new Pen(Color.FromArgb(203, 213, 225), 1))
            using (var darkBrush = new SolidBrush(Color.FromArgb(15, 23, 42)))
            using (var blueBrush = new SolidBrush(Color.FromArgb(17, 35, 64)))
            {
                g.DrawString("CỬA HÀNG LAPTOP STORE", headerFont, blueBrush, x, y);
                g.DrawString("Số 123 Đường Công Nghệ, Q. Cầu Giấy, TP. Hà Nội", italicFont, darkBrush, x, y + 22);
                g.DrawString("Hotline: 1900 6868 - Website: laptopstore.com.vn", italicFont, darkBrush, x, y + 38);
                g.DrawString($"Số phiếu: {txtMaPhieu.Text.Trim()}", boldFont, blueBrush, x + w - 210, y);
                g.DrawString($"Ngày lập: {DateTime.Now:dd/MM/yyyy HH:mm}", italicFont, darkBrush, x + w - 210, y + 22);
                y += 65;
                g.DrawLine(pen, x, y, x + w, y);
                y += 20;
                string title = "PHIẾU TIẾP NHẬN SỬA CHỮA & BẢO HÀNH LAPTOP";
                SizeF titleSize = g.MeasureString(title, titleFont);
                g.DrawString(title, titleFont, blueBrush, x + (w - titleSize.Width) / 2, y);
                y += 40;
                g.DrawRectangle(lightPen, x, y, w, 220);
                g.FillRectangle(new SolidBrush(Color.FromArgb(248, 250, 252)), x, y, w, 28);
                g.DrawString("THÔNG TIN TIẾP NHẬN THIẾT BỊ", boldFont, blueBrush, x + 10, y + 6);
                int ly = y + 38, col1 = x + 15, col2 = x + 380;
                g.DrawString($"Mã phiếu: {txtMaPhieu.Text}", boldFont, darkBrush, col1, ly);
                g.DrawString($"Nhân viên tiếp nhận: {txtTenNV.Text} ({txtMaNV.Text})", normalFont, darkBrush, col2, ly);
                ly += 26;
                g.DrawString($"Khách hàng: {txtTenKH.Text} ({txtMaKH.Text})", boldFont, darkBrush, col1, ly);
                g.DrawString($"Thiết bị laptop: {txtTenMay.Text}", boldFont, blueBrush, col2, ly);
                ly += 28;
                g.DrawString("Lỗi / Dịch vụ tiếp nhận:", boldFont, darkBrush, col1, ly);
                ly += 20;
                g.DrawString(txtLoiDichVu.Text, normalFont, darkBrush, new RectangleF(col1, ly, w - 30, 48));
                y += 240;
                g.DrawString("QUY ĐỊNH TIẾP NHẬN:", boldFont, darkBrush, x, y);
                y += 20;
                g.DrawString("1. Quý khách vui lòng mang phiếu này khi đến nhận lại máy.", italicFont, darkBrush, x, y);
                y += 18;
                g.DrawString("2. Cửa hàng không chịu trách nhiệm về dữ liệu cá nhân chưa được sao lưu.", italicFont, darkBrush, x, y);
                y += 45;
                int signCol1 = x + 60, signCol2 = x + w - 240;
                g.DrawString("KHÁCH HÀNG", boldFont, darkBrush, signCol1, y);
                g.DrawString("(Ký, ghi rõ họ tên)", italicFont, darkBrush, signCol1 - 10, y + 18);
                g.DrawString(txtTenKH.Text, boldFont, darkBrush, signCol1, y + 80);
                g.DrawString("NHÂN VIÊN TIẾP NHẬN", boldFont, darkBrush, signCol2, y);
                g.DrawString("(Ký và đóng dấu)", italicFont, darkBrush, signCol2 - 5, y + 18);
                g.DrawString(txtTenNV.Text, boldFont, darkBrush, signCol2, y + 80);
            }
        }
        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            var row = dgvBaoHanh.CurrentRow != null && dgvBaoHanh.CurrentRow.Index >= 0 && dgvBaoHanh.CurrentRow.Selected
                ? dgvBaoHanh.CurrentRow
                : (dgvBaoHanh.SelectedRows.Count > 0 ? dgvBaoHanh.SelectedRows[0] : null);
            if (row == null) { Function.ThongBaoCanhBao("Vui lòng chọn 1 hàng trong bảng để xuất Excel."); return; }
            string id = Convert.ToString(row.Cells[0].Value ?? "Phieu").Trim();
            GridExcelExporter.ExportRow(this, row, lblTableTitle.Text, "SuaChuaBaoHanh_" + id + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls");
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (NavigationManager.IsHosted) NavigationManager.Navigate(NavigationMenu.TrangChu);
            else Close();
        }
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (Function.XacNhan("Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?", "Xác nhận đăng xuất") != DialogResult.Yes) return;
            Function.ClearCurrentUser();
            if (NavigationManager.IsHosted) NavigationManager.Navigate(NavigationMenu.TrangChu);
            else Close();
        }
    }
}

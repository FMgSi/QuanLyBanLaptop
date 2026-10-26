using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmBaoHanhSuaChua : QuanLyBanLaptop.Classes.AppForm
    {
        // ── 1. ĐỐI TƯỢNG DỮ LIỆU ──────────────────────────────────────────
        public class TicketItem
        {
            public string Id { get; set; }
            public string Type { get; set; } // "repair" (Sửa chữa) hoặc "warranty" (Bảo hành)
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

        // ── 2. KHO DỮ LIỆU ĐÚNG CHUẨN MẪU HTML ─────────────────────────────
        private List<TicketItem> allTickets = new List<TicketItem>();
        private List<RepairServiceItem> repairServicesCatalog = new List<RepairServiceItem>();
        private List<WarrantyServiceItem> warrantyServicesCatalog = new List<WarrantyServiceItem>();

        private string currentTab = "all"; // 'all' | 'repair' | 'warranty'
        private string selectedTicketId = null;
        private string selectedServiceId = null;

        public frmBaoHanhSuaChua()
        {
            InitializeComponent();
            NavigationManager.RegisterForm(NavigationMenu.BaoHanhSuaChua, this);
            sidebarControl1.SelectedMenu = NavigationMenu.BaoHanhSuaChua;

            InitCatalogData();
            LoadDataFromDatabaseOrSeed();

            SwitchTab("all");
            UpdateUserInfoDisplay();
            txtMaPhieu.Text = SinhMaPhieuMoi();
            pnlHeaderTop.Visible = !NavigationManager.IsHosted;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.BaoHanhSuaChua;
                UpdateUserInfoDisplay();
                LoadDataFromDatabaseOrSeed();
                RenderGrid();
                if (string.IsNullOrWhiteSpace(txtMaPhieu.Text))
                {
                    txtMaPhieu.Text = SinhMaPhieuMoi();
                }
                pnlHeaderTop.Visible = !NavigationManager.IsHosted;
            }
        }

        // Hiển thị tên người dùng đăng nhập và tự động gán vào form
        private void UpdateUserInfoDisplay()
        {
            string name = !string.IsNullOrEmpty(UserSession.TenNV) ? UserSession.TenNV : "Nguyễn Văn An";
            string id = !string.IsNullOrEmpty(UserSession.MaNV) ? UserSession.MaNV : "NV001";
            string role = !string.IsNullOrEmpty(UserSession.ChucVu) ? UserSession.ChucVu : "Quản trị viên";
            lblHeaderUserInfo.Text = $"👤 {name} ({id}) - {role}";

            // Tự động điền Mã NV và Tên NV từ phiên đăng nhập
            txtMaNV.Text = id;
            txtTenNV.Text = name;
        }

        // ── SINH MÃ PHIẾU TỰ ĐỘNG THEO QUY TẮC MP_ddMMyyyy_xx ──────────────
        private string SinhMaPhieuMoi()
        {
            string dateStr = DateTime.Now.ToString("ddMMyyyy");
            string prefix = $"MP_{dateStr}_";

            int maxSeq = 0;

            // 1. Quét trong database nếu có kết nối
            try
            {
                string sql = "SELECT MaPhieuBH FROM dbo.BAO_HANH_SUA_CHUA WHERE MaPhieuBH LIKE @Pattern";
                DataTable dt = Function.DocBang(sql, new SqlParameter("@Pattern", prefix + "%"));
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        string id = r["MaPhieuBH"]?.ToString() ?? "";
                        if (id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        {
                            string seqStr = id.Substring(prefix.Length);
                            if (int.TryParse(seqStr, out int seq) && seq > maxSeq)
                            {
                                maxSeq = seq;
                            }
                        }
                    }
                }
            }
            catch { }

            // 2. Quét trong danh sách allTickets hiện tại
            if (allTickets != null)
            {
                foreach (var t in allTickets)
                {
                    if (t != null && !string.IsNullOrEmpty(t.Id) && t.Id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        string seqStr = t.Id.Substring(prefix.Length);
                        if (int.TryParse(seqStr, out int seq) && seq > maxSeq)
                        {
                            maxSeq = seq;
                        }
                    }
                }
            }

            return $"{prefix}{(maxSeq + 1):D2}";
        }

        // Sự kiện bấm nút [🔍 Khách hàng] mở bảng chọn khách hàng (giống nút Hàng bên frmHoaDon)
        private void btnChonKhachHang_Click(object sender, EventArgs e)
        {
            using (var frm = new frmTimKiemKhachHang())
            {
                if (frm.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(frm.SelectedMaKH))
                {
                    txtMaKH.Text = frm.SelectedMaKH;
                    txtTenKH.Text = frm.SelectedTenKH;
                    lblFooterNote.Text = $"Đã chọn khách hàng: {frm.SelectedTenKH} ({frm.SelectedMaKH})";
                    txtTenMay.Focus();
                }
            }
        }

        // Sự kiện bấm nút [🔍 Sản phẩm] mở bảng tìm kiếm sản phẩm (giống nút Hàng bên frmHoaDon)
        private void btnChonSanPham_Click(object sender, EventArgs e)
        {
            using (var frm = new QuanLyBanLaptop.HoaDon.frmTimKiemLaptop())
            {
                if (frm.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(frm.SelectedTenLaptop))
                {
                    txtTenMay.Text = frm.SelectedTenLaptop;
                    lblFooterNote.Text = $"Đã chọn sản phẩm: {frm.SelectedTenLaptop}";
                    txtLoiDichVu.Focus();
                }
            }
        }

        // Khởi tạo danh mục dịch vụ sửa chữa và bảo hành mẫu
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

        // Tải dữ liệu từ CSDL hoặc nạp danh sách chuẩn từ HTML
        private void LoadDataFromDatabaseOrSeed()
        {
            allTickets.Clear();
            bool dbSuccess = false;

            try
            {
                // Thử tải từ SQL Server nếu có kết nối
                string sql = @"
                    SELECT TOP 100 
                        b.MaPhieuBH, b.LoaiDichVu, b.MaNV, nv.TenNV, 
                        h.MaKH, k.TenKH, l.TenLaptop, b.MoTaLoi
                    FROM dbo.BAO_HANH_SUA_CHUA b
                    LEFT JOIN dbo.LAPTOP_SERIAL s ON s.SerialNo = b.SerialNo
                    LEFT JOIN dbo.LAPTOP l ON l.MaLaptop = s.MaLaptop
                    LEFT JOIN dbo.HOA_DON h ON h.MaHD = s.MaHD
                    LEFT JOIN dbo.KHACH_HANG k ON k.MaKH = h.MaKH
                    LEFT JOIN dbo.NHAN_VIEN nv ON nv.MaNV = b.MaNV
                    ORDER BY b.NgayNhan DESC";

                DataTable dt = Function.DocBang(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        string id = r["MaPhieuBH"]?.ToString() ?? "";
                        string ldv = r["LoaiDichVu"]?.ToString() ?? "Sửa chữa";
                        string type = ldv == "Bảo hành" ? "warranty" : "repair";

                        allTickets.Add(new TicketItem
                        {
                            Id = id,
                            Type = type,
                            EmpId = r["MaNV"]?.ToString() ?? "NV001",
                            EmpName = r["TenNV"]?.ToString() ?? "Nguyễn Văn An",
                            CustId = r["MaKH"]?.ToString() ?? "KH001",
                            CustName = r["TenKH"]?.ToString() ?? "Khách hàng",
                            Device = r["TenLaptop"]?.ToString() ?? "Laptop",
                            Problem = r["MoTaLoi"]?.ToString() ?? "Kiểm tra kỹ thuật"
                        });
                    }
                    dbSuccess = true;
                }
            }
            catch
            {
                dbSuccess = false;
            }

            // Nếu DB chưa có dữ liệu hoặc không kết nối, dùng danh sách mẫu chuẩn quy tắc MP_ddMMyyyy_xx
            if (!dbSuccess || allTickets.Count == 0)
            {
                string today = DateTime.Now.ToString("ddMMyyyy");
                allTickets = new List<TicketItem>
                {
                    new TicketItem { Id = $"MP_{today}_01", Type = "repair", EmpId = "NV001", EmpName = "Nguyễn Văn An", CustId = "KH001", CustName = "Trần Anh Tuấn", Device = "Acer Nitro 5 AN515", Problem = "Quạt kêu to, nhiệt độ tăng cao, tự sập nguồn" },
                    new TicketItem { Id = $"MP_{today}_02", Type = "repair", EmpId = "NV002", EmpName = "Trần Đình Hùng", CustId = "KH002", CustName = "Hoàng Minh Đức", Device = "Dell XPS 13 9315", Problem = "Màn hình bị sọc dọc xanh, chập chờn cáp tín hiệu" },
                    new TicketItem { Id = $"MP_{today}_03", Type = "warranty", EmpId = "NV005", EmpName = "Vũ Đức Thịnh", CustId = "KH006", CustName = "Đinh Công Trình", Device = "Asus ROG Strix G15", Problem = "Loa bên phải bị rè khi mở âm lượng trên 60%" },
                    new TicketItem { Id = $"MP_{today}_04", Type = "repair", EmpId = "NV003", EmpName = "Lê Văn Toàn", CustId = "KH003", CustName = "Nguyễn Thảo My", Device = "Asus ZenBook 14 OLED", Problem = "Liệt cụm phím số và phím cách Spacebar" },
                    new TicketItem { Id = $"MP_{today}_05", Type = "warranty", EmpId = "NV002", EmpName = "Trần Đình Hùng", CustId = "KH007", CustName = "Bùi Thị Mai", Device = "Dell Inspiron 15 3520", Problem = "Pin chai vượt mức 30% trong thời hạn bảo hành hãng" },
                    new TicketItem { Id = $"MP_{today}_06", Type = "repair", EmpId = "NV003", EmpName = "Lê Văn Toàn", CustId = "KH004", CustName = "Lê Quang Hải", Device = "Lenovo Legion 5", Problem = "Chập chân cắm Type-C, không nhận dòng sạc" },
                    new TicketItem { Id = $"MP_{today}_07", Type = "warranty", EmpId = "NV005", EmpName = "Vũ Đức Thịnh", CustId = "KH008", CustName = "Ngô Văn Long", Device = "Lenovo IdeaPad Slim 3", Problem = "Trackpad chập chờn lúc nhận cảm ứng lúc không" }
                };
            }
        }

        // ── 3. CHUYỂN TAB (TẤT CẢ - DỊCH VỤ SỬA CHỮA - QUẢN LÝ BẢO HÀNH) ─────
        private void SwitchTab(string tab)
        {
            currentTab = tab;
            selectedTicketId = null;
            selectedServiceId = null;
            lblCurrentSelected.Text = "Đang chọn: Chưa chọn";

            Color activeBg = Color.FromArgb(17, 35, 64);      // #112340
            Color inactiveBg = Color.White;
            Color activeFg = Color.White;
            Color inactiveFg = Color.FromArgb(51, 65, 85);

            btnTabAll.BackColor = (tab == "all") ? activeBg : inactiveBg;
            btnTabAll.ForeColor = (tab == "all") ? activeFg : inactiveFg;
            btnTabAll.Font = new Font("Segoe UI", 9F, (tab == "all") ? FontStyle.Bold : FontStyle.Regular);

            btnTabRepair.BackColor = (tab == "repair") ? activeBg : inactiveBg;
            btnTabRepair.ForeColor = (tab == "repair") ? activeFg : inactiveFg;
            btnTabRepair.Font = new Font("Segoe UI", 9F, (tab == "repair") ? FontStyle.Bold : FontStyle.Regular);

            btnTabWarranty.BackColor = (tab == "warranty") ? activeBg : inactiveBg;
            btnTabWarranty.ForeColor = (tab == "warranty") ? activeFg : inactiveFg;
            btnTabWarranty.Font = new Font("Segoe UI", 9F, (tab == "warranty") ? FontStyle.Bold : FontStyle.Regular);

            if (tab == "all")
            {
                lblFormHeader.Text = "TỔNG HỢP THÔNG TIN PHIẾU TIẾP NHẬN";
                lblTableTitle.Text = "DANH SÁCH TẤT CẢ PHIẾU DỊCH VỤ SỬA CHỮA & BẢO HÀNH";
                lblTableHint.Text = "Bấm vào dòng để chọn hoặc chỉnh sửa thông tin";
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

        // ── 4. RENDER BẢNG DỮ LIỆU ĐA NĂNG THEO TAB ─────────────────────────
        private void RenderGrid()
        {
            dgvBaoHanh.Columns.Clear();
            dgvBaoHanh.AutoGenerateColumns = false;

            if (currentTab == "all")
            {
                // Tab Tất cả: Danh sách tất cả các phiếu
                lblRecordCount.Text = $"Tổng số: {allTickets.Count} phiếu";

                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "Mã phiếu", Width = 95 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TypeDisplay", HeaderText = "Phân loại", Width = 110 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EmpId", HeaderText = "Mã NV", Width = 85 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EmpName", HeaderText = "Tên nhân viên", Width = 150 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CustId", HeaderText = "Mã KH", Width = 85 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CustName", HeaderText = "Tên khách hàng", Width = 150 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Device", HeaderText = "Tên máy", Width = 170 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Problem", HeaderText = "Lỗi / Dịch vụ tiếp nhận", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

                var viewList = allTickets.Select(t => new
                {
                    t.Id,
                    TypeDisplay = (t.Type == "repair" ? "Sửa chữa" : "Bảo hành"),
                    t.EmpId,
                    t.EmpName,
                    t.CustId,
                    t.CustName,
                    t.Device,
                    t.Problem
                }).ToList();

                dgvBaoHanh.DataSource = viewList;

                // Tô màu phân loại
                for (int i = 0; i < dgvBaoHanh.Rows.Count; i++)
                {
                    string pl = dgvBaoHanh.Rows[i].Cells[1].Value?.ToString() ?? "";
                    if (pl == "Sửa chữa")
                    {
                        dgvBaoHanh.Rows[i].Cells[1].Style.ForeColor = Color.FromArgb(29, 78, 216);
                        dgvBaoHanh.Rows[i].Cells[1].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    }
                    else
                    {
                        dgvBaoHanh.Rows[i].Cells[1].Style.ForeColor = Color.FromArgb(126, 34, 206);
                        dgvBaoHanh.Rows[i].Cells[1].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    }
                }
            }
            else if (currentTab == "repair")
            {
                // Tab Dịch vụ Sửa chữa: Bảng danh mục các gói sửa máy
                lblRecordCount.Text = $"Tổng số: {repairServicesCatalog.Count} dịch vụ có sẵn";

                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "Mã DV", Width = 95 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Tên dịch vụ sửa máy", Width = 280 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Symptom", HeaderText = "Triệu chứng lỗi thường gặp", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Price", HeaderText = "Giá tham khảo", Width = 140 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Time", HeaderText = "Thời gian", Width = 130 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Warranty", HeaderText = "Bảo hành", Width = 120 });

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
                // Tab Quản lý Bảo hành: Bảng các hạng mục bảo hành hãng
                lblRecordCount.Text = $"Tổng số: {warrantyServicesCatalog.Count} hạng mục bảo hành";

                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "Mã BH", Width = 95 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Hạng mục bảo hành", Width = 300 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Condition", HeaderText = "Điều kiện tiếp nhận bảo hành", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Fee", HeaderText = "Chi phí", Width = 150 });
                dgvBaoHanh.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Time", HeaderText = "Thời gian xử lý", Width = 150 });

                dgvBaoHanh.DataSource = warrantyServicesCatalog.ToList();

                for (int i = 0; i < dgvBaoHanh.Rows.Count; i++)
                {
                    dgvBaoHanh.Rows[i].Cells[0].Style.ForeColor = Color.FromArgb(126, 34, 206);
                    dgvBaoHanh.Rows[i].Cells[0].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    dgvBaoHanh.Rows[i].Cells[3].Style.ForeColor = Color.FromArgb(21, 128, 61);
                    dgvBaoHanh.Rows[i].Cells[3].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }

            // Khôi phục chọn dòng trực quan theo Tab giống như HTML
            bool hasSelection = false;
            if (!string.IsNullOrEmpty(selectedTicketId) && currentTab == "all")
            {
                for (int i = 0; i < dgvBaoHanh.Rows.Count; i++)
                {
                    if (dgvBaoHanh.Rows[i].Cells[0].Value?.ToString() == selectedTicketId)
                    {
                        dgvBaoHanh.Rows[i].Selected = true;
                        try { dgvBaoHanh.FirstDisplayedScrollingRowIndex = i; } catch { }
                        hasSelection = true;
                        break;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(selectedServiceId) && (currentTab == "repair" || currentTab == "warranty"))
            {
                for (int i = 0; i < dgvBaoHanh.Rows.Count; i++)
                {
                    if (dgvBaoHanh.Rows[i].Cells[0].Value?.ToString() == selectedServiceId)
                    {
                        dgvBaoHanh.Rows[i].Selected = true;
                        try { dgvBaoHanh.FirstDisplayedScrollingRowIndex = i; } catch { }
                        hasSelection = true;
                        break;
                    }
                }
            }

            if (!hasSelection)
            {
                dgvBaoHanh.ClearSelection();
            }
        }

        // ── 5. XỬ LÝ SỰ KIỆN CLICK CHỌN DÒNG TRÊN BẢNG ───────────────────────
        private void dgvBaoHanh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvBaoHanh.Rows.Count) return;

            if (currentTab == "all")
            {
                string id = dgvBaoHanh.Rows[e.RowIndex].Cells[0].Value?.ToString();
                SelectTicketRow(id);
            }
            else if (currentTab == "repair")
            {
                string id = dgvBaoHanh.Rows[e.RowIndex].Cells[0].Value?.ToString();
                SelectServiceRow(id, "repair");
            }
            else if (currentTab == "warranty")
            {
                string id = dgvBaoHanh.Rows[e.RowIndex].Cells[0].Value?.ToString();
                SelectServiceRow(id, "warranty");
            }
        }

        private void dgvBaoHanh_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvBaoHanh.Rows.Count) return;

            if (dgvBaoHanh.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                string id = dgvBaoHanh.Rows[e.RowIndex].Cells[0].Value?.ToString();
                if (currentTab == "repair")
                {
                    SelectServiceRow(id, "repair");
                }
                else if (currentTab == "warranty")
                {
                    SelectServiceRow(id, "warranty");
                }
            }
        }

        // Chọn một phiếu từ Tab Tất cả nạp lên đúng 7 trường
        private void SelectTicketRow(string id)
        {
            var item = allTickets.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                selectedTicketId = item.Id;
                selectedServiceId = null;
                lblCurrentSelected.Text = $"Đang chọn: Phiếu {item.Id}";
                lblFooterNote.Text = $"Đã nạp thông tin phiếu [{item.Id}] lên form chỉnh sửa";

                txtMaPhieu.Text = item.Id;
                txtMaNV.Text = item.EmpId;
                txtTenNV.Text = item.EmpName;
                txtMaKH.Text = item.CustId;
                txtTenKH.Text = item.CustName;
                txtTenMay.Text = item.Device;
                txtLoiDichVu.Text = item.Problem;
            }
        }

        // Chọn dịch vụ từ Tab Sửa chữa hoặc Bảo hành nạp vào form
        private void SelectServiceRow(string id, string type)
        {
            selectedServiceId = id;
            selectedTicketId = null;
            lblCurrentSelected.Text = $"Đang chọn: Dịch vụ {id}";

            // Điền thông tin nhân viên đăng nhập nếu chưa có
            if (string.IsNullOrWhiteSpace(txtMaNV.Text))
            {
                txtMaNV.Text = !string.IsNullOrEmpty(UserSession.MaNV) ? UserSession.MaNV : "NV001";
                txtTenNV.Text = !string.IsNullOrEmpty(UserSession.TenNV) ? UserSession.TenNV : "Nguyễn Văn An";
            }

            // Giữ hoặc tự sinh mã phiếu theo quy tắc MP_ddMMyyyy_xx
            if (string.IsNullOrWhiteSpace(txtMaPhieu.Text) || !txtMaPhieu.Text.StartsWith("MP_"))
            {
                txtMaPhieu.Text = SinhMaPhieuMoi();
            }

            if (type == "repair")
            {
                var s = repairServicesCatalog.FirstOrDefault(x => x.Id == id);
                if (s != null)
                {
                    txtLoiDichVu.Text = $"{s.Name} - [Lỗi: {s.Symptom}]";
                    lblFooterNote.Text = $"Đã chọn: {s.Name}. Bấm [🔍 Khách hàng] để chọn khách rồi bấm 'Thêm'!";
                    if (string.IsNullOrEmpty(txtMaKH.Text)) btnChonKhachHang.Focus();
                    else txtTenMay.Focus();
                }
            }
            else
            {
                var w = warrantyServicesCatalog.FirstOrDefault(x => x.Id == id);
                if (w != null)
                {
                    txtLoiDichVu.Text = $"{w.Name} - [ĐK: {w.Condition}]";
                    lblFooterNote.Text = $"Đã chọn: {w.Name}. Bấm [🔍 Khách hàng] để chọn khách rồi bấm 'Thêm'!";
                    if (string.IsNullOrEmpty(txtMaKH.Text)) btnChonKhachHang.Focus();
                    else txtTenMay.Focus();
                }
            }
        }

        // ── 6. THAO TÁC NÚT (THÊM, SỬA, XÓA, LƯU, LÀM MỚI, IN PHIẾU, THOÁT) ─
        private void btnThem_Click(object sender, EventArgs e)
        {
            string custName = txtTenKH.Text.Trim();
            string device = txtTenMay.Text.Trim();
            string problem = txtLoiDichVu.Text.Trim();

            if (string.IsNullOrEmpty(txtMaKH.Text.Trim()) && string.IsNullOrEmpty(custName))
            {
                Function.ThongBaoCanhBao("Vui lòng bấm nút [🔍 Khách hàng] để chọn khách hàng tiếp nhận!");
                btnChonKhachHang.Focus();
                return;
            }

            if (string.IsNullOrEmpty(custName) || string.IsNullOrEmpty(device))
            {
                Function.ThongBaoCanhBao("Vui lòng nhập tối thiểu Tên khách hàng và Tên máy!");
                if (string.IsNullOrEmpty(custName)) txtTenKH.Focus();
                else txtTenMay.Focus();
                return;
            }

            // Xác định loại phiếu dựa vào tab hiện tại hoặc dịch vụ
            string type = (currentTab == "warranty") ? "warranty" : "repair";

            // Sinh mã phiếu theo quy tắc MP_ddMMyyyy_xx nếu chưa có
            string ticketId = txtMaPhieu.Text.Trim();
            if (string.IsNullOrEmpty(ticketId) || !ticketId.StartsWith("MP_"))
            {
                ticketId = SinhMaPhieuMoi();
            }

            // Thông tin nhân viên luôn lấy từ phiên đăng nhập
            string empId = !string.IsNullOrEmpty(UserSession.MaNV) ? UserSession.MaNV : (!string.IsNullOrWhiteSpace(txtMaNV.Text) ? txtMaNV.Text.Trim() : "NV001");
            string empName = !string.IsNullOrEmpty(UserSession.TenNV) ? UserSession.TenNV : (!string.IsNullOrWhiteSpace(txtTenNV.Text) ? txtTenNV.Text.Trim() : "Nguyễn Văn An");
            string custId = string.IsNullOrWhiteSpace(txtMaKH.Text) ? $"KH{(allTickets.Count + 1):D3}" : txtMaKH.Text.Trim();

            var newTicket = new TicketItem
            {
                Id = ticketId,
                Type = type,
                EmpId = empId,
                EmpName = empName,
                CustId = custId,
                CustName = custName,
                Device = device,
                Problem = string.IsNullOrEmpty(problem) ? "Tiếp nhận kiểm tra kỹ thuật" : problem
            };

            // Tránh trùng mã phiếu
            allTickets.RemoveAll(t => t.Id == ticketId);
            allTickets.Insert(0, newTicket);

            // Cập nhật CSDL nếu có kết nối
            try
            {
                string sql = @"
                    IF NOT EXISTS (SELECT 1 FROM dbo.BAO_HANH_SUA_CHUA WHERE MaPhieuBH = @MaPhieu)
                    BEGIN
                        INSERT INTO dbo.BAO_HANH_SUA_CHUA (MaPhieuBH, SerialNo, MaNV, LoaiDichVu, NgayNhan, MoTaLoi, ChiPhi, TinhTrangBH)
                        VALUES (@MaPhieu, 'LT001-PN002-002', @MaNV, @LoaiDV, GETDATE(), @MoTaLoi, 0, N'Tiếp nhận')
                    END";
                Function.CapNhat(sql,
                    new SqlParameter("@MaPhieu", ticketId),
                    new SqlParameter("@MaNV", empId),
                    new SqlParameter("@LoaiDV", type == "warranty" ? "Bảo hành" : "Sửa chữa"),
                    new SqlParameter("@MoTaLoi", newTicket.Problem));
            }
            catch { }

            selectedTicketId = ticketId;
            lblCurrentSelected.Text = $"Đang chọn: Phiếu {ticketId}";

            SwitchTab("all");
            Function.ThongBaoThanhCong($"Đã thêm mới thành công phiếu {ticketId} ({(type == "repair" ? "Sửa chữa" : "Bảo hành")})!");

            // Chuẩn bị sẵn mã phiếu tự gen mới tiếp theo cho lượt tiếp nhận kế tiếp
            txtMaPhieu.Text = SinhMaPhieuMoi();
            txtMaKH.Text = "";
            txtTenKH.Text = "";
            txtTenMay.Text = "";
            txtLoiDichVu.Text = "";
            txtMaNV.Text = empId;
            txtTenNV.Text = empName;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedTicketId))
            {
                Function.ThongBaoCanhBao("Vui lòng chọn 1 phiếu trong tab 'Tất cả' để chỉnh sửa!");
                return;
            }

            var ticket = allTickets.FirstOrDefault(t => t.Id == selectedTicketId);
            if (ticket != null)
            {
                ticket.EmpId = string.IsNullOrWhiteSpace(txtMaNV.Text) ? ticket.EmpId : txtMaNV.Text.Trim();
                ticket.EmpName = string.IsNullOrWhiteSpace(txtTenNV.Text) ? ticket.EmpName : txtTenNV.Text.Trim();
                ticket.CustId = string.IsNullOrWhiteSpace(txtMaKH.Text) ? ticket.CustId : txtMaKH.Text.Trim();
                ticket.CustName = string.IsNullOrWhiteSpace(txtTenKH.Text) ? ticket.CustName : txtTenKH.Text.Trim();
                ticket.Device = string.IsNullOrWhiteSpace(txtTenMay.Text) ? ticket.Device : txtTenMay.Text.Trim();
                ticket.Problem = string.IsNullOrWhiteSpace(txtLoiDichVu.Text) ? ticket.Problem : txtLoiDichVu.Text.Trim();

                // Cập nhật CSDL
                try
                {
                    string sql = @"
                        UPDATE dbo.BAO_HANH_SUA_CHUA 
                        SET MaNV = @MaNV, MoTaLoi = @MoTaLoi
                        WHERE MaPhieuBH = @MaPhieu";
                    Function.CapNhat(sql,
                        new SqlParameter("@MaPhieu", ticket.Id),
                        new SqlParameter("@MaNV", ticket.EmpId),
                        new SqlParameter("@MoTaLoi", ticket.Problem));
                }
                catch { }

                RenderGrid();
                Function.ThongBaoThanhCong($"Đã lưu thay đổi cho phiếu {ticket.Id} thành công!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedTicketId))
            {
                Function.ThongBaoCanhBao("Vui lòng chọn 1 phiếu trong tab 'Tất cả' cần xóa!");
                return;
            }

            if (Function.XacNhan($"Bạn có chắc chắn muốn xóa phiếu '{selectedTicketId}'?", "Xác nhận xóa") == DialogResult.Yes)
            {
                string idToDelete = selectedTicketId;
                allTickets.RemoveAll(t => t.Id == idToDelete);

                try
                {
                    Function.CapNhat("DELETE FROM dbo.BAO_HANH_SUA_CHUA WHERE MaPhieuBH = @MaPhieu",
                        new SqlParameter("@MaPhieu", idToDelete));
                }
                catch { }

                HandleClearForm();
                RenderGrid();
                Function.ThongBaoThanhCong($"Đã xóa thành công phiếu {idToDelete}!");
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedTicketId))
            {
                btnSua_Click(sender, e);
            }
            else
            {
                btnThem_Click(sender, e);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            HandleClearForm();
        }

        private void HandleClearForm()
        {
            selectedTicketId = null;
            selectedServiceId = null;
            lblCurrentSelected.Text = "Đang chọn: Chưa chọn";

            // Tự động sinh mã phiếu mới theo quy tắc MP_ddMMyyyy_xx
            txtMaPhieu.Text = SinhMaPhieuMoi();

            // Luôn giữ mã và tên nhân viên đăng nhập
            txtMaNV.Text = !string.IsNullOrEmpty(UserSession.MaNV) ? UserSession.MaNV : "NV001";
            txtTenNV.Text = !string.IsNullOrEmpty(UserSession.TenNV) ? UserSession.TenNV : "Nguyễn Văn An";

            txtMaKH.Text = "";
            txtTenKH.Text = "";
            txtTenMay.Text = "";
            txtLoiDichVu.Text = "";

            if (currentTab == "all")
                lblFooterNote.Text = "Bấm vào một dòng trong bảng để chọn nhanh";
            else if (currentTab == "repair")
                lblFooterNote.Text = "Bấm vào bất kỳ dịch vụ nào để chọn vào phiếu tiếp nhận";
            else
                lblFooterNote.Text = "Bấm vào bất kỳ hạng mục nào để chọn vào phiếu tiếp nhận";

            dgvBaoHanh.ClearSelection();
        }

        // In phiếu tiếp nhận chuẩn A4
        private void btnInPhieu_Click(object sender, EventArgs e)
        {
            string ticketId = txtMaPhieu.Text.Trim();
            if (string.IsNullOrEmpty(ticketId) && !string.IsNullOrEmpty(selectedTicketId))
            {
                ticketId = selectedTicketId;
            }

            if (string.IsNullOrEmpty(ticketId))
            {
                Function.ThongBaoCanhBao("Vui lòng chọn hoặc nhập thông tin phiếu cần in!");
                return;
            }

            using (var printDoc = new PrintDocument())
            using (var previewDialog = new PrintPreviewDialog())
            {
                printDoc.PrintPage += PrintDoc_PrintPage;
                previewDialog.Document = printDoc;
                previewDialog.Width = 900;
                previewDialog.Height = 700;
                previewDialog.StartPosition = FormStartPosition.CenterScreen;
                previewDialog.Text = $"Xem trước phiếu tiếp nhận [{ticketId}]";
                previewDialog.ShowDialog(this);
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int x = 60;
            int y = 50;
            int w = e.MarginBounds.Width + 40;

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

                string maPhieu = txtMaPhieu.Text.Trim();
                string ngayIn = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                g.DrawString($"Số phiếu: {maPhieu}", boldFont, blueBrush, x + w - 210, y);
                g.DrawString($"Ngày lập: {ngayIn}", italicFont, darkBrush, x + w - 210, y + 22);

                y += 65;
                g.DrawLine(pen, x, y, x + w, y);
                y += 20;

                string title = "PHIẾU TIẾP NHẬN SỬA CHỮA & BẢO HÀNH LAPTOP";
                SizeF titleSize = g.MeasureString(title, titleFont);
                g.DrawString(title, titleFont, blueBrush, x + (w - titleSize.Width) / 2, y);

                y += 40;

                // Khung thông tin
                g.DrawRectangle(lightPen, x, y, w, 220);
                g.FillRectangle(new SolidBrush(Color.FromArgb(248, 250, 252)), x, y, w, 28);
                g.DrawString("THÔNG TIN TIẾP NHẬN THIẾT BỊ", boldFont, blueBrush, x + 10, y + 6);

                int ly = y + 38;
                int col1 = x + 15;
                int col2 = x + 380;

                g.DrawString($"Mã phiếu: {txtMaPhieu.Text}", boldFont, darkBrush, col1, ly);
                g.DrawString($"Nhân viên tiếp nhận: {txtTenNV.Text} ({txtMaNV.Text})", normalFont, darkBrush, col2, ly);

                ly += 26;
                g.DrawString($"Khách hàng: {txtTenKH.Text} ({txtMaKH.Text})", boldFont, darkBrush, col1, ly);
                g.DrawString($"Thiết bị laptop: {txtTenMay.Text}", boldFont, blueBrush, col2, ly);

                ly += 28;
                g.DrawString($"Lỗi / Dịch vụ tiếp nhận:", boldFont, darkBrush, col1, ly);
                ly += 20;
                g.DrawString(txtLoiDichVu.Text, normalFont, darkBrush, new RectangleF(col1, ly, w - 30, 48));

                y += 240;

                g.DrawString("QUY ĐỊNH TIẾP NHẬN:", boldFont, darkBrush, x, y);
                y += 20;
                g.DrawString("1. Quý khách vui lòng mang phiếu này khi đến nhận lại máy.", italicFont, darkBrush, x, y);
                y += 18;
                g.DrawString("2. Cửa hàng không chịu trách nhiệm về dữ liệu cá nhân chưa được sao lưu.", italicFont, darkBrush, x, y);

                y += 45;
                int signCol1 = x + 60;
                int signCol2 = x + w - 240;

                g.DrawString("KHÁCH HÀNG", boldFont, darkBrush, signCol1, y);
                g.DrawString("(Ký, ghi rõ họ tên)", italicFont, darkBrush, signCol1 - 10, y + 18);
                g.DrawString(txtTenKH.Text, boldFont, darkBrush, signCol1, y + 80);

                g.DrawString("NHÂN VIÊN TIẾP NHẬN", boldFont, darkBrush, signCol2, y);
                g.DrawString("(Ký và đóng dấu)", italicFont, darkBrush, signCol2 - 5, y + 18);
                g.DrawString(txtTenNV.Text, boldFont, darkBrush, signCol2, y + 80);
            }
        }

        // Xuất Excel / CSV
        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (allTickets == null || allTickets.Count == 0)
            {
                Function.ThongBaoCanhBao("Không có dữ liệu để xuất file!");
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                sfd.FileName = $"DANH_SACH_SUA_CHUA_BAO_HANH_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        var sb = new StringBuilder();
                        sb.Append('\uFEFF'); // UTF-8 BOM
                        sb.AppendLine("Mã Phiếu,Phân Loại,Mã NV,Tên Nhân Viên,Mã KH,Tên Khách Hàng,Tên Máy,Lỗi / Dịch Vụ Tiếp Nhận");

                        foreach (var t in allTickets)
                        {
                            string pl = t.Type == "repair" ? "Sửa chữa" : "Bảo hành";
                            string tl = "\"" + (t.Device ?? "").Replace("\"", "\"\"") + "\"";
                            string kh = "\"" + (t.CustName ?? "").Replace("\"", "\"\"") + "\"";
                            string nv = "\"" + (t.EmpName ?? "").Replace("\"", "\"\"") + "\"";
                            string pr = "\"" + (t.Problem ?? "").Replace("\"", "\"\"") + "\"";
                            sb.AppendLine($"{t.Id},{pl},{t.EmpId},{nv},{t.CustId},{kh},{tl},{pr}");
                        }

                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        Function.ThongBaoThanhCong($"Đã xuất thành công {allTickets.Count} bản ghi ra file:\n{sfd.FileName}");
                    }
                    catch (Exception ex)
                    {
                        Function.ThongBaoLoi("Lỗi khi xuất file: " + ex.Message);
                    }
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (NavigationManager.IsHosted)
            {
                NavigationManager.Navigate(NavigationMenu.TrangChu);
            }
            else
            {
                this.Close();
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (Function.XacNhan("Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?", "Xác nhận đăng xuất") == DialogResult.Yes)
            {
                Function.ClearCurrentUser();
                if (NavigationManager.IsHosted)
                {
                    NavigationManager.Navigate(NavigationMenu.TrangChu);
                }
                else
                {
                    this.Close();
                }
            }
        }
    }
}

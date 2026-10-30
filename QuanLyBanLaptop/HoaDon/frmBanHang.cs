using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmBanHang : AppForm
    {
        private DataTable _dtGioHang, _dtLaptop;
        private bool _isSaved = false;
        private enum FormMode { View, Add, Edit, Delete }
        private FormMode _mode = FormMode.View;
        private static readonly Color ColorDisabled = Color.FromArgb(148, 163, 184);
        private static readonly Color ColorGreen = Color.FromArgb(34, 197, 94);
        private static readonly Color ColorBlue = Color.FromArgb(37, 99, 235);
        private static readonly Color ColorRed = Color.FromArgb(239, 68, 68);
        private static readonly Color ColorIndigo = Color.FromArgb(99, 102, 241);
        private static readonly Color ColorGray = Color.FromArgb(100, 116, 139);
        public frmBanHang()
        {
            InitializeComponent();
            NavigationManager.RegisterForm(NavigationMenu.BanHang, this);
            sidebarControl1.SelectedMenu = NavigationMenu.BanHang;
            KhoiTaoGioHang();
            DangKySuKien();
            ClearForm();
            SetFormMode(FormMode.View);
        }
        private void StyleButton(Button btn, Color back, Color fore) => ButtonTheme.Apply(btn);
        private void SetFormMode(FormMode mode)
        {
            _mode = mode;
            bool isView = mode == FormMode.View;
            bool isEditing = mode == FormMode.Add || mode == FormMode.Edit;
            bool hasInvoice = !string.IsNullOrEmpty(txtMaHD.Text) && _isSaved;
            dtpNgayBan.Enabled = cboMaKH.Enabled = btnChonKH.Enabled = cboMaHang.Enabled = btnTimKiemSP.Enabled = isEditing;
            txtSDT.ReadOnly = txtTenKhach.ReadOnly = txtDiaChi.ReadOnly = txtSoLuong.ReadOnly = txtGiamGia.ReadOnly = !isEditing;
            cboMaHDTim.Enabled = btnTimKiem.Enabled = isView;
            btnThemHD.Visible = btnLuuHD.Visible = btnHuy.Visible = btnHuyHD.Visible = btnInHD.Visible = btnDong.Visible = true;
            btnSuaHD.Visible = btnXoaHD.Visible = false;
            btnThemHD.Enabled = isView;
            btnLuuHD.Enabled = btnHuy.Enabled = !isView;
            btnHuyHD.Enabled = isView && hasInvoice;
            btnInHD.Enabled = isView && hasInvoice && _dtGioHang.Rows.Count > 0;
            btnDong.Enabled = true;
            StyleButton(btnThemHD, btnThemHD.Enabled ? ColorGreen : ColorDisabled, Color.White);
            StyleButton(btnLuuHD, btnLuuHD.Enabled ? ColorBlue : ColorDisabled, Color.White);
            StyleButton(btnHuy, btnHuy.Enabled ? ColorRed : ColorDisabled, Color.White);
            StyleButton(btnHuyHD, btnHuyHD.Enabled ? ColorRed : ColorDisabled, Color.White);
            StyleButton(btnInHD, btnInHD.Enabled ? ColorIndigo : ColorDisabled, Color.White);
            StyleButton(btnDong, ColorGray, Color.White);
        }
        private void XuLyNhapSoDienThoai()
        {
            if (_mode != FormMode.Add && _mode != FormMode.Edit) return;
            string sdt = txtSDT.Text.Trim();
            if (string.IsNullOrEmpty(sdt)) return;
            try
            {
                DataTable dt = DataProcesser.ExecuteReader("SELECT MaKH, TenKH, HangThanhVien, DiaChi, Sdt FROM KHACH_HANG WHERE Sdt = @Sdt", new[] { new SqlParameter("@Sdt", sdt) });
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    string maKH = r["MaKH"].ToString();
                    cboMaKH.SelectedValue = maKH;
                    if (cboMaKH.SelectedIndex == -1) cboMaKH.Text = maKH;
                    txtTenKhach.Text = r["TenKH"] != DBNull.Value ? r["TenKH"].ToString() : "";
                    txtDiaChi.Text = r["DiaChi"] != DBNull.Value ? r["DiaChi"].ToString() : "";
                    string hang = r.Table.Columns.Contains("HangThanhVien") && r["HangThanhVien"] != DBNull.Value ? r["HangThanhVien"].ToString() : "Đồng";
                    txtHangKhach.Text = hang;
                    decimal phanTramGiam = LayPhanTramGiamGiaTheoHang(hang);
                    txtGiamGia.Text = phanTramGiam.ToString("0.##");
                    TinhThanhTien();
                    CapNhatGiamGiaChoGioHang(phanTramGiam);
                    txtTenKhach.ReadOnly = txtDiaChi.ReadOnly = true;
                }
                else
                {
                    cboMaKH.SelectedIndex = -1;
                    cboMaKH.Text = Function.SinhMaTuDong("KHACH_HANG", "MaKH", "KH", 2);
                    txtTenKhach.Clear();
                    txtDiaChi.Clear();
                    txtHangKhach.Text = "Đồng";
                    txtGiamGia.Text = "0";
                    TinhThanhTien();
                    txtTenKhach.ReadOnly = txtDiaChi.ReadOnly = false;
                    txtTenKhach.Focus();
                }
            }
            catch { }
        }
        private void ClearForm()
        {
            txtMaHD.Clear();
            cboMaKH.SelectedIndex = -1;
            cboMaKH.Text = "";
            txtTenKhach.Clear();
            txtHangKhach.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            cboMaHang.SelectedIndex = -1;
            cboMaHang.Text = "";
            txtTenHang.Clear();
            txtDonGia.Clear();
            txtBaoHanh.Clear();
            txtSoLuong.Text = "1";
            txtGiamGia.Text = "0";
            txtThanhTien.Clear();
            _dtGioHang.Clear();
            txtTongTien.Text = "0";
            lblBangChu.Text = "Bằng chữ: Không đồng";
        }
        private void KhoiTaoGioHang()
        {
            _dtGioHang = new DataTable();
            _dtGioHang.Columns.AddRange(new[]
            {
                new DataColumn("MaLaptop", typeof(string)), new DataColumn("TenLaptop", typeof(string)), new DataColumn("DonGia", typeof(decimal)),
                new DataColumn("SoLuong", typeof(int)), new DataColumn("GiamGia", typeof(decimal)), new DataColumn("ThanhTien", typeof(decimal)),
                new DataColumn("ThoiGianBaoHanh", typeof(int))
            });
            dgvGioHang.AutoGenerateColumns = false;
            colMaHang.DataPropertyName = "MaLaptop";
            colTenHang.DataPropertyName = "TenLaptop";
            colDonGia.DataPropertyName = "DonGia";
            colDonGia.DefaultCellStyle.Format = colThanhTien.DefaultCellStyle.Format = "N0";
            colDonGia.DefaultCellStyle.Alignment = colThanhTien.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colSoLuong.DataPropertyName = "SoLuong";
            colGiamGia.DataPropertyName = "GiamGia";
            colThanhTien.DataPropertyName = "ThanhTien";
            colThoiGianBaoHanh.DataPropertyName = "ThoiGianBaoHanh";
            colSoLuong.DefaultCellStyle.Alignment = colGiamGia.DefaultCellStyle.Alignment = colThoiGianBaoHanh.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvGioHang.DataSource = _dtGioHang;
        }
        private void DangKySuKien()
        {
            cboMaHang.SelectedIndexChanged += CboMaHang_SelectedIndexChanged;
            btnTimKiemSP.Click += (s, e) => MoFormTimKiemLaptop();
            txtSoLuong.TextChanged += (s, e) => TinhThanhTien();
            txtGiamGia.TextChanged += (s, e) => TinhThanhTien();
            txtSoLuong.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { txtGiamGia.Focus(); e.SuppressKeyPress = true; } };
            txtGiamGia.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { ThemMatHangVaoLuoi(); e.SuppressKeyPress = true; } };
            cboMaKH.SelectedIndexChanged += CboMaKH_SelectedIndexChanged;
            txtSDT.Leave += (s, e) => XuLyNhapSoDienThoai();
            txtSDT.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { XuLyNhapSoDienThoai(); e.SuppressKeyPress = true; } };
            btnThemHD.Click += (s, e) => { ThemHoaDonMoi(); SetFormMode(FormMode.Add); txtSDT.Focus(); };
            btnSuaHD.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtMaHD.Text) || !_isSaved) { Function.ThongBaoThanhCong("Vui lòng chọn hoặc tìm kiếm hóa đơn cần sửa.", "Thông báo"); return; }
                SetFormMode(FormMode.Edit);
                cboMaKH.Focus();
            };
            btnLuuHD.Click += (s, e) => { if (_mode == FormMode.Add) LuuHoaDon(); else if (_mode == FormMode.Edit) CapNhatHoaDon(); };
            btnHuy.Click += (s, e) => { if (_mode == FormMode.Add || _mode == FormMode.Edit) { ClearForm(); _isSaved = false; SetFormMode(FormMode.View); } };
            btnHuyHD.Click += (s, e) =>
            {
                if (_isSaved && !string.IsNullOrEmpty(txtMaHD.Text)) XoaHoaDon();
                else if (MessageBox.Show("Bạn có muốn hủy bỏ tạo hóa đơn này không?", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ClearForm(); _isSaved = false; SetFormMode(FormMode.View);
                }
            };
            btnInHD.Click += (s, e) =>
            {
                if (_dtGioHang.Rows.Count == 0) { Function.ThongBaoCanhBao("Không có dữ liệu mặt hàng để in hóa đơn!", "Thông báo"); return; }
                InHoaDonExcel();
            };
            btnDong.Click += (s, e) => { if (MessageBox.Show("Bạn có muốn đóng giao diện bán hàng không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) Close(); };
            dgvGioHang.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0 || e.RowIndex >= _dtGioHang.Rows.Count) return;
                DataRow r = _dtGioHang.Rows[e.RowIndex];
                cboMaHang.SelectedValue = r["MaLaptop"].ToString();
                txtTenHang.Text = r["TenLaptop"].ToString();
                txtDonGia.Text = Convert.ToDecimal(r["DonGia"]).ToString("0");
                txtSoLuong.Text = r["SoLuong"].ToString();
                txtGiamGia.Text = r["GiamGia"].ToString();
                txtThanhTien.Text = Convert.ToDecimal(r["ThanhTien"]).ToString("0");
                txtBaoHanh.Text = r.Table.Columns.Contains("ThoiGianBaoHanh") && r["ThoiGianBaoHanh"] != DBNull.Value ? r["ThoiGianBaoHanh"].ToString() : "12";
            };
            dgvGioHang.CellDoubleClick += DgvGioHang_CellDoubleClick;
            btnTimKiem.Click += (s, e) => TimKiemHoaDon();
            cboMaHDTim.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { TimKiemHoaDon(); e.SuppressKeyPress = true; } };
            btnChonNV.Click += (s, e) => Function.ThongBaoThanhCong("Mã nhân viên và tên nhân viên được cố định theo tài khoản đang đăng nhập.", "Thông tin nhân viên");
            btnChonKH.Click += (s, e) => cboMaKH.DroppedDown = true;
        }
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!Visible) return;
            sidebarControl1.SelectedMenu = NavigationMenu.BanHang;
            LoadThongTinNhanVien();
            LoadDanhSachKhachHang();
            LoadDanhSachMatHang();
            LoadDanhSachMaHDTim();
            if (string.IsNullOrEmpty(txtMaHD.Text) || !_isSaved) { ClearForm(); _isSaved = false; }
            SetFormMode(FormMode.View);
        }
        private void LoadThongTinNhanVien() { txtMaNV.Text = UserSession.MaNV; txtTenNV.Text = UserSession.TenNV; }
        private void LoadDanhSachKhachHang()
        {
            try
            {
                cboMaKH.DataSource = DataProcesser.ExecuteReader("SELECT MaKH, TenKH, HangThanhVien, DiaChi, Sdt FROM KHACH_HANG ORDER BY MaKH");
                cboMaKH.DisplayMember = cboMaKH.ValueMember = "MaKH";
                cboMaKH.SelectedIndex = -1;
            }
            catch (Exception ex) { Function.ThongBaoCanhBao("Lỗi tải danh sách khách hàng: " + ex.Message, "Lỗi"); }
        }
        private void LoadDanhSachMatHang()
        {
            try
            {
                _dtLaptop = DataProcesser.ExecuteReader("SELECT MaLaptop, TenLaptop, DonGia, SoLuongTon, ThoiGianBaoHanh FROM LAPTOP WHERE NgungKinhDoanh = 0 ORDER BY MaLaptop");
                cboMaHang.DataSource = _dtLaptop;
                cboMaHang.DisplayMember = cboMaHang.ValueMember = "MaLaptop";
                cboMaHang.SelectedIndex = -1;
            }
            catch (SqlException ex) { DataProcesser.ShowSqlError(ex); }
        }
        private void LoadDanhSachMaHDTim()
        {
            try
            {
                DataTable dt = DataProcesser.ExecuteReader("SELECT MaHD FROM HOA_DON ORDER BY NgayLap DESC");
                cboMaHDTim.Items.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    string ma = r["MaHD"].ToString();
                    if (!string.IsNullOrEmpty(ma)) cboMaHDTim.Items.Add(ma);
                }
            }
            catch { }
        }
        private void CboMaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaHang.SelectedItem is DataRowView drv)
            {
                txtTenHang.Text = drv["TenLaptop"].ToString();
                txtDonGia.Text = Convert.ToDecimal(drv["DonGia"]).ToString("0");
                txtBaoHanh.Text = drv.Row.Table.Columns.Contains("ThoiGianBaoHanh") && drv["ThoiGianBaoHanh"] != DBNull.Value ? drv["ThoiGianBaoHanh"].ToString() : "12";
                txtSoLuong.Text = "1";
                txtGiamGia.Text = LayPhanTramGiamGiaTheoHang(txtHangKhach.Text).ToString("0.##");
                TinhThanhTien();
            }
            else { txtTenHang.Clear(); txtDonGia.Clear(); txtBaoHanh.Clear(); txtThanhTien.Clear(); }
        }
        private void MoFormTimKiemLaptop()
        {
            using (var frm = new frmTimKiemLaptop())
            {
                if (frm.ShowDialog(this) != DialogResult.OK || string.IsNullOrEmpty(frm.SelectedMaLaptop)) return;
                cboMaHang.SelectedValue = cboMaHang.Text = frm.SelectedMaLaptop;
                txtTenHang.Text = frm.SelectedTenLaptop;
                txtDonGia.Text = frm.SelectedDonGia.ToString("0");
                txtBaoHanh.Text = frm.SelectedBaoHanh > 0 ? frm.SelectedBaoHanh.ToString() : "12";
                txtSoLuong.Text = "1";
                txtGiamGia.Text = LayPhanTramGiamGiaTheoHang(txtHangKhach.Text).ToString("0.##");
                TinhThanhTien();
                txtSoLuong.Focus();
                txtSoLuong.SelectAll();
            }
        }
        private void TinhThanhTien()
        {
            decimal.TryParse(txtDonGia.Text.Trim(), out decimal donGia);
            int.TryParse(txtSoLuong.Text.Trim(), out int sl);
            decimal.TryParse(txtGiamGia.Text.Trim(), out decimal giamGia);
            sl = Math.Max(0, sl);
            giamGia = Math.Min(100, Math.Max(0, giamGia));
            txtThanhTien.Text = Math.Round(donGia * sl * (1 - giamGia / 100m), 0).ToString("0");
        }
        private bool ThemMatHangVaoLuoi()
        {
            string maHang = !string.IsNullOrEmpty(cboMaHang.Text) ? cboMaHang.Text.Trim() : (cboMaHang.SelectedValue?.ToString() ?? "");
            if (string.IsNullOrEmpty(maHang)) { Function.ThongBaoCanhBao("Vui lòng nhấn nút 'Tìm máy' để chọn sản phẩm trước.", "Thông báo"); return false; }
            string tenHang = txtTenHang.Text.Trim();
            if (!int.TryParse(txtSoLuong.Text.Trim(), out int sl) || sl <= 0) { Function.ThongBaoCanhBao("Số lượng phải là số nguyên dương lớn hơn 0.", "Thông báo"); txtSoLuong.Focus(); return false; }
            if (!decimal.TryParse(txtGiamGia.Text.Trim(), out decimal giam) || giam < 0 || giam > 100) { Function.ThongBaoCanhBao("Giảm giá phải từ 0% đến 100%.", "Thông báo"); txtGiamGia.Focus(); return false; }
            decimal.TryParse(txtDonGia.Text.Trim(), out decimal donGia);
            decimal.TryParse(txtThanhTien.Text.Trim(), out decimal thanhTien);
            int thoiGianBH = (!string.IsNullOrEmpty(txtBaoHanh.Text.Trim()) && int.TryParse(txtBaoHanh.Text.Trim(), out int bh)) ? bh
                : (cboMaHang.SelectedItem is DataRowView drv && drv.Row.Table.Columns.Contains("ThoiGianBaoHanh") && drv["ThoiGianBaoHanh"] != DBNull.Value) ? Convert.ToInt32(drv["ThoiGianBaoHanh"]) : 12;
            object tonObj = DataProcesser.ExecuteScalar("SELECT SoLuongTon FROM LAPTOP WHERE MaLaptop = @Ma", new[] { new SqlParameter("@Ma", maHang) });
            int tonKho = tonObj != null && tonObj != DBNull.Value ? Convert.ToInt32(tonObj) : 0;
            if (sl > tonKho)
            {
                Function.ThongBaoCanhBao($"Mặt hàng [{tenHang}] chỉ còn {tonKho} máy trong kho, không đủ số lượng ({sl})!", "Không đủ tồn kho");
                return false;
            }
            DataRow[] existing = _dtGioHang.Select($"MaLaptop = '{maHang}'");
            if (existing.Length > 0)
            {
                existing[0]["SoLuong"] = sl;
                existing[0]["GiamGia"] = giam;
                existing[0]["DonGia"] = donGia;
                existing[0]["ThanhTien"] = thanhTien;
                existing[0]["ThoiGianBaoHanh"] = thoiGianBH;
            }
            else _dtGioHang.Rows.Add(maHang, tenHang, donGia, sl, giam, thanhTien, thoiGianBH);
            CapNhatTongTien();
            return true;
        }
        private void CapNhatTongTien()
        {
            decimal tong = 0;
            foreach (DataRow r in _dtGioHang.Rows) if (r.RowState != DataRowState.Deleted) tong += Convert.ToDecimal(r["ThanhTien"]);
            txtTongTien.Text = Math.Round(tong, 0).ToString("0");
            lblBangChu.Text = "Bằng chữ: " + Function.ChuyenSoThanhChu(tong);
        }
        private decimal LayPhanTramGiamGiaTheoHang(string hang) => new Services.ConfigurationService().DiscountFor(hang);
        private void CapNhatGiamGiaChoGioHang(decimal phanTramGiam)
        {
            if (_dtGioHang == null || _dtGioHang.Rows.Count == 0) return;
            foreach (DataRow r in _dtGioHang.Rows)
            {
                if (r.RowState == DataRowState.Deleted) continue;
                r["GiamGia"] = phanTramGiam;
                r["ThanhTien"] = Math.Round(Convert.ToDecimal(r["DonGia"]) * Convert.ToInt32(r["SoLuong"]) * (1 - phanTramGiam / 100m), 0);
            }
            CapNhatTongTien();
        }
        private void CboMaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaKH.SelectedItem is DataRowView drv)
            {
                txtTenKhach.Text = drv["TenKH"] != DBNull.Value ? drv["TenKH"].ToString() : "";
                string hang = drv.Row.Table.Columns.Contains("HangThanhVien") && drv["HangThanhVien"] != DBNull.Value ? drv["HangThanhVien"].ToString() : "Đồng";
                txtHangKhach.Text = hang;
                txtDiaChi.Text = drv["DiaChi"] != DBNull.Value ? drv["DiaChi"].ToString() : "";
                txtSDT.Text = drv["Sdt"] != DBNull.Value ? drv["Sdt"].ToString() : "";
                decimal phanTramGiam = LayPhanTramGiamGiaTheoHang(hang);
                txtGiamGia.Text = phanTramGiam.ToString("0.##");
                TinhThanhTien();
                if (_mode == FormMode.Add || _mode == FormMode.Edit) CapNhatGiamGiaChoGioHang(phanTramGiam);
            }
            else { txtTenKhach.Clear(); txtHangKhach.Clear(); txtDiaChi.Clear(); txtSDT.Clear(); txtGiamGia.Text = "0"; TinhThanhTien(); }
        }
        private void ThemHoaDonMoi()
        {
            try
            {
                ClearForm();
                _isSaved = false;
                dtpNgayBan.Value = DateTime.Now;
                txtMaHD.Text = Function.SinhMaHoaDonTuDong();
                LoadThongTinNhanVien();
                txtSDT.Focus();
            }
            catch (Exception ex) { Function.ThongBaoCanhBao("Lỗi tạo mới hóa đơn: " + ex.Message, "Lỗi"); }
        }
        private void MoFormThanhToan()
        {
            if (cboMaHang.SelectedItem != null && !string.IsNullOrEmpty(txtTenHang.Text.Trim()) && int.TryParse(txtSoLuong.Text.Trim(), out int slNhap) && slNhap > 0)
                ThemMatHangVaoLuoi();
            if (_dtGioHang.Rows.Count == 0) { Function.ThongBaoCanhBao("Hóa đơn phải có ít nhất một mặt hàng. Vui lòng chọn mặt hàng trước!", "Thông báo"); cboMaHang.Focus(); return; }
            string maHD = txtMaHD.Text.Trim();
            if (string.IsNullOrEmpty(maHD)) { ThemHoaDonMoi(); maHD = txtMaHD.Text.Trim(); }
            if (cboMaKH.SelectedItem == null || string.IsNullOrEmpty(cboMaKH.Text.Trim()))
            {
                if (cboMaKH.Items.Count > 0 && cboMaKH.SelectedIndex == -1) cboMaKH.SelectedIndex = 0;
                else { Function.ThongBaoCanhBao("Vui lòng chọn khách hàng trước khi thanh toán.", "Thông báo"); cboMaKH.Focus(); return; }
            }
            if (_isSaved)
            {
                var ask = MessageBox.Show("Hóa đơn " + maHD + " này đã được thanh toán và lưu vào CSDL rồi.\n\nBạn có muốn mở xem trước và in hóa đơn không?\n(Chọn 'Yes' để mở cửa sổ xem trước hóa đơn, 'No' để xuất file Excel)", "In lại hóa đơn", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (ask == DialogResult.Yes) XemTruocHoaDonHienTai();
                else if (ask == DialogResult.No) InHoaDonExcel();
                return;
            }
            decimal tongTien = 0;
            foreach (DataRow r in _dtGioHang.Rows) if (r.RowState != DataRowState.Deleted) tongTien += Convert.ToDecimal(r["ThanhTien"]);
            string maNV = string.IsNullOrEmpty(txtMaNV.Text.Trim()) ? (Function.CurrentMaNV ?? "NV001") : txtMaNV.Text.Trim();
            using (var frm = new frmThanhToan(maHD, cboMaKH.Text.Trim(), txtTenKhach.Text.Trim(), txtHangKhach.Text.Trim(), maNV, txtTenNV.Text.Trim(), txtDiaChi.Text.Trim(), txtSDT.Text.Trim(), dtpNgayBan.Value, tongTien, _dtGioHang))
                if (frm.ShowDialog(this) == DialogResult.OK) RefreshSale();
        }
        private void XemTruocHoaDonHienTai()
        {
            string maHD = txtMaHD.Text.Trim();
            if (string.IsNullOrEmpty(maHD)) return;
            decimal tongTien = 0;
            foreach (DataRow r in _dtGioHang.Rows) if (r.RowState != DataRowState.Deleted) tongTien += Convert.ToDecimal(r["ThanhTien"]);
            string phuongThuc = "Tiền mặt";
            int chuKy = 0;
            try
            {
                DataTable dtHD = DataProcesser.ExecuteReader("SELECT PhuongThucThanhToan, ChuKyTraGop, TinhTrang, TongTien FROM HOA_DON WHERE MaHD = @Ma", new[] { new SqlParameter("@Ma", maHD) });
                if (dtHD != null && dtHD.Rows.Count > 0)
                {
                    phuongThuc = dtHD.Rows[0]["PhuongThucThanhToan"]?.ToString() ?? "Tiền mặt";
                    if (dtHD.Rows[0]["ChuKyTraGop"] != DBNull.Value) chuKy = Convert.ToInt32(dtHD.Rows[0]["ChuKyTraGop"]);
                    if (dtHD.Rows[0]["TongTien"] != DBNull.Value) tongTien = Convert.ToDecimal(dtHD.Rows[0]["TongTien"]);
                }
            }
            catch { }
            bool isTraGop = phuongThuc == "Trả góp" || chuKy > 0;
            double laiSuat = 0;
            decimal downpayment = 0, loan = 0, interest = 0, monthly = 0;
            if (isTraGop)
            {
                var plan = Services.InstallmentCalculator.Load(maHD, tongTien, chuKy);
                laiSuat = (double)plan.Rate; downpayment = plan.DownPayment; loan = plan.Principal; interest = plan.Interest; monthly = plan.Monthly;
            }
            using (var frm = new frmPrintPreview(new HoaDonPrintData
            {
                MaHD = maHD, NgayLap = dtpNgayBan.Value, MaKH = cboMaKH.Text.Trim(), TenKH = txtTenKhach.Text.Trim(),
                HangKhach = txtHangKhach.Text.Trim(), SDT = txtSDT.Text.Trim(), DiaChi = txtDiaChi.Text.Trim(),
                MaNV = string.IsNullOrEmpty(txtMaNV.Text.Trim()) ? (Function.CurrentMaNV ?? "NV001") : txtMaNV.Text.Trim(),
                TenNV = txtTenNV.Text.Trim(), IsTraGop = isTraGop, PhuongThuc = phuongThuc, ChuKy = chuKy, LaiSuat = laiSuat,
                TongTien = tongTien, TienTraTruoc = downpayment, TienVay = loan, TienLai = interest, TienHangThang = monthly,
                KhachDua = tongTien, TienThua = 0, DtChiTiet = _dtGioHang
            })) frm.ShowDialog(this);
        }
        private void LuuHoaDon() => MoFormThanhToan();
        private void DgvGioHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _dtGioHang.Rows.Count) return;
            var row = _dtGioHang.Rows[e.RowIndex];
            if (Function.XacNhan("Trả/xóa sản phẩm " + row["TenLaptop"] + "?") != DialogResult.Yes) return;
            try
            {
                if (_isSaved) new Services.SaleService().RemoveLine(txtMaHD.Text.Trim(), Convert.ToString(row["MaLaptop"]));
                _dtGioHang.Rows.RemoveAt(e.RowIndex);
                CapNhatTongTien();
                LoadDanhSachMatHang();
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }
        private void CapNhatHoaDon()
        {
            if (cboMaHang.SelectedItem != null && int.TryParse(txtSoLuong.Text, out int quantity) && quantity > 0 && !ThemMatHangVaoLuoi()) return;
            try
            {
                new Services.SaleService().Update(new Services.SaleRequest
                {
                    InvoiceId = txtMaHD.Text.Trim(), CustomerId = cboMaKH.Text.Trim(), EmployeeId = UserSession.MaNV, Date = dtpNgayBan.Value, Items = _dtGioHang
                });
                RefreshSale();
                Function.ThongBaoThanhCong("Đã cập nhật hóa đơn.");
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }
        private void RefreshSale()
        {
            _isSaved = true;
            CapNhatTongTien();
            LoadDanhSachMaHDTim();
            LoadDanhSachMatHang();
            LoadDanhSachKhachHang();
            SetFormMode(FormMode.View);
        }
        private void XoaHoaDon()
        {
            if (!_isSaved) { ClearForm(); SetFormMode(FormMode.View); return; }
            if (Function.XacNhan("Xóa hóa đơn " + txtMaHD.Text + " và hoàn lại hàng vào kho?") != DialogResult.Yes) return;
            try
            {
                new Services.SaleService().Delete(txtMaHD.Text.Trim());
                ClearForm();
                _isSaved = false;
                LoadDanhSachMatHang();
                LoadDanhSachMaHDTim();
                SetFormMode(FormMode.View);
            }
            catch (Exception ex) { Function.ThongBaoLoi(ex.Message); }
        }
        private void TimKiemHoaDon()
        {
            string maHD = cboMaHDTim.Text.Trim();
            if (string.IsNullOrEmpty(maHD)) { Function.ThongBaoCanhBao("Vui lòng nhập hoặc chọn mã hóa đơn cần tìm.", "Thông báo"); cboMaHDTim.Focus(); return; }
            TaiThongTinHoaDon(maHD);
        }
        private bool TaiThongTinHoaDon(string maHD)
        {
            try
            {
                DataTable dtHD = DataProcesser.ExecuteReader(@"SELECT hd.MaHD, hd.NgayLap, hd.MaNV, nv.TenNV, hd.MaKH, kh.TenKH, kh.HangThanhVien, kh.DiaChi, kh.Sdt, hd.TongTien
                    FROM HOA_DON hd LEFT JOIN NHAN_VIEN nv ON hd.MaNV = nv.MaNV LEFT JOIN KHACH_HANG kh ON hd.MaKH = kh.MaKH
                    WHERE UPPER(hd.MaHD) = UPPER(@MaHD)", new[] { new SqlParameter("@MaHD", maHD) });
                if (dtHD == null || dtHD.Rows.Count == 0) { Function.ThongBaoThanhCong($"Không tìm thấy hóa đơn có mã [{maHD}] trong CSDL.", "Thông báo"); return false; }
                DataRow r = dtHD.Rows[0];
                txtMaHD.Text = r["MaHD"].ToString();
                if (r["NgayLap"] != DBNull.Value) dtpNgayBan.Value = Convert.ToDateTime(r["NgayLap"]);
                txtMaNV.Text = r["MaNV"] != DBNull.Value ? r["MaNV"].ToString() : "";
                txtTenNV.Text = r["TenNV"] != DBNull.Value ? r["TenNV"].ToString() : "";
                cboMaKH.SelectedValue = r["MaKH"] != DBNull.Value ? r["MaKH"].ToString() : "";
                txtTenKhach.Text = r["TenKH"] != DBNull.Value ? r["TenKH"].ToString() : "";
                txtHangKhach.Text = r["HangThanhVien"] != DBNull.Value ? r["HangThanhVien"].ToString() : "Đồng";
                txtDiaChi.Text = r["DiaChi"] != DBNull.Value ? r["DiaChi"].ToString() : "";
                txtSDT.Text = r["Sdt"] != DBNull.Value ? r["Sdt"].ToString() : "";
                DataTable dtCT = DataProcesser.ExecuteReader("SELECT ct.MaLaptop, l.TenLaptop, ct.DonGia, ct.SoLuong, ct.GiamGia, ct.ThanhTien, ct.ThoiGianBaoHanh FROM CHI_TIET_HD ct JOIN LAPTOP l ON ct.MaLaptop = l.MaLaptop WHERE ct.MaHD = @MaHD", new[] { new SqlParameter("@MaHD", maHD) });
                _dtGioHang.Clear();
                foreach (DataRow ct in dtCT.Rows)
                {
                    decimal donGia = Convert.ToDecimal(ct["DonGia"]);
                    int sl = Convert.ToInt32(ct["SoLuong"]);
                    decimal giam = ct["GiamGia"] != DBNull.Value ? Convert.ToDecimal(ct["GiamGia"]) : 0;
                    decimal tt = ct["ThanhTien"] != DBNull.Value ? Convert.ToDecimal(ct["ThanhTien"]) : Math.Round(donGia * sl * (1 - giam / 100m), 0);
                    int tgBH = ct["ThoiGianBaoHanh"] != DBNull.Value ? Convert.ToInt32(ct["ThoiGianBaoHanh"]) : 12;
                    _dtGioHang.Rows.Add(ct["MaLaptop"], ct["TenLaptop"], donGia, sl, giam, tt, tgBH);
                }
                CapNhatTongTien();
                _isSaved = true;
                if (dtCT.Rows.Count > 0)
                {
                    DataRow first = dtCT.Rows[0];
                    cboMaHang.SelectedValue = first["MaLaptop"].ToString();
                    txtTenHang.Text = first["TenLaptop"].ToString();
                    txtDonGia.Text = Convert.ToDecimal(first["DonGia"]).ToString("0");
                    txtSoLuong.Text = first["SoLuong"].ToString();
                    txtGiamGia.Text = first["GiamGia"].ToString();
                    txtThanhTien.Text = Convert.ToDecimal(first["ThanhTien"]).ToString("0");
                    txtBaoHanh.Text = first["ThoiGianBaoHanh"] != DBNull.Value ? first["ThoiGianBaoHanh"].ToString() : "12";
                }
                else
                {
                    cboMaHang.SelectedIndex = -1; cboMaHang.Text = ""; txtTenHang.Clear(); txtDonGia.Clear(); txtBaoHanh.Clear();
                    txtSoLuong.Text = "1"; txtGiamGia.Text = "0"; txtThanhTien.Clear();
                }
                SetFormMode(FormMode.View);
                return true;
            }
            catch (Exception ex) { Function.ThongBaoLoi("Lỗi khi tìm kiếm hóa đơn: " + ex.Message, "Lỗi"); return false; }
        }
        private void InHoaDonExcel()
        {
            if (_dtGioHang.Rows.Count == 0) { Function.ThongBaoCanhBao("Không có dữ liệu mặt hàng để in hóa đơn!", "Thông báo"); return; }
            string maHD = txtMaHD.Text.Trim(), ngayBan = dtpNgayBan.Value.ToString("dd/MM/yyyy");
            string maNV = txtMaNV.Text.Trim(), tenNV = txtTenNV.Text.Trim(), maKH = cboMaKH.Text.Trim(), tenKH = txtTenKhach.Text.Trim();
            string hangKhach = txtHangKhach.Text.Trim(), diaChi = txtDiaChi.Text.Trim(), sdt = txtSDT.Text.Trim(), tongTien = txtTongTien.Text.Trim();
            string bangChu = lblBangChu.Text.Replace("Bằng chữ: ", "");
            try
            {
                Type excelType = Type.GetTypeFromProgID("Excel.Application");
                if (excelType != null)
                {
                    dynamic excelApp = Activator.CreateInstance(excelType);
                    excelApp.Visible = true;
                    dynamic wb = excelApp.Workbooks.Add();
                    dynamic ws = wb.Sheets[1];
                    ws.Name = "HoaDonBanHang";
                    ws.Cells[1, 1] = "CỬA HÀNG BÁN LAPTOP CHÍNH HÃNG";
                    ws.Cells[1, 1].Font.Bold = true; ws.Cells[1, 1].Font.Size = 12;
                    ws.Cells[2, 1] = "Địa chỉ: Hà Nội - Hotline: 1900 6868"; ws.Cells[2, 1].Font.Italic = true;
                    dynamic rangeTitle = ws.Range["A4", "H4"];
                    rangeTitle.Merge(); rangeTitle.Value = "HÓA ĐƠN BÁN HÀNG";
                    rangeTitle.Font.Bold = true; rangeTitle.Font.Size = 18;
                    rangeTitle.Font.Color = ColorTranslator.ToOle(Color.DarkRed);
                    rangeTitle.HorizontalAlignment = -4108;
                    ws.Cells[6, 1] = "Mã hóa đơn:"; ws.Cells[6, 2] = "'" + maHD; ws.Cells[6, 2].Font.Bold = true;
                    ws.Cells[6, 5] = "Ngày bán:"; ws.Cells[6, 6] = ngayBan;
                    ws.Cells[7, 1] = "Nhân viên bán:"; ws.Cells[7, 2] = tenNV + " (" + maNV + ")";
                    ws.Cells[8, 1] = "Khách hàng:"; ws.Cells[8, 2] = tenKH + " [" + maKH + "]";
                    ws.Cells[8, 5] = "Hạng thành viên:"; ws.Cells[8, 6] = hangKhach;
                    ws.Cells[9, 1] = "Địa chỉ:"; ws.Cells[9, 2] = diaChi;
                    ws.Cells[10, 1] = "Điện thoại:"; ws.Cells[10, 2] = "'" + sdt;
                    string[] headers = { "STT", "Mã hàng", "Tên hàng", "Đơn giá (VNĐ)", "Số lượng", "Giảm giá (%)", "Thành tiền (VNĐ)", "Thời gian BH" };
                    for (int c = 0; c < headers.Length; c++)
                    {
                        ws.Cells[12, c + 1] = headers[c]; ws.Cells[12, c + 1].Font.Bold = true;
                        ws.Cells[12, c + 1].Interior.Color = ColorTranslator.ToOle(Color.FromArgb(215, 248, 215));
                        ws.Cells[12, c + 1].HorizontalAlignment = -4108;
                    }
                    int startRow = 13, stt = 1;
                    foreach (DataRow r in _dtGioHang.Rows)
                    {
                        if (r.RowState == DataRowState.Deleted) continue;
                        ws.Cells[startRow, 1] = stt++; ws.Cells[startRow, 1].HorizontalAlignment = -4108;
                        ws.Cells[startRow, 2] = "'" + r["MaLaptop"];
                        ws.Cells[startRow, 3] = r["TenLaptop"].ToString();
                        ws.Cells[startRow, 4] = Convert.ToDecimal(r["DonGia"]); ws.Cells[startRow, 4].NumberFormat = "#,##0";
                        ws.Cells[startRow, 5] = Convert.ToInt32(r["SoLuong"]); ws.Cells[startRow, 5].HorizontalAlignment = -4108;
                        ws.Cells[startRow, 6] = Convert.ToDecimal(r["GiamGia"]); ws.Cells[startRow, 6].HorizontalAlignment = -4108;
                        ws.Cells[startRow, 7] = Convert.ToDecimal(r["ThanhTien"]); ws.Cells[startRow, 7].NumberFormat = "#,##0";
                        ws.Cells[startRow, 8] = r.Table.Columns.Contains("ThoiGianBaoHanh") ? r["ThoiGianBaoHanh"] + " tháng" : "12 tháng";
                        ws.Cells[startRow, 8].HorizontalAlignment = -4108;
                        startRow++;
                    }
                    ws.Range["A12", "H" + (startRow - 1)].Borders.LineStyle = 1;
                    dynamic sumLabel = ws.Range["A" + startRow, "F" + startRow];
                    sumLabel.Merge(); sumLabel.Value = "TỔNG CỘNG TIỀN THANH TOÁN:"; sumLabel.Font.Bold = true; sumLabel.HorizontalAlignment = -4152;
                    ws.Cells[startRow, 7] = decimal.TryParse(tongTien, out decimal ttVal) ? ttVal : 0;
                    ws.Cells[startRow, 7].Font.Bold = true; ws.Cells[startRow, 7].NumberFormat = "#,##0";
                    startRow++;
                    dynamic txtBangChu = ws.Range["A" + startRow, "H" + startRow];
                    txtBangChu.Merge(); txtBangChu.Value = "Số tiền bằng chữ: " + bangChu; txtBangChu.Font.Italic = true;
                    startRow += 2;
                    ws.Cells[startRow, 2] = "Khách hàng"; ws.Cells[startRow, 2].Font.Bold = true; ws.Cells[startRow, 2].HorizontalAlignment = -4108;
                    ws.Cells[startRow, 7] = "Nhân viên bán hàng"; ws.Cells[startRow, 7].Font.Bold = true; ws.Cells[startRow, 7].HorizontalAlignment = -4108;
                    ws.Cells[startRow + 1, 2] = "(Ký, ghi rõ họ tên)"; ws.Cells[startRow + 1, 2].Font.Italic = true; ws.Cells[startRow + 1, 2].HorizontalAlignment = -4108;
                    ws.Cells[startRow + 1, 7] = "(Ký, ghi rõ họ tên)"; ws.Cells[startRow + 1, 7].Font.Italic = true; ws.Cells[startRow + 1, 7].HorizontalAlignment = -4108;
                    ws.Columns.AutoFit();
                    return;
                }
            }
            catch { }
            XuatFileExcelDuPhong(maHD, ngayBan, tenNV, maNV, tenKH, maKH, hangKhach, diaChi, sdt, tongTien, bangChu);
        }
        private void XuatFileExcelDuPhong(string maHD, string ngayBan, string tenNV, string maNV, string tenKH, string maKH, string hangKhach, string diaChi, string sdt, string tongTien, string bangChu)
        {
            using (var sfd = new SaveFileDialog { Filter = "Tệp Excel (*.xls)|*.xls", FileName = $"HoaDon_{maHD}.xls" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'>");
                    sb.AppendLine("<head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'></head><body>");
                    sb.AppendLine("<h2 style='color:#b22222; text-align:center;'>HÓA ĐƠN BÁN HÀNG</h2>");
                    sb.AppendLine($"<p><b>Mã hóa đơn:</b> {maHD} &nbsp;&nbsp;&nbsp;&nbsp; <b>Ngày bán:</b> {ngayBan}</p>");
                    sb.AppendLine($"<p><b>Nhân viên bán:</b> {tenNV} ({maNV})</p>");
                    sb.AppendLine($"<p><b>Khách hàng:</b> {tenKH} ({maKH}) &nbsp;&nbsp;&nbsp;&nbsp; <b>Hạng thành viên:</b> {hangKhach}</p>");
                    sb.AppendLine($"<p><b>Điện thoại:</b> {sdt} &nbsp;&nbsp;&nbsp;&nbsp; <b>Địa chỉ:</b> {diaChi}</p>");
                    sb.AppendLine("<table border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");
                    sb.AppendLine("<tr style='background-color:#d7f8d7; font-weight:bold;'><th>STT</th><th>Mã hàng</th><th>Tên hàng</th><th>Đơn giá</th><th>Số lượng</th><th>Giảm giá (%)</th><th>Thành tiền</th><th>Thời gian BH</th></tr>");
                    int stt = 1;
                    foreach (DataRow r in _dtGioHang.Rows)
                    {
                        if (r.RowState == DataRowState.Deleted) continue;
                        string bh = r.Table.Columns.Contains("ThoiGianBaoHanh") ? r["ThoiGianBaoHanh"] + " tháng" : "12 tháng";
                        sb.AppendLine($"<tr><td align='center'>{stt++}</td><td>{r["MaLaptop"]}</td><td>{r["TenLaptop"]}</td><td align='right'>{Convert.ToDecimal(r["DonGia"]):N0}</td><td align='center'>{r["SoLuong"]}</td><td align='center'>{r["GiamGia"]}</td><td align='right'>{Convert.ToDecimal(r["ThanhTien"]):N0}</td><td align='center'>{bh}</td></tr>");
                    }
                    decimal.TryParse(tongTien, out decimal ttVal);
                    sb.AppendLine($"<tr style='font-weight:bold;'><td colspan='6' align='right'>TỔNG CỘNG:</td><td align='right'>{ttVal:N0}</td><td></td></tr></table>");
                    sb.AppendLine($"<p><i>Số tiền bằng chữ: {bangChu}</i></p><br><br>");
                    sb.AppendLine("<table style='width:100%; text-align:center;'><tr><td><b>Người mua hàng</b><br><i>(Ký tên)</i></td><td><b>Người bán hàng</b><br><i>(Ký tên)</i></td></tr></table></body></html>");
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    if (MessageBox.Show($"Xuất hóa đơn thành công ra tệp:\n{sfd.FileName}\n\nBạn có muốn mở tệp ngay không?", "Xuất Excel thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch (Exception ex) { Function.ThongBaoLoi("Lỗi lưu file Excel: " + ex.Message, "Lỗi"); }
            }
        }
        private void lblBaoHanh_Click(object sender, EventArgs e) { }
    }
}

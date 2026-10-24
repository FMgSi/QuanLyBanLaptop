using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.HoaDon
{
    public partial class frmBanHang : Form
    {
        // DataTable cache laptop từ DB
        private DataTable _dtLaptop;

        // DataTable giỏ hàng (in-memory, chưa lưu DB)
        private DataTable _dtGioHang;

        public frmBanHang()
        {
            InitializeComponent();

            NavigationManager.RegisterForm(NavigationMenu.BanHang, this);
            sidebarControl1.SelectedMenu = NavigationMenu.BanHang;

            // Khởi tạo giỏ hàng in-memory
            KhoiTaoGioHang();

            // Đăng ký sự kiện
            cboLaptopBan.SelectedIndexChanged += CboLaptopBan_SelectedIndexChanged;
            btnThemVaoGio.Click += BtnThemVaoGio_Click;
            btnXoaKhoiGio.Click += BtnXoaKhoiGio_Click;
            btnThanhToan.Click  += BtnThanhToan_Click;
        }

        // ─── Khởi tạo DataTable giỏ hàng ─────────────────────────────────
        private void KhoiTaoGioHang()
        {
            _dtGioHang = new DataTable();
            _dtGioHang.Columns.Add("MaLaptop",  typeof(string));
            _dtGioHang.Columns.Add("TenLaptop", typeof(string));
            _dtGioHang.Columns.Add("DonGia",    typeof(decimal));
            _dtGioHang.Columns.Add("SoLuong",   typeof(int));
            _dtGioHang.Columns.Add("GiamGia",   typeof(decimal));   // %
            _dtGioHang.Columns.Add("ThanhTien", typeof(decimal));

            // Bind DataTable vào DataGridView
            dgvGioHang.AutoGenerateColumns = false;
            colMa.DataPropertyName         = "MaLaptop";
            colTenLaptop.DataPropertyName  = "TenLaptop";
            colDonGia.DataPropertyName     = "DonGia";
            colSoLuong.DataPropertyName    = "SoLuong";
            colGiamGia.DataPropertyName    = "GiamGia";
            colThanhTien.DataPropertyName  = "ThanhTien";
            dgvGioHang.DataSource = _dtGioHang;
        }

        // ─── Hiện form → tải dữ liệu từ DB ──────────────────────────────
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.BanHang;
                LoadDuLieu();
            }
        }

        private void LoadDuLieu()
        {
            try
            {
                // Laptop còn hàng, chưa ngừng kinh doanh
                _dtLaptop = DataProcesser.ExecuteReader(
                    @"SELECT MaLaptop, TenLaptop, DonGia, SoLuongTon
                      FROM LAPTOP
                      WHERE NgungKinhDoanh = 0 AND SoLuongTon > 0
                      ORDER BY TenLaptop");

                cboLaptopBan.DataSource    = _dtLaptop;
                cboLaptopBan.DisplayMember = "TenLaptop";
                cboLaptopBan.ValueMember   = "MaLaptop";

                if (_dtLaptop != null && _dtLaptop.Rows.Count > 0)
                    UpdateDonGia();
            }
            catch (SqlException ex)
            {
                DataProcesser.ShowSqlError(ex);
            }
        }

        // ─── Đổi laptop → cập nhật đơn giá ──────────────────────────────
        private void CboLaptopBan_SelectedIndexChanged(object sender, EventArgs e)
            => UpdateDonGia();

        private void UpdateDonGia()
        {
            if (cboLaptopBan.SelectedItem is DataRowView drv)
                txtDonGiaBan.Text = Function.FormatTien(Convert.ToDecimal(drv["DonGia"]));
        }

        // ─── Thêm vào giỏ ────────────────────────────────────────────────
        private void BtnThemVaoGio_Click(object sender, EventArgs e)
        {
            if (cboLaptopBan.SelectedItem == null) return;

            // Validate số lượng
            if (!int.TryParse(txtSoLuongBan.Text.Trim(), out int sl) || sl <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongBan.Focus();
                return;
            }

            // Validate giảm giá
            if (!decimal.TryParse(txtGiamGia.Text.Trim(), out decimal giam) || giam < 0 || giam > 100)
            {
                MessageBox.Show("Giảm giá phải từ 0 đến 100.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiamGia.Focus();
                return;
            }

            var drv        = (DataRowView)cboLaptopBan.SelectedItem;
            string maLT    = drv["MaLaptop"].ToString();
            string tenLT   = drv["TenLaptop"].ToString();
            decimal donGia = Convert.ToDecimal(drv["DonGia"]);
            int tonKho     = Convert.ToInt32(drv["SoLuongTon"]);

            // Kiểm tra đã có trong giỏ chưa → cộng dồn
            DataRow[] existing = _dtGioHang.Select($"MaLaptop = '{maLT}'");
            if (existing.Length > 0)
            {
                int slCu  = Convert.ToInt32(existing[0]["SoLuong"]);
                int slMoi = slCu + sl;

                if (slMoi > tonKho)
                {
                    MessageBox.Show($"Tổng số lượng ({slMoi}) vượt tồn kho ({tonKho}).", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal tt = donGia * slMoi * (1 - giam / 100m);
                existing[0]["SoLuong"]   = slMoi;
                existing[0]["GiamGia"]   = giam;
                existing[0]["ThanhTien"] = Math.Round(tt, 0);
            }
            else
            {
                if (sl > tonKho)
                {
                    MessageBox.Show($"Số lượng ({sl}) vượt tồn kho ({tonKho}).", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal tt = donGia * sl * (1 - giam / 100m);
                _dtGioHang.Rows.Add(maLT, tenLT, donGia, sl, giam, Math.Round(tt, 0));
            }

            CapNhatTongTien();

            // Reset input
            txtSoLuongBan.Text = "1";
            txtGiamGia.Text    = "0";
        }

        // ─── Xóa hàng đang chọn khỏi giỏ ────────────────────────────────
        private void BtnXoaKhoiGio_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null || _dtGioHang.Rows.Count == 0) return;
            int idx = dgvGioHang.CurrentRow.Index;
            if (idx >= 0 && idx < _dtGioHang.Rows.Count)
            {
                _dtGioHang.Rows.RemoveAt(idx);
                CapNhatTongTien();
            }
        }

        // ─── Tổng tiền ───────────────────────────────────────────────────
        private void CapNhatTongTien()
        {
            decimal tong = 0;
            foreach (DataRow r in _dtGioHang.Rows)
                tong += Convert.ToDecimal(r["ThanhTien"]);
            lblTongTienSo.Text = Function.FormatTien(tong);
        }

        // ─── Xác nhận thanh toán → sp_BanHang ───────────────────────────
        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (_dtGioHang.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống! Vui lòng thêm laptop vào giỏ.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sdt = txtSDT.Text.Trim();
            if (string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng nhập SĐT khách hàng.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            try
            {
                // 1. Tìm hoặc tạo khách hàng
                string maKH = TimHoacTaoKhachHang(sdt);
                if (maKH == null) return;

                // 2. Gọi sp_BanHang
                string maHD;
                using (var conn = DataProcesser.GetConnection())
                {
                    var tvp = new DataTable();
                    tvp.Columns.Add("MaLaptop", typeof(string));
                    tvp.Columns.Add("SoLuong",  typeof(int));
                    foreach (DataRow r in _dtGioHang.Rows)
                        tvp.Rows.Add(r["MaLaptop"], r["SoLuong"]);

                    using (var cmd = new SqlCommand("sp_BanHang", conn)
                    { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@MaKH",       maKH);
                        cmd.Parameters.AddWithValue("@MaNV",       Function.CurrentMaNV ?? "NV001");
                        cmd.Parameters.AddWithValue("@PhuongThuc", "Tiền mặt");
                        cmd.Parameters.AddWithValue("@ChuKyTraGop", 0);

                        var pItems = cmd.Parameters.Add("@Items", SqlDbType.Structured);
                        pItems.TypeName = "dbo.TVP_BanHang";
                        pItems.Value    = tvp;

                        var pOut = new SqlParameter("@MaHD", SqlDbType.VarChar, 10)
                        { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(pOut);

                        cmd.ExecuteNonQuery();
                        maHD = pOut.Value?.ToString();
                    }
                }

                MessageBox.Show($"Thanh toán thành công!\nMã hóa đơn: {maHD}", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reset form
                _dtGioHang.Clear();
                txtTenKhach.Clear();
                txtSDT.Clear();
                txtDiaChi.Clear();
                lblTongTienSo.Text = "0 ₫";
                LoadDuLieu(); // Refresh tồn kho
            }
            catch (SqlException ex)
            {
                DataProcesser.ShowSqlError(ex);
            }
        }

        // ─── Tìm hoặc tạo khách hàng theo SĐT ───────────────────────────
        private string TimHoacTaoKhachHang(string sdt)
        {
            // Tìm khách hàng đã có
            object found = DataProcesser.ExecuteScalar(
                "SELECT MaKH FROM KHACH_HANG WHERE Sdt = @Sdt",
                new[] { new SqlParameter("@Sdt", sdt) });

            if (found != null && found != DBNull.Value)
                return found.ToString();

            // Chưa có → yêu cầu nhập tên
            string tenKhach = txtTenKhach.Text.Trim();
            if (string.IsNullOrEmpty(tenKhach))
            {
                MessageBox.Show("Khách hàng mới! Vui lòng nhập tên khách.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhach.Focus();
                return null;
            }

            // Sinh mã KH
            object soMax = DataProcesser.ExecuteScalar(
                "SELECT ISNULL(MAX(CAST(SUBSTRING(MaKH,3,LEN(MaKH)) AS INT)),0) FROM KHACH_HANG WHERE MaKH LIKE 'KH%'");
            int so     = Convert.ToInt32(soMax) + 1;
            string maKH = "KH" + so.ToString("D3");

            DataProcesser.ExecuteNonQuery(
                @"INSERT INTO KHACH_HANG(MaKH, TenKH, DiaChi, Sdt)
                  VALUES(@MaKH, @TenKH, @DiaChi, @Sdt)",
                new[]
                {
                    new SqlParameter("@MaKH",  maKH),
                    new SqlParameter("@TenKH", tenKhach),
                    new SqlParameter("@DiaChi",
                        string.IsNullOrEmpty(txtDiaChi.Text.Trim())
                            ? (object)DBNull.Value
                            : txtDiaChi.Text.Trim()),
                    new SqlParameter("@Sdt", sdt)
                });

            return maKH;
        }
    }
}

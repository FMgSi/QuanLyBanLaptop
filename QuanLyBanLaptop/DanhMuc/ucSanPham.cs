using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QuanLyBanLaptop.Helpers;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class ucSanPham : UserControl
    {
        private DataTable _dtLaptop;

        public ucSanPham()
        {
            InitializeComponent();
            ControlHelper.SetDoubleBuffered(dgvLaptop);
        }

        private void ucSanPham_Load(object sender, EventArgs e)
        {
            LoadTieuChiTimKiem();
            LoadPhanLoai();
            LoadNhaCungCap();
            LoadDanhSachLaptop();
        }

        #region Nạp dữ liệu ComboBox & GridView

        private void LoadTieuChiTimKiem()
        {
            cboTieuChiTimKiem.Items.Clear();
            cboTieuChiTimKiem.Items.Add("Tất cả cột");
            cboTieuChiTimKiem.Items.Add("Theo Mã Laptop");
            cboTieuChiTimKiem.Items.Add("Theo Tên Laptop");
            cboTieuChiTimKiem.SelectedIndex = 0;
        }

        private void LoadPhanLoai()
        {
            try
            {
                DataTable dt = DbHelper.ReadTable("SELECT TenLoai FROM LOAI_LAPTOP ORDER BY TenLoai ASC");
                if (dt != null && dt.Rows.Count > 0)
                {
                    cboLoai.DataSource = dt;
                    cboLoai.DisplayMember = "TenLoai";
                    cboLoai.ValueMember = "TenLoai";
                }
                else
                {
                    ControlHelper.FillCombo(cboLoai, 0,
                        "Gaming",
                        "Văn phòng",
                        "Mỏng nhẹ",
                        "Đồ họa",
                        "Sinh viên",
                        "Cao cấp");
                }
            }
            catch
            {
                ControlHelper.FillCombo(cboLoai, 0,
                    "Gaming",
                    "Văn phòng",
                    "Mỏng nhẹ",
                    "Đồ họa",
                    "Sinh viên",
                    "Cao cấp");
            }
        }

        private void LoadNhaCungCap()
        {
            try
            {
                DataTable dt = DbHelper.ReadTable("SELECT MaNCC, TenNCC FROM NHA_CUNG_CAP ORDER BY TenNCC");
                cboNhaCungCap.DataSource = dt;
                cboNhaCungCap.DisplayMember = "TenNCC";
                cboNhaCungCap.ValueMember = "MaNCC";
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Không tải được danh sách Nhà cung cấp:\n" + ex.Message);
            }
        }

        private void LoadDanhSachLaptop()
        {
            try
            {
                string sql = @"
                    SELECT 
                        l.MaLaptop,
                        l.TenLaptop,
                        ISNULL(l.Loai, N'') AS Loai,
                        ISNULL(ncc.TenNCC, N'') AS TenNCC,
                        ISNULL(l.CPU, N'') AS CPU,
                        ISNULL(l.RAM, N'') AS RAM,
                        ISNULL(l.SSD, N'') AS SSD,
                        CAST(l.DonGia AS BIGINT) AS DonGia,
                        l.SoLuongTon,
                        l.MaNCC
                    FROM LAPTOP l
                    LEFT JOIN NHA_CUNG_CAP ncc ON l.MaNCC = ncc.MaNCC
                    WHERE l.NgungKinhDoanh = 0
                    ORDER BY l.MaLaptop ASC";

                _dtLaptop = DbHelper.ReadTable(sql);
                HienThiLenGridView(_dtLaptop);
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Không tải được danh sách sản phẩm:\n" + ex.Message);
            }
        }

        private void HienThiLenGridView(DataTable dt)
        {
            dgvLaptop.Rows.Clear();
            if (dt == null) return;

            foreach (DataRow row in dt.Rows)
            {
                decimal donGia = Convert.ToDecimal(row["DonGia"]);
                int index = dgvLaptop.Rows.Add(
                    row["MaLaptop"],
                    row["TenLaptop"],
                    row["Loai"],
                    row["TenNCC"],
                    row["CPU"],
                    row["RAM"],
                    row["SSD"],
                    FormatHelper.Money(donGia),
                    row["SoLuongTon"]
                );

                // Lưu MaNCC vào Tag của dòng để tái sử dụng khi click
                dgvLaptop.Rows[index].Tag = row["MaNCC"]?.ToString();
            }

            dgvLaptop.ClearSelection();
        }

        #endregion

        #region Ràng buộc nhập số nguyên

        // Chỉ cho phép nhập chữ số và phím điều khiển (Backspace) vào ô Số lượng và Đơn giá
        private void txtOnlyNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion

        #region Thao tác Click chọn dòng trên GridView

        private void dgvLaptop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvLaptop.Rows.Count) return;

            DataGridViewRow row = dgvLaptop.Rows[e.RowIndex];

            txtMa.Text = row.Cells["colMaLaptop"].Value?.ToString() ?? "";
            txtTenLaptop.Text = row.Cells["colTenLaptop"].Value?.ToString() ?? "";

            string loai = row.Cells["colLoai"].Value?.ToString() ?? "";
            cboLoai.Text = loai;
            if (cboLoai.SelectedIndex < 0 && cboLoai.Items.Count > 0)
                cboLoai.SelectedIndex = 0;

            txtCPU.Text = row.Cells["colCPU"].Value?.ToString() ?? "";
            txtRAM.Text = row.Cells["colRAM"].Value?.ToString() ?? "";
            txtSSD.Text = row.Cells["colSSD"].Value?.ToString() ?? "";

            // Đơn giá: bỏ dấu chấm phân cách ngàn để đưa vào textbox dạng số nguyên
            string donGiaStr = (row.Cells["colDonGia"].Value?.ToString() ?? "").Replace(".", "").Replace(",", "");
            txtDonGia.Text = donGiaStr;

            txtSoLuong.Text = row.Cells["colSoLuongTon"].Value?.ToString() ?? "0";

            // Chọn Nhà cung cấp tương ứng
            string maNCC = row.Tag as string;
            if (!string.IsNullOrEmpty(maNCC))
            {
                cboNhaCungCap.SelectedValue = maNCC;
            }

            // Khi click chọn dòng để xem/sửa -> khóa ô Mã Laptop không cho đổi khóa chính
            txtMa.ReadOnly = true;
            txtMa.BackColor = Color.FromArgb(240, 240, 240);
        }

        #endregion

        #region Chức năng THÊM

        private void btnThem_Click(object sender, EventArgs e)
        {
            string ma = txtMa.Text.Trim();
            string ten = txtTenLaptop.Text.Trim();

            // 1. Kiểm tra nhập đủ thông tin cơ bản
            if (string.IsNullOrEmpty(ma))
            {
                MessageHelper.Warning("Vui lòng nhập Mã Laptop.");
                txtMa.Focus();
                return;
            }

            if (string.IsNullOrEmpty(ten))
            {
                MessageHelper.Warning("Vui lòng nhập Tên Laptop.");
                txtTenLaptop.Focus();
                return;
            }

            if (cboNhaCungCap.SelectedValue == null)
            {
                MessageHelper.Warning("Vui lòng chọn Nhà cung cấp.");
                cboNhaCungCap.Focus();
                return;
            }

            // 2. Kiểm tra số lượng và đơn giá hợp lệ
            if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuong) || soLuong < 0)
            {
                MessageHelper.Warning("Số lượng phải là số nguyên không âm.");
                txtSoLuong.Focus();
                return;
            }

            if (!long.TryParse(txtDonGia.Text.Trim(), out long donGia) || donGia <= 0)
            {
                MessageHelper.Warning("Đơn giá phải là số nguyên dương lớn hơn 0.");
                txtDonGia.Focus();
                return;
            }

            // 3. Kiểm tra mã sản phẩm không được trùng
            try
            {
                object count = DbHelper.ReadScalar("SELECT COUNT(*) FROM LAPTOP WHERE MaLaptop = @0", ma);
                if (count != null && Convert.ToInt32(count) > 0)
                {
                    MessageHelper.Error("Mã Laptop '" + ma + "' đã tồn tại trong hệ thống. Vui lòng nhập mã khác!");
                    txtMa.SelectAll();
                    txtMa.Focus();
                    return;
                }

                // 4. Thêm vào CSDL
                string sqlInsert = @"
                    INSERT INTO LAPTOP 
                        (MaLaptop, TenLaptop, Loai, CPU, RAM, SSD, DonGia, SoLuongTon, MaNCC, ThoiGianBaoHanh, NgungKinhDoanh)
                    VALUES 
                        (@0, @1, @2, @3, @4, @5, @6, @7, @8, 12, 0)";

                DbHelper.Execute(sqlInsert,
                    ma,
                    ten,
                    cboLoai.Text.Trim(),
                    txtCPU.Text.Trim(),
                    txtRAM.Text.Trim(),
                    txtSSD.Text.Trim(),
                    donGia,
                    soLuong,
                    cboNhaCungCap.SelectedValue.ToString());

                MessageHelper.Info("Thêm mới sản phẩm thành công!");

                // 5. Nạp lại GridView và xóa trắng form
                LoadDanhSachLaptop();
                XoaTrangForm();
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Lỗi khi thêm sản phẩm:\n" + ex.Message);
            }
        }

        #endregion

        #region Chức năng SỬA

        private void btnSua_Click(object sender, EventArgs e)
        {
            string ma = txtMa.Text.Trim();
            if (string.IsNullOrEmpty(ma))
            {
                MessageHelper.Warning("Vui lòng chọn một sản phẩm từ danh sách dưới để sửa.");
                return;
            }

            string ten = txtTenLaptop.Text.Trim();
            if (string.IsNullOrEmpty(ten))
            {
                MessageHelper.Warning("Vui lòng nhập Tên Laptop.");
                txtTenLaptop.Focus();
                return;
            }

            if (cboNhaCungCap.SelectedValue == null)
            {
                MessageHelper.Warning("Vui lòng chọn Nhà cung cấp.");
                cboNhaCungCap.Focus();
                return;
            }

            if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuong) || soLuong < 0)
            {
                MessageHelper.Warning("Số lượng phải là số nguyên không âm.");
                txtSoLuong.Focus();
                return;
            }

            if (!long.TryParse(txtDonGia.Text.Trim(), out long donGia) || donGia <= 0)
            {
                MessageHelper.Warning("Đơn giá phải là số nguyên dương lớn hơn 0.");
                txtDonGia.Focus();
                return;
            }

            try
            {
                string sqlUpdate = @"
                    UPDATE LAPTOP 
                    SET 
                        TenLaptop = @0,
                        Loai = @1,
                        CPU = @2,
                        RAM = @3,
                        SSD = @4,
                        DonGia = @5,
                        SoLuongTon = @6,
                        MaNCC = @7
                    WHERE MaLaptop = @8";

                int affected = DbHelper.Execute(sqlUpdate,
                    ten,
                    cboLoai.Text.Trim(),
                    txtCPU.Text.Trim(),
                    txtRAM.Text.Trim(),
                    txtSSD.Text.Trim(),
                    donGia,
                    soLuong,
                    cboNhaCungCap.SelectedValue.ToString(),
                    ma);

                if (affected > 0)
                {
                    MessageHelper.Info("Cập nhật thông tin sản phẩm '" + ma + "' thành công!");
                    LoadDanhSachLaptop();
                }
                else
                {
                    MessageHelper.Error("Không tìm thấy sản phẩm có mã '" + ma + "' để cập nhật.");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Lỗi khi cập nhật sản phẩm:\n" + ex.Message);
            }
        }

        #endregion

        #region Chức năng XÓA

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string ma = txtMa.Text.Trim();
            string ten = txtTenLaptop.Text.Trim();

            if (string.IsNullOrEmpty(ma))
            {
                MessageHelper.Warning("Vui lòng chọn sản phẩm cần xóa từ bảng bên dưới.");
                return;
            }

            // Hỏi xác nhận trước khi xóa
            string msg = string.Format("Bạn có chắc chắn muốn xóa sản phẩm:\n[{0}] - {1}?", ma, ten);
            if (!MessageHelper.Confirm(msg))
            {
                return;
            }

            try
            {
                try
                {
                    // Thử xóa vật lý nếu sản phẩm chưa phát sinh giao dịch
                    DbHelper.Execute("DELETE FROM LAPTOP WHERE MaLaptop = @0", ma);
                }
                catch (SqlException ex) when (ex.Number == 547)
                {
                    // Nếu sản phẩm đã có hóa đơn / phiếu nhập liên quan, chuyển sang ngừng kinh doanh
                    DbHelper.Execute("UPDATE LAPTOP SET NgungKinhDoanh = 1 WHERE MaLaptop = @0", ma);
                }

                MessageHelper.Info("Đã xóa sản phẩm '" + ma + "' thành công!");
                LoadDanhSachLaptop();
                XoaTrangForm();
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Lỗi khi xóa sản phẩm:\n" + ex.Message);
            }
        }

        #endregion

        #region Chức năng LÀM MỚI / TÌM KIẾM

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            XoaTrangForm();
            txtTimKiem.Clear();
            LoadDanhSachLaptop();
        }

        private void XoaTrangForm()
        {
            txtMa.Clear();
            txtTenLaptop.Clear();
            txtCPU.Clear();
            txtRAM.Clear();
            txtSSD.Clear();
            txtDonGia.Clear();
            txtSoLuong.Clear();

            if (cboLoai.Items.Count > 0) cboLoai.SelectedIndex = 0;
            if (cboNhaCungCap.Items.Count > 0) cboNhaCungCap.SelectedIndex = 0;

            txtMa.ReadOnly = false;
            txtMa.BackColor = Color.White;
            txtMa.Focus();

            dgvLaptop.ClearSelection();
        }

        private void cboTieuChiTimKiem_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTimKiem_TextChanged(sender, e);
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (_dtLaptop == null) return;

            string keyword = txtTimKiem.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(keyword))
            {
                HienThiLenGridView(_dtLaptop);
                return;
            }

            DataView dv = new DataView(_dtLaptop);
            int tieuChi = cboTieuChiTimKiem.SelectedIndex;

            switch (tieuChi)
            {
                case 1: // Theo Mã Laptop
                    dv.RowFilter = string.Format("MaLaptop LIKE '%{0}%'", keyword);
                    break;
                case 2: // Theo Tên Laptop
                    dv.RowFilter = string.Format("TenLaptop LIKE '%{0}%'", keyword);
                    break;
                default: // Tất cả
                    dv.RowFilter = string.Format("MaLaptop LIKE '%{0}%' OR TenLaptop LIKE '%{0}%' OR TenNCC LIKE '%{0}%' OR Loai LIKE '%{0}%'", keyword);
                    break;
            }

            HienThiLenGridView(dv.ToTable());
        }

        #endregion

        #region Mở Bảng Quản lý Phân loại & Nhà cung cấp

        private void btnQuanLyLoai_Click(object sender, EventArgs e)
        {
            using (var frm = new Forms.frmQuanLyLoai())
            {
                frm.ShowDialog(this);
            }

            // Đồng bộ lại danh mục phân loại sau khi thêm/sửa/xóa
            string selectedLoai = cboLoai.Text;
            LoadPhanLoai();
            if (!string.IsNullOrEmpty(selectedLoai))
                cboLoai.Text = selectedLoai;

            // Nạp lại danh sách laptop vì có thể loại đã được đổi tên
            LoadDanhSachLaptop();
        }

        private void btnQuanLyNCC_Click(object sender, EventArgs e)
        {
            using (var frm = new Forms.frmQuanLyNhaCungCap())
            {
                frm.ShowDialog(this);
            }

            // Đồng bộ lại danh mục nhà cung cấp sau khi thêm/sửa/xóa
            object selectedNCC = cboNhaCungCap.SelectedValue;
            LoadNhaCungCap();
            if (selectedNCC != null)
                cboNhaCungCap.SelectedValue = selectedNCC;

            // Nạp lại danh sách laptop vì thông tin NCC hiển thị trên grid
            LoadDanhSachLaptop();
        }

        #endregion
    }
}

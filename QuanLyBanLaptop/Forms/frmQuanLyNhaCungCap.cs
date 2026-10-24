using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyBanLaptop.Helpers;

namespace QuanLyBanLaptop.Forms
{
    public partial class frmQuanLyNhaCungCap : Form
    {
        public frmQuanLyNhaCungCap()
        {
            InitializeComponent();
            ControlHelper.SetDoubleBuffered(dgvNCC);
        }

        private void frmQuanLyNhaCungCap_Load(object sender, EventArgs e)
        {
            LoadDanhSachNCC();
            LamMoiForm();
        }

        private void LoadDanhSachNCC()
        {
            try
            {
                DataTable dt = DbHelper.ReadTable("SELECT MaNCC, TenNCC, ISNULL(Sdt, N'') AS Sdt, ISNULL(DiaChi, N'') AS DiaChi FROM NHA_CUNG_CAP ORDER BY MaNCC ASC");
                dgvNCC.DataSource = dt;
                dgvNCC.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Lỗi tải danh sách nhà cung cấp:\n" + ex.Message);
            }
        }

        private void LamMoiForm()
        {
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtSdt.Clear();
            txtDiaChi.Clear();

            txtMaNCC.ReadOnly = false;
            txtMaNCC.BackColor = Color.White;
            txtMaNCC.Focus();
            dgvNCC.ClearSelection();
        }

        private void dgvNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvNCC.Rows.Count) return;

            DataGridViewRow row = dgvNCC.Rows[e.RowIndex];
            txtMaNCC.Text = row.Cells["colMaNCC"].Value?.ToString() ?? "";
            txtTenNCC.Text = row.Cells["colTenNCC"].Value?.ToString() ?? "";
            txtSdt.Text = row.Cells["colSdt"].Value?.ToString() ?? "";
            txtDiaChi.Text = row.Cells["colDiaChi"].Value?.ToString() ?? "";

            txtMaNCC.ReadOnly = true;
            txtMaNCC.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string ma = txtMaNCC.Text.Trim();
            string ten = txtTenNCC.Text.Trim();
            string sdt = txtSdt.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();

            if (string.IsNullOrEmpty(ma))
            {
                MessageHelper.Warning("Vui lòng nhập Mã Nhà cung cấp.");
                txtMaNCC.Focus();
                return;
            }

            if (string.IsNullOrEmpty(ten))
            {
                MessageHelper.Warning("Vui lòng nhập Tên Nhà cung cấp.");
                txtTenNCC.Focus();
                return;
            }

            try
            {
                object countMa = DbHelper.ReadScalar("SELECT COUNT(*) FROM NHA_CUNG_CAP WHERE MaNCC = @0", ma);
                if (countMa != null && Convert.ToInt32(countMa) > 0)
                {
                    MessageHelper.Error("Mã Nhà cung cấp '" + ma + "' đã tồn tại! Vui lòng chọn mã khác.");
                    txtMaNCC.Focus();
                    return;
                }

                DbHelper.Execute(
                    "INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, DiaChi, Sdt) VALUES (@0, @1, @2, @3)",
                    ma,
                    ten,
                    string.IsNullOrEmpty(diaChi) ? (object)DBNull.Value : diaChi,
                    string.IsNullOrEmpty(sdt) ? (object)DBNull.Value : sdt
                );

                MessageHelper.Info("Thêm Nhà cung cấp mới thành công!");
                LoadDanhSachNCC();
                LamMoiForm();
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Lỗi khi thêm Nhà cung cấp:\n" + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string ma = txtMaNCC.Text.Trim();
            string ten = txtTenNCC.Text.Trim();
            string sdt = txtSdt.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();

            if (string.IsNullOrEmpty(ma))
            {
                MessageHelper.Warning("Vui lòng chọn Nhà cung cấp cần sửa từ bảng bên dưới.");
                return;
            }

            if (string.IsNullOrEmpty(ten))
            {
                MessageHelper.Warning("Vui lòng nhập Tên Nhà cung cấp.");
                txtTenNCC.Focus();
                return;
            }

            try
            {
                int affected = DbHelper.Execute(
                    "UPDATE NHA_CUNG_CAP SET TenNCC = @0, DiaChi = @1, Sdt = @2 WHERE MaNCC = @3",
                    ten,
                    string.IsNullOrEmpty(diaChi) ? (object)DBNull.Value : diaChi,
                    string.IsNullOrEmpty(sdt) ? (object)DBNull.Value : sdt,
                    ma
                );

                if (affected > 0)
                {
                    MessageHelper.Info("Cập nhật Nhà cung cấp '" + ma + "' thành công!");
                    LoadDanhSachNCC();
                    LamMoiForm();
                }
                else
                {
                    MessageHelper.Error("Không tìm thấy Nhà cung cấp có mã '" + ma + "' để cập nhật.");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Lỗi khi cập nhật Nhà cung cấp:\n" + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string ma = txtMaNCC.Text.Trim();
            string ten = txtTenNCC.Text.Trim();

            if (string.IsNullOrEmpty(ma))
            {
                MessageHelper.Warning("Vui lòng chọn Nhà cung cấp cần xóa từ bảng bên dưới.");
                return;
            }

            try
            {
                // 1. Kiểm tra liên kết với bảng LAPTOP
                object countLaptop = DbHelper.ReadScalar("SELECT COUNT(*) FROM LAPTOP WHERE MaNCC = @0", ma);
                int soLap = countLaptop != null ? Convert.ToInt32(countLaptop) : 0;
                if (soLap > 0)
                {
                    MessageHelper.Error(string.Format("Không thể xóa NCC '{0}' vì đang có {1} sản phẩm laptop liên kết!", ten, soLap));
                    return;
                }

                // 2. Kiểm tra liên kết với bảng PHIEU_NHAP_HANG
                object countPN = DbHelper.ReadScalar("SELECT COUNT(*) FROM PHIEU_NHAP_HANG WHERE MaNCC = @0", ma);
                int soPN = countPN != null ? Convert.ToInt32(countPN) : 0;
                if (soPN > 0)
                {
                    MessageHelper.Error(string.Format("Không thể xóa NCC '{0}' vì đang có {1} phiếu nhập hàng liên quan!", ten, soPN));
                    return;
                }

                if (!MessageHelper.Confirm("Bạn có chắc chắn muốn xóa Nhà cung cấp:\n[" + ma + "] - " + ten + "?"))
                {
                    return;
                }

                DbHelper.Execute("DELETE FROM NHA_CUNG_CAP WHERE MaNCC = @0", ma);
                MessageHelper.Info("Đã xóa Nhà cung cấp thành công!");
                LoadDanhSachNCC();
                LamMoiForm();
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Lỗi khi xóa Nhà cung cấp:\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiForm();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

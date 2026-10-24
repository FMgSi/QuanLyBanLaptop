using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyBanLaptop.Helpers;

namespace QuanLyBanLaptop.Forms
{
    public partial class frmQuanLyLoai : Form
    {
        private string _tenLoaiGoc = string.Empty;

        public frmQuanLyLoai()
        {
            InitializeComponent();
            ControlHelper.SetDoubleBuffered(dgvLoai);
        }

        private void frmQuanLyLoai_Load(object sender, EventArgs e)
        {
            LoadDanhSachLoai();
            LamMoiForm();
        }

        private void LoadDanhSachLoai()
        {
            try
            {
                DataTable dt = DbHelper.ReadTable("SELECT MaLoai, TenLoai FROM LOAI_LAPTOP ORDER BY MaLoai ASC");
                dgvLoai.DataSource = dt;
                dgvLoai.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Lỗi tải danh sách phân loại:\n" + ex.Message);
            }
        }

        private void LamMoiForm()
        {
            txtMaLoai.Clear();
            txtTenLoai.Clear();
            txtMaLoai.ReadOnly = false;
            txtMaLoai.BackColor = Color.White;
            _tenLoaiGoc = string.Empty;
            txtMaLoai.Focus();
            dgvLoai.ClearSelection();
        }

        private void dgvLoai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvLoai.Rows.Count) return;

            DataGridViewRow row = dgvLoai.Rows[e.RowIndex];
            txtMaLoai.Text = row.Cells["colMaLoai"].Value?.ToString() ?? "";
            txtTenLoai.Text = row.Cells["colTenLoai"].Value?.ToString() ?? "";
            _tenLoaiGoc = txtTenLoai.Text.Trim();

            txtMaLoai.ReadOnly = true;
            txtMaLoai.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string ma = txtMaLoai.Text.Trim();
            string ten = txtTenLoai.Text.Trim();

            if (string.IsNullOrEmpty(ma))
            {
                MessageHelper.Warning("Vui lòng nhập Mã phân loại.");
                txtMaLoai.Focus();
                return;
            }

            if (string.IsNullOrEmpty(ten))
            {
                MessageHelper.Warning("Vui lòng nhập Tên phân loại.");
                txtTenLoai.Focus();
                return;
            }

            try
            {
                object countMa = DbHelper.ReadScalar("SELECT COUNT(*) FROM LOAI_LAPTOP WHERE MaLoai = @0", ma);
                if (countMa != null && Convert.ToInt32(countMa) > 0)
                {
                    MessageHelper.Error("Mã phân loại '" + ma + "' đã tồn tại! Vui lòng chọn mã khác.");
                    txtMaLoai.Focus();
                    return;
                }

                object countTen = DbHelper.ReadScalar("SELECT COUNT(*) FROM LOAI_LAPTOP WHERE TenLoai = @0", ten);
                if (countTen != null && Convert.ToInt32(countTen) > 0)
                {
                    MessageHelper.Error("Tên phân loại '" + ten + "' đã tồn tại!");
                    txtTenLoai.Focus();
                    return;
                }

                DbHelper.Execute("INSERT INTO LOAI_LAPTOP (MaLoai, TenLoai) VALUES (@0, @1)", ma, ten);
                MessageHelper.Info("Thêm phân loại mới thành công!");
                LoadDanhSachLoai();
                LamMoiForm();
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Lỗi khi thêm phân loại:\n" + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string ma = txtMaLoai.Text.Trim();
            string ten = txtTenLoai.Text.Trim();

            if (string.IsNullOrEmpty(ma))
            {
                MessageHelper.Warning("Vui lòng chọn phân loại cần sửa từ danh sách bên dưới.");
                return;
            }

            if (string.IsNullOrEmpty(ten))
            {
                MessageHelper.Warning("Vui lòng nhập Tên phân loại mới.");
                txtTenLoai.Focus();
                return;
            }

            try
            {
                // Kiểm tra xem tên mới có bị trùng với loại khác không
                object countTen = DbHelper.ReadScalar("SELECT COUNT(*) FROM LOAI_LAPTOP WHERE TenLoai = @0 AND MaLoai <> @1", ten, ma);
                if (countTen != null && Convert.ToInt32(countTen) > 0)
                {
                    MessageHelper.Error("Tên phân loại '" + ten + "' đã được sử dụng cho mã khác!");
                    txtTenLoai.Focus();
                    return;
                }

                // Cập nhật tên phân loại
                DbHelper.Execute("UPDATE LOAI_LAPTOP SET TenLoai = @0 WHERE MaLoai = @1", ten, ma);

                // Nếu tên loại đổi, đồng bộ luôn tên loại trong bảng LAPTOP
                if (!string.IsNullOrEmpty(_tenLoaiGoc) && !_tenLoaiGoc.Equals(ten, StringComparison.OrdinalIgnoreCase))
                {
                    DbHelper.Execute("UPDATE LAPTOP SET Loai = @0 WHERE Loai = @1", ten, _tenLoaiGoc);
                }

                MessageHelper.Info("Cập nhật phân loại '" + ma + "' thành công!");
                LoadDanhSachLoai();
                LamMoiForm();
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Lỗi khi cập nhật phân loại:\n" + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string ma = txtMaLoai.Text.Trim();
            string ten = txtTenLoai.Text.Trim();

            if (string.IsNullOrEmpty(ma))
            {
                MessageHelper.Warning("Vui lòng chọn phân loại cần xóa từ bảng bên dưới.");
                return;
            }

            // Kiểm tra xem có Laptop nào đang thuộc phân loại này không
            try
            {
                object countLaptop = DbHelper.ReadScalar("SELECT COUNT(*) FROM LAPTOP WHERE Loai = @0", ten);
                int soLap = countLaptop != null ? Convert.ToInt32(countLaptop) : 0;
                if (soLap > 0)
                {
                    MessageHelper.Error(string.Format("Không thể xóa phân loại '{0}' vì đang có {1} sản phẩm thuộc loại này!", ten, soLap));
                    return;
                }

                if (!MessageHelper.Confirm("Bạn có chắc chắn muốn xóa phân loại:\n[" + ma + "] - " + ten + "?"))
                {
                    return;
                }

                DbHelper.Execute("DELETE FROM LOAI_LAPTOP WHERE MaLoai = @0", ma);
                MessageHelper.Info("Đã xóa phân loại thành công!");
                LoadDanhSachLoai();
                LamMoiForm();
            }
            catch (Exception ex)
            {
                MessageHelper.Error("Lỗi khi xóa phân loại:\n" + ex.Message);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmCauHinhTraGop : QuanLyBanLaptop.Classes.AppForm
    {
        private readonly Dictionary<string, ThamSo> danhSachThamSo = TaoDanhSachThamSo();
        private readonly DataTable bangCauHinh = new DataTable();
        private bool dangSua;

        public frmCauHinhTraGop()
        {
            InitializeComponent();

            foreach (var item in danhSachThamSo.Values) cboThamSo.Items.Add(item);
            if (cboThamSo.Items.Count > 0) cboThamSo.SelectedIndex = 0;

            cboThamSo.SelectedIndexChanged += cboThamSo_SelectedIndexChanged;
            dgvCauHinh.SelectionChanged += dgvCauHinh_SelectionChanged;

            btnLamMoi.Click += (s, e) => TaiDuLieu();
            btnSua.Click += btnSua_Click;
            btnLuu.Click += btnLuu_Click;
            btnHuy.Click += btnHuy_Click;

            Shown += (s, e) => TaiDuLieu();
            CapNhatTrangThaiSua(false);

            dgvCauHinh.AllowUserToResizeColumns = false;
            dgvCauHinh.AllowUserToResizeRows = false;
            dgvCauHinh.AllowUserToOrderColumns = false;
        }

        private void TaiDuLieu()
        {
            try
            {
                DataTable dt = DataProcesser.ExecuteReader("SELECT Khoa, GiaTri, MoTa FROM CAU_HINH ORDER BY Khoa");
                bangCauHinh.Clear();
                if (dt != null)
                {
                    bangCauHinh.Merge(dt);
                }

                dgvCauHinh.Rows.Clear();
                foreach (DataRow row in bangCauHinh.Rows)
                {
                    string khoa = Convert.ToString(row["Khoa"]);
                    string moTa = Convert.ToString(row["MoTa"]);
                    ThamSo item;
                    if (!danhSachThamSo.TryGetValue(khoa, out item))
                    {
                        item = new ThamSo(khoa, string.IsNullOrWhiteSpace(moTa) ? khoa : moTa, "");
                    }
                    else if (!string.IsNullOrWhiteSpace(moTa))
                    {
                        item = new ThamSo(khoa, moTa, item.DonVi);
                    }
                    dgvCauHinh.Rows.Add(item.MoTa, khoa, Convert.ToString(row["GiaTri"]), item.DonVi);
                }

                if (dgvCauHinh.Rows.Count > 0)
                {
                    dgvCauHinh.Rows[0].Selected = true;
                }
                HienThiDongDangChon();

                if (dgvCauHinh.Rows.Count == 0)
                {
                    Function.ThongBaoCanhBao("Bảng CAU_HINH chưa có tham số nào.", "Cấu hình");
                }
            }
            catch (Exception ex)
            {
                Function.ThongBaoLoi("Không thể đọc cấu hình từ bảng CAU_HINH: " + ex.Message, "Lỗi dữ liệu");
            }
        }

        private void dgvCauHinh_SelectionChanged(object sender, EventArgs e)
        {
            if (!dangSua) HienThiDongDangChon();
        }

        private void HienThiDongDangChon()
        {
            if (dgvCauHinh == null || dgvCauHinh.CurrentRow == null) return;
            string khoa = Convert.ToString(dgvCauHinh.CurrentRow.Cells["Khoa"].Value);
            if (string.IsNullOrEmpty(khoa)) return;

            txtKhoa.Text = khoa;
            txtGiaTri.Text = Convert.ToString(dgvCauHinh.CurrentRow.Cells["GiaTri"].Value);
            txtMoTa.Text = Convert.ToString(dgvCauHinh.CurrentRow.Cells["MoTa"].Value);

            int index = cboThamSo.Items.Cast<object>()
                .Select((item, i) => new { item, i })
                .Where(x => x.item is ThamSo && ((ThamSo)x.item).Khoa == khoa)
                .Select(x => x.i)
                .DefaultIfEmpty(0)
                .First();
            cboThamSo.SelectedIndex = index;
        }

        private void cboThamSo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dangSua || !(cboThamSo.SelectedItem is ThamSo)) return;
            string khoa = ((ThamSo)cboThamSo.SelectedItem).Khoa;
            foreach (DataGridViewRow row in dgvCauHinh.Rows)
            {
                if (Convert.ToString(row.Cells["Khoa"].Value) == khoa)
                {
                    row.Selected = true;
                    dgvCauHinh.CurrentCell = row.Cells["Khoa"];
                    return;
                }
            }
            txtKhoa.Text = khoa;
            txtGiaTri.Clear();
            txtMoTa.Text = ((ThamSo)cboThamSo.SelectedItem).MoTa;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKhoa.Text))
            {
                Function.ThongBaoCanhBao("Hãy chọn một tham số đang có trong bảng CAU_HINH.", "Chưa chọn tham số");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtGiaTri.Text))
            {
                Function.ThongBaoCanhBao("Tham số này chưa có trong bảng CAU_HINH nên không thể chỉnh sửa.", "Thiếu dữ liệu");
                return;
            }
            CapNhatTrangThaiSua(true);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            decimal value;
            if (!decimal.TryParse(txtGiaTri.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out value) &&
                !decimal.TryParse(txtGiaTri.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out value))
            {
                Function.ThongBaoCanhBao("Giá trị phải là số hợp lệ.", "Dữ liệu không hợp lệ");
                txtGiaTri.Focus();
                return;
            }

            ThamSo item;
            danhSachThamSo.TryGetValue(txtKhoa.Text, out item);
            if (item != null && (item.Khoa == "TRA_TRUOC_TOI_THIEU" || item.Khoa.StartsWith("LAI_SUAT_")) && (value < 0 || value > 100))
            {
                Function.ThongBaoCanhBao("Tỷ lệ phần trăm phải nằm trong khoảng từ 0 đến 100.", "Dữ liệu không hợp lệ");
                return;
            }

            try
            {
                string sql = "UPDATE CAU_HINH SET GiaTri = @GiaTri WHERE Khoa = @Khoa";
                SqlParameter[] paras = new[]
                {
                    new SqlParameter("@GiaTri", SqlDbType.NVarChar, 100) { Value = value.ToString(CultureInfo.InvariantCulture) },
                    new SqlParameter("@Khoa", SqlDbType.VarChar, 50) { Value = txtKhoa.Text.Trim() }
                };

                int rows = DataProcesser.ExecuteNonQuery(sql, paras);
                if (rows == 0)
                {
                    Function.ThongBaoCanhBao("Không tìm thấy tham số trong CAU_HINH; chưa có thay đổi nào được lưu.", "Không tìm thấy");
                    return;
                }

                CapNhatTrangThaiSua(false);
                TaiDuLieu();
                Function.ThongBaoThanhCong("Đã lưu cấu hình thành công.", "Thành công");
            }
            catch (Exception ex)
            {
                Function.ThongBaoLoi("Không thể lưu cấu hình: " + ex.Message, "Lỗi dữ liệu");
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            CapNhatTrangThaiSua(false);
            HienThiDongDangChon();
        }

        private void CapNhatTrangThaiSua(bool sua)
        {
            dangSua = sua;
            txtGiaTri.ReadOnly = !sua;
            txtGiaTri.BackColor = sua ? Color.White : SystemColors.Control;
            btnSua.Enabled = !sua;
            btnLuu.Enabled = sua;
            btnHuy.Enabled = sua;
            cboThamSo.Enabled = !sua;
            dgvCauHinh.Enabled = !sua;
        }

        private static Dictionary<string, ThamSo> TaoDanhSachThamSo()
        {
            var items = new[]
            {
                new ThamSo("TRA_TRUOC_TOI_THIEU", "Tỷ lệ trả trước tối thiểu", "% tổng giá trị"),
                new ThamSo("LAI_SUAT_3_THANG", "Lãi suất kỳ hạn 3 tháng", "%"),
                new ThamSo("LAI_SUAT_6_THANG", "Lãi suất kỳ hạn 6 tháng", "%"),
                new ThamSo("LAI_SUAT_9_THANG", "Lãi suất kỳ hạn 9 tháng", "%"),
                new ThamSo("LAI_SUAT_12_THANG", "Lãi suất kỳ hạn 12 tháng", "%"),
                new ThamSo("TIEN_PHAT_NGAY", "Tiền phạt trễ hạn mỗi ngày", "đồng/ngày"),
                new ThamSo("NGUONG_BAC", "Tổng chi tiêu tối thiểu lên hạng Bạc", "đồng"),
                new ThamSo("NGUONG_VANG", "Tổng chi tiêu tối thiểu lên hạng Vàng", "đồng"),
                new ThamSo("NGUONG_VIP", "Tổng chi tiêu tối thiểu lên hạng VIP", "đồng")
            };
            return items.ToDictionary(x => x.Khoa, x => x);
        }

        private sealed class ThamSo
        {
            public string Khoa { get; private set; }
            public string MoTa { get; private set; }
            public string DonVi { get; private set; }
            public ThamSo(string khoa, string moTa, string donVi) { Khoa = khoa; MoTa = moTa; DonVi = donVi; }
            public override string ToString() { return MoTa + " (" + Khoa + ")"; }
        }
    }
}

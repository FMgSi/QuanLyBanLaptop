using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;

namespace QuanLyBanLaptop.DanhMuc
{
    public partial class frmCauHinhTraGop : QuanLyBanLaptop.Classes.AppForm
    {
        protected override bool RequiresAdmin => true;
        private readonly Dictionary<string, ThamSo> danhSachThamSo = TaoDanhSachThamSo();
        private readonly DataTable bangCauHinh = new DataTable();
        private bool dangSua;

        public frmCauHinhTraGop()
        {
            InitializeComponent();
            NavigationManager.RegisterForm(NavigationMenu.CauHinh, this);
            sidebarControl1.SelectedMenu = NavigationMenu.CauHinh;
            CapNhatThongTinNguoiDung();

            dgvCauHinh.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgvCauHinh.ColumnHeadersDefaultCellStyle.BackColor;
            dgvCauHinh.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgvCauHinh.ColumnHeadersDefaultCellStyle.ForeColor;

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

            dgvCauHinh.AllowUserToResizeColumns = true;
            dgvCauHinh.ScrollBars = ScrollBars.Both;
            dgvCauHinh.AllowUserToResizeRows = false;
            dgvCauHinh.AllowUserToOrderColumns = false;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                sidebarControl1.SelectedMenu = NavigationMenu.CauHinh;
                CapNhatThongTinNguoiDung();
                TaiDuLieu();
            }
        }

        private void CapNhatThongTinNguoiDung()
        {
            string role = !string.IsNullOrEmpty(UserSession.ChucVu) ? UserSession.ChucVu : "Quản trị viên";
            string name = !string.IsNullOrEmpty(UserSession.TenNV) ? UserSession.TenNV : "Quản trị viên";
            lblRole.Text = $"{role} - {name}";
        }

        private void TaiDuLieu()
        {
            try
            {
                DataTable dt = new QuanLyBanLaptop.Services.ConfigurationService().GetAll();
                bangCauHinh.Clear();
                if (dt != null)
                {
                    bangCauHinh.Merge(dt);
                }

                dgvCauHinh.Rows.Clear();
                var addedKeys = new HashSet<string>();

                // Ưu tiên nạp theo thứ tự định nghĩa nhóm tham số chuẩn
                foreach (var ts in danhSachThamSo.Values)
                {
                    DataRow[] found = bangCauHinh.Select("Khoa = '" + ts.Khoa + "'");
                    if (found.Length > 0)
                    {
                        dgvCauHinh.Rows.Add(ts.MoTa, ts.Khoa, Convert.ToString(found[0]["GiaTri"]), ts.DonVi);
                        addedKeys.Add(ts.Khoa);
                    }
                }

                // Các tham số khác từ DB nếu chưa có trong danh sách nhóm
                foreach (DataRow row in bangCauHinh.Rows)
                {
                    string khoa = Convert.ToString(row["Khoa"]);
                    if (addedKeys.Contains(khoa)) continue;
                    string moTa = Convert.ToString(row["MoTa"]);
                    ThamSo item;
                    if (!danhSachThamSo.TryGetValue(khoa, out item))
                    {
                        item = new ThamSo(khoa, string.IsNullOrWhiteSpace(moTa) ? khoa : moTa, "");
                    }
                    dgvCauHinh.Rows.Add(item.MoTa, khoa, Convert.ToString(row["GiaTri"]), item.DonVi);
                }

                if (dgvCauHinh.Rows.Count > 0)
                {
                    dgvCauHinh.Rows[0].Selected = true;
                }
                HienThiDongDangChon();
                CapNhatThongTinNguoiDung();

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
                Function.ThongBaoCanhBao("Hãy chọn một tham số cấu hình.", "Chưa chọn tham số");
                return;
            }
            CapNhatTrangThaiSua(true);
            txtGiaTri.Focus();
            txtGiaTri.SelectAll();
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
            if (item != null)
            {
                if (item.Khoa.StartsWith("TRA_TRUOC_") && (value < 0 || value >= 100))
                {
                    Function.ThongBaoCanhBao("Tỷ lệ trả trước phải từ 0% và nhỏ hơn 100%.", "Dữ liệu không hợp lệ");
                    txtGiaTri.Focus();
                    return;
                }
                if ((item.Khoa.StartsWith("LAI_SUAT_") || item.Khoa.StartsWith("GIAM_GIA_")) && (value < 0 || value > 100))
                {
                    Function.ThongBaoCanhBao("Tỷ lệ phần trăm phải nằm trong khoảng từ 0 đến 100%.", "Dữ liệu không hợp lệ");
                    txtGiaTri.Focus();
                    return;
                }
                if ((item.Khoa.StartsWith("NGUONG_") || item.Khoa == "TIEN_PHAT_NGAY") && value < 0)
                {
                    Function.ThongBaoCanhBao("Số tiền không được âm.", "Dữ liệu không hợp lệ");
                    txtGiaTri.Focus();
                    return;
                }
            }

            try
            {
                new QuanLyBanLaptop.Services.ConfigurationService().Save(txtKhoa.Text.Trim(), value);

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
                new ThamSo("TRA_TRUOC_3_THANG", "Tỷ lệ trả trước kỳ hạn 3 tháng", "% tổng giá trị"),
                new ThamSo("TRA_TRUOC_6_THANG", "Tỷ lệ trả trước kỳ hạn 6 tháng", "% tổng giá trị"),
                new ThamSo("TRA_TRUOC_9_THANG", "Tỷ lệ trả trước kỳ hạn 9 tháng", "% tổng giá trị"),
                new ThamSo("TRA_TRUOC_12_THANG", "Tỷ lệ trả trước kỳ hạn 12 tháng", "% tổng giá trị"),
                new ThamSo("LAI_SUAT_3_THANG", "Lãi suất kỳ hạn 3 tháng", "%"),
                new ThamSo("LAI_SUAT_6_THANG", "Lãi suất kỳ hạn 6 tháng", "%"),
                new ThamSo("LAI_SUAT_9_THANG", "Lãi suất kỳ hạn 9 tháng", "%"),
                new ThamSo("LAI_SUAT_12_THANG", "Lãi suất kỳ hạn 12 tháng", "%"),
                new ThamSo("GIAM_GIA_DONG", "Giảm giá thành viên hạng Đồng", "%"),
                new ThamSo("GIAM_GIA_BAC", "Giảm giá thành viên hạng Bạc", "%"),
                new ThamSo("GIAM_GIA_VANG", "Giảm giá thành viên hạng Vàng", "%"),
                new ThamSo("GIAM_GIA_VIP", "Giảm giá thành viên hạng VIP", "%"),
                new ThamSo("NGUONG_BAC", "Tổng chi tiêu tối thiểu lên hạng Bạc", "đồng"),
                new ThamSo("NGUONG_VANG", "Tổng chi tiêu tối thiểu lên hạng Vàng", "đồng"),
                new ThamSo("NGUONG_VIP", "Tổng chi tiêu tối thiểu lên hạng VIP", "đồng"),
                new ThamSo("TIEN_PHAT_NGAY", "Tiền phạt trễ hạn mỗi ngày", "đồng/ngày")
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

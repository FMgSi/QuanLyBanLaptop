using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QuanLyBanLaptop.Classes
{
    // Lớp tiện ích trung tâm gom tất cả các hàm dùng nhiều trong toàn bộ hệ thống (CSDL, Định dạng tiền/ngày, Kiểm tra dữ liệu, Sinh mã, Thông báo, Định dạng bảng, Xuất file). Các form sau này chỉ việc gọi Function.<TênHàm> mà không cần viết lại logic.
    public static class Function
    {
        #region 1. Thông tin phiên đăng nhập người dùng

        // Mã nhân viên đang đăng nhập.
        public static string CurrentMaNV
        {
            get => UserSession.MaNV;
            set => UserSession.MaNV = value;
        }

        // Tên nhân viên đang đăng nhập.
        public static string CurrentTenNV
        {
            get => UserSession.TenNV;
            set => UserSession.TenNV = value;
        }

        // Chức vụ của nhân viên đang đăng nhập.
        public static string CurrentChucVu
        {
            get => UserSession.ChucVu;
            set => UserSession.ChucVu = value;
        }

        // Nhân viên hiện tại có phải Quản trị viên / Quản lý không.
        public static bool IsAdmin =>
            CurrentChucVu == "Quản trị viên" || CurrentChucVu == "Quản lý";

        // Gán thông tin phiên đăng nhập từ DataRow.
        public static void SetCurrentUser(DataRow row)
        {
            if (row == null) return;
            CurrentMaNV = row["MaNV"]?.ToString();
            CurrentTenNV = row["TenNV"]?.ToString();
            CurrentChucVu = row["ChucVu"]?.ToString();
            if (row.Table.Columns.Contains("TenDN"))
            {
                UserSession.TenDN = row["TenDN"]?.ToString();
            }
        }

        // Xóa thông tin phiên đăng nhập khi đăng xuất.
        public static void ClearCurrentUser()
        {
            CurrentMaNV = null;
            CurrentTenNV = null;
            CurrentChucVu = null;
            UserSession.TenDN = null;
        }

        #endregion

        #region 2. Các hàm truy vấn và tương tác CSDL (gọi nhanh qua Function)

        // Đọc bảng dữ liệu với danh sách SqlParameter.
        public static DataTable DocBang(string sql, params SqlParameter[] parameters)
            => DataProcesser.ReadData(sql, parameters);

        // Đọc bảng dữ liệu với tham số vị trí @0, @1, @2...
        public static DataTable DocBang(string sql, params object[] values)
            => DataProcesser.ReadData(sql, values);

        // Thực thi lệnh INSERT, UPDATE, DELETE với danh sách SqlParameter.
        public static int CapNhat(string sql, params SqlParameter[] parameters)
            => DataProcesser.ChangeData(sql, parameters);

        // Thực thi lệnh INSERT, UPDATE, DELETE với tham số vị trí @0, @1, @2...
        public static int CapNhat(string sql, params object[] values)
            => DataProcesser.ChangeData(sql, values);

        // Lấy 1 giá trị đơn (COUNT, SUM, MAX...) với danh sách SqlParameter.
        public static object LayGiaTri(string sql, params SqlParameter[] parameters)
            => DataProcesser.ExecuteScalar(sql, parameters);

        // Lấy 1 giá trị đơn với tham số vị trí @0, @1, @2...
        public static object LayGiaTri(string sql, params object[] values)
            => DataProcesser.ExecuteScalar(sql, values);

        // Thực thi Stored Procedure trả về DataTable.
        public static DataTable ExecProc(string procName, params SqlParameter[] parameters)
            => DataProcesser.ExecProc(procName, parameters);

        // Thực thi Stored Procedure không trả về kết quả (Insert/Update/Delete).
        public static void ExecProcNonQuery(string procName, params SqlParameter[] parameters)
            => DataProcesser.ExecProcNonQuery(procName, parameters);

        // Nạp dữ liệu vào ComboBox chỉ với 1 dòng lệnh.
        public static void FillComboBox(ComboBox cbo, string sql, string displayMember, string valueMember, string defaultItemText = null)
            => DataProcesser.FillComboBox(cbo, sql, displayMember, valueMember, defaultItemText);

        // Kiểm tra kết nối tới cơ sở dữ liệu.
        public static bool TestKetNoi()
            => DataProcesser.TestConnection();

        #endregion

        #region 3. Định dạng và xử lý Tiền tệ, Ngày tháng, Số

        // Định dạng số tiền kiểu Việt Nam: "1.500.000 ₫".
        public static string FormatTien(decimal amount)
            => amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " ₫";

        // Bí danh cho FormatTien.
        public static string FormatCurrency(decimal amount)
            => FormatTien(amount);

        // Chuyển đổi chuỗi tiền tệ (vd: "1.500.000 ₫", "1500000", "1,500,000") về số decimal an toàn.
        public static decimal ParseTien(string text, decimal defaultValue = 0)
        {
            if (string.IsNullOrWhiteSpace(text)) return defaultValue;
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (char.IsDigit(c))
                {
                    sb.Append(c);
                }
            }
            if (sb.Length > 0 && decimal.TryParse(sb.ToString(), out decimal res))
            {
                return text.TrimStart().StartsWith("-") ? -res : res;
            }
            return defaultValue;
        }

        // Định dạng ngày tháng theo chuẩn Việt Nam: dd/MM/yyyy.
        public static string FormatNgay(DateTime dt)
            => dt.ToString("dd/MM/yyyy");

        // Định dạng ngày và giờ: dd/MM/yyyy HH:mm.
        public static string FormatNgayGio(DateTime dt)
            => dt.ToString("dd/MM/yyyy HH:mm");

        // Parse chuỗi ngày (dd/MM/yyyy, yyyy-MM-dd...) về DateTime an toàn.
        public static DateTime? ParseNgay(string dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr)) return null;
            string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "dd-MM-yyyy" };
            if (DateTime.TryParseExact(dateStr.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                return dt;
            }
            if (DateTime.TryParse(dateStr.Trim(), out dt)) return dt;
            return null;
        }

        // Đọc số tiền thành chữ tiếng Việt chuẩn (VD: 1.500.000 -> Một triệu năm trăm nghìn đồng).
        public static string ChuyenSoThanhChu(decimal number)
        {
            if (number == 0) return "Không đồng";
            if (number < 0) return "Âm " + ChuyenSoThanhChu(Math.Abs(number));

            long n = (long)Math.Round(number);
            if (n == 0) return "Không đồng";

            string sNumber = n.ToString();
            int len = sNumber.Length;
            int rem = len % 3;
            if (rem > 0)
                sNumber = sNumber.PadLeft(len + (3 - rem), '0');

            string[] chuSo = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] dvTien = { "", "nghìn", "triệu", "tỷ", "nghìn tỷ", "triệu tỷ" };

            int numGroups = sNumber.Length / 3;
            string result = "";

            for (int i = 0; i < numGroups; i++)
            {
                string group = sNumber.Substring(i * 3, 3);
                int tram = group[0] - '0';
                int chuc = group[1] - '0';
                int donvi = group[2] - '0';

                if (tram == 0 && chuc == 0 && donvi == 0)
                    continue;

                string groupStr = "";
                if (i == 0 && tram == 0)
                {
                    if (chuc == 1)
                        groupStr += "mười ";
                    else if (chuc > 1)
                        groupStr += chuSo[chuc] + " mươi ";

                    if (chuc > 0 && donvi == 1 && chuc != 1)
                        groupStr += "mốt ";
                    else if (chuc > 0 && donvi == 5)
                        groupStr += "lăm ";
                    else if (donvi > 0)
                        groupStr += chuSo[donvi] + " ";
                }
                else
                {
                    groupStr += chuSo[tram] + " trăm ";

                    if (chuc == 0 && donvi != 0)
                        groupStr += "lẻ ";
                    else if (chuc == 1)
                        groupStr += "mười ";
                    else if (chuc > 1)
                        groupStr += chuSo[chuc] + " mươi ";

                    if (chuc > 0 && donvi == 1 && chuc != 1)
                        groupStr += "mốt ";
                    else if (chuc > 0 && donvi == 5)
                        groupStr += "lăm ";
                    else if (donvi > 0)
                        groupStr += chuSo[donvi] + " ";
                }

                int groupLevel = numGroups - 1 - i;
                if (groupLevel >= 0 && groupLevel < dvTien.Length && !string.IsNullOrEmpty(dvTien[groupLevel]))
                    groupStr += dvTien[groupLevel] + " ";

                result += groupStr;
            }

            result = result.Trim();
            if (string.IsNullOrEmpty(result)) return "Không đồng";
            result = char.ToUpper(result[0]) + result.Substring(1) + " đồng";
            while (result.Contains("  ")) result = result.Replace("  ", " ");
            return result;
        }

        #endregion

        #region 4. Sinh ma tu dong thong minh

        // sinh ma tu dong theo tien to va so chu so dinh dang vd NV01, KH01, MH01
        // Cấp mã từ sequence DB để không trùng khi nhiều máy cùng thêm dữ liệu.
        public static string SinhMaTuDong(string tenBang, string tenCot, string tienTo, int doRong = 2)
        {
            string sequence;
            switch (tenBang + "." + tenCot)
            {
                case "LAPTOP.MaLaptop": sequence = "seq_AppLaptop"; break;
                case "NHA_CUNG_CAP.MaNCC": sequence = "seq_AppSupplier"; break;
                case "KHACH_HANG.MaKH": sequence = "seq_AppCustomer"; break;
                case "NHAN_VIEN.MaNV": sequence = "seq_AppEmployee"; break;
                default: throw new ArgumentException("Bảng sinh mã chưa được hỗ trợ.");
            }
            long next = 1;
            try
            {
                object val = DataProcesser.ExecuteScalar("SELECT NEXT VALUE FOR dbo." + sequence);
                if (val != null) next = Convert.ToInt64(val);
            }
            catch
            {
                next = 1;
            }
            string id = tienTo + next.ToString("D" + doRong);
            if (id.Length > 10) throw new InvalidOperationException("Mã vượt giới hạn 10 ký tự.");
            return id;
        }

        #endregion

        #region 5. Kiểm tra tính hợp lệ dữ liệu (Validation)

        // Kiểm tra chuỗi có bị rỗng hay không.
        public static bool KiemTraRong(string text, string tenTruong, out string thongBaoLoi)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                thongBaoLoi = $"Vui lòng nhập {tenTruong}!";
                return false;
            }
            thongBaoLoi = string.Empty;
            return true;
        }

        // Kiểm tra số điện thoại Việt Nam hợp lệ (10 chữ số, bắt đầu bằng 03, 05, 07, 08, 09...).
        public static bool KiemTraSDT(string sdt)
        {
            if (string.IsNullOrWhiteSpace(sdt)) return false;
            return Regex.IsMatch(sdt.Trim(), @"^(0[3|5|7|8|9])[0-9]{8}$");
        }

        // Kiểm tra số nguyên hợp lệ trong khoảng min đến max.
        public static bool KiemTraSoNguyen(string text, string tenTruong, out int ketQua, int min = 1, int max = int.MaxValue)
        {
            ketQua = 0;
            if (!int.TryParse(text?.Trim(), out ketQua) || ketQua < min || ketQua > max)
            {
                ThongBaoCanhBao($"{tenTruong} phải là số nguyên từ {min} đến {max}!");
                return false;
            }
            return true;
        }

        // Kiểm tra số thực (tiền, tỷ lệ) hợp lệ trong khoảng min đến max.
        public static bool KiemTraSoThuc(string text, string tenTruong, out decimal ketQua, decimal min = 0, decimal max = decimal.MaxValue)
        {
            ketQua = 0;
            decimal parsed = ParseTien(text, -1);
            if (parsed < min || parsed > max)
            {
                ThongBaoCanhBao($"{tenTruong} phải là số hợp lệ từ {min} đến {max}!");
                return false;
            }
            ketQua = parsed;
            return true;
        }

        #endregion

        #region 6. Hộp thoại thông báo & Xác nhận

        // Hiển thị hộp thoại lỗi.
        public static void ThongBaoLoi(string msg, string title = "Lỗi")
            => MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

        // Hiển thị hộp thoại thành công.
        public static void ThongBaoThanhCong(string msg, string title = "Thành công")
            => MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

        // Hiển thị hộp thoại cảnh báo.
        public static void ThongBaoCanhBao(string msg, string title = "Cảnh báo")
            => MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        // Hiển thị hộp thoại xác nhận Yes / No.
        public static DialogResult XacNhan(string msg, string title = "Xác nhận")
            => MessageBox.Show(msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        #endregion

        #region 7. Tiện ích giao diện DataGridView & Xuất báo cáo

        // Áp dụng bảng màu và kiểu hiển thị chuyên nghiệp, hiện đại cho DataGridView.
        public static void DinhDangBang(DataGridView dgv)
        {
            if (dgv == null) return;

            dgv.EnableHeadersVisualStyles = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(241, 245, 249); // slate-100

            // Header
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252); // slate-50
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(15, 23, 42);    // slate-900
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersHeight = 38;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Rows
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);       // slate-700
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255); // blue-50
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(29, 78, 216);  // blue-700
            dgv.RowTemplate.Height = 36;

            // Alternating Rows
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252); // slate-50
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(29, 78, 216);

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.ReadOnly = true;
        }

        // Xuất dữ liệu DataGridView ra file CSV / Excel UTF-8 kèm BOM, tương thích tiếng Việt có dấu.
        public static void XuatExcel(DataGridView dgv, string defaultFileName = "DanhSach.csv")
        {
            if (dgv == null || dgv.Rows.Count == 0)
            {
                ThongBaoCanhBao("Không có dữ liệu trong bảng để xuất!");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "File Excel CSV (*.csv)|*.csv|All Files (*.*)|*.*";
                sfd.FileName = defaultFileName;
                sfd.Title = "Chọn nơi lưu file xuất dữ liệu";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var sb = new StringBuilder();

                        // Header
                        var headers = new System.Collections.Generic.List<string>();
                        foreach (DataGridViewColumn col in dgv.Columns)
                        {
                            if (col.Visible)
                            {
                                headers.Add($"\"{col.HeaderText.Replace("\"", "\"\"")}\"");
                            }
                        }
                        sb.AppendLine(string.Join(",", headers));

                        // Rows
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (row.IsNewRow) continue;
                            var cells = new System.Collections.Generic.List<string>();
                            foreach (DataGridViewColumn col in dgv.Columns)
                            {
                                if (col.Visible)
                                {
                                    string val = row.Cells[col.Index].Value?.ToString() ?? "";
                                    cells.Add($"\"{val.Replace("\"", "\"\"")}\"");
                                }
                            }
                            sb.AppendLine(string.Join(",", cells));
                        }

                        // Ghi với UTF8 kèm BOM để Excel hiển thị đúng dấu tiếng Việt
                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);

                        if (XacNhan($"Xuất dữ liệu thành công:\n{sfd.FileName}\n\nBạn có muốn mở file ngay không?") == DialogResult.Yes)
                        {
                            Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                        }
                    }
                    catch (Exception ex)
                    {
                        ThongBaoLoi("Lỗi khi xuất file: " + ex.Message);
                    }
                }
            }
        }

        #endregion

        #region 8. Cac ham tien ich dung chung cho form (CRUD va giao dien)

        // thiet lap trang thai cac nut them sua xoa luu bo qua
        public static void DatTrangThaiNut(bool dangSua, Button btnThem, Button btnSua, Button btnXoa, Button btnLuu, Button btnBoQua, Button btnKhoa = null)
        {
            if (btnThem != null) btnThem.Enabled = !dangSua;
            if (btnSua != null) btnSua.Enabled = !dangSua;
            if (btnXoa != null) btnXoa.Enabled = !dangSua;
            if (btnKhoa != null) btnKhoa.Enabled = !dangSua;
            if (btnLuu != null) btnLuu.Enabled = dangSua;
            if (btnBoQua != null) btnBoQua.Enabled = dangSua;
        }

        // xoa trang toan bo o nhap lieu trong container
        public static void XoaTrangControl(Control container)
        {
            if (container == null) return;
            foreach (Control c in container.Controls)
            {
                if (c is TextBox tb) tb.Clear();
                else if (c is ComboBox cbo && cbo.Items.Count > 0) cbo.SelectedIndex = 0;
                else if (c is DateTimePicker dtp) dtp.Value = DateTime.Now;
                else if (c is RadioButton rb) rb.Checked = false;
                else if (c.HasChildren) XoaTrangControl(c);
            }
        }

        // khoa hoac mo khoa cac control nhap lieu
        public static void DatKhoaControl(bool khoa, params Control[] controls)
        {
            if (controls == null) return;
            foreach (var c in controls)
            {
                if (c is TextBox tb) tb.ReadOnly = khoa;
                else if (c != null) c.Enabled = !khoa;
            }
        }

        // gan tieu de tieng viet cho cot trong datagridview
        public static void DatTieuDeCot(DataGridView dgv, string tenCot, string tieuDe, int doRong = 0)
        {
            if (dgv != null && dgv.Columns.Contains(tenCot))
            {
                dgv.Columns[tenCot].HeaderText = tieuDe;
                if (doRong > 0) dgv.Columns[tenCot].Width = doRong;
            }
        }

        // lay gia tri chuoi an toan tu cell trong datagridview
        public static string LayGiaTriCell(DataGridViewRow row, string tenCot, string macDinh = "")
        {
            if (row == null || !row.DataGridView.Columns.Contains(tenCot)) return macDinh;
            return row.Cells[tenCot]?.Value?.ToString() ?? macDinh;
        }

        // sinh ma hoa don tu dong theo quy tac HDB_ddMMyyyy0xxx
        // Dùng namespace mã mới và sequence nguyên tử, không phụ thuộc đồng hồ máy khách.
        public static string SinhMaHoaDonTuDong()
        {
            long next = Convert.ToInt64(DataProcesser.ExecuteScalar("SELECT NEXT VALUE FOR dbo.seq_AppInvoice"));
            return $"HDB_{DateTime.Now:ddMMyyyy}{next:D4}";
        }

        #endregion
    }
}


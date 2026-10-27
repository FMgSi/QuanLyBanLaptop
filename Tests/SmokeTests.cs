using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.Services;

public static class SmokeTests
{
    private static int checks;
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
        checks++;
    }

    private static void ClickButton(Button btn)
    {
        var m = typeof(Button).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        m.Invoke(btn, new object[] { EventArgs.Empty });
    }

    [STAThread]
    public static void Main()
    {
        try
        {
            string connection = Environment.GetEnvironmentVariable("LAPTOP_TEST_CONNECTION");
            if (string.IsNullOrWhiteSpace(connection)) throw new Exception("Run Tests/Run-Tests.ps1 with an isolated test database.");
            DataProcesser.ConnectionString = connection;
            foreach (int months in new[] { 3, 6, 9, 12 })
            foreach (decimal total in new[] { 10000000m, 19999999m, 10000005m })
        {
            var plan = InstallmentCalculator.Calculate(total, months);
            decimal payable = plan.Principal + plan.Interest;
            decimal last = payable - plan.Monthly * (months - 1);
            Check(plan.DownPayment + plan.Principal == total, "Principal conservation");
            Check(plan.Monthly * (months - 1) + last == payable, "Installments must sum exactly");
            Check(last >= plan.Monthly, "Rounding remainder belongs to last installment");
        }
        var known = InstallmentCalculator.Calculate(10000000m, 3);
        Check(known.DownPayment == 3000000m && known.Interest == 168000m, "Known installment example");
        var legacy = InstallmentCalculator.Calculate(10000000m, 6, 0, 0);
        Check(legacy.DownPayment == 0 && legacy.Interest == 0, "Legacy contract preservation");
        bool rejected = false;
        try { InstallmentCalculator.Calculate(100m, 0); } catch (ArgumentException) { rejected = true; }
        Check(rejected, "Invalid term rejected");
        rejected = false;
        try { new SaleService().Save(new SaleRequest { Items = new DataTable() }); }
        catch (ArgumentException) { rejected = true; }
        Check(rejected, "Empty cart rejected without opening database");

        using (var form = new Form())
        using (var panel = new Panel())
        using (var enabled = new Button { Text = "Them", Size = new Size(85, 27) })
        using (var disabled = new Button { Text = "Xoa", Enabled = false })
        {
            form.Controls.Add(panel);
            panel.Controls.Add(enabled);
            panel.Controls.Add(disabled);
            ButtonTheme.ApplyAll(form);
            Check(enabled.BackColor == Color.White, "White action button");
            Check(enabled.FlatStyle == FlatStyle.Flat && enabled.FlatAppearance.BorderSize == 1, "Thin flat border");
            Check(enabled.Font.Name == "Segoe UI" && enabled.Font.Size == 9F, "Reference button font");
            Check(!disabled.Enabled, "Theme must preserve disabled state");
            Check(enabled.Size == new Size(85, 27), "Theme must preserve layout");
        }
        using (var sb = new QuanLyBanLaptop.SidebarControl())
        {
            ButtonTheme.ApplyAll(sb);
            var homeBtn = sb.Controls.Find("btnTrangChu", true).FirstOrDefault() as Button;
            Check(homeBtn != null, "Sidebar home button found");
            Check(homeBtn.BackColor == SidebarTheme.SidebarActive, "Active sidebar menu has royal blue background");
            Check(homeBtn.BackColor != Color.White, "Sidebar button must not be turned white by ButtonTheme");
            Check(homeBtn.ForeColor == Color.White, "Sidebar button text is white");

            UserSession.ChucVu = "Quản trị viên";
            Check(sb.Controls.Find("btnNhanVien", true).First().Visible, "Admin sees btnNhanVien");
            Check(sb.Controls.Find("btnThongKe", true).First().Visible, "Admin sees btnThongKe");
            Check(sb.Controls.Find("btnCauHinh", true).First().Visible, "Admin sees btnCauHinh");

            UserSession.ChucVu = "Nhân viên";
            Check(!sb.Controls.Find("btnNhanVien", true).First().Visible, "Non-admin hides btnNhanVien");
            Check(!sb.Controls.Find("btnThongKe", true).First().Visible, "Non-admin hides btnThongKe");
            Check(!sb.Controls.Find("btnCauHinh", true).First().Visible, "Non-admin hides btnCauHinh");
            UserSession.ChucVu = "Quản trị viên";
        }

        var forms = typeof(AppForm).Assembly.GetTypes().Where(t => t.Name.StartsWith("frm") && typeof(Form).IsAssignableFrom(t));
        Check(forms.All(t => typeof(AppForm).IsAssignableFrom(t)), "All forms must inherit shared theme");

        using (var qlTraGop = new QuanLyBanLaptop.HoaDon.frmQuanLyTraGop())
        {
            var fieldBtn = typeof(QuanLyBanLaptop.HoaDon.frmQuanLyTraGop).GetField("btnCauHinh", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Check(fieldBtn == null, "btnCauHinh removed from frmQuanLyTraGop");
            var split = qlTraGop.Controls.Find("pnlSplit", true).FirstOrDefault() as SplitContainer;
            Check(split != null && split.IsSplitterFixed, "pnlSplit must be fixed to prevent dragging");
            var dgvHopDong = qlTraGop.Controls.Find("dgvHopDong", true).FirstOrDefault() as DataGridView;
            Check(dgvHopDong != null && dgvHopDong.AllowUserToResizeColumns && !dgvHopDong.AllowUserToOrderColumns, "dgvHopDong must allow column resizing and not allow column reordering");
            var dgvLich = qlTraGop.Controls.Find("dgvLichTraGop", true).FirstOrDefault() as DataGridView;
            Check(dgvLich != null && dgvLich.AllowUserToResizeColumns && !dgvLich.AllowUserToOrderColumns, "dgvLichTraGop must allow column resizing and not allow column reordering");
            Check(qlTraGop.FormBorderStyle == FormBorderStyle.FixedSingle && !qlTraGop.MaximizeBox, "frmQuanLyTraGop must have fixed single border without maximize box");
        }

        using (var main = new QuanLyBanLaptop.frmMain())
        {
            Check(main.FormBorderStyle == FormBorderStyle.FixedSingle, "frmMain must have fixed single border");
            Check(main.WindowState == FormWindowState.Maximized, "frmMain must automatically open maximized / full screen");
        }

        using (var thongKe = new QuanLyBanLaptop.Thongke.frmThongKe())
        {
            var chart = thongKe.Controls.Find("chartDoanhthu", true).FirstOrDefault() as System.Windows.Forms.DataVisualization.Charting.Chart;
            Check(chart != null, "chartDoanhthu must exist in frmThongKe");
            var dgv = thongKe.Controls.Find("dgvThongKe", true).FirstOrDefault() as DataGridView;
            Check(dgv != null && dgv.AllowUserToResizeColumns && !dgv.AllowUserToResizeRows && !dgv.AllowUserToOrderColumns, "dgvThongKe must allow resizing columns and prevent row resize/order");
            var btnIn = thongKe.Controls.Find("btnInBaoCao", true).FirstOrDefault() as Button;
            Check(btnIn != null, "btnInBaoCao must exist in frmThongKe");
            Check(btnIn.Visible == false, "btnInBaoCao must be hidden in frmThongKe");
            var btnExcel = thongKe.Controls.Find("btnXuatExcel", true).FirstOrDefault() as Button;
            Check(btnExcel != null, "btnXuatExcel must exist in frmThongKe");
        }

        using (var cauHinh = new QuanLyBanLaptop.DanhMuc.frmCauHinhTraGop())
        {
            Check(cauHinh.Controls.Find("sidebarControl1", true).Length > 0, "frmCauHinhTraGop must contain sidebarControl1");
            Check(cauHinh.FormBorderStyle == FormBorderStyle.FixedSingle, "frmCauHinhTraGop must be FixedSingle");
            var dgvConfig = cauHinh.Controls.Find("dgvCauHinh", true).FirstOrDefault() as DataGridView;
            Check(dgvConfig != null && dgvConfig.AllowUserToResizeColumns && !dgvConfig.AllowUserToResizeRows && !dgvConfig.AllowUserToOrderColumns, "dgvCauHinh must allow column resizing and prevent row resize/order");
        }

        using (var baoHanh = new QuanLyBanLaptop.DanhMuc.frmBaoHanhSuaChua())
        {
            Check(typeof(AppForm).IsAssignableFrom(typeof(QuanLyBanLaptop.DanhMuc.frmBaoHanhSuaChua)), "frmBaoHanhSuaChua must inherit AppForm");
            var dgvBH = baoHanh.Controls.Find("dgvBaoHanh", true).FirstOrDefault() as DataGridView;
            Check(dgvBH != null && dgvBH.AllowUserToResizeColumns && !dgvBH.AllowUserToResizeRows && !dgvBH.AllowUserToOrderColumns, "dgvBaoHanh must allow resizing columns and prevent row resize/order");
            Check(dgvBH.ColumnHeadersDefaultCellStyle.SelectionBackColor == dgvBH.ColumnHeadersDefaultCellStyle.BackColor &&
                dgvBH.ColumnHeadersDefaultCellStyle.SelectionForeColor == dgvBH.ColumnHeadersDefaultCellStyle.ForeColor,
                "Selecting a ticket preserves column header colors");
            Check(!baoHanh.Controls.Find("pnlFormContainer", true).First().Visible, "All tab hides ticket detail panel");
            var btnThem = baoHanh.Controls.Find("btnThem", true).FirstOrDefault() as Button;
            var btnSua = baoHanh.Controls.Find("btnSua", true).FirstOrDefault() as Button;
            var btnXoa = baoHanh.Controls.Find("btnXoa", true).FirstOrDefault() as Button;
            var btnLuu = baoHanh.Controls.Find("btnLuu", true).FirstOrDefault() as Button;
            var btnHuy = baoHanh.Controls.Find("btnHuy", true).FirstOrDefault() as Button;
            var btnThoat = baoHanh.Controls.Find("btnThoat", true).FirstOrDefault() as Button;
            Check(btnThem != null && !btnThem.Enabled, "All tickets tab is read-only");
            Check(btnThoat != null && btnThoat.Enabled, "frmBaoHanhSuaChua btnThoat initially enabled");
            Check(btnSua != null && !btnSua.Enabled, "frmBaoHanhSuaChua btnSua initially disabled");
            Check(btnLuu != null && !btnLuu.Enabled, "frmBaoHanhSuaChua btnLuu initially disabled");
            Check(btnHuy != null && !btnHuy.Enabled, "frmBaoHanhSuaChua btnHuy initially disabled");
            ClickButton(btnThem);
            Check(!btnLuu.Enabled, "All tab rejects add even when handler is invoked directly");
            ClickButton((Button)baoHanh.Controls.Find("btnTabWarranty", true).First());
            Check(btnThem.Enabled, "Warranty tab supports creating a ticket");
            ClickButton(btnThem);
            Check(!btnThem.Enabled, "frmBaoHanhSuaChua btnThem disabled after click");
            Check(btnLuu.Enabled, "frmBaoHanhSuaChua btnLuu enabled after Them click");
            Check(btnHuy.Enabled, "frmBaoHanhSuaChua btnHuy enabled after Them click");
            ClickButton(btnHuy);
            Check(btnThem.Enabled && !btnLuu.Enabled && !btnHuy.Enabled, "frmBaoHanhSuaChua restored to initial state after Huy click");
            ClickButton(btnThem);
            ClickButton((Button)baoHanh.Controls.Find("btnTabAll", true).First());
            Check(!btnThem.Enabled && !btnLuu.Enabled && !btnSua.Enabled && !btnXoa.Enabled,
                "Switching from draft to all restores read-only mode");
            Check(((TextBox)baoHanh.Controls.Find("txtLoiDichVu",true).First()).ReadOnly,
                "Ticket details are read-only in all tab");

            var btnInP = baoHanh.Controls.Find("btnInPhieu", true).FirstOrDefault();
            Check(btnInP != null, "btnInPhieu must exist in frmBaoHanhSuaChua");
            Check(btnInP.Visible == false, "btnInPhieu must be hidden in frmBaoHanhSuaChua");
            Check(baoHanh.Controls.Find("btnXuatExcel", true).FirstOrDefault() != null, "btnXuatExcel must exist in frmBaoHanhSuaChua");
            Check(baoHanh.Controls.Find("btnChonSanPham", true).FirstOrDefault() != null, "btnChonSanPham must exist in frmBaoHanhSuaChua");
        }

        using (var kh = new QuanLyBanLaptop.DanhMuc.frmKhachHang())
        {
            var btnThem = kh.Controls.Find("btnThem", true).FirstOrDefault() as Button;
            var btnSua = kh.Controls.Find("btnSua", true).FirstOrDefault() as Button;
            var btnLuu = kh.Controls.Find("btnLuu", true).FirstOrDefault() as Button;
            var btnHuy = kh.Controls.Find("btnHuy", true).FirstOrDefault() as Button;
            var btnThoat = kh.Controls.Find("btnThoat", true).FirstOrDefault() as Button;
            Check(btnThem != null && btnThem.Enabled, "frmKhachHang btnThem initially enabled");
            Check(btnThoat != null && btnThoat.Enabled, "frmKhachHang btnThoat initially enabled");
            Check(btnSua != null && !btnSua.Enabled, "frmKhachHang btnSua initially disabled");
            Check(btnLuu != null && !btnLuu.Enabled, "frmKhachHang btnLuu initially disabled");
            Check(btnHuy != null && !btnHuy.Enabled, "frmKhachHang btnHuy initially disabled");
            ClickButton(btnThem);
            Check(!btnThem.Enabled, "frmKhachHang btnThem disabled after click");
            Check(btnLuu.Enabled, "frmKhachHang btnLuu enabled after Them click");
            Check(btnHuy.Enabled, "frmKhachHang btnHuy enabled after Them click");
            ClickButton(btnHuy);
            Check(btnThem.Enabled && !btnLuu.Enabled && !btnHuy.Enabled, "frmKhachHang restored to initial state after Huy click");
        }

        using (var nv = new QuanLyBanLaptop.DanhMuc.frmNhanVien())
        {
            var btnThem = nv.Controls.Find("btnThem", true).FirstOrDefault() as Button;
            var btnSua = nv.Controls.Find("btnSua", true).FirstOrDefault() as Button;
            var btnLuu = nv.Controls.Find("btnLuu", true).FirstOrDefault() as Button;
            var btnHuy = nv.Controls.Find("btnHuy", true).FirstOrDefault() as Button;
            var btnThoat = nv.Controls.Find("btnThoat", true).FirstOrDefault() as Button;
            Check(btnThem != null && btnThem.Enabled, "frmNhanVien btnThem initially enabled");
            Check(btnThoat != null && btnThoat.Enabled, "frmNhanVien btnThoat initially enabled");
            Check(btnSua != null && !btnSua.Enabled, "frmNhanVien btnSua initially disabled");
            Check(btnLuu != null && !btnLuu.Enabled, "frmNhanVien btnLuu initially disabled");
            Check(btnHuy != null && !btnHuy.Enabled, "frmNhanVien btnHuy initially disabled");
            ClickButton(btnThem);
            Check(!btnThem.Enabled, "frmNhanVien btnThem disabled after click");
            Check(btnLuu.Enabled, "frmNhanVien btnLuu enabled after Them click");
            Check(btnHuy.Enabled, "frmNhanVien btnHuy enabled after Them click");
            ClickButton(btnHuy);
            Check(btnThem.Enabled && !btnLuu.Enabled && !btnHuy.Enabled, "frmNhanVien restored to initial state after Huy click");
        }

        using (var sp = new QuanLyBanLaptop.DanhMuc.frmSanPham())
        {
            var btnThem = sp.Controls.Find("btnThem", true).FirstOrDefault() as Button;
            var btnSua = sp.Controls.Find("btnSua", true).FirstOrDefault() as Button;
            var btnLuu = sp.Controls.Find("btnLuu", true).FirstOrDefault() as Button;
            var btnHuy = sp.Controls.Find("btnHuy", true).FirstOrDefault() as Button;
            var btnThoat = sp.Controls.Find("btnThoat", true).FirstOrDefault() as Button;
            Check(btnThem != null && btnThem.Enabled, "frmSanPham btnThem initially enabled");
            Check(btnThoat != null && btnThoat.Enabled, "frmSanPham btnThoat initially enabled");
            Check(btnSua != null && !btnSua.Enabled, "frmSanPham btnSua initially disabled");
            Check(btnLuu != null && !btnLuu.Enabled, "frmSanPham btnLuu initially disabled");
            Check(btnHuy != null && !btnHuy.Enabled, "frmSanPham btnHuy initially disabled");
            ClickButton(btnThem);
            Check(!btnThem.Enabled, "frmSanPham btnThem disabled after click");
            Check(btnLuu.Enabled, "frmSanPham btnLuu enabled after Them click");
            Check(btnHuy.Enabled, "frmSanPham btnHuy enabled after Them click");
            ClickButton(btnHuy);
            Check(btnThem.Enabled && !btnLuu.Enabled && !btnHuy.Enabled, "frmSanPham restored to initial state after Huy click");
        }

        Check(InstallmentCalculator.RateFor(3) > 0, "RateFor must return valid positive interest rate");
        using (var exportGrid = new DataGridView { AllowUserToAddRows = false })
        {
            exportGrid.Columns.Add("Id", "Mã phiếu");
            exportGrid.Columns.Add("Detail", "Lỗi / Dịch vụ");
            exportGrid.Columns.Add("Hidden", "Ẩn");
            exportGrid.Columns["Hidden"].Visible = false;
            exportGrid.Columns["Detail"].DisplayIndex = 0;
            exportGrid.Rows.Add("001", "Bảo hành <máy> & \"pin\"", "không xuất");
            var exported = GridExcelExporter.ReadGrid(exportGrid);
            Check(exported.Rows.Count == 1 && exported.Columns.Count == 2 && exported.Columns[0].Caption == "Lỗi / Dịch vụ",
                "Export follows displayed columns and excludes hidden data");
            string html = GridExcelExporter.BuildHtml(exported, "Danh sách bảo hành");
            Check(System.Net.WebUtility.HtmlDecode(html).Contains("Bảo hành <máy> & \"pin\"") &&
                html.Contains("&lt;") && html.Contains("001") && !html.Contains("không xuất"),
                "Excel fallback preserves Vietnamese, leading zeros and escapes markup");
        }
        Check(InstallmentCalculator.GetDefaultDownPaymentRatio() > 0, "GetDefaultDownPaymentRatio must return valid positive down payment ratio");

        Console.WriteLine("PASS: " + checks + " checks (calculations, validation, recursive theme, sidebar blue theme, all forms, fixed grids/splitters, chart & config form, warranty form, button state machines).");
        }
        catch (Exception ex)
        {
            var err = ex.InnerException ?? ex;
            Console.WriteLine("TEST FAILURE: " + err.Message + "\n" + err.StackTrace);
            Environment.Exit(1);
        }
    }
}

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

    [STAThread]
    public static void Main()
    {
        try
        {
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
        }

        var forms = typeof(AppForm).Assembly.GetTypes().Where(t => t.Name.StartsWith("frm") && typeof(Form).IsAssignableFrom(t));
        Check(forms.All(t => typeof(AppForm).IsAssignableFrom(t)), "All forms must inherit shared theme");

        using (var qlTraGop = new QuanLyBanLaptop.HoaDon.frmQuanLyTraGop())
        {
            var split = qlTraGop.Controls.Find("pnlSplit", true).FirstOrDefault() as SplitContainer;
            Check(split != null && split.IsSplitterFixed, "pnlSplit must be fixed to prevent dragging");
            var dgvHopDong = qlTraGop.Controls.Find("dgvHopDong", true).FirstOrDefault() as DataGridView;
            Check(dgvHopDong != null && !dgvHopDong.AllowUserToResizeColumns && !dgvHopDong.AllowUserToOrderColumns, "dgvHopDong must not allow column resizing/reordering");
            var dgvLich = qlTraGop.Controls.Find("dgvLichTraGop", true).FirstOrDefault() as DataGridView;
            Check(dgvLich != null && !dgvLich.AllowUserToResizeColumns && !dgvLich.AllowUserToOrderColumns, "dgvLichTraGop must not allow column resizing/reordering");
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
            Check(dgv != null && !dgv.AllowUserToResizeColumns && !dgv.AllowUserToResizeRows && !dgv.AllowUserToOrderColumns, "dgvThongKe must not allow resizing or reordering");
            var btnIn = thongKe.Controls.Find("btnInBaoCao", true).FirstOrDefault() as Button;
            Check(btnIn != null, "btnInBaoCao must exist in frmThongKe");
            Check(btnIn.Visible == false, "btnInBaoCao must be hidden in frmThongKe");
            var btnExcel = thongKe.Controls.Find("btnXuatExcel", true).FirstOrDefault() as Button;
            Check(btnExcel != null, "btnXuatExcel must exist in frmThongKe");
        }

        using (var cauHinh = new QuanLyBanLaptop.DanhMuc.frmCauHinhTraGop())
        {
            Check(cauHinh.FormBorderStyle == FormBorderStyle.FixedSingle, "frmCauHinhTraGop must be FixedSingle");
            var dgvConfig = cauHinh.Controls.Find("dgvCauHinh", true).FirstOrDefault() as DataGridView;
            Check(dgvConfig != null && !dgvConfig.AllowUserToResizeColumns && !dgvConfig.AllowUserToResizeRows && !dgvConfig.AllowUserToOrderColumns, "dgvCauHinh must not allow column resizing or reordering");
        }

        using (var baoHanh = new QuanLyBanLaptop.DanhMuc.frmBaoHanhSuaChua())
        {
            Check(typeof(AppForm).IsAssignableFrom(typeof(QuanLyBanLaptop.DanhMuc.frmBaoHanhSuaChua)), "frmBaoHanhSuaChua must inherit AppForm");
            var dgvBH = baoHanh.Controls.Find("dgvBaoHanh", true).FirstOrDefault() as DataGridView;
            Check(dgvBH != null && !dgvBH.AllowUserToResizeColumns && !dgvBH.AllowUserToResizeRows && !dgvBH.AllowUserToOrderColumns, "dgvBaoHanh must not allow resizing or reordering");
            Check(baoHanh.Controls.Find("btnThem", true).FirstOrDefault() != null, "btnThem must exist in frmBaoHanhSuaChua");
            Check(baoHanh.Controls.Find("btnSua", true).FirstOrDefault() != null, "btnSua must exist in frmBaoHanhSuaChua");
            Check(baoHanh.Controls.Find("btnXoa", true).FirstOrDefault() != null, "btnXoa must exist in frmBaoHanhSuaChua");
            Check(baoHanh.Controls.Find("btnLuu", true).FirstOrDefault() != null, "btnLuu must exist in frmBaoHanhSuaChua");
            Check(baoHanh.Controls.Find("btnHuy", true).FirstOrDefault() != null, "btnHuy must exist in frmBaoHanhSuaChua");
            Check(baoHanh.Controls.Find("btnThoat", true).FirstOrDefault() != null, "btnThoat must exist in frmBaoHanhSuaChua");
            var btnInP = baoHanh.Controls.Find("btnInPhieu", true).FirstOrDefault();
            Check(btnInP != null, "btnInPhieu must exist in frmBaoHanhSuaChua");
            Check(btnInP.Visible == false, "btnInPhieu must be hidden in frmBaoHanhSuaChua");
            Check(baoHanh.Controls.Find("btnXuatExcel", true).FirstOrDefault() != null, "btnXuatExcel must exist in frmBaoHanhSuaChua");
            Check(baoHanh.Controls.Find("btnChonSanPham", true).FirstOrDefault() != null, "btnChonSanPham must exist in frmBaoHanhSuaChua");
        }

        Check(InstallmentCalculator.RateFor(3) > 0, "RateFor must return valid positive interest rate");
        Check(InstallmentCalculator.GetDefaultDownPaymentRatio() > 0, "GetDefaultDownPaymentRatio must return valid positive down payment ratio");

        Console.WriteLine("PASS: " + checks + " checks (calculations, validation, recursive theme, sidebar blue theme, all forms, fixed grids/splitters, chart & config form, warranty form).");
        }
        catch (Exception ex)
        {
            Console.WriteLine("TEST FAILURE: " + ex.Message + "\n" + ex.StackTrace);
            Environment.Exit(1);
        }
    }
}


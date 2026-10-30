using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyBanLaptop;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.DanhMuc;
using QuanLyBanLaptop.HoaDon;
public static class PermissionTests
{
    private static int checks;
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception("FAILED: " + message);
        checks++;
    }
    [STAThread]
    public static void Main()
    {
        try
        {
            string connection = Environment.GetEnvironmentVariable("LAPTOP_TEST_CONNECTION");
            if (!string.IsNullOrWhiteSpace(connection)) DataProcesser.ConnectionString = connection;
            UserSession.ChucVu = "Quản trị viên";
            UserSession.TenNV = "Nguyen Van Admin";
            Check(UserSession.IsAdmin, "UserSession.IsAdmin must be true for Quản trị viên");
            using (var sbAdmin = new SidebarControl())
            {
                var btnNV = sbAdmin.Controls.Find("btnNhanVien", true).FirstOrDefault() as Button;
                var btnTK = sbAdmin.Controls.Find("btnThongKe", true).FirstOrDefault() as Button;
                var btnCH = sbAdmin.Controls.Find("btnCauHinh", true).FirstOrDefault() as Button;
                Check(btnNV != null && btnNV.Visible, "Admin must see btnNhanVien");
                Check(btnTK != null && btnTK.Visible, "Admin must see btnThongKe");
                Check(btnCH != null && btnCH.Visible, "Admin must see btnCauHinh");
                Check(btnCH.Text.Contains("Cấu Hình"), "btnCauHinh text must contain Cấu Hình");
            }
            UserSession.ChucVu = "Nhân viên bán hàng";
            UserSession.TenNV = "Tran Thi NhanVien";
            Check(!UserSession.IsAdmin, "UserSession.IsAdmin must be false for Nhân viên");
            using (var sbStaff = new SidebarControl())
            {
                var btnNV = sbStaff.Controls.Find("btnNhanVien", true).FirstOrDefault() as Button;
                var btnTK = sbStaff.Controls.Find("btnThongKe", true).FirstOrDefault() as Button;
                var btnCH = sbStaff.Controls.Find("btnCauHinh", true).FirstOrDefault() as Button;
                Check(btnNV != null && !btnNV.Visible, "Staff must NOT see btnNhanVien");
                Check(btnTK != null && !btnTK.Visible, "Staff must NOT see btnThongKe");
                Check(btnCH != null && !btnCH.Visible, "Staff must NOT see btnCauHinh");
            }
            bool blockedNV = false;
            try { NavigationManager.GetForm(NavigationMenu.NhanVien); } catch (InvalidOperationException) { blockedNV = true; }
            Check(blockedNV, "Staff cannot access NhanVien");
            bool blockedTK = false;
            try { NavigationManager.GetForm(NavigationMenu.ThongKe); } catch (InvalidOperationException) { blockedTK = true; }
            Check(blockedTK, "Staff cannot access ThongKe");
            bool blockedCH = false;
            try { NavigationManager.GetForm(NavigationMenu.CauHinh); } catch (InvalidOperationException) { blockedCH = true; }
            Check(blockedCH, "Staff cannot access CauHinh");
            UserSession.ChucVu = "Quản trị viên";
            using (var qlTraGop = new frmQuanLyTraGop())
            {
                var pnlFilter = qlTraGop.Controls.Find("pnlFilter", true).FirstOrDefault();
                Check(pnlFilter != null && pnlFilter.Controls.Find("btnCauHinh", true).Length == 0, "btnCauHinh must be removed from pnlFilter of frmQuanLyTraGop");
                var fieldBtn = typeof(frmQuanLyTraGop).GetField("btnCauHinh", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Check(fieldBtn == null, "btnCauHinh field must not exist in frmQuanLyTraGop");
            }
            using (var formCH = new frmCauHinhTraGop())
            {
                var sb = formCH.Controls.Find("sidebarControl1", true).FirstOrDefault() as SidebarControl;
                Check(sb != null, "frmCauHinhTraGop must have sidebarControl1");
                Check(sb.SelectedMenu == NavigationMenu.CauHinh, "sidebarControl1 must have SelectedMenu = CauHinh");
                var cbo = formCH.Controls.Find("cboThamSo", true).FirstOrDefault() as ComboBox;
                Check(cbo != null, "cboThamSo must exist in frmCauHinhTraGop");
                var itemsText = string.Join(";", cbo.Items.Cast<object>().Select(x => x.ToString()));
                Check(itemsText.Contains("TRA_TRUOC_TOI_THIEU"), "Must contain TRA_TRUOC_TOI_THIEU");
                Check(itemsText.Contains("TRA_TRUOC_3_THANG"), "Must contain TRA_TRUOC_3_THANG");
                Check(itemsText.Contains("TRA_TRUOC_6_THANG"), "Must contain TRA_TRUOC_6_THANG");
                Check(itemsText.Contains("TRA_TRUOC_9_THANG"), "Must contain TRA_TRUOC_9_THANG");
                Check(itemsText.Contains("TRA_TRUOC_12_THANG"), "Must contain TRA_TRUOC_12_THANG");
                Check(itemsText.Contains("LAI_SUAT_3_THANG"), "Must contain LAI_SUAT_3_THANG");
                Check(itemsText.Contains("LAI_SUAT_6_THANG"), "Must contain LAI_SUAT_6_THANG");
                Check(itemsText.Contains("LAI_SUAT_9_THANG"), "Must contain LAI_SUAT_9_THANG");
                Check(itemsText.Contains("LAI_SUAT_12_THANG"), "Must contain LAI_SUAT_12_THANG");
                Check(itemsText.Contains("GIAM_GIA_BAC"), "Must contain GIAM_GIA_BAC");
                Check(itemsText.Contains("GIAM_GIA_VANG"), "Must contain GIAM_GIA_VANG");
                Check(itemsText.Contains("GIAM_GIA_VIP"), "Must contain GIAM_GIA_VIP");
                Check(itemsText.Contains("NGUONG_BAC"), "Must contain NGUONG_BAC");
                Check(itemsText.Contains("NGUONG_VANG"), "Must contain NGUONG_VANG");
                Check(itemsText.Contains("NGUONG_VIP"), "Must contain NGUONG_VIP");
            }
            UserSession.ChucVu = "Nhân viên bán hàng";
            using (var home = new QuanLyBanLaptop.frmTrangChu())
            {
                var cardNV = home.Controls.Find("pnlCardNhanVien", true).FirstOrDefault();
                Check(cardNV != null && !cardNV.Visible, "frmTrangChu pnlCardNhanVien must be hidden for staff");
            }
            Console.WriteLine("ALL PASS: " + checks + " permission & configuration checks passed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("TEST FAILURE: " + ex.Message + "\n" + ex.StackTrace);
            Environment.Exit(1);
        }
    }
}

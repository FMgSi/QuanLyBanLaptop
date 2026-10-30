using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyBanLaptop;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.DanhMuc;
using QuanLyBanLaptop.HoaDon;
using QuanLyBanLaptop.Thongke;
public static class GridScrollTests
{
    private static int checks = 0;
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception("FAILED: " + message);
        checks++;
        Console.WriteLine("  [PASS] " + message);
    }
    [STAThread]
    public static void Main()
    {
        try
        {
            string connection = Environment.GetEnvironmentVariable("LAPTOP_TEST_CONNECTION");
            if (!string.IsNullOrWhiteSpace(connection)) DataProcesser.ConnectionString = connection;
            Console.WriteLine("Starting Grid Scroll and Resize Verification Tests...");
            using (var testGrid = new DataGridView())
            {
                Function.DinhDangBang(testGrid);
                Check(testGrid.AllowUserToResizeColumns == true, "Function.DinhDangBang must set AllowUserToResizeColumns = true");
                Check(testGrid.ScrollBars == ScrollBars.Both, "Function.DinhDangBang must set ScrollBars = ScrollBars.Both");
            }
            using (var fNV = new frmNhanVien())
            {
                var dgv = fNV.Controls.Find("dgvNhanVien", true).FirstOrDefault() as DataGridView;
                Check(dgv != null, "dgvNhanVien must exist in frmNhanVien");
                Check(dgv.AllowUserToResizeColumns == true, "dgvNhanVien must allow column resizing (AllowUserToResizeColumns == true)");
                Check(dgv.ScrollBars == ScrollBars.Both, "dgvNhanVien must have horizontal and vertical scrollbars (ScrollBars.Both)");
                var colMK = dgv.Columns["colMatKhau"] ?? dgv.Columns["MatKhau"];
                Check(colMK != null && colMK.Visible, "colMatKhau must be visible in dgvNhanVien for Admin");
                foreach (DataGridViewColumn col in dgv.Columns)
                    if (col.Visible)
                        Check(col.MinimumWidth >= 80, $"dgvNhanVien column {col.Name} MinimumWidth must be >= 80 (got {col.MinimumWidth})");
            }
            using (var fKH = new frmKhachHang())
            {
                var dgv = fKH.Controls.Find("dgvKhachHang", true).FirstOrDefault() as DataGridView;
                Check(dgv != null, "dgvKhachHang must exist in frmKhachHang");
                Check(dgv.AllowUserToResizeColumns == true, "dgvKhachHang must allow column resizing");
                Check(dgv.ScrollBars == ScrollBars.Both, "dgvKhachHang must have ScrollBars.Both");
                Check(dgv.AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.None, "dgvKhachHang AutoSizeColumnsMode must be None");
            }
            using (var fSP = new frmSanPham())
            {
                var dgvLap = fSP.Controls.Find("dgvLaptop", true).FirstOrDefault() as DataGridView;
                Check(dgvLap != null, "dgvLaptop must exist in frmSanPham");
                Check(dgvLap.AllowUserToResizeColumns == true, "dgvLaptop must allow column resizing");
                Check(dgvLap.ScrollBars == ScrollBars.Both, "dgvLaptop must have ScrollBars.Both");
                var dgvLoai = fSP.Controls.Find("dgvLoaiMay", true).FirstOrDefault() as DataGridView;
                Check(dgvLoai != null, "dgvLoaiMay must exist in frmSanPham");
                Check(dgvLoai.AllowUserToResizeColumns == true, "dgvLoaiMay must allow column resizing");
                Check(dgvLoai.ScrollBars == ScrollBars.Both, "dgvLoaiMay must have ScrollBars.Both");
                var dgvNCC = fSP.Controls.Find("dgvNCC", true).FirstOrDefault() as DataGridView;
                Check(dgvNCC != null, "dgvNCC must exist in frmSanPham");
                Check(dgvNCC.AllowUserToResizeColumns == true, "dgvNCC must allow column resizing");
                Check(dgvNCC.ScrollBars == ScrollBars.Both, "dgvNCC must have ScrollBars.Both");
            }
            using (var fBH = new frmBaoHanhSuaChua())
            {
                var dgv = fBH.Controls.Find("dgvBaoHanh", true).FirstOrDefault() as DataGridView;
                Check(dgv != null, "dgvBaoHanh must exist in frmBaoHanhSuaChua");
                Check(dgv.AllowUserToResizeColumns == true, "dgvBaoHanh must allow column resizing");
                Check(dgv.ScrollBars == ScrollBars.Both, "dgvBaoHanh must have ScrollBars.Both");
            }
            using (var fCH = new frmCauHinhTraGop())
            {
                var dgv = fCH.Controls.Find("dgvCauHinh", true).FirstOrDefault() as DataGridView;
                Check(dgv != null, "dgvCauHinh must exist in frmCauHinhTraGop");
                Check(dgv.AllowUserToResizeColumns == true, "dgvCauHinh must allow column resizing");
                Check(dgv.ScrollBars == ScrollBars.Both, "dgvCauHinh must have ScrollBars.Both");
                Check(dgv.Columns["MoTa"].MinimumWidth >= 200, "dgvCauHinh MoTa MinimumWidth >= 200");
                Check(dgv.Columns["Khoa"].MinimumWidth >= 120, "dgvCauHinh Khoa MinimumWidth >= 120");
            }
            using (var fTG = new frmQuanLyTraGop())
            {
                var dgvHopDong = fTG.Controls.Find("dgvHopDong", true).FirstOrDefault() as DataGridView;
                Check(dgvHopDong != null, "dgvHopDong must exist in frmQuanLyTraGop");
                Check(dgvHopDong.AllowUserToResizeColumns == true, "dgvHopDong must allow column resizing");
                Check(dgvHopDong.ScrollBars == ScrollBars.Both, "dgvHopDong must have ScrollBars.Both");
                var dgvLich = fTG.Controls.Find("dgvLichTraGop", true).FirstOrDefault() as DataGridView;
                Check(dgvLich != null, "dgvLichTraGop must exist in frmQuanLyTraGop");
                Check(dgvLich.AllowUserToResizeColumns == true, "dgvLichTraGop must allow column resizing");
                Check(dgvLich.ScrollBars == ScrollBars.Both, "dgvLichTraGop must have ScrollBars.Both");
            }
            using (var fTK = new frmThongKe())
            {
                var dgv = fTK.Controls.Find("dgvThongKe", true).FirstOrDefault() as DataGridView;
                Check(dgv != null, "dgvThongKe must exist in frmThongKe");
                Check(dgv.AllowUserToResizeColumns == true, "dgvThongKe must allow column resizing");
                Check(dgv.ScrollBars == ScrollBars.Both, "dgvThongKe must have ScrollBars.Both");
            }
            using (var fBH = new frmBanHang())
            {
                var dgv = fBH.Controls.Find("dgvGioHang", true).FirstOrDefault() as DataGridView;
                Check(dgv != null, "dgvGioHang must exist in frmBanHang");
                Check(dgv.AllowUserToResizeColumns == true, "dgvGioHang must allow column resizing");
                Check(dgv.ScrollBars == ScrollBars.Both, "dgvGioHang must have ScrollBars.Both");
            }
            using (var fTKLT = new frmTimKiemLaptop())
            {
                var dgv = fTKLT.Controls.Find("dgvLaptop", true).FirstOrDefault() as DataGridView;
                Check(dgv != null, "dgvLaptop must exist in frmTimKiemLaptop");
                Check(dgv.AllowUserToResizeColumns == true, "dgvLaptop in frmTimKiemLaptop must allow column resizing");
                Check(dgv.ScrollBars == ScrollBars.Both, "dgvLaptop in frmTimKiemLaptop must have ScrollBars.Both");
            }
            using (var fTKKH = new frmTimKiemKhachHang())
            {
                var dgv = fTKKH.Controls.Find("dgvKhachHang", true).FirstOrDefault() as DataGridView;
                Check(dgv != null, "dgvKhachHang must exist in frmTimKiemKhachHang");
                Check(dgv.AllowUserToResizeColumns == true, "dgvKhachHang in frmTimKiemKhachHang must allow column resizing");
                Check(dgv.ScrollBars == ScrollBars.Both, "dgvKhachHang in frmTimKiemKhachHang must have ScrollBars.Both");
            }
            Console.WriteLine($"\nALL {checks} GRID CHECKS PASSED SUCCESSFULLY!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[ERROR] " + ex.Message);
            Environment.Exit(1);
        }
    }
}

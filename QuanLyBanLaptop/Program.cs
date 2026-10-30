using System;
using System.Windows.Forms;
namespace QuanLyBanLaptop
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, args) =>
                Classes.Function.ThongBaoLoi(args.Exception.Message);
            try
            {
                if (Classes.DataProcesser.ExecuteScalar(
                    "SELECT CASE WHEN OBJECT_ID('dbo.seq_AppInvoice','SO') IS NOT NULL THEN OBJECT_ID('dbo.HOP_DONG_TRA_GOP','U') END") == null)
                {
                    Classes.Function.ThongBaoCanhBao("Cần cài bộ SQL trong Database theo hướng dẫn README trước khi chạy.");
                    return;
                }
                using (var login = new frmDangNhap())
                    if (login.ShowDialog() != DialogResult.OK) return;
                Application.Run(new frmMain());
            }
            catch (Exception ex) { Classes.Function.ThongBaoLoi(ex.Message, "Không thể khởi động"); }
        }
    }
}

using System;
using System.Windows.Forms;

namespace QuanLyBanLaptop
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var login = new frmDangNhap())
            {
                if (login.ShowDialog() != DialogResult.OK) return;
            }

            Application.Run(new frmMain());
        }
    }
}

using System;
using QuanLyBanLaptop.Classes;
namespace QuanLyBanLaptop.Services
{
    public sealed class InstallmentSummary
    {
        public int Contracts { get; set; }
        public decimal Outstanding { get; set; }
        public int Overdue { get; set; }
        public int Completed { get; set; }
    }
    public sealed class InstallmentService
    {
        public InstallmentSummary GetSummary()
        {
            var row = DataProcesser.ReadData(@"SELECT
                (SELECT COUNT(*) FROM HOA_DON WHERE ChuKyTraGop>0) AS Contracts,
                ISNULL(SUM(CASE WHEN NgayTra IS NULL THEN SoTienPhaiTra-SoTienDaTra ELSE 0 END),0) AS Outstanding,
                COUNT(DISTINCT CASE WHEN NgayTra IS NULL AND NgayDenHan<CAST(GETDATE() AS date) THEN MaHD END) AS Overdue,
                (SELECT COUNT(*) FROM HOA_DON h WHERE ChuKyTraGop>0 AND NOT EXISTS
                    (SELECT 1 FROM LICH_TRA_GOP l WHERE l.MaHD=h.MaHD AND l.NgayTra IS NULL)) AS Completed
                FROM LICH_TRA_GOP").Rows[0];
            return new InstallmentSummary { Contracts = Convert.ToInt32(row["Contracts"]),
                Outstanding = Convert.ToDecimal(row["Outstanding"]), Overdue = Convert.ToInt32(row["Overdue"]),
                Completed = Convert.ToInt32(row["Completed"]) };
        }
        public decimal DailyPenalty()
        {
            decimal amount = new ConfigurationService().GetDecimal("TIEN_PHAT_NGAY");
            if (amount < 0) throw new InvalidOperationException("Tiền phạt không được âm.");
            return amount;
        }
    }
}

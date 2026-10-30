using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyBanLaptop.Classes;
using QuanLyBanLaptop.Services;
public static class BusinessTests
{
    private static int checks;
    private static void Check(bool value, string message)
    {
        if (!value) throw new Exception(message);
        checks++;
    }
    private static void Reject(Action action, string message)
    {
        try { action(); }
        catch (ArgumentException) { checks++; return; }
        catch (InvalidOperationException) { checks++; return; }
        catch (SqlException) { checks++; return; }
        throw new Exception(message);
    }
    private static decimal Scalar(string sql, params object[] args) => Convert.ToDecimal(DataProcesser.ExecuteScalar(sql, args));
    private static SaleRequest Sale(string id, int quantity, string method = "Tiền mặt", int months = 0)
    {
        var items = new DataTable();
        items.Columns.Add("MaLaptop", typeof(string));
        items.Columns.Add("SoLuong", typeof(int));
        items.Columns.Add("DonGia", typeof(decimal));
        items.Columns.Add("GiamGia", typeof(decimal));
        items.Rows.Add("LT003", quantity, 15000000m, 10m);
        return new SaleRequest { InvoiceId=id, CustomerId="KH001", EmployeeId="NV002",
            Date=DateTime.Today, Items=items, PaymentMethod=method, Months=months };
    }
    [STAThread]
    public static void Main()
    {
        try
        {
            string connection = Environment.GetEnvironmentVariable("LAPTOP_TEST_CONNECTION");
            if (string.IsNullOrEmpty(connection) || !new SqlConnectionStringBuilder(connection).InitialCatalog.StartsWith("QuanLyBanLaptop_Test_"))
                throw new Exception("Tests require an isolated QuanLyBanLaptop_Test_ database.");
            DataProcesser.ConnectionString = connection;
            Check(UserSession.MaNV == null && !UserSession.IsAdmin, "No implicit administrator session");
            Check(DataProcesser.DangNhap("admin", "wrong") == null, "Wrong password rejected");
            Check(DataProcesser.DangNhap("admin", "admin123") != null, "Demo administrator authenticates");
            UserSession.MaNV="NV001"; UserSession.ChucVu="Quản trị viên";
            foreach(int months in new[] {3,6,9,12})
            {
                var plan=InstallmentCalculator.Calculate(19999999m,months);
                decimal last=plan.Principal+plan.Interest-plan.Monthly*(months-1);
                Check(plan.Principal+plan.DownPayment==19999999m && last>=plan.Monthly,"Installment rounding");
            }
            Reject(()=>InstallmentCalculator.Calculate(100m,0,0,0),"Zero months");
            var settings=new ConfigurationService();
            Reject(()=>settings.Save("TRA_TRUOC_TOI_THIEU",100),"100 percent down payment rejected");
            settings.Save("TRA_TRUOC_TOI_THIEU",0);
            Check(InstallmentCalculator.GetDefaultDownPaymentRatio()==0,"Zero percent preserved");
            settings.Save("TRA_TRUOC_TOI_THIEU",30);
            var sales=new SaleService();
            var reports=new ReportService();
            var baseline=reports.GetRevenue(RevenueGrouping.Day,DateTime.Today,DateTime.Today);
            decimal priorRevenue=baseline.Rows.Count==0 ? 0 : Convert.ToDecimal(baseline.Compute("SUM(DoanhThu)",""));
            decimal stock=Scalar("SELECT SoLuongTon FROM LAPTOP WHERE MaLaptop='LT003'");
            sales.Save(Sale("TEST-CASH",2));
            Check(Scalar("SELECT SoLuongTon FROM LAPTOP WHERE MaLaptop='LT003'")==stock-2,"Sale allocates stock once");
            Check(Scalar("SELECT COUNT(*) FROM LAPTOP_SERIAL WHERE MaHD='TEST-CASH'")==2,"Exactly two serials allocated");
            Check(Scalar("SELECT TongTien FROM HOA_DON WHERE MaHD='TEST-CASH'")==27000000,"Discount reflected in total");
            var edit=Sale("TEST-CASH",1); edit.PaymentMethod=null;
            sales.Update(edit);
            Check(!edit.Items.Columns.Contains("OriginalCost"),"Update does not mutate UI table");
            Check(Scalar("SELECT SoLuongTon FROM LAPTOP WHERE MaLaptop='LT003'")==stock-1,"Edit releases old stock");
            var report=new ReportService();
            foreach(RevenueGrouping group in Enum.GetValues(typeof(RevenueGrouping)))
            {
                var rows=report.GetRevenue(group,DateTime.Today,DateTime.Today);
                Check(Convert.ToDecimal(rows.Compute("SUM(DoanhThu)",""))==priorRevenue+13500000,"Report groups agree after discount");
            }
            sales.Delete("TEST-CASH");
            Check(Scalar("SELECT SoLuongTon FROM LAPTOP WHERE MaLaptop='LT003'")==stock,"Delete restores stock");
            Reject(()=>sales.Save(Sale("TEST-FAIL",99999)),"Oversell rejected");
            Check(Scalar("SELECT COUNT(*) FROM HOA_DON WHERE MaHD='TEST-FAIL'")==0,"Failed sale rolls invoice back");
            Check(Scalar("SELECT SoLuongTon FROM LAPTOP WHERE MaLaptop='LT003'")==stock,"Failed sale preserves stock");
            var duplicate=Sale("TEST-DUP",1); duplicate.Items.Rows.Add("lt003",1,15000000m,0m);
            Reject(()=>sales.Save(duplicate),"Case-insensitive duplicates rejected");
            Reject(()=>sales.Update(Sale("TEST-NONE",0)),"Update validates quantity");
            bool opened=false;
            Reject(()=>new SaleService(()=>{ opened=true; return new DbHelpers(); }).Save(Sale("INVALID",0)),"Invalid input rejected before connection");
            Check(!opened,"Validation is independent of SQL connection");
            var first=Sale("TEST-RACE-A",2); first.Items.Rows[0]["MaLaptop"]="LT010";
            var second=Sale("TEST-RACE-B",2); second.Items.Rows[0]["MaLaptop"]="LT010";
            int successes=0;
            Action<SaleRequest> race=request=>
            {
                try { new SaleService().Save(request); System.Threading.Interlocked.Increment(ref successes); }
                catch(InvalidOperationException) { }
                catch(SqlException) { }
            };
            System.Threading.Tasks.Task.WaitAll(System.Threading.Tasks.Task.Run(()=>race(first)),System.Threading.Tasks.Task.Run(()=>race(second)));
            Check(successes==1 && Scalar("SELECT SoLuongTon FROM LAPTOP WHERE MaLaptop='LT010'")==1,"Concurrent sales cannot oversell");
            foreach(string id in new[]{first.InvoiceId,second.InvoiceId})
                if(Scalar("SELECT COUNT(*) FROM HOA_DON WHERE MaHD=@0",id)>0) sales.Delete(id);
            decimal beforeImport=Scalar("SELECT SoLuongTon FROM LAPTOP WHERE MaLaptop='LT003'");
            DataProcesser.ChangeData(@"DECLARE @items dbo.TVP_NhapHang,@id varchar(10);
                INSERT @items VALUES('LT003',1001,12000000);
                EXEC sp_NhapHang 'NCC03','NV003',@items,NULL,@id OUTPUT;");
            Check(Scalar("SELECT SoLuongTon FROM LAPTOP WHERE MaLaptop='LT003'")==beforeImport+1001,"Import 1001 serials without truncation or double allocation");
            sales.Save(Sale("TEST-CREDIT",1,"Trả góp",3));
            var schedule=DataProcesser.ReadData("SELECT * FROM LICH_TRA_GOP WHERE MaHD=@0","TEST-CREDIT");
            var calc=InstallmentCalculator.Load("TEST-CREDIT",13500000m,3);
            Check(schedule.Rows.Count==3 && Convert.ToDecimal(schedule.Compute("SUM(SoTienPhaiTra)",""))==calc.Principal+calc.Interest,"SQL schedule agrees with calculator");
            DataProcesser.ChangeData("EXEC sp_ThanhToanKy @MaHD=@0,@KyThu=@1", "TEST-CREDIT",1);
            Reject(()=>DataProcesser.ChangeData("EXEC sp_ThanhToanKy @MaHD=@0,@KyThu=@1", "TEST-CREDIT",1),"Duplicate payment rejected");
            Reject(()=>sales.Delete("TEST-CREDIT"),"Credit invoice cannot be deleted");
            Check(new InstallmentService().GetSummary().Contracts==3,"Installment summary query");
            var warranty=new WarrantyService();
            string serial=Convert.ToString(DataProcesser.ExecuteScalar("SELECT SerialNo FROM LAPTOP_SERIAL WHERE MaHD=@0","TEST-CREDIT"));
            string serialFilter="SerialNo='"+serial+"'";
            Check(warranty.GetCustomerDevices("KH001",true).Select(serialFilter).Length==1,"Unexpired purchased device is selectable");
            Check(warranty.GetCustomerDevices("KH002",true).Select(serialFilter).Length==0,"Device picker filters by customer");
            DataProcesser.ChangeData(@"INSERT SERIAL_BAO_HANH_DAC_THU(SerialNo,MaHD,NgayHetBH)
                VALUES(@0,'TEST-CREDIT',DATEADD(day,-1,CAST(GETDATE() AS date)))",serial);
            Check(warranty.GetCustomerDevices("KH001",true).Select(serialFilter).Length==0,"Expired warranty hidden");
            Check(warranty.GetCustomerDevices("KH001",false).Select(serialFilter).Length==1,"Expired device still eligible for paid repair");
            DataProcesser.ChangeData("UPDATE SERIAL_BAO_HANH_DAC_THU SET NgayHetBH=CAST(GETDATE() AS date) WHERE SerialNo=@0",serial);
            Check(warranty.GetCustomerDevices("KH001",true).Select(serialFilter).Length==1,"Warranty includes its final date");
            DataProcesser.ChangeData("UPDATE SERIAL_BAO_HANH_DAC_THU SET NgayHetBH=NULL WHERE SerialNo=@0",serial);
            Check(warranty.GetCustomerDevices("KH001",true).Select(serialFilter).Length==0,"Unknown warranty date is not eligible");
            DataProcesser.ChangeData("DELETE SERIAL_BAO_HANH_DAC_THU WHERE SerialNo=@0",serial);
            Reject(()=>warranty.Create("KH002",serial,"Bảo hành","Test"),"Customer/serial mismatch rejected");
            string ticket=warranty.Create("KH001",serial,"Bảo hành","Test fan");
            Check(Convert.ToString(DataProcesser.ExecuteScalar("SELECT TrangThai FROM LAPTOP_SERIAL WHERE SerialNo=@0",serial))=="Đang bảo hành","Warranty changes serial state");
            Reject(()=>warranty.Create("KH001",serial,"Bảo hành","Duplicate"),"Concurrent open ticket rejected");
            warranty.UpdateProblem(ticket,"Updated fan problem");
            warranty.Delete(ticket);
            Check(Convert.ToString(DataProcesser.ExecuteScalar("SELECT TrangThai FROM LAPTOP_SERIAL WHERE SerialNo=@0",serial))=="Đã bán","Delete pending ticket restores state");
            DataProcesser.ChangeData("INSERT KHACH_HANG_DATA(MaKH,TenKH,Sdt) VALUES(@0,@1,@2)","KH-REPAIR","Khách chỉ sửa máy","0909998877");
            Check(Scalar("SELECT COUNT(*) FROM HOA_DON WHERE MaKH=@0","KH-REPAIR")==0,"Repair customer has no purchases");
            string outside=warranty.Create("KH-REPAIR",null,"Sửa chữa","Vệ sinh máy mua ngoài","Dell mua nơi khác");
            string outside2=warranty.Create("KH-REPAIR",null,"Sửa chữa","Thay bàn phím","HP mua nơi khác");
            Check(warranty.GetTickets().Select("MaPhieuBH='"+outside+"'").Length==1,"External repair visible in ticket list");
            warranty.UpdateProblem(outside,"Updated external repair");
            DataProcesser.ChangeData("EXEC sp_CapNhatBaoHanh @MaPhieuBH=@0,@TinhTrang=@1,@ChiPhi=@2",outside,"Đã trả khách",100000m);
            Check(Convert.ToString(DataProcesser.ExecuteScalar("SELECT TinhTrangBH FROM BAO_HANH_SUA_CHUA WHERE MaPhieuBH=@0",outside))=="Đã trả khách","External repair can be completed");
            warranty.Delete(outside2);
            Reject(()=>warranty.Create("KH002",null,"Bảo hành","Test","Máy mua ngoài"),"External device cannot claim warranty");
            Reject(()=>warranty.Create("KH002",null,"Sửa chữa","Test"," "),"External repair requires device name");
            UserSession.ChucVu="Nhân viên";
            Reject(()=>settings.Save("TRA_TRUOC_TOI_THIEU",50),"Employee cannot change terms");
            Reject(()=>new EmployeeService().Delete("NV003"),"Employee cannot delete another employee");
            Console.WriteLine("PASS: " + checks + " business checks.");
        }
        catch(Exception ex) { Console.Error.WriteLine(ex); Environment.Exit(1); }
    }
}

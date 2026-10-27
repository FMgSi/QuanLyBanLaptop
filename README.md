# Quản lý bán laptop

Ứng dụng WinForms C# trên .NET Framework 4.7.2, sử dụng SQL Server. Xem [đánh giá khả năng dùng thực tế](docs/DANH_GIA_THUC_TE.md) để biết các luồng đã kiểm chứng và phần chưa hoàn thiện.

## Bộ SQL đã gom

16 script cũ được thay bằng 4 file, mỗi procedure/trigger chỉ còn một định nghĩa cuối cùng:

| File | Mục đích |
|---|---|
| `Database/01_structure.sql` | Bảng, ràng buộc, index, sequence, kiểu bảng, hàm, view và cấu hình bắt buộc |
| `Database/02_operations.sql` | Procedure và trigger nghiệp vụ |
| `Database/03_demo.sql` | Dữ liệu minh họa tùy chọn, nạp một lần |
| `Database/04_verify.sql` | Đối soát chỉ đọc; đúng thì không trả dòng nào |

Không còn script tự DROP database, đặt lại mật khẩu tài khoản cũ hoặc bổ sung serial demo thiếu phiếu nhập. Index trùng/phủ bởi index khác đã được bỏ. Bộ mới là **baseline cài mới**, không phải migration tự động cho mọi phiên bản DB cũ.

## Cài và chạy

1. Cài Visual Studio với workload .NET desktop development và targeting pack .NET Framework 4.7.2; chuẩn bị SQL Server và quyền tạo DB.
2. Mở PowerShell tại thư mục dự án, dùng một tên DB mới:

   ```powershell
   powershell -NoProfile -ExecutionPolicy Bypass -File Database/Install.ps1 -Server '.\SQLEXPRESS' -Database QuanLyBanLaptop_Demo -Demo
   ```

   Trình cài dừng trước khi chạy SQL nếu DB đã có cấu trúc. Cấu trúc/nghiệp vụ được cài trong một transaction; demo chạy trong transaction riêng sau đó để tránh giữ khóa cấu trúc khi sinh dữ liệu. Nếu demo lỗi, dữ liệu demo rollback nhưng cấu trúc đã cài được giữ lại. Nó không xóa DB đang có. Bỏ `-Demo` để chỉ tạo cấu trúc và cấu hình.

3. Sửa `Server` và `Database` trong `QuanLyBanLaptop/App.config` cho đúng DB vừa tạo, rồi build bằng Visual Studio. `build_project.bat` vẫn dùng đường dẫn Visual Studio riêng của máy hiện tại.
4. Khi chạy, ứng dụng yêu cầu đăng nhập. Chỉ với DB demo: `admin / admin123`, `binh_sale / 123456`, `cuong_kho / 123456`.

DB không nạp demo chưa có tài khoản: quản trị DB cần tạo tài khoản đầu tiên bằng `dbo.sp_ThemNhanVien`, truyền `@MaNV` OUTPUT, thông tin nhân viên, `@ChucVu=N'Quản trị viên'`, tên đăng nhập và mật khẩu riêng. Không chèn mật khẩu rõ trực tiếp vào bảng. Sau đó dùng ứng dụng để quản lý nhân viên.

**DB hiện có:** đợt refactor không tự sửa DB hoặc đổi connection string của bạn. Sao lưu và thử khôi phục trước khi xây dựng migration riêng theo phiên bản đang dùng. Không chạy `01_structure.sql` hay demo trên DB có dữ liệu; không chỉ chạy riêng `02_operations.sql` nếu chưa kiểm tra đủ các bảng/view phụ thuộc. Các script cũ còn trong lịch sử Git khi cần đối chiếu. Nếu dùng SSMS/sqlcmd cho DB trống, chạy theo thứ tự 01 → 02 → 03 (tùy chọn) → 04 và bật dừng khi lỗi (`sqlcmd -b`). Khuyến nghị dùng `Install.ps1` để kiểm tra DB trống và rollback đồng bộ.

## Tổ chức code

- `Models`: dữ liệu sản phẩm, lớp chung `Person`, khách hàng và nhân viên.
- `Services`: kiểm tra/điều phối nghiệp vụ; báo cáo và KPI dùng truy vấn chung.
- `Classes/IDatabaseSession.cs`, `DbHelpers.cs`: hợp đồng phiên dữ liệu, transaction, commit/rollback.
- Các form: thu nhận dữ liệu, hiển thị và xử lý tương tác; luồng ghi chính đã chuyển sang service.
- SQL: cấp serial, ràng buộc và dữ liệu dẫn xuất. C# không cấp serial thêm lần nữa.

Một số truy vấn của các form cũ vẫn dùng `DataProcesser`/`Function`; giới hạn kiến trúc được nêu rõ trong báo cáo, không coi đây là hoàn tất chuyển đổi mọi form sang repository.

## Kiểm thử

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File Tests/Run-Tests.ps1 -Server '.\SQLEXPRESS'
```

Script tìm MSBuild qua `vswhere`, build ứng dụng và test, tạo DB `QuanLyBanLaptop_Test_<GUID>`, chạy 45 kiểm tra nghiệp vụ và 115 kiểm tra tính toán/giao diện, đối soát rồi tự xóa DB thử. Cần quyền tạo/xóa DB thử. Không chạy test ghi dữ liệu trên DB bán hàng thật.

Các kiểm thử quan trọng gồm hai nhân viên cùng mua phần tồn cuối, rollback khi thiếu hàng, nhập 1.001 serial, giảm giá nhất quán trên báo cáo, thu kỳ không trùng và bảo hành đúng máy của khách.

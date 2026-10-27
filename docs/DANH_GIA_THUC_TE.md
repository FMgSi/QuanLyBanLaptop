# Đánh giá ứng dụng quản lý bán laptop

## Kết luận

Ứng dụng có nền tảng cho một cửa hàng nhỏ: danh mục máy, khách hàng, nhân viên, hóa đơn, serial, trả góp và báo cáo. Sau đợt sửa này có thể thử nghiệm các luồng đã kiểm chứng trên dữ liệu thử. **Chưa nên đưa vào vận hành chính thức cho nhân viên** vì nhập hàng trên giao diện, hoàn tiền/đổi trả, kiểm toán và bảo hành trọn quy trình chưa hoàn chỉnh.

Đây là đánh giá từ mã nguồn và kiểm thử tự động, không phải kết quả nghiệm thu với nhân viên cửa hàng hay thử tải hệ thống thực tế.

## Những lỗi đã xử lý

| Vấn đề trước đây | Thay đổi |
|---|---|
| 16 script tạo rồi sửa lại nhiều lần cùng một đối tượng; bộ cài có thể xóa DB | 4 file theo mục đích, cấu trúc cuối cùng được tạo trực tiếp; trình cài chỉ chấp nhận DB trống |
| C# và trigger cùng cấp serial khi bán; procedure nhập cũng cấp trùng serial với trigger | Trigger là nơi cấp serial duy nhất; C# kiểm tra số lượng đã cấp; tồn kho suy ra từ serial |
| Serial lô nhập bị cắt ở 3 chữ số | Giữ đủ số thứ tự; kiểm thử nhập 1.001 máy |
| Tỷ lệ 30% bị ép vào decimal(5,4), gây tràn số | Chuyển qua decimal(9,4) trước khi chia 100 |
| Migration tham chiếu view trạng thái trả góp không tồn tại | Bộ cấu trúc tạo đầy đủ view phụ thuộc |
| Phiếu bảo hành dùng serial cố định, mã MP dài hơn cột DB, báo thành công dù SQL lỗi | Chọn serial thuộc hóa đơn của khách; mã BH do SQL cấp; chỉ cập nhật danh sách sau khi lưu thành công |
| DB rỗng/lỗi bị thay bằng dữ liệu khách và phiếu bảo hành giả | Hiển thị dữ liệu thật hoặc lỗi; không tự trộn demo vào giao dịch |
| Tự gán tài khoản quản trị khi mở chương trình; form đăng nhập tự thử nhiều cách băm | Bắt buộc đăng nhập và dùng sp_DangNhap; không tải hash mật khẩu về form |
| Báo cáo sản phẩm/nhân viên bỏ qua giảm giá; báo cáo tháng có thể lấy ngoài khoảng ngày chọn | ReportService dùng chung công thức ThanhTien và cùng khoảng ngày cho mọi nhóm |
| Sai cấu hình trả góp/tiền phạt bị âm thầm thay bằng giá trị mặc định | Đọc và kiểm tra cấu hình; báo lỗi khi thiếu/sai dữ liệu |
| Các KPI công nợ gọi SQL bốn lần | Một truy vấn, trả về InstallmentSummary |

## Mức độ đáp ứng công việc của nhân viên

| Công việc | Hiện trạng sau sửa | Việc cần hoàn thiện |
|---|---|---|
| Đăng nhập | Xác thực qua SQL; không tự đăng nhập quản trị | Giới hạn thử sai, khóa phiên, đổi mật khẩu bắt buộc, rà soát quyền DB |
| Tra cứu laptop/khách | Có, tìm kiếm và danh mục hoạt động | Phân trang dữ liệu lớn, xử lý tìm kiếm không khóa giao diện |
| Bán hàng | Đã kiểm thử lưu, giảm giá, sửa/xóa, thiếu hàng và tranh chấp tồn kho | Chọn/quét đúng serial vật lý lúc giao máy, quyền giảm giá, xác nhận chuyển khoản |
| Nhập hàng | Có procedure và ProductService nhập tồn đầu; SQL nhập đã kiểm thử | frmNhapHang mới tải combobox, các nút thêm/xóa/xác nhận chưa nối nghiệp vụ; NavigationManager còn đưa mục NhapHang về trang chủ |
| Trả góp | Có lịch, chốt điều khoản, thu từng kỳ, chặn thu trùng | Lưu chứng từ tiền trả trước, sổ thu tiền, người thu, phương thức thanh toán, hoàn/hủy thu; thu một phần và điều chỉnh hợp đồng |
| Bảo hành/sửa chữa | Tiếp nhận đúng serial, sửa mô tả, kiểm tra hạn; chặn phiếu mở trùng | Giao diện chuyển trạng thái, bàn giao máy, phụ tùng, báo giá/phê duyệt, thu phí; danh mục dịch vụ hiện vẫn là dữ liệu tĩnh trong form |
| Đổi trả | Có thao tác xóa/trả dòng và hoàn serial | Cần chứng từ đổi trả, lý do, tiền hoàn, phí, người duyệt; không nên dùng xóa hóa đơn để thay nghiệp vụ hoàn tiền |
| Báo cáo | Doanh thu sau giảm giá và giá vốn giao dịch thống nhất giữa các nhóm | Phân biệt doanh thu với tiền thực thu; chi phí vận hành, thuế và kế toán chưa được mô hình hóa đầy đủ |
| Nhiều nhân viên | Có giao dịch SQL và khóa; đã thử hai lần bán tranh chấp cùng tồn kho | Thử tải nhiều máy, timeout/deadlock và phục hồi mạng thực tế; quản lý ca, chi nhánh/kho chưa có |

## Kiến trúc OOP

- `Models`: `Product`, `Person`, `Customer`, `Employee`. `Person` chứa thuộc tính chung của khách hàng và nhân viên.
- `Services`: bán hàng, sản phẩm, khách hàng, nhân viên, cấu hình, bảo hành, thống kê và tổng hợp trả góp. Form chuyển dữ liệu vào service, không tự lặp các bước ghi của những luồng đã tách.
- `IDatabaseSession` là hợp đồng cho phiên dữ liệu; `DbHelpers` quản lý kết nối, transaction và rollback. `SaleService` nhận factory tạo phiên để thay thế khi kiểm thử.
- `DataProcesser` là lớp tĩnh tương thích cho truy vấn cũ, không còn kế thừa lớp giao dịch chỉ để dùng hàm static.
- `InstallmentCalculator` nhận lãi suất/tỷ lệ cụ thể khi cần kiểm thử công thức hoặc tính hợp đồng lịch sử.
- Cơ sở dữ liệu giữ ràng buộc, khóa và cấp serial; không lặp lại việc cấp serial ở C#.

Đây là refactor các luồng chính và hạ tầng dùng chung, **chưa phải chuyển toàn bộ ứng dụng sang kiến trúc phân lớp nghiêm ngặt**. Một số màn hình danh mục, tra cứu và trả góp vẫn có SQL đọc trực tiếp; `Function` vẫn là facade tiện ích lớn; một số hợp đồng với giao diện vẫn dùng `DataTable`. Các file Designer giữ cấu trúc do WinForms sinh để tiếp tục chỉnh bằng Visual Studio. Không thêm interface cho mọi lớp chỉ để tăng số lượng lớp.

## Thứ tự hoàn thiện trước khi dùng thật

1. Hoàn thiện màn hình nhập hàng và quét serial; chạy thử quy trình nhập → bán → bảo hành → trả máy với nhân viên.
2. Thay việc xóa giao dịch bằng chứng từ điều chỉnh/đổi trả; lưu người thực hiện, thời điểm và lý do. Chốt sổ thu tiền cho tiền mặt, chuyển khoản và trả góp.
3. Kiểm soát quyền ở service và tại DB/API cho tất cả thao tác nhạy cảm. Hiện chỉ mới bổ sung chặn ở một số service; người có quyền SQL trực tiếp vẫn có thể vượt kiểm soát giao diện. Nâng cấp cơ chế lưu mật khẩu từ SHA-256 có salt sang thuật toán chuyên dụng có chi phí tính toán.
4. Thiết lập sao lưu, thử khôi phục, ghi log và xử lý mất kết nối. Dữ liệu khách hàng và tài khoản thật không dùng lẫn tài khoản demo.
5. Thử tải, kiểm kê hàng thực tế và nghiệm thu báo cáo với chủ cửa hàng; đo truy vấn trước khi thêm index hoặc cache.

## Bằng chứng kiểm thử

`Tests/Run-Tests.ps1` build chương trình, tạo DB có tên ngẫu nhiên, cài cả cấu trúc lẫn demo, chạy kiểm thử và xóa đúng DB đó ở cuối.

- 45 kiểm tra nghiệp vụ: đăng nhập, cấu hình, làm tròn trả góp, cấp serial một lần, giảm giá, sửa/xóa, rollback, tranh chấp bán hàng, nhập 1.001 serial, lịch/thu kỳ, khách–serial và quyền một số thao tác.
- 115 kiểm tra tính toán/giao diện: khởi tạo form, nút, trạng thái chỉnh sửa, bảng và theme.
- Đối soát cuối kiểm tra quan hệ số lượng nhập/bán với serial, tổng tiền, lịch trả góp và trạng thái bảo hành.

Chưa có bằng chứng chạy tải lớn, mọi thao tác giao diện bằng chuột, tích hợp ngân hàng, máy quét, máy in thực hay khôi phục sau sự cố. Không suy diễn việc test tự động qua thành hệ thống đã sẵn sàng vận hành.

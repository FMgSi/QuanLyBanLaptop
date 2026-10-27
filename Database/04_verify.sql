/* =====================================================================
   QuanLyBanLaptop - 04_verify.sql
   Đối soát dữ liệu dẫn xuất. Kết quả ĐÚNG là không có dòng nào.
   ===================================================================== */

GO

SELECT N'Tồn kho lệch số serial trong kho' AS Loai, MaLaptop AS Khoa,
       N'SoLuongTon=' + CAST(SoLuongTon AS NVARCHAR(10)) + N', serial trong kho=' + CAST(SoMayTrongKho AS NVARCHAR(10)) AS ChiTiet
FROM dbo.vw_TonKho WHERE ChenhLech <> 0

UNION ALL
SELECT N'Tổng tiền hóa đơn lệch chi tiết', hd.MaHD,
       N'TongTien=' + CAST(hd.TongTien AS NVARCHAR(30))
FROM HOA_DON hd
WHERE hd.TongTien <> ISNULL((SELECT SUM(ThanhTien) FROM CHI_TIET_HD WHERE MaHD = hd.MaHD), 0)

UNION ALL
SELECT N'Tổng tiền phiếu nhập lệch chi tiết', pn.MaPN,
       N'TongTien=' + CAST(pn.TongTien AS NVARCHAR(30))
FROM PHIEU_NHAP pn
WHERE pn.TongTien <> ISNULL((SELECT SUM(SoLuong * DonGiaNhap) FROM CHI_TIET_PN WHERE MaPN = pn.MaPN), 0)

UNION ALL
SELECT N'Số serial đã bán lệch số lượng trên hóa đơn', ct.MaHD + N'/' + ct.MaLaptop,
       N'SoLuong=' + CAST(ct.SoLuong AS NVARCHAR(10))
FROM CHI_TIET_HD ct
WHERE ct.SoLuong <> (SELECT COUNT(*) FROM LAPTOP_SERIAL s WHERE s.MaHD = ct.MaHD AND s.MaLaptop = ct.MaLaptop)

UNION ALL
SELECT N'Số serial lệch số lượng trên phiếu nhập', ct.MaPN + N'/' + ct.MaLaptop,
       N'SoLuong=' + CAST(ct.SoLuong AS NVARCHAR(10))
FROM CHI_TIET_PN ct
WHERE ct.SoLuong <> (SELECT COUNT(*) FROM LAPTOP_SERIAL s WHERE s.MaPN = ct.MaPN AND s.MaLaptop = ct.MaLaptop)

UNION ALL
SELECT N'Hóa đơn trả góp thiếu/sai lịch kỳ', hd.MaHD,
       N'ChuKy=' + CAST(hd.ChuKyTraGop AS NVARCHAR(10))
FROM HOA_DON hd
WHERE hd.PhuongThucThanhToan = N'Trả góp'
  AND (SELECT COUNT(*) FROM LICH_TRA_GOP WHERE MaHD = hd.MaHD) <> hd.ChuKyTraGop

UNION ALL
SELECT N'Tổng lịch lệch dư nợ và lãi hợp đồng', hd.MaHD,
       N'TongTien=' + CAST(hd.TongTien AS NVARCHAR(30))
FROM HOA_DON hd
WHERE hd.PhuongThucThanhToan = N'Trả góp'
  AND EXISTS (SELECT 1 FROM HOP_DONG_TRA_GOP c WHERE c.MaHD=hd.MaHD AND
    hd.TongTien-ROUND(hd.TongTien*c.TyLeTraTruoc,0)
    +ROUND((hd.TongTien-ROUND(hd.TongTien*c.TyLeTraTruoc,0))*c.LaiSuat/100,0)
    <> (SELECT SUM(SoTienPhaiTra) FROM LICH_TRA_GOP WHERE MaHD=hd.MaHD))

UNION ALL
SELECT N'Tình trạng hóa đơn không khớp lịch trả góp', hd.MaHD, hd.TinhTrang
FROM HOA_DON hd
WHERE hd.PhuongThucThanhToan = N'Trả góp'
  AND ((hd.TinhTrang = N'Đã thanh toán' AND EXISTS (SELECT 1 FROM LICH_TRA_GOP WHERE MaHD = hd.MaHD AND NgayTra IS NULL))
    OR (hd.TinhTrang = N'Đang trả góp'  AND NOT EXISTS (SELECT 1 FROM LICH_TRA_GOP WHERE MaHD = hd.MaHD AND NgayTra IS NULL)))

UNION ALL
SELECT N'Hạng thành viên chưa cập nhật', MaKH, HangThanhVien
FROM KHACH_HANG
WHERE HangThanhVien <> dbo.fn_HangThanhVien(dbo.fn_TongChiTieu(MaKH))

UNION ALL
SELECT N'Máy "Đang bảo hành" không có phiếu đang mở', s.SerialNo, s.TrangThai
FROM LAPTOP_SERIAL s
WHERE s.TrangThai = N'Đang bảo hành'
  AND NOT EXISTS (SELECT 1 FROM BAO_HANH_SUA_CHUA b WHERE b.SerialNo = s.SerialNo AND b.TinhTrangBH <> N'Đã trả khách');
GO

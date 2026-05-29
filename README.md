# 🏠 Hệ Thống Quản Lý Cho Thuê Phòng Trọ (Phòng Trọ VN)

[![Framework](https://img.shields.io/badge/Framework-ASP.NET%20Core%20MVC%208.0-blue)](https://dotnet.microsoft.com/en-us/apps/aspnet)
[![Language](https://img.shields.io/badge/Language-C%23-green)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Database](https://img.shields.io/badge/Database-SQL%20Server-red)](https://www.microsoft.com/en-us/sql-server/)

## 📝 Giới thiệu Đề tài
**Phòng Trọ VN** là hệ thống web toàn diện nhằm số hóa và tự động hóa quy trình quản lý cho thuê phòng trọ. Hệ thống giải quyết các vấn đề thực tiễn như quản lý hồ sơ khách thuê (bằng MSV và CCCD), theo dõi hợp đồng, chốt chỉ số điện nước hàng tháng, bóc tách hóa đơn dịch vụ minh bạch, và tiếp nhận yêu cầu bảo trì trực tuyến. 

Hệ thống bao gồm 2 phân hệ rõ ràng: Cổng thông tin cho người thuê (Front-end) và Bảng điều khiển quản trị (Back-end Admin).

## 👥 Thành viên nhóm & Phân công công việc

| Họ và tên | Công việc phụ trách |
| :--- | :--- |
| **Phạm Ngọc Phi** | <ul><li>Viết giới thiệu & Xác định yêu cầu chức năng.</li><li>Tra cứu & Thống kê báo cáo.</li></ul> |
| **Hà Phước Bình** | <ul><li>Vẽ sơ đồ Use Case.</li><li>Thiết kế UI cho Admin và User.</li><li>Xây dựng chức năng Quản lý phòng & Khách thuê.</li></ul> |
| **Trương Văn Quang** | <ul><li>Vẽ sơ đồ ERD & Thiết kế CSDL.</li><li>Xây dựng chức năng Tính tiền & Lập hóa đơn.</li><li>Tổng hợp báo cáo vào Google Docs.</li></ul> |

## 🔗 Liên kết tài liệu dự án
- **GitHub (Mã nguồn):** [https://github.com/truongvanquang/quanliphongtro](https://github.com/truongvanquang/quanliphongtro)
- **Trello (Quản lý tiến độ):** [https://trello.com/b/5AKd03Ep/quản-ly-phong-trọ](https://trello.com/b/5AKd03Ep/quản-ly-phong-trọ)
- **Google Docs (Báo cáo chi tiết):** [https://docs.google.com/document/d/1AgJaMBelIProQls5PCLkx2WMv4Kywx9_URo36DLHe0A/edit?usp=sharing](https://docs.google.com/document/d/1AgJaMBelIProQls5PCLkx2WMv4Kywx9_URo36DLHe0A/edit?usp=sharing)
- **Stitch (Thiết kế giao diện):** [https://stitch.withgoogle.com/projects/8155711785897004727](https://stitch.withgoogle.com/projects/8155711785897004727)

## 🚀 Tính năng nổi bật

### 👨‍💼 Phân hệ Quản trị (Admin)
1. **Tổng quan (Dashboard)**: Thống kê số phòng, tỷ lệ lấp đầy, biểu đồ xu hướng doanh thu.
2. **Quản lý phòng trọ**: Quản lý danh mục phòng, phân loại phòng, diện tích, trạng thái, thiết lập giá cơ bản.
3. **Quản lý khách thuê**: Lưu trữ hồ sơ định danh, MSV, CCCD và lịch sử thuê.
4. **Quản lý hợp đồng**: Theo dõi vòng đời hợp đồng, tiền cọc và cảnh báo sắp hết hạn.
5. **Quản lý thanh toán**: Lập hóa đơn, chốt chỉ số điện/nước và theo dõi nợ đọng.
6. **Yêu cầu sửa chữa**: Tiếp nhận ticket báo sự cố từ khách và điều phối xử lý.
7. **Báo cáo & Thống kê**: Phân tích tài chính, doanh thu qua 12 tháng, xuất file PDF/Excel.
8. **Quản lý tài khoản**: Phân quyền hệ thống (Admin/Nhân viên) và bảo mật truy cập.

### 👤 Phân hệ Khách thuê (User)
1. **Khám phá phòng trọ**: Tìm kiếm phòng trống theo khu vực, mức giá, loại hình.
2. **Bảng điều khiển cá nhân**: Tóm tắt tình trạng lưu trú và nhắc nhở khoản thanh toán tiếp theo.
3. **Thông tin hợp đồng**: Xem bản sao điện tử hợp đồng đã ký kết.
4. **Lịch sử thanh toán**: Đối soát các hóa đơn đã đóng, khoản còn nợ và chi tiết từng dịch vụ.
5. **Báo sự cố**: Gửi yêu cầu bảo trì kèm hình ảnh minh họa cho ban quản lý.

## 📊 Kiến trúc Cơ sở dữ liệu (12 Bảng Chuẩn Hóa)
Hệ thống sử dụng cấu trúc Database chuẩn hóa cao để đảm bảo tính toàn vẹn dữ liệu:
* **Hệ thống/Người dùng:** `TaiKhoan`, `KhachThue`
* **Quản lý tài sản:** `LoaiPhong`, `TienIch`, `PhongTro`, `PhongTienIch`
* **Vận hành/Kinh doanh:** `HopDong`, `ChiSoDienNuoc`
* **Tài chính:** `HoaDon`, `ChiTietHoaDon`, `LichSuThanhToan`
* **Dịch vụ khách hàng:** `YeuCauSuaChua`

---
*Dự án thực hiện cho môn học Lập trình C# - Giảng viên hướng dẫn: Đỗ Phú Huy.*

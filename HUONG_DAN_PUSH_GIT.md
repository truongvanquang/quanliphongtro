# Hướng dẫn Push Git theo từng thành viên

## Cấu trúc nhánh đã tạo

```
main (base trống)
├── feature/PN-setup-auth              → Thành viên PN (Thẻ 1, 2, 4, 5)
├── feature/PB-admin-core              → Thành viên PB (Thẻ 3, 6, 7, 8)
└── feature/QV-billing-user-report     → Thành viên QV (Thẻ 9, 10, 11, 12, 13)
```

---

## 👤 Thành viên PN — Cấu trúc Nền tảng & Xác thực

**Nhánh:** `feature/PN-setup-auth`

```bash
# Bước 1: Clone repo về máy
git clone <URL_REPO> QuanLiPhongTro
cd QuanLiPhongTro

# Bước 2: Checkout nhánh PN
git checkout feature/PN-setup-auth

# Bước 3: Push nhánh lên remote
git push origin feature/PN-setup-auth
```

**Commits của PN (4 thẻ):**
- Setup project + Scaffold Models
- Program.cs: DbContext + Session config
- _Layout.cshtml + Admin & User navbar
- Xây dựng hệ thống tài khoản & Profile

---

## 👤 Thành viên PB — Nghiệp vụ Cốt lõi Admin

**Nhánh:** `feature/PB-admin-core`

```bash
git checkout feature/PB-admin-core
git push origin feature/PB-admin-core
```

**Commits của PB (4 thẻ):**
- Xây dựng các lớp Helpers & SystemConstants
- Quản lý danh mục (Loại Phòng & Tiện Ích)
- Quản lý Phòng trọ & Trạng thái phòng
- Nghiệp vụ Hợp đồng và Khách thuê (Admin)

---

## 👤 Thành viên QV — Dòng tiền, Portal Khách & Báo cáo

**Nhánh:** `feature/QV-billing-user-report`

```bash
git checkout feature/QV-billing-user-report
git push origin feature/QV-billing-user-report
```

**Commits của QV (5 thẻ):**
- Quản lý Chỉ số điện nước và Hóa đơn
- Quản lý lịch sử Thanh toán & Yêu cầu
- Xây dựng giao diện Khách thuê (User Dashboard)
- Quản lý lịch sử và Hóa đơn khách hàng (User)
- Tổng quan và Báo cáo (Analytics)

---

## Merge về main (Team lead làm)

```bash
git checkout main
git merge feature/PN-setup-auth
git merge feature/PB-admin-core
git merge feature/QV-billing-user-report
git push origin main
```

> **Lưu ý:** Khi merge sẽ không có conflict vì mỗi nhánh chạm vào các file khác nhau hoàn toàn.

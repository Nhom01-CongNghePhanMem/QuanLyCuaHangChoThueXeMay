# 📦 Dự Án Quản Lý Cửa Hàng Cho Thuê Xe Máy

## 🔰 Mô tả dự án

Phần mềm hỗ trợ cửa hàng xe máy trong việc quản lý thuê – trả xe, hợp đồng, khách hàng, nhân viên và thống kê doanh thu một cách rõ ràng, khoa học. Phát triển bằng công nghệ ASP.NET Core MVC.

---

## 🎯 Các chức năng chính

### 1. Quản lý xe cho thuê
- Mỗi xe có:
  - Loại xe: tay ga, xe số, v.v.
  - Thương hiệu: Honda, Yamaha,...
  - Mức độ mới/cũ → ảnh hưởng đến giá thuê.
- Cho thuê theo **giờ** hoặc **ngày**.

### 2. Quản lý hợp đồng thuê xe
- Lập hợp đồng mỗi lần thuê.
- Giữ lại căn cước công dân (CCCD) của khách thuê.
- Khi trả xe:
  - Trả xe.
  - Thanh toán tiền thuê.
  - Trả lại CCCD.
  - Thanh lý hợp đồng.

### 3. Quản lý khách hàng
- Phân loại: khách hàng cũ, khách hàng mới.
- Cho phép **giảm giá thủ công** khi xuất hóa đơn.

### 4. Quản lý giá thuê
- Tính giá theo giờ hoặc ngày.
- Hỗ trợ giảm giá ví dụ:
  - 3 ngày x 50.000đ = 150.000đ → giảm còn 120.000đ.

### 5. Quản lý tình trạng xe
- Ghi nhận các sự cố:
  - Xe mất mốc.
  - Bị công an giữ.
  - Té xe, thu xe,...
- Xử lý tuỳ theo mức độ.

### 6. Thống kê – Báo cáo
- Theo khoảng thời gian:
  - Số lượng xe thuê.
  - Loại xe/hiệu xe nào được thuê nhiều.
  - Tổng doanh thu.
- Trạng thái xe: xe đang thuê / xe rảnh.

### 7. Quản lý nhân viên
- Khoảng 3–5 nhân viên cho ~40–50 xe.
- Phân quyền:
  - Giao xe.
  - Nhận xe.
  - Lập hợp đồng.
  - Xuất hóa đơn.

### 8. Giao diện – Người dùng
- **Quản lý**: thống kê, kiểm duyệt hợp đồng, duyệt giảm giá.
- **Nhân viên**: thực hiện nghiệp vụ thường ngày.

---

## 🚀 Công nghệ sử dụng
- Backend: ASP.NET Core MVC
- Database: SQL Server
- Frontend: Razor Pages, Bootstrap
- ORM: Entity Framework Core

---

## 📌 Ghi chú
- Dự án hướng đến **quản lý nội bộ** và có thể mở rộng thành web online trong tương lai.


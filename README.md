# FA25_PRN222_SE1834_ASM1_QE180078_ToanHH

## Giới thiệu dự án

Đây là dự án quản lý đại lý và hợp đồng xe, được phát triển trong khuôn khổ môn học PRN222 tại trường Đại học FPT. Dự án sử dụng công nghệ .NET 8, Entity Framework Core, và kiến trúc Repository Pattern để đảm bảo khả năng mở rộng, bảo trì và quản lý dữ liệu hiệu quả.

## Tính năng nổi bật

- Quản lý thông tin đại lý xe.
- Quản lý hợp đồng xe của từng đại lý.
- Tìm kiếm, lọc, phân trang dữ liệu hợp đồng và đại lý.
- Quản lý tài khoản đăng nhập hệ thống.
- Áp dụng Repository Pattern giúp tách biệt logic truy xuất dữ liệu và nghiệp vụ.

## Công nghệ sử dụng

- .NET 8
- Entity Framework Core
- SQL Server
- Visual Studio Code

## Hướng dẫn sử dụng

1. Clone dự án về máy:
   ```bash
   git clone <repo-url>
   ```

2. Cấu hình chuỗi kết nối trong `appsettings.json`.

3. Chạy lệnh migrate để khởi tạo database (nếu cần):
   ```bash
   dotnet ef database update
   ```

4. Build và chạy ứng dụng:
   ```bash
   dotnet build
   dotnet run
   ```

## Lưu ý bảo mật

- Không push file `appsettings.json` chứa thông tin nhạy cảm lên GitHub.
- Đảm bảo `.gitignore` đã cấu hình đúng để bảo vệ thông tin cấu hình.

## Tác giả

Yujihiro
---


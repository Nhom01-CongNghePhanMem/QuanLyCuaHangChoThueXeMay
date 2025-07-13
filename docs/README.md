# 🏍️ Hệ Thống Quản Lý Cửa Hàng Cho Thuê Xe Máy

<div align="center">

![.NET](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Vue.js](https://img.shields.io/badge/Vue.js%203-4FC08D?style=for-the-badge&logo=vue.js&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

</div>

## 📖 Tổng Quan Dự Án

### 🎯 Mục Tiêu
Phần mềm hỗ trợ cửa hàng xe máy trong việc quản lý thuê – trả xe, hợp đồng, khách hàng, nhân viên và thống kê doanh thu một cách rõ ràng, khoa học. Phát triển bằng công nghệ hiện đại với kiến trúc Clean Architecture.

### ✨ Tính Năng Chính

#### 🏍️ Quản Lý Xe Cho Thuê
- Quản lý thông tin xe: loại xe, thương hiệu, tình trạng
- Theo dõi lịch sử bảo trì và sự cố
- Tính giá thuê theo giờ/ngày với hệ thống giảm giá linh hoạt

#### 📋 Quản Lý Hợp Đồng & Khách Hàng
- Lập hợp đồng thuê xe tự động
- Quản lý thông tin CCCD và thanh toán
- Phân loại khách hàng (mới/cũ) với chính sách giảm giá

#### 👥 Quản Lý Nhân Viên & Phân Quyền
- Hệ thống JWT Authentication & Authorization
- Phân quyền chi tiết: Admin, Manager, Receptionist
- Theo dõi hoạt động và báo cáo hiệu suất

#### 📊 Thống Kê & Báo Cáo
- Dashboard thống kê theo thời gian thực
- Báo cáo doanh thu, xe được thuê nhiều nhất
- Trạng thái xe: đang thuê/rảnh

### 🚀 Công Nghệ Nổi Bật
- **Clean Architecture**: Tách biệt rõ ràng các tầng, dễ bảo trì và mở rộng
- **SOLID Principles**: Thiết kế hướng đối tượng tối ưu
- **JWT Authentication**: Bảo mật cao với token-based auth
- **Background Services**: Xử lý tác vụ nền (cleanup, notifications)
- **FluentValidation**: Validation logic mạnh mẽ và linh hoạt

---

## 🔧 Công Nghệ Sử Dụng

### Backend (.NET 8)
- **Framework**: ASP.NET Core 8 Web API
- **ORM**: Entity Framework Core 9
- **Database**: SQL Server
- **Authentication**: JWT Bearer Token
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Background Jobs**: BackgroundService
- **Email Service**: SendGrid
- **Password Hashing**: BCrypt.Net

### Frontend (Vue 3)
- **Framework**: Vue 3 với Composition API
- **Language**: TypeScript
- **Build Tool**: Vite
- **HTTP Client**: Axios
- **State Management**: Pinia
- **Routing**: Vue Router 4
- **Styling**: CSS3 + Modern Design

---

## 🏗️ Kiến Trúc Backend - Clean Architecture

Dự án áp dụng **Clean Architecture** với 4 tầng chính, tuân thủ nguyên lý **Dependency Inversion** và **SOLID Principles**.

### 📁 Sơ Đồ Thư Mục Backend

```
backend/src/MotorbikeRental/
├── 🟦 MotorbikeRental.Domain/           # Core Business Logic
│   ├── Entities/                       # Domain Entities
│   │   ├── Contract/                   # RentalContract, Payment
│   │   ├── Incidents/                  # Incident, IncidentImage
│   │   ├── Pricing/                    # PriceList, Discount
│   │   ├── User/                       # Employee, Customer, UserCredentials
│   │   └── Vehicles/                   # Motorbike, Category, MaintenanceRecord
│   ├── Enums/                         # Business Enums
│   │   ├── ContractEnum/              # ContractStatus, PaymentStatus
│   │   ├── IncidentEnum/              # SeverityStatus
│   │   ├── UserEnum/                  # UserRole, EmployeeStatus
│   │   └── VehicleEnum/               # MotorbikeStatus, ConditionStatus
│   └── Interfaces/                    # Domain Interfaces & Contracts
│
├── 🟨 MotorbikeRental.Application/      # Application Business Rules
│   ├── Common/                        # Shared Application Logic
│   ├── DTOs/                         # Data Transfer Objects
│   │   ├── AuthDTOs/                 # Login, Register DTOs
│   │   ├── ContractDTOs/             # Contract Management DTOs
│   │   ├── DiscountDTOs/             # Discount Management DTOs
│   │   ├── EmployeeDTOs/             # Employee Management DTOs
│   │   └── VehicleDTOs/              # Vehicle Management DTOs
│   ├── Exceptions/                   # Custom Application Exceptions
│   ├── Interface/                    # Application Service Interfaces
│   │   ├── IServices/                # Service Contracts
│   │   └── IValidators/              # Validation Contracts
│   ├── Mappers/                      # AutoMapper Profiles
│   ├── Services/                     # Business Logic Implementation
│   │   ├── AuthServices/             # Authentication & Authorization
│   │   ├── ContractServices/         # Contract Management
│   │   ├── DiscountServices/         # Discount Management
│   │   ├── EmployeeServices/         # Employee Management
│   │   └── VehicleServices/          # Vehicle Management
│   └── Validators/                   # FluentValidation Rules
│       ├── ContractValidators/       # Contract Validation
│       ├── EmployeeValidators/       # Employee Validation
│       └── VehicleValidators/        # Vehicle Validation
│
├── 🟩 MotorbikeRental.Infrastructure/   # External Concerns
│   ├── BackgroundJobs/               # Background Services
│   │   └── DiscountCleanupService.cs # Auto-cleanup expired discounts
│   ├── Constants/                    # Infrastructure Constants
│   ├── Data/                        # Data Access Layer
│   │   ├── Contexts/                # DbContext & Configurations
│   │   └── Repositories/            # Repository Pattern Implementation
│   │       ├── ContractRepositories/ # Contract Data Access
│   │       ├── CustomerRepositories/ # Customer Data Access
│   │       ├── IncidentRepositories/ # Incident Data Access
│   │       ├── PricingRepositories/  # Pricing Data Access
│   │       ├── UserRepositories/     # User Data Access
│   │       └── VehicleRepositories/  # Vehicle Data Access
│   ├── ExternalServices/             # External API Integrations
│   └── Migrations/                   # EF Core Database Migrations
│
└── 🟥 MotorbikeRental.API/             # Presentation Layer
    ├── Controllers/                  # API Controllers
    │   ├── AuthController.cs         # Authentication Endpoints
    │   ├── CategoryController.cs     # Vehicle Category Management
    │   ├── ContractController.cs     # Contract Management
    │   ├── CustomerController.cs     # Customer Management
    │   ├── DiscountController.cs     # Discount Management
    │   ├── EmployeeController.cs     # Employee Management
    │   ├── MotorbikeController.cs    # Motorbike Management
    │   └── UserCredentialsController.cs # User Management
    ├── Extensions/                   # Service Registration Extensions
    │   ├── SecurityExtension.cs      # JWT & Auth Configuration
    │   └── ServiceExtension.cs       # DI Container Setup
    ├── Middlewares/                  # Custom Middlewares
    │   ├── ExceptionHandlingMiddleware.cs  # Global Exception Handler
    │   └── RequestResponseLoggingMiddleware.cs # Request/Response Logging
    ├── Properties/                   # Project Properties
    ├── wwwroot/                     # Static Files
    │   └── uploads/                 # File Upload Storage
    ├── appsettings.json             # Configuration
    └── Program.cs                   # Application Entry Point
```

### 🎯 Chi Tiết Từng Tầng

#### 🟦 Domain Layer (Core)
**Vai trò**: Chứa business logic thuần túy, không phụ thuộc vào bất kỳ tầng nào khác.

**Thành phần chính**:
- **Entities**: Domain objects chứa business rules
- **Enums**: Các hằng số nghiệp vụ
- **Interfaces**: Contracts cho repository và services

**SOLID áp dụng**:
- **SRP**: Mỗi entity chỉ đảm nhiệm một nghiệp vụ cụ thể
- **OCP**: Entities có thể mở rộng thông qua inheritance
- **DIP**: Chỉ chứa abstractions, không phụ thuộc implementations

#### 🟨 Application Layer (Use Cases)
**Vai trò**: Orchestration layer, điều phối business logic và external concerns.

**Thành phần chính**:
- **DTOs**: Data contracts cho API communication
- **Services**: Implement business use cases
- **Validators**: FluentValidation rules
- **Mappers**: AutoMapper profiles

**SOLID áp dụng**:
- **SRP**: Mỗi service đảm nhiệm một domain cụ thể
- **ISP**: Interfaces được tách nhỏ theo chức năng
- **DIP**: Phụ thuộc vào abstractions từ Domain layer

**Dependencies**: `Domain` ← `Application`

#### 🟩 Infrastructure Layer (Technical Details)
**Vai trò**: Implement technical concerns như database, external APIs, file system.

**Thành phần chính**:
- **Repositories**: EF Core implementation của domain interfaces
- **DbContext**: Database configuration và mapping
- **BackgroundServices**: Scheduled tasks
- **ExternalServices**: Third-party integrations

**SOLID áp dụng**:
- **DIP**: Implement interfaces định nghĩa trong Application layer
- **SRP**: Mỗi repository chỉ quản lý một aggregate root
- **OCP**: Có thể thay đổi database provider mà không ảnh hưởng business logic

**Dependencies**: `Domain` ← `Application` ← `Infrastructure`

#### 🟥 API Layer (Presentation)
**Vai trò**: HTTP interface, authentication, authorization, và request/response handling.

**Thành phần chính**:
- **Controllers**: REST API endpoints
- **Middlewares**: Cross-cutting concerns
- **Extensions**: DI configuration
- **Filters**: Request/Response processing

**SOLID áp dụng**:
- **SRP**: Controllers chỉ đảm nhiệm HTTP handling
- **DIP**: Phụ thuộc vào Application services
- **OCP**: Middleware pipeline có thể mở rộng

**Dependencies**: `Domain` ← `Application` ← `Infrastructure` ← `API`

---

## 🎨 Frontend Architecture (Vue 3 + TypeScript)

### 📁 Sơ Đồ Thư Mục Frontend

```
frontend/MotorbikeRental.UI/src/
├── 📱 api/                          # API Layer
│   ├── config/                     # HTTP Client Configuration
│   └── services/                   # API Service Classes
│       ├── authService.ts          # Authentication APIs
│       ├── motorbikeService.ts     # Vehicle Management APIs
│       ├── contractService.ts      # Contract Management APIs
│       └── userService.ts          # User Management APIs
│
├── 🎨 assets/                      # Static Assets
│   ├── images/                     # Image Files
│   ├── icons/                      # Icon Assets
│   └── styles/                     # Global CSS/SCSS
│
├── 🧩 components/                  # Reusable Components
│   ├── Admin/                      # Admin-specific Components
│   │   ├── Dashboard/              # Admin Dashboard Components
│   │   ├── Reports/                # Report Components
│   │   └── UserManagement/         # User Management Components
│   ├── Receptionist/               # Receptionist Components
│   │   ├── ContractManagement/     # Contract Handling
│   │   ├── CustomerService/        # Customer Service Components
│   │   └── VehicleManagement/      # Vehicle Operations
│   └── Common/                     # Shared Components
│       ├── Layout/                 # Layout Components
│       ├── Forms/                  # Form Components
│       └── UI/                     # Basic UI Components
│
├── 🔧 composables/                 # Vue 3 Composition Functions
│   ├── useAuth.ts                  # Authentication Logic
│   ├── useMotorbike.ts             # Vehicle Management Logic
│   └── useContract.ts              # Contract Management Logic
│
├── 🛠️ lib/                        # Utility Libraries
│   ├── validators/                 # Form Validation
│   ├── formatters/                 # Data Formatting
│   └── constants/                  # Application Constants
│
├── 🗂️ router/                     # Vue Router Configuration
│   ├── index.ts                    # Main Router
│   ├── guards/                     # Route Guards
│   └── routes/                     # Route Definitions
│
├── 💾 stores/                      # Pinia State Management
│   ├── auth.ts                     # Authentication State
│   ├── motorbike.ts                # Vehicle State
│   ├── contract.ts                 # Contract State
│   └── user.ts                     # User State
│
├── 🔧 utils/                       # Utility Functions
│   ├── api.ts                      # API Helpers
│   ├── date.ts                     # Date Utilities
│   ├── format.ts                   # Format Helpers
│   └── validation.ts               # Validation Utilities
│
└── 📄 views/                       # Page Components
    ├── Auth/                       # Authentication Pages
    │   ├── Login.vue               # Login Page
    │   └── Register.vue            # Registration Page
    ├── Admin/                      # Admin Pages
    │   ├── Dashboard.vue           # Admin Dashboard
    │   ├── UserManagement.vue      # User Management
    │   └── Reports.vue             # Reports & Analytics
    ├── Receptionist/               # Receptionist Pages
    │   ├── Dashboard.vue           # Receptionist Dashboard
    │   ├── ContractCreate.vue      # Create Contract
    │   ├── ContractList.vue        # Contract Management
    │   └── VehicleManagement.vue   # Vehicle Operations
    └── layouts/                    # Layout Templates
        ├── AppLayout.vue           # Main App Layout
        └── AuthLayout.vue          # Authentication Layout
```

### 📂 Mô Tả Chi Tiết Từng Folder

| Folder | Mục Đích | Ví Dụ |
|--------|----------|-------|
| **📱 api/** | Quản lý tất cả API calls, HTTP client configuration | `authService.ts`, `motorbikeService.ts` |
| **🎨 assets/** | Chứa static files: images, icons, global styles | `logo.png`, `main.css` |
| **🧩 components/** | Reusable Vue components, chia theo module/role | `MotorbikeCard.vue`, `ContractForm.vue` |
| **🔧 composables/** | Vue 3 Composition API logic, business logic tái sử dụng | `useAuth()`, `useMotorbike()` |
| **🛠️ lib/** | Utility libraries, helpers, constants | `validators.ts`, `formatters.ts` |
| **🗂️ router/** | Vue Router config, route definitions, guards | `index.ts`, `authGuard.ts` |
| **💾 stores/** | Pinia stores cho global state management | `authStore`, `motorbikeStore` |
| **🔧 utils/** | Pure utility functions, helpers | `dateUtils.ts`, `apiHelpers.ts` |
| **📄 views/** | Page-level components, route components | `Dashboard.vue`, `Login.vue` |

---

## ⚙️ Cài Đặt & Chạy Dự Án

### 📋 Yêu Cầu Hệ Thống
- **.NET 8 SDK** (hoặc mới hơn)
- **Node.js 18+** và **npm/yarn**
- **SQL Server** (Local/Express/Cloud)
- **Visual Studio 2022** hoặc **VS Code**

### 🚀 Hướng Dẫn Chạy Backend

#### 1️⃣ Clone Repository
```bash
git clone https://github.com/your-repo/QuanLyCuaHangChoThueXeMay.git
cd QuanLyCuaHangChoThueXeMay/backend/src/MotorbikeRental
```

#### 2️⃣ Cấu Hình Database
```bash
# Cập nhật connection string trong appsettings.json
{
  "ConnectionStrings": {
    "MotorbikeRentalDB": "Server=(localdb)\\mssqllocaldb;Database=MotorbikeRentalDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

#### 3️⃣ Restore Packages & Migration
```bash
# Restore NuGet packages
dotnet restore

# Tạo database và apply migrations
dotnet ef database update --project MotorbikeRental.Infrastructure --startup-project MotorbikeRental.API
```

#### 4️⃣ Cấu Hình JWT & SendGrid
Cập nhật `appsettings.json`:
```json
{
  "AppSettings": {
    "JwtSecretKey": "your-super-secret-jwt-key-here-minimum-32-characters",
    "JwtIssuer": "MotorbikeRental.API",
    "JwtAudience": "MotorbikeRental.Client",
    "JwtExpirationInDays": 7
  },
  "SendGrid": {
    "ApiKey": "your-sendgrid-api-key",
    "SenderEmail": "noreply@motorbikerental.com",
    "SenderName": "Motorbike Rental System"
  }
}
```

#### 5️⃣ Chạy Backend
```bash
# Chạy từ thư mục MotorbikeRental.API
dotnet run

# Hoặc với hot reload
dotnet watch run
```

**API sẽ chạy tại**: `https://localhost:7000` và `http://localhost:5000`  
**Swagger UI**: `https://localhost:7000/swagger`

---

### 🎨 Hướng Dẫn Chạy Frontend

#### 1️⃣ Điều Hướng Đến Frontend
```bash
cd frontend/MotorbikeRental.UI
```

#### 2️⃣ Install Dependencies
```bash
# Sử dụng npm
npm install

# Hoặc yarn
yarn install
```

#### 3️⃣ Cấu Hình Environment
Tạo file `.env.local`:
```env
# API Base URL
VITE_API_BASE_URL=https://localhost:7000/api

# App Configuration
VITE_APP_NAME=Motorbike Rental Management
VITE_APP_VERSION=1.0.0

# Features Flags
VITE_ENABLE_LOGGING=true
VITE_ENABLE_ANALYTICS=false
```

#### 4️⃣ Chạy Development Server
```bash
# Development mode với hot reload
npm run dev

# Hoặc
yarn dev
```

#### 5️⃣ Build cho Production
```bash
# Build production
npm run build

# Preview production build
npm run preview
```

**Frontend sẽ chạy tại**: `http://localhost:5174`

---

### 🔧 Cấu Hình Bổ Sung

#### 📧 SendGrid Email Service
1. Đăng ký tài khoản [SendGrid](https://sendgrid.com)
2. Tạo API Key và cập nhật vào `appsettings.json`
3. Verify sender email

#### 🔐 JWT Configuration
- **Secret Key**: Tối thiểu 32 ký tự, nên sử dụng random generator
- **Expiration**: Mặc định 7 ngày, có thể điều chỉnh theo nhu cầu bảo mật
- **Issuer/Audience**: Phân biệt các client khác nhau

#### 📊 Logging & Monitoring
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "MotorbikeRental": "Debug"
    }
  }
}
```

---

## 🤝 Đóng Góp

1. Fork repository
2. Tạo feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Tạo Pull Request

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.

## 📞 Liên Hệ

- **Project Link**: [https://github.com/your-repo/QuanLyCuaHangChoThueXeMay](https://github.com/your-repo/QuanLyCuaHangChoThueXeMay)
- **Email**: support@motorbikerental.com

---

<div align="center">

**⭐ Nếu project hữu ích, đừng quên cho một star! ⭐**

Made with ❤️ by [Your Team Name]

</div>

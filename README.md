# MyAlbum - Multi-Role MVC Album Platform

一個使用 ASP.NET Core MVC 開發的多角色雲端相簿平台。  
本專案展示完整的前後台分離設計、角色權限控制、圖片管理機制與商業邏輯限制。

---

## 🔧 Tech Stack

- .NET 10
- ASP.NET Core MVC
- SQL Server
- DB First
- LINQ
- Cookie Authentication (Admin / Member 分離)
- Docker (Optional runtime support)

---

## 🏗 Architecture Overview

系統分為三種角色：

| Role | Description |
|------|------------|
| Visitor | 瀏覽相簿與熱門評論 |
| Member | 建立相簿、上傳照片、留言互動 |
| Admin | 後台完整 CRUD 管理 |

專案使用：

- Area 分離（Admin / Member）
- Master / Slave 連線設計
- 商業邏輯限制（不可評論自己照片）
- 首頁熱門評論排序機制（依留言數降冪）

---

## 🗂 Core Domain Tables

- Member
- Employee
- AlbumCategory
- Album
- AlbumPhoto
- AlbumComment

資料關聯：

```
Album (1)
 └── AlbumPhoto (N)
       └── AlbumComment (N)
```

---

## ⭐ Business Logic Highlights

- 首頁「熱門評論」依照片總留言數排序
- 會員不可對自己的照片留言
- 相片管理支援一次上傳多張圖片
- 圖片儲存路徑可透過環境變數覆蓋
- 多角色 Cookie 驗證分離

---

# 🚀 Local Development

## 1️⃣ appsettings.json

```json
"Upload": {
  "RootPath": "",
  "MemberImgRoot": "MemberImages",
  "CoverImgRoot": "CoverImages",
  "PhotoImgRoot": "PhotoImages"
}
```

---

## 2️⃣ appsettings.Development.json

```json
"ConnectionStrings": {
  "MasterConnection": "",
  "SlaveConnection": ""
},
"Upload": {
  "RootPath": "C:\\Album"
}
```

---

## 3️⃣ 使用 User-Secrets 設定資料庫連線

```bash
dotnet user-secrets set "ConnectionStrings:MasterConnection" "YourConnectionString"
dotnet user-secrets set "ConnectionStrings:SlaveConnection" "YourConnectionString"
```

---

# 🧪 Test Flow

---

## 🔐 Admin Testing

1. `/Admin/Identity/Login`
2. 管理相簿類別（預設：熱門景點、精緻美食、平價小吃、節慶活動）
3. 新增員工並測試登入
4. 新增會員
5. 建立相簿
6. 一次上傳多張相片
7. 查看留言預覽

---

## 🌐 Visitor Testing

1. 首頁查看熱門評論（依留言數排序）
2. 點擊圖片放大
3. 切換相簿類別瀏覽
4. 註冊會員
5. 會員登入

---

## 👤 Member Testing

1. 登入後右上角進入會員區
2. 建立相簿並上傳封面
3. 相片管理 → 一次新增多張照片
4. 查看留言
5. 使用第二帳號測試留言功能
6. 驗證不可評論自己照片

---

# 🗄 Database Initialization

SQL Script 位置：

```
MyAlbum.Infrastructure/sql/
```

包含：

- Admin 帳號：`admin / 123456`
- 測試會員：`mark / 123456`

---

# 🐳 Docker Support (Optional)

本專案支援透過 Docker 執行。

### Build Image

```bash
docker build -t myalbum-web:dev .
```

### Run Container

```bash
docker run --rm -it ^
  -p 8080:8080 ^
  -e ASPNETCORE_ENVIRONMENT=Production ^
  -e Upload__RootPath=/data/album ^
  -e ConnectionStrings__MasterConnection="XXXXXX" ^
  -e ConnectionStrings__SlaveConnection="XXXXXX" ^
  -v C:\Album:/data/album ^
  myalbum-web:dev
```

說明：

- 使用環境變數覆蓋設定
- Volume 掛載圖片儲存路徑
- 可於 Production 模式運行

---

# 📌 Engineering Highlights

- Area-based Role Separation
- Multi-connection DB design
- Business rule enforcement
- Environment-based configuration override
- Docker runtime support
- Production-ready project structure

---

# 👨‍💻 Author

Wei Chung

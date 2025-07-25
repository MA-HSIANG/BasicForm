# BasicForm

> 歡迎使用 BasicForm，此為下班空閒時所寫基於[AntdUI](https://github.com/AntdUI/AntdUI)控件實作RBAC功能管理系統。

## 增強
1. 可依照自己需求擴展如:多DbContext機制、基於按鈕權限設計、UnitOfWork、AutoMapper
2. 該項目純粹練習AntdUI，或當有需求時可快速展開一個項目所設計。程式碼內還可在優化或新增。

## 預設
1. 該專案預設使用SqlServer。
2. ORM框架使用Microsoft.EntityFrameworkCore。
3. 採多層次架構設定並保留DAL、BLL命名方式。
4. 採用Base64加密連接字符串依照加密程式自動獲取加密後連接字符串

## 目錄
BasicForm                  🖥️ UI 表現層（WinForms 介面）
├─ Controls                👉 自訂 UI 控制項（按鈕、輸入框、列表等）
├─ Helpers                 👉 UI 輔助工具（控制項操作、樣式處理）
└─ Pages                   👉 各功能頁面（登入、主頁、設定頁等）

BasicForm.BLL             ⚙️ 業務邏輯層（Service 層）
├─ Base                    👉 泛型服務基底類（IBaseService / BaseService）
└─ Interface               👉 模組服務介面定義（IUserService、ILogService 等）

BasicForm.DAL             🗄️ 資料存取層（Repository 層）
└─ Base                    👉 泛型資料倉儲（IBaseRepository / BaseRepository）

BasicForm.Common          🧰 公共工具層（通用設定與支援模組）
├─ App                     👉 全域設定（常數、快取 Key、共用旗標）
├─ DB                      👉 資料庫相關模組
│   ├─ DbContexts           👉 EF Core 資料上下文類別
│   └─ EF                   👉 DbContext 工廠方法（支援多資料庫）
├─ Helpers                 👉 工具類別（加解密、轉型、驗證等通用方法）
└─ Settings                👉 AppSettings 設定模型（對應 appsettings.json）

BasicForm.Model           📦 資料模型層（Entity 與 DTO）
├─ Base                    👉 模型基底類別（含 KeyModel、雙向綁定支援）
└─ Dtos                    👉 資料傳輸物件（DTO，提供 UI 與 Service 傳遞資料）
  
## 套件版本
1. AntdUI - 2.0.9
2. Autofac - 8.3.0
3. Autofac.Extensions.DependencyInjection 10.0.0
4. Microsoft.EntityFrameworkCore - 9.0.6
5. Microsoft.EntityFrameworkCore.Design - 9.0.6
6. Microsoft.EntityFrameworkCore.SqlServer - 9.0.6
7. Microsoft.EntityFrameworkCore.Tools - 9.0.6
8. Microsoft.Extensions.Configuration - 9.0.6
9. Microsoft.Extensions.Configuration.Binder - 9.0.6
10. Microsoft.Extensions.Configuration.Json - 9.0.6
11. Microsoft.Extensions.Options - 9.0.6
12. Microsoft.Extensions.Options.ConfigurationExtensions - 9.0.6


## 框架模組
1. 採用倉儲 + 服務 + 接口
2. 異步開發 (async/await)
3. 面相介面開發 (interface)
4. 採Autofac依賴注入，全組件、BLL、DAL 登入掃描注入

## 展示

![1753453938110](https://github.com/user-attachments/assets/7c95f4c9-79dc-41d1-8bfc-f3554c37a2f4)

![1753453938203](https://github.com/user-attachments/assets/4defc87e-94e0-486f-8ffb-a0425dac895a)

![1753453938233](https://github.com/user-attachments/assets/84bc0d5e-b14a-4f7d-9806-ef6571602318)

![1753453938361](https://github.com/user-attachments/assets/29988621-2d61-4002-af25-df0e621c4b83)

![1753453938470](https://github.com/user-attachments/assets/62746861-5416-4637-9103-9e9b4a048e5a)

# 程式架構與設計

本文件整理專案技術選型、模組分層與核心設計概念。

## 1. 技術選型
- 語言：C#
- 平台：WinForms
- 目標框架：.NET 10
- 資料庫：LiteDB 5.0.21（嵌入式 NoSQL）

## 2. 架構分層
- `Models`：資料模型（Restaurant、UserProfile、MealRecord 等）
- `Repositories`：資料存取層（底層 CRUD 操作，支援 JSON 與 LiteDB）
- `Services/Interfaces`：服務契約定義（IFavoriteService、IBlockedService 等）
- `Services/Adapters`：服務適配器（LiteDbFavoriteService、LiteDbBlockedService 等）
- `Services`：商業邏輯服務（RandomPickService、RestaurantFilterService 等）
- `Views`：WinForms UI 層（MainForm、MealSelectForm 等）

## 3. 核心服務說明

### 3.1 資料存取服務
- **IRestaurantRepository**：餐廳資料存取契約
  - 實作：LiteDbRestaurantRepository（使用 LiteDB）
  - 初始化時自動從 restaurants.csv 匯入資料

- **IFavoriteService**：收藏管理服務契約
  - 實作：FavoriteService（JSON）、LiteDbFavoriteService（LiteDB 適配器）
  - 新增 ClearByUserId() 方法支援 Reset Data

- **IBlockedService**：封鎖管理服務契約
  - 實作：BlockedService（JSON）、LiteDbBlockedService（LiteDB 適配器）
  - 新增 ClearByUserId() 方法支援 Reset Data

- **IUserProfileService**：使用者資料管理服務契約
  - 實作：UserProfileService（JSON）、LiteDbUserProfileService（LiteDB 適配器）

- **IMealRecordService**：用餐紀錄管理服務契約
  - 實作：LiteDbMealRecordService（LiteDB 適配器）

### 3.2 業務邏輯服務
- **RandomPickService**：隨機候選餐廳產生
- **RestaurantFilterService**：依時間與標籤篩選
- **SwipeMatchService**：二選一推薦流程
- **TodayMealService**：今日餐廳紀錄
- **UserPreferenceService**：使用者偏好與 MealHistory 管理
- **LanguageManager**：多語言管理

## 4. 介面驅動設計（Interface-Driven Design）

### 4.1 設計目的
- **解耦合**：UI 層不直接依賴具體服務實作，而是透過介面通訊
- **可測試性**：便於單元測試時注入 Mock 實作
- **可替換性**：支援快速切換不同的資料層實作（JSON ↔ LiteDB）

### 4.2 應用範圍
- MainForm、MealSelectForm、CategorySelectForm、SwipeForm、ResultForm、ManagePreferenceForm 均使用 IFavoriteService、IBlockedService、IUserProfileService 等介面
- Program.cs 進行依賴注入（目前注入 JSON 版本，可改為注入 LiteDB 適配器）

## 5. 適配器模式（Adapter Pattern）

### 5.1 為何需要適配器？
- 舊系統使用 JSON 檔儲存（FavoriteService、BlockedService）
- 新系統逐步遷移至 LiteDB（LiteDbFavoriteRepository 等）
- 適配器使新舊系統能相容，無需重寫 UI 層

### 5.2 適配器結構
```
IFavoriteService (介面)
  ├─ FavoriteService (JSON 實作)
  └─ LiteDbFavoriteService (LiteDB 實作，內部使用 LiteDbFavoriteRepository)
```

### 5.3 切換方式（在 Program.cs）
```csharp
// 當前：使用 JSON
var favoriteService = new FavoriteService(favoritePath);

// 未來：使用 LiteDB（只需改一行）
var favoriteService = new LiteDbFavoriteService(databasePath);
```

## 6. 資料存取與儲存

### 6.1 當前配置
- **餐廳資料**：LiteDB (restaurantpicker.db)
- **使用者資料**：JSON (users.json)
- **收藏與封鎖**：JSON (favorites.json, blocked.json)
- **用餐紀錄**：JSON 內嵌於 user_preferences.json

### 6.2 建議遷移路徑
1. 驗證 LiteDB 適配器與 JSON 相容性
2. 逐集合遷移（先 favorites → blocked → users）
3. 在 Program.cs 切換依賴注入配置
4. 執行資料完整性檢查
5. 妥善保留 JSON 備份

## 7. 新增功能：Reset Data

### 7.1 功能說明
- 使用者點擊「Reset Data」後，系統會同時清除：
  - 用餐歷史紀錄（MealHistory）
  - 喜愛項目清單（Favorites）
  - 封鎖項目清單（Blocked）

### 7.2 實作方式
- 在 IFavoriteService 與 IBlockedService 新增 ClearByUserId(userId) 方法
- 在 MainForm.BtnReset_Click 中按序呼叫清除邏輯
- 同時支援 JSON 與 LiteDB 後端

### 7.3 涉及的類別
- IFavoriteService / IBlockedService（介面）
- FavoriteService / BlockedService（JSON 實作）
- LiteDbFavoriteService / LiteDbBlockedService（LiteDB 適配器）
- MainForm.BtnReset_Click（UI 邏輯）

## 8. 擴充性設計
- UI 與服務層分離，避免畫面改動影響邏輯
- 資料格式集中管理，便於資料更新或替換
- 服務介面化，便於單元測試與未來遷移
- 適配器模式降低遷移成本與風險

## 9. 編譯狀態
- ✅ **成功**（0 個錯誤）
- ⚠️ 59 個 nullable 警告（.NET 10 標準檢查，不影響執行）

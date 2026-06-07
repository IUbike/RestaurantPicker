# 資料庫與資料層

本文件說明目前專案中資料庫（LiteDB）與其他資料存放方式的用途、對應程式碼位置與呼叫情況，並在開頭簡單介紹 LiteDB 函式庫與選用理由。

## LiteDB 簡介與選用理由
- LiteDB 是一個輕量、單檔（single-file）的嵌入式 NoSQL 資料庫，針對桌面應用（特別是 .NET）設計。
- 優勢：
  - 無需安裝伺服器，資料以單一檔案儲存，便於打包與備份。
  - API 簡潔，易於在小型至中型桌面應用中直接用 C# 操作。
  - 支援索引、查詢、Upsert 等常見功能，效能與開銷對於本專案需求通常足夠。
- 選用原因：本專案為單機桌面應用（Windows Forms），資料量有限，使用 LiteDB 可減少外部依賴（如 SQL Server），並讓資料以檔案形式與應用程序一同部署。

## 1. 資料來源與存放位置（專案目前實際情況）
- LiteDB 資料庫（預期）：Data/restaurantpicker.db
- 餐廳資料匯入來源（CSV）：Data/restaurants.csv
- 目前仍以 JSON 檔案儲存的資料：
  - Data/users.json
  - Data/user_preferences.json
  - Data/favorites.json
  - Data/blocked.json
- 圖片素材：Assets/images/
- 圖示素材：Assets/icons/

執行時以 AppDomain.CurrentDomain.BaseDirectory 為根目錄讀取資料。

## 2. 專案中 LiteDB 與其他存取實作對照

重要程式檔案（與用途）：

### 2.1 Repositories 層

- **LiteDbRestaurantRepository.cs** — 使用 LiteDB 儲存、查詢 restaurants collection（實作 IRestaurantRepository）。
  - 主要方法：LoadAll(), GetById(id), Add(), Update(), Delete(), SaveAll()
  - 行為：建構時會呼叫 EnsureDatabaseSeeded()，若資料庫為空且存在 restaurants.csv，會由 CsvRestaurantRepository 匯入初始資料（Upsert）。

- **CsvRestaurantRepository.cs** — CSV 檔案的讀寫實作，用於匯入與 CSV 保存。
  - 主要方法：LoadAll(), GetById(id), Add(), Update(), Delete(), SaveAll()

- **LiteDbFavoriteRepository.cs** — LiteDB 收藏 (favorites) 的底層 repository 類別（封裝 favorites collection 的 CRUD）。
  - 主要方法：GetByUserId(userId), Exists(userId, restaurantId), Add(favorite), Remove(userId, restaurantId), DeleteByUserId(userId), Count()

- **LiteDbBlockedRepository.cs** — LiteDB 封鎖 (blocked) 的底層 repository 類別（封裝 blocked collection 的 CRUD）。
  - 主要方法：GetByUserId(userId), Exists(userId, restaurantId), Add(blocked), Remove(userId, restaurantId), DeleteByUserId(userId), Count()

- **LiteDbMealRecordRepository.cs** — LiteDB 用餐紀錄 (meal_records) repository（CRUD 與索引）。
  - 主要方法：LoadAll(), GetByUserId(userId), GetByDate(date, userId), GetById(id), Add(record), Update(record), Delete(id), Count()

- **LiteDbUserProfileRepository.cs** — LiteDB 使用者資料 (users) repository（CRUD）。
  - 主要方法：LoadAll(), GetById(id), Add(profile), Update(profile), Delete(id)

### 2.2 Service 介面層

為支援資料層的靈活迁移與單元測試，已新增以下服務介面：

- **IFavoriteService.cs** — 收藏管理服務介面
  - 方法：LoadAll(), SaveAll(), GetByUserId(userId), IsFavorite(userId, restaurantId), AddFavorite(userId, restaurant), RemoveFavorite(userId, restaurantId), ClearByUserId(userId)

- **IBlockedService.cs** — 封鎖管理服務介面
  - 方法：LoadAll(), SaveAll(), GetByUserId(userId), IsBlocked(userId, restaurantId), AddBlocked(userId, restaurant), RemoveBlocked(userId, restaurantId), ClearByUserId(userId)

- **IUserProfileService.cs** — 使用者資料管理服務介面
  - 方法：LoadAll(), SaveAll(), AddUser(profile), GetById(id), GetAvailableTags(restaurantRepository)

- **IMealRecordService.cs** — 用餐紀錄管理服務介面
  - 方法：LoadAll(), GetByUserId(userId), GetByDate(date, userId), GetById(id), Add(record), Update(record), Delete(id), Count()

### 2.3 Service 實作層

- **FavoriteService.cs** — JSON 檔案版本的收藏服務實作（Data/favorites.json）。
  - 實作 IFavoriteService 介面。
  - 新增 ClearByUserId(userId) 方法用於 Reset Data 功能。

- **BlockedService.cs** — JSON 檔案版本的封鎖服務實作（Data/blocked.json）。
  - 實作 IBlockedService 介面。
  - 新增 ClearByUserId(userId) 方法用於 Reset Data 功能。

- **UserProfileService.cs** — 使用者資料 JSON 服務實作（Data/users.json）。

### 2.4 Service 適配器層（LiteDB 適配）

為逐步遷移至 LiteDB，已新增適配器類別以與現有 UI 保持相容：

- **LiteDbFavoriteService.cs** — LiteDB 收藏服務適配器
  - 實作 IFavoriteService 介面
  - 內部使用 LiteDbFavoriteRepository
  - 新增 ClearByUserId(userId) 方法

- **LiteDbBlockedService.cs** — LiteDB 封鎖服務適配器
  - 實作 IBlockedService 介面
  - 內部使用 LiteDbBlockedRepository
  - 新增 ClearByUserId(userId) 方法

- **LiteDbUserProfileService.cs** — LiteDB 使用者服務適配器
  - 實作 IUserProfileService 介面
  - 內部使用 LiteDbUserProfileRepository

- **LiteDbMealRecordService.cs** — LiteDB 用餐紀錄服務適配器
  - 實作 IMealRecordService 介面
  - 內部使用 LiteDbMealRecordRepository

## 3. 專案現況（哪些 LiteDB 類別實際被呼叫）與遷移進展

### 3.1 當前生產環境設定

在 Program.cs 與 MainForm 中：

- **餐廳資料（Restaurants）**：
  - 已完全遷移至 LiteDB
  - 使用 LiteDbRestaurantRepository(databasePath, csvPath) 作為 IRestaurantRepository 的實作
  - 初始化時會自動從 restaurants.csv 匯入

- **使用者資料（Users）**：
  - **當前**：使用 JSON 檔 (Data/users.json)
  - **計畫**：支援切換至 LiteDB（已實作 LiteDbUserProfileRepository 與 LiteDbUserProfileService）

- **收藏與封鎖（Favorites & Blocked）**：
  - **當前**：使用 JSON 檔 (Data/favorites.json, Data/blocked.json)
  - **計畫**：支援切換至 LiteDB（已實作 LiteDbFavoriteRepository / LiteDbBlockedRepository 與其適配器）
  - **UI 層**：已改用 IFavoriteService / IBlockedService 介面，解耦合具體實作

- **用餐紀錄（Meal Records）**：
  - **當前**：由 UserPreferenceService 以 JSON 方式存放在 Data/user_preferences.json
  - **計畫**：可整合 LiteDbMealRecordRepository 進行專屬管理（已預留實作）

### 3.2 介面驅動設計的優勢

近期重點改進：

1. **分層解耦**：UI Forms 現已透過服務介面與資料層通訊，而非直接呼叫具體服務類別
   - MainForm、MealSelectForm、CategorySelectForm、SwipeForm、ResultForm、ManagePreferenceForm 均已轉換為使用 IFavoriteService、IBlockedService、IUserProfileService 等介面

2. **適配器模式**：
   - LiteDbFavoriteService、LiteDbBlockedService 等適配器實現了 IFavoriteService 等介面
   - 既支援現有的 JSON 儲存方式，也能透過注入適配器快速切換至 LiteDB 後端
   - 無需修改 UI 層程式碼

3. **Reset Data 功能擴展**：
   - 新增 ClearByUserId(userId) 方法至 IFavoriteService 與 IBlockedService
   - 在 MainForm 重置時同時清除偏好檔、喜愛與封鎖項目
   - 底層支援 JSON 與 LiteDB 兩種實作

### 3.3 LiteDB 遷移路徑

完整遷移的預期步驟：

1. **已完成**：餐廳資料（已用 LiteDB）
2. **進行中**：服務層介面化與 LiteDB 適配器實作（已完成類別設計）
3. **待做**：
   - 在 Program.cs 中切換 DI 配置，使用 LiteDbFavoriteService、LiteDbBlockedService 等
   - 執行資料遷移測試（JSON → LiteDB）
   - 驗證各 Collection 索引與查詢效能
   - 更新測試案例與備份脚本

## 4. 資料集合（collection / 檔案）與對應 Model

- restaurants (LiteDB collection) <-> Restaurant model — 主要由 LiteDbRestaurantRepository 操作，初始資料可由 Data/restaurants.csv 匯入。
- users (LiteDB collection) <-> UserProfile model — 有 LiteDbUserProfileRepository，但目前實際讀寫仍為 Data/users.json（UserProfileService）。
- favorites (LiteDB collection) <-> FavoriteRestaurant model — 有 LiteDbFavoriteRepository，但目前實際讀寫仍為 Data/favorites.json（FavoriteService）。
- blocked (LiteDB collection) <-> BlockedRestaurant model — 有 LiteDbBlockedRepository，但目前實際讀寫仍為 Data/blocked.json（BlockedService）。
- meal_records (LiteDB collection) <-> MealRecord model — 有 LiteDbMealRecordRepository，可用於記錄用餐歷史；目前專案中未看到 central service 使用該 repository（可新增整合）。

## 5. 函式 / 方法 對應說明（快速參考）

- LiteDbRestaurantRepository.cs
  - EnsureDatabaseSeeded()：若 restaurants collection 為空且存在 CSV，會用 CsvRestaurantRepository 匯入並 Upsert。
  - LoadAll(), GetById(id), Add(), Update(), Delete(), SaveAll(list)

- CsvRestaurantRepository.cs
  - LoadAll()：解析 Data/restaurants.csv，回傳 List<Restaurant>
  - ParseCsvLine/FormatCsvLine：CSV 解析與格式化細節

- LiteDbFavoriteRepository.cs / LiteDbBlockedRepository.cs
  - GetByUserId(userId), Exists(userId, restaurantId), Add(entity), Remove(userId, restaurantId), Count()

- LiteDbMealRecordRepository.cs
  - LoadAll(), GetByUserId(userId), GetByDate(date, userId), GetById(id), Add(record), Update(record), Delete(id), Count()

- LiteDbUserProfileRepository.cs
  - LoadAll(), GetById(id), Add(profile), Update(profile), Delete(id)

- UserProfileService.cs (JSON)
  - LoadAll(), SaveAll(), AddUser(profile), GetById(id), GetAvailableTags(IRestaurantRepository)

- FavoriteService.cs / BlockedService.cs (JSON)
  - LoadAll(), SaveAll(), GetByUserId(userId), IsFavorite/IsBlocked(), AddFavorite/AddBlocked(), RemoveFavorite/RemoveBlocked()

## 6. 建議與注意事項

### 6.1 資料遷移考量

- **安全遷移策略**：
  - 在進行 JSON → LiteDB 遷移前，應保留 JSON 檔案備份
  - 建議在測試環境中先行驗證資料完整性
  - 可逐步遷移不同的集合（先遷移 favorites，再遷移 blocked、users 等）

- **測試檢查點**：
  - 確保索引建立正確（UserId、RestaurantId 等）
  - 驗證舊資料能否正確讀取
  - 檢查序列化/反序列化相容性（特別是日期時間格式）

### 6.2 開發建議

- **推薦方案**（統一 LiteDB）：
  - 在 Program.cs 中切換依賴注入配置，使用 LiteDbFavoriteService、LiteDbBlockedService、LiteDbUserProfileService
  - 移除或標記已棄用的 JSON-based FavoriteService / BlockedService
  - 定期備份 restaurantpicker.db 檔案

- **替代方案**（保留 JSON 為主）：
  - 若業務邏輯不變，可繼續使用 JSON 儲存
  - LiteDB 適配器可作為未來擴充的基礎
  - 考慮在文件中標記「可選 LiteDB 實作」以減少混淆

### 6.3 效能與擴展

- LiteDB 的單一檔案設計適合當前 < 10,000 條記錄的規模
- 若未來記錄數爆炸性成長，考慮遷移至 SQLite 或 SQL Server
- 現有介面設計已支援此類遷移（只需新增 ISqliteRestaurantRepository 等實作）

### 6.4 運維注意

- **備份策略**：
  - 定期備份 Data/ 目錄（JSON 和 LiteDB 檔案）
  - 在 UI 中提供「備份與還原」功能（已預留 Tools/backup-and-scan.ps1）
  - 記錄重要的資料異動事件（可搭配 Debug.WriteLine）

- **資料檢查**：
  - 使用 Tools/DBHealthCheck.cs 定期驗證 LiteDB collections 與索引狀態
  - 監測是否有孤立記錄（已刪除使用者的收藏項）

## 7. 結論（目前狀態總結）

### 7.1 現況快照

- **餐廳資料**：✅ 完全使用 LiteDB（LiteDbRestaurantRepository），初始資料由 CSV 匯入
- **使用者資料**：🟡 JSON 為主，LiteDB 適配器已實作，可按需切換
- **收藏/封鎖**：🟡 JSON 為主，但 UI 層已改用 IFavoriteService/IBlockedService 介面
  - LiteDbFavoriteService、LiteDbBlockedService 適配器已可用
  - 新增 ClearByUserId() 方法支援 Reset Data 功能
- **用餐紀錄**：🟡 JSON 內嵌於 UserPreferenceService，可遷移至 LiteDbMealRecordRepository
- **編譯狀態**：✅ 成功（0 個錯誤，59 個 nullable 警告）

### 7.2 主要改進（vs. 前期版本）

1. **介面驅動架構**：
   - UI 與服務層已完全解耦（透過 IFavoriteService、IBlockedService 等介面）
   - 支援快速切換不同的資料層實作（JSON ↔ LiteDB）

2. **適配器模式應用**：
   - LiteDb*Service 適配器可與現有 UI 相容
   - 既保護舊投資（JSON 資料），又開啟新路徑（LiteDB）

3. **Reset Data 功能強化**：
   - 單次重置可清除偏好檔、喜愛清單、封鎖清單
   - 同時支援 JSON 與 LiteDB 後端

4. **文件同步**：
   - 本文件詳述了各層設計與遷移計畫
   - 開發者可按需參考進行擴充或遷移

### 7.3 後續規劃

- **短期（1-2 sprint）**：在測試環境完成 JSON → LiteDB 全量遷移驗證
- **中期（3-6 months）**：在生產環境逐步切換，提供用戶備份工具
- **長期（6+ months）**：視資料規模和效能需求，評估進一步最佳化或遷移至其他儲存方案

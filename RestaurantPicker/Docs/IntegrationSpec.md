# 隨機餐廳選擇系統 串接規格

## 文件導覽
- 專案介紹：`Docs/ProjectIntroduction.md`
- 技術架構：`Docs/ProgramArchitecture.md`
- 介面與流程：`Docs/Interface.md`
- 資料層與契約：`Docs/Database.md`
- 操作手冊：`Docs/UserManual.md`

## 0. 資料層概觀

### 當前資料儲存方式
- **餐廳資料**（restaurants）：LiteDB（restaurantpicker.db）
  - 初始資料由 `Data/restaurants.csv` 匯入
  - 透過 LiteDbRestaurantRepository 操作

- **使用者相關資料**（users, favorites, blocked）：JSON 檔案
  - 已預留 LiteDB 實作（LiteDbFavoriteRepository 等）
  - 可按需切換至 LiteDB 後端（詳見 Database.md）

- **用餐紀錄**（meal_records）：JSON 內嵌於 user_preferences.json
  - 可遷移至 LitedbMealRecordRepository

### LiteDB 檔案位置
- `Data/restaurantpicker.db`：單一檔案型態，包含全部 collections（restaurants 目前已啟用）

## 1. 資料檔位置
- 餐廳資料：`Data/restaurants.csv`
- 使用者偏好：`Data/user_preferences.json`
- 圖片素材資料夾：`Assets/images/`
- 圖示素材資料夾：`Assets/icons/`

> 程式執行時以 `AppDomain.CurrentDomain.BaseDirectory` 為根目錄讀取 `Data/*`。

---

## 2. CSV 欄位契約（資料組）
檔名：`restaurants.csv`

### 2.1 標題列（順序固定）
`Id,Name,PriceRange,FoodTypes,CuisineStyle,Purposes,Feature,Phone,BusinessHours,Address,IsBreakfastAvailable,IsLunchAvailable,IsDinnerAvailable,ImageFileName`

### 2.2 欄位格式
- `Id`：整數，唯一
- `Name`：餐廳名稱
- `PriceRange`：例如 `NT$50-100`
- `FoodTypes`：多值，使用 `;` 分隔（例：`豆漿;蛋餅;吐司`）
- `CuisineStyle`：例如 `台式`、`日式`
- `Purposes`：多值，使用 `;` 分隔（例：`快速解決;聚會`）
- `Feature`：特色描述
- `Phone`：電話
- `BusinessHours`：營業時間（字串）
- `Address`：地址
- `IsBreakfastAvailable` / `IsLunchAvailable` / `IsDinnerAvailable`：`true/false`
- `ImageFileName`：圖片檔名（例：`lunch1.jpg`）

### 2.3 注意事項
- 目前 CSV 解析採用逗號分隔，不建議在欄位文字中直接使用逗號。
- `FoodTypes`、`Purposes` 請統一使用分號分隔。

---

## 3. 偏好資料契約（主程式 / 資料組）
檔名：`user_preferences.json`

```json
{
  "FavoriteRestaurantIds": [1, 5],
  "BlockedRestaurantIds": [7]
}
```

- 收藏按鈕：更新 `FavoriteRestaurantIds`
- 不想再看：更新 `BlockedRestaurantIds`
- 篩選流程會自動排除被封鎖餐廳

---

## 4. 介面串接規格

### 4.1 目前流程
`MainForm -> MealSelectForm -> CategorySelectForm -> SwipeForm -> ResultForm`

### 4.2 可替換素材（不需改商業邏輯）
- `MainForm`：首頁背景、開始按鈕、管理按鈕
- `SwipeForm`：左右卡片背景、選左/選右按鈕圖示
- `ResultForm`：收藏/分享/不想再看按鈕圖示

### 4.3 餐廳卡片必顯示欄位
- 名稱 `Name`
- 電話 `Phone`
- 營業時間 `BusinessHours`
- 種類 `FoodTypes`
- 特色 `Feature`

### 4.4 圖片命名
- `ImageFileName` 對應 `Assets/images/<ImageFileName>`
- UI 組改圖時請保留檔名不變，可避免主程式改碼

---

## 5. 後續擴充方向

### 5.1 資料層遷移（推薦）
- 目前 JSON 版本（FavoriteService、BlockedService）已有 LiteDB 適配器實作
- 遷移步驟：
  1. 驗證 LiteDB 適配器與現有 UI 相容性
  2. 在 Program.cs 切換依賴注入：`new LiteDbFavoriteService()` 替代 `new FavoriteService()`
  3. 執行資料完整性檢查
  4. 保留 JSON 備份用於回滾

- 遷移後優勢：
  - 統一資料存儲格式
  - 支援復雜查詢與索引
  - 便於備份與恢復

### 5.2 介面與功能擴充
- 在 `SwipeForm`/`ResultForm` 加入餐廳圖片顯示（依 `ImageFileName`）
- 新增「重置資料」功能（已實作 ClearByUserId 方法）
- 支援多使用者個人化資料隔離

### 5.3 服務層擴充
- 整合 LiteDbMealRecordService 用於獨立的用餐紀錄管理
- 新增資料匯出功能（餐廳、偏好到 Excel）
- 支援資料備份與還原（Tools/backup-and-scan.ps1）

### 5.4 不需改商業邏輯的調整
- 切換素材（圖片、圖示）無需修改 Service 層
- CSV 欄位新增無需改 UI，只需更新 CsvRestaurantRepository 解析邏輯

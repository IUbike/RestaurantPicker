# 隨機餐廳選擇系統 串接規格（UI 組 / 資料組）

## 文件導覽
- 專案總覽：`README.md`
- 操作手冊：`Docs/UserManual.md`
- 專題介紹：`Docs/ProjectIntroduction.md`

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

## 4. UI 組串接規格

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

## 5. 協作分工建議（Git）
- 主程式組：`Services/*`、`Repositories/*`、流程控制（`Views/*.cs`）
- UI 組：`Views/*.Designer.cs`、`Assets/*`
- 資料組：`Data/restaurants.csv`、分類與標籤

建議避免同時修改同一個 `Designer.cs`，降低衝突。

---

## 6. 後續擴充方向
- 將 `CsvRestaurantRepository` 換成資料庫 Repository（不改 Service 介面）
- 在 `SwipeForm`/`ResultForm` 加入圖片顯示（依 `ImageFileName`）
- 偏好資料可拆分為 `favorites.json` 與 `blocked.json`（若資料組需要）

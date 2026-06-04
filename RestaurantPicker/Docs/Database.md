# 資料庫與資料層

本文件整理專案目前的資料存放方式、資料契約與存取流程，便於了解資料層設計與維護方式。

## 1. 資料來源與存放位置
- LiteDB 資料庫：`Data/restaurantpicker.db`
- 餐廳資料匯入來源：`Data/restaurants.csv`
- 使用者與偏好資料：
  - `Data/users.json`
  - `Data/user_preferences.json`
  - `Data/favorites.json`
  - `Data/blocked.json`
- 圖片素材：`Assets/images/`
- 圖示素材：`Assets/icons/`

執行時以 `AppDomain.CurrentDomain.BaseDirectory` 為根目錄讀取資料。

## 2. LiteDB 使用說明
- 目前餐廳資料以 LiteDB 為主要存放方式。
- 首次啟動時，若資料庫為空且存在 `restaurants.csv`，會自動匯入成為初始資料。

## 3. CSV 餐廳資料契約
檔名：`restaurants.csv`

### 2.1 欄位順序
`Id,Name,PriceRange,FoodTypes,CuisineStyle,Purposes,Feature,Phone,BusinessHours,Address,IsBreakfastAvailable,IsLunchAvailable,IsDinnerAvailable,ImageFileName`

### 2.2 欄位規則
- `Id`：整數，唯一
- `FoodTypes`、`Purposes`：多值用 `;` 分隔
- `IsBreakfastAvailable/IsLunchAvailable/IsDinnerAvailable`：`true/false`
- `ImageFileName`：只填檔名（例：`restaurant_1.jpg`）

### 2.3 注意事項
- 目前 CSV 解析採用逗號分隔，欄位文字請避免包含逗號。

## 4. JSON 偏好資料結構
### 4.1 偏好資料
`user_preferences.json`

```json
{
  "FavoriteRestaurantIds": [1, 5],
  "BlockedRestaurantIds": [7]
}
```

### 4.2 使用者資料
`users.json`

```json
{
  "Users": [
	{
	  "Id": "user-1",
	  "Nickname": "Tina",
	  "AvatarPath": "Assets/images/restaurant_1.jpg"
	}
  ]
}
```

## 5. Repository 設計
- `IRestaurantRepository`：資料存取抽象介面
- `LiteDbRestaurantRepository`：目前主要使用的 LiteDB 實作
- `CsvRestaurantRepository`：CSV 讀寫實作（初始資料來源）

## 6. 典型資料流程
1. `Services` 呼叫 `Repositories` 取得餐廳清單
2. 篩選條件與排序在 `Services` 層完成
3. `Views` 顯示結果並更新偏好資料

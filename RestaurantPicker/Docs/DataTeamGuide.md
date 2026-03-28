# 資料組工作手冊（Data Team Guide）

本文件給資料組使用，目的：讓資料更新不會破壞主程式流程。

## 1. 你們主要要做的事

- 維護 `Data/restaurants.csv` 內容
- 維護分類標籤一致性（`FoodTypes`、`Purposes`）
- 確認 `ImageFileName` 與 UI 組素材檔名一致

## 2. 可以改的檔案

- `RestaurantPicker/Data/restaurants.csv`

## 3. 不要改的檔案

- `RestaurantPicker/Services/*`
- `RestaurantPicker/Repositories/*`
- `RestaurantPicker/Views/*.cs`
- `RestaurantPicker/Views/*.Designer.cs`

## 4. CSV 欄位格式（順序固定）

`Id,Name,PriceRange,FoodTypes,CuisineStyle,Purposes,Feature,Phone,BusinessHours,Address,IsBreakfastAvailable,IsLunchAvailable,IsDinnerAvailable,ImageFileName`

## 5. 欄位規則

- `Id`：整數且唯一，不可重複
- `FoodTypes`：多標籤用分號 `;` 分隔（例：`拉麵;日式`）
- `Purposes`：多標籤用分號 `;` 分隔
- `IsBreakfastAvailable/IsLunchAvailable/IsDinnerAvailable`：只能是 `true` 或 `false`
- `ImageFileName`：只填檔名（例：`lunch1.jpg`）

## 6. 每個欄位的中文介紹（填寫參考）

| 欄位名稱 | 中文說明 | 填寫範例 | 備註 |
|---|---|---|---|
| `Id` | 餐廳唯一編號 | `11` | 不可重複，建議遞增 |
| `Name` | 餐廳名稱 | `阿哲牛肉麵` | 必填 |
| `PriceRange` | 價格區間 | `NT$90-150` | 可用區間格式統一 |
| `FoodTypes` | 食物種類（可多個） | `牛肉麵;麵食` | 多個值用 `;` 分隔 |
| `CuisineStyle` | 料理風格 | `台式` | 例：台式、日式、西式 |
| `Purposes` | 用餐用途（可多個） | `快速解決;聚餐` | 多個值用 `;` 分隔 |
| `Feature` | 特色描述 | `湯頭濃郁;免費加麵` | 可填招牌或特色 |
| `Phone` | 聯絡電話 | `05-2720123` | 建議固定格式 |
| `BusinessHours` | 營業時間 | `11:00-21:00` | 目前為文字欄位 |
| `Address` | 地址 | `中正大學附近` | 可填完整地址 |
| `IsBreakfastAvailable` | 是否供應早餐 | `true` | 只能 `true/false` |
| `IsLunchAvailable` | 是否供應午餐 | `true` | 只能 `true/false` |
| `IsDinnerAvailable` | 是否供應晚餐 | `false` | 只能 `true/false` |
| `ImageFileName` | 餐廳圖片檔名 | `beefnoodle1.jpg` | 只填檔名，不含路徑 |

## 7. 請避免

- 欄位文字中使用逗號 `,`（目前解析器會用逗號切欄位）
- 任意更改標題列名稱或欄位順序

## 8. 驗收方式

1. 存檔後執行專案
2. 走流程到 `SwipeForm`
3. 確認新餐廳可被篩選到、資料顯示正常

## 9. 建議 Issue 標籤

- `data`
- `content-update`
- `csv-maintenance`

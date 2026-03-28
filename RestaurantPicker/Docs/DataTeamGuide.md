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

## 6. 請避免

- 欄位文字中使用逗號 `,`（目前解析器會用逗號切欄位）
- 任意更改標題列名稱或欄位順序

## 7. 驗收方式

1. 存檔後執行專案
2. 走流程到 `SwipeForm`
3. 確認新餐廳可被篩選到、資料顯示正常

## 8. 建議 Issue 標籤

- `data`
- `content-update`
- `csv-maintenance`

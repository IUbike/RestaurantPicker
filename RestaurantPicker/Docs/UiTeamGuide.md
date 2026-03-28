# 美術 / UI 組工作手冊（UI Team Guide）

本文件給 UI 組使用，目的：在不動商業邏輯的前提下完成介面與素材整合。

## 1. 你們主要要做的事

- 提供按鈕、背景、圖示、餐廳圖片素材
- 調整 WinForms 視覺樣式（色彩、字體、排版）
- 維持控制項名稱不變，避免事件斷線

## 2. 可以改的檔案

- `RestaurantPicker/Views/*.Designer.cs`（視覺布局）
- `RestaurantPicker/Assets/images/*`
- `RestaurantPicker/Assets/icons/*`

## 3. 不要改的檔案

- `RestaurantPicker/Services/*`
- `RestaurantPicker/Repositories/*`
- `RestaurantPicker/Models/*`
- `RestaurantPicker/Views/*.cs`（邏輯事件）

## 4. 素材規格

- 餐廳圖片資料夾：`RestaurantPicker/Assets/images/`
- 圖示資料夾：`RestaurantPicker/Assets/icons/`
- 圖片檔名需對應 `restaurants.csv` 的 `ImageFileName`

## 5. 目前關鍵頁面

- `MainForm`：首頁入口
- `MealSelectForm`：時間區間選擇
- `CategorySelectForm`：分類標籤複選
- `SwipeForm`：左右二選一卡片
- `ResultForm`：結果頁
- `ManagePreferenceForm`：收藏/封鎖管理

## 6. UI 調整注意

- 不要改動控制項 `Name`（例如 `btnNext`、`lblTitle`）
- 不要刪除已綁定事件的控制項
- 若需新增控制項，請在 PR 或 Issue 說明用途與命名

## 7. 驗收方式

1. Build 成功
2. 每個頁面可正常開啟
3. 按鈕點擊流程不中斷
4. 無圖片時可顯示 placeholder（暫無圖片）

## 8. 建議 Issue 標籤

- `ui-art`
- `designer`
- `asset`

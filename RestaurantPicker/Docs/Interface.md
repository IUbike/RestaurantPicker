# 介面與功能流程

本文件整理使用者介面與主要流程，便於快速理解使用體驗與功能分佈。

## 1. 主要流程
`MainForm -> MealSelectForm -> CategorySelectForm -> SwipeForm -> ResultForm`

## 2. 主要畫面與職責
- `MainForm`：首頁入口、功能導向
- `MealSelectForm`：選擇吃飯時間區間與快速預設
- `CategorySelectForm`：選擇或隨機餐廳類型標籤
- `SwipeForm`：左右二選一過濾餐廳
- `ResultForm`：顯示推薦結果與收藏/封鎖/分享
- `ManageRestaurantForm`：新增與維護餐廳資料
- `ManagePreferenceForm`：管理收藏與封鎖清單
- `UserProfileForm` / `UserSelectForm`：使用者切換與管理
- `RatingForm`：餐廳評分

## 3. 關鍵互動功能
- 時段篩選與 30 分鐘單位調整
- 標籤複選與隨機種類模式
- 二選一淘汰式推薦流程
- 收藏/封鎖清單管理與互斥處理
- 餐廳評分與評分顯示
- 使用者個人化資料與偏好紀錄

## 4. 圖示與視覺資產
- 圖示位置：`Assets/icons/`
- 圖片位置：`Assets/images/`
- UI 變更建議以更新素材為主，維持控制項名稱與事件邏輯。

## 5. 結果頁資訊顯示
- 餐廳名稱、電話、營業時間、地址
- 食物種類、特色、價位
- 收藏/封鎖/分享功能按鈕

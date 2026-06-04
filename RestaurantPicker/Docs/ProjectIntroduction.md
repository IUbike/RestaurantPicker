# 專題介紹：隨機餐廳選擇系統

## 一、背景與動機

大學生在日常生活中常面臨「不知道要吃什麼」的選擇困難。本專題透過篩選與互動式淘汰流程，協助使用者快速找到可接受且符合當下需求的餐廳。

## 二、專題目標

- 以「時間區間」作為主要篩選條件
- 以「食物分類標籤」作為第二層篩選
- 透過二選一互動降低選擇負擔
- 提供收藏與封鎖機制，建立個人化體驗
- 以模組化結構支援團隊分工與後續擴充

## 三、功能亮點

1. 時間區間（30 分鐘單位）篩選
2. 分類標籤複選
3. 隨機種類模式
4. 左右二選一推薦流程
5. 收藏 / 封鎖清單管理
6. 餐廳評分與今日餐廳紀錄
7. 使用者個人化資料與偏好
8. CSV / JSON / LiteDB 可替換資料層

## 四、技術架構

- 語言：C#
- 平台：WinForms
- 目標框架：.NET 10
- 架構分層：`Models / Services / Repositories / Views`
- 介面流程：`MainForm -> MealSelectForm -> CategorySelectForm -> SwipeForm -> ResultForm`

## 五、資料與擴充性

- 餐廳資料：`Data/restaurants.csv`
- 偏好資料：`Data/user_preferences.json`
- Repository 層可切換 CSV / LiteDB 實作，降低對 UI 與邏輯層的影響。

## 六、文件導覽

- 技術架構：`Docs/ProgramArchitecture.md`
- 介面與流程：`Docs/Interface.md`
- 資料層與契約：`Docs/Database.md`
- 操作手冊：`Docs/UserManual.md`

# RestaurantPicker

`RestaurantPicker` 是一個以 C# WinForms 開發的「隨機餐廳選擇系統」，用來解決「不知道要吃什麼」的日常問題。

## 專案介紹

本系統提供一個可逐步篩選、再透過二選一（左/右）淘汰的流程，最後推薦一間餐廳。

目前核心流程：

1. 首頁進入選餐流程
2. 選擇預計用餐時間（30 分鐘為單位，可設定最小/最大）
3. 選擇分類標籤（支援複選）或隨機種類
4. 進入二選一餐廳卡片頁面，直到產生最終推薦
5. 可收藏或封鎖餐廳

## 主要功能

- 以時間區間為第一層篩選
- 分類標籤複選（加入標籤後從清單隱藏，可移除）
- 隨機種類模式（等同該時間區間全部種類）
- 二選一淘汰機制
- 收藏 / 封鎖管理
- 新增餐廳資料（CSV）
- 缺圖時自動顯示 placeholder（不影響流程）

## 專案結構

- `RestaurantPicker/Models`：資料模型
- `RestaurantPicker/Services`：篩選、隨機、偏好等商業邏輯
- `RestaurantPicker/Repositories`：CSV 資料存取
- `RestaurantPicker/Views`：WinForms UI
- `RestaurantPicker/Data`：CSV / JSON 資料
- `RestaurantPicker/Assets`：圖片與圖示
- `RestaurantPicker/Docs`：規格與操作文件

## 執行環境

- .NET: `net10.0-windows`
- IDE: Visual Studio 2026 (建議)

## 如何執行

1. 以 Visual Studio 開啟方案
2. Build 專案
3. 執行 `RestaurantPicker`

> `Data` 目錄已設定為建置時複製到輸出資料夾。

## 文件

- 串接規格：`RestaurantPicker/Docs/IntegrationSpec.md`
- 操作手冊：`RestaurantPicker/Docs/UserManual.md`
- 專案介紹：`RestaurantPicker/Docs/ProjectIntroduction.md`

---

如需部署或協作流程，請見下方 GitHub 上傳步驟。
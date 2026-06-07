# 文件導覽

本目錄整理 RestaurantPicker 的技術與功能文件，便於快速掌握專案進度與設計。

## 文件速查表

| 文件 | 適用對象 | 主要內容 |
|------|--------|--------|
| ProjectIntroduction.md | 所有人 | 專案背景、功能與目標 |
| ProgramArchitecture.md | 開發人員 | 技術棧、分層設計、介面驅動設計、遷移路徑 |
| Database.md | 資料庫 / 後端開發 | 資料層詳細說明、LiteDB 現況、遷移計畫 |
| Interface.md | 前端 / UI 開發 | 介面流程、畫面職責、按鈕與功能 |
| UserManual.md | 使用者 | 操作步驟、常見問題、Reset Data 說明 |
| IntegrationSpec.md | 資料組 / 串接人員 | CSV/JSON 格式、欄位契約、擴充方向 |
| GitWorkflowGuide.md | 開發人員 | Git 工作流程與協作規範 |

## 1. 新手入門（5 分鐘快速了解）

1. 閱讀 `ProjectIntroduction.md`
2. 瀏覽 `Interface.md` - 認識主要介面
3. 查看 `UserManual.md` - 操作示例

## 2. 開發人員（系統架構理解）

1. `ProgramArchitecture.md` - 整體架構與分層
   - 介面驅動設計
   - 適配器模式
   - Reset Data 新增功能

2. `Database.md` - 資料層深度解析
   - LiteDB 與 JSON 實作對照
   - 遷移進展與計畫
   - 服務介面化設計

3. `IntegrationSpec.md` - 資料契約與規格

## 3. 資料組 / 維護人員（資料與格式）

1. `IntegrationSpec.md` - 資料檔格式與欄位定義
2. `UserManual.md` - Reset Data 等操作功能
3. `Database.md` - 若需了解資料遷移細節

## 4. 最新更新亮點

### 📌 介面層設計（Interface-Driven）
- UI 已改用 IFavoriteService、IBlockedService、IUserProfileService 等介面
- 支援無縫切換 JSON ↔ LiteDB 實作

### 📌 Reset Data 功能
- 新增一鍵重置：同時清除偏好、收藏、封鎖項目
- 詳見 `UserManual.md` 第 7 章

### 📌 LiteDB 遷移準備
- 適配器層（LiteDbFavoriteService 等）已就緒
- 遷移路徑已規劃，詳見 `Database.md` 與 `ProgramArchitecture.md`

### 📌 編譯狀態
- ✅ 零編譯錯誤
- 49 個 nullable 警告（.NET 10 標準，不影響執行）

## 5. 常見問題速查

- **想看程式碼結構？** → ProgramArchitecture.md
- **想新增餐廳資料？** → IntegrationSpec.md（CSV 格式）
- **想重置使用者資料？** → UserManual.md（第 7 章）
- **想遷移至 LiteDB？** → Database.md（第 3.2 - 3.3 節）
- **怎麼新增功能？** → ProgramArchitecture.md（擴充性設計）

---

**更新時間**：2025 年 1 月 | **版本**：1.1

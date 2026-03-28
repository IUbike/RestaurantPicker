# Git 操作與 Visual Studio 上傳流程

本文件提供給團隊成員，用於日常開發的 Git 基本操作、Visual Studio 建立分支、提交與推送。

## 1. 開發原則

- 不要直接在 `master`（或 `main`）上改功能
- 一個功能一個分支
- 提交訊息要清楚
- Push 前先 Build、先自我測試

---

## 2. 建議分支命名

- `feature/xxx`：新功能
- `fix/xxx`：修正 bug
- `docs/xxx`：文件更新
- `refactor/xxx`：重構

範例：
- `feature/manage-preference-form`
- `fix/swipe-random-category`
- `docs/update-user-manual`

---

## 3. Visual Studio 建立分支（GUI）

1. 開啟專案後，點上方 `Git` 選單
2. 選 `Manage Branches`（管理分支）
3. 在目前分支（如 `master`）按右鍵 `New Local Branch From...`
4. 輸入分支名稱（例：`feature/ui-assets`）
5. 勾選 `Checkout branch`（建立後切換）
6. 按 `Create Branch`

---

## 4. 在 Visual Studio 提交（Commit）

1. 修改檔案
2. 開啟 `Git Changes` 視窗
3. 在 `Included Changes` 確認本次要提交的檔案
4. 填寫 Commit 訊息
5. 按 `Commit`（或 `Commit All`）

提交訊息建議：
- `feat: 新增收藏/封鎖管理頁`
- `fix: 修正隨機種類候選數計算`
- `docs: 新增資料組與 UI 組手冊`

---

## 5. 推送到 GitHub（Push）

### Visual Studio
1. 在 `Git Changes` 視窗按 `Push`
2. 第一次推送新分支時，選擇 `Publish Branch`

### PowerShell（替代）
```powershell
git add .
git commit -m "feat: your message"
git push -u origin <branch-name>
```

---

## 6. 建立 Pull Request（PR）

1. Push 後到 GitHub 專案頁
2. 點 `Compare & pull request`
3. Base 選 `master`（或團隊主要分支）
4. 填寫 PR 標題與說明
5. 指派 reviewer

PR 說明建議包含：
- 做了什麼
- 測試怎麼做
- 影響哪些檔案

---

## 7. 同步最新主分支（避免衝突）

### Visual Studio
- 在分支管理先切到 `master`
- `Pull`
- 再切回你的功能分支
- 進行 `Merge from master`

### PowerShell
```powershell
git checkout master
git pull origin master
git checkout <branch-name>
git merge master
```

---

## 8. 發生衝突怎麼辦

1. 先不要慌，檔案不會消失
2. 在 Visual Studio 開啟衝突檔
3. 使用 `Accept Current / Accept Incoming / Both`
4. 手動確認程式碼後存檔
5. 再 Commit 一次衝突解決結果

---

## 9. 上傳前檢查清單

- [ ] Build 成功
- [ ] 功能可正常操作
- [ ] 不該改的檔案沒有被加入
- [ ] Commit 訊息清楚
- [ ] 已 Push 到正確分支

---

## 10. 本專案協作提醒

- 資料組優先改：`Data/restaurants.csv`
- UI 組優先改：`Views/*.Designer.cs`、`Assets/*`
- 主程式組優先改：`Services/*`、`Repositories/*`、`Views/*.cs`

參考文件：
- `Docs/DataTeamGuide.md`
- `Docs/UiTeamGuide.md`
- `.github/ISSUE_TEMPLATE/task.md`

# 程式架構與設計

本文件整理專案技術選型、模組分層與核心設計概念。

## 1. 技術選型
- 語言：C#
- 平台：WinForms
- 目標框架：.NET 10

## 2. 架構分層
- `Models`：資料模型（Restaurant、UserProfile、MealRecord 等）
- `Repositories`：資料存取抽象與實作
- `Services`：商業邏輯、篩選、偏好管理
- `Views`：WinForms 介面與使用流程

## 3. 核心服務說明
- `RandomPickService`：隨機候選餐廳產生
- `RestaurantFilterService`：依時間與標籤篩選
- `SwipeMatchService`：二選一推薦流程
- `FavoriteService` / `BlockedService`：偏好清單管理
- `TodayMealService`：今日餐廳紀錄
- `UserPreferenceService` / `UserProfileService`：使用者偏好與資料管理

## 4. 資料存取與儲存
- 餐廳資料目前以 LiteDB 為主要存放
- 初始資料由 `restaurants.csv` 匯入
- 透過 `IRestaurantRepository` 抽象保持可替換性

## 5. 擴充性設計
- UI 與服務層分離，避免畫面改動影響邏輯
- 資料格式集中管理，便於資料更新或替換

## 6. 程式設計特色
- 以 Form 導向的流程控制
- 清楚的分層結構與責任分離
- 可逐步擴充的資料儲存介面

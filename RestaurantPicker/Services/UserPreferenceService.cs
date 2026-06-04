using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using LiteDB;
using RestaurantPicker.Models;

namespace RestaurantPicker.Services
{
    /// <summary>
    /// 使用者偏好服務
    /// 負責讀寫 LiteDB，管理收藏、封鎖清單和用餐紀錄
    /// </summary>
    public class UserPreferenceService
    {
        private readonly string _databasePath;
        private readonly string? _legacyPreferencePath;
        private UserPreference _userPreference;

        public UserPreferenceService(string databasePath, string? legacyPreferencePath = null)
        {
            _databasePath = databasePath;
            _legacyPreferencePath = legacyPreferencePath;
            _userPreference = new UserPreference();
        }

        /// <summary>
        /// 載入使用者偏好
        /// 若資料不存在，建立新的空偏好
        /// </summary>
        public UserPreference LoadPreferences()
        {
            try
            {
                using var db = new LiteDatabase(_databasePath);
                var collection = db.GetCollection<UserPreference>("user_preferences");
                collection.EnsureIndex(p => p.Id, true);

                var preference = collection.FindById(1);
                if (preference == null)
                {
                    preference = LoadLegacyPreferences() ?? new UserPreference();
                    preference.Id = 1;
                    collection.Upsert(preference);
                }

                _userPreference = preference;
                return _userPreference;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"讀取使用者偏好失敗: {ex.Message}");
                _userPreference = new UserPreference();
                return _userPreference;
            }
        }

        private UserPreference? LoadLegacyPreferences()
        {
            if (string.IsNullOrWhiteSpace(_legacyPreferencePath) || !File.Exists(_legacyPreferencePath))
            {
                return null;
            }

            try
            {
                string json = File.ReadAllText(_legacyPreferencePath);
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                var preference = JsonSerializer.Deserialize<UserPreference>(json, options);
                if (preference == null)
                {
                    return null;
                }

                preference.Id = 1;
                return preference;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"讀取舊版偏好失敗: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 儲存使用者偏好到 LiteDB
        /// </summary>
        public void SavePreferences(UserPreference userPreference)
        {
            try
            {
                _userPreference = userPreference ?? new UserPreference();
                _userPreference.Id = 1;

                using var db = new LiteDatabase(_databasePath);
                var collection = db.GetCollection<UserPreference>("user_preferences");
                collection.EnsureIndex(p => p.Id, true);
                collection.Upsert(_userPreference);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"儲存使用者偏好失敗: {ex.Message}");
            }
        }

        /// <summary>
        /// 取得目前的使用者偏好
        /// </summary>
        public UserPreference GetCurrentPreference()
        {
            return _userPreference;
        }

        /// <summary>
        /// 新增收藏
        /// </summary>
        public void AddFavorite(int restaurantId)
        {
            _userPreference.AddFavorite(restaurantId);
            SavePreferences(_userPreference);
        }

        /// <summary>
        /// 移除收藏
        /// </summary>
        public void RemoveFavorite(int restaurantId)
        {
            _userPreference.RemoveFavorite(restaurantId);
            SavePreferences(_userPreference);
        }

        /// <summary>
        /// 檢查餐廳是否已收藏
        /// </summary>
        public bool IsFavorite(int restaurantId)
        {
            return _userPreference.IsFavorite(restaurantId);
        }

        /// <summary>
        /// 新增封鎖
        /// </summary>
        public void AddBlocked(int restaurantId)
        {
            _userPreference.AddBlocked(restaurantId);
            SavePreferences(_userPreference);
        }

        /// <summary>
        /// 移除封鎖
        /// </summary>
        public void RemoveBlocked(int restaurantId)
        {
            _userPreference.RemoveBlocked(restaurantId);
            SavePreferences(_userPreference);
        }

        /// <summary>
        /// 檢查餐廳是否已被封鎖
        /// </summary>
        public bool IsBlocked(int restaurantId)
        {
            return _userPreference.IsBlocked(restaurantId);
        }

        /// <summary>
        /// 新增用餐紀錄
        /// </summary>
        public void AddMealRecord(MealRecord record)
        {
            if (record != null)
            {
                if (record.Id == 0)
                {
                    record.Id = (_userPreference.MealHistory.Count > 0
                        ? _userPreference.MealHistory.Max(m => m.Id)
                        : 0) + 1;
                }
                _userPreference.MealHistory.Add(record);
                SavePreferences(_userPreference);
                System.Diagnostics.Debug.WriteLine($"[DEBUG] 新增用餐紀錄 Id={record.Id} RestaurantId={record.RestaurantId} MealTime={record.MealTime} CreatedAt={record.CreatedAt}");
            }
        }

        /// <summary>
        /// 更新用餐紀錄的評分
        /// </summary>
        public void UpdateMealRating(int recordId, int rating)
        {
            var record = _userPreference.MealHistory.FirstOrDefault(m => m.Id == recordId);
            if (record != null)
            {
                record.Rating = rating;
                record.HasRating = true;
                SavePreferences(_userPreference);
                System.Diagnostics.Debug.WriteLine($"[DEBUG] 更新用餐紀錄評分 Id={recordId} Rating={rating}");
            }
        }

        /// <summary>
        /// 取得今日的所有用餐紀錄
        /// </summary>
        public List<MealRecord> GetMealRecordsForToday(string? userId = null)
        {
            var today = DateTime.Now.Date;
            var query = _userPreference.MealHistory
                .Where(m => m.MealDate.Date == today);

            if (!string.IsNullOrWhiteSpace(userId))
            {
                query = query.Where(m => string.Equals(m.UserId, userId, StringComparison.OrdinalIgnoreCase));
            }

            return query
                .ToList();
        }

        /// <summary>
        /// 取得特定餐廳的所有用餐紀錄
        /// </summary>
        public List<MealRecord> GetMealRecordsByRestaurant(int restaurantId)
        {
            return _userPreference.MealHistory
                .Where(m => m.RestaurantId == restaurantId)
                .ToList();
        }

        /// <summary>
        /// 取得特定餐廳的統計信息
        /// 返回 (造訪次數, 平均評分)
        /// </summary>
        public (int visitCount, double averageRating) GetRestaurantStats(int restaurantId)
        {
            var records = GetMealRecordsByRestaurant(restaurantId);
            if (records.Count == 0)
                return (0, 0.0);

            var ratedRecords = records.Where(m => m.HasRating).ToList();
            if (ratedRecords.Count == 0)
                return (records.Count, 0.0);

            double avgRating = ratedRecords.Average(m => m.Rating);
            return (records.Count, avgRating);
        }

        /// <summary>
        /// 取得特定時段今天的用餐紀錄
        /// </summary>
        public MealRecord? GetMealRecordForTimeSlot(string mealTime, string? userId = null)
        {
            var todayRecords = GetMealRecordsForToday(userId);
            return todayRecords.FirstOrDefault(m => m.MealTime == mealTime);
        }

        /// <summary>
        /// 刪除用餐紀錄
        /// </summary>
        public void DeleteMealRecord(int recordId)
        {
            var record = _userPreference.MealHistory.FirstOrDefault(m => m.Id == recordId);
            if (record != null)
            {
                _userPreference.MealHistory.Remove(record);
                SavePreferences(_userPreference);
                System.Diagnostics.Debug.WriteLine($"[DEBUG] 刪除用餐紀錄 Id={recordId}");
            }
        }

        /// <summary>
        /// 重置所有使用者偏好並寫入空檔
        /// </summary>
        public void ResetPreferences()
        {
            _userPreference = new UserPreference();
            SavePreferences(_userPreference);
            System.Diagnostics.Debug.WriteLine("[DEBUG] 已重置使用者偏好（已清除所有紀錄）");
        }
    }
}

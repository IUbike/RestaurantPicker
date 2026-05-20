using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using RestaurantPicker.Models;

namespace RestaurantPicker.Services
{
    /// <summary>
    /// 使用者偏好服務
    /// 負責讀寫 user_preferences.json，管理收藏、封鎖清單和用餐紀錄
    /// </summary>
    public class UserPreferenceService
    {
        private readonly string _preferenceFilePath;
        private UserPreference _userPreference;

        public UserPreferenceService(string preferenceFilePath)
        {
            _preferenceFilePath = preferenceFilePath;
            _userPreference = new UserPreference();
        }

        /// <summary>
        /// 載入使用者偏好
        /// 若檔案不存在，建立新的空偏好
        /// </summary>
        public UserPreference LoadPreferences()
        {
            try
            {
                if (!File.Exists(_preferenceFilePath))
                {
                    // 檔案不存在，建立新的空偏好
                    _userPreference = new UserPreference();
                    SavePreferences(_userPreference);
                    return _userPreference;
                }

                // 讀取 JSON 檔案
                string json = File.ReadAllText(_preferenceFilePath);
                
                // 反序列化
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                _userPreference = JsonSerializer.Deserialize<UserPreference>(json, options);

                if (_userPreference == null)
                {
                    _userPreference = new UserPreference();
                }

                return _userPreference;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"讀取使用者偏好失敗: {ex.Message}");
                // 發生錯誤時，返回空偏好
                _userPreference = new UserPreference();
                return _userPreference;
            }
        }

        /// <summary>
        /// 儲存使用者偏好到 JSON 檔案
        /// </summary>
        public void SavePreferences(UserPreference userPreference)
        {
            try
            {
                _userPreference = userPreference;

                // 序列化
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(_userPreference, options);

                // 確保目錄存在
                string directory = Path.GetDirectoryName(_preferenceFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 寫入檔案
                File.WriteAllText(_preferenceFilePath, json);
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
                    // 自增 ID
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
        public List<MealRecord> GetMealRecordsForToday()
        {
            var today = DateTime.Now.Date;
            return _userPreference.MealHistory
                .Where(m => m.MealDate.Date == today)
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
        public MealRecord? GetMealRecordForTimeSlot(string mealTime)
        {
            var todayRecords = GetMealRecordsForToday();
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

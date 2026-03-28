using System;
using System.IO;
using System.Text.Json;
using RestaurantPicker.Models;

namespace RestaurantPicker.Services
{
    /// <summary>
    /// 使用者偏好服務
    /// 負責讀寫 user_preferences.json，管理收藏和封鎖清單
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
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using RestaurantPicker.Models;
using RestaurantPicker.Repositories;
using RestaurantPicker.Services.Interfaces;

namespace RestaurantPicker.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly string _filePath;

        public UserProfileService(string filePath)
        {
            _filePath = filePath;
        }

        public List<UserProfile> LoadAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<UserProfile>();
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                return JsonSerializer.Deserialize<List<UserProfile>>(json, options) ?? new List<UserProfile>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"讀取使用者資料失敗: {ex.Message}");
                return new List<UserProfile>();
            }
        }

        public void SaveAll(List<UserProfile> users)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(users ?? new List<UserProfile>(), options);
                var directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"儲存使用者資料失敗: {ex.Message}");
            }
        }

        public UserProfile AddUser(UserProfile profile)
        {
            var users = LoadAll();
            if (string.IsNullOrWhiteSpace(profile.Id))
            {
                profile.Id = Guid.NewGuid().ToString("N");
            }

            users.Add(profile);
            SaveAll(users);
            return profile;
        }

        public UserProfile? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            return LoadAll().FirstOrDefault(u => string.Equals(u.Id, id, StringComparison.OrdinalIgnoreCase));
        }

        public List<string> GetAvailableTags(IRestaurantRepository restaurantRepository)
        {
            var restaurants = restaurantRepository.LoadAll();
            return restaurants
                .Where(r => r.FoodTypes != null)
                .SelectMany(r => r.FoodTypes)
                .Where(tag => !string.IsNullOrWhiteSpace(tag))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(tag => tag)
                .ToList();
        }
    }
}

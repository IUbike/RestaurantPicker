using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using RestaurantPicker.Models;
using RestaurantPicker.Services.Interfaces;

namespace RestaurantPicker.Services
{
    public class BlockedService : IBlockedService
    {
        private readonly string _filePath;

        public BlockedService(string filePath)
        {
            _filePath = filePath;
        }

        public List<BlockedRestaurant> LoadAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<BlockedRestaurant>();
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                return JsonSerializer.Deserialize<List<BlockedRestaurant>>(json, options) ?? new List<BlockedRestaurant>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"讀取封鎖資料失敗: {ex.Message}");
                return new List<BlockedRestaurant>();
            }
        }

        public void SaveAll(List<BlockedRestaurant> blocks)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(blocks ?? new List<BlockedRestaurant>(), options);
                var directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"儲存封鎖資料失敗: {ex.Message}");
            }
        }

        public List<BlockedRestaurant> GetByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new List<BlockedRestaurant>();
            }

            return LoadAll()
                .Where(b => string.Equals(b.UserId, userId, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(b => b.CreatedAt)
                .ToList();
        }

        public bool IsBlocked(string userId, int restaurantId)
        {
            return GetByUserId(userId).Any(b => b.RestaurantId == restaurantId);
        }

        public void AddBlocked(string userId, Restaurant restaurant)
        {
            if (string.IsNullOrWhiteSpace(userId) || restaurant == null)
            {
                return;
            }

            var blocks = LoadAll();
            if (blocks.Any(b => string.Equals(b.UserId, userId, StringComparison.OrdinalIgnoreCase) && b.RestaurantId == restaurant.Id))
            {
                return;
            }

            blocks.Add(new BlockedRestaurant
            {
                UserId = userId,
                RestaurantId = restaurant.Id,
                RestaurantName = restaurant.Name,
                CreatedAt = DateTime.Now
            });

            SaveAll(blocks);
        }

        public void RemoveBlocked(string userId, int restaurantId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var blocks = LoadAll();
            blocks.RemoveAll(b => string.Equals(b.UserId, userId, StringComparison.OrdinalIgnoreCase) && b.RestaurantId == restaurantId);
            SaveAll(blocks);
        }

        public void ClearByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var blocks = LoadAll();
            blocks.RemoveAll(b => string.Equals(b.UserId, userId, StringComparison.OrdinalIgnoreCase));
            SaveAll(blocks);
        }
    }
}

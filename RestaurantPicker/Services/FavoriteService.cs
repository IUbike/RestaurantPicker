using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using RestaurantPicker.Models;
using RestaurantPicker.Services.Interfaces;

namespace RestaurantPicker.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly string _filePath;

        public FavoriteService(string filePath)
        {
            _filePath = filePath;
        }

        public List<FavoriteRestaurant> LoadAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<FavoriteRestaurant>();
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                return JsonSerializer.Deserialize<List<FavoriteRestaurant>>(json, options) ?? new List<FavoriteRestaurant>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"讀取收藏資料失敗: {ex.Message}");
                return new List<FavoriteRestaurant>();
            }
        }

        public void SaveAll(List<FavoriteRestaurant> favorites)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(favorites ?? new List<FavoriteRestaurant>(), options);
                var directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"儲存收藏資料失敗: {ex.Message}");
            }
        }

        public List<FavoriteRestaurant> GetByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new List<FavoriteRestaurant>();
            }

            return LoadAll()
                .Where(f => string.Equals(f.UserId, userId, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(f => f.CreatedAt)
                .ToList();
        }

        public bool IsFavorite(string userId, int restaurantId)
        {
            return GetByUserId(userId).Any(f => f.RestaurantId == restaurantId);
        }

        public void AddFavorite(string userId, Restaurant restaurant)
        {
            if (string.IsNullOrWhiteSpace(userId) || restaurant == null)
            {
                return;
            }

            var favorites = LoadAll();
            if (favorites.Any(f => string.Equals(f.UserId, userId, StringComparison.OrdinalIgnoreCase) && f.RestaurantId == restaurant.Id))
            {
                return;
            }

            favorites.Add(new FavoriteRestaurant
            {
                UserId = userId,
                RestaurantId = restaurant.Id,
                RestaurantName = restaurant.Name,
                CreatedAt = DateTime.Now
            });

            SaveAll(favorites);
        }

        public void RemoveFavorite(string userId, int restaurantId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var favorites = LoadAll();
            favorites.RemoveAll(f => string.Equals(f.UserId, userId, StringComparison.OrdinalIgnoreCase) && f.RestaurantId == restaurantId);
            SaveAll(favorites);
        }

        public void ClearByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return;
            }

            var favorites = LoadAll();
            favorites.RemoveAll(f => string.Equals(f.UserId, userId, StringComparison.OrdinalIgnoreCase));
            SaveAll(favorites);
        }
    }
}

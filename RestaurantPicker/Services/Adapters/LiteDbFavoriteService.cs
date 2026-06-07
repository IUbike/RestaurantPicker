using System.Collections.Generic;
using RestaurantPicker.Models;
using RestaurantPicker.Repositories;
using RestaurantPicker.Services.Interfaces;

namespace RestaurantPicker.Services.Adapters
{
    public class LiteDbFavoriteService : IFavoriteService
    {
        private readonly LiteDbFavoriteRepository _repo;

        public LiteDbFavoriteService(string databasePath)
        {
            _repo = new LiteDbFavoriteRepository(databasePath);
        }

        public List<FavoriteRestaurant> LoadAll()
        {
            // LiteDbFavoriteRepository 不提供 LoadAll，但可用 Count/Query pattern。
            return _repo.GetByUserId(string.Empty);
        }

        public void SaveAll(List<FavoriteRestaurant> favorites)
        {
            // LiteDB repository 以 Upsert / Remove 為主，這裡採逐筆 Upsert
            if (favorites == null) return;
            foreach (var f in favorites)
            {
                _repo.Add(f);
            }
        }

        public List<FavoriteRestaurant> GetByUserId(string userId)
        {
            return _repo.GetByUserId(userId);
        }

        public bool IsFavorite(string userId, int restaurantId)
        {
            return _repo.Exists(userId, restaurantId);
        }

        public void AddFavorite(string userId, Restaurant restaurant)
        {
            var fav = new FavoriteRestaurant
            {
                UserId = userId,
                RestaurantId = restaurant.Id,
                RestaurantName = restaurant.Name,
                CreatedAt = System.DateTime.Now
            };
            _repo.Add(fav);
        }

        public void RemoveFavorite(string userId, int restaurantId)
        {
            _repo.Remove(userId, restaurantId);
        }

        public void ClearByUserId(string userId)
        {
            _repo.DeleteByUserId(userId);
        }
    }
}

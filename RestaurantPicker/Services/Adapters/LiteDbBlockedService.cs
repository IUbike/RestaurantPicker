using System.Collections.Generic;
using RestaurantPicker.Models;
using RestaurantPicker.Repositories;
using RestaurantPicker.Services.Interfaces;

namespace RestaurantPicker.Services.Adapters
{
    public class LiteDbBlockedService : IBlockedService
    {
        private readonly LiteDbBlockedRepository _repo;

        public LiteDbBlockedService(string databasePath)
        {
            _repo = new LiteDbBlockedRepository(databasePath);
        }

        public List<BlockedRestaurant> LoadAll()
        {
            return _repo.GetByUserId(string.Empty);
        }

        public void SaveAll(List<BlockedRestaurant> blocks)
        {
            if (blocks == null) return;
            foreach (var b in blocks)
            {
                _repo.Add(b);
            }
        }

        public List<BlockedRestaurant> GetByUserId(string userId)
        {
            return _repo.GetByUserId(userId);
        }

        public bool IsBlocked(string userId, int restaurantId)
        {
            return _repo.Exists(userId, restaurantId);
        }

        public void AddBlocked(string userId, Restaurant restaurant)
        {
            var blocked = new BlockedRestaurant
            {
                UserId = userId,
                RestaurantId = restaurant.Id,
                RestaurantName = restaurant.Name,
                CreatedAt = System.DateTime.Now
            };
            _repo.Add(blocked);
        }

        public void RemoveBlocked(string userId, int restaurantId)
        {
            _repo.Remove(userId, restaurantId);
        }

        public void ClearByUserId(string userId)
        {
            _repo.DeleteByUserId(userId);
        }
    }
}

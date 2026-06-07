using System.Collections.Generic;
using RestaurantPicker.Models;

namespace RestaurantPicker.Services.Interfaces
{
    public interface IBlockedService
    {
        List<BlockedRestaurant> LoadAll();
        void SaveAll(List<BlockedRestaurant> blocks);
        List<BlockedRestaurant> GetByUserId(string userId);
        bool IsBlocked(string userId, int restaurantId);
        void AddBlocked(string userId, Restaurant restaurant);
        void RemoveBlocked(string userId, int restaurantId);
        void ClearByUserId(string userId);
    }
}

using System.Collections.Generic;
using RestaurantPicker.Models;

namespace RestaurantPicker.Services.Interfaces
{
    public interface IFavoriteService
    {
        List<FavoriteRestaurant> LoadAll();
        void SaveAll(List<FavoriteRestaurant> favorites);
        List<FavoriteRestaurant> GetByUserId(string userId);
        bool IsFavorite(string userId, int restaurantId);
        void AddFavorite(string userId, Restaurant restaurant);
        void RemoveFavorite(string userId, int restaurantId);
        void ClearByUserId(string userId);
    }
}

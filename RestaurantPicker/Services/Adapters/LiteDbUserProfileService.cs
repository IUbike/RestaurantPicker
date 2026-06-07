using System.Collections.Generic;
using RestaurantPicker.Models;
using RestaurantPicker.Repositories;
using RestaurantPicker.Services.Interfaces;

namespace RestaurantPicker.Services.Adapters
{
    public class LiteDbUserProfileService : IUserProfileService
    {
        private readonly LiteDbUserProfileRepository _repo;

        public LiteDbUserProfileService(string databasePath)
        {
            _repo = new LiteDbUserProfileRepository(databasePath);
        }

        public List<UserProfile> LoadAll()
        {
            return _repo.LoadAll();
        }

        public void SaveAll(List<UserProfile> users)
        {
            if (users == null) return;
            foreach (var u in users)
            {
                _repo.Add(u);
            }
        }

        public UserProfile AddUser(UserProfile profile)
        {
            return _repo.Add(profile);
        }

        public UserProfile? GetById(string id)
        {
            return _repo.GetById(id);
        }

        public List<string> GetAvailableTags(IRestaurantRepository restaurantRepository)
        {
            var restaurants = restaurantRepository.LoadAll();
            return restaurants
                .Where(r => r.FoodTypes != null)
                .SelectMany(r => r.FoodTypes)
                .Where(tag => !string.IsNullOrWhiteSpace(tag))
                .Distinct(System.StringComparer.OrdinalIgnoreCase)
                .OrderBy(tag => tag)
                .ToList();
        }
    }
}

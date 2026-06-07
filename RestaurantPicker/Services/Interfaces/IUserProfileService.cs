using System.Collections.Generic;
using RestaurantPicker.Models;
using RestaurantPicker.Repositories;

namespace RestaurantPicker.Services.Interfaces
{
    public interface IUserProfileService
    {
        List<UserProfile> LoadAll();
        void SaveAll(List<UserProfile> users);
        UserProfile AddUser(UserProfile profile);
        UserProfile? GetById(string id);
        List<string> GetAvailableTags(IRestaurantRepository restaurantRepository);
    }
}

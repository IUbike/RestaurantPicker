using System;
using RestaurantPicker.Repositories;
using RestaurantPicker.Services;
using RestaurantPicker.Views;

namespace RestaurantPicker
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string databasePath = System.IO.Path.Combine(basePath, "Data", "restaurantpicker.db");
            string csvPath = System.IO.Path.Combine(basePath, "Data", "restaurants.csv");
            string usersPath = System.IO.Path.Combine(basePath, "Data", "users.json");
            string favoritesPath = System.IO.Path.Combine(basePath, "Data", "favorites.json");
            string blockedPath = System.IO.Path.Combine(basePath, "Data", "blocked.json");

            var restaurantRepository = new LiteDbRestaurantRepository(databasePath, csvPath);
            var userProfileService = new UserProfileService(usersPath);
            var favoriteService = new FavoriteService(favoritesPath);
            var blockedService = new BlockedService(blockedPath);

            using var userSelectForm = new UserSelectForm(userProfileService, restaurantRepository);
            if (userSelectForm.ShowDialog() != DialogResult.OK || userSelectForm.SelectedUser == null)
            {
                return;
            }

            Application.Run(new MainForm(userSelectForm.SelectedUser, favoriteService, blockedService, userProfileService));
        }
    }
}
using System;
using RestaurantPicker.Repositories;
using RestaurantPicker.Services;
using RestaurantPicker.Services.Interfaces;
using RestaurantPicker.Services.Adapters;
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

            var restaurantRepository = new LiteDbRestaurantRepository(databasePath, csvPath);
            // 使用 LiteDB-backed services（完全轉換到 LiteDB）
            var userProfileService = new LiteDbUserProfileService(databasePath);
            var favoriteService = new LiteDbFavoriteService(databasePath);
            var blockedService = new LiteDbBlockedService(databasePath);

            // 預設執行（所有資料已在 LiteDB 中管理）
            // 遺留的遷移邏輯已移除，所有資料互動都透過 LiteDB 進行

            using var userSelectForm = new UserSelectForm(userProfileService, restaurantRepository);
            if (userSelectForm.ShowDialog() != DialogResult.OK || userSelectForm.SelectedUser == null)
            {
                return;
            }

            Application.Run(new MainForm(userSelectForm.SelectedUser, favoriteService, blockedService, userProfileService));
        }
    }
}
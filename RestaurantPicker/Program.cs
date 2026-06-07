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
            string usersPath = System.IO.Path.Combine(basePath, "Data", "users.json");
            string favoritesPath = System.IO.Path.Combine(basePath, "Data", "favorites.json");
            string blockedPath = System.IO.Path.Combine(basePath, "Data", "blocked.json");

            var restaurantRepository = new LiteDbRestaurantRepository(databasePath, csvPath);
            // 使用 LiteDB-backed services（Adapter）
            var userProfileService = new LiteDbUserProfileService(databasePath);
            var favoriteService = new LiteDbFavoriteService(databasePath);
            var blockedService = new LiteDbBlockedService(databasePath);

            // 預設執行 dry-run 遷移，除非傳入 --migrate
            bool doMigrate = false;
            var args = Environment.GetCommandLineArgs();
            foreach (var a in args)
            {
                if (a.Equals("--migrate", StringComparison.OrdinalIgnoreCase))
                {
                    doMigrate = true;
                    break;
                }
            }

            if (!doMigrate)
            {
                // dry-run: 檢查 JSON 與 LiteDB 的差異，列出報告
                // 注：DataMigrationTool 尚未集成到主專案中，暫時註解
                // try
                // {
                //     var migrator = new Tools.DataMigrationTool(AppDomain.CurrentDomain.BaseDirectory);
                //     var dry = migrator.DryRunMigrate();
                //     System.Diagnostics.Debug.WriteLine($"[MIGRATION DRY RUN] Favorites={dry.FavoritesCount}, Blocked={dry.BlockedCount}, Users={dry.UsersCount}");
                // }
                // catch (Exception ex)
                // {
                //     System.Diagnostics.Debug.WriteLine($"Migration dry-run failed: {ex.Message}");
                // }
            }

            using var userSelectForm = new UserSelectForm(userProfileService, restaurantRepository);
            if (userSelectForm.ShowDialog() != DialogResult.OK || userSelectForm.SelectedUser == null)
            {
                return;
            }

            Application.Run(new MainForm(userSelectForm.SelectedUser, favoriteService, blockedService, userProfileService));
        }
    }
}
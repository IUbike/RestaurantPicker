using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;
using RestaurantPicker.Models;

namespace RestaurantPicker.Repositories
{
    /// <summary>
    /// LiteDB 餐廳資料存取實現
    /// </summary>
    public class LiteDbRestaurantRepository : IRestaurantRepository
    {
        private readonly string _databasePath;
        private readonly string? _legacyCsvPath;

        public LiteDbRestaurantRepository(string databasePath, string? legacyCsvPath = null)
        {
            _databasePath = databasePath;
            _legacyCsvPath = legacyCsvPath;
            EnsureDatabaseSeeded();
        }

        private void EnsureDatabaseSeeded()
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<Restaurant>("restaurants");
            collection.EnsureIndex(r => r.Id, true);

            if (collection.Count() > 0)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(_legacyCsvPath) || !File.Exists(_legacyCsvPath))
            {
                return;
            }

            var csvRepository = new CsvRestaurantRepository(_legacyCsvPath);
            var restaurants = csvRepository.LoadAll();
            if (restaurants.Count == 0)
            {
                return;
            }

            collection.Upsert(restaurants);
        }

        public List<Restaurant> LoadAll()
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<Restaurant>("restaurants");
            collection.EnsureIndex(r => r.Id, true);
            return collection.FindAll().OrderBy(r => r.Id).ToList();
        }

        public Restaurant GetById(int id)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<Restaurant>("restaurants");
            collection.EnsureIndex(r => r.Id, true);
            return collection.FindById(id);
        }

        public void Add(Restaurant restaurant)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<Restaurant>("restaurants");
            collection.EnsureIndex(r => r.Id, true);

            if (restaurant.Id <= 0)
            {
                var maxRestaurant = collection.Query()
                    .OrderByDescending(r => r.Id)
                    .Limit(1)
                    .FirstOrDefault();
                restaurant.Id = (maxRestaurant?.Id ?? 0) + 1;
            }

            collection.Insert(restaurant);
        }

        public void Update(Restaurant restaurant)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<Restaurant>("restaurants");
            collection.EnsureIndex(r => r.Id, true);
            collection.Update(restaurant);
        }

        public void Delete(int id)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<Restaurant>("restaurants");
            collection.EnsureIndex(r => r.Id, true);
            collection.Delete(id);
        }

        public void SaveAll(List<Restaurant> restaurants)
        {
            if (restaurants == null)
            {
                return;
            }

            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<Restaurant>("restaurants");
            collection.EnsureIndex(r => r.Id, true);
            collection.Upsert(restaurants);
        }
    }
}

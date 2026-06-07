using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using RestaurantPicker.Models;

namespace RestaurantPicker.Repositories
{
    public class LiteDbFavoriteRepository
    {
        private readonly string _databasePath;

        public LiteDbFavoriteRepository(string databasePath)
        {
            _databasePath = databasePath;
            EnsureIndex();
        }

        private void EnsureIndex()
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<FavoriteRestaurant>("favorites");
            collection.EnsureIndex(f => f.UserId);
            collection.EnsureIndex(f => f.RestaurantId);
        }

        public List<FavoriteRestaurant> GetByUserId(string userId)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<FavoriteRestaurant>("favorites");
            collection.EnsureIndex(f => f.UserId);
            collection.EnsureIndex(f => f.RestaurantId);
            return collection.Query()
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.CreatedAt)
                .ToList();
        }

        public bool Exists(string userId, int restaurantId)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<FavoriteRestaurant>("favorites");
            collection.EnsureIndex(f => f.UserId);
            collection.EnsureIndex(f => f.RestaurantId);
            return collection.Exists(f => f.UserId == userId && f.RestaurantId == restaurantId);
        }

        public void Add(FavoriteRestaurant favorite)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<FavoriteRestaurant>("favorites");
            collection.EnsureIndex(f => f.UserId);
            collection.EnsureIndex(f => f.RestaurantId);
            collection.Upsert(favorite);
        }

        public void Remove(string userId, int restaurantId)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<FavoriteRestaurant>("favorites");
            collection.EnsureIndex(f => f.UserId);
            collection.EnsureIndex(f => f.RestaurantId);
            collection.DeleteMany(f => f.UserId == userId && f.RestaurantId == restaurantId);
        }

        public int Count()
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<FavoriteRestaurant>("favorites");
            return collection.Count();
        }

        public void DeleteByUserId(string userId)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<FavoriteRestaurant>("favorites");
            collection.DeleteMany(f => f.UserId == userId);
        }
    }
}

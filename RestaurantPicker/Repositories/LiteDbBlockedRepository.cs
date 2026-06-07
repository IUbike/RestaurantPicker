using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using RestaurantPicker.Models;

namespace RestaurantPicker.Repositories
{
    public class LiteDbBlockedRepository
    {
        private readonly string _databasePath;

        public LiteDbBlockedRepository(string databasePath)
        {
            _databasePath = databasePath;
            EnsureIndex();
        }

        private void EnsureIndex()
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<BlockedRestaurant>("blocked");
            collection.EnsureIndex(b => b.UserId);
            collection.EnsureIndex(b => b.RestaurantId);
        }

        public List<BlockedRestaurant> GetByUserId(string userId)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<BlockedRestaurant>("blocked");
            collection.EnsureIndex(b => b.UserId);
            collection.EnsureIndex(b => b.RestaurantId);
            return collection.Query()
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToList();
        }

        public bool Exists(string userId, int restaurantId)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<BlockedRestaurant>("blocked");
            collection.EnsureIndex(b => b.UserId);
            collection.EnsureIndex(b => b.RestaurantId);
            return collection.Exists(b => b.UserId == userId && b.RestaurantId == restaurantId);
        }

        public void Add(BlockedRestaurant blocked)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<BlockedRestaurant>("blocked");
            collection.EnsureIndex(b => b.UserId);
            collection.EnsureIndex(b => b.RestaurantId);
            collection.Upsert(blocked);
        }

        public void Remove(string userId, int restaurantId)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<BlockedRestaurant>("blocked");
            collection.EnsureIndex(b => b.UserId);
            collection.EnsureIndex(b => b.RestaurantId);
            collection.DeleteMany(b => b.UserId == userId && b.RestaurantId == restaurantId);
        }

        public int Count()
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<BlockedRestaurant>("blocked");
            return collection.Count();
        }

        public void DeleteByUserId(string userId)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<BlockedRestaurant>("blocked");
            collection.DeleteMany(b => b.UserId == userId);
        }
    }
}

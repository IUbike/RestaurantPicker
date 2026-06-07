using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using RestaurantPicker.Models;

namespace RestaurantPicker.Repositories
{
    public class LiteDbUserProfileRepository
    {
        private readonly string _databasePath;

        public LiteDbUserProfileRepository(string databasePath)
        {
            _databasePath = databasePath;
            EnsureIndex();
        }

        private void EnsureIndex()
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<UserProfile>("users");
            collection.EnsureIndex(u => u.Id, true);
        }

        public List<UserProfile> LoadAll()
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<UserProfile>("users");
            collection.EnsureIndex(u => u.Id, true);
            return collection.FindAll().ToList();
        }

        public UserProfile? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<UserProfile>("users");
            collection.EnsureIndex(u => u.Id, true);
            return collection.FindOne(u => u.Id == id);
        }

        public UserProfile Add(UserProfile profile)
        {
            if (string.IsNullOrWhiteSpace(profile.Id))
            {
                profile.Id = Guid.NewGuid().ToString("N");
            }

            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<UserProfile>("users");
            collection.EnsureIndex(u => u.Id, true);
            collection.Upsert(profile);
            return profile;
        }

        public void Update(UserProfile profile)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<UserProfile>("users");
            collection.EnsureIndex(u => u.Id, true);
            collection.Update(profile);
        }

        public void Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return;
            }

            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<UserProfile>("users");
            collection.EnsureIndex(u => u.Id, true);
            collection.DeleteMany(u => u.Id == id);
        }
    }
}

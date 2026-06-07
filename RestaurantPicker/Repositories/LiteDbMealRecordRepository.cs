using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using RestaurantPicker.Models;

namespace RestaurantPicker.Repositories
{
    public class LiteDbMealRecordRepository
    {
        private readonly string _databasePath;

        public LiteDbMealRecordRepository(string databasePath)
        {
            _databasePath = databasePath;
            EnsureIndex();
        }

        private void EnsureIndex()
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<MealRecord>("meal_records");
            collection.EnsureIndex(m => m.Id, true);
            collection.EnsureIndex(m => m.UserId);
            collection.EnsureIndex(m => m.RestaurantId);
            collection.EnsureIndex(m => m.MealDate);
            collection.EnsureIndex(m => m.CreatedAt);
        }

        public List<MealRecord> LoadAll()
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<MealRecord>("meal_records");
            collection.EnsureIndex(m => m.Id, true);
            return collection.FindAll().ToList();
        }

        public List<MealRecord> GetByUserId(string userId)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<MealRecord>("meal_records");
            collection.EnsureIndex(m => m.UserId);
            return collection.Query().Where(m => m.UserId == userId).ToList();
        }

        public List<MealRecord> GetByDate(DateTime date, string? userId = null)
        {
            var target = date.Date;
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<MealRecord>("meal_records");
            var query = collection.Query().Where(m => m.MealDate.Date == target);
            if (!string.IsNullOrWhiteSpace(userId))
            {
                query = query.Where(m => m.UserId == userId);
            }

            return query.ToList();
        }

        public MealRecord? GetById(int id)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<MealRecord>("meal_records");
            collection.EnsureIndex(m => m.Id, true);
            return collection.FindById(id);
        }

        public MealRecord Add(MealRecord record)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<MealRecord>("meal_records");
            collection.EnsureIndex(m => m.Id, true);

            if (record.Id == 0)
            {
                var maxRecord = collection.Query().OrderByDescending(m => m.Id).Limit(1).FirstOrDefault();
                record.Id = (maxRecord?.Id ?? 0) + 1;
            }

            collection.Upsert(record);
            return record;
        }

        public void Update(MealRecord record)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<MealRecord>("meal_records");
            collection.EnsureIndex(m => m.Id, true);
            collection.Update(record);
        }

        public void Delete(int id)
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<MealRecord>("meal_records");
            collection.EnsureIndex(m => m.Id, true);
            collection.Delete(id);
        }

        public int Count()
        {
            using var db = new LiteDatabase(_databasePath);
            var collection = db.GetCollection<MealRecord>("meal_records");
            return collection.Count();
        }
    }
}

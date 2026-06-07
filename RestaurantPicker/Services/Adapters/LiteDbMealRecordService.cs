using System;
using System.Collections.Generic;
using RestaurantPicker.Models;
using RestaurantPicker.Repositories;
using RestaurantPicker.Services.Interfaces;

namespace RestaurantPicker.Services.Adapters
{
    public class LiteDbMealRecordService : IMealRecordService
    {
        private readonly LiteDbMealRecordRepository _repo;

        public LiteDbMealRecordService(string databasePath)
        {
            _repo = new LiteDbMealRecordRepository(databasePath);
        }

        public List<MealRecord> LoadAll() => _repo.LoadAll();
        public List<MealRecord> GetByUserId(string userId) => _repo.GetByUserId(userId);
        public List<MealRecord> GetByDate(DateTime date, string? userId = null) => _repo.GetByDate(date, userId);
        public MealRecord? GetById(int id) => _repo.GetById(id);
        public MealRecord Add(MealRecord record) => _repo.Add(record);
        public void Update(MealRecord record) => _repo.Update(record);
        public void Delete(int id) => _repo.Delete(id);
        public int Count() => _repo.Count();
    }
}

using System;
using System.Collections.Generic;
using RestaurantPicker.Models;

namespace RestaurantPicker.Services.Interfaces
{
    public interface IMealRecordService
    {
        List<MealRecord> LoadAll();
        List<MealRecord> GetByUserId(string userId);
        List<MealRecord> GetByDate(DateTime date, string? userId = null);
        MealRecord? GetById(int id);
        MealRecord Add(MealRecord record);
        void Update(MealRecord record);
        void Delete(int id);
        int Count();
    }
}

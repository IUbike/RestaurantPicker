using System;
using System.Collections.Generic;
using System.Linq;
using RestaurantPicker.Models;

namespace RestaurantPicker.Services
{
    /// <summary>
    /// 隨機選擇服務
    /// 負責隨機挑選餐廳和隨機選擇食物種類
    /// </summary>
    public class RandomPickService
    {
        private readonly Random _random;

        public RandomPickService()
        {
            _random = new Random();
        }

        /// <summary>
        /// 從清單中隨機選出一家餐廳
        /// </summary>
        public Restaurant PickRandomRestaurant(List<Restaurant> restaurants)
        {
            if (restaurants == null || restaurants.Count == 0)
                return null;

            int randomIndex = _random.Next(restaurants.Count);
            return restaurants[randomIndex];
        }

        /// <summary>
        /// 從清單中隨機選出指定數量的餐廳
        /// （用於初始化二選一的左右兩家餐廳）
        /// </summary>
        public List<Restaurant> PickRandomRestaurants(List<Restaurant> restaurants, int count)
        {
            if (restaurants == null || restaurants.Count == 0)
                return new List<Restaurant>();

            // 確保不會超過清單裡的數量
            count = Math.Min(count, restaurants.Count);

            // 隨機排序後取前 count 個
            var shuffled = restaurants.OrderBy(r => _random.Next()).ToList();
            return shuffled.Take(count).ToList();
        }

        /// <summary>
        /// 從清單中隨機選擇一個食物種類
        /// 用於「隨機種類」選項
        /// </summary>
        public string PickRandomFoodType(List<string> foodTypes)
        {
            if (foodTypes == null || foodTypes.Count == 0)
                return null;

            int randomIndex = _random.Next(foodTypes.Count);
            return foodTypes[randomIndex];
        }

        /// <summary>
        /// 隨機打亂清單
        /// 用於準備候選餐廳池
        /// </summary>
        public List<Restaurant> ShuffleRestaurants(List<Restaurant> restaurants)
        {
            if (restaurants == null)
                return new List<Restaurant>();

            return restaurants.OrderBy(r => _random.Next()).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using RestaurantPicker.Models;

namespace RestaurantPicker.Services
{
    /// <summary>
    /// 餐廳篩選服務
    /// 負責根據用戶選擇的條件篩選餐廳
    /// </summary>
    public class RestaurantFilterService
    {
        /// <summary>
        /// 根據篩選條件取得符合的餐廳清單
        /// </summary>
        /// <param name="restaurants">所有餐廳清單</param>
        /// <param name="filterOption">篩選條件</param>
        /// <param name="userPreference">使用者偏好（用來排除被封鎖的餐廳）</param>
        /// <returns>符合條件的餐廳清單</returns>
        public List<Restaurant> FilterRestaurants(
            List<Restaurant> restaurants,
            FilterOption filterOption,
            UserPreference userPreference)
        {
            if (restaurants == null || restaurants.Count == 0)
                return new List<Restaurant>();

            var filtered = restaurants.ToList();

            // 第一步：優先用時間區間篩選，若沒提供區間則使用舊版 mealTime
            if (filterOption != null && filterOption.MinMealHour >= 0 && filterOption.MaxMealHour >= 0)
            {
                filtered = FilterByMealTimeRange(filtered, filterOption.MinMealHour, filterOption.MaxMealHour);
            }
            else
            {
                filtered = FilterByMealTime(filtered, filterOption?.MealTime);
            }

            // 第二步：根據食物種類篩選
            filtered = FilterByFoodType(filtered, filterOption?.FoodType);

            // 第三步：排除被使用者封鎖的餐廳
            if (userPreference != null)
            {
                filtered = filtered.Where(r => !userPreference.IsBlocked(r.Id)).ToList();
            }

            return filtered;
        }

        /// <summary>
        /// 根據用餐時段字串篩選（舊版）
        /// </summary>
        public List<Restaurant> FilterByMealTime(List<Restaurant> restaurants, string mealTime)
        {
            if (string.IsNullOrEmpty(mealTime))
                return restaurants;

            return mealTime.ToLower() switch
            {
                "breakfast" => restaurants.Where(r => r.IsBreakfastAvailable).ToList(),
                "lunch" => restaurants.Where(r => r.IsLunchAvailable).ToList(),
                "dinner" => restaurants.Where(r => r.IsDinnerAvailable).ToList(),
                _ => restaurants
            };
        }

        /// <summary>
        /// 根據用餐時間範圍篩選（新版本）
        /// 目前使用早餐/午餐/晚餐可用性做區間映射。
        /// </summary>
        public List<Restaurant> FilterByMealTimeRange(List<Restaurant> restaurants, int minMealHour, int maxMealHour)
        {
            if (restaurants == null)
                return new List<Restaurant>();

            minMealHour = Math.Clamp(minMealHour, 0, 23);
            maxMealHour = Math.Clamp(maxMealHour, 0, 23);
            if (minMealHour > maxMealHour)
            {
                (minMealHour, maxMealHour) = (maxMealHour, minMealHour);
            }

            return restaurants
                .Where(r => IsRestaurantAvailableInHourRange(r, minMealHour, maxMealHour))
                .ToList();
        }

        private bool IsRestaurantAvailableInHourRange(Restaurant restaurant, int minHour, int maxHour)
        {
            // 可依資料組需求調整映射規則
            bool breakfastHit = restaurant.IsBreakfastAvailable && RangesOverlap(minHour, maxHour, 5, 10);
            bool lunchHit = restaurant.IsLunchAvailable && RangesOverlap(minHour, maxHour, 11, 14);
            bool dinnerHit = restaurant.IsDinnerAvailable && RangesOverlap(minHour, maxHour, 17, 22);

            return breakfastHit || lunchHit || dinnerHit;
        }

        private bool RangesOverlap(int aStart, int aEnd, int bStart, int bEnd)
        {
            return aStart <= bEnd && bStart <= aEnd;
        }

        /// <summary>
        /// 根據食物種類篩選
        /// 一家餐廳可能有多個 FoodTypes，只要有一個符合就算符合
        /// </summary>
        public List<Restaurant> FilterByFoodType(List<Restaurant> restaurants, string foodType)
        {
            if (string.IsNullOrEmpty(foodType))
                return restaurants;

            return restaurants.Where(r =>
                r.FoodTypes != null && r.FoodTypes.Contains(foodType)
            ).ToList();
        }

        /// <summary>
        /// 取得指定用餐時段下，所有可用的食物種類
        /// 用途：當使用者選擇「隨機種類」時，從這些種類中隨機選一個
        /// </summary>
        public List<string> GetAvailableFoodTypes(List<Restaurant> restaurants, string mealTime)
        {
            var mealtimeFiltered = FilterByMealTime(restaurants, mealTime);

            var foodTypes = new HashSet<string>();
            foreach (var restaurant in mealtimeFiltered)
            {
                if (restaurant.FoodTypes != null)
                {
                    foreach (var foodType in restaurant.FoodTypes)
                    {
                        foodTypes.Add(foodType);
                    }
                }
            }

            return foodTypes.ToList();
        }

        /// <summary>
        /// 取得指定用餐時間範圍下，所有可用的食物種類
        /// </summary>
        public List<string> GetAvailableFoodTypesByHourRange(List<Restaurant> restaurants, int minMealHour, int maxMealHour)
        {
            var rangeFiltered = FilterByMealTimeRange(restaurants, minMealHour, maxMealHour);

            var foodTypes = new HashSet<string>();
            foreach (var restaurant in rangeFiltered)
            {
                if (restaurant.FoodTypes != null)
                {
                    foreach (var foodType in restaurant.FoodTypes)
                    {
                        foodTypes.Add(foodType);
                    }
                }
            }

            return foodTypes.ToList();
        }

        /// <summary>
        /// 排除被封鎖的餐廳
        /// </summary>
        public List<Restaurant> ExcludeBlocked(List<Restaurant> restaurants, UserPreference userPreference)
        {
            if (userPreference == null || restaurants == null)
                return restaurants;

            return restaurants.Where(r => !userPreference.IsBlocked(r.Id)).ToList();
        }

        /// <summary>
        /// 取得收藏的餐廳
        /// </summary>
        public List<Restaurant> GetFavorites(List<Restaurant> restaurants, UserPreference userPreference)
        {
            if (userPreference == null || restaurants == null)
                return new List<Restaurant>();

            return restaurants.Where(r => userPreference.IsFavorite(r.Id)).ToList();
        }
    }
}

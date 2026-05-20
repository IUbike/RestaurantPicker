using System;
using System.Collections.Generic;
using System.Linq;
using RestaurantPicker.Models;
using RestaurantPicker.Repositories;

namespace RestaurantPicker.Services
{
    /// <summary>
    /// 今日餐廳服務
    /// 管理今日餐廳面板的邏輯（6 個時段槽位）
    /// 現在以紀錄順序決定槽位，不再使用 breakfast/lunch/dinner
    /// </summary>
    public class TodayMealService
    {
        private const string BREAKFAST = "breakfast";
        private const string LUNCH = "lunch";
        private const string DINNER = "dinner";
        private const string CUSTOM = "custom";

        private readonly UserPreferenceService _preferenceService;
        private readonly IRestaurantRepository _restaurantRepository;

        // 存儲今日自訂時段標籤
        private readonly Dictionary<int, string> _customLabels = new Dictionary<int, string>();

        public TodayMealService(UserPreferenceService preferenceService, IRestaurantRepository restaurantRepository)
        {
            _preferenceService = preferenceService;
            _restaurantRepository = restaurantRepository;
        }

        /// <summary>
        /// 取得今日所有時段的用餐資訊（依照建立時間排序，最多 6 筆）
        /// 不再以 breakfast/lunch/dinner 切分，全部以時序填入 6 個槽位
        /// </summary>
        public TodayMealSlot[] GetTodayMeals()
        {
            var slots = new TodayMealSlot[6];

            // 取得今天的紀錄，依建立時間排序
            var todayRecords = _preferenceService.GetMealRecordsForToday()
                .OrderBy(m => m.CreatedAt)
                .ToList();

            for (int i = 0; i < 6; i++)
            {
                if (i < todayRecords.Count)
                {
                    var record = todayRecords[i];
                    var restaurant = _restaurantRepository.GetById(record.RestaurantId);

                    // 嘗試在 repository 未載入時重試一次
                    if (restaurant == null)
                    {
                        try
                        {
                            _restaurantRepository.LoadAll();
                            restaurant = _restaurantRepository.GetById(record.RestaurantId);
                        }
                        catch
                        {
                            // 忽略錯誤
                        }
                    }

                    slots[i] = new TodayMealSlot
                    {
                        MealTime = record.MealTime,
                        Label = record.CustomMealLabel ?? string.Empty,
                        CustomSlotIndex = 0,
                        RestaurantId = record.RestaurantId,
                        RestaurantName = restaurant?.Name ?? $"(找不到餐廳 #{record.RestaurantId})",
                        HasMeal = true,
                        HasRating = record.HasRating,
                        Rating = record.Rating,
                        MealRecordId = record.Id
                    };
                }
                else
                {
                    slots[i] = new TodayMealSlot
                    {
                        MealTime = CUSTOM,
                        Label = string.Empty,
                        CustomSlotIndex = 0,
                        RestaurantId = 0,
                        RestaurantName = string.Empty,
                        HasMeal = false,
                        HasRating = false,
                        Rating = 0,
                        MealRecordId = 0
                    };
                }
            }

            System.Diagnostics.Debug.WriteLine($"[DEBUG] GetTodayMeals -> found {todayRecords.Count} records for today. Mapped to slots.");

            return slots;
        }

        /// <summary>
        /// 設定時段的餐廳和評分（保留以相容性，會刪除同 mealTime 的舊紀錄）
        /// </summary>
        public MealRecord SetMealForSlot(string mealTime, int restaurantId, int rating, string? customLabel = null)
        {
            var existingRecord = _preferenceService.GetMealRecordForTimeSlot(mealTime);
            if (existingRecord != null)
            {
                _preferenceService.DeleteMealRecord(existingRecord.Id);
            }

            var record = new MealRecord
            {
                RestaurantId = restaurantId,
                MealDate = DateTime.Now,
                MealTime = mealTime,
                CustomMealLabel = customLabel,
                Rating = rating,
                HasRating = rating > 0,
                CreatedAt = DateTime.Now
            };

            _preferenceService.AddMealRecord(record);
            System.Diagnostics.Debug.WriteLine($"[DEBUG] SetMealForSlot -> added record Id={record.Id} RestaurantId={restaurantId} MealTime={mealTime}");
            return record;
        }

        /// <summary>
        /// 設定自訂時段的餐廳（相容接口）
        /// </summary>
        public MealRecord SetCustomMealForSlot(int slotIndex, int restaurantId, string label, int rating)
        {
            var record = new MealRecord
            {
                RestaurantId = restaurantId,
                MealDate = DateTime.Now,
                MealTime = CUSTOM,
                CustomMealLabel = label,
                Rating = rating,
                HasRating = rating > 0,
                CreatedAt = DateTime.Now
            };

            _preferenceService.AddMealRecord(record);
            System.Diagnostics.Debug.WriteLine($"[DEBUG] SetCustomMealForSlot -> added custom record Id={record.Id} RestaurantId={restaurantId} Label={label}");
            return record;
        }

        /// <summary>
        /// 新增餐廳到下一個可用的今日槽位（依照時序）。
        /// 超過 6 筆會移除最舊的一筆。
        /// </summary>
        public MealRecord AddMealToNextAvailableSlot(int restaurantId, int rating = 0, string? customLabel = null, string mealTime = CUSTOM)
        {
            _preferenceService.LoadPreferences();
            var todayRecords = _preferenceService.GetMealRecordsForToday()
                .OrderBy(m => m.CreatedAt)
                .ToList();

            var normalizedMealTime = string.IsNullOrWhiteSpace(mealTime) ? CUSTOM : mealTime;

            // 如果已經存在相同餐廳的紀錄（今天內），不再重複新增：改為更新 CreatedAt（移到最後）
            var existingSame = todayRecords.FirstOrDefault(r => r.RestaurantId == restaurantId);
            if (existingSame != null)
            {
                // 更新 CreatedAt 以將其視為最新
                existingSame.CreatedAt = DateTime.Now;
                existingSame.MealTime = normalizedMealTime;
                existingSame.CustomMealLabel = normalizedMealTime == CUSTOM ? (customLabel ?? "其他") : null;
                if (rating > 0)
                {
                    existingSame.Rating = rating;
                    existingSame.HasRating = true;
                }
                _preferenceService.SavePreferences(_preferenceService.GetCurrentPreference());
                System.Diagnostics.Debug.WriteLine($"[DEBUG] AddMealToNextAvailableSlot -> updated existing record Id={existingSame.Id} RestaurantId={restaurantId} MealTime={normalizedMealTime}");
                return existingSame;
            }

            var newRecord = new MealRecord
            {
                RestaurantId = restaurantId,
                MealDate = DateTime.Now,
                MealTime = normalizedMealTime,
                CustomMealLabel = normalizedMealTime == CUSTOM ? (customLabel ?? "其他") : null,
                Rating = rating,
                HasRating = rating > 0,
                CreatedAt = DateTime.Now
            };

            // 如果超過 6 筆，移除最舊的一筆
            if (todayRecords.Count >= 6)
            {
                var oldest = todayRecords.OrderBy(r => r.CreatedAt).FirstOrDefault();
                if (oldest != null)
                {
                    _preferenceService.DeleteMealRecord(oldest.Id);
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] AddMealToNextAvailableSlot -> removed oldest record Id={oldest.Id}");
                }
            }

            _preferenceService.AddMealRecord(newRecord);
            System.Diagnostics.Debug.WriteLine($"[DEBUG] AddMealToNextAvailableSlot -> added new record Id={newRecord.Id} RestaurantId={restaurantId} MealTime={normalizedMealTime}");
            return newRecord;
        }

        /// <summary>
        /// 清除某時段的餐廳紀錄
        /// </summary>
        public void ClearMealSlot(int mealRecordId)
        {
            _preferenceService.DeleteMealRecord(mealRecordId);
        }
    }

    /// <summary>
    /// 今日餐廳時段資訊
    /// </summary>
    public class TodayMealSlot
    {
        /// <summary>
        /// 時段類型：原始紀錄的 MealTime
        /// </summary>
        public string MealTime { get; set; }

        /// <summary>
        /// 時段標籤（如果有）
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 自訂時段的槽位索引（保留欄位）
        /// </summary>
        public int CustomSlotIndex { get; set; }

        /// <summary>
        /// 餐廳 ID（0 表示未選擇）
        /// </summary>
        public int RestaurantId { get; set; }

        /// <summary>
        /// 餐廳名稱
        /// </summary>
        public string RestaurantName { get; set; }

        /// <summary>
        /// 是否已選擇餐廳
        /// </summary>
        public bool HasMeal { get; set; }

        /// <summary>
        /// 是否已評分
        /// </summary>
        public bool HasRating { get; set; }

        /// <summary>
        /// 評分（0-5）
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// 用餐紀錄 ID
        /// </summary>
        public int MealRecordId { get; set; }
    }
}

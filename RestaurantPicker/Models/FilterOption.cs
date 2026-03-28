namespace RestaurantPicker.Models
{
    /// <summary>
    /// 使用者選擇的篩選條件
    /// </summary>
    public class FilterOption
    {
        /// <summary>
        /// 舊版用餐時段: "breakfast", "lunch", "dinner"
        /// </summary>
        public string MealTime { get; set; }

        /// <summary>
        /// 用餐時間範圍（小時，0~23）
        /// </summary>
        public int MinMealHour { get; set; }
        public int MaxMealHour { get; set; }

        /// <summary>
        /// 食物種類 (若 IsRandomCategory 為 true，通常不指定單一種類)
        /// </summary>
        public string FoodType { get; set; }

        /// <summary>
        /// 是否使用隨機種類
        /// </summary>
        public bool IsRandomCategory { get; set; }

        public FilterOption()
        {
            IsRandomCategory = false;
            MinMealHour = -1;
            MaxMealHour = -1;
        }

        public FilterOption(string mealTime, string foodType, bool isRandomCategory)
            : this()
        {
            MealTime = mealTime;
            FoodType = foodType;
            IsRandomCategory = isRandomCategory;
        }

        public FilterOption(int minMealHour, int maxMealHour, string foodType, bool isRandomCategory)
            : this()
        {
            MinMealHour = minMealHour;
            MaxMealHour = maxMealHour;
            FoodType = foodType;
            IsRandomCategory = isRandomCategory;
        }

        public override string ToString()
        {
            return $"用餐時間: {MinMealHour:00}:00-{MaxMealHour:00}:00, 食物種類: {FoodType}, 隨機: {IsRandomCategory}";
        }
    }
}

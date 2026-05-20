using System;

namespace RestaurantPicker.Models
{
    /// <summary>
    /// 用餐記錄模型
    /// 記錄用戶某次用餐的餐廳、時段、評分等信息
    /// </summary>
    public class MealRecord
    {
        /// <summary>
        /// 記錄 ID（自增）
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 餐廳 ID
        /// </summary>
        public int RestaurantId { get; set; }

        /// <summary>
        /// 用餐日期（YYYY-MM-DD 格式）
        /// </summary>
        public DateTime MealDate { get; set; }

        /// <summary>
        /// 時段類型："breakfast"（早餐）/ "lunch"（午餐）/ "dinner"（晚餐）/ "custom"（自訂）
        /// </summary>
        public string MealTime { get; set; }

        /// <summary>
        /// 自訂時段名稱（如果 MealTime == "custom"）
        /// 例如："下午茶"、"宵夜"
        /// </summary>
        public string? CustomMealLabel { get; set; }

        /// <summary>
        /// 評分（0-5 星）
        /// HasRating == false 時，Rating 無效
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// 是否已評分
        /// true: Rating 有效，false: Rating 無效（尚未評分）
        /// </summary>
        public bool HasRating { get; set; }

        /// <summary>
        /// 記錄建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        public MealRecord()
        {
            MealDate = DateTime.Now;
            MealTime = "lunch";
            CustomMealLabel = null;
            Rating = 0;
            HasRating = false;
            CreatedAt = DateTime.Now;
        }
    }
}

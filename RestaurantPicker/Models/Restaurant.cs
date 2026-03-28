using System;
using System.Collections.Generic;

namespace RestaurantPicker.Models
{
    /// <summary>
    /// 餐廳資料模型
    /// </summary>
    public class Restaurant
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        /// <summary>
        /// 價格帶 (例如: "$$", "$$$", "$$$$")
        /// </summary>
        public string PriceRange { get; set; }
        
        /// <summary>
        /// 食物種類清單 (例如: ["台灣菜", "熱炒"])
        /// </summary>
        public List<string> FoodTypes { get; set; } = new List<string>();
        
        /// <summary>
        /// 餐廳特色或美食風格
        /// </summary>
        public string CuisineStyle { get; set; }
        
        /// <summary>
        /// 餐廳用途 (例如: ["聚餐", "約會", "商務"])
        /// </summary>
        public List<string> Purposes { get; set; } = new List<string>();
        
        /// <summary>
        /// 招牌菜 / 特色介紹
        /// </summary>
        public string Feature { get; set; }
        
        /// <summary>
        /// 聯絡電話
        /// </summary>
        public string Phone { get; set; }
        
        /// <summary>
        /// 營業時間 (例如: "11:00-22:00")
        /// </summary>
        public string BusinessHours { get; set; }
        
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// 是否提供早餐
        /// </summary>
        public bool IsBreakfastAvailable { get; set; }
        
        /// <summary>
        /// 是否提供午餐
        /// </summary>
        public bool IsLunchAvailable { get; set; }
        
        /// <summary>
        /// 是否提供晚餐
        /// </summary>
        public bool IsDinnerAvailable { get; set; }
        
        /// <summary>
        /// 餐廳圖片檔名 (用來串接 UI 團隊的資源)
        /// </summary>
        public string ImageFileName { get; set; }
        
        /// <summary>
        /// 是否為使用者收藏
        /// </summary>
        public bool IsFavorite { get; set; }
        
        /// <summary>
        /// 是否被使用者封鎖 (不想再看到)
        /// </summary>
        public bool IsBlocked { get; set; }

        public Restaurant()
        {
            FoodTypes = new List<string>();
            Purposes = new List<string>();
            IsFavorite = false;
            IsBlocked = false;
        }

        public override string ToString()
        {
            return $"{Name} - {string.Join(", ", FoodTypes)}";
        }
    }
}

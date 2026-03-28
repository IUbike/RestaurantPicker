using System;
using System.Collections.Generic;

namespace RestaurantPicker.Models
{
    /// <summary>
    /// 使用者的偏好設定（收藏、封鎖清單）
    /// </summary>
    public class UserPreference
    {
        /// <summary>
        /// 收藏的餐廳 ID 清單
        /// </summary>
        public List<int> FavoriteRestaurantIds { get; set; } = new List<int>();
        
        /// <summary>
        /// 被封鎖的餐廳 ID 清單（使用者不想再看到的餐廳）
        /// </summary>
        public List<int> BlockedRestaurantIds { get; set; } = new List<int>();

        public UserPreference()
        {
            FavoriteRestaurantIds = new List<int>();
            BlockedRestaurantIds = new List<int>();
        }

        /// <summary>
        /// 檢查餐廳是否被收藏
        /// </summary>
        public bool IsFavorite(int restaurantId)
        {
            return FavoriteRestaurantIds.Contains(restaurantId);
        }

        /// <summary>
        /// 檢查餐廳是否被封鎖
        /// </summary>
        public bool IsBlocked(int restaurantId)
        {
            return BlockedRestaurantIds.Contains(restaurantId);
        }

        /// <summary>
        /// 新增收藏
        /// </summary>
        public void AddFavorite(int restaurantId)
        {
            if (!FavoriteRestaurantIds.Contains(restaurantId))
            {
                FavoriteRestaurantIds.Add(restaurantId);
            }
        }

        /// <summary>
        /// 移除收藏
        /// </summary>
        public void RemoveFavorite(int restaurantId)
        {
            FavoriteRestaurantIds.Remove(restaurantId);
        }

        /// <summary>
        /// 新增封鎖
        /// </summary>
        public void AddBlocked(int restaurantId)
        {
            if (!BlockedRestaurantIds.Contains(restaurantId))
            {
                BlockedRestaurantIds.Add(restaurantId);
            }
        }

        /// <summary>
        /// 移除封鎖
        /// </summary>
        public void RemoveBlocked(int restaurantId)
        {
            BlockedRestaurantIds.Remove(restaurantId);
        }
    }
}

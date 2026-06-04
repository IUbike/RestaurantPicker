using System;

namespace RestaurantPicker.Models
{
    public class FavoriteRestaurant
    {
        public string UserId { get; set; } = string.Empty;

        public int RestaurantId { get; set; }

        public string RestaurantName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}

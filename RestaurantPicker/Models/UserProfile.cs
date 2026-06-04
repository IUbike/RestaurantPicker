using System.Collections.Generic;

namespace RestaurantPicker.Models
{
    public class UserProfile
    {
        public string Id { get; set; } = string.Empty;

        public string Nickname { get; set; } = string.Empty;

        public string AvatarPath { get; set; } = string.Empty;

        public List<string> PreferredTags { get; set; } = new List<string>();
    }
}

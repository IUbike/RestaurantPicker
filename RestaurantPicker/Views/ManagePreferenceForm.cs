using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RestaurantPicker.Models;
using RestaurantPicker.Repositories;
using RestaurantPicker.Services;

namespace RestaurantPicker.Views
{
    public partial class ManagePreferenceForm : Form
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly UserPreferenceService _preferenceService;
        private List<Restaurant> _allRestaurants = new List<Restaurant>();

        public ManagePreferenceForm(IRestaurantRepository restaurantRepository)
        {
            InitializeComponent();
            _restaurantRepository = restaurantRepository;

            string preferencePath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "user_preferences.json"
            );
            _preferenceService = new UserPreferenceService(preferencePath);
            _preferenceService.LoadPreferences();

            Text = "管理收藏 / 封鎖清單";
            StartPosition = FormStartPosition.CenterScreen;
            AcceptButton = btnClose;
            CancelButton = btnClose;
        }

        private void ManagePreferenceForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            _allRestaurants = _restaurantRepository.LoadAll();

            lstAllRestaurants.Items.Clear();
            foreach (var restaurant in _allRestaurants.OrderBy(r => r.Name))
            {
                lstAllRestaurants.Items.Add(new RestaurantListItem(restaurant.Id, restaurant.Name));
            }

            RefreshPreferenceLists();
        }

        private void RefreshPreferenceLists()
        {
            var preference = _preferenceService.GetCurrentPreference();

            lstFavorites.Items.Clear();
            foreach (var id in preference.FavoriteRestaurantIds)
            {
                var restaurant = _allRestaurants.FirstOrDefault(r => r.Id == id);
                if (restaurant != null)
                {
                    lstFavorites.Items.Add(new RestaurantListItem(restaurant.Id, restaurant.Name));
                }
            }

            lstBlocked.Items.Clear();
            foreach (var id in preference.BlockedRestaurantIds)
            {
                var restaurant = _allRestaurants.FirstOrDefault(r => r.Id == id);
                if (restaurant != null)
                {
                    lstBlocked.Items.Add(new RestaurantListItem(restaurant.Id, restaurant.Name));
                }
            }
        }

        private int? GetSelectedRestaurantIdFromAll()
        {
            if (lstAllRestaurants.SelectedItem is not RestaurantListItem item)
                return null;

            return item.Id;
        }

        private int? GetSelectedRestaurantId(ListBox listBox)
        {
            if (listBox.SelectedItem is not RestaurantListItem item)
                return null;

            return item.Id;
        }

        private void btnAddFavorite_Click(object sender, EventArgs e)
        {
            var id = GetSelectedRestaurantIdFromAll();
            if (!id.HasValue)
            {
                MessageBox.Show("請先從左側選擇一家餐廳", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 收藏與封鎖互斥：加入收藏時自動移出封鎖
            _preferenceService.RemoveBlocked(id.Value);
            _preferenceService.AddFavorite(id.Value);
            RefreshPreferenceLists();
        }

        private void btnAddBlocked_Click(object sender, EventArgs e)
        {
            var id = GetSelectedRestaurantIdFromAll();
            if (!id.HasValue)
            {
                MessageBox.Show("請先從左側選擇一家餐廳", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 收藏與封鎖互斥：加入封鎖時自動移出收藏
            _preferenceService.RemoveFavorite(id.Value);
            _preferenceService.AddBlocked(id.Value);
            RefreshPreferenceLists();
        }

        private void btnRemoveFavorite_Click(object sender, EventArgs e)
        {
            var id = GetSelectedRestaurantId(lstFavorites);
            if (!id.HasValue)
            {
                MessageBox.Show("請先在『收藏清單』選擇要移除的餐廳", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _preferenceService.RemoveFavorite(id.Value);
            RefreshPreferenceLists();
        }

        private void btnRemoveBlocked_Click(object sender, EventArgs e)
        {
            var id = GetSelectedRestaurantId(lstBlocked);
            if (!id.HasValue)
            {
                MessageBox.Show("請先在『封鎖清單』選擇要移除的餐廳", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _preferenceService.RemoveBlocked(id.Value);
            RefreshPreferenceLists();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private sealed class RestaurantListItem
        {
            public int Id { get; }
            public string Name { get; }

            public RestaurantListItem(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public override string ToString()
            {
                return $"{Name} (#{Id})";
            }
        }
    }
}

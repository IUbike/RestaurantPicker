using System;
using System.Collections.Generic;
using System.Drawing;
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
        private readonly FavoriteService _favoriteService;
        private readonly BlockedService _blockedService;
        private readonly UserProfile _currentUser;
        private List<Restaurant> _allRestaurants = new List<Restaurant>();

        public ManagePreferenceForm(IRestaurantRepository restaurantRepository, UserProfile currentUser, FavoriteService favoriteService, BlockedService blockedService)
        {
            InitializeComponent();
            _restaurantRepository = restaurantRepository;
            _currentUser = currentUser;
            _favoriteService = favoriteService;
            _blockedService = blockedService;

            if (_currentUser == null)
            {
                MessageBox.Show(
                    LanguageManager.CurrentLanguage == LanguageType.Chinese ? "請先選擇使用者" : "Please select a user first",
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            string databasePath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "restaurantpicker.db"
            );
            string preferencePath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "user_preferences.json"
            );
            _preferenceService = new UserPreferenceService(databasePath, preferencePath);
            _preferenceService.LoadPreferences();

            this.StartPosition = FormStartPosition.CenterScreen;
            AcceptButton = btnClose;
            CancelButton = btnClose;
            this.BackgroundImage = LanguageManager.LoadAssetImage("back3.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void ManagePreferenceForm_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            LoadData();
        }

        private void ApplyLanguage()
        {
            var titleName = _currentUser?.Nickname ?? string.Empty;
            this.Text = string.IsNullOrWhiteSpace(titleName)
                ? LanguageManager.GetTranslation("preferenceTitle")
                : $"{titleName} 的收藏餐廳";
            lblAll.Text = LanguageManager.GetTranslation("lblAll");
            lblFavorites.Text = LanguageManager.GetTranslation("lblFavorites");
            lblBlocked.Text = LanguageManager.GetTranslation("lblBlocked");
            lblHint.Text = LanguageManager.GetTranslation("lblHint");
            btnAddFavorite.Text = LanguageManager.GetTranslation("btnAddFavorite");
            btnAddBlocked.Text = LanguageManager.GetTranslation("btnAddBlocked");
            btnRemoveFavorite.Text = LanguageManager.GetTranslation("btnRemoveFavorite");
            btnRemoveBlocked.Text = LanguageManager.GetTranslation("btnRemoveBlocked");

            LanguageManager.ApplyFullButtonImage(btnClose, "icons_complete.png");
            MakeControlsTransparent(this);
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
            var favorites = _favoriteService.GetByUserId(_currentUser.Id);
            var blocked = _blockedService.GetByUserId(_currentUser.Id);

            lstFavorites.Items.Clear();
            foreach (var favorite in favorites)
            {
                var restaurant = _allRestaurants.FirstOrDefault(r => r.Id == favorite.RestaurantId);
                var name = restaurant?.Name ?? favorite.RestaurantName;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    lstFavorites.Items.Add(new RestaurantListItem(favorite.RestaurantId, name));
                }
            }

            if (lstFavorites.Items.Count == 0)
            {
                lstFavorites.Items.Add(LanguageManager.CurrentLanguage == LanguageType.Chinese
                    ? "目前尚無收藏餐廳"
                    : "No favorites yet");
            }

            btnRemoveFavorite.Enabled = favorites.Count > 0;

            lstBlocked.Items.Clear();
            foreach (var block in blocked)
            {
                var restaurant = _allRestaurants.FirstOrDefault(r => r.Id == block.RestaurantId);
                var name = restaurant?.Name ?? block.RestaurantName;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    lstBlocked.Items.Add(new RestaurantListItem(block.RestaurantId, name));
                }
            }

            if (lstBlocked.Items.Count == 0)
            {
                lstBlocked.Items.Add(LanguageManager.CurrentLanguage == LanguageType.Chinese
                    ? "目前尚無封鎖餐廳"
                    : "No blocked restaurants");
            }

            btnRemoveBlocked.Enabled = blocked.Count > 0;
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
                MessageBox.Show(
                    LanguageManager.GetTranslation("selectLeftPrompt"),
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // 收藏與封鎖互斥：加入收藏時自動移出封鎖
            _blockedService.RemoveBlocked(_currentUser.Id, id.Value);
            var restaurant = _allRestaurants.FirstOrDefault(r => r.Id == id.Value);
            if (restaurant != null)
            {
                _favoriteService.AddFavorite(_currentUser.Id, restaurant);
            }
            RefreshPreferenceLists();
        }

        private void btnAddBlocked_Click(object sender, EventArgs e)
        {
            var id = GetSelectedRestaurantIdFromAll();
            if (!id.HasValue)
            {
                MessageBox.Show(
                    LanguageManager.GetTranslation("selectLeftPrompt"),
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // 收藏與封鎖互斥：加入封鎖時自動移出收藏
            _favoriteService.RemoveFavorite(_currentUser.Id, id.Value);
            var restaurant = _allRestaurants.FirstOrDefault(r => r.Id == id.Value);
            if (restaurant != null)
            {
                _blockedService.AddBlocked(_currentUser.Id, restaurant);
            }
            RefreshPreferenceLists();
        }

        private void btnRemoveFavorite_Click(object sender, EventArgs e)
        {
            var id = GetSelectedRestaurantId(lstFavorites);
            if (!id.HasValue)
            {
                MessageBox.Show(
                    LanguageManager.GetTranslation("selectFavoritePrompt"),
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            _favoriteService.RemoveFavorite(_currentUser.Id, id.Value);
            RefreshPreferenceLists();
        }

        private void btnRemoveBlocked_Click(object sender, EventArgs e)
        {
            var id = GetSelectedRestaurantId(lstBlocked);
            if (!id.HasValue)
            {
                MessageBox.Show(
                    LanguageManager.GetTranslation("selectBlockedPrompt"),
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            _blockedService.RemoveBlocked(_currentUser.Id, id.Value);
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

        private void MakeControlsTransparent(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Label || ctrl is RadioButton || ctrl is Panel || ctrl is GroupBox || ctrl is CheckBox)
                {
                    ctrl.BackColor = Color.Transparent;
                }
                if (ctrl.HasChildren)
                {
                    MakeControlsTransparent(ctrl);
                }
            }
        }
    }
}

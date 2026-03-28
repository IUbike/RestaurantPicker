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
    public partial class CategorySelectForm : Form
    {
        // 傳入的用餐時間範圍
        private readonly int _minMealHour;
        private readonly int _maxMealHour;

        // 服務層
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly RestaurantFilterService _filterService;
        private readonly UserPreferenceService _preferenceService;

        // 類別清單狀態
        private List<string> _allAvailableFoodTypes = new List<string>();
        private readonly List<string> _selectedFoodTypes = new List<string>();

        // 存儲使用者選擇
        public string SelectedFoodType { get; private set; }
        public bool IsRandomCategory { get; private set; }

        public CategorySelectForm(int minMealHour, int maxMealHour)
        {
            InitializeComponent();
            _minMealHour = minMealHour;
            _maxMealHour = maxMealHour;
            Text = "選擇食物種類";
            StartPosition = FormStartPosition.CenterScreen;

            string csvPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "restaurants.csv"
            );
            _restaurantRepository = new CsvRestaurantRepository(csvPath);
            _filterService = new RestaurantFilterService();

            string preferencePath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "user_preferences.json"
            );
            _preferenceService = new UserPreferenceService(preferencePath);
            _preferenceService.LoadPreferences();

            AcceptButton = btnNext;
            CancelButton = btnCancel;
        }

        private void CategorySelectForm_Load(object sender, EventArgs e)
        {
            LoadAvailableCategories();
        }

        private List<Restaurant> GetMealTimeAvailableRestaurants()
        {
            var allRestaurants = _restaurantRepository.LoadAll();
            var mealTimeRestaurants = _filterService.FilterByMealTimeRange(allRestaurants, _minMealHour, _maxMealHour);
            var preference = _preferenceService.GetCurrentPreference();
            return _filterService.ExcludeBlocked(mealTimeRestaurants, preference);
        }

        private void LoadAvailableCategories()
        {
            try
            {
                var mealTimeRestaurants = GetMealTimeAvailableRestaurants();
                _allAvailableFoodTypes = mealTimeRestaurants
                    .Where(r => r.FoodTypes != null)
                    .SelectMany(r => r.FoodTypes)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                RefreshCategoryCombo();

                rbSpecific.Checked = true;
                RefreshSelectedTagChips();
                UpdateCandidateCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"載入種類失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshCategoryCombo()
        {
            cbCategory.Items.Clear();
            foreach (var foodType in _allAvailableFoodTypes.Where(x => !_selectedFoodTypes.Contains(x)))
            {
                cbCategory.Items.Add(foodType);
            }

            if (cbCategory.Items.Count > 0)
            {
                cbCategory.SelectedIndex = 0;
            }
        }

        private void btnAddTag_Click(object sender, EventArgs e)
        {
            if (cbCategory.SelectedIndex < 0)
            {
                MessageBox.Show("請先從清單選擇一個種類", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedTag = cbCategory.SelectedItem.ToString();
            if (string.IsNullOrWhiteSpace(selectedTag))
                return;

            if (!_selectedFoodTypes.Contains(selectedTag))
            {
                _selectedFoodTypes.Add(selectedTag);
            }

            RefreshCategoryCombo();
            RefreshSelectedTagChips();
            UpdateCandidateCount();
        }

        private void btnClearTags_Click(object sender, EventArgs e)
        {
            _selectedFoodTypes.Clear();
            RefreshCategoryCombo();
            RefreshSelectedTagChips();
            UpdateCandidateCount();
        }

        private void RefreshSelectedTagChips()
        {
            pnlSelectedTags.Controls.Clear();

            foreach (var tag in _selectedFoodTypes)
            {
                var chip = new Button
                {
                    AutoSize = true,
                    Text = $"{tag} ×",
                    BackColor = Color.LightSteelBlue,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(4),
                    Tag = tag
                };
                chip.FlatAppearance.BorderColor = Color.SteelBlue;
                chip.Click += RemoveTagChip_Click;
                pnlSelectedTags.Controls.Add(chip);
            }

            btnClearTags.Enabled = _selectedFoodTypes.Count > 0;
        }

        private void RemoveTagChip_Click(object sender, EventArgs e)
        {
            if (sender is not Button chip || chip.Tag is not string tag)
                return;

            _selectedFoodTypes.Remove(tag);
            RefreshCategoryCombo();
            RefreshSelectedTagChips();
            UpdateCandidateCount();
        }

        private void UpdateCandidateCount()
        {
            try
            {
                var mealTimeRestaurants = GetMealTimeAvailableRestaurants();

                int candidateCount;
                if (rbRandom.Checked)
                {
                    candidateCount = mealTimeRestaurants.Count;
                }
                else
                {
                    if (_selectedFoodTypes.Count == 0)
                    {
                        candidateCount = 0;
                    }
                    else
                    {
                        candidateCount = mealTimeRestaurants
                            .Where(r => r.FoodTypes != null && r.FoodTypes.Any(ft => _selectedFoodTypes.Contains(ft)))
                            .Count();
                    }
                }

                lblCandidateCount.Text = $"候選餐廳數量：{candidateCount} 家";

                if (rbSpecific.Checked && _selectedFoodTypes.Count > 0)
                {
                    lblCandidateCount.ForeColor = candidateCount < 2 ? Color.DarkRed : Color.DarkGreen;
                }
                else
                {
                    lblCandidateCount.ForeColor = Color.DarkGreen;
                }
            }
            catch
            {
                lblCandidateCount.Text = "候選餐廳數量：計算失敗";
                lblCandidateCount.ForeColor = Color.DarkRed;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (rbSpecific.Checked)
            {
                if (_selectedFoodTypes.Count == 0)
                {
                    MessageBox.Show("請先加入至少一個種類標籤", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var mealTimeRestaurants = GetMealTimeAvailableRestaurants();
                var candidateCount = mealTimeRestaurants
                    .Where(r => r.FoodTypes != null && r.FoodTypes.Any(ft => _selectedFoodTypes.Contains(ft)))
                    .Count();

                if (candidateCount < 2)
                {
                    MessageBox.Show("目前候選餐廳不足 2 家，請調整標籤。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SelectedFoodType = _selectedFoodTypes.First();
                IsRandomCategory = false;
            }
            else
            {
                SelectedFoodType = null;
                IsRandomCategory = true;
            }

            using var swipeForm = new SwipeForm(_minMealHour, _maxMealHour, _selectedFoodTypes.ToList(), IsRandomCategory);
            if (swipeForm.ShowDialog() == DialogResult.OK)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void rbSpecific_CheckedChanged(object sender, EventArgs e)
        {
            cbCategory.Enabled = rbSpecific.Checked;
            btnAddTag.Enabled = rbSpecific.Checked;
            btnClearTags.Enabled = rbSpecific.Checked && _selectedFoodTypes.Count > 0;
            pnlSelectedTags.Enabled = rbSpecific.Checked;
            pnlSelectedTags.Visible = rbSpecific.Checked;
            lblSelectedTitle.Visible = rbSpecific.Checked;
            UpdateCandidateCount();
        }

        private void rbRandom_CheckedChanged(object sender, EventArgs e)
        {
            UpdateCandidateCount();
        }
    }
}

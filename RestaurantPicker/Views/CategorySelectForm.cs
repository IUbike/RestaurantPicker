using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RestaurantPicker.Models;
using RestaurantPicker.Repositories;
using RestaurantPicker.Services;
using RestaurantPicker.Services.Interfaces;

namespace RestaurantPicker.Views
{
    public partial class CategorySelectForm : Form
    {
        // 傳入的用餐時間範圍
        private readonly int _minMealHour;
        private readonly int _maxMealHour;

        // 用餐時段類型：breakfast/lunch/dinner
        private readonly string _mealTimeType;

        // 服務層
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly RestaurantFilterService _filterService;
        private readonly UserProfile _currentUser;
        private readonly IFavoriteService _favoriteService;
        private readonly IBlockedService _blockedService;

        // 類別清單狀態
        private List<string> _allAvailableFoodTypes = new List<string>();
        private readonly List<string> _selectedFoodTypes = new List<string>();

        // 存儲使用者選擇
        public string SelectedFoodType { get; private set; }
        public bool IsRandomCategory { get; private set; }

        public CategorySelectForm(int minMealHour, int maxMealHour, string mealTimeType, UserProfile currentUser, IFavoriteService favoriteService, IBlockedService blockedService)
        {
            InitializeComponent();
            _minMealHour = minMealHour;
            _maxMealHour = maxMealHour;
            Text = "選擇食物種類";
            StartPosition = FormStartPosition.CenterScreen;
            this.BackgroundImage = LanguageManager.LoadAssetImage("back3.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            _currentUser = currentUser;
            _favoriteService = favoriteService;
            _blockedService = blockedService;

            string csvPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "restaurants.csv"
            );
            string databasePath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "restaurantpicker.db"
            );
            _restaurantRepository = new LiteDbRestaurantRepository(databasePath, csvPath);
            _filterService = new RestaurantFilterService();

            AcceptButton = btnNext;
            CancelButton = btnCancel;
        }

        private void btnUsePreferredTags_Click(object sender, EventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show(
                    LanguageManager.CurrentLanguage == LanguageType.Chinese ? "請先選擇使用者" : "Please select a user first",
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (_currentUser.PreferredTags == null || _currentUser.PreferredTags.Count == 0)
            {
                MessageBox.Show(
                    LanguageManager.CurrentLanguage == LanguageType.Chinese ? "目前尚未設定偏好標籤" : "No preferred tags configured",
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var matchedTags = _currentUser.PreferredTags
                .Where(tag => _allAvailableFoodTypes.Any(a => string.Equals(a, tag, StringComparison.OrdinalIgnoreCase)))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (matchedTags.Count == 0)
            {
                MessageBox.Show(
                    LanguageManager.CurrentLanguage == LanguageType.Chinese ? "目前尚未設定偏好標籤" : "No preferred tags configured",
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            rbSpecific.Checked = true;
            _selectedFoodTypes.Clear();
            _selectedFoodTypes.AddRange(matchedTags);
            RefreshCategoryCombo();
            RefreshSelectedTagChips();
            UpdateCandidateCount();
        }

        private void CategorySelectForm_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            btnUsePreferredTags.Enabled = _currentUser != null;
            LoadAvailableCategories();
        }

        private void ApplyLanguage()
        {
            this.Text = LanguageManager.GetTranslation("categoryTitle");
            lblTitle.Text = LanguageManager.GetTranslation("categoryHeader");
            groupBoxCategory.Text = LanguageManager.GetTranslation("categorySelection");
            rbSpecific.Text = LanguageManager.GetTranslation("selectCategory");
            rbRandom.Text = LanguageManager.GetTranslation("randomCategory");
            lblSelectedTitle.Text = LanguageManager.GetTranslation("lblSelectedTitle");
            lblTagHint.Text = LanguageManager.GetTranslation("tagHint");

            // Make labels, radiobuttons, panels, groupboxes transparent recursively
            MakeControlsTransparent(this);

            btnUsePreferredTags.Text = LanguageManager.CurrentLanguage == LanguageType.Chinese
                ? "使用我的偏好標籤"
                : "Use my preferred tags";

            // Apply full-button images dynamically
            LanguageManager.ApplyFullButtonImage(btnAddTag, "icons_add.png");
            LanguageManager.ApplyFullButtonImage(btnClearTags, "icons_clear.png");
            LanguageManager.ApplyFullButtonImage(btnNext, "icons_next.png");
            LanguageManager.ApplyFullButtonImage(btnCancel, "icons_cancel.png");
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

        private List<Restaurant> GetMealTimeAvailableRestaurants()
        {
            var allRestaurants = _restaurantRepository.LoadAll();
            var mealTimeRestaurants = _filterService.FilterByMealTimeRange(allRestaurants, _minMealHour, _maxMealHour);
            if (_currentUser == null)
            {
                return mealTimeRestaurants;
            }

            var blockedIds = _blockedService.GetByUserId(_currentUser.Id)
                .Select(b => b.RestaurantId)
                .ToHashSet();
            return mealTimeRestaurants.Where(r => !blockedIds.Contains(r.Id)).ToList();
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
                MessageBox.Show(
                    LanguageManager.CurrentLanguage == LanguageType.Chinese ? $"載入種類失敗: {ex.Message}" : $"Failed to load food styles: {ex.Message}",
                    LanguageManager.GetTranslation("resetFailedTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RefreshCategoryCombo()
        {
            cbCategory.Items.Clear();
            foreach (var foodType in _allAvailableFoodTypes.Where(x => !_selectedFoodTypes.Contains(x)))
            {
                cbCategory.Items.Add(LanguageManager.GetLocalizedTag(foodType));
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
                MessageBox.Show(
                    LanguageManager.CurrentLanguage == LanguageType.Chinese ? "請先從清單選擇一個種類" : "Please select a category from the list first",
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var displayedTag = cbCategory.SelectedItem.ToString();
            if (string.IsNullOrWhiteSpace(displayedTag))
                return;

            var selectedTag = LanguageManager.GetChineseTag(displayedTag);

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
                    Text = $"{LanguageManager.GetLocalizedTag(tag)} ×",
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

                lblCandidateCount.Text = LanguageManager.GetTranslation("candidatePrefix") + candidateCount + (LanguageManager.CurrentLanguage == LanguageType.Chinese ? " 家" : "");

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
                lblCandidateCount.Text = LanguageManager.CurrentLanguage == LanguageType.Chinese ? "候選餐廳數量：計算失敗" : "Candidates: calculation failed";
                lblCandidateCount.ForeColor = Color.DarkRed;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (rbSpecific.Checked)
            {
                if (_selectedFoodTypes.Count == 0)
                {
                    MessageBox.Show(
                        LanguageManager.GetTranslation("tagPrompt"),
                        LanguageManager.GetTranslation("hintTitle"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var mealTimeRestaurants = GetMealTimeAvailableRestaurants();
                var candidateCount = mealTimeRestaurants
                    .Where(r => r.FoodTypes != null && r.FoodTypes.Any(ft => _selectedFoodTypes.Contains(ft)))
                    .Count();

                if (candidateCount < 2)
                {
                    MessageBox.Show(
                        LanguageManager.GetTranslation("insufficientCandidates"),
                        LanguageManager.GetTranslation("hintTitle"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
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

            using var swipeForm = new SwipeForm(_minMealHour, _maxMealHour, _selectedFoodTypes.ToList(), IsRandomCategory, _mealTimeType, _currentUser, _favoriteService, _blockedService);
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
            btnUsePreferredTags.Enabled = rbSpecific.Checked && _currentUser != null;
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

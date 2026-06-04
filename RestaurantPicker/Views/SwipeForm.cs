using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RestaurantPicker.Models;
using RestaurantPicker.Repositories;
using RestaurantPicker.Services;

namespace RestaurantPicker.Views
{
    public partial class SwipeForm : Form
    {
        // 傳入的篩選條件
        private readonly int _minMealHour;
        private readonly int _maxMealHour;
        private string _foodType;
        private bool _isRandomCategory;
        private readonly List<string> _selectedFoodTypes;

        // 用餐時段類型：breakfast/lunch/dinner（從 CategorySelectForm 傳入）
        private string _mealTimeType = "lunch";

        // 服務層
        private IRestaurantRepository _restaurantRepository;
        private RestaurantFilterService _filterService;
        private SwipeMatchService _swipeMatchService;
        private UserPreferenceService _preferenceService;
        private readonly UserProfile _currentUser;
        private readonly FavoriteService _favoriteService;
        private readonly BlockedService _blockedService;

        private int _totalFilteredCount;

        // 舊版相容建構子
        public SwipeForm(string mealTime, string foodType, bool isRandomCategory, UserProfile currentUser, FavoriteService favoriteService, BlockedService blockedService)
            : this(
                mealTime?.ToLower() switch
                {
                    "breakfast" => 6,
                    "lunch" => 11,
                    "dinner" => 17,
                    _ => 11
                },
                mealTime?.ToLower() switch
                {
                    "breakfast" => 10,
                    "lunch" => 14,
                    "dinner" => 21,
                    _ => 14
                },
                string.IsNullOrWhiteSpace(foodType) ? new List<string>() : new List<string> { foodType },
                isRandomCategory,
                "lunch",
                currentUser,
                favoriteService,
                blockedService)
        {
        }

        public SwipeForm(int minMealHour, int maxMealHour, List<string> selectedFoodTypes, bool isRandomCategory, string mealTimeType, UserProfile currentUser, FavoriteService favoriteService, BlockedService blockedService)
        {
            InitializeComponent();
            _minMealHour = Math.Clamp(minMealHour, 0, 23);
            _maxMealHour = Math.Clamp(maxMealHour, 0, 23);
            _isRandomCategory = isRandomCategory;
            _selectedFoodTypes = selectedFoodTypes ?? new List<string>();
            _foodType = _selectedFoodTypes.FirstOrDefault();
            _mealTimeType = mealTimeType;  // 記錄用餐時段類型
            _currentUser = currentUser;
            _favoriteService = favoriteService;
            _blockedService = blockedService;

            this.Text = "選擇餐廳";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackgroundImage = LanguageManager.LoadAssetImage("back3.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // 初始化服務
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
            _swipeMatchService = new SwipeMatchService();

            // 初始化使用者偏好服務
            string preferencePath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "user_preferences.json"
            );
            _preferenceService = new UserPreferenceService(databasePath, preferencePath);
            _preferenceService.LoadPreferences();
        }

        private void SwipeForm_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            InitializeSwipeCards();
        }

        private void ApplyLanguage()
        {
            this.Text = LanguageManager.GetTranslation("swipeTitle");

            // Make labels, panels transparent for the background
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label || ctrl is Panel)
                {
                    ctrl.BackColor = Color.Transparent;
                }
            }

            // Make PictureBoxes transparent so they show the card's white background instead of grey
            picLeftImage.BackColor = Color.Transparent;
            picRightImage.BackColor = Color.Transparent;

            // Apply full-button images dynamically (LanguageManager handles English automatic _e naming!)
            LanguageManager.ApplyFullButtonImage(btnSelectLeft, "icons_left.png");
            LanguageManager.ApplyFullButtonImage(btnSelectRight, "icons_right.png");
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left && btnSelectLeft.Enabled)
            {
                btnSelectLeft.PerformClick();
                return true;
            }

            if (keyData == Keys.Right && btnSelectRight.Enabled)
            {
                btnSelectRight.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void UpdateFilterSummaryTitle()
        {
            string typeStr = _isRandomCategory
                ? LanguageManager.GetTranslation("randomCategory")
                : (_selectedFoodTypes.Count > 0 ? string.Join(", ", _selectedFoodTypes) : LanguageManager.GetTranslation("selectCategory"));
            
            string hint = LanguageManager.CurrentLanguage == LanguageType.Chinese
                ? "（←/→ 可快速選擇）"
                : " (←/→ keys to select)";

            lblTitle.Text = $"{_minMealHour:00}:00~{_maxMealHour:00}:00 | {typeStr}{hint}";
        }

        private Image CreatePlaceholderImage(Size size)
        {
            int width = Math.Max(size.Width, 40);
            int height = Math.Max(size.Height, 40);
            var bitmap = new Bitmap(width, height);
            using var g = Graphics.FromImage(bitmap);
            g.Clear(Color.Transparent);
            using var brush = new SolidBrush(Color.DimGray);
            using var font = new Font("微軟正黑體", 10F, FontStyle.Regular);
            var rect = new RectangleF(0, 0, width, height);
            var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString(LanguageManager.CurrentLanguage == LanguageType.Chinese ? "暫無圖片" : "No Image", font, brush, rect, format);
            return bitmap;
        }

        private Image? LoadRestaurantImage(string? imageFileName)
        {
            if (string.IsNullOrWhiteSpace(imageFileName))
                return null;

            try
            {
                string imagesDir = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Assets",
                    "images"
                );

                // 1. 先嘗試精確文件名匹配
                string imagePath = Path.Combine(imagesDir, imageFileName);
                if (File.Exists(imagePath))
                {
                    using var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using var temp = Image.FromStream(fs);
                    return new Bitmap(temp);
                }

                // 2. 若精確匹配失敗，嘗試去掉前導零的版本
                // 例如：restaurant_01.jpg → restaurant_1.jpg
                if (imageFileName.Contains("restaurant_") && imageFileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    string withoutZero = System.Text.RegularExpressions.Regex.Replace(imageFileName, @"restaurant_0+(\d+)\.jpg", "restaurant_$1.jpg", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (withoutZero != imageFileName)
                    {
                        imagePath = Path.Combine(imagesDir, withoutZero);
                        if (File.Exists(imagePath))
                        {
                            using var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            using var temp = Image.FromStream(fs);
                            return new Bitmap(temp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"載入圖片失敗 {imageFileName}: {ex.Message}");
            }

            return null;
        }

        private void SetRestaurantImage(PictureBox pictureBox, Restaurant restaurant)
        {
            pictureBox.Image?.Dispose();
            pictureBox.Image = LoadRestaurantImage(restaurant.ImageFileName) ?? CreatePlaceholderImage(pictureBox.Size);
        }

        /// <summary>
        /// 初始化左右兩張餐廳卡片
        /// 核心的篩選與初始化邏輯
        /// </summary>
        private void InitializeSwipeCards()
        {
            try
            {
                var allRestaurants = _restaurantRepository.LoadAll();
                // 先依時段 + 封鎖篩選
                var mealTimeFiltered = _filterService.FilterByMealTimeRange(allRestaurants, _minMealHour, _maxMealHour);
                if (_currentUser != null)
                {
                    var blockedIds = _blockedService.GetByUserId(_currentUser.Id)
                        .Select(b => b.RestaurantId)
                        .ToHashSet();
                    mealTimeFiltered = mealTimeFiltered.Where(r => !blockedIds.Contains(r.Id)).ToList();
                }

                List<Restaurant> filteredRestaurants;

                // 隨機種類：代表該時段全部種類（不做 foodType 縮限）
                if (_isRandomCategory)
                {
                    filteredRestaurants = mealTimeFiltered;
                }
                else
                {
                    // 指定種類（支援多標籤 OR 條件）
                    var selectedSet = new HashSet<string>(_selectedFoodTypes, StringComparer.OrdinalIgnoreCase);
                    if (selectedSet.Count == 0 && !string.IsNullOrWhiteSpace(_foodType))
                    {
                        selectedSet.Add(_foodType);
                    }

                    filteredRestaurants = mealTimeFiltered
                        .Where(r => r.FoodTypes != null && r.FoodTypes.Any(ft => selectedSet.Contains(ft)))
                        .ToList();
                }

                _totalFilteredCount = filteredRestaurants.Count;
                UpdateFilterSummaryTitle();

                if (filteredRestaurants.Count < 2)
                {
                    MessageBox.Show(
                        LanguageManager.GetTranslation("insufficientRestaurants", filteredRestaurants.Count),
                        LanguageManager.GetTranslation("hintTitle"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                _swipeMatchService.Initialize(filteredRestaurants);
                DisplayRestaurants();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    LanguageManager.CurrentLanguage == LanguageType.Chinese ? $"初始化失敗: {ex.Message}" : $"Initialization failed: {ex.Message}",
                    LanguageManager.GetTranslation("resetFailedTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        /// <summary>
        /// 在 UI 上顯示左右兩家餐廳的完整資訊（含歷史統計）
        /// </summary>
        private void DisplayRestaurants()
        {
            // 顯示左邊餐廳
            if (_swipeMatchService.LeftRestaurant != null)
            {
                var left = _swipeMatchService.LeftRestaurant;
                var (leftVisitCount, leftAvgRating) = _preferenceService.GetRestaurantStats(left.Id);

                lblLeftName.Text = left.Name;
                lblLeftPhone.Text = LanguageManager.GetTranslation("phoneLabel", left.Phone);
                lblLeftHours.Text = LanguageManager.GetTranslation("hoursLabel", left.BusinessHours);
                lblLeftFeature.Text = LanguageManager.GetTranslation("featureLabel", LanguageManager.GetLocalizedFeature(left.Feature));
                lblLeftFoodType.Text = LanguageManager.GetTranslation("foodTypeLabel", string.Join(", ", left.FoodTypes.Select(LanguageManager.GetLocalizedTag)));

                // 添加歷史統計
                if (leftVisitCount > 0)
                {
                    if (leftAvgRating > 0)
                    {
                        lblLeftName.Text += LanguageManager.GetTranslation("timesVisited", leftVisitCount, leftAvgRating);
                    }
                    else
                    {
                        lblLeftName.Text += LanguageManager.GetTranslation("timesVisitedNoRating", leftVisitCount);
                    }
                }

                SetRestaurantImage(picLeftImage, left);
            }

            // 顯示右邊餐廳
            if (_swipeMatchService.RightRestaurant != null)
            {
                var right = _swipeMatchService.RightRestaurant;
                var (rightVisitCount, rightAvgRating) = _preferenceService.GetRestaurantStats(right.Id);

                lblRightName.Text = right.Name;
                lblRightPhone.Text = LanguageManager.GetTranslation("phoneLabel", right.Phone);
                lblRightHours.Text = LanguageManager.GetTranslation("hoursLabel", right.BusinessHours);
                lblRightFeature.Text = LanguageManager.GetTranslation("featureLabel", LanguageManager.GetLocalizedFeature(right.Feature));
                lblRightFoodType.Text = LanguageManager.GetTranslation("foodTypeLabel", string.Join(", ", right.FoodTypes.Select(LanguageManager.GetLocalizedTag)));

                // 添加歷史統計
                if (rightVisitCount > 0)
                {
                    if (rightAvgRating > 0)
                    {
                        lblRightName.Text += LanguageManager.GetTranslation("timesVisited", rightVisitCount, rightAvgRating);
                    }
                    else
                    {
                        lblRightName.Text += LanguageManager.GetTranslation("timesVisitedNoRating", rightVisitCount);
                    }
                }

                SetRestaurantImage(picRightImage, right);
            }

            UpdateProgressLabel();
        }

        private void UpdateProgressLabel()
        {
            int remaining = _swipeMatchService.GetRemainingCandidateCount() + 1;
            lblProgress.Text = LanguageManager.GetTranslation("swipeProgress", remaining, _totalFilteredCount);
        }

        private void btnSelectLeft_Click(object sender, EventArgs e)
        {
            bool canContinue = _swipeMatchService.SelectLeft();

            if (canContinue)
            {
                DisplayRestaurants();
            }
            else
            {
                ShowFinalResult();
            }
        }

        private void btnSelectRight_Click(object sender, EventArgs e)
        {
            bool canContinue = _swipeMatchService.SelectRight();

            if (canContinue)
            {
                DisplayRestaurants();
            }
            else
            {
                ShowFinalResult();
            }
        }

        private void ShowFinalResult()
        {
            var finalRestaurant = _swipeMatchService.GetFinalResult();

            if (finalRestaurant == null)
            {
                MessageBox.Show(
                    LanguageManager.CurrentLanguage == LanguageType.Chinese ? "無法取得推薦結果，請重新開始。" : "Failed to obtain recommendation. Please restart.",
                    LanguageManager.GetTranslation("resetFailedTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            // 將結果視為序列填入今日六格（左到右，上到下）
            if (_currentUser == null)
            {
                MessageBox.Show(
                    LanguageManager.CurrentLanguage == LanguageType.Chinese ? "請先選擇使用者" : "Please select a user first",
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using var resultForm = new ResultForm(finalRestaurant, "sequential", _currentUser, _favoriteService, _blockedService);
            if (resultForm.ShowDialog() == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}

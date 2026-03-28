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

        // 服務層
        private IRestaurantRepository _restaurantRepository;
        private RestaurantFilterService _filterService;
        private SwipeMatchService _swipeMatchService;
        private UserPreferenceService _preferenceService;

        private int _totalFilteredCount;

        // 舊版相容建構子
        public SwipeForm(string mealTime, string foodType, bool isRandomCategory)
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
                isRandomCategory)
        {
        }

        public SwipeForm(int minMealHour, int maxMealHour, List<string> selectedFoodTypes, bool isRandomCategory)
        {
            InitializeComponent();
            _minMealHour = Math.Clamp(minMealHour, 0, 23);
            _maxMealHour = Math.Clamp(maxMealHour, 0, 23);
            _isRandomCategory = isRandomCategory;
            _selectedFoodTypes = selectedFoodTypes ?? new List<string>();
            _foodType = _selectedFoodTypes.FirstOrDefault();

            this.Text = "選擇餐廳";
            this.StartPosition = FormStartPosition.CenterScreen;

            // 初始化服務
            string csvPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "restaurants.csv"
            );
            _restaurantRepository = new CsvRestaurantRepository(csvPath);
            _filterService = new RestaurantFilterService();
            _swipeMatchService = new SwipeMatchService();

            // 初始化使用者偏好服務
            string preferencePath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "user_preferences.json"
            );
            _preferenceService = new UserPreferenceService(preferencePath);
            _preferenceService.LoadPreferences();
        }

        private void SwipeForm_Load(object sender, EventArgs e)
        {
            InitializeSwipeCards();
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
            if (_isRandomCategory)
            {
                lblTitle.Text = $"{_minMealHour:00}:00~{_maxMealHour:00}:00｜隨機種類（←/→ 可快速選擇）";
                return;
            }

            if (_selectedFoodTypes.Count > 0)
            {
                lblTitle.Text = $"{_minMealHour:00}:00~{_maxMealHour:00}:00｜{string.Join("、", _selectedFoodTypes)}（←/→ 可快速選擇）";
            }
            else
            {
                lblTitle.Text = $"{_minMealHour:00}:00~{_maxMealHour:00}:00｜指定種類（←/→ 可快速選擇）";
            }
        }

        private Image CreatePlaceholderImage(Size size)
        {
            int width = Math.Max(size.Width, 40);
            int height = Math.Max(size.Height, 40);
            var bitmap = new Bitmap(width, height);
            using var g = Graphics.FromImage(bitmap);
            g.Clear(Color.Gainsboro);
            using var brush = new SolidBrush(Color.DimGray);
            using var font = new Font("微軟正黑體", 10F, FontStyle.Regular);
            var rect = new RectangleF(0, 0, width, height);
            var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString("暫無圖片", font, brush, rect, format);
            return bitmap;
        }

        private Image? LoadRestaurantImage(string? imageFileName)
        {
            if (string.IsNullOrWhiteSpace(imageFileName))
                return null;

            string imagePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Assets",
                "images",
                imageFileName
            );

            if (!File.Exists(imagePath))
                return null;

            using var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var temp = Image.FromStream(fs);
            return new Bitmap(temp);
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
                var userPreference = _preferenceService.GetCurrentPreference();

                // 先依時段 + 封鎖篩選
                var mealTimeFiltered = _filterService.FilterByMealTimeRange(allRestaurants, _minMealHour, _maxMealHour);
                mealTimeFiltered = _filterService.ExcludeBlocked(mealTimeFiltered, userPreference);

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
                        $"符合條件的餐廳不足 2 家。找到 {filteredRestaurants.Count} 家餐廳，無法進行選擇。",
                        "提示",
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
                    $"初始化失敗: {ex.Message}",
                    "錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        /// <summary>
        /// 在 UI 上顯示左右兩家餐廳的完整資訊
        /// </summary>
        private void DisplayRestaurants()
        {
            // 顯示左邊餐廳
            if (_swipeMatchService.LeftRestaurant != null)
            {
                var left = _swipeMatchService.LeftRestaurant;
                lblLeftName.Text = left.Name;
                lblLeftPhone.Text = $"電話: {left.Phone}";
                lblLeftHours.Text = $"營業: {left.BusinessHours}";
                lblLeftFeature.Text = $"特色: {left.Feature}";
                lblLeftFoodType.Text = $"種類: {string.Join(", ", left.FoodTypes)}";
                SetRestaurantImage(picLeftImage, left);
            }

            // 顯示右邊餐廳
            if (_swipeMatchService.RightRestaurant != null)
            {
                var right = _swipeMatchService.RightRestaurant;
                lblRightName.Text = right.Name;
                lblRightPhone.Text = $"電話: {right.Phone}";
                lblRightHours.Text = $"營業: {right.BusinessHours}";
                lblRightFeature.Text = $"特色: {right.Feature}";
                lblRightFoodType.Text = $"種類: {string.Join(", ", right.FoodTypes)}";
                SetRestaurantImage(picRightImage, right);
            }

            UpdateProgressLabel();
        }

        private void UpdateProgressLabel()
        {
            int remaining = _swipeMatchService.GetRemainingCandidateCount() + 1;
            lblProgress.Text = $"剩餘 {remaining} / {_totalFilteredCount} 家餐廳";
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
                MessageBox.Show("無法取得推薦結果，請重新開始。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            using var resultForm = new ResultForm(finalRestaurant);
            if (resultForm.ShowDialog() == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}

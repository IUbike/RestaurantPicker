using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using RestaurantPicker.Models;
using RestaurantPicker.Services;
using RestaurantPicker.Repositories;

namespace RestaurantPicker.Views
{
    public partial class ResultForm : Form
    {
        // 最終推薦的餐廳
        private readonly Restaurant _recommendedRestaurant;

        // 用餐時段模式："breakfast"/"lunch"/"dinner" 或 "sequential"
        private readonly string _mealTimeMode;

        // 使用者偏好服務
        private readonly UserPreferenceService _preferenceService;

        // 今日餐廳服務（用於 sequential 模式）
        private readonly TodayMealService _todayMealService;

        // 已自動儲存標記，避免重複儲存
        private bool _autoSaved = false;

        // 是否已經收藏
        private bool _isFavorited = false;

        public ResultForm(Restaurant recommendedRestaurant, string mealTimeMode = "lunch")
        {
            InitializeComponent();
            _recommendedRestaurant = recommendedRestaurant;
            _mealTimeMode = mealTimeMode;
            this.Text = "推薦結果";
            this.StartPosition = FormStartPosition.CenterScreen;

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

            // 為了支援 sequential 行為，初始化 TodayMealService
            string csvPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "restaurants.csv"
            );
            var restaurantRepository = new LiteDbRestaurantRepository(databasePath, csvPath);
            _todayMealService = new TodayMealService(_preferenceService, restaurantRepository);
        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            DisplayResult();

            // 如果是 sequential 模式，開啟時自動儲存到下一個可用槽位
            if (string.Equals(_mealTimeMode, "sequential", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    _preferenceService.LoadPreferences();
                    var saved = _todayMealService.AddMealToNextAvailableSlot(_recommendedRestaurant.Id, 0, null);
                    _autoSaved = true;
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] ResultForm auto-saved restaurant Id={_recommendedRestaurant.Id} as MealRecord Id={saved?.Id}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"自動儲存今日餐廳失敗: {ex.Message}");
                }
            }

            // 自動彈出評分對話框
            System.Windows.Forms.Application.DoEvents();  // 讓 UI 先顯示
            this.BeginInvoke(new Action(() =>
            {
                ShowRatingDialog();
            }));
        }

        private Image CreatePlaceholderImage(Size size)
        {
            int width = Math.Max(size.Width, 40);
            int height = Math.Max(size.Height, 40);
            var bitmap = new Bitmap(width, height);
            using var g = Graphics.FromImage(bitmap);
            g.Clear(Color.Gainsboro);
            using var brush = new SolidBrush(Color.DimGray);
            using var font = new Font("微軟正 黑體", 10F, FontStyle.Regular);
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

        private void ApplyLanguage()
        {
            this.Text = LanguageManager.GetTranslation("resultTitle");
            lblTitle.Text = LanguageManager.GetTranslation("resultTitle");
            lblTitleText.Text = LanguageManager.GetTranslation("resultSub");

            // Apply full-button images dynamically (resolves _e versions automatically!)
            LanguageManager.ApplyFullButtonImage(btnConfirm, "icons_ok.png");
            LanguageManager.ApplyFullButtonImage(btnFavorite, "icons_like.png");
            LanguageManager.ApplyFullButtonImage(btnShare, "icons_share.png");
            LanguageManager.ApplyFullButtonImage(btnDontShow, "icons_block.png");
        }

        private void DisplayResult()
        {
            if (_recommendedRestaurant == null)
            {
                MessageBox.Show(
                    LanguageManager.CurrentLanguage == LanguageType.Chinese ? "無效的推薦結果" : "Invalid recommendation result",
                    LanguageManager.GetTranslation("resetFailedTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblRestaurantName.Text = _recommendedRestaurant.Name;
            lblRestaurantPhone.Text = LanguageManager.GetTranslation("phoneLabel", _recommendedRestaurant.Phone);
            lblRestaurantHours.Text = LanguageManager.CurrentLanguage == LanguageType.Chinese 
                ? $"營業時間: {_recommendedRestaurant.BusinessHours}" 
                : $"Hours: {_recommendedRestaurant.BusinessHours}";
            lblRestaurantAddress.Text = LanguageManager.CurrentLanguage == LanguageType.Chinese 
                ? $"地址: {_recommendedRestaurant.Address}" 
                : $"Address: {_recommendedRestaurant.Address}";
            lblRestaurantFeature.Text = LanguageManager.GetTranslation("featureLabel", _recommendedRestaurant.Feature);
            lblRestaurantFoodType.Text = LanguageManager.CurrentLanguage == LanguageType.Chinese 
                ? $"食物種類: {string.Join(", ", _recommendedRestaurant.FoodTypes)}" 
                : $"Food Style: {string.Join(", ", _recommendedRestaurant.FoodTypes)}";
            lblRestaurantPrice.Text = LanguageManager.CurrentLanguage == LanguageType.Chinese 
                ? $"價位: {_recommendedRestaurant.PriceRange}" 
                : $"Price: {_recommendedRestaurant.PriceRange}";

            picRestaurantImage.Image?.Dispose();
            picRestaurantImage.Image = LoadRestaurantImage(_recommendedRestaurant.ImageFileName)
                ?? CreatePlaceholderImage(picRestaurantImage.Size);

            _isFavorited = _preferenceService.IsFavorite(_recommendedRestaurant.Id);
            UpdateFavoriteButtonAppearance();
        }

        private void UpdateFavoriteButtonAppearance()
        {
            if (_isFavorited)
            {
                LanguageManager.ApplyFullButtonImage(btnFavorite, "icons_collection.png");
            }
            else
            {
                LanguageManager.ApplyFullButtonImage(btnFavorite, "icons_like.png");
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // 如果為 sequential 模式，且尚未自動儲存，將餐廳加入下一個可用槽位（左到右、上到下）
            if (string.Equals(_mealTimeMode, "sequential", StringComparison.OrdinalIgnoreCase))
            {
                if (!_autoSaved)
                {
                    try
                    {
                        _preferenceService.LoadPreferences();
                        _todayMealService.AddMealToNextAvailableSlot(_recommendedRestaurant.Id, 0, null);
                        _autoSaved = true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"儲存今日餐廳失敗: {ex.Message}");
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }

            // 1. 先刪除今天同一個時段的舊紀錄 (避免重複綁定同一個時段)
            var todayRecords = _preferenceService.GetMealRecordsForToday();
            var existingRecord = todayRecords.FirstOrDefault(m => m.MealTime == _mealTimeMode);
            if (existingRecord != null)
            {
                _preferenceService.DeleteMealRecord(existingRecord.Id);
            }

            // 2. 建立新的用餐紀錄 (預設 0 分，代表未評分)
            var mealRecord = new MealRecord
            {
                RestaurantId = _recommendedRestaurant.Id,
                MealDate = DateTime.Now,
                MealTime = _mealTimeMode,
                Rating = 0,
                HasRating = false,
                CreatedAt = DateTime.Now
            };

            // 3. 確實存檔
            _preferenceService.AddMealRecord(mealRecord);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnFavorite_Click(object sender, EventArgs e)
        {
            if (_isFavorited)
            {
                _preferenceService.RemoveFavorite(_recommendedRestaurant.Id);
                _isFavorited = false;
                MessageBox.Show(
                    LanguageManager.GetTranslation("favoriteRemoved"),
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                _preferenceService.AddFavorite(_recommendedRestaurant.Id);
                _isFavorited = true;
                MessageBox.Show(
                    LanguageManager.GetTranslation("favoriteAdded"),
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            UpdateFavoriteButtonAppearance();
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            string shareText;
            if (LanguageManager.CurrentLanguage == LanguageType.English)
            {
                shareText = $"{_recommendedRestaurant.Name}\n" +
                            $"Phone: {_recommendedRestaurant.Phone}\n" +
                            $"Address: {_recommendedRestaurant.Address}\n" +
                            $"Hours: {_recommendedRestaurant.BusinessHours}\n" +
                            $"Feature: {_recommendedRestaurant.Feature}";
            }
            else
            {
                shareText = $"{_recommendedRestaurant.Name}\n" +
                            $"電話: {_recommendedRestaurant.Phone}\n" +
                            $"地址: {_recommendedRestaurant.Address}\n" +
                            $"營業時間: {_recommendedRestaurant.BusinessHours}\n" +
                            $"特色: {_recommendedRestaurant.Feature}";
            }

            try
            {
                Clipboard.SetText(shareText);
                MessageBox.Show(
                    LanguageManager.GetTranslation("copiedToClipboard"),
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    LanguageManager.GetTranslation("copyFailed") + ex.Message,
                    LanguageManager.GetTranslation("resetFailedTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDontShow_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                LanguageManager.GetTranslation("blockConfirm", _recommendedRestaurant.Name),
                LanguageManager.GetTranslation("confirmTitle"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                _preferenceService.AddBlocked(_recommendedRestaurant.Id);
                MessageBox.Show(
                    LanguageManager.GetTranslation("blockDone"),
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// 對當前推薦的餐廳進行評分
        /// </summary>
        public void ShowRatingDialog()
        {
            // 如果你希望在 Result 畫面還是可以評分，可以直接更新剛存好的紀錄
            using var ratingForm = new RatingForm(_recommendedRestaurant);
            if (ratingForm.ShowDialog() == DialogResult.OK)
            {
                int rating = ratingForm.SelectedRating;

                // 嘗試找到今天已儲存的此餐廳紀錄，優先以 RestaurantId 比對
                try
                {
                    _preferenceService.LoadPreferences();
                    var todayRecords = _preferenceService.GetMealRecordsForToday();
                    var record = todayRecords.FirstOrDefault(r => r.RestaurantId == _recommendedRestaurant.Id);

                    if (record != null)
                    {
                        _preferenceService.UpdateMealRating(record.Id, rating);
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] ResultForm updated rating for MealRecord Id={record.Id} Rating={rating}");
                        MessageBox.Show(
                            LanguageManager.GetTranslation("ratingRecorded", rating),
                            LanguageManager.GetTranslation("hintTitle"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        // 如果找不到今天的紀錄，建立一筆新的（適用於非 sequential 或尚未儲存的情況）
                        var newRecord = new MealRecord
                        {
                            RestaurantId = _recommendedRestaurant.Id,
                            MealDate = DateTime.Now,
                            MealTime = _mealTimeMode == "sequential" ? "custom" : _mealTimeMode,
                            Rating = rating,
                            HasRating = true,
                            CreatedAt = DateTime.Now
                        };

                        _preferenceService.AddMealRecord(newRecord);
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] ResultForm created new MealRecord Id={newRecord.Id} with Rating={rating}");
                        MessageBox.Show(
                            LanguageManager.GetTranslation("ratingRecorded", rating),
                            LanguageManager.GetTranslation("hintTitle"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"更新評分失敗: {ex.Message}");
                    MessageBox.Show(
                        (LanguageManager.CurrentLanguage == LanguageType.Chinese ? "記錄評分失敗: " : "Failed to record rating: ") + ex.Message,
                        LanguageManager.GetTranslation("resetFailedTitle"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                string ratingDisplay = rating >= 0 ? $"{rating} 顆星" : "未評分";
                // 移除原本的提示文字
                // MessageBox.Show($"已記錄評分意願：{ratingDisplay}\n(請至首頁點擊該餐廳完成最終評分)", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

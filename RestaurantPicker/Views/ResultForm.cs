using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using RestaurantPicker.Models;
using RestaurantPicker.Services;

namespace RestaurantPicker.Views
{
    public partial class ResultForm : Form
    {
        // 最終推薦的餐廳
        private readonly Restaurant _recommendedRestaurant;

        // 使用者偏好服務
        private readonly UserPreferenceService _preferenceService;

        // 是否已經收藏
        private bool _isFavorited = false;

        public ResultForm(Restaurant recommendedRestaurant)
        {
            InitializeComponent();
            _recommendedRestaurant = recommendedRestaurant;
            this.Text = "推薦結果";
            this.StartPosition = FormStartPosition.CenterScreen;

            string preferencePath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "user_preferences.json"
            );
            _preferenceService = new UserPreferenceService(preferencePath);
            _preferenceService.LoadPreferences();
        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            DisplayResult();
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

        private void DisplayResult()
        {
            if (_recommendedRestaurant == null)
            {
                MessageBox.Show("無效的推薦結果", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblRestaurantName.Text = _recommendedRestaurant.Name;
            lblRestaurantPhone.Text = $"電話: {_recommendedRestaurant.Phone}";
            lblRestaurantHours.Text = $"營業時間: {_recommendedRestaurant.BusinessHours}";
            lblRestaurantAddress.Text = $"地址: {_recommendedRestaurant.Address}";
            lblRestaurantFeature.Text = $"特色: {_recommendedRestaurant.Feature}";
            lblRestaurantFoodType.Text = $"食物種類: {string.Join(", ", _recommendedRestaurant.FoodTypes)}";
            lblRestaurantPrice.Text = $"價位: {_recommendedRestaurant.PriceRange}";

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
                btnFavorite.Text = "♥ 已收藏";
                btnFavorite.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                btnFavorite.Text = "♡ 收藏";
                btnFavorite.BackColor = System.Drawing.Color.Orange;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnFavorite_Click(object sender, EventArgs e)
        {
            if (_isFavorited)
            {
                _preferenceService.RemoveFavorite(_recommendedRestaurant.Id);
                _isFavorited = false;
                MessageBox.Show("已取消收藏", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _preferenceService.AddFavorite(_recommendedRestaurant.Id);
                _isFavorited = true;
                MessageBox.Show("已收藏此餐廳", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            UpdateFavoriteButtonAppearance();
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            string shareText = $"{_recommendedRestaurant.Name}\n" +
                             $"電話: {_recommendedRestaurant.Phone}\n" +
                             $"地址: {_recommendedRestaurant.Address}\n" +
                             $"營業時間: {_recommendedRestaurant.BusinessHours}\n" +
                             $"特色: {_recommendedRestaurant.Feature}";

            try
            {
                Clipboard.SetText(shareText);
                MessageBox.Show("已複製到剪貼板", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"複製失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDontShow_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                $"確定要封鎖「{_recommendedRestaurant.Name}」嗎？\n" +
                "未來篩選時將不會再看到此餐廳。",
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                _preferenceService.AddBlocked(_recommendedRestaurant.Id);
                MessageBox.Show("已封鎖此餐廳", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}

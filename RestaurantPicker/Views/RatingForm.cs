using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using RestaurantPicker.Models;
using RestaurantPicker.Services;

namespace RestaurantPicker.Views
{
    public partial class RatingForm : Form
    {
        private readonly Restaurant _restaurant;
        private int _selectedRating = 0;
        private int _hoverRating = 0;
        private Button[] _starButtons;
        private Label _ratingLabel;
        private Image? _starGrayImage;
        private Image? _starYellowImage;

        public int SelectedRating { get; private set; }

        public RatingForm(Restaurant restaurant)
        {
            _restaurant = restaurant;
            this.Text = LanguageManager.GetTranslation("ratingTitleText");
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 380);  // 增大視窗尺寸以容納更大的星星
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            SelectedRating = 0;

            InitializeUI();
            this.Load += RatingForm_Load;
            this.FormClosed += RatingForm_FormClosed;
        }

        private void InitializeUI()
        {
            // 清空控件
            this.Controls.Clear();

            // 餐廳名稱標籤
            string ratingPrefix = LanguageManager.GetTranslation("ratingTitleText");
            var nameLabel = new Label
            {
                Text = $"{ratingPrefix}{_restaurant.Name}",
                Font = new Font("微軟正黑體", 12F, FontStyle.Bold),
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            this.Controls.Add(nameLabel);

            // 星星按鈕面板
            var starPanel = new Panel
            {
                Left = 30,
                Top = 70,
                Width = 420,
                Height = 90,
                BackColor = this.BackColor
            };

            int starSize = 70;
            _starGrayImage = CreateStarImage("icons_star_gray.png", starSize, starSize);
            _starYellowImage = CreateStarImage("icons_star_yellow.png", starSize, starSize);

            _starButtons = new Button[5];
            for (int i = 0; i < 5; i++)
            {
                int starValue = i + 1;
                var button = new Button
                {
                    Text = "",
                    Width = starSize,
                    Height = starSize,
                    Left = i * (starSize + 10),
                    Top = 0,
                    BackColor = Color.Transparent,
                    FlatStyle = FlatStyle.Flat,
                    TabIndex = i,
                    UseVisualStyleBackColor = false
                };

                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = Color.Silver;
                button.FlatAppearance.MouseDownBackColor = Color.Transparent;
                button.FlatAppearance.MouseOverBackColor = Color.Transparent;
                button.BackgroundImageLayout = ImageLayout.Center;
                button.BackgroundImage = _starGrayImage;

                int capturedValue = starValue;
                button.Click += (s, e) =>
                {
                    SetRating(capturedValue);
                    UpdateStarDisplay();
                };

                button.MouseEnter += (s, e) =>
                {
                    _hoverRating = capturedValue;
                    UpdateStarDisplay();
                };

                button.MouseLeave += (s, e) =>
                {
                    _hoverRating = 0;
                    UpdateStarDisplay();
                };

                _starButtons[i] = button;
                starPanel.Controls.Add(button);
            }

            this.Controls.Add(starPanel);

            // 評分文本
            _ratingLabel = new Label
            {
                Text = LanguageManager.GetTranslation("unselected"),
                Font = new Font("微軟正黑體", 11F),
                Left = 30,
                Top = 175,
                Width = 400,
                AutoSize = true
            };
            this.Controls.Add(_ratingLabel);

            // 確認按鈕
            var okButton = new Button
            {
                Text = "確認",
                DialogResult = DialogResult.OK,
                Width = 100,
                Height = 50,
                Left = 110,
                Top = 270
            };

            okButton.Click += (s, e) =>
            {
                SelectedRating = _selectedRating;
            };

            this.Controls.Add(okButton);
            LanguageManager.ApplyFullButtonImage(okButton, "icons_ok.png");

            // 取消按鈕
            var cancelButton = new Button
            {
                Text = "取消",
                DialogResult = DialogResult.Cancel,
                Width = 100,
                Height = 50,
                Left = 290,
                Top = 270
            };

            this.Controls.Add(cancelButton);
            LanguageManager.ApplyFullButtonImage(cancelButton, "icons_cancel.png");
        }

        private void RatingForm_Load(object sender, EventArgs e)
        {
            // 表單載入時的初始化
        }

        private void RatingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _starGrayImage?.Dispose();
            _starYellowImage?.Dispose();
        }

        private void SetRating(int rating)
        {
            _selectedRating = rating;
        }

        private void InitializeComponent()
        {

        }

        private void UpdateStarDisplay()
        {
            int activeRating = _hoverRating > 0 ? _hoverRating : _selectedRating;
            for (int i = 0; i < 5; i++)
            {
                _starButtons[i].BackgroundImage = (i < activeRating) ? _starYellowImage : _starGrayImage;
            }

            if (_ratingLabel != null)
            {
                _ratingLabel.Text = _selectedRating > 0 
                    ? $"{_selectedRating} {LanguageManager.GetTranslation("starText")}" 
                    : LanguageManager.GetTranslation("unselected");
            }
        }

        private Image? CreateStarImage(string iconName, int width, int height)
        {
            var icon = LanguageManager.LoadIcon(iconName);
            if (icon == null)
            {
                return null;
            }

            var cropped = LanguageManager.CropTransparentBordersRect(icon);
            var resized = LanguageManager.ResizeImageKeepAspect(cropped, width, height);
            icon.Dispose();
            cropped.Dispose();
            return resized;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using RestaurantPicker.Models;

namespace RestaurantPicker.Views
{
    public partial class RatingForm : Form
    {
        private readonly Restaurant _restaurant;
        private int _selectedRating = 0;
        private Button[] _starButtons;
        private Label _ratingLabel;

        public int SelectedRating { get; private set; }

        public RatingForm(Restaurant restaurant)
        {
            _restaurant = restaurant;
            this.Text = "評分";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 380);  // 增大視窗尺寸以容納更大的星星
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            SelectedRating = 0;

            InitializeUI();
            this.Load += RatingForm_Load;
        }

        private void InitializeUI()
        {
            // 清空控件
            this.Controls.Clear();

            // 餐廳名稱標籤
            var nameLabel = new Label
            {
                Text = $"評分：{_restaurant.Name}",
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

            _starButtons = new Button[5];
            for (int i = 0; i < 5; i++)
            {
                int starValue = i + 1;
                var button = new Button
                {
                    Text = "★",
                    Font = new Font("Arial", 36F),
                    Width = 70,
                    Height = 70,
                    Left = i * 80,
                    Top = 0,
                    BackColor = SystemColors.Control,
                    ForeColor = Color.Gray,
                    FlatStyle = FlatStyle.Flat,
                    TabIndex = i
                };

                int capturedValue = starValue;
                button.Click += (s, e) =>
                {
                    SetRating(capturedValue);
                    UpdateStarDisplay();
                };

                button.MouseEnter += (s, e) =>
                {
                    button.ForeColor = Color.Gold;
                };

                button.MouseLeave += (s, e) =>
                {
                    button.ForeColor = (_selectedRating >= capturedValue) ? Color.Gold : Color.Gray;
                };

                _starButtons[i] = button;
                starPanel.Controls.Add(button);
            }

            this.Controls.Add(starPanel);

            // 評分文本
            _ratingLabel = new Label
            {
                Text = "未選擇",
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
        }

        private void RatingForm_Load(object sender, EventArgs e)
        {
            // 表單載入時的初始化
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
            for (int i = 0; i < 5; i++)
            {
                _starButtons[i].ForeColor = (i < _selectedRating) ? Color.Gold : Color.Gray;
            }

            if (_ratingLabel != null)
            {
                _ratingLabel.Text = _selectedRating > 0 ? $"{_selectedRating} 顆星" : "未選擇";
            }
        }
    }
}

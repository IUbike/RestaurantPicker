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
    public partial class SelectRestaurantForm : Form
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly UserPreferenceService _preferenceService;
        private List<Restaurant> _allRestaurants;
        private ListBox _listBox;

        // 用餐時段類型：breakfast/lunch/dinner
        private readonly string _mealTimeType;

        public int SelectedRestaurantId { get; private set; }

        public SelectRestaurantForm(IRestaurantRepository restaurantRepository, UserPreferenceService preferenceService, string mealTimeType = "lunch")
        {
            this.Text = "選擇餐廳";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(600, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            _restaurantRepository = restaurantRepository;
            _preferenceService = preferenceService;
            _mealTimeType = mealTimeType;
            SelectedRestaurantId = 0;

            InitializeUI();
            this.Load += SelectRestaurantForm_Load;
        }

        private void InitializeUI()
        {
            // ListBox
            _listBox = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("微軟正黑體", 11F),
                Margin = new Padding(0, 0, 0, 50)
            };

            _listBox.DoubleClick += (s, e) =>
            {
                if (_listBox.SelectedValue != null)
                {
                    SelectedRestaurantId = (int)_listBox.SelectedValue;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };

            // 按鈕面板
            var buttonPanel = new Panel
            {
                Height = 50,
                Dock = DockStyle.Bottom,
                BackColor = SystemColors.Control
            };

            var okButton = new Button
            {
                Text = "確認",
                DialogResult = DialogResult.OK,
                Width = 80,
                Height = 40,
                Left = 420,
                Top = 5
            };

            okButton.Click += (s, e) =>
            {
                if (_listBox.SelectedValue != null)
                {
                    SelectedRestaurantId = (int)_listBox.SelectedValue;
                }
            };

            var cancelButton = new Button
            {
                Text = "取消",
                DialogResult = DialogResult.Cancel,
                Width = 80,
                Height = 40,
                Left = 510,
                Top = 5
            };

            buttonPanel.Controls.Add(okButton);
            buttonPanel.Controls.Add(cancelButton);

            this.Controls.Add(_listBox);
            this.Controls.Add(buttonPanel);
        }

        private void SelectRestaurantForm_Load(object sender, EventArgs e)
        {
            LoadRestaurants();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // SelectRestaurantForm
            // 
            ClientSize = new Size(838, 601);
            Name = "SelectRestaurantForm";
            ResumeLayout(false);

        }

        private void LoadRestaurants()
        {
            try
            {
                _allRestaurants = _restaurantRepository.LoadAll();

                // 根據時段過濾餐廳（確保餐廳符合該時段的 CSV boolean 值）
                var filteredRestaurants = _allRestaurants.Where(r =>
                {
                    return _mealTimeType switch
                    {
                        "breakfast" => r.IsBreakfastAvailable,
                        "lunch" => r.IsLunchAvailable,
                        "dinner" => r.IsDinnerAvailable,
                        _ => true  // custom 時段允許所有餐廳
                    };
                }).ToList();

                var dataSource = filteredRestaurants.Select(r =>
                {
                    var (visitCount, avgRating) = _preferenceService.GetRestaurantStats(r.Id);
                    var ratingText = visitCount > 0 && avgRating > 0
                        ? $" ⭐ {avgRating:F1}"
                        : "";
                    var visitText = visitCount > 0 ? $" (去過 {visitCount} 次)" : "";

                    return new
                    {
                        RestaurantId = r.Id,
                        RestaurantName = r.Name,
                        DisplayText = $"{r.Name}{visitText}{ratingText}",
                        VisitCount = visitCount,
                        AverageRating = avgRating
                    };
                }).ToList();

                _listBox.DataSource = dataSource;
                _listBox.DisplayMember = "DisplayText";
                _listBox.ValueMember = "RestaurantId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"載入餐廳失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

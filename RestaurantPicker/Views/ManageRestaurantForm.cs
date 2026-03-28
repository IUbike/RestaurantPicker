using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RestaurantPicker.Models;
using RestaurantPicker.Repositories;

namespace RestaurantPicker.Views
{
    public partial class ManageRestaurantForm : Form
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public ManageRestaurantForm(IRestaurantRepository restaurantRepository)
        {
            InitializeComponent();
            _restaurantRepository = restaurantRepository;
            Text = "管理餐廳（新增）";
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("請輸入餐廳名稱", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                var foodTypes = ParseMultiValueText(txtFoodTypes.Text);
                if (foodTypes.Count == 0)
                {
                    MessageBox.Show("請至少輸入一個食物種類", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFoodTypes.Focus();
                    return;
                }

                var newRestaurant = new Restaurant
                {
                    Name = txtName.Text.Trim(),
                    PriceRange = txtPriceRange.Text.Trim(),
                    FoodTypes = foodTypes,
                    CuisineStyle = txtCuisineStyle.Text.Trim(),
                    Purposes = ParseMultiValueText(txtPurposes.Text),
                    Feature = txtFeature.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    BusinessHours = txtBusinessHours.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    IsBreakfastAvailable = chkBreakfast.Checked,
                    IsLunchAvailable = chkLunch.Checked,
                    IsDinnerAvailable = chkDinner.Checked,
                    ImageFileName = txtImageFileName.Text.Trim(),
                    IsFavorite = false,
                    IsBlocked = false
                };

                // 先載入現有資料，確保 Add 與 SaveAll 使用同一份列表
                var restaurants = _restaurantRepository.LoadAll();
                _restaurantRepository.Add(newRestaurant);
                _restaurantRepository.SaveAll(restaurants);

                MessageBox.Show("餐廳新增成功", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新增失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private static List<string> ParseMultiValueText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return new List<string>();
            }

            return text
                .Split(new[] { ';', ',', '，' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}

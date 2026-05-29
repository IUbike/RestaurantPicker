using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RestaurantPicker.Models;
using RestaurantPicker.Repositories;
using RestaurantPicker.Services;

namespace RestaurantPicker.Views
{
    public partial class ManageRestaurantForm : Form
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public ManageRestaurantForm(IRestaurantRepository restaurantRepository)
        {
            InitializeComponent();
            _restaurantRepository = restaurantRepository;
            this.StartPosition = FormStartPosition.CenterScreen;
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            this.Text = LanguageManager.GetTranslation("manageTitle");
            lblTitle.Text = LanguageManager.GetTranslation("manageTitle");
            lblName.Text = LanguageManager.GetTranslation("lblName");
            lblPriceRange.Text = LanguageManager.GetTranslation("lblPriceRange");
            lblFoodTypes.Text = LanguageManager.GetTranslation("lblFoodTypes");
            lblCuisineStyle.Text = LanguageManager.GetTranslation("lblCuisineStyle");
            lblPurposes.Text = LanguageManager.GetTranslation("lblPurposes");
            lblFeature.Text = LanguageManager.GetTranslation("lblFeature");
            lblPhone.Text = LanguageManager.GetTranslation("lblPhone");
            lblBusinessHours.Text = LanguageManager.GetTranslation("lblBusinessHours");
            lblAddress.Text = LanguageManager.GetTranslation("lblAddress");
            lblImageFileName.Text = LanguageManager.GetTranslation("lblImageFileName");
            lblMealTime.Text = LanguageManager.GetTranslation("lblMealTime");
            chkBreakfast.Text = LanguageManager.GetTranslation("chkBreakfast");
            chkLunch.Text = LanguageManager.GetTranslation("chkLunch");
            chkDinner.Text = LanguageManager.GetTranslation("chkDinner");

            LanguageManager.ApplyFullButtonImage(btnSave, "icons_save.png");
            LanguageManager.ApplyFullButtonImage(btnCancel, "icons_cancel.png");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show(
                        LanguageManager.GetTranslation("inputNamePrompt"),
                        LanguageManager.GetTranslation("hintTitle"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                var foodTypes = ParseMultiValueText(txtFoodTypes.Text);
                if (foodTypes.Count == 0)
                {
                    MessageBox.Show(
                        LanguageManager.GetTranslation("inputFoodTypePrompt"),
                        LanguageManager.GetTranslation("hintTitle"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
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

                MessageBox.Show(
                    LanguageManager.GetTranslation("saveSuccess"),
                    LanguageManager.GetTranslation("resetDoneTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    LanguageManager.GetTranslation("saveFailed") + ex.Message,
                    LanguageManager.GetTranslation("resetFailedTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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

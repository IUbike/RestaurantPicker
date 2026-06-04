using System.Drawing;
using RestaurantPicker.Models;
using RestaurantPicker.Services;

namespace RestaurantPicker.Views
{
    public partial class MealSelectForm : Form
    {
        private readonly UserProfile _currentUser;
        private readonly FavoriteService _favoriteService;
        private readonly BlockedService _blockedService;
        public int SelectedMinMealHour { get; private set; }
        public int SelectedMaxMealHour { get; private set; }

        /// <summary>
        /// 識別選擇的用餐時段類型：breakfast/lunch/dinner
        /// </summary>
        public string SelectedMealTimeType { get; private set; } = "lunch";

        private readonly List<string> _timeOptions = new List<string>();

        public MealSelectForm(UserProfile currentUser, FavoriteService favoriteService, BlockedService blockedService)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _favoriteService = favoriteService;
            _blockedService = blockedService;
            this.Text = "選擇用餐時段";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackgroundImage = LanguageManager.LoadAssetImage("back3.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            AcceptButton = btnNext;
            CancelButton = btnCancel;
        }

        private void MealSelectForm_Load(object sender, EventArgs e)
        {
            LoadTimeOptions();
            ApplyLanguage();

            // 依裝置現在時間預設區間
            var nowHour = DateTime.Now.Hour;
            if (nowHour <= 10)
            {
                rbBreakfast.Checked = true;
                SetTimeRange("06:00", "10:00");
                SelectedMealTimeType = "breakfast";
            }
            else if (nowHour <= 15)
            {
                rbLunch.Checked = true;
                SetTimeRange("11:00", "14:00");
                SelectedMealTimeType = "lunch";
            }
            else
            {
                rbDinner.Checked = true;
                SetTimeRange("17:00", "21:00");
                SelectedMealTimeType = "dinner";
            }
        }

        private void ApplyLanguage()
        {
            this.Text = LanguageManager.GetTranslation("mealTitle");
            lblTitle.Text = LanguageManager.GetTranslation("mealTitle");
            groupBoxMealTime.Text = LanguageManager.GetTranslation("mealPreset");
            rbBreakfast.Text = LanguageManager.GetTranslation("breakfast");
            rbLunch.Text = LanguageManager.GetTranslation("lunch");
            rbDinner.Text = LanguageManager.GetTranslation("dinner");
            lblRangeTitle.Text = LanguageManager.GetTranslation("mealRange");
            lblHint.Text = LanguageManager.GetTranslation("mealHint");

            // Make labels, radiobuttons, panels, groupboxes transparent recursively
            MakeControlsTransparent(this);

            // Apply full-button images dynamically
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

        private void LoadTimeOptions()
        {
            _timeOptions.Clear();
            for (int hour = 0; hour <= 23; hour++)
            {
                _timeOptions.Add($"{hour:00}:00");
                _timeOptions.Add($"{hour:00}:30");
            }

            cbMinTime.Items.Clear();
            cbMaxTime.Items.Clear();
            cbMinTime.Items.AddRange(_timeOptions.Cast<object>().ToArray());
            cbMaxTime.Items.AddRange(_timeOptions.Cast<object>().ToArray());
        }

        private void MealPreset_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not RadioButton radio || !radio.Checked)
                return;

            if (rbBreakfast.Checked)
            {
                SetTimeRange("06:00", "10:00");
                SelectedMealTimeType = "breakfast";  // 記錄時段類型
            }
            else if (rbLunch.Checked)
            {
                SetTimeRange("11:00", "14:00");
                SelectedMealTimeType = "lunch";
            }
            else if (rbDinner.Checked)
            {
                SetTimeRange("17:00", "21:00");
                SelectedMealTimeType = "dinner";
            }
        }

        private void SetTimeRange(string minTime, string maxTime)
        {
            cbMinTime.SelectedItem = minTime;
            cbMaxTime.SelectedItem = maxTime;
        }

        private bool TryGetSelectedTimes(out TimeSpan minTime, out TimeSpan maxTime)
        {
            minTime = default;
            maxTime = default;

            if (cbMinTime.SelectedItem == null || cbMaxTime.SelectedItem == null)
                return false;

            return TimeSpan.TryParse(cbMinTime.SelectedItem.ToString(), out minTime)
                && TimeSpan.TryParse(cbMaxTime.SelectedItem.ToString(), out maxTime);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedTimes(out var minTime, out var maxTime))
            {
                MessageBox.Show(
                    LanguageManager.GetTranslation("invalidTimeRange"),
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (minTime > maxTime)
            {
                MessageBox.Show(
                    LanguageManager.GetTranslation("minMaxTimeError"),
                    LanguageManager.GetTranslation("hintTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // 目前篩選核心仍是小時級，先做相容映射。
            // 例如 11:30~13:30 會轉成 11~14。
            SelectedMinMealHour = minTime.Hours;
            SelectedMaxMealHour = maxTime.Hours + (maxTime.Minutes > 0 ? 1 : 0);
            if (SelectedMaxMealHour > 23)
            {
                SelectedMaxMealHour = 23;
            }

            // 傳入 SelectedMealTimeType，讓 CategorySelectForm 知道是哪個時段
            using var categoryForm = new CategorySelectForm(SelectedMinMealHour, SelectedMaxMealHour, SelectedMealTimeType, _currentUser, _favoriteService, _blockedService);
            if (categoryForm.ShowDialog() == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

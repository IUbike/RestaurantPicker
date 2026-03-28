using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RestaurantPicker.Views
{
    public partial class MealSelectForm : Form
    {
        public int SelectedMinMealHour { get; private set; }
        public int SelectedMaxMealHour { get; private set; }

        private readonly List<string> _timeOptions = new List<string>();

        public MealSelectForm()
        {
            InitializeComponent();
            this.Text = "選擇用餐時段";
            this.StartPosition = FormStartPosition.CenterScreen;

            AcceptButton = btnNext;
            CancelButton = btnCancel;
        }

        private void MealSelectForm_Load(object sender, EventArgs e)
        {
            LoadTimeOptions();

            // 依裝置現在時間預設區間
            var nowHour = DateTime.Now.Hour;
            if (nowHour <= 10)
            {
                rbBreakfast.Checked = true;
                SetTimeRange("06:00", "10:00");
            }
            else if (nowHour <= 15)
            {
                rbLunch.Checked = true;
                SetTimeRange("11:00", "14:00");
            }
            else
            {
                rbDinner.Checked = true;
                SetTimeRange("17:00", "21:00");
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
            }
            else if (rbLunch.Checked)
            {
                SetTimeRange("11:00", "14:00");
            }
            else if (rbDinner.Checked)
            {
                SetTimeRange("17:00", "21:00");
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
                MessageBox.Show("請選擇有效的時間範圍", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (minTime > maxTime)
            {
                MessageBox.Show("最小時間不可大於最大時間", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            using var categoryForm = new CategorySelectForm(SelectedMinMealHour, SelectedMaxMealHour);
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

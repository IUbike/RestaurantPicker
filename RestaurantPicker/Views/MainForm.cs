using System;
using System.Windows.Forms;
using RestaurantPicker.Repositories;
using RestaurantPicker.Services;

namespace RestaurantPicker.Views
{
    public partial class MainForm : Form
    {
        // 依賴注入的服務
        private IRestaurantRepository _restaurantRepository;
        private RestaurantFilterService _filterService;
        private RandomPickService _randomPickService;

        public MainForm()
        {
            InitializeComponent();
            this.Text = "隨機餐廳選擇系統";
            this.StartPosition = FormStartPosition.CenterScreen;

            // 初始化服務
            string csvPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "restaurants.csv"
            );
            _restaurantRepository = new CsvRestaurantRepository(csvPath);
            _filterService = new RestaurantFilterService();
            _randomPickService = new RandomPickService();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 主頁面初始化邏輯可在此加入
        }

        /// <summary>
        /// 「開始」按鈕點擊事件
        /// 進入用餐時段選擇
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            using var mealForm = new MealSelectForm();
            if (mealForm.ShowDialog() == DialogResult.OK)
            {
                // 使用者已完成整個流程
            }
        }

        /// <summary>
        /// 「管理餐廳」按鈕點擊事件
        /// 進入新增/編輯餐廳頁面
        /// </summary>
        private void btnManageRestaurant_Click(object sender, EventArgs e)
        {
            using var manageForm = new ManageRestaurantForm(_restaurantRepository);
            if (manageForm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("餐廳資料已更新。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnManagePreference_Click(object sender, EventArgs e)
        {
            using var preferenceForm = new ManagePreferenceForm(_restaurantRepository);
            preferenceForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 程式即將關閉，可在此進行清理
        }
    }
}

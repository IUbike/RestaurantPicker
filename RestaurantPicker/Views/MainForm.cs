using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using RestaurantPicker.Properties;
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
        private UserPreferenceService _preferenceService;
        private TodayMealService _todayMealService;

        // 今日餐廳面板
        private Panel _todayMealPanel;
        private Panel[] _mealSlotPanels = new Panel[6];
        private Label[] _mealSlotLabels = new Label[6];
        private Button[] _mealSlotButtons = new Button[6];

        public MainForm()
        {
            InitializeComponent();
            this.Text = "隨機餐廳選擇系統";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.MinimumSize = new Size(960, 600);
            this.ClientSize = new Size(1280, 720);
            this.BackColor = Color.FromArgb(250, 241, 224);
            this.BackgroundImage = LoadAssetImage("back1.jpg");
            this.BackgroundImageLayout = ImageLayout.Zoom;
            lblTitle.Visible = false;

            // 初始化服務
            string csvPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "restaurants.csv"
            );
            string databasePath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "restaurantpicker.db"
            );
            _restaurantRepository = new LiteDbRestaurantRepository(databasePath, csvPath);
            _filterService = new RestaurantFilterService();
            _randomPickService = new RandomPickService();

            // 初始化偏好服務
            string preferencePath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data",
                "user_preferences.json"
            );
            _preferenceService = new UserPreferenceService(databasePath, preferencePath);
            _preferenceService.LoadPreferences();

            // 初始化今日餐廳服務
            _todayMealService = new TodayMealService(_preferenceService, _restaurantRepository);

            // 初始化今日餐廳面板
            InitializeTodayMealPanel();

            // 視窗縮放時重新排版
            this.Resize += MainForm_Resize;

            // Configure Language Button
            btnLanguage.Click += btnLanguage_Click;
            btnLanguage.BackColor = Color.FromArgb(253, 249, 238);
            btnLanguage.ForeColor = Color.FromArgb(115, 87, 61);
            btnLanguage.FlatStyle = FlatStyle.Flat;
            btnLanguage.FlatAppearance.BorderSize = 0;
            btnLanguage.FlatAppearance.MouseDownBackColor = Color.FromArgb(242, 234, 214);
            btnLanguage.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 242, 226);
            btnLanguage.Font = new Font("微軟正黑體", 10F, FontStyle.Bold);
            btnLanguage.Cursor = Cursors.Hand;
            btnLanguage.Padding = new Padding(20, 0, 0, 0);
            btnLanguage.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                // Draw latte border outline
                using (var pen = new Pen(Color.FromArgb(180, 217, 198, 175), 1.5f))
                {
                    var borderPath = new System.Drawing.Drawing2D.GraphicsPath();
                    int d = 32;
                    borderPath.AddArc(new Rectangle(1, 1, d, d), 180, 90);
                    borderPath.AddArc(new Rectangle(btnLanguage.Width - d - 2, 1, d, d), 270, 90);
                    borderPath.AddArc(new Rectangle(btnLanguage.Width - d - 2, btnLanguage.Height - d - 2, d, d), 0, 90);
                    borderPath.AddArc(new Rectangle(1, btnLanguage.Height - d - 2, d, d), 90, 90);
                    borderPath.CloseFigure();
                    pe.Graphics.DrawPath(pen, borderPath);
                }

                // Draw minimalist GDI+ globe icon on the left
                int iconSize = 16;
                int iconX = 14;
                int iconY = (btnLanguage.Height - iconSize) / 2;
                using (var iconPen = new Pen(Color.FromArgb(115, 87, 61), 1.5f))
                {
                    pe.Graphics.DrawEllipse(iconPen, iconX, iconY, iconSize, iconSize);
                    pe.Graphics.DrawEllipse(iconPen, iconX + 4, iconY, iconSize - 8, iconSize);
                    pe.Graphics.DrawLine(iconPen, iconX, iconY + iconSize / 2, iconX + iconSize, iconY + iconSize / 2);
                    pe.Graphics.DrawLine(iconPen, iconX + iconSize / 2, iconY, iconX + iconSize / 2, iconY + iconSize);
                }
            };

            // Initial translation and icon loading
            ApplyLanguage();
        }

        private void MainForm_Resize(object? sender, EventArgs e)
        {
            ApplyLanguage();
            if (_todayMealPanel?.Visible == true)
            {
                RefreshTodayMealPanel();
            }
        }

        private void btnLanguage_Click(object? sender, EventArgs e)
        {
            if (LanguageManager.CurrentLanguage == LanguageType.Chinese)
            {
                LanguageManager.CurrentLanguage = LanguageType.English;
            }
            else
            {
                LanguageManager.CurrentLanguage = LanguageType.Chinese;
            }
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            if (lblTitle == null || btnStart == null || btnTodayMeal == null ||
                btnManageRestaurant == null || btnManagePreference == null || btnReset == null || btnLanguage == null)
            {
                return;
            }

            this.Text = LanguageManager.GetTranslation("lblTitle");
            lblTitle.Text = LanguageManager.GetTranslation("lblTitle");

            // 1. Resize and arrange all buttons first to get their final dimensions
            ApplyMainLayout();

            // 2. Scale and apply high-fidelity images to exactly fit the final button dimensions
            LanguageManager.ApplyFullButtonImage(btnStart, "icons_start.png");
            LanguageManager.ApplyFullButtonImage(btnTodayMeal, "icons_myfavorites.png");
            LanguageManager.ApplyFullButtonImage(btnManageRestaurant, "icons_favoritesmanagement.png");
            LanguageManager.ApplyFullButtonImage(btnManagePreference, "icons_collection.png");
            LanguageManager.ApplyFullButtonImage(btnReset, "icons_clear.png");

            // For the language switch button, style it cleanly as a flat button
            btnLanguage.FlatStyle = FlatStyle.Flat;
            btnLanguage.FlatAppearance.BorderSize = 0;
            btnLanguage.BackColor = Color.FromArgb(253, 249, 238);
            btnLanguage.ForeColor = Color.FromArgb(115, 87, 61);
            btnLanguage.Text = LanguageManager.GetTranslation("langToggle");
            btnLanguage.Image = null; // No standard icon image (GDI+ draws the globe icon instead!)

            // Translate today's panel elements
            if (_todayMealPanel != null)
            {
                var btnBack = _todayMealPanel.Controls["btnBackToHome"] as Button;
                if (btnBack != null)
                {
                    LanguageManager.ApplyFullButtonImage(btnBack, "icons_leave.png");
                }
                RefreshTodayMealPanel();
            }
        }

        private void ApplyMainLayout()
        {
            // 避免在 InitializeComponent 尚未完成時觸發 Resize 造成 NullReference
            if (lblTitle == null || btnStart == null || btnTodayMeal == null ||
                btnManageRestaurant == null || btnManagePreference == null || btnReset == null || btnLanguage == null)
            {
                return;
            }

            // 主頁按鈕採用 1:5 的黃金比例排版 (高度為寬度的 20%)，呈現最專業的遊戲感膠囊按鈕
            int buttonWidth = Math.Clamp((int)(ClientSize.Width * 0.34), 280, 380);
            int buttonHeight = Math.Clamp((int)(buttonWidth * 0.20), 56, 76);
            int gap = Math.Clamp(ClientSize.Height / 72, 6, 12);
            int titleTop = Math.Clamp(ClientSize.Height / 14, 24, 50);
            int x = (ClientSize.Width - buttonWidth) / 2;

            lblTitle.Location = new Point((ClientSize.Width - lblTitle.Width) / 2, titleTop);

            btnStart.Size = new Size(buttonWidth, buttonHeight);
            int controlsHeight = (buttonHeight * 5) + (gap * 4);
            int maxFirstButtonTop = Math.Max(96, ClientSize.Height - controlsHeight - 24);
            
            // 首頁按鈕群垂直置中微調，保留上下平衡的美感空間
            int firstButtonTop = Math.Clamp((int)(ClientSize.Height * 0.34), 120, maxFirstButtonTop);
            btnStart.Location = new Point(x, firstButtonTop);

            btnTodayMeal.Size = new Size(buttonWidth, buttonHeight);
            btnTodayMeal.Location = new Point(x, btnStart.Bottom + gap);

            btnManageRestaurant.Size = new Size(buttonWidth, buttonHeight);
            btnManageRestaurant.Location = new Point(x, btnTodayMeal.Bottom + gap);

            btnManagePreference.Size = new Size(buttonWidth, buttonHeight);
            btnManagePreference.Location = new Point(x, btnManageRestaurant.Bottom + gap);

            btnReset.Size = new Size(buttonWidth, buttonHeight);
            btnReset.Location = new Point(x, btnManagePreference.Bottom + gap);

            btnLanguage.Size = new Size(130, 42);
            btnLanguage.Location = new Point(ClientSize.Width - btnLanguage.Width - 24, 24);

            // Round the corners of the language button to make it a premium capsule shape
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int diameter = 32;
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, diameter, diameter), 180, 90);
            path.AddArc(new Rectangle(btnLanguage.Width - diameter - 1, 0, diameter, diameter), 270, 90);
            path.AddArc(new Rectangle(btnLanguage.Width - diameter - 1, btnLanguage.Height - diameter - 1, diameter, diameter), 0, 90);
            path.AddArc(new Rectangle(0, btnLanguage.Height - diameter - 1, diameter, diameter), 90, 90);
            path.CloseFigure();
            btnLanguage.Region = new Region(path);

            ApplyTodayMealPanelLayout();
        }

        private void ApplyTodayMealPanelLayout()
        {
            if (_todayMealPanel == null)
                return;

            // 今日面板不可大於目前視窗，否則在小螢幕或高 DPI 環境會被裁切。
            int outerPadding = Math.Clamp(ClientSize.Width / 34, 16, 32);
            int panelWidth = Math.Max(280, ClientSize.Width - (outerPadding * 2));
            int panelHeight = Math.Max(320, ClientSize.Height - (outerPadding * 2));
            _todayMealPanel.Size = new Size(panelWidth, panelHeight);
            _todayMealPanel.Location = new Point(outerPadding, outerPadding);

            int gridPadding = 16;
            int gapX = 16;
            int gapY = 16;
            int cols = panelWidth >= 820 ? 3 : panelWidth >= 520 ? 2 : 1;
            int rows = (int)Math.Ceiling(6d / cols);
            int reservedBottom = 70; // 返回按鈕區

            int usableWidth = panelWidth - (gridPadding * 2) - SystemInformation.VerticalScrollBarWidth;
            int viewportHeight = panelHeight - gridPadding - reservedBottom;

            int slotWidth = (usableWidth - (gapX * (cols - 1))) / cols;
            int slotHeight = Math.Clamp((viewportHeight - (gapY * (rows - 1))) / rows, 170, 240);

            for (int idx = 0; idx < 6; idx++)
            {
                var slotPanel = _mealSlotPanels[idx];
                if (slotPanel == null) continue;

                int row = idx / 3;
                int col = idx % 3;

                int sx = gridPadding + col * (slotWidth + gapX);
                int sy = gridPadding + row * (slotHeight + gapY);

                slotPanel.Location = new Point(sx, sy);
                slotPanel.Size = new Size(slotWidth, slotHeight);

                if (_mealSlotLabels[idx] != null)
                {
                    _mealSlotLabels[idx].Location = new Point(5, 6);
                    _mealSlotLabels[idx].Size = new Size(slotWidth - 10, 28);
                }

                var button = _mealSlotButtons[idx];
                var ratingLabel = slotPanel.Controls.Count > 2 ? slotPanel.Controls[2] as Label : null;
                if (button != null)
                {
                    int buttonHeight = Math.Max(92, slotHeight - 78);
                    button.Location = new Point(10, 38);
                    button.Size = new Size(slotWidth - 20, buttonHeight);
                }

                if (ratingLabel != null && button != null)
                {
                    ratingLabel.Location = new Point(10, button.Bottom + 4);
                    ratingLabel.Size = new Size(slotWidth - 20, 30);
                }
            }

            int gridHeight = gridPadding + rows * slotHeight + (rows - 1) * gapY;
            int backTop = Math.Max(gridHeight + 14, panelHeight - 58);

            // 返回按鈕置中
            var btnBack = _todayMealPanel.Controls["btnBackToHome"] as Button;
            if (btnBack != null)
            {
                btnBack.Location = new Point((_todayMealPanel.Width - btnBack.Width) / 2, backTop);
            }

            _todayMealPanel.AutoScrollMinSize = new Size(0, backTop + 58);
        }

        private void ConfigureStartButton()
        {
            // 檢查按鈕是否初始化且尺寸有效
            if (btnStart == null || btnStart.Size.Width <= 0 || btnStart.Size.Height <= 0)
                return;

            try
            {
                // 設置按鈕外觀
                btnStart.FlatAppearance.BorderSize = 0;
                btnStart.FlatAppearance.MouseDownBackColor = Color.Transparent;
                btnStart.FlatAppearance.MouseOverBackColor = Color.Transparent;
                btnStart.BackColor = Color.Transparent;
                btnStart.ForeColor = Color.Black;
                btnStart.FlatStyle = FlatStyle.Flat;
                btnStart.Cursor = Cursors.Hand;
                btnStart.BackgroundImage = null;

                // 嘗試動態載入圖片
                using var startIcon = LanguageManager.LoadIcon("icons_start.png");
                if (startIcon != null)
                {
                    var coverImage = CreateCoverImage(startIcon, btnStart.Size);
                    if (coverImage != null)
                    {
                        btnStart.Image?.Dispose();
                        btnStart.Image = coverImage;
                        btnStart.ImageAlign = ContentAlignment.MiddleCenter;
                        btnStart.TextAlign = ContentAlignment.MiddleCenter;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"配置 Start 按鈕失敗: {ex.Message}");
                // 失敗時，保留預設樣式
                btnStart.BackColor = Color.LightBlue;
                btnStart.Text = LanguageManager.GetTranslation("btnStart") == "  " ? "開始選餐" : LanguageManager.GetTranslation("btnStart");
                btnStart.Font = new Font("微軟正黑體", 14F, FontStyle.Bold);
            }
        }

        private static Bitmap CreateCoverImage(Image image, Size size)
        {
            // 先裁剪透明邊緣，只保留最小方形
            var croppedImage = CropTransparentBorders(image);

            var bitmap = new Bitmap(size.Width, size.Height);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Transparent);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float scale = Math.Max((float)size.Width / croppedImage.Width, (float)size.Height / croppedImage.Height);
            float drawWidth = croppedImage.Width * scale;
            float drawHeight = croppedImage.Height * scale;
            float x = (size.Width - drawWidth) / 2f;
            float y = (size.Height - drawHeight) / 2f;

            graphics.DrawImage(croppedImage, new RectangleF(x, y, drawWidth, drawHeight));
            croppedImage.Dispose();
            return bitmap;
        }

        private static Bitmap CropTransparentBorders(Image image)
        {
            if (image == null)
                return new Bitmap(1, 1);

            var bitmap = new Bitmap(image);

            // 如果圖像太小或沒有有效的內容，直接返回
            if (bitmap.Width < 2 || bitmap.Height < 2)
                return bitmap;

            int minX = bitmap.Width;
            int maxX = -1;
            int minY = bitmap.Height;
            int maxY = -1;

            // 掃描所有非透明像素，找出邊界
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                unsafe
                {
                    byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
                    int bytesPerPixel = 4;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            int pixelIndex = (y * bitmapData.Stride) + (x * bytesPerPixel);
                            byte alpha = ptr[pixelIndex + 3];  // Alpha 通道

                            if (alpha > 0)  // 非透明
                            {
                                minX = Math.Min(minX, x);
                                maxX = Math.Max(maxX, x);
                                minY = Math.Min(minY, y);
                                maxY = Math.Max(maxY, y);
                            }
                        }
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            // 沒有找到非透明像素
            if (maxX < minX || maxY < minY)
                return bitmap;

            // 計算最小方形的大小
            int width = maxX - minX + 1;
            int height = maxY - minY + 1;
            int squareSize = Math.Max(width, height);

            // 中心對齐計算起點
            int squareX = minX + (width - squareSize) / 2;
            int squareY = minY + (height - squareSize) / 2;

            // 確保不超出邊界
            squareX = Math.Max(0, Math.Min(squareX, bitmap.Width - squareSize));
            squareY = Math.Max(0, Math.Min(squareY, bitmap.Height - squareSize));

            // 裁剪為正方形
            var croppedBitmap = new Bitmap(squareSize, squareSize);
            using var graphics = Graphics.FromImage(croppedBitmap);
            graphics.Clear(Color.Transparent);
            graphics.DrawImage(bitmap, 
                new Rectangle(0, 0, squareSize, squareSize),
                new Rectangle(squareX, squareY, squareSize, squareSize),
                GraphicsUnit.Pixel);

            bitmap.Dispose();
            return croppedBitmap;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 在窗體完全載入後配置按鈕，確保所有控件已初始化
            ApplyMainLayout();
            ApplyLanguage();

            // 確保今日餐廳面板在最前面（層級設置）
            if (_todayMealPanel != null)
            {
                this.Controls.Remove(_todayMealPanel);
                this.Controls.Add(_todayMealPanel);
                _todayMealPanel.BringToFront();
                _todayMealPanel.Visible = false;  // 初始隱藏
            }
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
                _preferenceService.LoadPreferences();
                // 使用者已完成整個流程，刷新今日餐廳面板
                RefreshTodayMealPanel();
                // 返回隱藏主頁面，顯示今日餐廌面板
                btnTodayMeal_Click(null, null);
            }
        }

        /// <summary>
        /// 「今日餐廳」按鈕點擊事件
        /// 顯示今日餐廳面板
        /// </summary>
        private void btnTodayMeal_Click(object sender, EventArgs e)
        {
            // 隱藏主頁面控件
            lblTitle.Visible = false;
            btnStart.Visible = false;
            btnManageRestaurant.Visible = false;
            btnManagePreference.Visible = false;
            btnTodayMeal.Visible = false;
            btnReset.Visible = false;
            btnLanguage.Visible = false;

            // 顯示今日餐廳面板
            _todayMealPanel.Visible = true;
            _todayMealPanel.BringToFront();
            ApplyTodayMealPanelLayout();

            // 重新載入使用者偏好，確保顯示為最新紀錄
            _preferenceService.LoadPreferences();
            RefreshTodayMealPanel();

            // 添加返回按鈕到面板
            if (!_todayMealPanel.Controls.ContainsKey("btnBackToHome"))
            {
                var btnBack = new Button
                {
                    Name = "btnBackToHome",
                    Size = new Size(220, 50),
                    Cursor = Cursors.Hand
                };
                LanguageManager.ApplyFullButtonImage(btnBack, "icons_leave.png");
                btnBack.Click += (s, e) => ReturnToHome();
                _todayMealPanel.Controls.Add(btnBack);
            }

            ApplyTodayMealPanelLayout();
            RefreshTodayMealPanel();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                LanguageManager.GetTranslation("resetConfirm"),
                LanguageManager.GetTranslation("resetTitle"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
                return;

            try
            {
                // 清除偏好檔
                _preferenceService.ResetPreferences();

                // 重新載入偏好並刷新 UI
                _preferenceService.LoadPreferences();
                RefreshTodayMealPanel();

                MessageBox.Show(
                    LanguageManager.GetTranslation("resetDone"),
                    LanguageManager.GetTranslation("resetDoneTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                System.Diagnostics.Debug.WriteLine("[DEBUG] 使用者按下 Reset，已清除所有偏好檔案。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    LanguageManager.GetTranslation("resetFailed") + ex.Message,
                    LanguageManager.GetTranslation("resetFailedTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Reset 失敗: {ex.Message}");
            }
        }

        /// <summary>
        /// 返回主頁面
        /// </summary>
        private void ReturnToHome()
        {
            // 隱藏今日餐廳面板
            _todayMealPanel.Visible = false;

            // 顯示主頁面控件
            lblTitle.Visible = false;
            btnStart.Visible = true;
            btnManageRestaurant.Visible = true;
            btnManagePreference.Visible = true;
            btnTodayMeal.Visible = true;
            btnReset.Visible = true;
            btnLanguage.Visible = true;

            // 移除返回按鈕（下次進入時重新建立）
            var btnBack = _todayMealPanel.Controls["btnBackToHome"];
            if (btnBack != null)
            {
                _todayMealPanel.Controls.Remove(btnBack);
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
                MessageBox.Show(
                    LanguageManager.GetTranslation("updateDone"),
                    LanguageManager.GetTranslation("resetDoneTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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

        /// <summary>
        /// 初始化今日餐廳面板
        /// </summary>
        private void InitializeTodayMealPanel()
        {
            _todayMealPanel = new Panel
            {
                Location = new Point(50, 150),
                Size = new Size(840, 580),  // 增加高度以容納返回按鈕
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true,
                Visible = false
            };

            // 建立 3x2 網格（6 個餐廳槽位）
            int[] slotIndices = { 0, 1, 2, 3, 4, 5 };

            foreach (var idx in slotIndices)
            {
                int row = idx / 3;
                int col = idx % 3;
                int x = 50 + col * 260;
                int y = 30 + row * 220;

                // 外層面板
                var slotPanel = new Panel
                {
                    Location = new Point(x, y),
                    Size = new Size(240, 200),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.White
                };

                // 上方標籤（改為顯示目前店名）
                var label = new Label
                {
                    Location = new Point(5, 5),
                    Size = new Size(230, 25),
                    Font = new Font("微軟正黑體", 10F, FontStyle.Bold),
                    Text = "",
                    AutoEllipsis = true,
                    TextAlign = ContentAlignment.TopCenter
                };

                // 餐廳按鈕（圖片或灰色方框）
                var button = new Button
                {
                    Location = new Point(10, 35),
                    Size = new Size(220, 130),
                    BackColor = Color.LightGray,
                    FlatStyle = FlatStyle.Flat,
                    Text = "點擊新增",
                    Font = new Font("微軟正黑體", 11F),
                    ForeColor = Color.DarkGray,
                    Tag = idx
                };

                button.Click += (s, e) => MealSlot_Click(idx);

                // 評分文本或按鈕
                var ratingLabel = new Label
                {
                    Location = new Point(10, 166),
                    Size = new Size(220, 28),
                    Font = new Font("微軟正黑體", 9F),
                    Text = "",
                    TextAlign = ContentAlignment.MiddleCenter
                };

                slotPanel.Controls.Add(label);
                slotPanel.Controls.Add(button);
                slotPanel.Controls.Add(ratingLabel);

                _todayMealPanel.Controls.Add(slotPanel);
                _mealSlotPanels[idx] = slotPanel;
                _mealSlotLabels[idx] = label;
                _mealSlotButtons[idx] = button;
            }

            this.Controls.Add(_todayMealPanel);
        }

        /// <summary>
        /// 刷新今日餐廳面板顯示
        /// </summary>
        private void RefreshTodayMealPanel()
        {
            // 重新載入偏好，確保取得最新的 JSON 紀錄
            _preferenceService.LoadPreferences();

            var slots = _todayMealService.GetTodayMeals();

            for (int i = 0; i < 6; i++)
            {
                var slot = slots[i];
                var button = _mealSlotButtons[i];
                var label = _mealSlotLabels[i];
                var ratingLabel = _mealSlotPanels[i].Controls[2] as Label;

                if (slot.HasMeal)
                {
                    // 嘗試用 repository 取得完整餐廳資料
                    var restaurant = _restaurantRepository.GetById(slot.RestaurantId);

                    // 如果尚未載入或找不到，嘗試強制載入一次 CSV 再取
                    if (restaurant == null)
                    {
                        try
                        {
                            _restaurantRepository.LoadAll();
                            restaurant = _restaurantRepository.GetById(slot.RestaurantId);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"重新載入餐廳資料失敗: {ex.Message}");
                        }
                    }

                    string displayName = restaurant?.Name ?? slot.RestaurantName;
                    label.Text = displayName;

                    button.Text = displayName;
                    button.BackColor = Color.LightBlue;
                    button.ForeColor = Color.Black;

                    // 嘗試載入餐廳圖片
                    if (restaurant != null && !string.IsNullOrEmpty(restaurant.ImageFileName))
                    {
                        var image = LoadRestaurantImage(restaurant.ImageFileName);
                        if (image != null)
                        {
                            button.Image?.Dispose();
                            button.Image = CreateContainImage(image, button.Size);
                            image.Dispose();
                            button.ImageAlign = ContentAlignment.MiddleCenter;
                            button.Text = "";
                        }
                        else
                        {
                            button.Image = null;
                            button.Text = displayName;
                        }
                    }
                    else
                    {
                        button.Image = null;
                        button.Text = displayName;
                    }

                    if (slot.HasRating)
                    {
                        ratingLabel.Text = LanguageManager.GetTranslation("ratingTitle") + GetStarDisplay(slot.Rating);
                        ratingLabel.ForeColor = Color.Black;
                    }
                    else
                    {
                        ratingLabel.Text = LanguageManager.GetTranslation("notRatedYet");
                        ratingLabel.ForeColor = Color.Red;
                    }
                }
                else
                {
                    label.Text = "";
                    button.Text = LanguageManager.GetTranslation("clickToAdd");
                    button.Image = null;
                    button.BackColor = Color.LightGray;
                    button.ForeColor = Color.DarkGray;
                    ratingLabel.Text = "";
                }
            }
        }

        /// <summary>
        /// 載入餐廳圖片（支持多種文件名格式）
        /// </summary>
        private Image LoadRestaurantImage(string imageFileName)
        {
            if (string.IsNullOrWhiteSpace(imageFileName))
                return null;

            try
            {
                string imagesDir = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Assets",
                    "images"
                );

                System.Diagnostics.Debug.WriteLine($"[DEBUG] 基礎目錄: {AppDomain.CurrentDomain.BaseDirectory}");
                System.Diagnostics.Debug.WriteLine($"[DEBUG] 圖片目錄: {imagesDir}");
                System.Diagnostics.Debug.WriteLine($"[DEBUG] 尋找圖片: {imageFileName}");

                // 1. 先嘗試精確文件名匹配
                string imagePath = System.IO.Path.Combine(imagesDir, imageFileName);
                System.Diagnostics.Debug.WriteLine($"[DEBUG] 完整路徑 (精確): {imagePath}");
                System.Diagnostics.Debug.WriteLine($"[DEBUG] 檔案存在? {System.IO.File.Exists(imagePath)}");

                if (System.IO.File.Exists(imagePath))
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] ✅ 找到圖片: {imagePath}");
                    using var fs = new System.IO.FileStream(imagePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                    using var temp = Image.FromStream(fs);
                    return new Bitmap(temp);
                }

                // 2. 若精確匹配失敗，嘗試去掉前導零的版本
                // 例如：restaurant_01.jpg → restaurant_1.jpg
                if (imageFileName.Contains("restaurant_") && imageFileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    string withoutZero = System.Text.RegularExpressions.Regex.Replace(imageFileName, @"restaurant_0+(\d+)\.jpg", "restaurant_$1.jpg", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] 嘗試去掉前導零: {withoutZero}");

                    if (withoutZero != imageFileName)
                    {
                        imagePath = System.IO.Path.Combine(imagesDir, withoutZero);
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] 完整路徑 (無零): {imagePath}");
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] 檔案存在? {System.IO.File.Exists(imagePath)}");

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.Diagnostics.Debug.WriteLine($"[DEBUG] ✅ 找到圖片: {imagePath}");
                            using var fs = new System.IO.FileStream(imagePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                            using var temp = Image.FromStream(fs);
                            return new Bitmap(temp);
                        }
                    }
                }

                // 3. 如果以上都失敗，返回 null（會顯示灰色占位符）
                System.Diagnostics.Debug.WriteLine($"[DEBUG] ❌ 找不到圖片檔案: {imageFileName}");
                System.Diagnostics.Debug.WriteLine($"[DEBUG] 圖片目錄是否存在? {System.IO.Directory.Exists(imagesDir)}");

                // 列出目錄中的所有文件
                if (System.IO.Directory.Exists(imagesDir))
                {
                    var files = System.IO.Directory.GetFiles(imagesDir, "*.jpg");
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] 目錄中找到 {files.Length} 個 JPG 文件");
                    if (files.Length > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] 前 5 個文件:");
                        foreach (var f in files.Take(5))
                        {
                            System.Diagnostics.Debug.WriteLine($"[DEBUG]   - {System.IO.Path.GetFileName(f)}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] ❌ 載入圖片失敗 {imageFileName}: {ex.Message}");
            }

            return null;
        }

        private static Image? LoadAssetImage(string fileName)
        {
            try
            {
                string imagePath = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Assets",
                    fileName
                );

                if (!System.IO.File.Exists(imagePath))
                    return null;

                using var stream = new System.IO.FileStream(imagePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                using var image = Image.FromStream(stream);
                return new Bitmap(image);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"載入首頁背景失敗: {ex.Message}");
                return null;
            }
        }

        private static Bitmap CreateContainImage(Image image, Size size)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float scale = Math.Min((float)size.Width / image.Width, (float)size.Height / image.Height);
            int drawWidth = Math.Max(1, (int)(image.Width * scale));
            int drawHeight = Math.Max(1, (int)(image.Height * scale));
            int x = (size.Width - drawWidth) / 2;
            int y = (size.Height - drawHeight) / 2;

            graphics.DrawImage(image, new Rectangle(x, y, drawWidth, drawHeight));
            return bitmap;
        }

        /// <summary>
        /// 餐廳槽位點擊事件
        /// </summary>
        private void MealSlot_Click(int slotIndex)
        {
            var slots = _todayMealService.GetTodayMeals();
            var slot = slots[slotIndex];

            if (slot.HasMeal)
            {
                // 已有餐廳，點擊進行評分
                if (!slot.HasRating)
                {
                    ShowRatingDialog(slot.MealRecordId, slot.RestaurantId);
                }
                else
                {
                    // 已評分，點擊修改評分
                    var result = MessageBox.Show(
                        $"已評分 {GetStarDisplay(slot.Rating)}\n\n是否要修改評分？",
                        "提示",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        ShowRatingDialog(slot.MealRecordId, slot.RestaurantId);
                    }
                }
            }
            else
            {
                // 未選擇，打開餐廳選擇對話框
                ShowSelectRestaurantDialog(slotIndex);
            }
        }

        /// <summary>
        /// 顯示餐廳選擇對話框
        /// </summary>
        private void ShowSelectRestaurantDialog(int slotIndex)
        {
            using var selectForm = new SelectRestaurantForm(_restaurantRepository, _preferenceService, "custom");
            if (selectForm.ShowDialog() == DialogResult.OK && selectForm.SelectedRestaurantId > 0)
            {
                ShowRatingDialog(0, selectForm.SelectedRestaurantId, "custom", slotIndex);
            }
        }

        /// <summary>
        /// 顯示評分對話框
        /// </summary>
        private void ShowRatingDialog(int mealRecordId, int restaurantId, string mealTimeType = "lunch", int customSlotIndex = -1)
        {
            var restaurant = _restaurantRepository.GetById(restaurantId);
            if (restaurant == null)
            {
                MessageBox.Show("餐廳不存在", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var ratingForm = new RatingForm(restaurant);
            if (ratingForm.ShowDialog() == DialogResult.OK)
            {
                int rating = ratingForm.SelectedRating;

                if (mealRecordId > 0)
                {
                    _preferenceService.UpdateMealRating(mealRecordId, rating);
                }
                else
                {
                    var selectedMealType = ShowMealTypeSelectionDialog();
                    if (string.IsNullOrWhiteSpace(selectedMealType))
                    {
                        return;
                    }

                    var customLabel = selectedMealType == "custom" ? "其他" : null;
                    _todayMealService.AddMealToNextAvailableSlot(restaurantId, rating, customLabel, selectedMealType);
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] MainForm direct-add -> RestaurantId={restaurantId}, MealType={selectedMealType}, Rating={rating}");
                }

                RefreshTodayMealPanel();
            }
        }

        private string? ShowMealTypeSelectionDialog()
        {
            var form = new Form
            {
                Text = "選擇用餐時段",
                Width = 520,
                Height = 300,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var label = new Label
            {
                Left = 20,
                Top = 24,
                Width = 460,
                Height = 40,
                Text = "請選擇這筆紀錄的用餐時段：",
                Font = new Font("微軟正黑體", 12F, FontStyle.Bold)
            };

            var combo = new ComboBox
            {
                Left = 20,
                Top = 80,
                Width = 460,
                Height = 40,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("微軟正黑體", 12F)
            };
            combo.Items.Add("早餐");
            combo.Items.Add("午餐");
            combo.Items.Add("晚餐");
            combo.Items.Add("其他");
            combo.SelectedIndex = 1;

            var okButton = new Button
            {
                Text = "確認",
                Left = 240,
                Width = 110,
                Height = 48,
                Top = 170,
                DialogResult = DialogResult.OK,
                Font = new Font("微軟正黑體", 11F, FontStyle.Bold)
            };

            var cancelButton = new Button
            {
                Text = "取消",
                Left = 370,
                Width = 110,
                Height = 48,
                Top = 170,
                DialogResult = DialogResult.Cancel,
                Font = new Font("微軟正黑體", 11F)
            };

            form.Controls.Add(label);
            form.Controls.Add(combo);
            form.Controls.Add(okButton);
            form.Controls.Add(cancelButton);
            form.AcceptButton = okButton;
            form.CancelButton = cancelButton;

            if (form.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            return combo.SelectedItem?.ToString() switch
            {
                "早餐" => "breakfast",
                "午餐" => "lunch",
                "晚餐" => "dinner",
                _ => "custom"
            };
        }

        /// <summary>
        /// 取得星數顯示文本
        /// </summary>
        private string GetStarDisplay(int rating)
        {
            if (rating <= 0) return "";
            return new string('★', rating) + new string('☆', 5 - rating);
        }

        /// <summary>
        /// 簡單的輸入對話框
        /// </summary>
        private string InputBox(string prompt, string defaultValue = "")
        {
            var form = new Form
            {
                Text = "輸入",
                Width = 420,
                Height = 200,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var label = new Label { Left = 20, Top = 20, Text = prompt, Width = 360, AutoSize = false, Height = 30 };
            var textBox = new TextBox { Left = 20, Top = 60, Width = 360, Height = 35, Text = defaultValue, Font = new Font("微軟正黑體", 11F) };
            var okButton = new Button { Text = "確認", Left = 120, Width = 100, Height = 45, Top = 120, DialogResult = DialogResult.OK, Font = new Font("微軟正黑體", 11F) };
            var cancelButton = new Button { Text = "取消", Left = 240, Width = 100, Height = 45, Top = 120, DialogResult = DialogResult.Cancel, Font = new Font("微軟正黑體", 11F) };

            form.Controls.Add(label);
            form.Controls.Add(textBox);
            form.Controls.Add(okButton);
            form.Controls.Add(cancelButton);
            form.AcceptButton = okButton;
            form.CancelButton = cancelButton;

            return form.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}

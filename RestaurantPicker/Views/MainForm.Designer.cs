namespace RestaurantPicker.Views
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的設計工具變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受管理資源則為 true；否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            btnManageRestaurant = new Button();
            btnManagePreference = new Button();
            lblTitle = new Label();
            btnTodayMeal = new Button();
            btnReset = new Button();
            btnStart = new Button();
            btnLanguage = new Button();
            btnSwitchUser = new Button();
            SuspendLayout();
            // 
            // btnManageRestaurant
            // 
            btnManageRestaurant.BackColor = Color.Orange;
            btnManageRestaurant.Font = new Font("微軟正黑體", 14F, FontStyle.Regular, GraphicsUnit.Point, 136);
            btnManageRestaurant.ForeColor = Color.White;
            btnManageRestaurant.Location = new Point(327, 551);
            btnManageRestaurant.Margin = new Padding(7);
            btnManageRestaurant.Name = "btnManageRestaurant";
            btnManageRestaurant.Size = new Size(467, 106);
            btnManageRestaurant.TabIndex = 3;
            btnManageRestaurant.Text = "管理餐廳";
            btnManageRestaurant.UseVisualStyleBackColor = false;
            btnManageRestaurant.Click += btnManageRestaurant_Click;
            // 
            // btnManagePreference
            // 
            btnManagePreference.BackColor = Color.SteelBlue;
            btnManagePreference.Font = new Font("微軟正黑體", 13F, FontStyle.Regular, GraphicsUnit.Point, 136);
            btnManagePreference.ForeColor = Color.White;
            btnManagePreference.Location = new Point(327, 674);
            btnManagePreference.Margin = new Padding(7);
            btnManagePreference.Name = "btnManagePreference";
            btnManagePreference.Size = new Size(467, 106);
            btnManagePreference.TabIndex = 4;
            btnManagePreference.Text = "管理收藏/封鎖";
            btnManagePreference.UseVisualStyleBackColor = false;
            btnManagePreference.Click += btnManagePreference_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("微軟正黑體", 24F, FontStyle.Bold, GraphicsUnit.Point, 136);
            lblTitle.ForeColor = Color.DarkBlue;
            lblTitle.Location = new Point(107, 88);
            lblTitle.Margin = new Padding(7, 0, 7, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(483, 81);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "不知道吃什麼？";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnTodayMeal
            // 
            btnTodayMeal.BackColor = Color.Violet;
            btnTodayMeal.Font = new Font("微軟正黑體", 14F, FontStyle.Bold, GraphicsUnit.Point, 136);
            btnTodayMeal.ForeColor = Color.White;
            btnTodayMeal.Location = new Point(327, 428);
            btnTodayMeal.Margin = new Padding(7);
            btnTodayMeal.Name = "btnTodayMeal";
            btnTodayMeal.Size = new Size(467, 106);
            btnTodayMeal.TabIndex = 2;
            btnTodayMeal.Text = "📅 今日餐廳";
            btnTodayMeal.UseVisualStyleBackColor = false;
            btnTodayMeal.Click += btnTodayMeal_Click;
            // 
            // btnReset
            // 
            btnReset.BackColor = Color.DarkRed;
            btnReset.Font = new Font("微軟正黑體", 12F, FontStyle.Bold, GraphicsUnit.Point, 136);
            btnReset.ForeColor = Color.White;
            btnReset.Location = new Point(327, 798);
            btnReset.Margin = new Padding(7);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(467, 46);
            btnReset.TabIndex = 5;
            btnReset.Text = "重置紀錄 (清除所有使用者資料)";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btnReset_Click;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.Transparent;
            btnStart.BackgroundImage = Properties.Resources.icons_start1;
            btnStart.BackgroundImageLayout = ImageLayout.Stretch;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnStart.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("微軟正黑體", 18F, FontStyle.Bold, GraphicsUnit.Point, 136);
            btnStart.ForeColor = Color.Transparent;
            btnStart.Location = new Point(327, 273);
            btnStart.Margin = new Padding(7);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(467, 138);
            btnStart.TabIndex = 1;
            btnStart.Text = "  ";
            btnStart.UseMnemonic = false;
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnSwitchUser
            // 
            btnSwitchUser.BackColor = Color.FromArgb(253, 249, 238);
            btnSwitchUser.FlatStyle = FlatStyle.Flat;
            btnSwitchUser.Font = new Font("微軟正黑體", 10F, FontStyle.Bold, GraphicsUnit.Point, 136);
            btnSwitchUser.ForeColor = Color.FromArgb(115, 87, 61);
            btnSwitchUser.Location = new Point(902, 22);
            btnSwitchUser.Name = "btnSwitchUser";
            btnSwitchUser.Size = new Size(160, 42);
            btnSwitchUser.TabIndex = 6;
            btnSwitchUser.Text = "切換使用者";
            btnSwitchUser.UseVisualStyleBackColor = false;
            btnSwitchUser.Click += btnSwitchUser_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(14F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(1195, 1034);
            Controls.Add(btnSwitchUser);
            Controls.Add(btnLanguage);
            Controls.Add(btnReset);
            Controls.Add(btnManagePreference);
            Controls.Add(btnManageRestaurant);
            Controls.Add(btnTodayMeal);
            Controls.Add(btnStart);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(7);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "隨機餐廳選擇系統";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnTodayMeal;
        private System.Windows.Forms.Button btnManageRestaurant;
        private System.Windows.Forms.Button btnManagePreference;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnLanguage;
        private System.Windows.Forms.Button btnSwitchUser;
    }
}

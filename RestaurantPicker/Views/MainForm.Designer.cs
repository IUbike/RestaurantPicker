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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnManageRestaurant = new System.Windows.Forms.Button();
            this.btnManagePreference = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微軟正黑體", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitle.Location = new System.Drawing.Point(80, 40);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(240, 50);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "不知道吃什麼？";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // btnStart
            this.btnStart.BackColor = System.Drawing.Color.Green;
            this.btnStart.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(100, 120);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(200, 60);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "開始選擇";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            
            // btnManageRestaurant
            this.btnManageRestaurant.BackColor = System.Drawing.Color.Orange;
            this.btnManageRestaurant.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnManageRestaurant.ForeColor = System.Drawing.Color.White;
            this.btnManageRestaurant.Location = new System.Drawing.Point(100, 200);
            this.btnManageRestaurant.Name = "btnManageRestaurant";
            this.btnManageRestaurant.Size = new System.Drawing.Size(200, 46);
            this.btnManageRestaurant.TabIndex = 2;
            this.btnManageRestaurant.Text = "管理餐廳";
            this.btnManageRestaurant.UseVisualStyleBackColor = false;
            this.btnManageRestaurant.Click += new System.EventHandler(this.btnManageRestaurant_Click);
            
            // btnManagePreference
            this.btnManagePreference.BackColor = System.Drawing.Color.SteelBlue;
            this.btnManagePreference.Font = new System.Drawing.Font("微軟正黑體", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnManagePreference.ForeColor = System.Drawing.Color.White;
            this.btnManagePreference.Location = new System.Drawing.Point(100, 258);
            this.btnManagePreference.Name = "btnManagePreference";
            this.btnManagePreference.Size = new System.Drawing.Size(200, 46);
            this.btnManagePreference.TabIndex = 3;
            this.btnManagePreference.Text = "管理收藏/封鎖";
            this.btnManagePreference.UseVisualStyleBackColor = false;
            this.btnManagePreference.Click += new System.EventHandler(this.btnManagePreference_Click);
            
            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.Controls.Add(this.btnManagePreference);
            this.Controls.Add(this.btnManageRestaurant);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "隨機餐廳選擇系統";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnManageRestaurant;
        private System.Windows.Forms.Button btnManagePreference;
        private System.Windows.Forms.Label lblTitle;
    }
}

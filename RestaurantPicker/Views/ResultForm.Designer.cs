namespace RestaurantPicker.Views
{
    partial class ResultForm
    {
        /// <summary>
        /// 必需的設計工具變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理任何使用中的資源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTitleText = new System.Windows.Forms.Label();
            this.picRestaurantImage = new System.Windows.Forms.PictureBox();
            this.lblRestaurantName = new System.Windows.Forms.Label();
            this.lblRestaurantPhone = new System.Windows.Forms.Label();
            this.lblRestaurantHours = new System.Windows.Forms.Label();
            this.lblRestaurantAddress = new System.Windows.Forms.Label();
            this.lblRestaurantFeature = new System.Windows.Forms.Label();
            this.lblRestaurantFoodType = new System.Windows.Forms.Label();
            this.lblRestaurantPrice = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnFavorite = new System.Windows.Forms.Button();
            this.btnShare = new System.Windows.Forms.Button();
            this.btnDontShow = new System.Windows.Forms.Button();
            this.pnlResult = new System.Windows.Forms.Panel();
            this.pnlResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRestaurantImage)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微軟正黑體", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTitle.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblTitle.Location = new System.Drawing.Point(60, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(280, 36);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "✓ 推薦給你的餐廳";

            // lblTitleText
            this.lblTitleText.AutoSize = true;
            this.lblTitleText.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTitleText.ForeColor = System.Drawing.Color.Gray;
            this.lblTitleText.Location = new System.Drawing.Point(100, 60);
            this.lblTitleText.Name = "lblTitleText";
            this.lblTitleText.Size = new System.Drawing.Size(200, 20);
            this.lblTitleText.TabIndex = 1;
            this.lblTitleText.Text = "根據你的選擇，推薦如下：";

            // pnlResult
            this.pnlResult.BackColor = System.Drawing.Color.White;
            this.pnlResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlResult.Controls.Add(this.lblRestaurantFeature);
            this.pnlResult.Controls.Add(this.lblRestaurantFoodType);
            this.pnlResult.Controls.Add(this.lblRestaurantPrice);
            this.pnlResult.Controls.Add(this.lblRestaurantAddress);
            this.pnlResult.Controls.Add(this.lblRestaurantHours);
            this.pnlResult.Controls.Add(this.lblRestaurantPhone);
            this.pnlResult.Controls.Add(this.lblRestaurantName);
            this.pnlResult.Controls.Add(this.picRestaurantImage);
            this.pnlResult.Location = new System.Drawing.Point(30, 90);
            this.pnlResult.Name = "pnlResult";
            this.pnlResult.Size = new System.Drawing.Size(340, 350);
            this.pnlResult.TabIndex = 2;

            // picRestaurantImage
            this.picRestaurantImage.BackColor = System.Drawing.Color.Gainsboro;
            this.picRestaurantImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picRestaurantImage.Location = new System.Drawing.Point(15, 12);
            this.picRestaurantImage.Name = "picRestaurantImage";
            this.picRestaurantImage.Size = new System.Drawing.Size(310, 105);
            this.picRestaurantImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picRestaurantImage.TabIndex = 3;
            this.picRestaurantImage.TabStop = false;

            // lblRestaurantName
            this.lblRestaurantName.Font = new System.Drawing.Font("微軟正黑體", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRestaurantName.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblRestaurantName.Location = new System.Drawing.Point(15, 122);
            this.lblRestaurantName.Name = "lblRestaurantName";
            this.lblRestaurantName.Size = new System.Drawing.Size(310, 35);
            this.lblRestaurantName.TabIndex = 4;
            this.lblRestaurantName.Text = "餐廳名稱";
            this.lblRestaurantName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblRestaurantPhone
            this.lblRestaurantPhone.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRestaurantPhone.Location = new System.Drawing.Point(15, 158);
            this.lblRestaurantPhone.Name = "lblRestaurantPhone";
            this.lblRestaurantPhone.Size = new System.Drawing.Size(310, 25);
            this.lblRestaurantPhone.TabIndex = 5;
            this.lblRestaurantPhone.Text = "電話: 05-xxxxx";

            // lblRestaurantHours
            this.lblRestaurantHours.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRestaurantHours.Location = new System.Drawing.Point(15, 184);
            this.lblRestaurantHours.Name = "lblRestaurantHours";
            this.lblRestaurantHours.Size = new System.Drawing.Size(310, 25);
            this.lblRestaurantHours.TabIndex = 6;
            this.lblRestaurantHours.Text = "營業時間: 11:00-21:00";

            // lblRestaurantAddress
            this.lblRestaurantAddress.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRestaurantAddress.Location = new System.Drawing.Point(15, 210);
            this.lblRestaurantAddress.Name = "lblRestaurantAddress";
            this.lblRestaurantAddress.Size = new System.Drawing.Size(310, 25);
            this.lblRestaurantAddress.TabIndex = 7;
            this.lblRestaurantAddress.Text = "地址: 中正大學附近";

            // lblRestaurantPrice
            this.lblRestaurantPrice.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRestaurantPrice.Location = new System.Drawing.Point(15, 236);
            this.lblRestaurantPrice.Name = "lblRestaurantPrice";
            this.lblRestaurantPrice.Size = new System.Drawing.Size(310, 25);
            this.lblRestaurantPrice.TabIndex = 8;
            this.lblRestaurantPrice.Text = "價位: NT$50-100";

            // lblRestaurantFoodType
            this.lblRestaurantFoodType.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRestaurantFoodType.Location = new System.Drawing.Point(15, 262);
            this.lblRestaurantFoodType.Name = "lblRestaurantFoodType";
            this.lblRestaurantFoodType.Size = new System.Drawing.Size(310, 25);
            this.lblRestaurantFoodType.TabIndex = 9;
            this.lblRestaurantFoodType.Text = "食物種類: 豆漿, 蛋餅";

            // lblRestaurantFeature
            this.lblRestaurantFeature.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRestaurantFeature.Location = new System.Drawing.Point(15, 289);
            this.lblRestaurantFeature.Name = "lblRestaurantFeature";
            this.lblRestaurantFeature.Size = new System.Drawing.Size(310, 48);
            this.lblRestaurantFeature.TabIndex = 10;
            this.lblRestaurantFeature.Text = "特色: 有冷氣";

            // btnConfirm
            this.btnConfirm.BackColor = System.Drawing.Color.Green;
            this.btnConfirm.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(270, 450);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(100, 45);
            this.btnConfirm.TabIndex = 11;
            this.btnConfirm.Text = "確定";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);

            // btnFavorite
            this.btnFavorite.BackColor = System.Drawing.Color.Orange;
            this.btnFavorite.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnFavorite.ForeColor = System.Drawing.Color.White;
            this.btnFavorite.Location = new System.Drawing.Point(30, 450);
            this.btnFavorite.Name = "btnFavorite";
            this.btnFavorite.Size = new System.Drawing.Size(80, 45);
            this.btnFavorite.TabIndex = 12;
            this.btnFavorite.Text = "♡ 收藏";
            this.btnFavorite.UseVisualStyleBackColor = false;
            this.btnFavorite.Click += new System.EventHandler(this.btnFavorite_Click);

            // btnShare
            this.btnShare.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnShare.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnShare.ForeColor = System.Drawing.Color.White;
            this.btnShare.Location = new System.Drawing.Point(120, 450);
            this.btnShare.Name = "btnShare";
            this.btnShare.Size = new System.Drawing.Size(60, 45);
            this.btnShare.TabIndex = 13;
            this.btnShare.Text = "分享";
            this.btnShare.UseVisualStyleBackColor = false;
            this.btnShare.Click += new System.EventHandler(this.btnShare_Click);

            // btnDontShow
            this.btnDontShow.BackColor = System.Drawing.Color.Gray;
            this.btnDontShow.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnDontShow.ForeColor = System.Drawing.Color.White;
            this.btnDontShow.Location = new System.Drawing.Point(190, 450);
            this.btnDontShow.Name = "btnDontShow";
            this.btnDontShow.Size = new System.Drawing.Size(75, 45);
            this.btnDontShow.TabIndex = 14;
            this.btnDontShow.Text = "✕ 不想再看";
            this.btnDontShow.UseVisualStyleBackColor = false;
            this.btnDontShow.Click += new System.EventHandler(this.btnDontShow_Click);

            // ResultForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(400, 510);
            this.Controls.Add(this.btnDontShow);
            this.Controls.Add(this.btnShare);
            this.Controls.Add(this.btnFavorite);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.pnlResult);
            this.Controls.Add(this.lblTitleText);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResultForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "推薦結果";
            this.Load += new System.EventHandler(this.ResultForm_Load);
            this.pnlResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picRestaurantImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTitleText;
        private System.Windows.Forms.PictureBox picRestaurantImage;
        private System.Windows.Forms.Label lblRestaurantName;
        private System.Windows.Forms.Label lblRestaurantPhone;
        private System.Windows.Forms.Label lblRestaurantHours;
        private System.Windows.Forms.Label lblRestaurantAddress;
        private System.Windows.Forms.Label lblRestaurantFeature;
        private System.Windows.Forms.Label lblRestaurantFoodType;
        private System.Windows.Forms.Label lblRestaurantPrice;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnFavorite;
        private System.Windows.Forms.Button btnShare;
        private System.Windows.Forms.Button btnDontShow;
        private System.Windows.Forms.Panel pnlResult;
    }
}

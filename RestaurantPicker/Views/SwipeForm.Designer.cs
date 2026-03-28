namespace RestaurantPicker.Views
{
    partial class SwipeForm
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
            this.lblProgress = new System.Windows.Forms.Label();

            this.pnlLeft = new System.Windows.Forms.Panel();
            this.picLeftImage = new System.Windows.Forms.PictureBox();
            this.lblLeftName = new System.Windows.Forms.Label();
            this.lblLeftPhone = new System.Windows.Forms.Label();
            this.lblLeftHours = new System.Windows.Forms.Label();
            this.lblLeftFeature = new System.Windows.Forms.Label();
            this.lblLeftFoodType = new System.Windows.Forms.Label();

            this.pnlRight = new System.Windows.Forms.Panel();
            this.picRightImage = new System.Windows.Forms.PictureBox();
            this.lblRightName = new System.Windows.Forms.Label();
            this.lblRightPhone = new System.Windows.Forms.Label();
            this.lblRightHours = new System.Windows.Forms.Label();
            this.lblRightFeature = new System.Windows.Forms.Label();
            this.lblRightFoodType = new System.Windows.Forms.Label();

            this.btnSelectLeft = new System.Windows.Forms.Button();
            this.btnSelectRight = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTitle.Location = new System.Drawing.Point(16, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(113, 19);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "選擇你喜歡的餐廳";

            // lblProgress
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblProgress.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblProgress.Location = new System.Drawing.Point(18, 33);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(100, 18);
            this.lblProgress.TabIndex = 1;
            this.lblProgress.Text = "剩餘 X 家餐廳";

            // ===== 左邊面板 =====
            this.pnlLeft.BackColor = System.Drawing.Color.White;
            this.pnlLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLeft.Location = new System.Drawing.Point(20, 62);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(220, 338);
            this.pnlLeft.TabIndex = 2;

            // picLeftImage
            this.picLeftImage.BackColor = System.Drawing.Color.Gainsboro;
            this.picLeftImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLeftImage.Location = new System.Drawing.Point(10, 10);
            this.picLeftImage.Name = "picLeftImage";
            this.picLeftImage.Size = new System.Drawing.Size(198, 100);
            this.picLeftImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLeftImage.TabIndex = 3;
            this.picLeftImage.TabStop = false;
            this.pnlLeft.Controls.Add(this.picLeftImage);

            // lblLeftName
            this.lblLeftName.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblLeftName.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblLeftName.Location = new System.Drawing.Point(10, 115);
            this.lblLeftName.Name = "lblLeftName";
            this.lblLeftName.Size = new System.Drawing.Size(200, 30);
            this.lblLeftName.TabIndex = 4;
            this.lblLeftName.Text = "餐廳名稱";
            this.lblLeftName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.pnlLeft.Controls.Add(this.lblLeftName);

            // lblLeftFoodType
            this.lblLeftFoodType.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblLeftFoodType.ForeColor = System.Drawing.Color.Gray;
            this.lblLeftFoodType.Location = new System.Drawing.Point(10, 145);
            this.lblLeftFoodType.Name = "lblLeftFoodType";
            this.lblLeftFoodType.Size = new System.Drawing.Size(200, 35);
            this.lblLeftFoodType.TabIndex = 5;
            this.lblLeftFoodType.Text = "食物種類";
            this.pnlLeft.Controls.Add(this.lblLeftFoodType);

            // lblLeftPhone
            this.lblLeftPhone.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblLeftPhone.Location = new System.Drawing.Point(10, 182);
            this.lblLeftPhone.Name = "lblLeftPhone";
            this.lblLeftPhone.Size = new System.Drawing.Size(200, 25);
            this.lblLeftPhone.TabIndex = 6;
            this.lblLeftPhone.Text = "電話: 05-xxxxx";
            this.pnlLeft.Controls.Add(this.lblLeftPhone);

            // lblLeftHours
            this.lblLeftHours.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblLeftHours.Location = new System.Drawing.Point(10, 207);
            this.lblLeftHours.Name = "lblLeftHours";
            this.lblLeftHours.Size = new System.Drawing.Size(200, 25);
            this.lblLeftHours.TabIndex = 7;
            this.lblLeftHours.Text = "營業: 11:00-21:00";
            this.pnlLeft.Controls.Add(this.lblLeftHours);

            // lblLeftFeature
            this.lblLeftFeature.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblLeftFeature.Location = new System.Drawing.Point(10, 232);
            this.lblLeftFeature.Name = "lblLeftFeature";
            this.lblLeftFeature.Size = new System.Drawing.Size(200, 95);
            this.lblLeftFeature.TabIndex = 8;
            this.lblLeftFeature.Text = "特色介紹";
            this.pnlLeft.Controls.Add(this.lblLeftFeature);

            // ===== 右邊面板 =====
            this.pnlRight.BackColor = System.Drawing.Color.White;
            this.pnlRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRight.Location = new System.Drawing.Point(280, 62);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(220, 338);
            this.pnlRight.TabIndex = 9;

            // picRightImage
            this.picRightImage.BackColor = System.Drawing.Color.Gainsboro;
            this.picRightImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picRightImage.Location = new System.Drawing.Point(10, 10);
            this.picRightImage.Name = "picRightImage";
            this.picRightImage.Size = new System.Drawing.Size(198, 100);
            this.picRightImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picRightImage.TabIndex = 10;
            this.picRightImage.TabStop = false;
            this.pnlRight.Controls.Add(this.picRightImage);

            // lblRightName
            this.lblRightName.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRightName.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblRightName.Location = new System.Drawing.Point(10, 115);
            this.lblRightName.Name = "lblRightName";
            this.lblRightName.Size = new System.Drawing.Size(200, 30);
            this.lblRightName.TabIndex = 11;
            this.lblRightName.Text = "餐廳名稱";
            this.lblRightName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.pnlRight.Controls.Add(this.lblRightName);

            // lblRightFoodType
            this.lblRightFoodType.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRightFoodType.ForeColor = System.Drawing.Color.Gray;
            this.lblRightFoodType.Location = new System.Drawing.Point(10, 145);
            this.lblRightFoodType.Name = "lblRightFoodType";
            this.lblRightFoodType.Size = new System.Drawing.Size(200, 35);
            this.lblRightFoodType.TabIndex = 12;
            this.lblRightFoodType.Text = "食物種類";
            this.pnlRight.Controls.Add(this.lblRightFoodType);

            // lblRightPhone
            this.lblRightPhone.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRightPhone.Location = new System.Drawing.Point(10, 182);
            this.lblRightPhone.Name = "lblRightPhone";
            this.lblRightPhone.Size = new System.Drawing.Size(200, 25);
            this.lblRightPhone.TabIndex = 13;
            this.lblRightPhone.Text = "電話: 05-xxxxx";
            this.pnlRight.Controls.Add(this.lblRightPhone);

            // lblRightHours
            this.lblRightHours.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRightHours.Location = new System.Drawing.Point(10, 207);
            this.lblRightHours.Name = "lblRightHours";
            this.lblRightHours.Size = new System.Drawing.Size(200, 25);
            this.lblRightHours.TabIndex = 14;
            this.lblRightHours.Text = "營業: 11:00-21:00";
            this.pnlRight.Controls.Add(this.lblRightHours);

            // lblRightFeature
            this.lblRightFeature.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRightFeature.Location = new System.Drawing.Point(10, 232);
            this.lblRightFeature.Name = "lblRightFeature";
            this.lblRightFeature.Size = new System.Drawing.Size(200, 95);
            this.lblRightFeature.TabIndex = 15;
            this.lblRightFeature.Text = "特色介紹";
            this.pnlRight.Controls.Add(this.lblRightFeature);

            // btnSelectLeft
            this.btnSelectLeft.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSelectLeft.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSelectLeft.ForeColor = System.Drawing.Color.White;
            this.btnSelectLeft.Location = new System.Drawing.Point(20, 410);
            this.btnSelectLeft.Name = "btnSelectLeft";
            this.btnSelectLeft.Size = new System.Drawing.Size(220, 50);
            this.btnSelectLeft.TabIndex = 16;
            this.btnSelectLeft.Text = "選左 ← 左邊";
            this.btnSelectLeft.UseVisualStyleBackColor = false;
            this.btnSelectLeft.Click += new System.EventHandler(this.btnSelectLeft_Click);

            // btnSelectRight
            this.btnSelectRight.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSelectRight.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSelectRight.ForeColor = System.Drawing.Color.White;
            this.btnSelectRight.Location = new System.Drawing.Point(280, 410);
            this.btnSelectRight.Name = "btnSelectRight";
            this.btnSelectRight.Size = new System.Drawing.Size(220, 50);
            this.btnSelectRight.TabIndex = 17;
            this.btnSelectRight.Text = "選右 → 右邊";
            this.btnSelectRight.UseVisualStyleBackColor = false;
            this.btnSelectRight.Click += new System.EventHandler(this.btnSelectRight_Click);

            // SwipeForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(520, 480);
            this.Controls.Add(this.btnSelectRight);
            this.Controls.Add(this.btnSelectLeft);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SwipeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "選擇餐廳";
            this.Load += new System.EventHandler(this.SwipeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblProgress;

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.PictureBox picLeftImage;
        private System.Windows.Forms.Label lblLeftName;
        private System.Windows.Forms.Label lblLeftPhone;
        private System.Windows.Forms.Label lblLeftHours;
        private System.Windows.Forms.Label lblLeftFeature;
        private System.Windows.Forms.Label lblLeftFoodType;

        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.PictureBox picRightImage;
        private System.Windows.Forms.Label lblRightName;
        private System.Windows.Forms.Label lblRightPhone;
        private System.Windows.Forms.Label lblRightHours;
        private System.Windows.Forms.Label lblRightFeature;
        private System.Windows.Forms.Label lblRightFoodType;

        private System.Windows.Forms.Button btnSelectLeft;
        private System.Windows.Forms.Button btnSelectRight;
    }
}

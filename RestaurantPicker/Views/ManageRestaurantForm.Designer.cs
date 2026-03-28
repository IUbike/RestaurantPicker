namespace RestaurantPicker.Views
{
    partial class ManageRestaurantForm
    {
        private System.ComponentModel.IContainer components = null;

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
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblPriceRange = new System.Windows.Forms.Label();
            this.txtPriceRange = new System.Windows.Forms.TextBox();
            this.lblFoodTypes = new System.Windows.Forms.Label();
            this.txtFoodTypes = new System.Windows.Forms.TextBox();
            this.lblCuisineStyle = new System.Windows.Forms.Label();
            this.txtCuisineStyle = new System.Windows.Forms.TextBox();
            this.lblPurposes = new System.Windows.Forms.Label();
            this.txtPurposes = new System.Windows.Forms.TextBox();
            this.lblFeature = new System.Windows.Forms.Label();
            this.txtFeature = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblBusinessHours = new System.Windows.Forms.Label();
            this.txtBusinessHours = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblImageFileName = new System.Windows.Forms.Label();
            this.txtImageFileName = new System.Windows.Forms.TextBox();
            this.lblMealTime = new System.Windows.Forms.Label();
            this.chkBreakfast = new System.Windows.Forms.CheckBox();
            this.chkLunch = new System.Windows.Forms.CheckBox();
            this.chkDinner = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(194, 24);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "新增餐廳（寫入 CSV）";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(22, 55);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(65, 12);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "餐廳名稱*";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(120, 52);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(360, 22);
            this.txtName.TabIndex = 2;
            // 
            // lblPriceRange
            // 
            this.lblPriceRange.AutoSize = true;
            this.lblPriceRange.Location = new System.Drawing.Point(22, 86);
            this.lblPriceRange.Name = "lblPriceRange";
            this.lblPriceRange.Size = new System.Drawing.Size(53, 12);
            this.lblPriceRange.TabIndex = 3;
            this.lblPriceRange.Text = "價格區間";
            // 
            // txtPriceRange
            // 
            this.txtPriceRange.Location = new System.Drawing.Point(120, 83);
            this.txtPriceRange.Name = "txtPriceRange";
            this.txtPriceRange.Size = new System.Drawing.Size(360, 22);
            this.txtPriceRange.TabIndex = 4;
            // 
            // lblFoodTypes
            // 
            this.lblFoodTypes.AutoSize = true;
            this.lblFoodTypes.Location = new System.Drawing.Point(22, 117);
            this.lblFoodTypes.Name = "lblFoodTypes";
            this.lblFoodTypes.Size = new System.Drawing.Size(65, 12);
            this.lblFoodTypes.TabIndex = 5;
            this.lblFoodTypes.Text = "食物種類*";
            // 
            // txtFoodTypes
            // 
            this.txtFoodTypes.Location = new System.Drawing.Point(120, 114);
            this.txtFoodTypes.Name = "txtFoodTypes";
            this.txtFoodTypes.Size = new System.Drawing.Size(360, 22);
            this.txtFoodTypes.TabIndex = 6;
            // 
            // lblCuisineStyle
            // 
            this.lblCuisineStyle.AutoSize = true;
            this.lblCuisineStyle.Location = new System.Drawing.Point(22, 148);
            this.lblCuisineStyle.Name = "lblCuisineStyle";
            this.lblCuisineStyle.Size = new System.Drawing.Size(53, 12);
            this.lblCuisineStyle.TabIndex = 7;
            this.lblCuisineStyle.Text = "料理風格";
            // 
            // txtCuisineStyle
            // 
            this.txtCuisineStyle.Location = new System.Drawing.Point(120, 145);
            this.txtCuisineStyle.Name = "txtCuisineStyle";
            this.txtCuisineStyle.Size = new System.Drawing.Size(360, 22);
            this.txtCuisineStyle.TabIndex = 8;
            // 
            // lblPurposes
            // 
            this.lblPurposes.AutoSize = true;
            this.lblPurposes.Location = new System.Drawing.Point(22, 179);
            this.lblPurposes.Name = "lblPurposes";
            this.lblPurposes.Size = new System.Drawing.Size(53, 12);
            this.lblPurposes.TabIndex = 9;
            this.lblPurposes.Text = "用途標籤";
            // 
            // txtPurposes
            // 
            this.txtPurposes.Location = new System.Drawing.Point(120, 176);
            this.txtPurposes.Name = "txtPurposes";
            this.txtPurposes.Size = new System.Drawing.Size(360, 22);
            this.txtPurposes.TabIndex = 10;
            // 
            // lblFeature
            // 
            this.lblFeature.AutoSize = true;
            this.lblFeature.Location = new System.Drawing.Point(22, 210);
            this.lblFeature.Name = "lblFeature";
            this.lblFeature.Size = new System.Drawing.Size(53, 12);
            this.lblFeature.TabIndex = 11;
            this.lblFeature.Text = "餐廳特色";
            // 
            // txtFeature
            // 
            this.txtFeature.Location = new System.Drawing.Point(120, 207);
            this.txtFeature.Multiline = true;
            this.txtFeature.Name = "txtFeature";
            this.txtFeature.Size = new System.Drawing.Size(360, 50);
            this.txtFeature.TabIndex = 12;
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(22, 269);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(29, 12);
            this.lblPhone.TabIndex = 13;
            this.lblPhone.Text = "電話";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(120, 266);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(360, 22);
            this.txtPhone.TabIndex = 14;
            // 
            // lblBusinessHours
            // 
            this.lblBusinessHours.AutoSize = true;
            this.lblBusinessHours.Location = new System.Drawing.Point(22, 300);
            this.lblBusinessHours.Name = "lblBusinessHours";
            this.lblBusinessHours.Size = new System.Drawing.Size(53, 12);
            this.lblBusinessHours.TabIndex = 15;
            this.lblBusinessHours.Text = "營業時間";
            // 
            // txtBusinessHours
            // 
            this.txtBusinessHours.Location = new System.Drawing.Point(120, 297);
            this.txtBusinessHours.Name = "txtBusinessHours";
            this.txtBusinessHours.Size = new System.Drawing.Size(360, 22);
            this.txtBusinessHours.TabIndex = 16;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(22, 331);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(29, 12);
            this.lblAddress.TabIndex = 17;
            this.lblAddress.Text = "地址";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(120, 328);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(360, 22);
            this.txtAddress.TabIndex = 18;
            // 
            // lblImageFileName
            // 
            this.lblImageFileName.AutoSize = true;
            this.lblImageFileName.Location = new System.Drawing.Point(22, 362);
            this.lblImageFileName.Name = "lblImageFileName";
            this.lblImageFileName.Size = new System.Drawing.Size(67, 12);
            this.lblImageFileName.TabIndex = 19;
            this.lblImageFileName.Text = "圖片檔名(jpg)";
            // 
            // txtImageFileName
            // 
            this.txtImageFileName.Location = new System.Drawing.Point(120, 359);
            this.txtImageFileName.Name = "txtImageFileName";
            this.txtImageFileName.Size = new System.Drawing.Size(360, 22);
            this.txtImageFileName.TabIndex = 20;
            // 
            // lblMealTime
            // 
            this.lblMealTime.AutoSize = true;
            this.lblMealTime.Location = new System.Drawing.Point(22, 394);
            this.lblMealTime.Name = "lblMealTime";
            this.lblMealTime.Size = new System.Drawing.Size(53, 12);
            this.lblMealTime.TabIndex = 21;
            this.lblMealTime.Text = "提供時段";
            // 
            // chkBreakfast
            // 
            this.chkBreakfast.AutoSize = true;
            this.chkBreakfast.Location = new System.Drawing.Point(120, 392);
            this.chkBreakfast.Name = "chkBreakfast";
            this.chkBreakfast.Size = new System.Drawing.Size(48, 16);
            this.chkBreakfast.TabIndex = 22;
            this.chkBreakfast.Text = "早餐";
            this.chkBreakfast.UseVisualStyleBackColor = true;
            // 
            // chkLunch
            // 
            this.chkLunch.AutoSize = true;
            this.chkLunch.Location = new System.Drawing.Point(190, 392);
            this.chkLunch.Name = "chkLunch";
            this.chkLunch.Size = new System.Drawing.Size(48, 16);
            this.chkLunch.TabIndex = 23;
            this.chkLunch.Text = "午餐";
            this.chkLunch.UseVisualStyleBackColor = true;
            // 
            // chkDinner
            // 
            this.chkDinner.AutoSize = true;
            this.chkDinner.Location = new System.Drawing.Point(260, 392);
            this.chkDinner.Name = "chkDinner";
            this.chkDinner.Size = new System.Drawing.Size(48, 16);
            this.chkDinner.TabIndex = 24;
            this.chkDinner.Text = "晚餐";
            this.chkDinner.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.SeaGreen;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(324, 430);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 32);
            this.btnSave.TabIndex = 25;
            this.btnSave.Text = "儲存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(405, 430);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 32);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ManageRestaurantForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 480);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkDinner);
            this.Controls.Add(this.chkLunch);
            this.Controls.Add(this.chkBreakfast);
            this.Controls.Add(this.lblMealTime);
            this.Controls.Add(this.txtImageFileName);
            this.Controls.Add(this.lblImageFileName);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtBusinessHours);
            this.Controls.Add(this.lblBusinessHours);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.txtFeature);
            this.Controls.Add(this.lblFeature);
            this.Controls.Add(this.txtPurposes);
            this.Controls.Add(this.lblPurposes);
            this.Controls.Add(this.txtCuisineStyle);
            this.Controls.Add(this.lblCuisineStyle);
            this.Controls.Add(this.txtFoodTypes);
            this.Controls.Add(this.lblFoodTypes);
            this.Controls.Add(this.txtPriceRange);
            this.Controls.Add(this.lblPriceRange);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageRestaurantForm";
            this.Text = "管理餐廳";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblPriceRange;
        private System.Windows.Forms.TextBox txtPriceRange;
        private System.Windows.Forms.Label lblFoodTypes;
        private System.Windows.Forms.TextBox txtFoodTypes;
        private System.Windows.Forms.Label lblCuisineStyle;
        private System.Windows.Forms.TextBox txtCuisineStyle;
        private System.Windows.Forms.Label lblPurposes;
        private System.Windows.Forms.TextBox txtPurposes;
        private System.Windows.Forms.Label lblFeature;
        private System.Windows.Forms.TextBox txtFeature;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblBusinessHours;
        private System.Windows.Forms.TextBox txtBusinessHours;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblImageFileName;
        private System.Windows.Forms.TextBox txtImageFileName;
        private System.Windows.Forms.Label lblMealTime;
        private System.Windows.Forms.CheckBox chkBreakfast;
        private System.Windows.Forms.CheckBox chkLunch;
        private System.Windows.Forms.CheckBox chkDinner;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}

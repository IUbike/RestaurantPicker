namespace RestaurantPicker.Views
{
    partial class MealSelectForm
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
            this.rbBreakfast = new System.Windows.Forms.RadioButton();
            this.rbLunch = new System.Windows.Forms.RadioButton();
            this.rbDinner = new System.Windows.Forms.RadioButton();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBoxMealTime = new System.Windows.Forms.GroupBox();
            this.lblRangeTitle = new System.Windows.Forms.Label();
            this.cbMinTime = new System.Windows.Forms.ComboBox();
            this.cbMaxTime = new System.Windows.Forms.ComboBox();
            this.lblRangeSeparator = new System.Windows.Forms.Label();
            this.lblHint = new System.Windows.Forms.Label();
            this.groupBoxMealTime.SuspendLayout();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微軟正黑體", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTitle.Location = new System.Drawing.Point(35, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(332, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "先選擇預計吃飯時間（可再微調）";
            
            // groupBoxMealTime
            this.groupBoxMealTime.Controls.Add(this.rbBreakfast);
            this.groupBoxMealTime.Controls.Add(this.rbLunch);
            this.groupBoxMealTime.Controls.Add(this.rbDinner);
            this.groupBoxMealTime.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBoxMealTime.Location = new System.Drawing.Point(40, 52);
            this.groupBoxMealTime.Name = "groupBoxMealTime";
            this.groupBoxMealTime.Size = new System.Drawing.Size(320, 112);
            this.groupBoxMealTime.TabIndex = 1;
            this.groupBoxMealTime.TabStop = false;
            this.groupBoxMealTime.Text = "快速預設";
            
            // rbBreakfast
            this.rbBreakfast.AutoSize = true;
            this.rbBreakfast.Location = new System.Drawing.Point(20, 32);
            this.rbBreakfast.Name = "rbBreakfast";
            this.rbBreakfast.Size = new System.Drawing.Size(58, 23);
            this.rbBreakfast.TabIndex = 2;
            this.rbBreakfast.TabStop = true;
            this.rbBreakfast.Text = "早餐";
            this.rbBreakfast.UseVisualStyleBackColor = true;
            this.rbBreakfast.CheckedChanged += new System.EventHandler(this.MealPreset_CheckedChanged);
            
            // rbLunch
            this.rbLunch.AutoSize = true;
            this.rbLunch.Location = new System.Drawing.Point(120, 32);
            this.rbLunch.Name = "rbLunch";
            this.rbLunch.Size = new System.Drawing.Size(58, 23);
            this.rbLunch.TabIndex = 3;
            this.rbLunch.TabStop = true;
            this.rbLunch.Text = "午餐";
            this.rbLunch.UseVisualStyleBackColor = true;
            this.rbLunch.CheckedChanged += new System.EventHandler(this.MealPreset_CheckedChanged);
            
            // rbDinner
            this.rbDinner.AutoSize = true;
            this.rbDinner.Location = new System.Drawing.Point(220, 32);
            this.rbDinner.Name = "rbDinner";
            this.rbDinner.Size = new System.Drawing.Size(58, 23);
            this.rbDinner.TabIndex = 4;
            this.rbDinner.TabStop = true;
            this.rbDinner.Text = "晚餐";
            this.rbDinner.UseVisualStyleBackColor = true;
            this.rbDinner.CheckedChanged += new System.EventHandler(this.MealPreset_CheckedChanged);
            
            // lblRangeTitle
            this.lblRangeTitle.AutoSize = true;
            this.lblRangeTitle.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRangeTitle.Location = new System.Drawing.Point(40, 177);
            this.lblRangeTitle.Name = "lblRangeTitle";
            this.lblRangeTitle.Size = new System.Drawing.Size(262, 18);
            this.lblRangeTitle.TabIndex = 5;
            this.lblRangeTitle.Text = "用餐時間範圍（30 分鐘為單位，含邊界）";
            
            // cbMinTime
            this.cbMinTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMinTime.FormattingEnabled = true;
            this.cbMinTime.Location = new System.Drawing.Point(44, 201);
            this.cbMinTime.Name = "cbMinTime";
            this.cbMinTime.Size = new System.Drawing.Size(110, 21);
            this.cbMinTime.TabIndex = 6;
            
            // lblRangeSeparator
            this.lblRangeSeparator.AutoSize = true;
            this.lblRangeSeparator.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRangeSeparator.Location = new System.Drawing.Point(162, 201);
            this.lblRangeSeparator.Name = "lblRangeSeparator";
            this.lblRangeSeparator.Size = new System.Drawing.Size(22, 19);
            this.lblRangeSeparator.TabIndex = 7;
            this.lblRangeSeparator.Text = "~";
            
            // cbMaxTime
            this.cbMaxTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMaxTime.FormattingEnabled = true;
            this.cbMaxTime.Location = new System.Drawing.Point(191, 201);
            this.cbMaxTime.Name = "cbMaxTime";
            this.cbMaxTime.Size = new System.Drawing.Size(110, 21);
            this.cbMaxTime.TabIndex = 8;
            
            // lblHint
            this.lblHint.AutoSize = true;
            this.lblHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblHint.Location = new System.Drawing.Point(40, 230);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(277, 13);
            this.lblHint.TabIndex = 9;
            this.lblHint.Text = "提示：系統先用時間範圍篩選，再用分類標籤細篩。";
            
            // btnNext
            this.btnNext.BackColor = System.Drawing.Color.Green;
            this.btnNext.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Location = new System.Drawing.Point(188, 262);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(80, 40);
            this.btnNext.TabIndex = 10;
            this.btnNext.Text = "下一步";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            
            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(280, 262);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 40);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // MealSelectForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(400, 320);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.cbMaxTime);
            this.Controls.Add(this.lblRangeSeparator);
            this.Controls.Add(this.cbMinTime);
            this.Controls.Add(this.lblRangeTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.groupBoxMealTime);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MealSelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "選擇用餐時段";
            this.Load += new System.EventHandler(this.MealSelectForm_Load);
            this.groupBoxMealTime.ResumeLayout(false);
            this.groupBoxMealTime.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.RadioButton rbBreakfast;
        private System.Windows.Forms.RadioButton rbLunch;
        private System.Windows.Forms.RadioButton rbDinner;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBoxMealTime;
        private System.Windows.Forms.Label lblRangeTitle;
        private System.Windows.Forms.ComboBox cbMinTime;
        private System.Windows.Forms.ComboBox cbMaxTime;
        private System.Windows.Forms.Label lblRangeSeparator;
        private System.Windows.Forms.Label lblHint;
    }
}

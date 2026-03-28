namespace RestaurantPicker.Views
{
    partial class CategorySelectForm
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
            this.rbSpecific = new System.Windows.Forms.RadioButton();
            this.rbRandom = new System.Windows.Forms.RadioButton();
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBoxCategory = new System.Windows.Forms.GroupBox();
            this.lblSelectedTitle = new System.Windows.Forms.Label();
            this.pnlSelectedTags = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCandidateCount = new System.Windows.Forms.Label();
            this.lblTagHint = new System.Windows.Forms.Label();
            this.btnClearTags = new System.Windows.Forms.Button();
            this.groupBoxCategory.SuspendLayout();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微軟正黑體", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblTitle.Location = new System.Drawing.Point(28, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(345, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "請選擇食物種類（支援複選標籤）";
            
            // groupBoxCategory
            this.groupBoxCategory.Controls.Add(this.btnAddTag);
            this.groupBoxCategory.Controls.Add(this.rbSpecific);
            this.groupBoxCategory.Controls.Add(this.cbCategory);
            this.groupBoxCategory.Controls.Add(this.rbRandom);
            this.groupBoxCategory.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBoxCategory.Location = new System.Drawing.Point(33, 59);
            this.groupBoxCategory.Name = "groupBoxCategory";
            this.groupBoxCategory.Size = new System.Drawing.Size(446, 122);
            this.groupBoxCategory.TabIndex = 1;
            this.groupBoxCategory.TabStop = false;
            this.groupBoxCategory.Text = "種類選擇";
            
            // rbSpecific
            this.rbSpecific.AutoSize = true;
            this.rbSpecific.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbSpecific.Location = new System.Drawing.Point(20, 34);
            this.rbSpecific.Name = "rbSpecific";
            this.rbSpecific.Size = new System.Drawing.Size(90, 24);
            this.rbSpecific.TabIndex = 2;
            this.rbSpecific.TabStop = true;
            this.rbSpecific.Text = "指定種類";
            this.rbSpecific.UseVisualStyleBackColor = true;
            this.rbSpecific.CheckedChanged += new System.EventHandler(this.rbSpecific_CheckedChanged);
            
            // cbCategory
            this.cbCategory.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbCategory.FormattingEnabled = true;
            this.cbCategory.Location = new System.Drawing.Point(116, 33);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(212, 28);
            this.cbCategory.TabIndex = 3;
            
            // btnAddTag
            this.btnAddTag.BackColor = System.Drawing.Color.SteelBlue;
            this.btnAddTag.ForeColor = System.Drawing.Color.White;
            this.btnAddTag.Location = new System.Drawing.Point(334, 32);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(92, 30);
            this.btnAddTag.TabIndex = 4;
            this.btnAddTag.Text = "加入標籤";
            this.btnAddTag.UseVisualStyleBackColor = false;
            this.btnAddTag.Click += new System.EventHandler(this.btnAddTag_Click);
            
            // rbRandom
            this.rbRandom.AutoSize = true;
            this.rbRandom.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rbRandom.Location = new System.Drawing.Point(20, 78);
            this.rbRandom.Name = "rbRandom";
            this.rbRandom.Size = new System.Drawing.Size(90, 24);
            this.rbRandom.TabIndex = 5;
            this.rbRandom.TabStop = true;
            this.rbRandom.Text = "隨機種類";
            this.rbRandom.UseVisualStyleBackColor = true;
            this.rbRandom.CheckedChanged += new System.EventHandler(this.rbRandom_CheckedChanged);
            
            // lblSelectedTitle
            this.lblSelectedTitle.AutoSize = true;
            this.lblSelectedTitle.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblSelectedTitle.Location = new System.Drawing.Point(33, 191);
            this.lblSelectedTitle.Name = "lblSelectedTitle";
            this.lblSelectedTitle.Size = new System.Drawing.Size(106, 18);
            this.lblSelectedTitle.TabIndex = 2;
            this.lblSelectedTitle.Text = "已選擇標籤：";
            
            // btnClearTags
            this.btnClearTags.Location = new System.Drawing.Point(402, 187);
            this.btnClearTags.Name = "btnClearTags";
            this.btnClearTags.Size = new System.Drawing.Size(77, 24);
            this.btnClearTags.TabIndex = 3;
            this.btnClearTags.Text = "清空標籤";
            this.btnClearTags.UseVisualStyleBackColor = true;
            this.btnClearTags.Click += new System.EventHandler(this.btnClearTags_Click);
            
            // pnlSelectedTags
            this.pnlSelectedTags.AutoScroll = true;
            this.pnlSelectedTags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSelectedTags.Location = new System.Drawing.Point(33, 214);
            this.pnlSelectedTags.Name = "pnlSelectedTags";
            this.pnlSelectedTags.Size = new System.Drawing.Size(446, 76);
            this.pnlSelectedTags.TabIndex = 4;
            
            // lblTagHint
            this.lblTagHint.AutoSize = true;
            this.lblTagHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblTagHint.Location = new System.Drawing.Point(33, 293);
            this.lblTagHint.Name = "lblTagHint";
            this.lblTagHint.Size = new System.Drawing.Size(278, 13);
            this.lblTagHint.TabIndex = 5;
            this.lblTagHint.Text = "提示：點選標籤上的 × 可移除，或按「清空標籤」重選";
            
            // lblCandidateCount
            this.lblCandidateCount.AutoSize = true;
            this.lblCandidateCount.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblCandidateCount.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblCandidateCount.Location = new System.Drawing.Point(33, 313);
            this.lblCandidateCount.Name = "lblCandidateCount";
            this.lblCandidateCount.Size = new System.Drawing.Size(129, 19);
            this.lblCandidateCount.TabIndex = 6;
            this.lblCandidateCount.Text = "候選餐廳數量：0";
            
            // btnNext
            this.btnNext.BackColor = System.Drawing.Color.Green;
            this.btnNext.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Location = new System.Drawing.Point(303, 343);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(84, 40);
            this.btnNext.TabIndex = 7;
            this.btnNext.Text = "下一步";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            
            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(395, 343);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 40);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // CategorySelectForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(516, 396);
            this.Controls.Add(this.lblCandidateCount);
            this.Controls.Add(this.lblTagHint);
            this.Controls.Add(this.btnClearTags);
            this.Controls.Add(this.pnlSelectedTags);
            this.Controls.Add(this.lblSelectedTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.groupBoxCategory);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CategorySelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "選擇食物種類";
            this.Load += new System.EventHandler(this.CategorySelectForm_Load);
            this.groupBoxCategory.ResumeLayout(false);
            this.groupBoxCategory.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.RadioButton rbSpecific;
        private System.Windows.Forms.RadioButton rbRandom;
        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.Button btnAddTag;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBoxCategory;
        private System.Windows.Forms.Label lblSelectedTitle;
        private System.Windows.Forms.FlowLayoutPanel pnlSelectedTags;
        private System.Windows.Forms.Label lblCandidateCount;
        private System.Windows.Forms.Label lblTagHint;
        private System.Windows.Forms.Button btnClearTags;
    }
}

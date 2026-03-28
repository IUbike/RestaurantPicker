namespace RestaurantPicker.Views
{
    partial class ManagePreferenceForm
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
            this.lblAll = new System.Windows.Forms.Label();
            this.lstAllRestaurants = new System.Windows.Forms.ListBox();
            this.lblFavorites = new System.Windows.Forms.Label();
            this.lstFavorites = new System.Windows.Forms.ListBox();
            this.lblBlocked = new System.Windows.Forms.Label();
            this.lstBlocked = new System.Windows.Forms.ListBox();
            this.btnAddFavorite = new System.Windows.Forms.Button();
            this.btnAddBlocked = new System.Windows.Forms.Button();
            this.btnRemoveFavorite = new System.Windows.Forms.Button();
            this.btnRemoveBlocked = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblAll
            // 
            this.lblAll.AutoSize = true;
            this.lblAll.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblAll.Location = new System.Drawing.Point(18, 14);
            this.lblAll.Name = "lblAll";
            this.lblAll.Size = new System.Drawing.Size(84, 19);
            this.lblAll.TabIndex = 0;
            this.lblAll.Text = "餐廳總清單";
            // 
            // lstAllRestaurants
            // 
            this.lstAllRestaurants.FormattingEnabled = true;
            this.lstAllRestaurants.ItemHeight = 12;
            this.lstAllRestaurants.Location = new System.Drawing.Point(22, 38);
            this.lstAllRestaurants.Name = "lstAllRestaurants";
            this.lstAllRestaurants.Size = new System.Drawing.Size(250, 304);
            this.lstAllRestaurants.TabIndex = 1;
            // 
            // lblFavorites
            // 
            this.lblFavorites.AutoSize = true;
            this.lblFavorites.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblFavorites.Location = new System.Drawing.Point(387, 14);
            this.lblFavorites.Name = "lblFavorites";
            this.lblFavorites.Size = new System.Drawing.Size(69, 19);
            this.lblFavorites.TabIndex = 2;
            this.lblFavorites.Text = "收藏清單";
            // 
            // lstFavorites
            // 
            this.lstFavorites.FormattingEnabled = true;
            this.lstFavorites.ItemHeight = 12;
            this.lstFavorites.Location = new System.Drawing.Point(391, 38);
            this.lstFavorites.Name = "lstFavorites";
            this.lstFavorites.Size = new System.Drawing.Size(250, 148);
            this.lstFavorites.TabIndex = 3;
            // 
            // lblBlocked
            // 
            this.lblBlocked.AutoSize = true;
            this.lblBlocked.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblBlocked.Location = new System.Drawing.Point(387, 207);
            this.lblBlocked.Name = "lblBlocked";
            this.lblBlocked.Size = new System.Drawing.Size(69, 19);
            this.lblBlocked.TabIndex = 4;
            this.lblBlocked.Text = "封鎖清單";
            // 
            // lstBlocked
            // 
            this.lstBlocked.FormattingEnabled = true;
            this.lstBlocked.ItemHeight = 12;
            this.lstBlocked.Location = new System.Drawing.Point(391, 232);
            this.lstBlocked.Name = "lstBlocked";
            this.lstBlocked.Size = new System.Drawing.Size(250, 148);
            this.lstBlocked.TabIndex = 5;
            // 
            // btnAddFavorite
            // 
            this.btnAddFavorite.Location = new System.Drawing.Point(288, 90);
            this.btnAddFavorite.Name = "btnAddFavorite";
            this.btnAddFavorite.Size = new System.Drawing.Size(90, 30);
            this.btnAddFavorite.TabIndex = 6;
            this.btnAddFavorite.Text = "加到收藏 →";
            this.btnAddFavorite.UseVisualStyleBackColor = true;
            this.btnAddFavorite.Click += new System.EventHandler(this.btnAddFavorite_Click);
            // 
            // btnAddBlocked
            // 
            this.btnAddBlocked.Location = new System.Drawing.Point(288, 127);
            this.btnAddBlocked.Name = "btnAddBlocked";
            this.btnAddBlocked.Size = new System.Drawing.Size(90, 30);
            this.btnAddBlocked.TabIndex = 7;
            this.btnAddBlocked.Text = "加到封鎖 →";
            this.btnAddBlocked.UseVisualStyleBackColor = true;
            this.btnAddBlocked.Click += new System.EventHandler(this.btnAddBlocked_Click);
            // 
            // btnRemoveFavorite
            // 
            this.btnRemoveFavorite.Location = new System.Drawing.Point(647, 80);
            this.btnRemoveFavorite.Name = "btnRemoveFavorite";
            this.btnRemoveFavorite.Size = new System.Drawing.Size(95, 30);
            this.btnRemoveFavorite.TabIndex = 8;
            this.btnRemoveFavorite.Text = "移除收藏";
            this.btnRemoveFavorite.UseVisualStyleBackColor = true;
            this.btnRemoveFavorite.Click += new System.EventHandler(this.btnRemoveFavorite_Click);
            // 
            // btnRemoveBlocked
            // 
            this.btnRemoveBlocked.Location = new System.Drawing.Point(647, 274);
            this.btnRemoveBlocked.Name = "btnRemoveBlocked";
            this.btnRemoveBlocked.Size = new System.Drawing.Size(95, 30);
            this.btnRemoveBlocked.TabIndex = 9;
            this.btnRemoveBlocked.Text = "移除封鎖";
            this.btnRemoveBlocked.UseVisualStyleBackColor = true;
            this.btnRemoveBlocked.Click += new System.EventHandler(this.btnRemoveBlocked_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(647, 350);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 30);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "完成";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblHint.Location = new System.Drawing.Point(20, 355);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(260, 24);
            this.lblHint.TabIndex = 11;
            this.lblHint.Text = "提示：加入封鎖時會自動從收藏移除；\r\n加入收藏時也會自動從封鎖移除。";
            // 
            // ManagePreferenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 398);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRemoveBlocked);
            this.Controls.Add(this.btnRemoveFavorite);
            this.Controls.Add(this.btnAddBlocked);
            this.Controls.Add(this.btnAddFavorite);
            this.Controls.Add(this.lstBlocked);
            this.Controls.Add(this.lblBlocked);
            this.Controls.Add(this.lstFavorites);
            this.Controls.Add(this.lblFavorites);
            this.Controls.Add(this.lstAllRestaurants);
            this.Controls.Add(this.lblAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManagePreferenceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "管理收藏 / 封鎖清單";
            this.Load += new System.EventHandler(this.ManagePreferenceForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAll;
        private System.Windows.Forms.ListBox lstAllRestaurants;
        private System.Windows.Forms.Label lblFavorites;
        private System.Windows.Forms.ListBox lstFavorites;
        private System.Windows.Forms.Label lblBlocked;
        private System.Windows.Forms.ListBox lstBlocked;
        private System.Windows.Forms.Button btnAddFavorite;
        private System.Windows.Forms.Button btnAddBlocked;
        private System.Windows.Forms.Button btnRemoveFavorite;
        private System.Windows.Forms.Button btnRemoveBlocked;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblHint;
    }
}

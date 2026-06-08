using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RestaurantPicker.Models;
using RestaurantPicker.Services;
namespace RestaurantPicker.Views
{
    public class UserProfileForm : Form
    {
        private readonly List<string> _availableTags;
        private TextBox _txtNickname;
        private TextBox _txtAvatar;
        private CheckedListBox _tagList;
        private Button _btnSave;
        private Button _btnCancel;

        public UserProfile? CreatedUser { get; private set; }

        public UserProfileForm(List<string> availableTags)
        {
            _availableTags = availableTags ?? new List<string>();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Add User";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(480, 520);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var lblNickname = new Label
            {
                Text = "Nickname",
                Location = new Point(20, 20),
                AutoSize = true
            };

            _txtNickname = new TextBox
            {
                Location = new Point(20, 45),
                Width = 420
            };

            var lblAvatar = new Label
            {
                Text = "Avatar Path (Optional)",
                Location = new Point(20, 85),
                AutoSize = true
            };

            _txtAvatar = new TextBox
            {
                Location = new Point(20, 110),
                Width = 420
            };

            var lblTags = new Label
            {
                Text = "Preferred Tags",
                Location = new Point(20, 150),
                AutoSize = true
            };

            _tagList = new CheckedListBox
            {
                Location = new Point(20, 175),
                Size = new Size(420, 220)
            };

            foreach (var tag in _availableTags)
            {
                _tagList.Items.Add(LanguageManager.GetLocalizedTag(tag));
            }

            _btnSave = new Button
            {
                Text = "Save",
                Location = new Point(260, 420),
                Size = new Size(80, 32)
            };
            _btnSave.Click += Save_Click;

            _btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(360, 420),
                Size = new Size(80, 32)
            };
            _btnCancel.Click += Cancel_Click;

            Controls.Add(lblNickname);
            Controls.Add(_txtNickname);
            Controls.Add(lblAvatar);
            Controls.Add(_txtAvatar);
            Controls.Add(lblTags);
            Controls.Add(_tagList);
            Controls.Add(_btnSave);
            Controls.Add(_btnCancel);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            var nickname = _txtNickname.Text.Trim();
            if (string.IsNullOrWhiteSpace(nickname))
            {
                MessageBox.Show("Please enter a nickname", "Hint", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var tags = _tagList.CheckedItems
                .Cast<string>()
                .Select(tag => LanguageManager.GetChineseTag(tag))
                .ToList();
            CreatedUser = new UserProfile
            {
                Nickname = nickname,
                AvatarPath = _txtAvatar.Text.Trim(),
                PreferredTags = tags
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

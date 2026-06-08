using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using RestaurantPicker.Models;
using RestaurantPicker.Services;
namespace RestaurantPicker.Views
{
    public class UserProfileCard : UserControl
    {
        private readonly Panel _cardPanel;
        private readonly PictureBox _avatar;
        private readonly Label _nameLabel;
        private readonly FlowLayoutPanel _tagPanel;
        private bool _isHovered;
        private readonly Size _normalSize = new Size(220, 248);
        private readonly Size _hoverSize = new Size(236, 264);

        public UserProfile? Profile { get; }
        public bool IsAddCard { get; }

        public event EventHandler? CardClicked;

        public UserProfileCard(UserProfile? profile, bool isAddCard = false)
        {
            Profile = profile;
            IsAddCard = isAddCard;

            Size = _normalSize;
            Margin = new Padding(26);
            BackColor = Color.Transparent;

            _cardPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(32, 34, 40),
                Padding = new Padding(14),
                Margin = new Padding(14)
            };

            _avatar = new PictureBox
            {
                Size = new Size(88, 88),
                Location = new Point(66, 18),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            _nameLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(10, 118),
                Size = new Size(200, 32),
                AutoEllipsis = true
            };

            _tagPanel = new FlowLayoutPanel
            {
                Location = new Point(10, 156),
                Size = new Size(200, 72),
                AutoScroll = false,
                WrapContents = true
            };

            _cardPanel.Controls.Add(_avatar);
            _cardPanel.Controls.Add(_nameLabel);
            _cardPanel.Controls.Add(_tagPanel);
            Controls.Add(_cardPanel);

            ApplyContent();
            RegisterHoverEvents(this);
            RegisterHoverEvents(_cardPanel);
            RegisterHoverEvents(_avatar);
            RegisterHoverEvents(_nameLabel);
            RegisterHoverEvents(_tagPanel);

            Click += (_, _) => CardClicked?.Invoke(this, EventArgs.Empty);
            _cardPanel.Click += (_, _) => CardClicked?.Invoke(this, EventArgs.Empty);
            _avatar.Click += (_, _) => CardClicked?.Invoke(this, EventArgs.Empty);
            _nameLabel.Click += (_, _) => CardClicked?.Invoke(this, EventArgs.Empty);
            _tagPanel.Click += (_, _) => CardClicked?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var shadowBrush = new SolidBrush(Color.FromArgb(_isHovered ? 80 : 60, 0, 0, 0));
            using var path = CreateRoundedRectanglePath(new Rectangle(6, 6, Width - 12, Height - 12), 18);
            e.Graphics.FillPath(shadowBrush, path);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _cardPanel?.Invalidate();
        }

        private void ApplyContent()
        {
            if (IsAddCard)
            {
                _nameLabel.Text = "＋ Add User";
                _nameLabel.ForeColor = Color.FromArgb(255, 186, 88);
                _avatar.Image = CreateAddIcon();
                _tagPanel.Visible = false;
                return;
            }

            _nameLabel.Text = Profile?.Nickname ?? string.Empty;
            _avatar.Image = CreateAvatar(Profile);
            _tagPanel.Controls.Clear();

            var tags = Profile?.PreferredTags ?? new List<string>();
            foreach (var tag in tags.Take(2))
            {
                var displayTag = LanguageManager.GetLocalizedTag(tag);
                _tagPanel.Controls.Add(CreateTagChip(displayTag));
            }
        }

        private static Label CreateTagChip(string tag)
        {
            var color = GetTagColor(tag);
            return new Label
            {
                AutoSize = true,
                Text = tag,
                ForeColor = Color.White,
                BackColor = color,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Padding = new Padding(6, 2, 6, 2),
                Margin = new Padding(3, 2, 3, 2)
            };
        }

        private static Color GetTagColor(string tag)
        {
            var palette = new[]
            {
                Color.FromArgb(255, 128, 90),
                Color.FromArgb(255, 176, 90),
                Color.FromArgb(255, 210, 90),
                Color.FromArgb(255, 120, 140),
                Color.FromArgb(255, 150, 210)
            };

            var index = Math.Abs(tag.GetHashCode()) % palette.Length;
            return palette[index];
        }

        private Image CreateAvatar(UserProfile? profile)
        {
            if (profile == null)
            {
                return CreateCircleAvatar("?");
            }

            if (!string.IsNullOrWhiteSpace(profile.AvatarPath) && System.IO.File.Exists(profile.AvatarPath))
            {
                using var fs = new System.IO.FileStream(profile.AvatarPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                using var temp = Image.FromStream(fs);
                return new Bitmap(temp, new Size(88, 88));
            }

            var initial = string.IsNullOrWhiteSpace(profile.Nickname) ? "?" : profile.Nickname.Substring(0, 1).ToUpperInvariant();
            return CreateCircleAvatar(initial);
        }

        private Image CreateCircleAvatar(string text)
        {
            var bitmap = new Bitmap(88, 88);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var colorSeed = string.IsNullOrWhiteSpace(text) ? "?" : text;
            var color = GetTagColor(colorSeed);
            using var brush = new SolidBrush(color);
            graphics.FillEllipse(brush, 0, 0, 88, 88);

            using var font = new Font("Segoe UI", 22F, FontStyle.Bold);
            using var textBrush = new SolidBrush(Color.White);
            var size = graphics.MeasureString(text, font);
            graphics.DrawString(text, font, textBrush, (88 - size.Width) / 2, (88 - size.Height) / 2 - 2);
            return bitmap;
        }

        private Image CreateAddIcon()
        {
            var bitmap = new Bitmap(88, 88);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(255, 186, 88), 6);
            graphics.DrawEllipse(pen, 6, 6, 76, 76);
            graphics.DrawLine(pen, 44, 24, 44, 64);
            graphics.DrawLine(pen, 24, 44, 64, 44);
            return bitmap;
        }

        private static GraphicsPath CreateRoundedRectanglePath(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void RegisterHoverEvents(Control control)
        {
            control.MouseEnter += (_, _) => ApplyHover(true);
            control.MouseLeave += (_, _) => ApplyHover(false);
        }

        private void ApplyHover(bool isHovered)
        {
            _isHovered = isHovered;
            _cardPanel.BackColor = isHovered ? Color.FromArgb(45, 47, 56) : Color.FromArgb(32, 34, 40);
            Size = isHovered ? _hoverSize : _normalSize;
            Invalidate();
        }
    }
}

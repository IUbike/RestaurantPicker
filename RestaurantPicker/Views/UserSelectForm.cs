using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using RestaurantPicker.Models;
using RestaurantPicker.Repositories;
using RestaurantPicker.Services;

namespace RestaurantPicker.Views
{
    public class UserSelectForm : Form
    {
        private readonly UserProfileService _userProfileService;
        private readonly IRestaurantRepository _restaurantRepository;
        private FlowLayoutPanel _userPanel;
        private Label _lblTitle;
        private Label _lblSubtitle;
        private Label _lblWelcome;
        private Timer _fadeTimer;
        private Timer _welcomeTimer;
        private Panel _gridPanel;

        public UserProfile? SelectedUser { get; private set; }

        public UserSelectForm(UserProfileService userProfileService, IRestaurantRepository restaurantRepository)
        {
            _userProfileService = userProfileService;
            _restaurantRepository = restaurantRepository;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "選擇使用者";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1280, 720);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(18, 19, 24);
            Opacity = 0d;

            _lblTitle = new Label
            {
                Text = "誰正在使用？",
                Font = new Font("Segoe UI", 28F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };

            _lblSubtitle = new Label
            {
                Text = "選擇個人檔案以開始探索餐廳",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(180, 180, 190),
                AutoSize = true
            };

            _gridPanel = new Panel
            {
                BackColor = Color.Transparent
            };

            _lblWelcome = new Label
            {
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 200, 120),
                AutoSize = true,
                Visible = false
            };

            Controls.Add(_lblTitle);
            Controls.Add(_lblSubtitle);
            Controls.Add(_gridPanel);
            Controls.Add(_lblWelcome);

            Load += UserSelectForm_Load;
            Resize += UserSelectForm_Resize;
        }

        private void UserSelectForm_Load(object sender, EventArgs e)
        {
            SetupFadeIn();
            LoadUsers();
            LayoutControls();
        }

        private void UserSelectForm_Resize(object? sender, EventArgs e)
        {
            LayoutControls();
        }

        private void LoadUsers()
        {
            _gridPanel.Controls.Clear();
            var users = _userProfileService.LoadAll();

            var cards = new List<UserProfileCard>();
            foreach (var user in users.OrderBy(u => u.Nickname))
            {
                var card = new UserProfileCard(user);
                card.CardClicked += (_, _) => SelectUser(user);
                cards.Add(card);
            }

            var addCard = new UserProfileCard(null, true);
            addCard.CardClicked += (_, _) => AddUser_Click(this, EventArgs.Empty);
            cards.Add(addCard);

            ArrangeCards(cards);
        }

        private void ArrangeCards(List<UserProfileCard> cards)
        {
            _gridPanel.Controls.Clear();
            int columns = 3;
            int rows = 2;
            int cardWidth = cards.Count > 0 ? cards[0].Width : 220;
            int cardHeight = cards.Count > 0 ? cards[0].Height : 248;
            int gapX = 40;
            int gapY = 32;

            int totalWidth = columns * cardWidth + (columns - 1) * gapX;
            int totalHeight = rows * cardHeight + (rows - 1) * gapY;

            int panelX = (ClientSize.Width - totalWidth) / 2;
            int panelY = _lblSubtitle.Bottom + 40;

            _gridPanel.Location = new Point(panelX, panelY);
            _gridPanel.Size = new Size(totalWidth, totalHeight);

            for (int i = 0; i < rows * columns; i++)
            {
                if (i >= cards.Count)
                {
                    break;
                }

                int row = i / columns;
                int col = i % columns;
                var card = cards[i];
                card.Location = new Point(col * (cardWidth + gapX), row * (cardHeight + gapY));
                _gridPanel.Controls.Add(card);
            }

            CenterWelcomeLabel();
        }

        private void CenterWelcomeLabel()
        {
            _lblWelcome.Location = new Point((ClientSize.Width - _lblWelcome.Width) / 2, (ClientSize.Height - _lblWelcome.Height) / 2);
        }

        private void AddUser_Click(object sender, EventArgs e)
        {
            var tags = _userProfileService.GetAvailableTags(_restaurantRepository);
            using var form = new UserProfileForm(tags);
            if (form.ShowDialog() == DialogResult.OK && form.CreatedUser != null)
            {
                _userProfileService.AddUser(form.CreatedUser);
                LoadUsers();
            }
        }

        private void SelectUser(UserProfile user)
        {
            SelectedUser = user;
            _lblWelcome.Text = $"歡迎回來，{user.Nickname}";
            _lblWelcome.Visible = true;
            CenterWelcomeLabel();

            _welcomeTimer?.Stop();
            _welcomeTimer = new Timer { Interval = 1000 };
            _welcomeTimer.Tick += (_, _) =>
            {
                _welcomeTimer.Stop();
                DialogResult = DialogResult.OK;
                Close();
            };
            _welcomeTimer.Start();
        }

        private void SetupFadeIn()
        {
            _fadeTimer?.Stop();
            _fadeTimer = new Timer { Interval = 20 };
            _fadeTimer.Tick += (_, _) =>
            {
                Opacity = Math.Min(1d, Opacity + 0.08d);
                if (Opacity >= 1d)
                {
                    _fadeTimer.Stop();
                }
            };
            _fadeTimer.Start();
        }

        private void LayoutControls()
        {
            int centerX = ClientSize.Width / 2;
            _lblTitle.Location = new Point(centerX - _lblTitle.Width / 2, 72);
            _lblSubtitle.Location = new Point(centerX - _lblSubtitle.Width / 2, _lblTitle.Bottom + 10);

            var cards = _gridPanel.Controls.OfType<UserProfileCard>().ToList();
            if (cards.Count > 0)
            {
                ArrangeCards(cards);
            }
            else
            {
                CenterWelcomeLabel();
            }
        }
    }
}

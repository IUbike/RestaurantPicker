using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace RestaurantPicker.Services
{
    public enum LanguageType
    {
        Chinese,
        English
    }

    public static class LanguageManager
    {
        public static LanguageType CurrentLanguage { get; set; } = LanguageType.English;

        private static readonly HashSet<string> UniversalIcons = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "icons_add.png",
            "icons_ok.png",
            "icons_cancel.png",
            "icons_setting_e.png",
            "icons_block.png",
            "icons_star_gray.png",
            "icons_star_yellow.png"
        };

        private static readonly Dictionary<string, string> ChineseTranslations = new Dictionary<string, string>
        {
            // MainForm
            { "lblTitle", "不知道吃什麼？" },
            { "btnStart", "開始選餐" },
            { "btnTodayMeal", "今日餐廳" },
            { "btnManageRestaurant", "管理餐廳" },
            { "btnManagePreference", "收藏與封鎖" },
            { "btnReset", "重置紀錄" },
            { "btnBackToHome", "返回主頁" },
            { "langToggle", "English" },
            { "ratingTitle", "評分: " },
            { "notRatedYet", "[未評分] 點擊評分" },
            { "clickToAdd", "點擊新增" },
            { "resetConfirm", "確定要重置所有使用者資料嗎？這會清除評分、今日紀錄與收藏/封鎖，無法還原。" },
            { "resetTitle", "重置確認" },
            { "resetDone", "已重置所有使用者資料。" },
            { "resetDoneTitle", "完成" },
            { "resetFailed", "重置失敗: " },
            { "resetFailedTitle", "錯誤" },
            { "updateDone", "餐廳資料已更新。" },
            
            // MealSelectForm
            { "mealTitle", "先選擇預計吃飯時間（可再微調）" },
            { "mealPreset", "快速預設" },
            { "breakfast", "早餐" },
            { "lunch", "午餐" },
            { "dinner", "晚餐" },
            { "mealRange", "用餐時間範圍（30 分鐘為單位，含邊界）" },
            { "mealHint", "提示：系統先用時間範圍篩選，再用分類標籤細篩。" },
            { "btnNext", "下一步" },
            { "btnCancel", "取消" },
            { "invalidTimeRange", "請選擇有效的時間範圍" },
            { "minMaxTimeError", "最小時間不可大於最大時間" },
            { "hintTitle", "提示" },
            
            // CategorySelectForm
            { "categoryTitle", "選擇食物種類" },
            { "selectCategory", "指定種類" },
            { "randomCategory", "隨機種類" },
            { "addTag", "加入標籤" },
            { "clearTags", "清除已選" },
            { "candidateCount", "候選餐廳數量：" },
            { "tagPrompt", "請先加入至少一個種類標籤" },
            { "insufficientCandidates", "目前候選餐廳不足 2 家，請調整標籤。" },
            { "candidatePrefix", "候選餐廳數量：" },
            { "lblSelectedTitle", "已加入篩選標籤 (點擊可移除):" },
            { "selectComboHint", "-- 請選擇分類 --" },
            { "categoryHeader", "請選擇食物種類（支援複選標籤）" },
            { "categorySelection", "種類選擇" },
            { "tagHint", "提示：點選標籤上的 × 可移除，或按「清空標籤」重選" },
            
            // SwipeForm
            { "swipeTitle", "選擇你喜歡的餐廳" },
            { "swipeSelectLeft", "選左 ← 左邊" },
            { "swipeSelectRight", "選右 → 右邊" },
            { "swipeProgress", "剩餘 {0} / {1} 家餐廳" },
            { "insufficientRestaurants", "符合條件的餐廳不足 2 家。找到 {0} 家餐廳，無法進行選擇。" },
            { "timesVisited", " (去過 {0} 次 ⭐ {1:F1})" },
            { "timesVisitedNoRating", " (去過 {0} 次 (未評分))" },
            { "phoneLabel", "電話: {0}" },
            { "hoursLabel", "營業: {0}" },
            { "featureLabel", "特色: {0}" },
            { "foodTypeLabel", "種類: {0}" },
            
            // ResultForm
            { "resultTitle", "✓ 推薦給你的餐廳" },
            { "resultSub", "根據你的選擇，推薦如下：" },
            { "btnConfirm", "確定" },
            { "btnFavorite", "♡ 收藏" },
            { "btnFavorited", "♥ 已收藏" },
            { "btnShare", "分享" },
            { "btnDontShow", "✕ 不想再看" },
            { "copiedToClipboard", "已複製到剪貼板" },
            { "copyFailed", "複製失敗: " },
            { "favoriteAdded", "已收藏此餐廳" },
            { "favoriteRemoved", "已取消收藏" },
            { "blockConfirm", "確定要封鎖「{0}」嗎？\n未來篩選時將不會再看到此餐廳。" },
            { "confirmTitle", "確認" },
            { "blockDone", "已封鎖此餐廳" },
            { "ratingRecorded", "已記錄評分：{0} 顆星" },
            
            // ManageRestaurantForm
            { "manageTitle", "新增餐廳" },
            { "lblName", "餐廳名稱*" },
            { "lblPriceRange", "價格區間" },
            { "lblFoodTypes", "食物種類*" },
            { "lblCuisineStyle", "料理風格" },
            { "lblPurposes", "用途標籤" },
            { "lblFeature", "餐廳特色" },
            { "lblPhone", "電話" },
            { "lblBusinessHours", "營業時間" },
            { "lblAddress", "地址" },
            { "lblImageFileName", "圖片檔名(jpg)" },
            { "lblMealTime", "提供時段" },
            { "saveSuccess", "餐廳新增成功" },
            { "saveFailed", "新增失敗: " },
            { "inputNamePrompt", "請輸入餐廳名稱" },
            { "inputFoodTypePrompt", "請至少輸入一個食物種類" },
            
            // ManagePreferenceForm
            { "preferenceTitle", "管理收藏 / 封鎖清單" },
            { "lblAll", "餐廳總清單" },
            { "lblFavorites", "收藏清單" },
            { "lblBlocked", "封鎖清單" },
            { "lblHint", "提示：加入封鎖時會自動從收藏移除；\n加入收藏時也會自動從封鎖移除。" },
            { "selectLeftPrompt", "請先從左側選擇一家餐廳" },
            { "selectFavoritePrompt", "請先在『收藏清單』選擇要移除的餐廳" },
            { "selectBlockedPrompt", "請先在『封鎖清單』選擇要移除的餐廳" },
            { "btnAddFavorite", "加到收藏 →" },
            { "btnAddBlocked", "加到封鎖 →" },
            { "btnRemoveFavorite", "移除收藏" },
            { "btnRemoveBlocked", "移除封鎖" },
            
            // RatingForm
            { "ratingTitleText", "評分：" },
            { "starText", "顆星" },
            { "unselected", "未選擇" },
            
            // SelectRestaurantForm
            { "selectFormTitle", "選擇餐廳" },
            
            // Shared buttons
            { "btnSave", "儲存" },
            { "btnClose", "完成" },
            { "okButton", "確認" },
            { "cancelButton", "取消" }
        };

        private static readonly Dictionary<string, string> EnglishTranslations = new Dictionary<string, string>
        {
            // MainForm
            { "lblTitle", "Don't know what to eat?" },
            { "btnStart", "START!" },
            { "btnTodayMeal", "TODAY'S MEALS" },
            { "btnManageRestaurant", "MANAGE RESTAURANTS" },
            { "btnManagePreference", "FAVORITES & BLOCKED" },
            { "btnReset", "RESET DATA" },
            { "btnBackToHome", "Back to Home" },
            { "langToggle", "中文" },
            { "ratingTitle", "Rating: " },
            { "notRatedYet", "[Not Rated] Click to Rate" },
            { "clickToAdd", "Click to Add" },
            { "resetConfirm", "Are you sure you want to reset all user data? This will clear all ratings, today's meals, and favorites/blocked lists. This cannot be undone." },
            { "resetTitle", "Confirm Reset" },
            { "resetDone", "All user data has been reset." },
            { "resetDoneTitle", "Done" },
            { "resetFailed", "Reset failed: " },
            { "resetFailedTitle", "Error" },
            { "updateDone", "Restaurant data updated successfully." },
            
            // MealSelectForm
            { "mealTitle", "Select Dine Time (Adjustable)" },
            { "mealPreset", "Presets" },
            { "breakfast", "Breakfast" },
            { "lunch", "Lunch" },
            { "dinner", "Dinner" },
            { "mealRange", "Time Range (in 30-min steps, inclusive)" },
            { "mealHint", "Hint: System filters by time range first, then by tags." },
            { "btnNext", "Next" },
            { "btnCancel", "Cancel" },
            { "invalidTimeRange", "Please select a valid time range." },
            { "minMaxTimeError", "Min time cannot be greater than max time." },
            { "hintTitle", "Hint" },
            
            // CategorySelectForm
            { "categoryTitle", "Select Food Types" },
            { "selectCategory", "Select Tags" },
            { "randomCategory", "Random Tags" },
            { "addTag", "Add Tag" },
            { "clearTags", "Clear Tags" },
            { "candidateCount", "Candidates: " },
            { "tagPrompt", "Please add at least one tag first" },
            { "insufficientCandidates", "Insufficient candidates (less than 2). Please adjust tags." },
            { "candidatePrefix", "Candidates: " },
            { "lblSelectedTitle", "Selected tags for filter (click to remove):" },
            { "selectComboHint", "-- Select Category --" },
            { "categoryHeader", "Please Select Food Types (Multiple Tags Supported)" },
            { "categorySelection", "Category Selection" },
            { "tagHint", "Tip: Click × on a tag to remove, or click \"Clear Tags\" to reset" },
            
            // SwipeForm
            { "swipeTitle", "Choose Your Favorite" },
            { "swipeSelectLeft", "Left ← Select" },
            { "swipeSelectRight", "Right → Select" },
            { "swipeProgress", "{0} / {1} Left" },
            { "insufficientRestaurants", "Insufficient restaurants (less than 2 matching). Found {0}. Cannot proceed." },
            { "timesVisited", " (Visited {0} times ⭐ {1:F1})" },
            { "timesVisitedNoRating", " (Visited {0} times (No Rating))" },
            { "phoneLabel", "Phone: {0}" },
            { "hoursLabel", "Hours: {0}" },
            { "featureLabel", "Feature: {0}" },
            { "foodTypeLabel", "Style: {0}" },
            
            // ResultForm
            { "resultTitle", "✓ Recommended for You" },
            { "resultSub", "Based on your choices:" },
            { "btnConfirm", "Confirm" },
            { "btnFavorite", "♡ Favorite" },
            { "btnFavorited", "♥ Favorited" },
            { "btnShare", "Share" },
            { "btnDontShow", "✕ Block" },
            { "copiedToClipboard", "Copied to clipboard!" },
            { "copyFailed", "Failed to copy: " },
            { "favoriteAdded", "Added to favorites!" },
            { "favoriteRemoved", "Removed from favorites." },
            { "blockConfirm", "Are you sure you want to block \"{0}\"?\nIt won't appear in future picker recommendations." },
            { "confirmTitle", "Confirm" },
            { "blockDone", "Restaurant blocked successfully." },
            { "ratingRecorded", "Rating recorded: {0} Stars" },
            
            // ManageRestaurantForm
            { "manageTitle", "Add Restaurant" },
            { "lblName", "Name*" },
            { "lblPriceRange", "Price Range" },
            { "lblFoodTypes", "Food Types*" },
            { "lblCuisineStyle", "Cuisine Style" },
            { "lblPurposes", "Purposes" },
            { "lblFeature", "Feature" },
            { "lblPhone", "Phone" },
            { "lblBusinessHours", "Business Hours" },
            { "lblAddress", "Address" },
            { "lblImageFileName", "Image Filename (jpg)" },
            { "lblMealTime", "Dine Times" },
            { "saveSuccess", "Restaurant added successfully!" },
            { "saveFailed", "Failed to add restaurant: " },
            { "inputNamePrompt", "Please enter restaurant name" },
            { "inputFoodTypePrompt", "Please enter at least one food style" },
            
            // ManagePreferenceForm
            { "preferenceTitle", "Manage Favorites & Blocked" },
            { "lblAll", "All Restaurants" },
            { "lblFavorites", "Favorites" },
            { "lblBlocked", "Blocked" },
            { "lblHint", "Note: Blocking a restaurant removes it from Favorites, and vice versa." },
            { "selectLeftPrompt", "Please select a restaurant from the left list first" },
            { "selectFavoritePrompt", "Please select a restaurant to remove from Favorites" },
            { "selectBlockedPrompt", "Please select a restaurant to remove from Blocked" },
            { "btnAddFavorite", "Add Favorite →" },
            { "btnAddBlocked", "Add Blocked →" },
            { "btnRemoveFavorite", "Remove Favorite" },
            { "btnRemoveBlocked", "Remove Blocked" },
            
            // RatingForm
            { "ratingTitleText", "Rate: " },
            { "starText", "Stars" },
            { "unselected", "Not Selected" },
            
            // SelectRestaurantForm
            { "selectFormTitle", "Select Restaurant" },
            
            // Shared buttons
            { "btnSave", "Save" },
            { "btnClose", "Done" },
            { "okButton", "Confirm" },
            { "cancelButton", "Cancel" }
        };

        public static string GetTranslation(string key, params object[] args)
        {
            var dict = CurrentLanguage == LanguageType.Chinese ? ChineseTranslations : EnglishTranslations;
            if (dict.TryGetValue(key, out var val))
            {
                try
                {
                    return string.Format(val, args);
                }
                catch
                {
                    return val;
                }
            }
            return key;
        }

        public static Image LoadIcon(string baseName)
        {
            try
            {
                string fileName = baseName;
                // If it's English, try to append "_e" before the extension
                if (CurrentLanguage == LanguageType.English)
                {
                    string ext = Path.GetExtension(baseName);
                    string nameWithoutExt = Path.GetFileNameWithoutExtension(baseName);
                    string englishName = nameWithoutExt + "_e" + ext;
                    string testPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "icons", englishName);
                    if (File.Exists(testPath))
                    {
                        fileName = englishName;
                    }
                }

                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "icons", fileName);
                if (!File.Exists(imagePath))
                {
                    // Fallback to non-English name if file doesn't exist
                    imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "icons", baseName);
                }

                if (!File.Exists(imagePath))
                    return null;

                using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var tempImage = Image.FromStream(stream);
                
                // Clean the checkered background (fake transparency) if present!
                var loadedBmp = new Bitmap(tempImage);
                var cleanedBmp = CleanCheckeredBackground(loadedBmp);
                loadedBmp.Dispose();
                return cleanedBmp;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load icon {baseName}: {ex.Message}");
                return null;
            }
        }

        public static Image? LoadAssetImage(string fileName)
        {
            try
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", fileName);
                if (!File.Exists(imagePath))
                {
                    // Fallback to searching in icons folder
                    imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "icons", fileName);
                }

                if (!File.Exists(imagePath))
                    return null;

                using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var image = Image.FromStream(stream);
                return new Bitmap(image);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load asset image {fileName}: {ex.Message}");
                return null;
            }
        }

        public static Bitmap CleanCheckeredBackground(Bitmap source)
        {
            int w = source.Width;
            int h = source.Height;
            
            // Create a copy to work on
            Bitmap bmp = new Bitmap(source);
            
            // We will do a breadth-first search (BFS) flood fill from the corners
            bool[,] visited = new bool[w, h];
            Queue<Point> queue = new Queue<Point>();
            
            // Push the 4 corners
            int[] startX = { 0, w - 1, 0, w - 1 };
            int[] startY = { 0, 0, h - 1, h - 1 };
            
            for (int i = 0; i < 4; i++)
            {
                int x = startX[i];
                int y = startY[i];
                if (x >= 0 && x < w && y >= 0 && y < h)
                {
                    queue.Enqueue(new Point(x, y));
                    visited[x, y] = true;
                }
            }
            
            while (queue.Count > 0)
            {
                Point p = queue.Dequeue();
                Color c = bmp.GetPixel(p.X, p.Y);
                
                // If it is already transparent or near-transparent, we just set it and continue
                if (c.A < 50)
                {
                    bmp.SetPixel(p.X, p.Y, Color.Transparent);
                }
                else if (c.R < 120 && c.G < 120 && c.B < 120)
                {
                    // Dark border, stop
                    continue;
                }
                else
                {
                    // Checkered or other solid background, fill with transparent
                    bmp.SetPixel(p.X, p.Y, Color.Transparent);
                }
                
                // Check 4 neighbors
                int[] dx = { 1, -1, 0, 0 };
                int[] dy = { 0, 0, 1, -1 };
                
                for (int i = 0; i < 4; i++)
                {
                    int nx = p.X + dx[i];
                    int ny = p.Y + dy[i];
                    
                    if (nx >= 0 && nx < w && ny >= 0 && ny < h && !visited[nx, ny])
                    {
                        visited[nx, ny] = true;
                        queue.Enqueue(new Point(nx, ny));
                    }
                }
            }
            
            return bmp;
        }

        public static void ApplyButtonIcon(Button button, string iconName, int iconSize = 24)
        {
            try
            {
                var icon = LoadIcon(iconName);
                if (icon != null)
                {
                    var scaled = ResizeImage(icon, iconSize, iconSize);
                    button.Image?.Dispose();
                    button.Image = scaled;
                    button.ImageAlign = ContentAlignment.MiddleLeft;
                    button.TextImageRelation = TextImageRelation.ImageBeforeText;
                    icon.Dispose();
                }
                else
                {
                    button.Image = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to apply button icon for {button.Name}: {ex.Message}");
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Bitmap ResizeImageKeepAspect(Image image, int canvasWidth, int canvasHeight)
        {
            var destImage = new Bitmap(canvasWidth, canvasHeight);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            double scaleX = (double)canvasWidth / image.Width;
            double scaleY = (double)canvasHeight / image.Height;
            double scale = Math.Min(scaleX, scaleY);

            int newWidth = (int)Math.Round(image.Width * scale);
            int newHeight = (int)Math.Round(image.Height * scale);

            if (newWidth < 1) newWidth = 1;
            if (newHeight < 1) newHeight = 1;

            int xOffset = (canvasWidth - newWidth) / 2;
            int yOffset = (canvasHeight - newHeight) / 2;

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.Clear(Color.Transparent);
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, new Rectangle(xOffset, yOffset, newWidth, newHeight), 
                                       0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Bitmap CropTransparentBordersRect(Image image)
        {
            if (image == null)
                return new Bitmap(1, 1);

            var bitmap = new Bitmap(image);

            if (bitmap.Width < 2 || bitmap.Height < 2)
                return bitmap;

            int minX = bitmap.Width;
            int maxX = -1;
            int minY = bitmap.Height;
            int maxY = -1;

            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            try
            {
                unsafe
                {
                    byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
                    int bytesPerPixel = 4;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            int pixelIndex = (y * bitmapData.Stride) + (x * bytesPerPixel);
                            byte alpha = ptr[pixelIndex + 3];

                            if (alpha > 0)
                            {
                                minX = Math.Min(minX, x);
                                maxX = Math.Max(maxX, x);
                                minY = Math.Min(minY, y);
                                maxY = Math.Max(maxY, y);
                            }
                        }
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            if (maxX < minX || maxY < minY)
                return bitmap;

            int width = maxX - minX + 1;
            int height = maxY - minY + 1;

            var croppedBitmap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(croppedBitmap);
            graphics.Clear(Color.Transparent);
            graphics.DrawImage(bitmap, 
                new Rectangle(0, 0, width, height),
                new Rectangle(minX, minY, width, height),
                GraphicsUnit.Pixel);

            bitmap.Dispose();
            return croppedBitmap;
        }

        public static void ApplyFullButtonImage(Button button, string iconName)
        {
            try
            {
                // Check if this is a text-based gaming button to be rendered programmatically
                var (textKey, bgColor, textColor) = GetButtonConfig(iconName, button.Name);
                if (textKey != null)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.FlatAppearance.MouseDownBackColor = Color.Transparent;
                    button.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    button.BackColor = Color.Transparent;
                    button.UseVisualStyleBackColor = false;
                    button.Text = "";
                    button.ForeColor = Color.Transparent;

                    string translatedText = GetTranslation(textKey);

                    // Dynamic font size: 28% of button height
                    float fontSize = button.Height * 0.28f;
                    if (fontSize < 10) fontSize = 10;

                    float finalFontSize = fontSize;
                    using (var dummyBmp = new Bitmap(1, 1))
                    using (var g = Graphics.FromImage(dummyBmp))
                    {
                        float maxUsableWidth = button.Width - (button.Height * 0.7f);
                        float maxUsableHeight = button.Height * 0.75f;
                        while (finalFontSize > 6f)
                        {
                            using (var tempFont = new Font("微軟正黑體", finalFontSize, FontStyle.Bold))
                            {
                                var textSize = g.MeasureString(translatedText, tempFont);
                                if (textSize.Width <= maxUsableWidth && textSize.Height <= maxUsableHeight)
                                {
                                    break;
                                }
                            }
                            finalFontSize -= 0.5f;
                        }
                    }

                    using (var font = new Font("微軟正黑體", finalFontSize, FontStyle.Bold))
                    {
                        var capBmp = DrawCapsuleButton(translatedText, bgColor, textColor, button.Width, button.Height, font);
                        button.Image?.Dispose();
                        button.BackgroundImage?.Dispose();
                        button.Image = null;
                        button.BackgroundImage = capBmp;
                        button.BackgroundImageLayout = ImageLayout.None;
                    }
                    return;
                }

                // If not programmatically drawn, load the original image from assets
                bool isEnglish = CurrentLanguage == LanguageType.English;
                bool hasEnglishIcon = false;

                if (isEnglish)
                {
                    string ext = Path.GetExtension(iconName);
                    string nameWithoutExt = Path.GetFileNameWithoutExtension(iconName);
                    string englishName = nameWithoutExt + "_e" + ext;
                    string testPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "icons", englishName);
                    if (File.Exists(testPath))
                    {
                        hasEnglishIcon = true;
                    }
                }

                // If in English mode, and there is no dedicated English image and it's not a universal/graphical icon,
                // we must fallback to standard flat button with localized English text, to avoid showing Chinese text in the fallback image!
                if (isEnglish && !hasEnglishIcon && !UniversalIcons.Contains(iconName))
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 1;
                    button.FlatAppearance.BorderColor = Color.DarkGray;
                    button.BackColor = Color.FromArgb(245, 245, 245);
                    button.ForeColor = Color.Black;
                    button.UseVisualStyleBackColor = false;

                    button.Image?.Dispose();
                    button.BackgroundImage?.Dispose();
                    button.Image = null;
                    button.BackgroundImage = null;

                    string textKeyFallback = button.Name;
                    if (textKeyFallback == "btnBack") textKeyFallback = "btnBackToHome";
                    if (textKeyFallback == "btnClearTags") textKeyFallback = "clearTags";
                    if (textKeyFallback == "btnAddTag") textKeyFallback = "addTag";

                    button.Text = GetTranslation(textKeyFallback);
                }
                else
                {
                    // Chinese mode, or English mode with English/Universal icon asset: Completely transparent image button!
                    var icon = LoadIcon(iconName);
                    if (icon != null)
                    {
                        button.FlatStyle = FlatStyle.Flat;
                        button.FlatAppearance.BorderSize = 0;
                        button.FlatAppearance.MouseDownBackColor = Color.Transparent;
                        button.FlatAppearance.MouseOverBackColor = Color.Transparent;
                        button.BackColor = Color.Transparent;
                        button.UseVisualStyleBackColor = false;
                        button.Text = "";
                        button.ForeColor = Color.Transparent;

                        var cropped = CropTransparentBordersRect(icon);
                        var scaled = ResizeImageKeepAspect(cropped, button.Width, button.Height);

                        button.Image?.Dispose();
                        button.BackgroundImage?.Dispose();
                        
                        button.Image = null;
                        button.BackgroundImage = scaled;
                        button.BackgroundImageLayout = ImageLayout.Stretch;

                        cropped.Dispose();
                        icon.Dispose();
                    }
                    else
                    {
                        // Fallback to standard
                        button.Image = null;
                        button.BackgroundImage = null;
                        button.FlatStyle = FlatStyle.Standard;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to apply full button image for {button.Name}: {ex.Message}");
            }
        }

        public static (string? TextKey, Color BgColor, Color TextColor) GetButtonConfig(string iconName, string buttonName)
        {
            string name = iconName.ToLower();
            
            if (name.Contains("start"))
            {
                return ("btnStart", Color.FromArgb(235, 59, 90), Color.White); // Red
            }
            if (name.Contains("myfavorites"))
            {
                return ("btnTodayMeal", Color.FromArgb(254, 211, 48), Color.FromArgb(45, 52, 54)); // Yellow
            }
            if (name.Contains("favoritesmanagement"))
            {
                return ("btnManageRestaurant", Color.FromArgb(38, 222, 129), Color.White); // Green
            }
            if (name.Contains("collection"))
            {
                string key = buttonName == "btnFavorite" ? "btnFavorited" : "btnManagePreference";
                return (key, Color.FromArgb(235, 59, 90), Color.White); // Red
            }
            if (name.Contains("clear"))
            {
                string key = buttonName == "btnClearTags" ? "clearTags" : "btnReset";
                return (key, Color.FromArgb(254, 211, 48), Color.FromArgb(45, 52, 54)); // Yellow
            }
            if (name.Contains("leave") || name.Contains("back"))
            {
                return ("btnBackToHome", Color.FromArgb(254, 211, 48), Color.FromArgb(45, 52, 54)); // Yellow
            }
            if (name.Contains("next"))
            {
                return ("btnNext", Color.FromArgb(38, 222, 129), Color.White); // Green
            }
            if (name.Contains("cancel") || name.Contains("discard"))
            {
                return ("cancelButton", Color.FromArgb(235, 59, 90), Color.White); // Red
            }
            if (name.Contains("complete") || name.Contains("save"))
            {
                string key = buttonName == "btnSave" ? "btnSave" : "btnClose";
                return (key, Color.FromArgb(38, 222, 129), Color.White); // Green
            }
            if (name.Contains("ok"))
            {
                string key = buttonName == "btnConfirm" ? "btnConfirm" : "okButton";
                return (key, Color.FromArgb(38, 222, 129), Color.White); // Green
            }
            if (name.Contains("like"))
            {
                return ("btnFavorite", Color.FromArgb(235, 59, 90), Color.White); // Red
            }
            if (name.Contains("share"))
            {
                return ("btnShare", Color.FromArgb(254, 211, 48), Color.FromArgb(45, 52, 54)); // Yellow
            }
            if (name.Contains("block"))
            {
                return ("btnDontShow", Color.FromArgb(235, 59, 90), Color.White); // Red
            }

            return (null, Color.Empty, Color.Empty);
        }

        public static Bitmap DrawCapsuleButton(string text, Color bgColor, Color textColor, int width, int height, Font font)
        {
            var bmp = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                
                g.Clear(Color.Transparent);

                // Draw capsule background
                int radius = height / 2 - 2;
                if (radius < 4) radius = 4;
                int diameter = radius * 2;
                
                var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.StartFigure();
                path.AddArc(new Rectangle(2, 2, diameter, diameter), 90, 180);
                path.AddArc(new Rectangle(width - diameter - 3, 2, diameter, diameter), 270, 180);
                path.CloseFigure();

                // Fill background
                using (var brush = new SolidBrush(bgColor))
                {
                    g.FillPath(brush, path);
                }

                // Draw thick black border (3.5px)
                using (var pen = new Pen(Color.Black, 3.5f))
                {
                    g.DrawPath(pen, path);
                }

                // Draw text with outline
                var textPath = new System.Drawing.Drawing2D.GraphicsPath();
                using (var sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.FormatFlags |= StringFormatFlags.NoWrap;
                    
                    float emSize = g.DpiY * font.Size / 72;
                    textPath.AddString(text, font.FontFamily, (int)font.Style, emSize, new RectangleF(0, 0, width, height), sf);
                }

                // Determine outline color (high contrast)
                Color outlineColor = textColor == Color.White ? Color.Black : Color.White;
                using (var outlinePen = new Pen(outlineColor, 4.5f))
                {
                    outlinePen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    g.DrawPath(outlinePen, textPath);
                }

                using (var textBrush = new SolidBrush(textColor))
                {
                    g.FillPath(textBrush, textPath);
                }
            }
            return bmp;
        }

        private static readonly Dictionary<string, string> TagTranslations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "豆漿", "Soy Milk" },
            { "蛋餅", "Egg Pancake" },
            { "燒餅", "Clay Oven Roll" },
            { "義大利麵", "Pasta" },
            { "燉飯", "Risotto" },
            { "炒麵", "Fried Noodles" },
            { "炒米粉", "Fried Rice Noodles" },
            { "關東煮", "Oden" },
            { "雞排", "Chicken Fillet" },
            { "炸物", "Fried Food" },
            { "炒飯", "Fried Rice" },
            { "燴飯", "Braising Rice" },
            { "簡餐", "Easy Meal" },
            { "廣東粥", "Cantonese Congee" },
            { "鍋燒麵", "Pot Noodles" },
            { "滷味", "Braised Food" },
            { "湯包", "Soup Dumpling" },
            { "煎餃", "Fried Dumpling" },
            { "雞肉飯", "Chicken Rice" },
            { "便當", "Bento" },
            { "椒麻雞", "Spicy Chicken" },
            { "現打果汁", "Fresh Juice" },
            { "臭豆腐", "Stinky Tofu" },
            { "水餃", "Dumpling" },
            { "酸辣湯", "Hot & Sour Soup" },
            { "雞排飯", "Chicken Fillet Rice" },
            { "韓式炸雞", "Korean Fried Chicken" },
            { "麻辣燙", "Spicy Hot Pot" },
            { "拉麵", "Ramen" },
            { "丼飯", "Donburi" },
            { "定食", "Set Meal" },
            { "快餐", "Fast Food" },
            { "蒸餃", "Steamed Dumpling" },
            { "熱炒", "Stir Fry" },
            { "早午餐", "Brunch" },
            { "漢堡", "Burger" },
            { "鐵板麵", "Teppan Noodles" },
            { "輕食", "Light Food" },
            { "咖啡", "Coffee" },
            { "泰式簡餐", "Thai Style Meal" },
            { "打拋豬", "Pad Kra Prow" },
            { "日式丼飯", "Japanese Donburi" },
            { "吐司", "Toast" },
            { "麵食", "Noodle Meal" },
            { "三明治", "Sandwich" },
            { "早午餐拼盤", "Brunch Platter" },
            { "鍋貼", "Potsticker" },
            { "健康沙拉", "Healthy Salad" },
            { "飯食", "Rice Meal" },
            { "牛肉麵", "Beef Noodles" },
            { "韓食", "Korean Food" },
            { "豆腐鍋", "Tofu Pot" },
            { "舒肥健康餐", "Sous Vide Meal" },
            { "小火鍋", "Mini Hot Pot" },
            { "咖哩", "Curry" },
            { "炸雞", "Fried Chicken" },
            { "芋圓", "Taro Ball" },
            { "手工豆花", "Handmade Tofu Pudding" },
            { "中式麵食", "Chinese Noodles" },
            { "飲料", "Drinks" },
            { "飯糰", "Rice Ball" },
            { "手搖飲", "Hand-shaken Tea" },
            { "牛排", "Steak" }
        };

        private static readonly Dictionary<string, string> EnglishToChineseTag = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        static LanguageManager()
        {
            foreach (var kvp in TagTranslations)
            {
                EnglishToChineseTag[kvp.Value] = kvp.Key;
            }
        }

        public static string GetLocalizedTag(string chTag)
        {
            if (string.IsNullOrWhiteSpace(chTag)) return chTag;
            if (CurrentLanguage == LanguageType.Chinese) return chTag;

            if (TagTranslations.TryGetValue(chTag.Trim(), out var enTag))
            {
                return enTag;
            }
            return chTag;
        }

        public static string GetChineseTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return tag;
            if (EnglishToChineseTag.TryGetValue(tag.Trim(), out var chTag))
            {
                return chTag;
            }
            return tag;
        }

        private static readonly Dictionary<string, string> FeatureTranslations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "學生愛店", "Student's Favorite" },
            { "醬料自助吧", "Sauce Buffet Bar" },
            { "餐點選擇多樣", "Diverse Menu Selections" },
            { "有冷氣", "AC Available" },
            { "飲料無限供應", "Unlimited Drinks" },
            { "價格平實", "Affordable" },
            { "宵夜首選", "Best for Midnight Snacks" },
            { "現點現炸", "Freshly Fried" },
            { "香酥多汁", "Crispy & Juicy" },
            { "白飯/飲料吃到飽", "Unlimited Rice & Drinks" },
            { "濃湯飲料無限供應", "Unlimited Soup & Drinks" },
            { "口味清淡", "Mild Flavor" },
            { "學生推薦", "Student Recommended" },
            { "品項多元", "Diverse Selections" },
            { "脆皮底層", "Crispy Bottom" },
            { "現蒸現做", "Freshly Steamed" },
            { "學生價位", "Student Friendly Prices" },
            { "份量足", "Generous Portions" },
            { "獨家椒麻醬汁", "Exclusive Spicy Sauce" },
            { "湯頭甘甜", "Sweet Broth" },
            { "熱門宵夜", "Popular Midnight Snack" },
            { "醬汁濃郁", "Rich Sauce" },
            { "乾溼皆可", "Dry or Soup Available" },
            { "新鮮水果現打", "Freshly Squeezed Fruit" },
            { "種類繁多", "Wide Variety" },
            { "臭豆腐外酥內嫩", "Crispy Outside, Tender Inside" },
            { "手工現包", "Handmade" },
            { "皮Q餡實", "Chewy Skin & Rich Filling" },
            { "飲料吃到飽", "Unlimited Drinks" },
            { "可調整辣度", "Adjustable Spiciness" },
            { "免費加麵一次", "One Free Noodle Refill" },
            { "附味噌湯與飲品", "Includes Miso Soup & Drinks" },
            { "主菜選擇多", "Many Main Course Selections" },
            { "出餐快", "Fast Service" },
            { "鮮肉湯包", "Pork Soup Dumplings" },
            { "內餡多汁", "Juicy Fillings" },
            { "份量充足", "Generous Portions" },
            { "環境舒適", "Cozy Environment" },
            { "附湯與飲料", "Includes Soup & Drinks" },
            { "平價家常菜", "Affordable Home Cooking" },
            { "適合讀書", "Good for Studying" },
            { "有插座", "Outlets Available" },
            { "環境安靜", "Quiet Environment" },
            { "口味獨特", "Unique Flavor" },
            { "學生熱門", "Popular with Students" },
            { "食材豐富", "Rich Ingredients" },
            { "傳統早點", "Traditional Breakfast" },
            { "價格實惠", "Reasonable Prices" },
            { "麵條Q彈", "Chewy Noodles" },
            { "自製辣醬", "Homemade Spicy Sauce" },
            { "連鎖早餐", "Chain Breakfast" },
            { "附味噌湯", "Includes Miso Soup" },
            { "環境乾淨", "Clean Environment" },
            { "擺盤精美", "Beautifully Plated" },
            { "連鎖品牌", "Chain Brand" },
            { "品質穩定", "Consistent Quality" },
            { "便宜大碗", "Cheap & Large Portion" },
            { "素食友善", "Vegetarian Friendly" },
            { "低GI飲食", "Low GI Diet" },
            { "環境整潔", "Clean Environment" },
            { "湯頭濃郁", "Rich Broth" },
            { "韓式小菜吃到飽", "Unlimited Korean Side Dishes" },
            { "食材新鮮", "Fresh Ingredients" },
            { "吃到飽自助吧", "All-You-Can-Eat Buffet" },
            { "環境文青", "Chic Environment" },
            { "營業至午夜", "Open Until Midnight" },
            { "目前暫停營業", "Temporarily Closed" },
            { "價格親民", "Affordable Price" },
            { "裝潢明亮舒適", "Bright & Comfortable Decor" },
            { "道地日式風味", "Authentic Japanese Flavor" },
            { "傳統中式早點", "Traditional Chinese Breakfast" }
        };

        public static string GetLocalizedFeature(string chFeature)
        {
            if (string.IsNullOrWhiteSpace(chFeature)) return chFeature;
            if (CurrentLanguage == LanguageType.Chinese) return chFeature;

            var parts = chFeature.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var translatedParts = parts.Select(p =>
            {
                var trimmed = p.Trim();
                if (FeatureTranslations.TryGetValue(trimmed, out var enFeature))
                {
                    return enFeature;
                }
                return trimmed;
            });

            return string.Join("; ", translatedParts);
        }
    }
}

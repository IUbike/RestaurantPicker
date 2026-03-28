using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RestaurantPicker.Models;

namespace RestaurantPicker.Repositories
{
    /// <summary>
    /// CSV 檔案方式的餐廳資料存取實現
    /// 讀取與寫入 CSV 檔案
    /// </summary>
    public class CsvRestaurantRepository : IRestaurantRepository
    {
        private readonly string _csvFilePath;
        private List<Restaurant> _restaurants;

        public CsvRestaurantRepository(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
            _restaurants = new List<Restaurant>();
        }

        /// <summary>
        /// 載入所有餐廳資料
        /// </summary>
        public List<Restaurant> LoadAll()
        {
            _restaurants = new List<Restaurant>();

            if (!File.Exists(_csvFilePath))
            {
                throw new FileNotFoundException($"CSV 檔案不存在: {_csvFilePath}");
            }

            try
            {
                using (var reader = new StreamReader(_csvFilePath, Encoding.UTF8))
                {
                    // 讀取標題列
                    string headerLine = reader.ReadLine();
                    if (headerLine == null)
                        return _restaurants;

                    var headers = headerLine.Split(',');

                    // 讀取資料列
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var restaurant = ParseCsvLine(line, headers);
                        if (restaurant != null)
                        {
                            _restaurants.Add(restaurant);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"讀取 CSV 檔案失敗: {ex.Message}", ex);
            }

            return _restaurants;
        }

        /// <summary>
        /// 根據 ID 取得單家餐廳
        /// </summary>
        public Restaurant GetById(int id)
        {
            return _restaurants.FirstOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// 新增餐廳
        /// </summary>
        public void Add(Restaurant restaurant)
        {
            // 若列表為空，載入現有資料
            if (_restaurants.Count == 0)
            {
                LoadAll();
            }

            // 確定新 ID
            int maxId = _restaurants.Count > 0 ? _restaurants.Max(r => r.Id) : 0;
            restaurant.Id = maxId + 1;

            _restaurants.Add(restaurant);
        }

        /// <summary>
        /// 更新餐廳資訊
        /// </summary>
        public void Update(Restaurant restaurant)
        {
            var existing = _restaurants.FirstOrDefault(r => r.Id == restaurant.Id);
            if (existing != null)
            {
                int index = _restaurants.IndexOf(existing);
                _restaurants[index] = restaurant;
            }
        }

        /// <summary>
        /// 刪除餐廳
        /// </summary>
        public void Delete(int id)
        {
            var restaurant = _restaurants.FirstOrDefault(r => r.Id == id);
            if (restaurant != null)
            {
                _restaurants.Remove(restaurant);
            }
        }

        /// <summary>
        /// 儲存所有變更回 CSV 檔案
        /// </summary>
        public void SaveAll(List<Restaurant> restaurants)
        {
            _restaurants = restaurants;
            WriteCsv();
        }

        /// <summary>
        /// 寫入 CSV 檔案
        /// </summary>
        private void WriteCsv()
        {
            try
            {
                using (var writer = new StreamWriter(_csvFilePath, false, Encoding.UTF8))
                {
                    // 寫入標題列
                    writer.WriteLine("Id,Name,PriceRange,FoodTypes,CuisineStyle,Purposes,Feature,Phone,BusinessHours,Address,IsBreakfastAvailable,IsLunchAvailable,IsDinnerAvailable,ImageFileName");

                    // 寫入資料列
                    foreach (var restaurant in _restaurants)
                    {
                        string line = FormatCsvLine(restaurant);
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"寫入 CSV 檔案失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 解析 CSV 一行資料為 Restaurant 物件
        /// </summary>
        private Restaurant ParseCsvLine(string line, string[] headers)
        {
            try
            {
                var values = line.Split(',');
                if (values.Length < headers.Length)
                    return null;

                var restaurant = new Restaurant
                {
                    Id = int.Parse(values[0]),
                    Name = values[1],
                    PriceRange = values[2],
                    CuisineStyle = values[4],
                    Feature = values[6],
                    Phone = values[7],
                    BusinessHours = values[8],
                    Address = values[9],
                    IsBreakfastAvailable = bool.Parse(values[10]),
                    IsLunchAvailable = bool.Parse(values[11]),
                    IsDinnerAvailable = bool.Parse(values[12]),
                    ImageFileName = values[13],
                    IsFavorite = false,
                    IsBlocked = false
                };

                // 解析 FoodTypes (用分號分隔)
                if (!string.IsNullOrEmpty(values[3]))
                {
                    restaurant.FoodTypes = values[3].Split(';').Select(s => s.Trim()).ToList();
                }

                // 解析 Purposes (用分號分隔)
                if (!string.IsNullOrEmpty(values[5]))
                {
                    restaurant.Purposes = values[5].Split(';').Select(s => s.Trim()).ToList();
                }

                return restaurant;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"解析 CSV 行失敗: {line}. 錯誤: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 將 Restaurant 物件格式化為 CSV 一行
        /// </summary>
        private string FormatCsvLine(Restaurant restaurant)
        {
            var parts = new[]
            {
                restaurant.Id.ToString(),
                restaurant.Name,
                restaurant.PriceRange,
                string.Join(";", restaurant.FoodTypes),
                restaurant.CuisineStyle,
                string.Join(";", restaurant.Purposes),
                restaurant.Feature,
                restaurant.Phone,
                restaurant.BusinessHours,
                restaurant.Address,
                restaurant.IsBreakfastAvailable.ToString(),
                restaurant.IsLunchAvailable.ToString(),
                restaurant.IsDinnerAvailable.ToString(),
                restaurant.ImageFileName
            };

            return string.Join(",", parts);
        }
    }
}

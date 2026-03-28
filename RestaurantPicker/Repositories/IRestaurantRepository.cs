using System;
using System.Collections.Generic;
using RestaurantPicker.Models;

namespace RestaurantPicker.Repositories
{
    /// <summary>
    /// 餐廳資料存取介面
    /// 未來若要改成資料庫，只需實現此介面即可，不需要改業務邏輯層
    /// </summary>
    public interface IRestaurantRepository
    {
        /// <summary>
        /// 載入所有餐廳資料
        /// </summary>
        List<Restaurant> LoadAll();

        /// <summary>
        /// 根據 ID 取得單家餐廳
        /// </summary>
        Restaurant GetById(int id);

        /// <summary>
        /// 新增餐廳
        /// </summary>
        void Add(Restaurant restaurant);

        /// <summary>
        /// 更新餐廳資訊
        /// </summary>
        void Update(Restaurant restaurant);

        /// <summary>
        /// 刪除餐廳
        /// </summary>
        void Delete(int id);

        /// <summary>
        /// 儲存所有變更（對於 CSV 檔案系統）
        /// </summary>
        void SaveAll(List<Restaurant> restaurants);
    }
}

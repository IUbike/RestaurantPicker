using System;
using System.Collections.Generic;
using System.Linq;
using RestaurantPicker.Models;

namespace RestaurantPicker.Services
{
    /// <summary>
    /// 二選一交友軟體風格的比對服務
    /// 管理左右兩家餐廳的選擇、篩選、補充邏輯
    /// </summary>
    public class SwipeMatchService
    {
        private RandomPickService _randomPickService;

        // 目前顯示的左右兩家餐廳
        public Restaurant LeftRestaurant { get; private set; }
        public Restaurant RightRestaurant { get; private set; }

        // 候選餐廳池（還沒被顯示過的）
        private List<Restaurant> _candidatePool;

        // 已經淘汰的餐廳（被選掉的）
        private List<Restaurant> _eliminatedRestaurants;

        public SwipeMatchService()
        {
            _randomPickService = new RandomPickService();
            _candidatePool = new List<Restaurant>();
            _eliminatedRestaurants = new List<Restaurant>();
        }

        /// <summary>
        /// 初始化二選一流程
        /// 傳入篩選後的餐廳清單，隨機選出左右兩家，其餘放入候選池
        /// </summary>
        public void Initialize(List<Restaurant> filteredRestaurants)
        {
            if (filteredRestaurants == null || filteredRestaurants.Count == 0)
            {
                LeftRestaurant = null;
                RightRestaurant = null;
                _candidatePool.Clear();
                _eliminatedRestaurants.Clear();
                return;
            }

            // 打亂順序
            var shuffled = _randomPickService.ShuffleRestaurants(filteredRestaurants);

            // 取前兩家作為左右餐廳
            LeftRestaurant = shuffled.Count > 0 ? shuffled[0] : null;
            RightRestaurant = shuffled.Count > 1 ? shuffled[1] : null;

            // 其餘的放入候選池
            _candidatePool = shuffled.Skip(2).ToList();
            _eliminatedRestaurants.Clear();
        }

        /// <summary>
        /// 選擇左邊的餐廳
        /// - 保留左邊餐廳
        /// - 淘汰右邊餐廳
        /// - 右邊補入新餐廳
        /// </summary>
        public bool SelectLeft()
        {
            if (LeftRestaurant == null || RightRestaurant == null)
                return false;

            // 淘汰右邊
            _eliminatedRestaurants.Add(RightRestaurant);

            // 補充右邊
            if (_candidatePool.Count > 0)
            {
                RightRestaurant = _candidatePool[0];
                _candidatePool.RemoveAt(0);
                return true;
            }
            else
            {
                // 候選池用完，RightRestaurant 變為 null
                RightRestaurant = null;
                return false;
            }
        }

        /// <summary>
        /// 選擇右邊的餐廳
        /// - 保留右邊餐廳
        /// - 淘汰左邊餐廳
        /// - 左邊補入新餐廳
        /// </summary>
        public bool SelectRight()
        {
            if (LeftRestaurant == null || RightRestaurant == null)
                return false;

            // 淘汰左邊
            _eliminatedRestaurants.Add(LeftRestaurant);

            // 補充左邊
            if (_candidatePool.Count > 0)
            {
                LeftRestaurant = _candidatePool[0];
                _candidatePool.RemoveAt(0);
                return true;
            }
            else
            {
                // 候選池用完，LeftRestaurant 變為 null
                LeftRestaurant = null;
                return false;
            }
        }

        /// <summary>
        /// 檢查是否還有餐廳可以比對
        /// （左右兩邊都存在時才能繼續）
        /// </summary>
        public bool HasNextPair()
        {
            return LeftRestaurant != null && RightRestaurant != null;
        }

        /// <summary>
        /// 取得最終推薦結果
        /// 當只剩一家餐廳時，該餐廳就是推薦結果
        /// </summary>
        public Restaurant GetFinalResult()
        {
            // 優先回傳還存在的單家餐廳
            if (LeftRestaurant != null && RightRestaurant == null)
                return LeftRestaurant;

            if (RightRestaurant != null && LeftRestaurant == null)
                return RightRestaurant;

            // 如果左右都還在，代表比對還沒結束
            if (LeftRestaurant != null && RightRestaurant != null)
                return null;

            // 都沒有了，代表沒有初始化或所有餐廳都被淘汰了
            return null;
        }

        /// <summary>
        /// 取得目前還有多少候選餐廳可供補充
        /// </summary>
        public int GetRemainingCandidateCount()
        {
            return _candidatePool.Count;
        }

        /// <summary>
        /// 取得已淘汰的餐廳清單
        /// </summary>
        public List<Restaurant> GetEliminatedRestaurants()
        {
            return _eliminatedRestaurants.ToList();
        }

        /// <summary>
        /// 取得所有候選餐廳（還未顯示過的）
        /// </summary>
        public List<Restaurant> GetCandidatePool()
        {
            return _candidatePool.ToList();
        }

        /// <summary>
        /// 重置狀態
        /// </summary>
        public void Reset()
        {
            LeftRestaurant = null;
            RightRestaurant = null;
            _candidatePool.Clear();
            _eliminatedRestaurants.Clear();
        }
    }
}

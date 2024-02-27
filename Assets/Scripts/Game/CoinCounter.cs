using DataStore;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class CoinCounter : MonoBehaviour
    {
        public static CoinCounter Instance;

        [SerializeField]
        private UnityEvent<int> coinCountChanged;

        public int CoinCount { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
        }

        private void Start()
        {
            var game = GameManager.Instance;
            game.GameEnding += SaveCoins;
        }

        public void AddCoin()
        {
            CoinCount++;
            coinCountChanged.Invoke(CoinCount);
        }

        public void SaveCoins()
        {
            var goldStorage = new GoldStorage();
            goldStorage.AddGold(CoinCount);
        }
    }
}
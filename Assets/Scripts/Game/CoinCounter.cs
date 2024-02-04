using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class CoinCounter : MonoBehaviour
    {
        private int _coinCount;

        public static CoinCounter Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
        }

        [SerializeField]
        private UnityEvent<int> coinCountChanged;

        public void AddCoin()
        {
            _coinCount++;
            coinCountChanged.Invoke(_coinCount);
        }
    }
}
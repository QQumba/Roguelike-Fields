using Events;
using Game;
using UnityEngine;

namespace Effects
{
    public class PickUpCoinEffect : MonoBehaviour
    {
        public void PickUpCoin(CellEventArgs _)
        {
            CoinCounter.Instance.AddCoin();
        }
    }
}
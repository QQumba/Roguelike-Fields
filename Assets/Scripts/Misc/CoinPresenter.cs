using TMPro;
using UnityEngine;

namespace Misc
{
    public class CoinPresenter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro text;

        public void UpdateCount(int count)
        {
            text.text = $"{count}";
        }
    }
}
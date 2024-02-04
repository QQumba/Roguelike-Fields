using Events;
using TMPro;
using UnityEngine;

namespace Misc
{
    public class HealthPresenter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro text;

        [SerializeField]
        private bool showMax;

        public void UpdateHealth(HealthChangedEventArgs args)
        {
            if (showMax)
            {
                text.text = $"{args.CurrentValue}/{args.MaxValue}";
                return;
            }

            text.text = $"{args.CurrentValue}";
        }
    }
}
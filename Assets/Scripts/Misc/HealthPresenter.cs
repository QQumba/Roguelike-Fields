using Events;
using TMPro;
using UnityEngine;

namespace Misc
{
    [RequireComponent(typeof(TextMeshPro))]
    public class HealthPresenter : MonoBehaviour
    {
        [SerializeField]
        private bool showMax;

        private TextMeshPro _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshPro>();
        }

        public void UpdateHealth(HealthChangedEventArgs args)
        {
            if (showMax)
            {
                _text.text = $"{args.CurrentValue}/{args.MaxValue}";
                return;
            }

            _text.text = $"{args.CurrentValue}";
        }
    }
}
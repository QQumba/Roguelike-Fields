using Events;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Misc
{
    public class ValuePresenter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro text;

        [SerializeField]
        private bool showMax;
        
        [SerializeField] private UnityEvent valueUpdatedEvent;
        [SerializeField] private UnityEvent valueHiddenEvent;

        public void UpdateValue(ValueChangedEventArgs args)
        {
            if (showMax)
            {
                text.text = $"{args.CurrentValue}/{args.MaxValue}";
                return;
            }

            text.text = $"{args.CurrentValue}";

            valueUpdatedEvent.Invoke();
        }

        public void HideValue()
        {
            text.text = "";
            
            valueHiddenEvent.Invoke();
        }
    }
}
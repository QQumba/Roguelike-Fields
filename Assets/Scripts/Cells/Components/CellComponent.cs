using Events;
using UnityEngine;
using UnityEngine.Events;

namespace Cells.Components
{
    public abstract class CellComponent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<CellEventArgs> turnEndedEvent;

        public Cell Cell { get; set; }

        public abstract string CellTag { get; }

        public virtual void OnTurnEnded()
        {
            // move this to cell level
            turnEndedEvent.Invoke(new CellEventArgs(Cell));
        }
        
        public virtual void OnTurnStarted()
        {
        }
    }
}
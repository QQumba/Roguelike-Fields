using Events;
using TurnData;
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

        public virtual void OnTurnEnded(TurnAction turnAction)
        {
            turnEndedEvent.Invoke(new CellEventArgs(Cell));
        }
        
        public virtual void OnTurnStarted()
        {
        }
    }
}
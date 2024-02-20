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

        public void OnTurnEnded()
        {
            turnEndedEvent.Invoke(new CellEventArgs(Cell));
        }
    }
}
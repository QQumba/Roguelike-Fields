using Cells.Components.Interfaces;
using UnityEngine;

namespace Cells.Components
{
    public abstract class CellComponent : MonoBehaviour, ICellComponent
    {
        public Cell Cell { get; set; }

        public abstract string CellTag { get; }

        public virtual void OnTurnEnded()
        {
        }
    }
}
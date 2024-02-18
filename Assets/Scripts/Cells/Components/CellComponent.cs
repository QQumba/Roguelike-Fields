using UnityEngine;

namespace Cells.Components
{
    public abstract class CellComponent : MonoBehaviour
    {
        public Cell Cell { get; set; }

        public abstract string CellTag { get; }
    }
}
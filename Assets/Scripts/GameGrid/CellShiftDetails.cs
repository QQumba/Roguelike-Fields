using System.Collections.Generic;
using Cells;
using UnityEngine;

namespace GameGrid
{
    public class CellShiftDetails
    {
        public List<Cell> Cells { get; set; } = new List<Cell>();

        public Vector2Int LastCellIndex { get; set; }

        public Vector2Int ShiftFrom { get; set; }
    }
}
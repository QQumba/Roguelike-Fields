using System;
using UnityEngine;

namespace Cells
{
    public class CellSlot : MonoBehaviour
    {
        [SerializeField]
        private float width = 1f;

        [SerializeField]
        private float height = 1f;
        
        public float Width => width;

        public float Height => height;

        public Cell Cell { get; private set; }

        public static event Action<Cell> CellClicked;

        public event Action Cleared;

        private void OnMouseUpAsButton()
        {
            CellClicked?.Invoke(Cell);
        }

        public void SetCell(Cell cell)
        {
            if (cell == null)
            {
                Cell = null;
                return;
            }

            var cellTransform = cell.transform;
            
            cellTransform.SetParent(transform);
            cellTransform.localPosition = Vector3.zero;
            
            Cell = cell;
        }

        public void Clear()
        {
            Cleared?.Invoke();
            Cell = null;
        }
        
        // I think I should explicitly call set cell
        // private void Start()
        // {
        //     Cell ??= GetComponentInChildren<Cell>();
        // }
    }
}
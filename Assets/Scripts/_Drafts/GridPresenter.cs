using Cells;
using UnityEngine;

namespace GameGrid
{
    public class GridPresenter : MonoBehaviour
    {
        private Grid _grid;

        private void Start()
        {
            _grid = GetComponent<Grid>();
        }

        public bool TryGetCellAtPosition(Vector3 position, out Cell cell)
        {
            cell = null;
            var x = Mathf.RoundToInt(position.x - 0 + (_grid.Width - 1) * 0.5f);
            var y = Mathf.RoundToInt(position.y - 0 + (_grid.Height - 1) * 0.5f);
            if (!_grid.IsIndexInBounds(x, y))
            {
                return false;
            }

            cell = _grid.GetCell(x, y);
            return true;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cells;
using Cells.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameGrid
{
    public class Grid : MonoBehaviour
    {
        [SerializeField]
        private int height = 3;

        [SerializeField]
        private int width = 3;

        [FormerlySerializedAs("wrapperPrefab")] [SerializeField]
        private CellSlot slotPrefab;

        private CellSlot[,] _cells;

        public static Grid Instance { get; private set; }
        
        public IEnumerable<Cell> Cells => Flatten();

        public Cell Hero => Cells.Where(x => x != null).First(x => x.GetCellComponent<Hero>() != null);
        
        public int Height => height;

        public int Width => width;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            InitializeCellSlots();
        }

        public Vector2Int IndexOf(Cell cell)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (_cells[x, y].Cell == cell)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }

            return new Vector2Int(-1, -1);
        }

        public bool IsCellBelongsToGrid(Cell cell)
        {
            var index = IndexOf(cell);
            return IsIndexInBounds(index.x, index.y);
        }

        public bool IsIndexInBounds(Vector2Int index)
        {
            return index.x >= 0 && index.x < Width && index.y >= 0 && index.y < Height;
        }

        public bool IsIndexInBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public bool IsCellsAdjacent(Cell a, Cell b, bool includeDiagonals = false)
        {
            var indexOfA = IndexOf(a);
            var indexOfB = IndexOf(b);
            var diffX = Mathf.Abs(indexOfA.x - indexOfB.x);
            var diffY = Mathf.Abs(indexOfA.y - indexOfB.y);

            switch (diffX)
            {
                case 1 when diffY == 0:
                case 0 when diffY == 1:
                case 1 when diffY == 1 && includeDiagonals:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsCellAdjacentToHero(Cell cell)
        {
            return IsCellsAdjacent(cell, Hero, false);
        }

        public List<Cell> GetAdjacentCells(Cell cell)
        {
            var index = IndexOf(cell);
            return GetAdjacentCells(index);
        }
        
        public List<Cell> GetAdjacentCells(Vector2Int index)
        {
            var cells = new List<Cell>(4);
            var direction = Direction.Up;

            for (int i = 0; i < 4; i++)
            {
                direction = direction.NextDirectionClockwise();
                var cell = GetCell(index + direction.ToIndex());
                if (cell is not null)
                {
                    cells.Add(cell);
                }
            }

            return cells;
        }

        public Cell GetCell(Vector2Int index)
        {
            return IsIndexInBounds(index.x, index.y) ? _cells[index.x, index.y].Cell : null;
        }

        /// <summary>
        /// Gets a cell at a given indices.
        /// </summary>
        /// <param name="x">X index.</param>
        /// <param name="y">Y index.</param>
        /// <returns>Cell or null if any of the indices are invalid.</returns>
        public Cell GetCell(int x, int y)
        {
            return IsIndexInBounds(x, y) ? _cells[x, y].Cell : null;
        }

        /// <summary>
        /// Place a cell in the grid.
        /// </summary>
        /// <param name="cell">Cell that will be placed to grid.</param>
        /// <param name="x">X index.</param>
        /// <param name="y">Y index.</param>
        public void SetCell(Cell cell, int x, int y)
        {
            if (!IsIndexInBounds(x, y))
            {
                return;
            }

            _cells[x, y].SetCell(cell);
        }

        public void SetCell(Cell cell, Vector2Int index)
        {
            SetCell(cell, index.x, index.y);
        }

        public Vector2Int RemoveCell(Cell cell)
        {
            var index = IndexOf(cell);
            Destroy(cell.gameObject);
            _cells[index.x, index.y].Clear();

            return index;
        }

        private IEnumerable<Cell> Flatten()
        {
            var items = new Cell[Width * Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    items[x * Width + y] = _cells[x, y].Cell;
                }
            }

            return items;
        }

        private void InitializeCellSlots()
        {
            _cells = new CellSlot[height, width];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _cells[x, y] = Instantiate(slotPrefab, GetCellPosition(x, y), Quaternion.identity);
                }
            }
        }

        public Vector2 GetCellPosition(Cell cell)
        {
            var index = IndexOf(cell);
            return GetCellPosition(index.x, index.y);
        }
        
        public Vector2 GetCellPosition(Vector2Int index)
        {
            return GetCellPosition(index.x, index.y);
        }

        private Vector2 GetCellPosition(int x, int y)
        {
            var gridCenter = Vector2.zero;
            var position = gridCenter;
            position.x -= ((Width - 1) * 0.5f - x) * slotPrefab.Width;
            position.y -= ((Height - 1) * 0.5f - y) * slotPrefab.Height;

            return position;
        }
    }
}
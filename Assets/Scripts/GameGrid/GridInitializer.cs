using Cells;
using Game;
using Game.CellGenerator;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameGrid
{
    /// <summary>
    /// Initialize the grid.
    /// </summary>
    [RequireComponent(typeof(Grid))]
    [RequireComponent(typeof(CellSpawner))]
    public class GridInitializer : MonoBehaviour
    {
        private Grid _grid;
        private GridController _gridController;
        private CellSpawner _spawner;
        
        public static GridInitializer Instance { get; private set; }

        private void Awake()
        {
            _grid = GetComponent<Grid>();
            _gridController = GetComponent<GridController>();
            _spawner = GetComponent<CellSpawner>();
         
            Instance = this;
        }

        private void Start()
        {
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            var heroPositionX = Random.Range(0, _grid.Width);
            var heroPositionY = Random.Range(0, _grid.Height);

            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    Cell cell;
                    if (x == heroPositionX && y == heroPositionY)
                    {
                        cell = _spawner.SpawnHero();
                    }
                    else
                    {
                        cell = _spawner.SpawnCell(_gridController.GetSpawnerState());
                    }

                    _grid.SetCell(cell, x, y);
                }
            }
        }
    }
}
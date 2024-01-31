using Animations.AsyncAnimations;
using Cells;
using Game;
using UnityEngine;

namespace GameGrid
{
    /// <summary>
    /// Allows complex actions on the grid
    /// </summary>
    [RequireComponent(
        typeof(Grid),
        typeof(CellSpawner))]
    public class GridController : MonoBehaviour
    {
        private Grid _grid;
        private CellSpawner _spawner;
        private AsyncAnimator _animator;

        public static GridController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            _grid = GetComponent<Grid>();
            _spawner = GetComponent<CellSpawner>();
        }

        private void Start()
        {
            _animator = AsyncAnimator.Instance;
        }

        /// <summary>
        /// Move cell to a new position.
        /// </summary>
        /// <param name="a">Cell that will be moved.</param>
        /// <param name="b">Cell to which position cell <see cref="a"/> wil be moved. Destroyed in process.</param>
        public async void Move(Cell a, Cell b)
        {
            var heroIndex = _grid.IndexOf(a);
            var cellIndex = _grid.IndexOf(b);
            var newCell = _spawner.SpawnCell(Vector3.zero);

            await _animator.Play(new ScaleAsync(b.transform, Vector3.zero), b);

            _grid.RemoveCell(b);

            await _animator.Play(new MoveAsync(a.transform, _grid.GetCellPosition(cellIndex)), a);

            _grid.SetCell(a, cellIndex);
            _grid.SetCell(newCell, heroIndex);

            await _animator.Play(new ScaleAsync(newCell.transform, Vector3.one), newCell);
        }

        public async void Replace(Cell a, Cell b)
        {
            var index = _grid.IndexOf(a);
            var position = _grid.GetCellPosition(index);
            const float rotationSpeed = 3f;

            await _animator.Play(new FlipAsync(a.transform, 90, 0, rotationSpeed * 2), a);

            _grid.RemoveCell(a);
            _grid.SetCell(b, index);

            var emptyCell = _spawner.SpawnEmptyCell();
            emptyCell.transform.position = position;

            await _animator.Play(new FlipAsync(emptyCell.transform, 270, 90, rotationSpeed), emptyCell);

            Destroy(emptyCell.gameObject);
            b.gameObject.SetActive(true);

            await _animator.Play(new FlipAsync(b.transform, 360, 270, rotationSpeed * 2), b);
        }
    }
}
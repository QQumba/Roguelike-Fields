using Animations.AsyncAnimations;
using Cells;
using Game;
using TurnData;
using UnityEngine;
using UnityEngine.Events;

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

        [SerializeField]
        private UnityEvent turnFinishedEvent;
        
        public static GridController Instance { get; private set; }

        public TurnContext CurrentTurn { get; private set; }

        private void Awake()
        {
            Instance = this;

            _grid = GetComponent<Grid>();
            _spawner = GetComponent<CellSpawner>();
        }

        private void Start()
        {
            CreateNewTurn();
        }
        
        public void Move(Cell a, Cell b)
        {
            var indexOfA = _grid.IndexOf(a);
            var turnDirection = _grid.GetTurnDirection(a, b);
            var shiftDetails = _grid.GetShiftDetails(indexOfA, turnDirection);

            MoveCell(a, b);
            ShiftCells(indexOfA, shiftDetails);
            SpawnCell(shiftDetails.LastCellIndex);
        }

        private void MoveCell(Cell a, Cell b)
        {
            var indexOfB = _grid.IndexOf(b);
            CurrentTurn.Next(
                () => new ScaleAsync(b.transform, Vector3.zero).Play(b),
                "shrink cell");

            CurrentTurn.Next(() => { _grid.RemoveCell(b); }, "remove cell");

            CurrentTurn.Next(
                () => new MoveAsync(a.transform, _grid.GetCellPosition(indexOfB)).Play(a),
                "move cell to new position");

            CurrentTurn.Next(() => _grid.SetCell(a, indexOfB), "set cell to new position");
        }

        private void ShiftCells(Vector2Int index, CellShiftDetails shiftDetails)
        {
            var moveAction = new TurnAction();
            var setCellAction = new TurnAction();
            foreach (var c in shiftDetails.Cells)
            {
                var shiftToPosition = _grid.GetCellPosition(index);
                var capturedIndex = index;

                moveAction.Add(() => new MoveAsync(c.transform, shiftToPosition).Play(c));
                setCellAction.Add(() => _grid.SetCell(c, capturedIndex));

                index += shiftDetails.ShiftFrom;
            }

            CurrentTurn.Next(moveAction);
            CurrentTurn.Next(setCellAction);
        }

        private void SpawnCell(Vector2Int index)
        {
            var newCell = _spawner.SpawnCell(Vector3.zero);

            CurrentTurn.Next(() => _grid.SetCell(newCell, index));
            CurrentTurn.Next(() => new ScaleAsync(newCell.transform, Vector3.one).Play(newCell));
        }

        public void Replace(Cell a, Cell b)
        {
            var index = _grid.IndexOf(a);
            var position = _grid.GetCellPosition(index);
            var emptyCell = _spawner.SpawnEmptyCell();
            const float rotationSpeed = 3f;

            CurrentTurn.Next(
                () => new FlipAsync(a.transform, 90, 0, rotationSpeed * 2).Play(a),
                "flip cell");

            CurrentTurn.Next(() =>
            {
                _grid.RemoveCell(a);
                _grid.SetCell(b, index);
            }, "replace cell");

            CurrentTurn.Next(() =>
            {
                emptyCell.transform.position = position;
                return new FlipAsync(emptyCell.transform, 270, 90, rotationSpeed).Play(emptyCell);
            }, "flip empty cell");

            CurrentTurn.Next(() =>
            {
                Destroy(emptyCell.gameObject);
                b.gameObject.SetActive(true);
            }, "remove cell and set new cell active");

            CurrentTurn.Next(
                () => new FlipAsync(b.transform, 360, 270, rotationSpeed * 2).Play(b),
                "flip new cell to normal");
        }

        private void CreateNewTurn()
        {
            if (CurrentTurn != null)
            {
                CurrentTurn.TurnFinished -= CreateNewTurn;
                CurrentTurn.TurnFinished -= turnFinishedEvent.Invoke;

                foreach (var cell in _grid.Cells)
                {
                    cell.OnTurnEnded();
                }
            }
            
            var turnContext = new TurnContext(StartCoroutine);
            turnContext.TurnFinished += CreateNewTurn;
            turnContext.TurnFinished += turnFinishedEvent.Invoke;
            CurrentTurn = turnContext;
        }
    }
}
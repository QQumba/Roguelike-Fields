using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animations.Factories;
using Cells;
using Game.CellGenerator;
using TurnData;
using TurnData.FragmentedTurn;
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
        private AnimationFactory _animations;

        private int _powerLevel = 10;

        [SerializeField]
        private UnityEvent turnFinishedEvent;

        public static GridController Instance { get; private set; }

        public ITurnContext CurrentTurn { get; private set; }

        public int TurnCount { get; private set; }

        private void Awake()
        {
            Instance = this;

            _grid = GetComponent<Grid>();
            _spawner = GetComponent<CellSpawner>();
        }

        private void Start()
        {
            CreateNewTurn();

            _animations = AnimationFactory.Instance;
        }

        public void Move(Cell a, Cell b)
        {
            var indexOfA = _grid.IndexOf(a);

            var turnDirection = CurrentTurn.TurnDirection;
            var shiftDetails = _grid.GetShiftDetails(indexOfA, turnDirection);

            MoveCell(a, b);
            ShiftCells(indexOfA, shiftDetails);
            SpawnCell(shiftDetails.LastCellIndex);
        }

        public void Remove(Cell cell)
        {
            var index = _grid.IndexOf(cell);

            var shiftDetails = _grid.GetRandomShiftDetails(index, CurrentTurn.TurnDirection);

            CurrentTurn.Next(() => _animations.Shrink(cell.transform, Vector3.zero).Play(cell));
            CurrentTurn.Next(() => _grid.RemoveCell(cell));

            ShiftCells(index, shiftDetails);
            SpawnCell(shiftDetails.LastCellIndex);
        }

        public void ReplaceWithEmpty(Cell cell)
        {
            var emptyContent = _spawner.SpawnEmptyContent();
            Replace(cell, emptyContent);
        }

        public void ReplaceWithContent(Cell cell, CellContent contentPrefab)
        {
            var newCell = _spawner.SpawnCellWithContent(contentPrefab, Vector3.zero);
            Replace(cell, newCell);
        }

        private void Replace(Cell a, Cell b)
        {
            var index = _grid.IndexOf(a);
            var position = _grid.GetCellPosition(index);
            var emptyCell = _spawner.SpawnEmptyCell(Vector3.one);
            const float rotationSpeed = 1f;

            CurrentTurn.Next(() => _animations.Rotate(a.transform, 0, 90, rotationSpeed * 2).Play(a));

            CurrentTurn.Next(() =>
            {
                _grid.RemoveCell(a);
                _grid.SetCell(b, index);
            });

            CurrentTurn.Next(() =>
            {
                emptyCell.transform.position = position;
                return _animations.Rotate(emptyCell.transform, 90, 270, rotationSpeed).Play(emptyCell);
            });

            CurrentTurn.Next(() =>
            {
                Destroy(emptyCell.gameObject);
                b.transform.localScale = Vector3.one;
            });

            CurrentTurn.Next(() => _animations.Rotate(b.transform, 270, 360, rotationSpeed * 2).Play(b));
        }

        public void SwapCells(Cell a, Cell b)
        {
            var shrink = new TurnAction(new List<Func<IEnumerator>>
            {
                () => _animations.Shrink(a.transform, Vector3.zero).Play(a),
                () => _animations.Shrink(b.transform, Vector3.zero).Play(b)
            });

            var grow = new TurnAction(new List<Func<IEnumerator>>
            {
                () => _animations.Grow(a.transform, Vector3.one).Play(a),
                () => _animations.Grow(b.transform, Vector3.one).Play(b)
            });

            var indexOfA = _grid.IndexOf(a);
            var indexOfB = _grid.IndexOf(b);

            CurrentTurn.Next(shrink);
            CurrentTurn.Next(() =>
            {
                _grid.SetCell(a, indexOfB);
                _grid.SetCell(b, indexOfA);
            });
            CurrentTurn.Next(grow);
        }

        private void MoveCell(Cell a, Cell b)
        {
            var indexOfB = _grid.IndexOf(b);

            CurrentTurn.Next(() => _animations.Shrink(b.transform, Vector3.zero).Play(b));
            CurrentTurn.Next(() => _grid.RemoveCell(b));
            CurrentTurn.Next(() => _animations.Move(a.transform, _grid.GetCellPosition(indexOfB)).Play(a));
            CurrentTurn.Next(() => _grid.SetCell(a, indexOfB));
        }

        private void ShiftCells(Vector2Int index, CellShiftDetails shiftDetails)
        {
            var moveAction = new TurnAction();
            var setCellAction = new TurnAction();
            for (var i = 0; i < shiftDetails.Cells.Count; i++)
            {
                var c = shiftDetails.Cells[i];

                var shiftToPosition = _grid.GetCellPosition(index);
                var capturedIndex = index;

                moveAction.Add(() => _animations.Move(c.transform, shiftToPosition).Play(c), i == 0 ? 0 : 0.2f);
                setCellAction.Add(() => _grid.SetCell(c, capturedIndex));

                index += shiftDetails.ShiftFrom;
            }

            CurrentTurn.Next(moveAction);
            CurrentTurn.Next(setCellAction);
        }

        private void SpawnCell(Vector2Int index)
        {
            var newCell = _spawner.SpawnCell(GetSpawnerState(), Vector3.zero);

            CurrentTurn.Next(() => _grid.SetCell(newCell, index));
            CurrentTurn.Next(() => _animations.Grow(newCell.transform, Vector3.one).Play(newCell));
        }

        public CellSpawnerState GetSpawnerState()
        {
            var criteria = _grid.Cells
                .Where(x => x is not null)
                .Select(x => x.SpawnCriteria)
                .ToList();

            var state = new CellSpawnerState
            {
                CellSpawnCriteria = criteria,
                MaxPowerLevel = _powerLevel
            };

            return state;
        }

        private void CreateNewTurn()
        {
            if (CurrentTurn != null)
            {
                CurrentTurn.TurnFinished -= CreateNewTurn;
                CurrentTurn.TurnFinished -= turnFinishedEvent.Invoke;

                TurnCount++;

                // why 2?
                _powerLevel += 2;
            }

            var turnContext = new FragmentedTurnContext(StartCoroutine);
            turnContext.MainFragmentCompleted += () =>
            {
                foreach (var cell in _grid.Cells)
                {
                    turnContext.NextFragment(new TurnAction(() => cell.OnTurnEnded()));
                }

                turnContext.ContinueTurn();
            };
            turnContext.TurnFinished += CreateNewTurn;
            turnContext.TurnFinished += turnFinishedEvent.Invoke;
            CurrentTurn = turnContext;
        }
    }
}
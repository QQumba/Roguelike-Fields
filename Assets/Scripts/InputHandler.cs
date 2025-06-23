using Animations.AsyncAnimations;
using Cells;
using Cells.Components;
using GameGrid;
using TurnData;
using TurnData.FragmentedTurn;
using UnityEngine;
using Grid = GameGrid.Grid;

public class InputHandler : MonoBehaviour
{
    private Grid _grid;
    private GridController _controller;

    private bool _turnExecuting = true;

    private void Start()
    {
        _grid = FindObjectOfType<Grid>();
        _controller = FindObjectOfType<GridController>();

        CellSlot.CellClicked += HandleClick;
    }

    private void HandleClick(Cell cell)
    {
        // ignore input if turn is executing
        if (_turnExecuting)
        {
            return;
        }

        if (!_grid.IsCellAdjacentToHero(cell))
        {
            return;
        }

        PulseCell(cell);

        var turnDirection = _grid.GetTurnDirection(cell);

        _controller.CurrentTurn.TurnDirection = turnDirection;
        _controller.CurrentTurn.AddAction(() =>
        {
            var hero = _grid.Hero.GetCellComponent<Hero>();
            cell.Interaction.InteractWith(hero);
        });

        _turnExecuting = true;
        _controller.CurrentTurn.TurnFinished += () => _turnExecuting = false;
        _controller.CurrentTurn.StartTurn();
    }

    // need some rework
    private void PulseCell(Cell cell)
    {
        const float pulseSpeed = 16f;
        var pulseScale = new Vector3(0.85f, 0.85f, 1f);
        
        _controller.CurrentTurn.AddAction(() => new ScaleAsync(cell.transform, pulseScale, speed: pulseSpeed).Play(cell));
        // _controller.CurrentTurn.AddAction(() => new ScaleAsync(cell.transform, Vector3.one, speed: pulseSpeed).Play(cell));
    }
}
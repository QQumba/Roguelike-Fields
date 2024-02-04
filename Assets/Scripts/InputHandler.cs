using Animations.AsyncAnimations;
using Cells;
using Cells.Components;
using GameGrid;
using UnityEngine;
using Grid = GameGrid.Grid;

public class InputHandler : MonoBehaviour
{
    private Grid _grid;
    private GridController _controller;
    private AsyncAnimator _animator;

    private bool _waitForInput = true;

    private void Start()
    {
        _grid = FindObjectOfType<Grid>();
        _controller = FindObjectOfType<GridController>();
        _animator = AsyncAnimator.Instance;

        CellSlot.CellClicked += HandleClick;
    }

    private void HandleClick(Cell cell)
    {
        if (!_waitForInput)
        {
            return;
        }

        if (!_grid.IsCellAdjacentToHero(cell))
        {
            return;
        }

        _controller.CurrentTurn.Next(
            () => new ScaleAsync(cell.transform, new Vector3(0.85f, 0.85f, 1f), speed: 16).Play(cell),
            "scale cell to normal");
 
        _controller.CurrentTurn.Next(
            () => new ScaleAsync(cell.transform, Vector3.one, speed: 16).Play(cell),
            "scale cell to normal");

        _controller.CurrentTurn.Next(() =>
        {
            var hero = _grid.Hero.GetCellComponent<Hero>();
            cell.Accept(hero);
        }, "start turn");

        _waitForInput = false;
        _controller.CurrentTurn.TurnFinished += () => _waitForInput = true;
        _controller.CurrentTurn.StartTurn();

        // TODO wait for all animations to end
        // _controller.Move(hero.Cell, cell);
        // _controller.ReplaceWithNewCell(cell);
    }

    private void Paint(Cell cell)
    {
        var sr = cell.GetComponent<SpriteRenderer>();
        var color = Color.cyan;

        sr.color = sr.color == color ? Color.white : color;
    }
}
using Animations.AsyncAnimations;
using Cells;
using Cells.Components;
using GameGrid;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Grid = GameGrid.Grid;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro turnDirectionText;
    [SerializeField]
    private TextMeshPro shiftDirectionText;
    
    private Grid _grid;
    private GridController _controller;

    private bool _waitForInput = true;

    private void Start()
    {
        _grid = FindObjectOfType<Grid>();
        _controller = FindObjectOfType<GridController>();

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

        var hero = _grid.Hero;
        var turnDirection = _grid.GetTurnDirection(hero, cell);
        turnDirectionText.text = $"turn: {turnDirection}";
        var shiftDirection = _grid.GetShiftDirection(hero, cell);
        shiftDirectionText.text = $"shift: {shiftDirection}";

        var cellsToShift = _grid.GetCellsToShift(cell, turnDirection);
        foreach (var c in cellsToShift)
        {
            var cPosition = _grid.GetCellPosition(_grid.IndexOf(c));
            Debug.Log(cPosition);
        }
        
        PulseCell(cell);

        _controller.CurrentTurn.Next(() =>
        {
            var hero = _grid.Hero.GetCellComponent<Hero>();
            cell.Accept(hero);
        }, "start turn");

        _waitForInput = false;
        _controller.CurrentTurn.TurnFinished += () => _waitForInput = true;
        _controller.CurrentTurn.StartTurn();
    }

    private void PulseCell(Cell cell)
    {
        _controller.CurrentTurn.Next(
            () => new ScaleAsync(cell.transform, new Vector3(0.85f, 0.85f, 1f), speed: 16).Play(cell),
            "scale cell to normal");
 
        _controller.CurrentTurn.Next(
            () => new ScaleAsync(cell.transform, Vector3.one, speed: 16).Play(cell),
            "scale cell to normal");
    }
}
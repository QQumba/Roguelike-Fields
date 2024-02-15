﻿using Animations.AsyncAnimations;
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

        var heroCell = _grid.Hero;
        var turnDirection = _grid.GetTurnDirection(heroCell, cell);
        turnDirectionText.text = $"turn: {turnDirection}";

        PulseCell(cell);

        _controller.CurrentTurn.Next(() =>
        {
            var hero = _grid.Hero.GetCellComponent<Hero>();
            cell.Accept(hero);
        });

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
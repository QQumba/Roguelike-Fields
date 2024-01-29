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
    
    private void Start()
    {
        _grid = FindObjectOfType<Grid>();
        _controller = FindObjectOfType<GridController>();
        _animator = AsyncAnimator.Instance;

        CellSlot.CellClicked += HandleClick;
    }

    private void HandleClick(Cell cell)
    {
        if (_animator.IsAnimating)
        {
            return;
        }
        
        if (!_grid.IsCellAdjacentToHero(cell))
        {
            return;
        }

        var hero = _grid.Hero.GetCellComponent<Hero>();
        cell.Accept(hero);
        
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
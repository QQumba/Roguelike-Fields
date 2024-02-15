using System;
using System.Collections;
using System.Collections.Generic;
using Cells;
using GameGrid;
using Unity.VisualScripting;
using UnityEngine;
using Grid = GameGrid.Grid;

public class TouchHandler : MonoBehaviour
{
    [SerializeField]
    private float _triggerThreshhold = 1f;
    
    private bool _isTouching = false;
    private Vector3 _startTouchPosition;
    private Vector3 _currentTouchPosition;
    private Vector3 _normalVector;
    private Cell _currentCell;

    private Grid _grid;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        _grid = Grid.Instance;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            StartTouch();
        }

        if (Input.GetMouseButtonUp((int)MouseButton.Left))
        {
            EndTouch();
        }
        
        if (_isTouching)
        {
            HandleTouch();
        }
    }

    private void StartTouch()
    {
        _isTouching = true;
        _startTouchPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void EndTouch()
    {
        _isTouching = false;
        if (_currentCell is not null)
        {
            _currentCell.Highlight(false);
        }

        _currentCell = null;
    }

    private void HandleTouch()
    {
        var currentTouchPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var diff = currentTouchPosition - _startTouchPosition;
        
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            diff.y = 0;
        }
        else
        {
            diff.x = 0;
        }

        var heroIndex = _grid.IndexOf(_grid.Hero);
        if (diff.magnitude < _triggerThreshhold)
        {
            if (_currentCell is not null)
            {
                _currentCell.Highlight(false);
                _currentCell = null;
            }
            
            return;
        }

        var index = heroIndex + (Vector2)diff.normalized;
        var cell = _grid.GetCell((int)index.x, (int)index.y);
            
        if (cell is null)
        {
            return;
        }

        if (cell == _currentCell)
        {
            return;
        }
        
        if (_currentCell is not null)
        {
            _currentCell.Highlight(false);
            _currentCell = null;
        }

        cell.Highlight(true);
        _currentCell = cell;
    }

    private void OnDrawGizmos()
    {
        if (_isTouching)
        {
            Gizmos.color = Color.white;
            if (_normalVector.magnitude > _triggerThreshhold)
            {
                Gizmos.color = Color.red;
            }
            
            Gizmos.DrawLine(_startTouchPosition, _startTouchPosition + _normalVector);
        }
    }
}

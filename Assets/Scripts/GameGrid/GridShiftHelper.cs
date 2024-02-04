using System;
using System.Collections.Generic;
using Cells;
using UnityEngine;

namespace GameGrid
{
    public static class GridShiftExtensions
    {
        /// <summary>
        /// Only work for vertical and horizontal movements
        /// </summary>
        public static Direction GetTurnDirection(this Grid grid, Cell a, Cell b)
        {
            var indexOfA = grid.IndexOf(a);
            var indexOfB = grid.IndexOf(b);

            return VectorToDirection(indexOfB - indexOfA);
        }

        public static Direction GetShiftDirection(this Grid grid, Cell a, Cell b)
        {
            var turnDirection = grid.GetTurnDirection(a, b);
            var shiftDirection = (Direction)(((int)turnDirection + 1) % 4);
            return shiftDirection;
        }

        public static List<Cell> GetCellsToShift(this Grid grid, Cell cell, Direction turnDirection)
        {
            var shiftFromDirection = (Direction)(((int)turnDirection + 2) % 4);
            var shiftFromIndex = DirectionToIndex(shiftFromDirection);

            var cellIndex = grid.IndexOf(cell);

            var cells = new List<Cell>();
            var index = cellIndex + shiftFromIndex;
            while (grid.IsIndexInBounds(index))
            {
                cells.Add(grid.GetCell(index));
                index += shiftFromIndex;
            }

            return cells;
        }

        public static Direction VectorToDirection(Vector2Int vector)
        {
            if (vector == Vector2Int.up)
            {
                return Direction.Up;
            }

            if (vector == Vector2Int.right)
            {
                return Direction.Right;
            }

            if (vector == Vector2Int.down)
            {
                return Direction.Down;
            }

            if (vector == Vector2Int.left)
            {
                return Direction.Left;
            }

            throw new ArgumentOutOfRangeException();
        }
        
        public static Vector2Int DirectionToIndex(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Vector2Int.up,
                Direction.Right => Vector2Int.right,
                Direction.Down => Vector2Int.down,
                Direction.Left => Vector2Int.left,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    // this is just for readability
    // for actual usage Vector2 looks more usable
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
}
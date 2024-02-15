using System;
using System.Collections.Generic;
using System.Linq;
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
            var shiftDirection = turnDirection.NextDirectionClockwise();
            return shiftDirection;
        }

        public static CellShiftDetails GetShiftDetails(this Grid grid, Cell cell, Direction turnDirection)
        {
            var cellIndex = grid.IndexOf(cell);
            return grid.GetShiftDetails(cellIndex, turnDirection);
        }

        public static CellShiftDetails GetShiftDetails(this Grid grid, Vector2Int cellIndex, Direction turnDirection)
        {
            var cellShiftDetails = new List<CellShiftDetails>();
            
            for (int i = 1; i < 4; i++)
            {
                var currentShiftFromDirection = turnDirection.NextDirectionClockwise(i);
                cellShiftDetails.Add(grid.GetCellsFromDirection(cellIndex, currentShiftFromDirection));
            }

            var maxCellsCount = cellShiftDetails.Select(x => x.Cells.Count).Max();
            return cellShiftDetails.First(x => x.Cells.Count == maxCellsCount);
        }

        private static CellShiftDetails GetCellsFromDirection(this Grid grid, Vector2Int startIndex, Direction direction)
        {
            var shiftFromIndex = ToIndex(direction);
            var cellShiftDetails = new CellShiftDetails
            {
                ShiftFrom = direction.ToIndex()
            };
            
            var index = startIndex + shiftFromIndex;
            while (grid.IsIndexInBounds(index))
            {
                cellShiftDetails.Cells.Add(grid.GetCell(index));
                cellShiftDetails.LastCellIndex = index; 
                index += shiftFromIndex;
            }
            
            return cellShiftDetails;
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

        public static Direction NextDirectionClockwise(this Direction direction, int offset = 1)
        {
            return (Direction)(((int)direction + offset) % 4);
        }

        public static Vector2Int ToIndex(this Direction direction)
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
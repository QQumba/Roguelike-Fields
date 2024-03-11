using System.Collections.Generic;
using System.Text;
using Cells;
using UnityEngine;

namespace GameGrid
{
    public static class GridLogger
    {
        public static void LogCells(IEnumerable<Cell> cells)
        {
            var sb = new StringBuilder();
            foreach (var cell in cells)
            {
                sb.Append(cell.name).Append(',');
            }

            if (sb.Length > 0)
            {
                sb.Length--;
            }
            
            Debug.Log(sb.ToString());            
        }   
    }
}
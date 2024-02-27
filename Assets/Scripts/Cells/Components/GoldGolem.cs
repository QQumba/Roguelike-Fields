using System.Linq;
using UnityEngine;
using Grid = GameGrid.Grid;

namespace Cells.Components
{
    public class GoldGolem : CellComponent
    {
        public override string CellTag => "gold-golem";

        public override void OnTurnEnded()
        {
            base.OnTurnEnded();
            
            StealGold();
        }

        public void StealGold()
        {
            var grid = Grid.Instance;
            var adjacentCells = grid.GetAdjacentCells(Cell);
            // TODO create a way to find a coin cell (by tag or component)
        }
    }
}
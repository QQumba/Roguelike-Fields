using System;
using System.Collections.Generic;
using System.Linq;
using GameGrid;
using TurnData;
using UnityEngine;
using Grid = GameGrid.Grid;
using Random = UnityEngine.Random;

namespace Cells.Components
{
    public class Bomb : CellComponent
    {
        [SerializeField]
        private int damage = 6;
        
        [SerializeField]
        private ValueProvider turnsToExplosion;
        
        public override string CellTag => "bomb";
        
        public override void OnTurnEnded(TurnAction turnAction)
        {
            base.OnTurnEnded(turnAction);

            turnsToExplosion.Value--;
            
            if (turnsToExplosion.Value == 0)
            {
                // var currentTurn = GridController.Instance.CurrentTurn;
                turnAction.Next(Explode);
            }
        }

        // public override void OnTurnStarted()
        // {
        //     base.OnTurnStarted();
        //
        //     if (turnToExplosion == 0)
        //     {
        //         Explode();
        //     }
        // }

        private void Explode()
        {
            var damageables = GetSurroundingDamageables();
            foreach (var damageable in damageables)
            {
                damageable.DealDamage(damage);
            }

            return;
            
            // looks sus                    |
            //                             \/
            // not working correctly at a start of a turn 
            var grid = Grid.Instance;
            var direction = Enum.Parse<Direction>(Random.Range(0, 4).ToString());

            if (grid.IsCellAdjacentToHero(Cell) && grid.GetTurnDirection(Cell, grid.Hero) == direction)
            {
                direction.NextDirectionClockwise();
            }
            
            grid.GetShiftDetails(Cell, direction);
        }

        private IEnumerable<Damageable> GetSurroundingDamageables()
        {
            var grid = Grid.Instance;
 
            var damageables = grid.GetAdjacentCells(Cell)
                .Where(x => x.HasCellComponent<Damageable>())
                .Select(x => x.GetCellComponent<Damageable>());
            
            return damageables;
        }
    }
}
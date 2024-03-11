using System;
using System.Collections.Generic;
using System.Linq;
using Cells.Components.Interfaces;
using GameGrid;
using TurnData;
using TurnData.FragmentedTurn;
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

        private GridController _gridController;
        
        public override string CellTag => "bomb";

        private void Start()
        {
            _gridController = GridController.Instance;
        }

        public override void OnTurnEnded()
        {
            base.OnTurnEnded();

            turnsToExplosion.Value--;
            
            if (turnsToExplosion.Value == 0)
            {
                Explode();
            }
        }

        private void Explode()
        {
            var damageables = GetSurroundingDamageables();
            foreach (var damageable in damageables)
            {
                _gridController.CurrentTurn.Next(() => damageable.DealDamage(damage));    
            }

            _gridController.CurrentTurn.Next(() =>
                _gridController.CurrentTurn.Next(() =>
                    _gridController.Remove(Cell)));
        }

        private IEnumerable<IDamageable> GetSurroundingDamageables()
        {
            var grid = Grid.Instance;
 
            var damageables = grid.GetAdjacentCells(Cell)
                .Where(x => x.HasCellComponent<IDamageable>())
                .Select(x => x.GetCellComponent<IDamageable>());
            
            return damageables;
        }
    }
}
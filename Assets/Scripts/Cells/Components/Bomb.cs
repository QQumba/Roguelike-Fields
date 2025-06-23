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

        public override string CellTag => "bomb";

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
            var controller = GridController.Instance;
            var turn = controller.CurrentTurn;

            var damageables = Grid.Instance.GetAdjacentCellsComponent<IDamageable>(Cell);
            foreach (var damageable in damageables)
            {
                turn.AddAction(() => damageable.DealDamage(damage));
            }

            turn.AddAction(() => turn.AddAction(() => controller.Remove(Cell)));
        }
    }
}
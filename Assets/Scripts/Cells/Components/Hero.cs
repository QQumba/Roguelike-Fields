using Events;
using GameGrid;
using Tags;
using UnityEngine;
using UnityEngine.Events;

namespace Cells.Components
{
    public class Hero : CellComponent, IVisitor
    {
        [SerializeField]
        private UnityEvent<CellEventArgs> enemyAttackedEvent;
        
        public GridController GridController { get; set; }

        public override string DefaultTag => CellTags.Hero;

        public void Visit(Enemy enemy)
        {
            enemyAttackedEvent.Invoke(new CellEventArgs(enemy.Cell));
            enemy.Damageable.DealDamage(999);
        }

        public void Visit(Pickable pickable)
        {
            pickable.PickUp();
            GridController.Move(Cell, pickable.Cell);
        }
    }
}
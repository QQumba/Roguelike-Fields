using Cells.Weapons;
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

        private GridController _controller;

        public Weapon Weapon { get; set; }

        public Damageable Damageable => Cell.GetCellComponent<Damageable>();

        protected override void Initialize()
        {
            _controller = GridController.Instance;
        }

        public override string CellTag => CellTags.Hero;

        public void Visit(Enemy enemy)
        {
            if (Weapon is null)
            {
                var enemyHealth = enemy.Health.Value;
                Damageable.DealDamage(enemyHealth);
                _controller.Move(Cell, enemy.Cell);
                return;
            }

            Weapon.Attack(enemy);
            
            // enemyAttackedEvent.Invoke(new CellEventArgs(enemy.Cell));
            // enemy.Damageable.DealDamage(999);
        }

        public void Visit(Pickable pickable)
        {
            pickable.PickUp();
            
            _controller.Move(Cell, pickable.Cell);
        }

        public void Visit(Activatable activatable)
        {
            activatable.Activate();
        }
    }
}
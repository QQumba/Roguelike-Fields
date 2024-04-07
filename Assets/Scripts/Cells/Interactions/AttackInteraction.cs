using Cells.Components;
using GameGrid;

namespace Cells.Interactions
{
    public class AttackInteraction : Interaction
    {
        public override void InteractWith(Hero hero)
        {
            var enemy = Cell.GetCellComponent<Enemy>();
            var controller = GridController.Instance;

            if (hero.Weapon is null)
            {
                var enemyHealth = enemy.Health.Value;
                hero.Damageable.DealDamage(enemyHealth);
                controller.Move(hero.Cell, Cell);
                return;
            }

            hero.Weapon.Attack(enemy);
        }
    }
}
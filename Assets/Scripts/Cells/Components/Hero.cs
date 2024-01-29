using GameGrid;
using Tags;

namespace Cells.Components
{
    public class Hero : CellComponent, IVisitor
    {
        public GridController GridController { get; set; }

        public override string DefaultTag => CellTags.Hero;

        public void Visit(Enemy enemy)
        {
            enemy.Damageable.DealDamage(999);
        }

        public void Visit(Pickable pickable)
        {
            pickable.PickUp();
            GridController.Move(Cell, pickable.Cell);
        }
    }
}
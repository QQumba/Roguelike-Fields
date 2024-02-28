using Tags;

namespace Cells.Components
{
    public class Enemy : CellComponent, IVisitable
    {
        public Damageable Damageable => Cell.GetCellComponent<Damageable>();

        public Health Health => Cell.GetCellComponent<Health>();

        public void Accept(IVisitor visitor)
        {
            // visitor.Visit(this);
        }

        public override string CellTag => CellTags.Enemy;
    }
}
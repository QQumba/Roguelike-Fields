using Cells.Components.Interfaces;
using Tags;

namespace Cells.Components
{
    public class Enemy : CellComponent, IVisitable
    {
        public IDamageable Damageable => Cell.GetCellComponent<IDamageable>();

        public ValueProvider Health => Damageable.Health;

        public void Accept(IVisitor visitor)
        {
            // visitor.Visit(this);
        }

        public override string CellTag => CellTags.Enemy;
    }
}
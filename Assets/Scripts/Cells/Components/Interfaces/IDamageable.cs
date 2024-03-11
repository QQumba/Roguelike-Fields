namespace Cells.Components.Interfaces
{
    public interface IDamageable : ICellComponent
    {
        ValueProvider Health { get; }

        int DealDamage(int damage);

        void Eliminate();
    }
}
namespace Cells.Components.Interfaces
{
    public interface IHealable : ICellComponent
    {
        int ApplyHealing(int healing);
    }
}
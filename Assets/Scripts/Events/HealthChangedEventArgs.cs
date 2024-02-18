using Cells;

namespace Events
{
    public class HealthChangedEventArgs : ValueChangedEventArgs
    {
        public HealthChangedEventArgs(Cell cell) : base(cell)
        {
        }
    }
}
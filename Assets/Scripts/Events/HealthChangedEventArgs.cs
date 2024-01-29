using Cells;

namespace Events
{
    public class HealthChangedEventArgs : CellEventArgs
    {
        public HealthChangedEventArgs(Cell cell) : base(cell)
        {
        }

        public int CurrentValue { get; set; }

        public int PreviousValue { get; set; }

        public int MaxValue { get; set; }
    }
}
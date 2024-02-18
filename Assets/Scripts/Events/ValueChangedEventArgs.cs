using Cells;

namespace Events
{
    public class ValueChangedEventArgs : CellEventArgs
    {
        public ValueChangedEventArgs(Cell cell) : base(cell)
        {
        }

        public int CurrentValue { get; set; }

        public int PreviousValue { get; set; }

        public int MaxValue { get; set; }
    }
}
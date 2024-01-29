using Cells;

namespace Events
{
    public class CellEventArgs
    {
        public CellEventArgs(Cell cell)
        {
            Cell = cell;
        }

        public Cell Cell { get; set; }
    }
}
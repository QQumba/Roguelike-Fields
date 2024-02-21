namespace Cells.Components
{
    // bad name
    public class Swappable : CellComponent, IVisitable
    {
        public override string CellTag => "swappable";

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
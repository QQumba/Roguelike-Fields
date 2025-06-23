namespace Cells.Components
{
    /// <summary>
    /// Mark cells that will swap place with player when activated
    /// </summary>
    public class Swappable : CellComponent, IVisitable
    {
        public override string CellTag => "swappable";

        public void Accept(IVisitor visitor)
        {
            // visitor.Visit(this);
        }
    }
}
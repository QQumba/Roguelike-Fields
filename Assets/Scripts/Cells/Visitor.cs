using Cells.Components;

namespace Cells
{
    public interface IVisitor
    {
        void Visit(Enemy enemy);

        void Visit(Pickable pickable);
    }

    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}
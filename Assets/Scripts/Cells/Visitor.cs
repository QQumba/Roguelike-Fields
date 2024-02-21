using Cells.Components;

namespace Cells
{
    public interface IVisitor
    {
        void Visit(Enemy enemy);

        void Visit(Pickable pickable);
        void Visit(Activatable activatable);
        void Visit(Swappable swappable);
    }

    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}
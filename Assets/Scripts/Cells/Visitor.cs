using Cells.Interactions;

namespace Cells
{
    public interface IVisitor
    {
        void Visit(AttackInteraction enemy);

        void Visit(PickUpInteraction pickable);
        void Visit(ActivateInteraction activatable);
        void Visit(SwapInteraction swappable);
    }

    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}
using Cells.Components;

namespace Cells.Interactions
{
    public class ActivateInteraction : Interaction
    {
        public override void InteractWith(Hero hero)
        {
            var activatable = Cell.GetCellComponent<Activatable>();
            activatable.Activate();
        }
    }
}
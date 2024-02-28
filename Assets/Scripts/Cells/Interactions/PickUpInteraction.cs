using Cells.Components;
using GameGrid;

namespace Cells.Interactions
{
    public class PickUpInteraction : Interaction
    {
        public override void InteractWith(Hero hero)
        {
            var pickable = Cell.GetCellComponent<Pickable>();
            var controller = GridController.Instance;
            
            pickable.PickUp();
            controller.Move(hero.Cell, Cell);
        }
    }
}
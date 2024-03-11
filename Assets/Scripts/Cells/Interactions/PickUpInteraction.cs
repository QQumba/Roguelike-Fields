using Cells.Components;
using Cells.Components.Interfaces;
using GameGrid;

namespace Cells.Interactions
{
    public class PickUpInteraction : Interaction
    {
        public override void InteractWith(Hero hero)
        {
            var controller = GridController.Instance;
            
            var pickables = Cell.GetCellComponents<IPickable>();
            foreach (var p in pickables)
            {
                p.PickUp();
            }
            
            controller.Move(hero.Cell, Cell);
        }
    }
}
using Cells.Components;
using GameGrid;

namespace Cells.Interactions
{
    public class SwapInteraction : Interaction
    {
        public override void InteractWith(Hero hero)
        {
            var controller = GridController.Instance;
            controller.SwapCells(hero.Cell, Cell);
        }
    }
}
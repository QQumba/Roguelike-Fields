using Cells.Components.Interfaces;
using Game;

namespace Cells.Components
{
    public class GoldCoin : CellComponent, IPickable
    {
        public override string CellTag => "gold-coin";

        public void PickUp()
        {
            CoinCounter.Instance.AddCoin();
        }
    }
}
using System.Linq;
using Cells.Components.Interfaces;
using UnityEngine;
using Grid = GameGrid.Grid;

namespace Cells.Components
{
    public class HealingPotion : CellComponent, IPickable
    {
        [SerializeField] private int healingValue;
        [SerializeField] private string healableTag;

        [SerializeField] private ValueProvider healingValueProvider;

        public override string CellTag => "healing-potion";
        
        private Grid _grid;

        private void Start()
        {
            _grid = Grid.Instance;
        }
        
        public void PickUp()
        {
            // get cell with correct tag and damageable component
            var cell = _grid.Cells.FirstOrDefault(x =>
            {
                var isDamageable = x.HasCellComponent<IHealable>();
                var hasCorrectTag = x.HasCellTag(healableTag);
                return isDamageable && hasCorrectTag;
            });

            if (cell == null)
            {
                return;
            }
            
            var damageable = cell.GetCellComponent<IHealable>();

            damageable.ApplyHealing(healingValueProvider.Value);
        }
    }
}
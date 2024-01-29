using System.Linq;
using Cells.Components;
using Events;
using UnityEngine;
using Grid = GameGrid.Grid;

namespace Effects
{
    public class HealingEffect : MonoBehaviour
    {
        [SerializeField] private int healingValue;
        [SerializeField] private string healableTag;
        
        private Grid _grid;

        private void Start()
        {
            _grid = Grid.Instance;
        }

        public void Heal(CellEventArgs args)
        {
            // get cell with correct tag and damageable component
            var cell = _grid.Cells.FirstOrDefault(x =>
            {
                var isDamageable = x.HasCellComponent<Damageable>();
                var hasCorrectTag = x.HasCellTag(healableTag);
                return isDamageable && hasCorrectTag;
            });

            if (cell == null)
            {
                return;
            }
            
            var damageable = cell.GetCellComponent<Damageable>();

            damageable.Heal(healingValue);
        }
    }
}
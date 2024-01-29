using Cells;
using Cells.Components;
using Effects.Utilities;
using UnityEngine;

namespace Effects
{
    public class HealingEffectLegacy : Effect
    {
        [SerializeField] private EffectTarget target;

        public override void Apply(Cell cell)
        {
            var heal = cell.GetCellComponent<Health>().Value;
            var cells = target.GetTargets(cell);

            foreach (var c in cells)
            {
                var damageable = c.GetCellComponent<Damageable>();
                if (damageable != null)
                {
                    damageable.Heal(heal);
                }
            }
        }
    }
}
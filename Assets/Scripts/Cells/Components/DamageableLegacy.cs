using System;
using Cells.Components.Interfaces;
using Events;
using Tags;
using UnityEngine;
using UnityEngine.Events;

namespace Cells.Components
{
    /// <summary>
    /// Cell component that allow cell to damage and heal the cell.
    /// </summary>
    public sealed class DamageableLegacy : CellComponent, IDamageable, IHealable
    {
        [SerializeField]
        private UnityEvent<CellEventArgs> diedEvent;

        // it looks like something that can be put on health component
        // or health and damageable can be even merged together
        [SerializeField]
        private UnityEvent<CellEventArgs> healedEvent;

        public ValueProvider Health => Cell.GetCellComponent<ValueProvider>();
        
        public event Action<CellEventArgs> Died;
        
        public override string CellTag => CellTags.Damageable;

        private void Awake()
        {
            Died += diedEvent.Invoke;
        }

        public int DealDamage(int damage)
        {
            var damageDealt = Mathf.Min(damage, Health.Value);
            Health.Value -= damageDealt;
            if (Health.Value <= 0)
            {
                Eliminate();    
            }

            return damageDealt;
        }

        public int ApplyHealing(int healing)
        {
            var heathRestored = Mathf.Min(healing, Health.MaxValue - Health.Value);
            Health.Value += heathRestored;
            healedEvent.Invoke(new CellEventArgs(Cell));

            return heathRestored;
        }

        public void Eliminate()
        {
            Died?.Invoke(new CellEventArgs(Cell));
        }
    }
}
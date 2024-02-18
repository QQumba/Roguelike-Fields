using System;
using Events;
using Tags;
using UnityEngine;
using UnityEngine.Events;

namespace Cells.Components
{
    /// <summary>
    /// Cell component that allow cell to damage and heal the cell.
    /// </summary>
    public sealed class Damageable : CellComponent
    {
        [SerializeField]
        private UnityEvent<CellEventArgs> diedEvent;

        // it looks like something that can be put on health component
        // or health and damageable can be even merged together
        [SerializeField]
        private UnityEvent<CellEventArgs> healedEvent;
        
        public Health Health => Cell.GetCellComponent<Health>();

        public event Action<CellEventArgs> Died;
        
        public override string CellTag => CellTags.Damageable;

        protected void Awake()
        {
            Died += diedEvent.Invoke;
        }

        public int DealDamage(int damage)
        {
            var damageDealt = Mathf.Min(damage, Health.Value);
            Health.Value -= damage;
            if (Health.Value <= 0)
            {
                Kill();    
            }

            return damageDealt;
        }

        public void Heal(int healing)
        {
            Health.Value += healing;
            healedEvent.Invoke(new CellEventArgs(Cell));
        }

        public void Kill()
        {
            Died?.Invoke(new CellEventArgs(Cell));
        }
    }
}
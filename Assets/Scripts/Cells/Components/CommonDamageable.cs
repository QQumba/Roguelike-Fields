using Cells.Components.Interfaces;
using Events;
using Tags;
using UnityEngine;
using UnityEngine.Events;

namespace Cells.Components
{
    public class CommonDamageable : CellComponent, IDamageable, IHealable
    {
        [SerializeField]
        private ValueProvider health;

        [SerializeField]
        private UnityEvent<CellEventArgs> diedEvent;

        [SerializeField]
        private UnityEvent<CellEventArgs> healingAppliedEvent;
        
        public override string CellTag => CellTags.Damageable;

        public ValueProvider Health => health;

        public int DealDamage(int damage)
        {
            var damageDealt = Mathf.Min(damage, Health.Value);
            Health.Value -= damage;
            if (Health.Value <= 0)
            {
                Eliminate();    
            }

            return damageDealt;
        }
        
        public int ApplyHealing(int healing)
        {
            var heathRestored = Mathf.Min(healing, Health.MaxValue - Health.Value);
            health.Value += healing;

            healingAppliedEvent.Invoke(new CellEventArgs(Cell));
            return heathRestored;
        }

        public void Eliminate()
        {
            diedEvent.Invoke(new CellEventArgs(Cell));
        }
    }
}
﻿using System;
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

        public Health Health => Cell.GetCellComponent<Health>();

        public event Action<CellEventArgs> Died;

        public override string DefaultTag => CellTags.Damageable;

        protected override void Initialize()
        {
            Died += diedEvent.Invoke;
        }

        public void DealDamage(int damage)
        {
            Health.Value -= damage;
            if (Health.Value <= 0)
            {
                Kill();    
            }
        }

        public void Heal(int healing)
        {
            Health.Value += healing;
        }

        public void Kill()
        {
            Died?.Invoke(new CellEventArgs(Cell));
        }
    }
}
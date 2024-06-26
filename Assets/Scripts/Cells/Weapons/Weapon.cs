﻿using System;
using Cells.Components;
using UnityEngine;

namespace Cells.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract event Action WeaponBroken;
        
        public int BaseDamage { get; set; }
        
        protected ValueProvider Damage { get; set; }
        
        public abstract void Attack(Enemy enemy);

        public abstract void BindDamageValueProvider(ValueProvider valueProvider);

        public abstract bool TryReinforce(Weapon weapon);

        public abstract void BreakWeapon();
    }
}
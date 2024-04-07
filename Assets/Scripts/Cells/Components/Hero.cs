using Cells.Components.Interfaces;
using Cells.Weapons;
using Tags;
using UnityEngine;

namespace Cells.Components
{
    public class Hero : CellComponent
    {
        [SerializeField]
        private ValueProvider weaponDamageValueProvider;
        
        public Weapon Weapon { get; private set; }

        public IDamageable Damageable => Cell.GetCellComponent<IDamageable>();

        public override string CellTag => CellTags.Hero;

        public void EquipWeapon(Weapon weapon)
        {
            if (Weapon is not null)
            {
                if (Weapon.TryReinforce(weapon))
                {
                    return;
                }

                Weapon.BreakWeapon();
            }

            // TODO replace this with instantiate from prefab
            weapon.transform.SetParent(transform);
            Weapon = weapon;

            weapon.BindDamageValueProvider(weaponDamageValueProvider);
            weapon.WeaponBroken += () =>
            {
                Destroy(Weapon.gameObject);
                weaponDamageValueProvider.Dispose();
                Weapon = null;
            };
        }
    }
}
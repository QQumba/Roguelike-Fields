using Cells.Interactions;
using Cells.Weapons;
using Events;
using GameGrid;
using Tags;
using UnityEngine;
using UnityEngine.Events;

namespace Cells.Components
{
    public class Hero : CellComponent
    {
        [SerializeField]
        private UnityEvent<CellEventArgs> enemyAttackedEvent;

        [SerializeField]
        private ValueProvider weaponDamageValueProvider;
        
        private GridController _controller;

        public Weapon Weapon { get; private set; }

        public Damageable Damageable => Cell.GetCellComponent<Damageable>();

        protected void Awake()
        {
            _controller = GridController.Instance;
        }

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
        
        // public void Visit(AttackInteraction enemy)
        // {
        //     if (Weapon is null)
        //     {
        //         var enemyHealth = enemy.Health.Value;
        //         Damageable.DealDamage(enemyHealth);
        //         _controller.Move(Cell, enemy.Cell);
        //         return;
        //     }
        //
        //     Weapon.Attack(enemy);
        // }
        //
        // public void Visit(PickUpInteraction pickable)
        // {
        //     pickable.PickUp();
        //     
        //     _controller.Move(Cell, pickable.Cell);
        // }
        //
        // public void Visit(ActivateInteraction activatable)
        // {
        //     activatable.Activate();
        // }
        //
        // public void Visit(SwapInteraction swappable)
        // {
        //     _controller.SwapCells(Cell, swappable.Cell);
        // }
    }
}
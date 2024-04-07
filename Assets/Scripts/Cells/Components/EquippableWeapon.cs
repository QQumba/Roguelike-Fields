using Cells.Components.Interfaces;
using Cells.Weapons;
using UnityEngine;
using Grid = GameGrid.Grid;

namespace Cells.Components
{
    public class EquippableWeapon : CellComponent, IPickable
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private ValueProvider damage;
        
        public override string CellTag => "equippable-weapon";

        public void PickUp()
        {
            weapon.BaseDamage = damage.Value;
            
            var hero = Grid.Instance.Hero.GetCellComponent<Hero>();
            hero.EquipWeapon(weapon);
        }
    }
}
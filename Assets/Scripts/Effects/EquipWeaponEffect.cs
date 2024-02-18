using Cells;
using Cells.Components;
using Cells.Weapons;
using Events;
using UnityEngine;
using UnityEngine.Serialization;
using Grid = GameGrid.Grid;

namespace Effects
{
    public class EquipWeaponEffect : MonoBehaviour
    {
        [SerializeField]
        private Weapon weapon;
        
        private Grid _grid;

        private void Start()
        {
            _grid = Grid.Instance;
        }

        public void Equip(CellEventArgs _)
        {
            var hero = _grid.Hero.GetCellComponent<Hero>();

            if (hero.Weapon is null)
            {
                weapon.transform.SetParent(hero.transform);
                hero.Weapon = weapon;
                return;
            }

            Debug.Log(weapon.GetType());
            if (hero.Weapon.GetType() == weapon.GetType())
            {
                hero.Weapon.Damage += weapon.Damage;
                return;
            }
            
            Destroy(hero.Weapon.gameObject);
            weapon.transform.SetParent(hero.transform);
            hero.Weapon = weapon;
        }
    }
}
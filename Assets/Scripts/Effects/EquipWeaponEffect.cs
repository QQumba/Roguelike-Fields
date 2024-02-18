using Cells.Components;
using Cells.Weapons;
using Events;
using UnityEngine;
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
            hero.EquipWeapon(weapon);
        }
    }
}
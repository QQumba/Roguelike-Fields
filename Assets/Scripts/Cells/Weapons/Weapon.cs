using Cells.Components;
using UnityEngine;

namespace Cells.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public int Damage { get; set; }
        
        public abstract void Attack(Enemy enemy);
    }
}
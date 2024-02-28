using Cells.Components;
using UnityEngine;

namespace Cells.Interactions
{
    public abstract class Interaction : MonoBehaviour
    {
        public Cell Cell { get; set; }
        
        public abstract void InteractWith(Hero hero);
    }
}
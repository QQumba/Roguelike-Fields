using Cells;
using UnityEngine;

namespace Effects
{
    public abstract class Effect : MonoBehaviour
    {
        public abstract void Apply(Cell cell);
    }
}

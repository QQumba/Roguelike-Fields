using System.Collections.Generic;
using System.Linq;
using Cells.Components;
using UnityEngine;

namespace Cells
{
    public class CellContent : MonoBehaviour
    {
        public IEnumerable<CellComponent> GetCellComponents()
        {
            var components = GetComponentsInChildren<CellComponent>();
            return components;
        }

        public int GetSortingOrder()
        {
            var sprite = GetComponentsInChildren<SpriteRenderer>().First();
            return sprite.sortingOrder;
        }
    }
}
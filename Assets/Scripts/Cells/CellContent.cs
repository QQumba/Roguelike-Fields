﻿using System.Collections.Generic;
using System.Linq;
using Cells.Components;
using Cells.Interactions;
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

        public bool TryGetInteraction(out Interaction interaction)
        {
            interaction = GetComponentInChildren<Interaction>();
            return interaction is not null;
        }

        public int GetSortingOrder()
        {
            var sprite = GetComponentsInChildren<SpriteRenderer>().First();
            return sprite.sortingOrder;
        }
    }
}
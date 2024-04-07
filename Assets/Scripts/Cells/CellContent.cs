using System.Collections.Generic;
using System.Linq;
using Cells.Components;
using Cells.Interactions;
using Game.CellGenerator;
using UnityEngine;

namespace Cells
{
    public class CellContent : MonoBehaviour
    {
        [SerializeField] private CellSpawnCriteria spawnCriteria;

        public CellSpawnCriteria SpawnCriteria => spawnCriteria;
        
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

        public int GetSortingOrder(int defaultOrder)
        {
            var sprite = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault();
            return sprite is null ? defaultOrder : sprite.sortingOrder;
        }
    }
}
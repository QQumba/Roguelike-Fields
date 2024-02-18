using System;
using System.Collections.Generic;
using System.Linq;
using Cells;
using Cells.Components;
using GameGrid;
using Tags;
using UnityEngine;
using UnityEngine.Serialization;
using Grid = GameGrid.Grid;
using Random = UnityEngine.Random;

namespace Game
{
    /// <summary>
    /// Allow to spawn new cells. 
    /// </summary>
    public class CellSpawner : MonoBehaviour
    {
        [SerializeField]
        private Cell cellPrefab;

        [SerializeField]
        private CellContent heroContent;

        [SerializeField]
        private CellContent[] prefabs;

        public static CellSpawner Instance;

        private Grid _grid;

        // ReSharper disable once Unity.NoNullCoalescing
        private Grid Grid => _grid ?? Grid.Instance;

        private void Awake()
        {
            Instance = this;
        }

        public Cell SpawnEmptyCell(Vector3? initialScale = null)
        {
            var cell = Instantiate(cellPrefab);
            initialScale ??= Vector3.one;
            cell.transform.localScale = initialScale.Value;

            return cell;
        }

        public Cell SpawnHero()
        {
            var cell = SpawnEmptyCell();
            var hero = Instantiate(heroContent, cell.transform);

            // I assume that hero have same sorting layer as cell and higher order within that layer then other components
            var cellRenderer = cell.GetComponent<SpriteRenderer>();
            cellRenderer.sortingOrder = hero.GetSortingOrder() - 1;

            foreach (var component in hero.GetCellComponents())
            {
                cell.AddCellComponent(component);
            }
            
            return cell;
        }

        public Cell SpawnCell(Vector3? initialScale = null)
        {
            var cell = SpawnEmptyCell(initialScale);

            var i = Random.Range(0, prefabs.Length);
            var prefabToSpawn = prefabs[i];
            var content = Instantiate(prefabToSpawn, cell.transform);

            var cellRenderer = cell.GetComponent<SpriteRenderer>();
            cellRenderer.sortingOrder = content.GetSortingOrder() - 1;

            var components = content.GetCellComponents();
            foreach (var component in components)
            {
                cell.AddCellComponent(component);
            }

            return cell;
        }

        private void ReadSpawnableCellsTags()
        {
            prefabs
                .SelectMany(x => x.GetCellComponents().Select(c => c.CellTag))
                .Distinct()
                .ToList()
                .ForEach(Debug.Log);
        }

        private string GetLeastPresentTag()
        {
            var tagNumber = new Dictionary<string, int>();
            foreach (var cell in Grid.Cells)
            {
                if (cell is null)
                {
                    continue;
                }
                
                var tags = cell.GetCellTags();

                foreach (var t in tags)
                {
                    if (tagNumber.ContainsKey(t))
                    {
                        tagNumber[t] += 1;
                    }
                    else
                    {
                        tagNumber.Add(t, 1);
                    }
                }
            }

            if (tagNumber.Count == 0)
            {
                return null;
            }
            
            var min = tagNumber
                .Where(x => x.Key != CellTags.Hero)
                .Min(x => x.Value);
            
            return tagNumber.First(x => x.Value == min).Key;
        }

        private Cell SpawnCellWithComponent(CellComponent cellComponent, Vector3 initialScale)
        {
            var cell = SpawnEmptyCell(initialScale);
            var instance = Instantiate(cellComponent, cell.transform);

            var cellRenderer = cell.GetComponent<SpriteRenderer>();
            var componentRenderer = instance.GetComponent<Renderer>();
            cellRenderer.sortingOrder = componentRenderer.sortingOrder - 1;

            var components = instance.GetComponents<CellComponent>();
            foreach (var component in components)
            {
                cell.AddCellComponent(component);
            }

            return cell;
        }
        
        public Cell SpawnCellWithContent(CellContent cellComponent, Vector3 initialScale)
        {
            var cell = SpawnEmptyCell(initialScale);
            var content = Instantiate(cellComponent, cell.transform);

            var cellRenderer = cell.GetComponent<SpriteRenderer>();
            cellRenderer.sortingOrder = content.GetSortingOrder() - 1;

            var components = content.GetCellComponents();
            foreach (var component in components)
            {
                cell.AddCellComponent(component);
            }

            return cell;
        }
    }
}
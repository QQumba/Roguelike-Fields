using System.Collections.Generic;
using System.Linq;
using Cells;
using Tags;
using UnityEngine;
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

        [SerializeField]
        private CellSpawnerPrefab[] spawnerPrefabs;

        public static CellSpawner Instance;

        private Grid _grid;

        // ReSharper disable once Unity.NoNullCoalescing
        private Grid Grid => _grid ?? Grid.Instance;

        private void Awake()
        {
            Instance = this;
        }

        public Cell SpawnEmptyCell(Vector3 initialScale)
        {
            var cell = Instantiate(cellPrefab);
            cell.transform.localScale = initialScale;

            return cell;
        }

        public Cell SpawnHero()
        {
            return SpawnCellWithContent(heroContent, Vector3.one);
        }

        public Cell SpawnCell(Vector3? initialScale = null)
        {
            var prefabsToSpawn = spawnerPrefabs.Where(x => x.Spawn).ToArray();
            var i = Random.Range(0, prefabsToSpawn.Length);
            var cellContent = prefabsToSpawn[i].Prefab;
            initialScale ??= Vector3.one;

            return SpawnCellWithContent(cellContent, initialScale.Value);
        }

        public Cell SpawnCellWithContent(CellContent cellContent, Vector3 initialScale)
        {
            var cell = SpawnEmptyCell(initialScale);
            var content = Instantiate(cellContent, cell.transform);

            var cellRenderer = cell.GetComponent<SpriteRenderer>();
            cellRenderer.sortingOrder = content.GetSortingOrder() - 1;

            var components = content.GetCellComponents();
            foreach (var component in components)
            {
                cell.AddCellComponent(component);
            }

            if (content.TryGetInteraction(out var interaction))
            {
                cell.AddInteraction(interaction);
            }

            return cell;
        }

        private void ReadSpawnableCellsTags()
        {
            spawnerPrefabs
                .Select(x => x.Prefab)
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cells;
using Tags;
using UnityEngine;
using Grid = GameGrid.Grid;
using Random = UnityEngine.Random;

namespace Game.CellGenerator
{
    /// <summary>
    /// Allow to spawn new cells. 
    /// </summary>
    public class CellSpawner : MonoBehaviour
    {
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private CellContent emptyContent;
        [SerializeField] private CellContent heroContent;
        [SerializeField] private CellContent[] prefabs;
        [SerializeField] private CellSpawnerPrefab[] spawnerPrefabs;

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

        public Cell SpawnEmptyContent()
        {
            return SpawnCellWithContent(emptyContent, Vector3.one);
        }

        public Cell SpawnCell(Vector3? initialScale = null)
        {
            var prefabsToSpawn = spawnerPrefabs.Where(x => x.Spawn).ToArray();
            var i = Random.Range(0, prefabsToSpawn.Length);
            var cellContent = prefabsToSpawn[i].Prefab;
            initialScale ??= Vector3.one;

            return SpawnCellWithContent(cellContent, initialScale.Value);
        }

        public Cell SpawnCell(CellSpawnerState state, Vector3? initialScale = null)
        {
            var criteria = state.GetMaximumAvailableCriteria();
            Debug.Log(
                $"trying to spawn cell with criteria: cellType {criteria.CellType}, powerLevel: {criteria.PowerLevel}");
            var cellContent = GetSatisfyingContent(criteria);
            initialScale ??= Vector3.one;

            return SpawnCellWithContent(cellContent, initialScale.Value);
        }

        private CellContent GetSatisfyingContent(CellSpawnCriteria criteria)
        {
            const int maxIterations = 100;
            var prefabsToSpawn = Array.Empty<CellSpawnerPrefab>();

            var i = 0;
            while (prefabsToSpawn.Length == 0 && i < maxIterations)
            {
                prefabsToSpawn = spawnerPrefabs.Where(x => x.Spawn && x.Prefab.SpawnCriteria.Satisfies(criteria))
                    .ToArray();
                criteria = criteria.Soften();
                i++;
            }

            var dbg = prefabsToSpawn.Select(x => new { x.Prefab.name, x.Prefab.SpawnCriteria });

            var sb = new StringBuilder("Prefabs to spawn:");
            foreach (var x in dbg)
            {
                sb.Append($"\n[{x.name}, {x.SpawnCriteria.CellType}, {x.SpawnCriteria.PowerLevel}]");
            }

            Debug.Log(sb.ToString());

            // no criteria satisfied, spawn random cell
            if (prefabsToSpawn.Length == 0)
            {
                prefabsToSpawn = spawnerPrefabs.Where(x => x.Spawn).ToArray();
            }

            var index = Random.Range(0, prefabsToSpawn.Length);
            return prefabsToSpawn[index].Prefab;
        }

        public Cell SpawnCellWithContent(CellContent contentPrefab, Vector3 initialScale)
        {
            var cell = SpawnEmptyCell(initialScale);
            var content = Instantiate(contentPrefab, cell.transform);
            content.name = contentPrefab.name;

            var cellRenderer = cell.GetComponent<SpriteRenderer>();
            // need to check
            cellRenderer.sortingOrder = content.GetSortingOrder(cellRenderer.sortingOrder) - 1;

            var components = content.GetCellComponents();
            foreach (var component in components)
            {
                cell.AddCellComponent(component);
            }

            cell.name = content.name;

            if (content.TryGetInteraction(out var interaction))
            {
                cell.AddInteraction(interaction);
            }

            cell.SetSpawnCriteria(content.SpawnCriteria);

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
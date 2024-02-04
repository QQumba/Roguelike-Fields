using Cells;
using Cells.Components;
using GameGrid;
using UnityEngine;
using UnityEngine.Serialization;

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
        private Hero heroPrefab;

        [SerializeField]
        private CellContent heroContent;

        [SerializeField]
        private Enemy enemyPrefab;

        [SerializeField]
        private CellContent[] prefabs;

        public static CellSpawner Instance;

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

        public Cell SpawnHero(GridController controller)
        {
            var cell = SpawnEmptyCell();
            var hero = Instantiate(heroContent, cell.transform);

            // I assume that hero have same sorting layer as cell and higher order within that layer then other components
            var cellRenderer = cell.GetComponent<SpriteRenderer>();
            cellRenderer.sortingOrder = hero.GetSortingOrder() - 1;

            foreach (var component in hero.GetCellComponents())
            {
                if (component is Hero heroComponent)
                {
                    heroComponent.GridController = controller;
                }
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

        public Cell SpawnCellWithComponent(CellComponent cellComponent, Vector3 initialScale)
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
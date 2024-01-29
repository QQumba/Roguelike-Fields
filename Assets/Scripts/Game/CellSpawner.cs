using System;
using Cells;
using Cells.Components;
using GameGrid;
using UnityEngine;

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
        private Enemy enemyPrefab;

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
            var hero = Instantiate(heroPrefab, cell.transform);
            hero.GridController = controller;

            // I assume that hero have same sorting layer as cell and higher order within that layer then other components
            var cellRenderer = cell.GetComponent<SpriteRenderer>();
            var heroRenderer = hero.GetComponent<Renderer>();
            cellRenderer.sortingOrder = heroRenderer.sortingOrder - 1;

            cell.AddCellComponent(hero);
            return cell;
        }

        public Cell SpawnCell(Vector3? initialScale = null)
        {
            var cell = SpawnEmptyCell(initialScale);
            var enemy = Instantiate(enemyPrefab, cell.transform);

            var cellRenderer = cell.GetComponent<SpriteRenderer>();
            var componentRenderer = enemy.GetComponent<Renderer>();
            cellRenderer.sortingOrder = componentRenderer.sortingOrder - 1;

            // add all components that are on an enemy game object

            var components = enemy.GetComponents<CellComponent>();
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
    }
}
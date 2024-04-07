using Cells;
using Events;
using Game;
using Game.CellGenerator;
using GameGrid;
using UnityEngine;

namespace Effects
{
    public class ReplaceCellRandomEffect : MonoBehaviour
    {
        [SerializeField]
        private CellContent[] cellPrefabs;

        private GridController _gridController;

        private void Start()
        {
            _gridController = GridController.Instance;
        }

        public void ReplaceCell(CellEventArgs e)
        {
            var randomIndex = Random.Range(0, cellPrefabs.Length);
            var cellPrefab = cellPrefabs[randomIndex];

            _gridController.ReplaceWithContent(e.Cell, cellPrefab);
        }
    }
}
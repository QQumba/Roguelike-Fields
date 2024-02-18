using Cells;
using Events;
using Game;
using GameGrid;
using UnityEngine;

namespace Effects
{
    public class ReplaceCellRandomEffect : MonoBehaviour
    {
        [SerializeField]
        private CellContent[] cellPrefabs;

        private GridController _gridController;
        private CellSpawner _cellSpawner;

        private void Start()
        {
            _gridController = GridController.Instance;
            _cellSpawner = CellSpawner.Instance;
        }

        public void ReplaceCell(CellEventArgs e)
        {
            var randomIndex = Random.Range(0, cellPrefabs.Length);
            var cellPrefab = cellPrefabs[randomIndex];
            
            var newCell = _cellSpawner.SpawnCellWithContent(cellPrefab, Vector3.one);
            newCell.gameObject.SetActive(false);
            _gridController.Replace(e.Cell, newCell);
        }
    }
}
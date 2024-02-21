using Cells;
using Events;
using Game;
using GameGrid;
using UnityEngine;

namespace Effects
{
    public class ReplaceCellEffect : MonoBehaviour
    {
        [SerializeField]
        private CellContent cellPrefab;

        private GridController _gridController;
        private CellSpawner _cellSpawner;

        private void Start()
        {
            _gridController = GridController.Instance;
            _cellSpawner = CellSpawner.Instance;
        }

        public void ReplaceCell(CellEventArgs e)
        {
            var newCell = _cellSpawner.SpawnCellWithContent(cellPrefab, Vector3.zero);
            // newCell.gameObject.SetActive(false);
            _gridController.Replace(e.Cell, newCell);
        }
    }
}
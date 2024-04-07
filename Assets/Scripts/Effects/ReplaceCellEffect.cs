using Cells;
using Events;
using Game;
using Game.CellGenerator;
using GameGrid;
using UnityEngine;

namespace Effects
{
    public class ReplaceCellEffect : MonoBehaviour
    {
        [SerializeField]
        private CellContent cellPrefab;

        private GridController _gridController;

        private void Start()
        {
            _gridController = GridController.Instance;
        }

        public void ReplaceCell(CellEventArgs e)
        {
            _gridController.ReplaceWithContent(e.Cell, cellPrefab);
        }
    }
}
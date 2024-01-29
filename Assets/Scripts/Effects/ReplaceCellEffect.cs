﻿using Cells.Components;
using Events;
using Game;
using GameGrid;
using UnityEngine;

namespace Effects
{
    public class ReplaceCellEffect : MonoBehaviour
    {
        [SerializeField]
        private CellComponent cellPrefab;

        private GridController _gridController;
        private CellSpawner _cellSpawner;

        private void Start()
        {
            _gridController = GridController.Instance;
            _cellSpawner = CellSpawner.Instance;
        }

        public void ReplaceCell(CellEventArgs e)
        {
            var newCell = _cellSpawner.SpawnCellWithComponent(cellPrefab, Vector3.one);
            newCell.gameObject.SetActive(false);
            _gridController.Replace(e.Cell, newCell);
        }
    }
}
using System;
using Cells;
using UnityEngine;

namespace Game.CellGenerator
{
    [Serializable]
    public class CellSpawnerPrefab
    {
        [SerializeField]
        private CellContent prefab;

        [SerializeField]
        private bool spawn;

        [SerializeField]
        private int maxCount = 99;
        
        public CellContent Prefab => prefab;
        
        public bool Spawn => spawn;

        public int MaxCount => maxCount;
    }
}
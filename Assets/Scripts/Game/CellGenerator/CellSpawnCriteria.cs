using System;
using UnityEngine;

namespace Game.CellGenerator
{
    [Serializable]
    public struct CellSpawnCriteria
    {
        [SerializeField] private CellType cellType;
        [SerializeField] private float powerLevel;

        public CellSpawnCriteria(CellType cellType, float powerLevel)
        {
            this.cellType = cellType;
            this.powerLevel = powerLevel;
        }

        public CellType CellType => cellType;

        public float PowerLevel => powerLevel;


        public bool Satisfies(CellSpawnCriteria otherCriteria)
        {
            return CellType == otherCriteria.CellType && PowerLevel < otherCriteria.PowerLevel;
        }

        public CellSpawnCriteria Soften()
        {
            // loosen the criteria or even cycle through different cell types
            return new CellSpawnCriteria(cellType, powerLevel + 1);
        }
    }
}
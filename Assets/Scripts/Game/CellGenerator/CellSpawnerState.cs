using System.Collections.Generic;
using System.Linq;

namespace Game.CellGenerator
{
    public class CellSpawnerState
    {
        /// <summary>
        /// List of cell details in game
        /// </summary>
        public List<CellSpawnCriteria> CellSpawnCriteria { get; set; }

        public float MaxPowerLevel { get; set; }
        
        public float MaxTotalPowerLevel => MaxPowerLevel * 9;

        public CellSpawnCriteria GetMaximumAvailableCriteria()
        {
            if (CellSpawnCriteria.Count == 0)
            {
                return new CellSpawnCriteria(CellType.Buff, 10);
            }
            
            var cellTypeTotalPowerLevel = new Dictionary<CellType, float>
            {
                { CellType.Buff, 0 },
                { CellType.Debuff, 0 }
            };

            foreach (var c in CellSpawnCriteria)
            {
                if (cellTypeTotalPowerLevel.ContainsKey(c.CellType))
                {
                    cellTypeTotalPowerLevel[c.CellType] += c.PowerLevel;
                }
                else
                {
                    cellTypeTotalPowerLevel.Add(c.CellType, c.PowerLevel);
                }
            }
            
            var leastPresentType = cellTypeTotalPowerLevel.OrderBy(x => x.Value).First();
            
            return new CellSpawnCriteria(leastPresentType.Key, MaxPowerLevel);
        }
    }
}
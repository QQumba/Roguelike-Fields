using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cells;
using Cells.Components;
using Game;
using GameGrid;
using UnityEngine;

namespace Effects.Utilities
{
    [CreateAssetMenu(fileName = "EffectTarget", menuName = "ScriptableObjects/EffectTarget", order = 1)]
    public class EffectTarget : ScriptableObject
    {
        [SerializeField]
        private bool affectSelf;

        [SerializeField]
        private EffectTargets targets;

        [SerializeField]
        private EffectTargetLocation targetLocation;

        [SerializeField]
        private int targetCount;

        // replace with grid controller?
        private GridInitializer _gridInitializer;

        private void Awake()
        {
            _gridInitializer = GridInitializer.Instance;
        }

        public IEnumerable<Cell> GetTargets(Cell cell)
        {
            if (affectSelf)
            {
                return new[] { cell };
            }

            var cells = FilterTargets(FilterTargetLocation(cell)); 
            return cells;
        }

        private IEnumerable<Cell> FilterTargetLocation(Cell cell)
        {
            return Enumerable.Empty<Cell>();
        }

        private IEnumerable<Cell> FilterTargets(IEnumerable<Cell> cells)
        {
            switch (targets)
            {
                case EffectTargets.Hero:
                    cells = cells.Where(x => x.HasCellComponent<Hero>());
                    break;
                case EffectTargets.Enemies:
                    cells = Enumerable.Empty<Cell>();
                    break;
                case EffectTargets.PickUps:
                    cells = cells.Where(x => x.HasCellComponent<Pickable>());
                    break;
            }

            return cells;
        }
    }

    internal enum EffectTargetLocation
    {
        Any,
        Adjacent
    }

    [Flags]
    public enum EffectTargets
    {
        Any = 0,
        Hero = 1,
        Enemies = 2,
        PickUps = 4
    }
}
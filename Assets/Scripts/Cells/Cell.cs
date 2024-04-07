using System;
using System.Collections.Generic;
using System.Linq;
using Cells.Components;
using Cells.Components.Interfaces;
using Cells.Interactions;
using Game.CellGenerator;
using GameGrid;
using UnityEngine;

namespace Cells
{
    public class Cell : MonoBehaviour
    {
        private readonly List<CellComponent> _components = new List<CellComponent>();
        private int _age;

        private GridController _controller;

        public Interaction Interaction { get; private set; }
        
        /// <summary>
        /// Indicates whether cell is animated at the moment or not.
        /// Currently used to control hover animation.
        /// </summary>
        public bool IsAnimated { get; set; }
        
        public CellSpawnCriteria SpawnCriteria { get; private set; }

        private void Start()
        {
            _controller = GridController.Instance;
        }

        /// <summary>
        /// Finds a cell component of type T.
        /// </summary>
        /// <typeparam name="T">Cell component type to find.</typeparam>
        /// <returns>Component or null if not found.</returns>
        public T GetCellComponent<T>() where T : ICellComponent
        {
            var component = _components.Where(x => x is T).Cast<T>().FirstOrDefault();
            return component;
        }
        
        public List<T> GetCellComponents<T>() where T : ICellComponent
        {
            var component = _components.Where(x => x is T).Cast<T>();
            return component.ToList();
        }

        public bool HasCellComponent<T>() where T : ICellComponent
        {
            return _components.Any(x => x is T);
        }

        // EXPERIMENTAL!!
        public bool HasCellTag(string cellTag)
        {
            return _components.Any(x => x.CellTag == cellTag);
        }

        public IEnumerable<string> GetCellTags()
        {
            return _components.Select(x => x.CellTag);
        }
    
        public T AddCellComponent<T>() where T : CellComponent
        {
            var component = gameObject.AddComponent<T>();
            return component;
        }

        public void AddCellComponent<T>(T component) where T : CellComponent
        {
            component.Cell = this;
            _components.Add(component);
        }

        public void AddInteraction<T>(T interaction) where T : Interaction
        {
            interaction.Cell = this;
            Interaction = interaction;
        }

        // questionable
        public void OnTurnEnded()
        {
            if (_age > 0 || _controller.TurnCount == 0)
            {
                _components.ForEach(x => x.OnTurnEnded());
            }
            
            _age++;
        }

        public void Highlight(bool yes)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = yes ? Color.cyan : Color.white;
        }

        public void SetSpawnCriteria(CellSpawnCriteria contentSpawnCriteria)
        {
            SpawnCriteria = contentSpawnCriteria;
        }
    }
}
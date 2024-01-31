using System.Collections.Generic;
using System.Linq;
using Cells.Components;
using UnityEngine;

namespace Cells
{
    public class Cell : MonoBehaviour
    {
        private readonly List<CellComponent> _components = new List<CellComponent>();

        /// <summary>
        /// Indicates whether cell is animated at the moment or not.
        /// Currently used to control hover animation.
        /// </summary>
        public bool IsAnimated { get; set; }
        
        /// <summary>
        /// Finds a cell component of type T.
        /// </summary>
        /// <typeparam name="T">Cell component type to find.</typeparam>
        /// <returns>Component or null if not found.</returns>
        public T GetCellComponent<T>() where T : CellComponent
        {
            var component = _components.Where(x => x is T).Cast<T>().FirstOrDefault();
            return component;
        }

        public bool HasCellComponent<T>() where T : CellComponent
        {
            return _components.Any(x => x is T);
        }

        // EXPERIMENTAL!!
        public bool HasCellTag(string cellTag)
        {
            return _components.Any(x => x.DefaultTag == cellTag);
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

        public void Accept(IVisitor visitor)
        {
            var visitableComponent = _components.Single(x => x is IVisitable) as IVisitable;
            visitableComponent!.Accept(visitor);
        }
    }
}
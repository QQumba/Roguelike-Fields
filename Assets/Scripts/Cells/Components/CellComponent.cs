using UnityEngine;

namespace Cells.Components
{
    public abstract class CellComponent : MonoBehaviour
    {
        public Cell Cell { get; set; }

        public abstract string CellTag { get; }

        // TODO just allow to call awake in each component
        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Called on awake to initialize component.
        /// </summary>
        protected virtual void Initialize()
        {
        }
    }
}
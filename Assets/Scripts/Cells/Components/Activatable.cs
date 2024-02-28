using System;
using Events;
using Tags;
using UnityEngine;
using UnityEngine.Events;

namespace Cells.Components
{
    public class Activatable : CellComponent, IVisitable
    {
        [SerializeField] private UnityEvent<CellEventArgs> activatedEvent;

        public event Action<CellEventArgs> Activated;

        public override string CellTag => CellTags.Pickable;

        protected void Awake()
        {
            Activated += activatedEvent.Invoke;
        }

        public void Activate()
        {
            Activated?.Invoke(new CellEventArgs(Cell));
        }

        public void Accept(IVisitor visitor)
        {
            // visitor.Visit(this);
        }
    }
}
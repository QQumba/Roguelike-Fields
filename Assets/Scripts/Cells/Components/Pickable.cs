using System;
using Events;
using Tags;
using UnityEngine;
using UnityEngine.Events;

namespace Cells.Components
{
    public class Pickable : CellComponent, IVisitable
    {
        [SerializeField] private UnityEvent<CellEventArgs> pickedUpEvent;

        public event Action<CellEventArgs> PickedUp;

        public override string CellTag => CellTags.Pickable;

        protected void Awake()
        {
            PickedUp += pickedUpEvent.Invoke;
        }

        public void PickUp()
        {
            PickedUp?.Invoke(new CellEventArgs(Cell));
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
using System;
using Events;
using Tags;
using UnityEngine;
using UnityEngine.Events;

namespace Cells.Components
{
    /// <summary>
    /// Currently the same component as a Health, but us meant to be used not only as health value.
    /// Most likely Health component should be replaced with this.
    /// </summary>
    public class ValueProvider : CellComponent
    {
        [SerializeField]
        private int currentValue = 4;

        [SerializeField] private int maxValue = 4;
        [SerializeField] private int minValue = 0;

        [SerializeField] private UnityEvent<ValueChangedEventArgs> valueChangedEvent;
        [SerializeField] private UnityEvent<ValueChangedEventArgs> valueInitializedEvent;
        [SerializeField] private UnityEvent valueDisposedEvent;

        public event Action<ValueChangedEventArgs> ValueChanged;

        public override string CellTag => CellTags.HasHealth;

        protected void Awake()
        {
            ValueChanged += valueChangedEvent.Invoke;
        }

        private void Start()
        {
            Initialize();
        }

        public int Value
        {
            get => currentValue;
            set
            {
                var previousValue = currentValue;
                currentValue = Mathf.Clamp(value, MinValue, MaxValue);
                var args = new ValueChangedEventArgs(Cell)
                {
                    CurrentValue = currentValue,
                    PreviousValue = previousValue,
                    MaxValue = MaxValue
                };

                ValueChanged?.Invoke(args);
            }
        }

        public int MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                var args = new ValueChangedEventArgs(Cell)
                {
                    CurrentValue = Value,
                    PreviousValue = Value,
                    MaxValue = maxValue
                };

                ValueChanged?.Invoke(args);
            }
        }

        public int MinValue
        {
            get => minValue;
            set => minValue = value;
        }

        public void Initialize()
        {
            var args = new ValueChangedEventArgs(Cell)
            {
                CurrentValue = currentValue,
                PreviousValue = currentValue,
                MaxValue = MaxValue
            };

            valueInitializedEvent?.Invoke(args);
        }

        public void Dispose()
        {
            valueDisposedEvent?.Invoke();
        }
    }
}
using System;
using Events;
using Tags;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Cells.Components
{
    public class Health : CellComponent
    {
        [FormerlySerializedAs("currentCurrentValue")] [SerializeField]
        private int currentValue = 4;

        [SerializeField] private int maxValue = 4;
        [SerializeField] private int minValue = 0;

        [SerializeField] private UnityEvent<HealthChangedEventArgs> healthChangedEvent;
        [SerializeField] private UnityEvent<HealthChangedEventArgs> healthInitializedEvent;

        public event Action<HealthChangedEventArgs> HealthChanged;
        
        public override string CellTag => CellTags.HasHealth;

        protected void Awake()
        {
            HealthChanged += healthChangedEvent.Invoke;
        }

        private void Start()
        {
            healthInitializedEvent?.Invoke(new HealthChangedEventArgs(Cell)
            {
                CurrentValue = currentValue,
                PreviousValue = currentValue,
                MaxValue = MaxValue
            });
        }

        public int Value
        {
            get => currentValue;
            set
            {
                var previousValue = currentValue;
                currentValue = Mathf.Clamp(value, MinValue, MaxValue);
                HealthChanged?.Invoke(new HealthChangedEventArgs(Cell)
                {
                    CurrentValue = currentValue,
                    PreviousValue = previousValue,
                    MaxValue = MaxValue
                });
            }
        }

        public int MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                HealthChanged?.Invoke(new HealthChangedEventArgs(Cell)
                {
                    CurrentValue = Value,
                    PreviousValue = Value,
                    MaxValue = maxValue
                });
            }
        }

        public int MinValue
        {
            get => minValue;
            set => minValue = value;
        }
    }
}
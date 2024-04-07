using System;
using Events;
using Misc;
using Tags;
using UnityEngine;
using UnityEngine.Events;

namespace Cells.Components
{
    /// <summary>
    /// Currently the same component as a Health, but meant to be used not only as health value.
    /// Most likely Health component should be replaced with this.
    /// </summary>
    public class ValueProvider : CellComponent
    {
        [SerializeField] private int currentValue = 4;

        [SerializeField] private int maxValue = -1;
        [SerializeField] private int minValue = -1;

        [SerializeField] private UnityEvent<ValueChangedEventArgs> valueChangedEvent;
        [SerializeField] private UnityEvent<ValueChangedEventArgs> valueInitializedEvent;
        [SerializeField] private UnityEvent valueDisposedEvent;

        [SerializeField] private ValuePresenter presenter;
        
        private event Action<ValueChangedEventArgs> ValueChanged;
        private event Action<ValueChangedEventArgs> ValueInitialized;
        private event Action ValueDisposed;

        public override string CellTag => CellTags.HasHealth;

        protected void Awake()
        {
            if (minValue < 0)
            {
                minValue = 0;
            }

            if (maxValue < 0)
            {
                maxValue = currentValue;
            }

            ValueChanged += args =>
            {
                if (presenter is not null)
                {
                    presenter.UpdateValue(args);
                }

                valueChangedEvent.Invoke(args);
            };
            ValueInitialized += args =>
            {
                if (presenter is not null)
                {
                    presenter.UpdateValue(args);
                }

                valueInitializedEvent.Invoke(args);
            };
            ValueDisposed += () =>
            {
                if (presenter is not null)
                {
                    presenter.HideValue();
                }

                valueDisposedEvent.Invoke();
            };
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

            ValueInitialized?.Invoke(args);
        }

        public void Dispose()
        {
            ValueDisposed?.Invoke();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnData
{
    public class TurnAction
    {
        private readonly List<Func<IEnumerator>> _innerActions = new List<Func<IEnumerator>>();
        private int _coroutinesInProgress;

        public event Action Finished;

        private event Action InnerActionFinished;

        public TurnAction(Func<IEnumerator> action)
        {
            _innerActions.Add(action);
        }

        public TurnAction(Action action)
        {
            _innerActions.Add(() => WrapAction(action));
        }

        public TurnAction()
        {
        }

        public void Start(Func<IEnumerator, Coroutine> coroutineRunner)
        {
            _coroutinesInProgress = _innerActions.Count;
            InnerActionFinished += () =>
            {
                _coroutinesInProgress -= 1;
                if (_coroutinesInProgress == 0)
                {
                    Finished?.Invoke();
                }
            };

            foreach (var action in _innerActions)
            {
                coroutineRunner(Invoke(action()));
            }
        }

        // what if instead of combining with another TurnAction Combine can just add another action to _innerActions list directly
        public TurnAction Combine(TurnAction action)
        {
            _innerActions.AddRange(action._innerActions);
            return this;
        }
        
        public void Add(Func<IEnumerator> action)
        {
            _innerActions.Add(action);
        }
        
        public void Add(Action action)
        {
            _innerActions.Add(() => WrapAction(action));
        }

        private IEnumerator Invoke(IEnumerator innerEnumerator)
        {
            yield return innerEnumerator;
            InnerActionFinished?.Invoke();
        }

        private IEnumerator WrapAction(Action action)
        {
            action();
            yield break;
        }
    }
}
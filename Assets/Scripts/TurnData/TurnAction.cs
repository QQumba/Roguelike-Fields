﻿using System;
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
            if (_innerActions.Count == 0)
            {
                Finished?.Invoke();
                return;
            }
        
            _coroutinesInProgress = _innerActions.Count;
            foreach (var action in _innerActions)
            {
                coroutineRunner(Invoke(action()));
            }
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

            _coroutinesInProgress -= 1;
            if (_coroutinesInProgress == 0)
            {
                Finished?.Invoke();
            }
        }

        private IEnumerator WrapAction(Action action)
        {
            action();
            yield break;
        }
    }
}
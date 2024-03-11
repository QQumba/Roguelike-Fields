using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TurnData
{
    public class TurnAction : ITurnAction
    {
        private readonly List<SubAction> _innerActions = new List<SubAction>();
        private int _coroutinesInProgress;

        public string DebugMessage { get; set; }

        public event Action Finished;

        public TurnAction(Func<IEnumerator> action)
        {
            _innerActions.Add(new SubAction(action));
        }

        public TurnAction(Action action)
        {
            _innerActions.Add(new SubAction(() => WrapAction(action)));
        }

        public TurnAction(List<Func<IEnumerator>> actions)
        {
            _innerActions.AddRange(actions.Select(x => new SubAction(x)));
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
        
            foreach (var subAction in _innerActions)
            {
                _coroutinesInProgress++;
                coroutineRunner(Invoke(subAction));
            }
        }

        public void Add(Func<IEnumerator> action)
        {
            _innerActions.Add(new SubAction(action));
        }

        public void Add(Func<IEnumerator> action, float delay)
        {
            _innerActions.Add(new SubAction(action, delay));
        }

        public void Add(Action action)
        {
            _innerActions.Add(new SubAction(() => WrapAction(action)));
        }

        private IEnumerator Invoke(SubAction subAction)
        {
            yield return new WaitForSeconds(subAction.Delay);
            yield return subAction.Action();

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
        
        private struct SubAction
        {
            public SubAction(Func<IEnumerator> action)
            {
                Action = action;
                Delay = 0;
            }
            
            public SubAction(Func<IEnumerator> action, float delay)
            {
                Action = action;
                Delay = delay;
            }
            
            public Func<IEnumerator> Action { get; }            
            
            public float Delay { get; }
        }
    }
}
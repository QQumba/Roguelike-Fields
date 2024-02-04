using System;
using System.Collections;
using UnityEngine;

namespace TurnData
{
    /// <summary>
    /// DRAFT
    /// Each turn new context is created
    /// </summary>
    public class TurnContext
    {
        // 1. how to end turn
        // 2. do I need to store any data during the turn

        private readonly ActionQueue<TurnAction> _actions;
        private readonly Func<IEnumerator, Coroutine> coroutineRunner;

        public event Action TurnFinished;
        
        public TurnContext(Func<IEnumerator, Coroutine> coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
            _actions = new ActionQueue<TurnAction>();
        }
        
        public void StartTurn()
        {
            ExecuteNext();
        }

        private void ExecuteNext()
        {
            if (_actions.Count == 0)
            {
                TurnFinished?.Invoke();
                return;
            }

            var action = _actions.Dequeue();

            // Debug.Log($"execute: {action.Message}");
            action.Finished += ExecuteNext;
            // coroutineRunner(action.Invoke());
            action.StartCoroutine(coroutineRunner);
        }

        public void Next(Action action, string message)
        {
            Next(WrapEnumerator(WrapAction(action)), message);   
        }
        
        // public void Now(Action action)
        // {
        //     Now(WrapEnumerator(WrapAction(action)));   
        // }
        
        public void Next(Func<IEnumerator> action, string message)
        {
            _actions.Enqueue(new TurnAction(action, message));
        }
        
        // public void Now(Func<IEnumerator> action)
        // {
        //     _actions.EnqueueAtFront(new TurnAction(action));
        // }

        private IEnumerator WrapAction(Action action)
        {
            action();
            // yield return new WaitForSeconds(0.1f);
            yield break;
        }

        private Func<IEnumerator> WrapEnumerator(IEnumerator enumerator)
        {
            return () => enumerator;
        }
    }
}
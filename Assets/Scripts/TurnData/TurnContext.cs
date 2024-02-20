using System;
using System.Collections;
using System.Collections.Generic;
using GameGrid;
using UnityEngine;

namespace TurnData
{
    public class TurnContext
    {
        private readonly Queue<ITurnAction> _actions;
        private readonly Func<IEnumerator, Coroutine> coroutineRunner;

        public event Action TurnFinished;
        
        public TurnContext(Func<IEnumerator, Coroutine> coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
            _actions = new Queue<ITurnAction>();
        }

        public Direction TurnDirection { get; set; }

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

            action.Finished += ExecuteNext;
            action.Start(coroutineRunner);
        }

        public void Next(Action action)
        {
            _actions.Enqueue(new TurnAction(action));
        }
        
        public void Next(Action action, string _)
        {
            _actions.Enqueue(new TurnAction(action));
        }
        
        public void Next(Func<IEnumerator> action)
        {
            _actions.Enqueue(new TurnAction(action));
        }
        
        public void Next(Func<IEnumerator> action, string _)
        {
            _actions.Enqueue(new TurnAction(action));
        }

        public void Next(TurnAction action)
        {
            _actions.Enqueue(action);
        }
    }
}
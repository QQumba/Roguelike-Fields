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
        
        private readonly Func<IEnumerator, Coroutine> _coroutineRunner;
        private readonly Action _onTurnEnded;

        private bool _onTurnEndedTriggered;
        
        public event Action TurnFinished;
        
        public TurnContext(Func<IEnumerator, Coroutine> coroutineRunner, Action onTurnEnded)
        {
            _coroutineRunner = coroutineRunner;
            _onTurnEnded = onTurnEnded;
            
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
                if (_onTurnEndedTriggered)
                {
                    TurnFinished?.Invoke();
                    return;
                }

                Next(_onTurnEnded);
                _onTurnEndedTriggered = true;
            }

            var action = _actions.Dequeue();

            action.Finished += ExecuteNext;

            if (action.Message is not null)
            {
                var time = DateTime.Now;
                Debug.Log($"{time:O}: {action.Message}");
            }
            
            action.Start(_coroutineRunner);
        }

        public void Next(Action action)
        {
            var a = new TurnAction(action);
            Next(a);
        }
        
        public void Next(Action action, string message)
        {
            var a = new TurnAction(action) { Message = message };
            Next(a);
        }
        
        public void Next(Func<IEnumerator> action)
        {
            var a = new TurnAction(action);
            Next(a);
        }
        
        public void Next(Func<IEnumerator> action, string message)
        {
            var a = new TurnAction(action) { Message = message };
            Next(a);
        }

        public void Next(TurnAction action)
        {
            action.CoroutineRunner = _coroutineRunner;
            _actions.Enqueue(action);
        }

        public void Log(string message)
        {
            _actions.Enqueue(new TurnAction(() => { }) { Message = message });
        }
    }
}
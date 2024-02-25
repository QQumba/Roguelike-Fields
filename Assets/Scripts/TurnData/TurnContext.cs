using System;
using System.Collections;
using System.Collections.Generic;
using GameGrid;
using TurnData.FragmentedTurn;
using UnityEngine;

namespace TurnData
{
    public class TurnContext : ITurnContext
    {
        private readonly Queue<ITurnAction> _actions;
        
        private readonly Func<IEnumerator, Coroutine> startCoroutine;
        private readonly Action _onTurnEnded;

        private bool _onTurnEndedTriggered;
        
        public event Action TurnFinished;
        
        public TurnContext(Func<IEnumerator, Coroutine> startCoroutine, Action onTurnEnded)
        {
            this.startCoroutine = startCoroutine;
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

                this.Next(_onTurnEnded);
                _onTurnEndedTriggered = true;
            }

            var action = _actions.Dequeue();

            action.Finished += ExecuteNext;

            if (action.Message is not null)
            {
                var time = DateTime.Now;
                Debug.Log($"{time:O}: {action.Message}");
            }
            
            action.Start(startCoroutine);
        }

        public void Next(TurnAction action)
        {
            _actions.Enqueue(action);
        }

        public void Log(string message)
        {
            _actions.Enqueue(new TurnAction(() => { }) { Message = message });
        }
    }
}
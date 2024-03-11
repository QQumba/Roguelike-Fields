using System;
using System.Collections;
using System.Collections.Generic;
using GameGrid;
using TurnData.FragmentedTurn;
using UnityEngine;
using Object = UnityEngine.Object;

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

        public void Destroy(Object obj, int secondsDelay = 3)
        {
            throw new NotImplementedException();
        }

        public ITurnContext Next(TurnAction action)
        {
            _actions.Enqueue(action);
            return this;
        }

        public void Log(string message)
        {
            _actions.Enqueue(new TurnAction(() => { }) { DebugMessage = message });
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

            if (action.DebugMessage is not null)
            {
                var time = DateTime.Now;
                Debug.Log($"{time:O}: {action.DebugMessage}");
            }
            
            action.Start(startCoroutine);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnData.FragmentedTurn
{
    public class TurnFragment
    {
        private readonly Queue<TurnAction> _actions;
        
        private readonly Func<IEnumerator, Coroutine> _startCoroutine;

        public event Action Completed;

        public TurnFragment(Func<IEnumerator, Coroutine> startCoroutine, TurnFragmentType fragmentType)
        {
            _actions = new Queue<TurnAction>();
            _startCoroutine = startCoroutine;
            
            FragmentType = fragmentType;
        }
        
        public TurnFragmentType FragmentType { get; }

        public void Start()
        {
            StartNext();                    
        }

        public void Next(TurnAction action)
        {
            _actions.Enqueue(action);
        }
        
        private void StartNext()
        {
            if (_actions.Count == 0)
            {
                Completed?.Invoke();
                return;
            }

            var action = _actions.Dequeue();
            action.Finished += StartNext;
            
            action.Start(_startCoroutine);
        }
    }
}
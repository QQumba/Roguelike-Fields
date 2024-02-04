using System;
using System.Collections;
using UnityEngine;

namespace TurnData
{
    public class TurnAction
    {
        private readonly Func<IEnumerator> _innerAction;

        public string Message { get; }

        public event Action Finished;
        
        public TurnAction(Func<IEnumerator> innerAction, string message)
        {   
            Debug.Log($"queue: {message}");
            _innerAction = innerAction;
            Message = message;
        }
        
        public IEnumerator Invoke()
        {
            yield return _innerAction();
            Finished?.Invoke();
        }

        public void StartCoroutine(Func<IEnumerator, Coroutine> coroutineRunner)
        {
            coroutineRunner(Invoke());
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace TurnData
{
    public interface ITurnAction
    {
        string Message { get; }
        event Action Finished;
        void Start(Func<IEnumerator, Coroutine> coroutineRunner);
    }
}
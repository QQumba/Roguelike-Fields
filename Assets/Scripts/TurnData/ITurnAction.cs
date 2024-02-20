using System;
using System.Collections;
using UnityEngine;

namespace TurnData
{
    public interface ITurnAction
    {
        event Action Finished;
        void Start(Func<IEnumerator, Coroutine> coroutineRunner);
    }
}
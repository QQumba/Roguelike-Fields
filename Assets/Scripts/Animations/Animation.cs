using System;
using System.Collections;
using UnityEngine;

namespace Animations
{
    public abstract class Animation : MonoBehaviour
    {
        public event Action AnimationFinished;

        public abstract AnimationType Type { get; }

        public Coroutine Play(AnimationArgs args, Action callback)
        {
            var coroutine = StartCoroutine(BeginPlay(args, callback));
            return coroutine;
        }

        private IEnumerator BeginPlay(AnimationArgs args, Action callback)
        {
            yield return OnPlay(args);
            callback?.Invoke();
            AnimationFinished?.Invoke();
        }

        protected abstract IEnumerator OnPlay(AnimationArgs args);
    }
}
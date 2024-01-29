using System;
using System.Collections;
using UnityEngine;

namespace Animations
{
    public class EmptyAnimation : CoroutineAnimation
    {
        protected override IEnumerator OnPlay()
        {
            yield break;
        }
    }

    public class MoveAnimation1 : CoroutineAnimation
    {
        private readonly Transform _target;
        private readonly Vector3 _initialPosition;
        private readonly Vector3 _targetPosition;
        private readonly float _speed;

        public MoveAnimation1(
            Transform target,
            Vector3 targetPosition,
            Vector3? initialPosition = null,
            float speed = 4)
        {
            _initialPosition = initialPosition ?? target.position;
            _targetPosition = targetPosition;
            _target = target;
            _speed = speed;
        }

        protected override IEnumerator OnPlay()
        {
            for (float i = 0; i < 1; i += Time.deltaTime * _speed)
            {
                _target.position = Vector3.Lerp(_initialPosition, _targetPosition, i);
                yield return null;
            }

            _target.position = _targetPosition;
        }
    }

    public class ScaleAnimation1 : CoroutineAnimation
    {
        private readonly Transform _target;
        private readonly Vector3 _initialScale;
        private readonly Vector3 _targetScale;
        private readonly float _speed;

        public ScaleAnimation1(
            Transform target,
            Vector3 targetScale,
            Vector3? initialScale = null,
            float speed = 4)
        {
            _initialScale = initialScale ?? target.localScale;
            _targetScale = targetScale;
            _target = target;
            _speed = speed;
        }

        protected override IEnumerator OnPlay()
        {
            for (float i = 0; i < 1; i += Time.deltaTime * _speed)
            {
                _target.localScale = Vector3.Lerp(_initialScale, _targetScale, i);
                yield return null;
            }

            _target.localScale = _targetScale;
        }
    }

    public abstract class CoroutineAnimation
    {
        public event Action AnimationFinished;

        public IEnumerator Play()
        {
            yield return OnPlay();
            AnimationFinished?.Invoke();
        }
        
        protected abstract IEnumerator OnPlay();
    }
}
using System.Collections;
using System.Threading.Tasks;
using Cells;
using DefaultNamespace;
using UnityEngine;

namespace Animations.AsyncAnimations
{
    public class ScaleWithCurve : IAsyncAnimation
    {
        private readonly Transform _target;
        private readonly Vector3 _initialScale;
        private readonly AnimationCurve _curve;
        private readonly Vector3 _targetScale;
        private readonly float _speed;

        private bool _stopRequested;

        public ScaleWithCurve(
            AnimationCurve curve,
            Transform target,
            Vector3 targetScale,
            Vector3? initialScale = null,
            float speed = 4)
        {
            _initialScale = initialScale ?? target.localScale;
            _curve = curve;
            _targetScale = targetScale;
            _target = target;
            _speed = speed;
        }

        public IEnumerator Play()
        {
            for (float i = 0; i < 1; i += Time.deltaTime * _speed)
            {
                if (_stopRequested)
                {
                    yield break;
                }
                
                _target.localScale = VectorHelpers.LerpUnclamped(_initialScale, _targetScale, _curve.Evaluate(i));
                yield return null;
            }

            _target.localScale = _targetScale;
        }
        
        public IEnumerator Play(Cell cell)
        {
            cell.IsAnimated = true;
            yield return Play();
            cell.IsAnimated = false;
        }

        public void RequestStop()
        {
            _stopRequested = true;
        }
    }
}
using System.Collections;
using System.Threading.Tasks;
using Cells;
using UnityEngine;

namespace Animations.AsyncAnimations
{
    public class ScaleQuadratic : IAsyncAnimation
    {
        private readonly Transform _target;
        private readonly Vector3 _initialScale;
        private readonly Vector3 _targetScale;
        private readonly float _speed;

        private float _projectileAcceleration = 10f;
        private float _initialSpeed = 5f;
        
        private bool _stopRequested;

        public ScaleQuadratic(
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
        
        public IEnumerator Play()
        {
            var initialScale = _target.localScale;
            var currentSpeed = _initialSpeed;
            
            for (float i = 0; i < 1; i += Time.deltaTime * currentSpeed)
            {
                var t = i;

                // Update the speed with acceleration
                currentSpeed += _projectileAcceleration * Time.deltaTime;

                // Move the object based on the direction, speed, and time
                _target.localScale = Vector3.Lerp(initialScale, _targetScale, t);

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
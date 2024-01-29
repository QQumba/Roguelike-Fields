using System.Threading.Tasks;
using UnityEngine;

namespace Animations.AsyncAnimations
{
    public class ScaleAsync : IAsyncAnimation
    {
        private readonly Transform _target;
        private readonly Vector3 _initialScale;
        private readonly Vector3 _targetScale;
        private readonly float _speed;

        private bool _stopRequested;

        public ScaleAsync(
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

        public async Task Play()
        {
            for (float i = 0; i < 1; i += Time.deltaTime * _speed)
            {
                if (_stopRequested)
                {
                    return;
                }
                
                _target.localScale = Vector3.Lerp(_initialScale, _targetScale, i);
                await Task.Yield();
            }

            _target.localScale = _targetScale;
        }

        public void RequestStop()
        {
            _stopRequested = true;
        }
    }
}
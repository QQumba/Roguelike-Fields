using System.Collections;
using System.Threading.Tasks;
using Cells;
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

        public async Task PlayAsync()
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

        public IEnumerator Play()
        {
            for (float i = 0; i < 1; i += Time.deltaTime * _speed)
            {
                if (_stopRequested)
                {
                    yield break;
                }
                
                _target.localScale = Vector3.Lerp(_initialScale, _targetScale, i);
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
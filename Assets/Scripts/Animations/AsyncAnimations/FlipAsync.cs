using System.Collections;
using System.Threading.Tasks;
using Cells;
using UnityEngine;

namespace Animations.AsyncAnimations
{
    public class FlipAsync : IAsyncAnimation
    {
        private readonly Transform _target;
        private readonly float _initialAngle;
        private readonly float _targetAngle;
        private readonly float _speed;

        public FlipAsync(
            Transform target,
            float targetAngle,
            float? initialAngle = null,
            float speed = 4)
        {
            _initialAngle = initialAngle ?? target.eulerAngles.y;
            _targetAngle = targetAngle;
            _target = target;
            _speed = speed;
        }

        public async Task PlayAsync()
        {
            for (float i = 0; i < 1; i += Time.deltaTime * _speed)
            {
                _target.eulerAngles = new Vector3(0, Mathf.Lerp(_initialAngle, _targetAngle, i), 0);
                await Task.Yield();
            }

            _target.eulerAngles = new Vector3(0, _targetAngle, 0);
        }

        public IEnumerator Play()
        {
            for (float i = 0; i < 1; i += Time.deltaTime * _speed)
            {
                _target.eulerAngles = new Vector3(0, Mathf.Lerp(_initialAngle, _targetAngle, i), 0);
                yield return null;
            }

            _target.eulerAngles = new Vector3(0, _targetAngle, 0);
        }
        
        public IEnumerator Play(Cell cell)
        {
            cell.IsAnimated = true;
            yield return Play();
            cell.IsAnimated = false;
        }

        public void RequestStop()
        {
            throw new System.NotImplementedException();
        }
    }
}
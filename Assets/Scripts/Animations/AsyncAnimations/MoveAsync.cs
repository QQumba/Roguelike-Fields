using System.Threading.Tasks;
using UnityEngine;

namespace Animations.AsyncAnimations
{
    public class MoveAsync : IAsyncAnimation
    {
        private readonly Transform _target;
        private readonly Vector3 _initialPosition;
        private readonly Vector3 _targetPosition;
        private readonly float _speed;

        public MoveAsync(
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

        public async Task Play()
        {
            for (float i = 0; i < 1; i += Time.deltaTime * _speed)
            {
                _target.position = Vector3.Lerp(_initialPosition, _targetPosition, i);
                await Task.Yield();
            }

            _target.position = _targetPosition;
        }

        public void RequestStop()
        {
            throw new System.NotImplementedException();
        }
    }
}
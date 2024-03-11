using Animations.AsyncAnimations;
using UnityEngine;

namespace Animations.Factories
{
    public class AnimationFactory : MonoBehaviour
    {
        [SerializeField]
        private float baseSpeed = 4;

        public static AnimationFactory Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        // Move

        public IAsyncAnimation Move(Transform target, Vector3 targetPosition, float speedMultiplier = 1)
        {
            return new MoveAsync(target, targetPosition, speed: baseSpeed * speedMultiplier);
        }

        public IAsyncAnimation Move(Transform target, Vector3 targetPosition, Vector3? initialPosition, float speedMultiplier)
        {
            return new MoveAsync(target, targetPosition, initialPosition, baseSpeed * speedMultiplier);
        }

        // Scale

        public IAsyncAnimation Scale(Transform target, Vector3 targetScale, float speedMultiplier = 1)
        {
            return new ScaleAsync(target, targetScale, speed: baseSpeed * speedMultiplier);
        }

        public IAsyncAnimation Scale(Transform target, Vector3 targetScale, Vector3? initialScale, float speedMultiplier)
        {
            return new ScaleAsync(target, targetScale, initialScale, baseSpeed * speedMultiplier);
        }

        // Rotate / flip

        public IAsyncAnimation Rotate(Transform target, float targetAngle, float speedMultiplier = 1)
        {
            return new FlipAsync(target, targetAngle, speed: baseSpeed * speedMultiplier);
        }

        public IAsyncAnimation Rotate(Transform target, float initialAngle, float targetAngle, float speedMultiplier)
        {
            return new FlipAsync(target, targetAngle, initialAngle, baseSpeed * speedMultiplier);
        }
    }
}
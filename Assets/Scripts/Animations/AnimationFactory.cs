using UnityEngine;

namespace Animations
{
    public class AnimationFactory : MonoBehaviour
    {
        public static AnimationFactory Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        public EmptyAnimation None { get; } = new EmptyAnimation();
        
        public MoveAnimation1 Move(Transform target, Vector3 targetPosition, Vector3? initialPosition = null, float speed = 4)
        {
            return new MoveAnimation1(target, targetPosition, initialPosition, speed);
        }
        
        public ScaleAnimation1 Scale(Transform target, Vector3 targetScale, Vector3? initialScale = null, float speed = 4)
        {
            return new ScaleAnimation1(target, targetScale, initialScale, speed);
        }
    }
}
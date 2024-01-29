using System.Collections;
using UnityEngine;

namespace Animations
{
    public class MoveAnimation : Animation
    {
        public override AnimationType Type => AnimationType.Move;

        protected override IEnumerator OnPlay(AnimationArgs args)
        {
            return Play(args.Transform, args.InitialPosition, args.TargetPosition, args.Speed);
        }

        private static IEnumerator Play(Transform target, Vector3 initialPosition, Vector3 targetPosition, float speed)
        {
            for (float i = 0; i < 1; i += Time.deltaTime * speed)
            {
                target.position = Vector3.Lerp(initialPosition, targetPosition, i);
                yield return null;
            }

            target.position = targetPosition;
        }
    }
}
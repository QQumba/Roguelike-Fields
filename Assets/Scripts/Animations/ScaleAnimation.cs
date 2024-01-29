using System.Collections;
using UnityEngine;

namespace Animations
{
    public class ScaleAnimation : Animation
    {
        public override AnimationType Type => AnimationType.Scale;

        protected override IEnumerator OnPlay(AnimationArgs args)
        {
            return Play(args.Transform, args.InitialScale, args.TargetScale, args.Speed);
        }

        public static IEnumerator Play(Transform target, Vector3 initialScale, Vector3 targetScale, float speed)
        {
            for (float i = 0; i < 1; i += Time.deltaTime * speed)
            {
                target.localScale = Vector3.Lerp(initialScale, targetScale, i);
                yield return null;
            }

            target.localScale = targetScale;
        }
    }
}
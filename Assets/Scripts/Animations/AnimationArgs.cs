using System;
using UnityEngine;

namespace Animations
{
    [Serializable]
    public class AnimationArgs
    {
        private AnimationArgs(Transform transform,
            AnimationType type,
            Vector3 targetPosition,
            Vector3 targetScale,
            Vector3? initialPosition = null,
            Vector3? initialScale = null,
            float speed = 1)
        {
            Transform = transform;
            Type = type;

            TargetPosition = targetPosition;
            TargetScale = targetScale;

            InitialPosition = initialPosition ?? transform.position;
            InitialScale = initialScale ?? transform.localScale;

            Speed = speed;
        }

        public Transform Transform { get; }
        
        public AnimationType Type { get; }

        public Vector3 InitialPosition { get; }

        public Vector3 TargetPosition { get; }

        public Vector3 InitialScale { get; }

        public Vector3 TargetScale { get; }

        public float Speed { get; }

        public Action Callback { get; set; }

        public static AnimationArgs ForMove(
            Transform transform,
            Vector3 targetPosition,
            Vector3? initialPosition = null,
            float speed = 4)
        {
            return new AnimationArgs(
                transform,
                AnimationType.Move,
                targetPosition,
                Vector3.one,
                initialPosition,
                Vector3.one,
                speed);
        }

        public static AnimationArgs ForScale(
            Transform transform,
            Vector3 targetScale,
            Vector3? initialScale = null,
            float speed = 4)
        {
            var position = transform.position;
            return new AnimationArgs(
                transform,
                AnimationType.Scale,
                position,
                targetScale,
                position,
                initialScale,
                speed);
        }
    }
}
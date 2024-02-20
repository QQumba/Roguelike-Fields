using System.Collections;
using UnityEngine;

namespace Animations.AsyncAnimations
{
    public class MoveAnimation : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 initialPosition;
        [SerializeField] private Vector3 targetPosition;

        public IEnumerator Play()
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
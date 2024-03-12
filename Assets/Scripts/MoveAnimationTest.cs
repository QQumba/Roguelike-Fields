using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class MoveAnimationTest : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve curve;

        [SerializeField]
        private float speed;

        [SerializeField]
        private Vector3 initialPosition;
        
        [SerializeField]
        private Vector3 targetPosition;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(Animate());
            }
        }

        private IEnumerator Animate()
        {
            var target = transform;
            for (float i = 0; i < 1; i += Time.deltaTime * speed)
            {
                target.position = VectorHelpers.LerpUnclamped(initialPosition, targetPosition, curve.Evaluate(i));
                yield return null;
            }

            target.position = targetPosition;
        }
    }
}
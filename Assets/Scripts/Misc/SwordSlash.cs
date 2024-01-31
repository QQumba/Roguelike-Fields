using System;
using Animations.AsyncAnimations;
using Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Misc
{
    public class SwordSlash : MonoBehaviour
    {
        [SerializeField]
        private Transform slash;

        [SerializeField]
        private float speed = 4;

        private AsyncAnimator _animator;
        private TrailRenderer trail;

        private void Start()
        {
            trail = slash.GetComponent<TrailRenderer>();
            _animator = AsyncAnimator.Instance;
        }
        
        // looks boring, try to replace linear move with something else
        public async void Slash(CellEventArgs args)
        {
            var position = args.Cell.transform.position;

            var (a, b) = GetOppositePointsOnCircle(position, 0.3f);

            trail.Clear();
            slash.gameObject.SetActive(false);
            slash.position = a;
            slash.gameObject.SetActive(true);

            await _animator.Play(new MoveAsync(slash, b, a, speed));
        }
        
        private static (Vector2 a, Vector2 b) GetOppositePointsOnCircle(Vector2 position, float radius)
        {
            var a = Random.insideUnitCircle.normalized * radius;
            var b = -a;

            a += position;
            b += position;

            return (a, b);
        }
    }
}
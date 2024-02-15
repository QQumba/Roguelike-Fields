using Animations.AsyncAnimations;
using Events;
using GameGrid;
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

        private TrailRenderer _trail;
        private GridController _gridController;
        
        private void Start()
        {
            _trail = slash.GetComponent<TrailRenderer>();
            _gridController = GridController.Instance;
            
            // ensure that slash trail not active
            slash.gameObject.SetActive(false);
        }
        
        public void Slash(CellEventArgs args)
        {
            var position = args.Cell.transform.position;

            var (a, b) = GetOppositePointsOnCircle(position, 0.3f);

            slash.position = a;
            slash.gameObject.SetActive(true);

            _gridController.CurrentTurn.Next(() => new MoveAsync(slash, b, a, speed).Play());

            _gridController.CurrentTurn.Next(() =>
            {
                _trail.Clear();
                slash.gameObject.SetActive(false);
            });
        }
        
        private static (Vector2 a, Vector2 b) GetOppositePointsOnCircle(Vector2 circleCenter, float radius)
        {
            var a = Random.insideUnitCircle.normalized * radius;
            var b = -a;

            a += circleCenter;
            b += circleCenter;

            return (a, b);
        }
    }
}
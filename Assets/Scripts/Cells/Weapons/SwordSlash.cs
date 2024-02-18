using Animations.AsyncAnimations;
using Events;
using GameGrid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cells.Weapons
{
    public class SwordSlash : MonoBehaviour
    {
        [SerializeField]
        private float speed = 4;

        private TrailRenderer _trail;
        private GridController _gridController;
        
        private void Start()
        {
            _trail = GetComponent<TrailRenderer>();
            _gridController = GridController.Instance;
        }
        
        public void Slash(CellEventArgs args)
        {
            var position = args.Cell.transform.position;

            var (a, b) = GetOppositePointsOnCircle(position, 0.3f);

            transform.position = a;

            _gridController.CurrentTurn.Next(() => new MoveAsync(transform, b, a, speed).Play());

            _gridController.CurrentTurn.Next(() =>
            {
                _trail.Clear();
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
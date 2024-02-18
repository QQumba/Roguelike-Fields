using Animations.AsyncAnimations;
using Cells.Components;
using GameGrid;
using TurnData;
using UnityEngine;

namespace Cells.Weapons
{
    public class Sword : Weapon
    {
        [SerializeField]
        private Transform slash;

        [SerializeField]
        private float speed = 4;

        [SerializeField]
        private int damage = 7;

        private TrailRenderer _trail;
        private GridController _gridController;

        private TurnContext CurrentTurn => _gridController.CurrentTurn;
        
        private void Start()
        {
            // ensure that slash object world position is independent
            slash.SetParent(null);
            
            _trail = slash.GetComponent<TrailRenderer>();
            _gridController = GridController.Instance;
            
            // ensure that slash trail not active
            slash.gameObject.SetActive(false);

            Damage = damage;
        }
        
        public override void Attack(Enemy enemy)
        {
            var position = enemy.Cell.transform.position;

            var (a, b) = GetOppositePointsOnCircle(position, 0.3f);

            slash.position = a;
            slash.gameObject.SetActive(true);

            CurrentTurn.Next(() => new MoveAsync(slash, b, a, speed).Play());

            CurrentTurn.Next(() =>
            {
                _trail.Clear();
                slash.gameObject.SetActive(false);
            });
            
            CurrentTurn.Next(() => enemy.Damageable.DealDamage(Damage));
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
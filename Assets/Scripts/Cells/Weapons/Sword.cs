using System;
using Animations.AsyncAnimations;
using Cells.Components;
using GameGrid;
using TurnData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cells.Weapons
{
    public class Sword : Weapon
    {
        [SerializeField]
        private Transform slashPrefab;

        [SerializeField]
        private float speed = 4;

        [SerializeField]
        private int damage = 7;

        private GridController _gridController;

        private TurnContext CurrentTurn => _gridController.CurrentTurn;

        public override event Action WeaponBroken;

        private void Start()
        {
            _gridController = GridController.Instance;
        }

        public override void Attack(Enemy enemy)
        {
            var position = enemy.Cell.transform.position;
            var (a, b) = GetOppositePointsOnCircle(position, 0.3f);

            var slash = Instantiate(slashPrefab, a, Quaternion.identity);

            CurrentTurn.Next(() => new MoveAsync(slash, b, a, speed).Play());
            CurrentTurn.Next(() => Destroy(slash.gameObject));
            CurrentTurn.Next(() =>
            {
                var damageDealt = enemy.Damageable.DealDamage(Damage.Value);
                Damage.Value -= damageDealt;
                if (Damage.Value == 0)
                {
                    WeaponBroken?.Invoke();
                }
            });
        }

        public override void BindDamageValueProvider(ValueProvider valueProvider)
        {
            valueProvider.Value = damage;
            Damage = valueProvider;
        }

        public override bool TryReinforce(Weapon weapon)
        {
            if (weapon is Sword sword)
            {
                Damage.Value += sword.damage / 2;
                return true;
            }

            return false;
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
using System;
using System.Collections;
using Animations;
using Cells.Components;
using GameGrid;
using TurnData;
using TurnData.FragmentedTurn;
using UnityEngine;
using Grid = GameGrid.Grid;
using Object = UnityEngine.Object;

namespace Cells.Weapons
{
    public class FireStaff : Weapon
    {
        [SerializeField]
        private Transform projectilePrefab;

        [SerializeField]
        private ParticleSystem fireBlastPrefab;
        
        [SerializeField]
        private float projectileSpeed = 4;

        [SerializeField]
        private int damage = 8;

        [SerializeField]
        private AnimationCurve projectileCurve;

        [SerializeField]
        private float projectileAcceleration = 10f;
        
        [SerializeField]
        private float initialSpeed = 5f;
        
        private GridController _gridController;

        private ITurnContext CurrentTurn => _gridController.CurrentTurn;

        public override event Action WeaponBroken;

        private void Start()
        {
            _gridController = GridController.Instance;
        }

        public override void Attack(Enemy enemy)
        {
            var heroPosition = Grid.Instance.Hero.transform.position;
            var enemyPosition = enemy.Cell.transform.position;

            FireProjectile(heroPosition, enemyPosition);
            CurrentTurn.Next(() =>
            {
                var damageDealt = enemy.Damageable.DealDamage(Damage.Value);
                
                AttackNextCellInLine(enemy);
                
                Damage.Value -= damageDealt;
                if (Damage.Value == 0)
                {
                    WeaponBroken?.Invoke();
                }
            });
        }

        private void AttackNextCellInLine(Enemy originalEnemy)
        {
            var grid = Grid.Instance;
            
            var direction = CurrentTurn.TurnDirection.ToIndex();
            var originalEnemyIndex = grid.IndexOf(originalEnemy.Cell);
            var originalEnemyPosition = grid.GetCellPosition(originalEnemyIndex);
            var nextCell = grid.GetCell(originalEnemyIndex + direction);
            
            if (nextCell is null)
            {
                return;
            }

            var enemyComponent = nextCell.GetCellComponent<Enemy>();

            if (enemyComponent is null)
            {
                return;
            }

            var enemyPosition = grid.GetCellPosition(enemyComponent.Cell);
            FireProjectile(originalEnemyPosition, enemyPosition);

            var damageValue = Damage.Value;
            CurrentTurn.Next(() => enemyComponent.Damageable.DealDamage(damageValue));
        }

        private void FireProjectile(Vector3 from, Vector3 to)
        {
            var projectile = Instantiate(projectilePrefab, from, Quaternion.identity);
            var fireBlast = Instantiate(fireBlastPrefab, to, Quaternion.identity);
            
            // TODO turn context can be responsible for this kind of cleanup
            StartCoroutine(DestroyProjectile(projectile.gameObject));
            StartCoroutine(DestroyProjectile(fireBlast.gameObject));
            
            CurrentTurn.Next(() => Move(projectile, to));
            CurrentTurn.Next(() =>
            {
                fireBlast.Play();
                return Coroutines.Wait(fireBlast.main.duration);
            });
        }
        
        // TODO move up to base class
        public override void BindDamageValueProvider(ValueProvider valueProvider)
        {
            valueProvider.Value = damage;
            Damage = valueProvider;
        }

        // TODO move up to base class
        public override bool TryReinforce(Weapon weapon)
        {
            if (weapon is FireStaff sword)
            {
                Damage.Value += sword.damage / 2;
                return true;
            }

            return false;
        }

        public override void BreakWeapon()
        {
            WeaponBroken?.Invoke();
        }

        private IEnumerator DestroyProjectile(Object objectToDestroy)
        {
            yield return new WaitForSeconds(3);
            Destroy(objectToDestroy);
        }
        
        private IEnumerator Move(Transform projectile, Vector3 targetPosition)
        {
            var initialPosition = projectile.position;
            var currentSpeed = initialSpeed;
            
            for (float i = 0; i < 1; i += Time.deltaTime * currentSpeed)
            {
                var t = i;

                // Update the speed with acceleration
                currentSpeed += projectileAcceleration * Time.deltaTime;

                // Move the object based on the direction, speed, and time
                projectile.position = Vector3.Lerp(initialPosition, targetPosition, t);

                yield return null;
            }

            projectile.position = targetPosition;
        }
    }
}
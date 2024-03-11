using System.Collections;
using System.Linq;
using Cells.Components.Interfaces;
using GameGrid;
using TurnData;
using TurnData.FragmentedTurn;
using UnityEngine;
using Grid = GameGrid.Grid;

namespace Cells.Components
{
    public class GoldGolem : CellComponent
    {
        [SerializeField]
        public Transform goldStealingParticlesPrefab;
        
        public override string CellTag => "gold-golem";

        public override void OnTurnEnded()
        {
            base.OnTurnEnded();
            
            StealGold();
        }

        private void StealGold()
        {
            var grid = Grid.Instance;
            var coin = grid.GetAdjacentCells(Cell)
                .Where(x => x.HasCellComponent<GoldCoin>())
                .Select(x => x.GetCellComponent<GoldCoin>())
                .FirstOrDefault();

            if (coin is null)
            {
                return;
            }

            var controller = GridController.Instance;
            
            var health = Cell.GetCellComponent<IDamageable>().Health;

            // currently constant value
            // this can be changed when I add value to coins
            controller.CurrentTurn.Next(() => StealGold(coin.Cell.transform.position, Cell.transform.position));
            controller.CurrentTurn.Next(() => controller.ReplaceWithEmpty(coin.Cell));
            controller.CurrentTurn.Next(() =>
            {
                health.MaxValue += 4;
                health.Value += 4;
            });
        }
        
        private void StealGold(Vector3 from, Vector3 to)
        {
            var projectile = Instantiate(goldStealingParticlesPrefab, from, Quaternion.identity);
            
            StartCoroutine(DestroyProjectile(projectile.gameObject));
            
            GridController.Instance.CurrentTurn.Next(() => Move(projectile, to));
        }
        
        private IEnumerator DestroyProjectile(Object objectToDestroy)
        {
            yield return new WaitForSeconds(3);
            Destroy(objectToDestroy);
        }
        
        
        private readonly float initialSpeed = 5f;
        private readonly float projectileAcceleration = 10f;
        
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
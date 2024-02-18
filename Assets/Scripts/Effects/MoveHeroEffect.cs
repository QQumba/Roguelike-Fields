using Events;
using GameGrid;
using UnityEngine;
using Grid = GameGrid.Grid;

namespace Effects
{
    public class MoveHeroEffect : MonoBehaviour
    {
        public void Move(CellEventArgs args)
        {
            var hero = Grid.Instance.Hero;
            GridController.Instance.Move(hero, args.Cell);
        }
    }
}
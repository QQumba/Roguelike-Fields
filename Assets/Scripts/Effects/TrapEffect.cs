using Cells.Components;
using Events;
using GameGrid;
using UnityEngine;
using Grid = GameGrid.Grid;

namespace Effects
{
    public class TrapEffect : MonoBehaviour
    {
        [SerializeField]
        private int damage = 4;

        [SerializeField]
        private Sprite trapActiveSprite;
        
        [SerializeField]
        private Sprite trapInactiveSprite;

        [SerializeField]
        private SpriteRenderer trapSpriteRenderer;
        
        private bool _active = true;
        
        public void ToggleTrap(CellEventArgs _)
        {
            _active = !_active;
            trapSpriteRenderer.sprite = _active ? trapActiveSprite : trapInactiveSprite;
        }
        
        public void OnHeroEnter(CellEventArgs args)
        {
            var hero = Grid.Instance.Hero.GetCellComponent<Hero>();
            
            if (_active)
            {
                hero.Damageable.DealDamage(damage);
            }
        }
    }
}
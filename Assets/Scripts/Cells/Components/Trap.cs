using Cells.Components.Interfaces;
using UnityEngine;
using Grid = GameGrid.Grid;

namespace Cells.Components
{
    public class Trap : CellComponent, IPickable
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

        public override string CellTag => "trap";

        public override void OnTurnEnded()
        {
            ToggleTrap();
        }

        public void ToggleTrap()
        {
            _active = !_active;
            trapSpriteRenderer.sprite = _active ? trapActiveSprite : trapInactiveSprite;
        }
        
        public void PickUp()
        {
            var hero = Grid.Instance.Hero.GetCellComponent<Hero>();
            
            if (_active)
            {
                hero.DamageableLegacy.DealDamage(damage);
            }
        }
    }
}
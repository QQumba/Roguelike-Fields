using UnityEngine;

namespace Cells
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CellBackground : MonoBehaviour
    {
        [SerializeField]
        private Sprite defaultSprite;
        
        [SerializeField]
        private Sprite hoverSprite;

        private SpriteRenderer _renderer;
        
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void Hover()
        {
            _renderer.sprite = hoverSprite;
        }

        public void EndHover()
        {
            _renderer.sprite = defaultSprite;
        }
    }
}
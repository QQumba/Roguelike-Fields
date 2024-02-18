using UnityEngine;

namespace Misc
{
    public class SpritePresenter : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer sprite;
        
        public void ShowSprite()
        {
            if (sprite.enabled == false)
            {
                sprite.enabled = true;
            }
        }
        
        public void HideSprite()
        {
            sprite.enabled = false;
        }
    }
}
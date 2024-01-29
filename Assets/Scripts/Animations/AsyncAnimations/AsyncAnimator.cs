using System.Threading.Tasks;
using UnityEngine;

namespace Animations.AsyncAnimations
{
    public class AsyncAnimator : MonoBehaviour
    {
        private int _animationsInProgress;
        
        public static AsyncAnimator Instance { get; private set; }

        public bool IsAnimating => _animationsInProgress > 0;

        private void Awake()
        {
            Instance = this;
        }

        public async Task Play(IAsyncAnimation asyncAnimation)
        {
            _animationsInProgress++;
            await asyncAnimation.Play();
            _animationsInProgress--;
        }
    }
}
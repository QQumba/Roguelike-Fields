using System.Threading.Tasks;
using Cells;
using UnityEngine;

namespace Animations.AsyncAnimations
{
    public class AsyncAnimator : MonoBehaviour
    {
        private int _animationsInProgress;
        private int _millisecondsScheduledToWait;

        public static AsyncAnimator Instance { get; private set; }

        public bool IsAnimating => _animationsInProgress > 0 || _millisecondsScheduledToWait > 0;

        private void Awake()
        {
            Instance = this;
        }

        public void Wait(int milliseconds)
        {
            _millisecondsScheduledToWait += milliseconds;
        }

        public async Task Play(IAsyncAnimation asyncAnimation)
        {
            _animationsInProgress++;

            if (_millisecondsScheduledToWait > 0)
            {
                var timeToWait = _millisecondsScheduledToWait;
                _millisecondsScheduledToWait -= timeToWait;
                await Task.Delay(timeToWait);
            }

            await asyncAnimation.PlayAsync();
            _animationsInProgress--;
        }

        public async Task Play(IAsyncAnimation asyncAnimation, Cell cell)
        {
            cell.IsAnimated = true;
            await Play(asyncAnimation);
            cell.IsAnimated = false;
        }
    }
}
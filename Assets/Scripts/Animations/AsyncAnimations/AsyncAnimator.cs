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
    }
}
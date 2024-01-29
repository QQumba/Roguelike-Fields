using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Animations
{
    public interface IAnimator
    {
        void Play(AnimationArgs args);
        void PlayCoroutine(CoroutineAnimation args);

        // void Enqueue(AnimationArgs args, bool playImmediately = false);
    }
    
    /// <summary>
    /// Entry point to call any animation
    /// </summary>
    public class Animator : MonoBehaviour, IAnimator
    {
        [SerializeField]
        private Animation[] animations;

        public static IAnimator Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private readonly Queue<AnimationArgs> _animationQueue;

        public bool IsAnimating => _animationQueue.Any();

        public void Play(AnimationArgs args)
        {
            var a = animations.First(x => x.Type == args.Type);
            a.Play(args, args.Callback);
        }

        public void PlayCoroutine(CoroutineAnimation a)
        {
            StartCoroutine(a.Play());
        }
        
        public void Enqueue(AnimationArgs args, bool playImmediately = false)
        {
            _animationQueue.Enqueue(args);
            if (_animationQueue.Count == 1)
            {
                PlayNext();
            }
        }

        private void PlayNext()
        {
            if (!_animationQueue.Any())
            {
                return;
            }

            if (_animationQueue.Count > 1)
            {
                _animationQueue.Dequeue();
            }

            var args = _animationQueue.Peek();
            var a = animations.First(x => x.Type == args.Type);
            a.AnimationFinished += PlayNext;
            a.Play(args, () => { });
        }

        private Animation GetAnimation(AnimationType type)
        {
            return animations.First(x => x.Type == type);
        }
    }
}
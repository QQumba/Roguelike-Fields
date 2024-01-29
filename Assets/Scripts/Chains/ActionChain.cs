using System;
using System.Collections.Generic;
using Animations;

namespace Chains
{
    // TODO: WARNING!!! Very much WIP, need to refactor this and rethink animation handling
    public class ActionChain
    {
        private readonly Queue<IActionChainLink> _links = new Queue<IActionChainLink>();

        public ActionChain Then(Action action)
        {
            var link = new ActionChainLink(action);
            _links.Enqueue(link);
            return this;
        }
        
        public ActionChain Then(AnimationArgs args)
        {
            var link = new AnimationActionChainLink(args);
            _links.Enqueue(link);
            return this;
        }
        
        public ActionChain Then(Func<CoroutineAnimation> animationCallback)
        {
            var link = new CoroutineAnimationActionChainLink(animationCallback);
            _links.Enqueue(link);
            return this;
        }

        public void Run()
        {
            RunNext();
        }

        private void RunNext()
        {
            if (_links.Count == 0)
            {
                return;
            }

            var link = _links.Dequeue();
            link.Run(RunNext);
        }
    }

    public interface IActionChainLink
    {
        void Run(Action next);
    }

    public class ActionChainLink : IActionChainLink
    {
        private readonly Action _action;
        
        public ActionChainLink(Action action)
        {
            _action = action;
        }
        
        public void Run(Action next)
        {
            _action();
            next();
        }
    }

    public class AnimationActionChainLink : IActionChainLink
    {
        private readonly AnimationArgs _animationArgs;

        public AnimationActionChainLink(AnimationArgs animationArgs)
        {
            _animationArgs = animationArgs;
        }

        public void Run(Action next)
        {
            _animationArgs.Callback = next;
            Animator.Instance.Play(_animationArgs);            
        }
    }
    
    public class CoroutineAnimationActionChainLink : IActionChainLink
    {
        private readonly Func<CoroutineAnimation> _func;

        public CoroutineAnimationActionChainLink(Func<CoroutineAnimation> func)
        {
            _func = func;
        }

        public void Run(Action next)
        {
            var animation = _func();
            animation.AnimationFinished += next;
            Animator.Instance.PlayCoroutine(animation);            
        }
    }

}
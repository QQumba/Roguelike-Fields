using System;
using Animations.AsyncAnimations;
using UnityEngine;

namespace Cells.Utilities
{
    [RequireComponent(
        typeof(BoxCollider2D),
        typeof(CellSlot))]
    public class CellHoverHandler : MonoBehaviour
    {
        [SerializeField]
        private float hoverSpeed = 4;

        [SerializeField]
        private float hoverScale = 0.95f;

        [SerializeField]
        private float defaultScale = 1f;

        private CellSlot _slot;
        private Coroutine _coroutine;
        private AsyncAnimator _animator;
        private IAsyncAnimation _activeAnimation;

        private void Awake()
        {
            _slot = GetComponent<CellSlot>();
            // _slot.Cleared += () =>
            // {
            //     if (_coroutine != null)
            //     {
            //         StopCoroutine(_coroutine);
            //     }
            // };
            
            _slot.Cleared += () =>
            {
                _activeAnimation?.RequestStop();
            };
        }

        private void Start()
        {
            _animator = AsyncAnimator.Instance;
        }

        // TODO stop animation when cell is clicked to avoid any errors
        private void OnMouseEnter()
        {
            // if (_slot.Cell == null)
            // {
            //     return;
            // }
            //
            // if (_coroutine != null)
            // {
            //     StopCoroutine(_coroutine);
            // }
            //
            // _coroutine = StartCoroutine(ChangeScale(hoverScale));

            if (_animator.IsAnimating)
            {
                return;
            }
            
            if (_slot.Cell == null)
            {
                return;
            }

            _activeAnimation?.RequestStop();
            ChangeScale(hoverScale);
        }

        private void OnMouseExit()
        {
            // if (_slot.Cell == null)
            // {
            //     return;
            // }
            //
            // StopCoroutine(_coroutine);
            // _coroutine = StartCoroutine(ChangeScale(defaultScale));
            
            if (_animator.IsAnimating)
            {
                return;
            }
            
            if (_slot.Cell == null)
            {
                return;
            }
            
            _activeAnimation?.RequestStop();
            ChangeScale(defaultScale);
        }

        // private IEnumerator ChangeScale(float targetScale)
        // {
        //     var targetTransform = _slot.Cell.transform;
        //     return ScaleAnimation.Play(targetTransform,
        //         targetTransform.localScale,
        //         Vector3.one * targetScale,
        //         hoverSpeed);
        // }

        private async void ChangeScale(float targetScale)
        {
            var targetTransform = _slot.Cell.transform;

            var scaleAnimation =  new ScaleAsync(
                targetTransform,
                Vector3.one * targetScale,
                targetTransform.localScale,
                hoverSpeed);

            _activeAnimation = scaleAnimation;
            await scaleAnimation.Play();
        }
    }
}
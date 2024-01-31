using Animations.AsyncAnimations;
using Events;
using UnityEngine;

namespace Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PlayParticleEffect : MonoBehaviour
    {
        public void Play(CellEventArgs args)
        {
            var particles = GetComponent<ParticleSystem>();
            particles.Play();
            var waitMs = (int)(particles.main.duration * 1000);
            AsyncAnimator.Instance.Wait(waitMs);
        }
    }
}
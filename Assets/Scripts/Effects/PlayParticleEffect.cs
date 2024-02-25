using Animations;
using Events;
using GameGrid;
using TurnData;
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
            GridController.Instance.CurrentTurn.Next(() => Coroutines.Wait(particles.main.duration), "wait for particles"); 
        }
    }
}
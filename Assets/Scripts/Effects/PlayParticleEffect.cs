using System.Collections;
using Events;
using GameGrid;
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
            GridController.Instance.CurrentTurn.Next(() => Wait(particles.main.duration), "wait for particles"); 
        }

        private IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
    }
}
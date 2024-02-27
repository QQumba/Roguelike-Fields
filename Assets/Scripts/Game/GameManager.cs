using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public event Action GameEnding;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
        }

        public void EndGame()
        {
            GameEnding?.Invoke();

            SceneManager.LoadScene(0);
        }
    }
}
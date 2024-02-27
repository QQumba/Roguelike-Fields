using Game;
using UnityEngine;

namespace Effects
{
    public class GameOverEffect : MonoBehaviour
    {
        public void GameOver()
        {
            GameManager.Instance.EndGame();
        }
    }
}
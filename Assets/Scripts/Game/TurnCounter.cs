using TMPro;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(TextMeshPro))]
    public class TurnCounter : MonoBehaviour
    {
        private TextMeshPro _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshPro>();
        }

        private int _turnCount;

        public void AddTurn()
        {
            _turnCount++;
            _text.text = _turnCount.ToString();
        }
    }
}
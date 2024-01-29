using Events;
using UnityEngine;

namespace DefaultNamespace
{
    public class CellEventHandler : MonoBehaviour
    {
        public void OnCellEvent(CellEventArgs cellEventArgs)
        {
            Debug.Log(cellEventArgs.Cell.name);
        }
    }
}
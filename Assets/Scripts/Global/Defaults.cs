using Cells;
using UnityEngine;

namespace Global
{
    public class Defaults : MonoBehaviour
    {
        [SerializeField] CellContent emptyContent;
        
        public static CellContent EmptyContent { get; private set; }

        private void Start()
        {
            EmptyContent = emptyContent;
        }
    }
}
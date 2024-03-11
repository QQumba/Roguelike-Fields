using System;
using GameGrid;
using Object = UnityEngine.Object;

namespace TurnData.FragmentedTurn
{
    public interface ITurnContext
    {
        ITurnContext Next(TurnAction action);

        void Destroy(Object obj, int secondsDelay = 3);
        
        event Action TurnFinished;

        Direction TurnDirection { get; set; }

        void StartTurn();
    }
}
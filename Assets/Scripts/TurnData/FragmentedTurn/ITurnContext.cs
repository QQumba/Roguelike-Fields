using System;
using GameGrid;

namespace TurnData.FragmentedTurn
{
    public interface ITurnContext
    {
        void Next(TurnAction action);

        event Action TurnFinished;

        Direction TurnDirection { get; set; }

        void StartTurn();
    }
}
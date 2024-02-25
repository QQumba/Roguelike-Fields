using System;

namespace TurnData.FragmentedTurn
{
    public interface IActionQueue
    {
        event Action Completed;

        void Start();
    }
}
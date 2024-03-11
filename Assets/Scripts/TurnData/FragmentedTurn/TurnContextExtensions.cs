using System;
using System.Collections;

namespace TurnData.FragmentedTurn
{
    public static class TurnContextExtensions
    {
        public static ITurnContext Next(this ITurnContext context, Action action)
        {
            var a = new TurnAction(action);
            context.Next(a);
            return context;
        }

        public static ITurnContext Next(this ITurnContext context, Action action, string message)
        {
            var a = new TurnAction(action) { DebugMessage = message };
            context.Next(a);
            return context;
        }

        public static ITurnContext Next(this ITurnContext context, Func<IEnumerator> action)
        {
            var a = new TurnAction(action);
            context.Next(a);
            return context;
        }

        public static ITurnContext Next(this ITurnContext context, Func<IEnumerator> action, string message)
        {
            var a = new TurnAction(action) { DebugMessage = message };
            context.Next(a);
            return context;
        }
    }
}
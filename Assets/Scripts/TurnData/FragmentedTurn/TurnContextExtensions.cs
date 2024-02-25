using System;
using System.Collections;
using TurnData.FragmentedTurn;

namespace TurnData
{
    public static class TurnContextExtensions
    {
        public static void Next(this ITurnContext context, Action action)
        {
            var a = new TurnAction(action);
            context.Next(a);
        }

        public static void Next(this ITurnContext context, Action action, string message)
        {
            var a = new TurnAction(action) { Message = message };
            context.Next(a);
        }

        public static void Next(this ITurnContext context, Func<IEnumerator> action)
        {
            var a = new TurnAction(action);
            context.Next(a);
        }

        public static void Next(this ITurnContext context, Func<IEnumerator> action, string message)
        {
            var a = new TurnAction(action) { Message = message };
            context.Next(a);
        }
    }
}
using System.Collections;
using UnityEngine;

namespace Animations
{
    public class Coroutines
    {
        public static IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
    }
}
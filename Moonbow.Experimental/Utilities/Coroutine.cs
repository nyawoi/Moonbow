using System;
using System.Collections.Generic;
using Plukit.Base;

namespace AetharNet.Moonbow.Experimental.Utilities
{
    public static class Coroutine
    {
        internal static readonly LinkedList<IEnumerator<Timestep>> Coroutines = new();
        
        public static void StartCoroutine(IEnumerable<Timestep> coroutine)
        {
            Coroutines.AddLast(coroutine.GetEnumerator());
        }
        
        public static void StartCoroutine(IEnumerator<Timestep> coroutine)
        {
            Coroutines.AddLast(coroutine);
        }

        public static void AwaitCondition(Func<bool> condition, Action callback)
        {
            StartCoroutine(AwaitConditionCoroutine(condition, callback));
        }

        public static void AwaitTimedCondition(Timestep expiration, Func<bool> condition, Action<bool> callback)
        {
            StartCoroutine(AwaitTimedConditionCoroutine(expiration, condition, callback));
        }

        private static IEnumerable<Timestep> AwaitConditionCoroutine(Func<bool> condition, Action callback)
        {
            while (true)
            {
                if (condition())
                {
                    callback();
                    yield break;
                }

                yield return GameUtilities.Universe.Step + 1;
            }
        }

        private static IEnumerable<Timestep> AwaitTimedConditionCoroutine(Timestep expiration, Func<bool> condition, Action<bool> callback)
        {
            while (true)
            {
                var step = GameUtilities.Universe.Step;

                if (condition() || step > expiration)
                {
                    callback(step > expiration);
                    yield break;
                }

                yield return step + 1;
            }
        }
    }
}
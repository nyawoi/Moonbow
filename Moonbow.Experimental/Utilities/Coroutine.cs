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
    }
}
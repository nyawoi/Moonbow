using System;
using System.Collections.Generic;
using AetharNet.Moonbow.Experimental.Templates;
using AetharNet.Moonbow.Experimental.Utilities;
using Plukit.Base;
using Staxel.Logic;
using Staxel.Modding;

namespace AetharNet.Moonbow.Experimental.Hooks
{
    public class CoroutineHook : ModHookV4Template, IModHookV4
    {
        private static LinkedList<IEnumerator<Timestep>> Coroutines => Coroutine.Coroutines;
        
        public override void UniverseUpdateBefore(Universe universe, Timestep step)
        {
            // This is a basic reimplementation of Staxel's Villager/Merchant coroutines
            
            var nextNode = Coroutines.First;
            while (nextNode != null)
            {
                var currentNode = nextNode;
                nextNode = nextNode.Next;
                
                bool shouldLoop;
                
                do
                {
                    shouldLoop = false;
                    
                    try
                    {
                        if (!(currentNode.Value.Current > step))
                        {
                            if (!currentNode.Value.MoveNext())
                            {
                                currentNode.Value.Dispose();
                                Coroutines.Remove(currentNode);
                            }
                            else
                            {
                                shouldLoop = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("Exception processing Moonbow coroutine", ex);
                        currentNode.Value.Dispose();
                        Coroutines.Remove(currentNode);
                    }
                } while (shouldLoop);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using AetharNet.Moonbow.Experimental.Hooks;
using AetharNet.Moonbow.Experimental.Interfaces;
using HarmonyLib;
using Staxel.Client;
using Staxel.Core;

namespace AetharNet.Moonbow.Experimental.Patches
{
    internal static class OnInputPatch
    {
        private const string HarmonyPatchId = "net.aethar.Moonbow.Patches.OnInputPatch";
        private static IEnumerable<IClientMessagingHook> MessagingHooks => MessagingComponentHook.ClientMessagingHooks;

        private static bool Prefix(ref string input)
        {
            if (input.Length == 0) return false;
            
            var shouldRunOriginal = true;

            if (input.StartsWith(Constants.ConsoleCommandToken))
            {
                foreach (var hook in MessagingHooks)
                {
                    try
                    {
                        shouldRunOriginal &= hook.InterceptOutgoingCommand(ref input);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Exception in Mod {hook.GetType().Name}", e);
                    }
                }
            }
            else
            {
                foreach (var hook in MessagingHooks)
                {
                    try
                    {
                        shouldRunOriginal &= hook.InterceptOutgoingMessage(ref input);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Exception in Mod {hook.GetType().Name}", e);
                    }
                }
            }

            return shouldRunOriginal;
        }

        private static void Postfix(string input, bool __runOriginal)
        {
            if (input.Length == 0 || __runOriginal) return;

            if (input.StartsWith(Constants.ConsoleCommandToken))
            {
                foreach (var hook in MessagingHooks)
                {
                    try
                    {
                        hook.OnPlayerCommandSent(input);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Exception in Mod {hook.GetType().Name}", e);
                    }
                }
            }
            else
            {
                foreach (var hook in MessagingHooks)
                {
                    try
                    {
                        hook.OnPlayerMessageSent(input);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Exception in Mod {hook.GetType().Name}", e);
                    }
                }
            }
        }

        public static void Initialize()
        {
            if (Harmony.HasAnyPatches(HarmonyPatchId)) return; 
            var harmonyInstance = new Harmony(HarmonyPatchId);
            
            var methodBase = AccessTools.Method(typeof(ChatController), "OnInput"); 
            var methodPrefix = AccessTools.Method(typeof(OnInputPatch), nameof(Prefix));
            var methodPostfix = AccessTools.Method(typeof(OnInputPatch), nameof(Postfix));
            
            harmonyInstance.Patch(methodBase, new HarmonyMethod(methodPrefix), new HarmonyMethod(methodPostfix));
        }
    }
}
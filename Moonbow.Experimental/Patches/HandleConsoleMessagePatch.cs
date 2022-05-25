using System;
using System.Collections.Generic;
using AetharNet.Moonbow.Experimental.Hooks;
using AetharNet.Moonbow.Experimental.Interfaces;
using HarmonyLib;
using Plukit.Base;
using Staxel;
using Staxel.Commands;
using Staxel.Core;
using Staxel.Server;

namespace AetharNet.Moonbow.Experimental.Patches
{
    internal static class HandleConsoleMessagePatch
    {
        private const string HarmonyPatchId = "net.aethar.Moonbow.Patches.HandleConsoleMessagePatch";
        private static IEnumerable<IServerMessagingHook> MessagingHooks => MessagingComponentHook.ServerMessagingHooks;

        private static bool Prefix(ICommandsApi __instance, Blob blob, ClientServerConnection connection)
        {
            var message = blob.GetString("message");
            var shouldRunOriginal = true;

            if (message.StartsWith(Constants.ConsoleCommandToken))
            {
                foreach (var hook in MessagingHooks)
                {
                    try
                    {
                        shouldRunOriginal &= hook.InterceptPlayerCommand(connection.ConnectionEntityId, ref message, __instance);
                    }
                    catch (Exception e)
                    {   
                        throw new Exception($"Exception in Mod {hook.GetType().Name}", e);
                    }
                }
            }
            else if (!ServerContext.RightsManager.IsMuted(connection.Credentials.Uid))
            {
                foreach (var hook in MessagingHooks)
                {
                    try
                    {
                        shouldRunOriginal &= hook.InterceptPlayerMessage(connection.ConnectionEntityId, ref message, __instance);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Exception in Mod {hook.GetType().Name}", e);
                    }
                }
            }
            
            return shouldRunOriginal;
        }

        private static void Postfix(ICommandsApi __instance, Blob blob, ClientServerConnection connection, bool __runOriginal)
        {
            if (__runOriginal) return;

            var message = blob.GetString("message");

            if (message.StartsWith(Constants.ConsoleCommandToken))
            {
                foreach (var component in MessagingHooks)
                {
                    try
                    {
                        component.OnPlayerCommandReceived(connection.ConnectionEntityId, message, __instance);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Exception in Mod {component.GetType().Name}", e);
                    }
                }
            }
            else if (ServerContext.RightsManager.IsMuted(connection.Credentials.Uid))
            {
                foreach (var hook in MessagingHooks)
                {
                    try
                    {
                        hook.OnPlayerMessageBlocked(connection.ConnectionEntityId, message, __instance);
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
                        hook.OnPlayerMessageReceived(connection.ConnectionEntityId, message, __instance);
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
            
            var methodBase = AccessTools.Method(typeof(ServerMainLoop), nameof(ServerMainLoop.HandleConsoleMessage));
            var methodPrefix = AccessTools.Method(typeof(HandleConsoleMessagePatch), nameof(Prefix));
            var methodPostfix = AccessTools.Method(typeof(HandleConsoleMessagePatch), nameof(Postfix));

            harmonyInstance.Patch(methodBase, new HarmonyMethod(methodPrefix), new HarmonyMethod(methodPostfix));
        }
    }
}
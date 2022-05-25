using System;
using System.Collections.Generic;
using AetharNet.Moonbow.Experimental.Hooks;
using AetharNet.Moonbow.Experimental.Interfaces;
using HarmonyLib;
using Plukit.Base;
using Staxel.Client;

namespace AetharNet.Moonbow.Experimental.Patches
{
    internal static class ReceiveEntitySayPatch
    {
        private const string HarmonyPatchId = "net.aethar.Moonbow.Patches.ReceiveEntitySayPatch";
        private static IEnumerable<IClientMessagingHook> MessagingHooks => MessagingComponentHook.ClientMessagingHooks;

        private static bool Prefix(Blob blob)
        {
            var message = blob.GetString("message");
            var shouldRunOriginal = true;

            foreach (var hook in MessagingHooks)
            {
                try
                {
                    shouldRunOriginal &= hook.InterceptIncomingMessage(blob.GetString("nick"), ref message);
                }
                catch (Exception e)
                {
                    throw new Exception($"Exception in Mod {hook.GetType().Name}", e);
                }
            }
            
            blob.SetString("message", message);
            return shouldRunOriginal;
        }

        private static void Postfix(Blob blob, bool __runOriginal)
        {
            if (__runOriginal) return;
            
            foreach (var hook in MessagingHooks)
            {
                try
                {
                    hook.OnEntityMessageReceived(blob.GetString("nick"), blob.GetString("message"));
                }
                catch (Exception e)
                {
                    throw new Exception($"Exception in Mod {hook.GetType().Name}", e);
                }
            }
        }

        public static void Initialize()
        {
            if (Harmony.HasAnyPatches(HarmonyPatchId)) return; 
            var harmonyInstance = new Harmony(HarmonyPatchId);
            
            var methodBase = AccessTools.Method(typeof(ChatController), nameof(ChatController.ReceiveEntitySay)); 
            var methodPrefix = AccessTools.Method(typeof(ReceiveEntitySayPatch), nameof(Prefix));
            var methodPostfix = AccessTools.Method(typeof(ReceiveEntitySayPatch), nameof(Postfix));
            
            harmonyInstance.Patch(methodBase, new HarmonyMethod(methodPrefix), new HarmonyMethod(methodPostfix));
        }
    }
}
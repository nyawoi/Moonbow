using System;
using System.Collections.Generic;
using AetharNet.Moonbow.Experimental.Hooks;
using AetharNet.Moonbow.Experimental.Interfaces;
using AetharNet.Moonbow.Experimental.Utilities;
using HarmonyLib;
using Staxel.Server;

namespace AetharNet.Moonbow.Experimental.Patches
{
    internal static class KickPlayerPatch
    {
        private const string HarmonyPatchId = "net.aethar.Moonbow.Patches.KickPlayerPatch";
        private static IEnumerable<IServerConnectionHook> ConnectionHooks => ConnectionComponentHook.ServerConnectionHooks;

        private static void Postfix(string uid)
        {
            foreach (var hook in ConnectionHooks)
            {
                try
                {
                    if (GameUtilities.Universe.TryGetEntityByGUID(uid, out var playerEntity))
                    {
                        hook.OnPlayerKicked(playerEntity);
                    }
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
            
            var methodBase = AccessTools.Method(typeof(ServerMainLoop), nameof(ServerMainLoop.KickPlayer));
            var methodPostfix = AccessTools.Method(typeof(KickPlayerPatch), nameof(Postfix));
            
            harmonyInstance.Patch(methodBase, postfix: new HarmonyMethod(methodPostfix));
        }
    }
}
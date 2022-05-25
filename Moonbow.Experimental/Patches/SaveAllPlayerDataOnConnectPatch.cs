using System;
using System.Collections.Generic;
using AetharNet.Moonbow.Experimental.Hooks;
using AetharNet.Moonbow.Experimental.Interfaces;
using HarmonyLib;
using Staxel.EntityStorage;
using Staxel.Logic;

namespace AetharNet.Moonbow.Experimental.Patches
{
    internal static class SaveAllPlayerDataOnConnectPatch
    {
        private const string HarmonyPatchId = "net.aethar.Moonbow.Patches.SaveAllPlayerDataOnConnectPatch";
        private static IEnumerable<IServerConnectionHook> ConnectionHooks => ConnectionComponentHook.ServerConnectionHooks;
        
        private static void Postfix(Entity entity)
        {
            if (entity.PlayerEntityLogic == null) return;

            foreach (var hook in ConnectionHooks)
            {
                try
                {
                    hook.OnPlayerConnect(entity);
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
            
            var methodBase = AccessTools.Method(typeof(PlayerPersistence), nameof(PlayerPersistence.SaveAllPlayerDataOnConnect));
            var methodPostfix = AccessTools.Method(typeof(SaveAllPlayerDataOnConnectPatch), nameof(Postfix));
            
            harmonyInstance.Patch(methodBase, postfix: new HarmonyMethod(methodPostfix));
        }
    }
}
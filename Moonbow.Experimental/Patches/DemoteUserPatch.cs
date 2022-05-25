﻿using System;
using System.Collections.Generic;
using AetharNet.Moonbow.Experimental.Hooks;
using AetharNet.Moonbow.Experimental.Interfaces;
using HarmonyLib;
using Staxel.Player;
using Staxel.Rights;

namespace AetharNet.Moonbow.Experimental.Patches
{
    internal static class DemoteUserPatch
    {
        private const string HarmonyPatchId = "net.aethar.Moonbow.Patches.DemoteUserPatch";
        private static IEnumerable<IServerConnectionHook> ConnectionHooks => ConnectionComponentHook.ServerConnectionHooks;

        private static void Postfix(bool __result, PlayerEntityLogic user)
        {
            if (!__result) return;
            
            foreach (var hook in ConnectionHooks)
            {
                try
                {
                    hook.OnPlayerDemoted(user.PlayerEntity);
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
            
            var methodBase = AccessTools.Method(typeof(RightsManager), nameof(RightsManager.DemoteUser));
            var methodPostfix = AccessTools.Method(typeof(DemoteUserPatch), nameof(Postfix));
            
            harmonyInstance.Patch(methodBase, postfix: new HarmonyMethod(methodPostfix));
        }
    }
}
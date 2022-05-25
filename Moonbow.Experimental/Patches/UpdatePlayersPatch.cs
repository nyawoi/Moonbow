using System;
using System.Collections.Generic;
using System.Linq;
using AetharNet.Moonbow.Experimental.Hooks;
using AetharNet.Moonbow.Experimental.Interfaces;
using AetharNet.Moonbow.Experimental.Utilities;
using HarmonyLib;
using Staxel;
using Staxel.Client;
using Staxel.Logic;

namespace AetharNet.Moonbow.Experimental.Patches
{
    internal static class UpdatePlayersPatch
    {
        private const string HarmonyPatchId = "net.aethar.Moonbow.Patches.UpdatePlayersPatch";
        private static IEnumerable<IClientConnectionHook> ConnectionHooks => ConnectionComponentHook.ClientConnectionHooks;
        private static List<PlayerListEntry> CachedPlayerList => ConnectionComponentHook.CachedClientPlayerList;
        private static IEnumerable<PlayerListEntry> CurrentPlayerList => ClientContext.OverlayController?.PlayerList?.GetPlayerList();
        private static Universe Universe => GameUtilities.Universe;

        private static void Postfix()
        {
            var currentPlayerList = CurrentPlayerList.ToList();
            
            if (CachedPlayerList.Count == 0)
            {
                foreach (var entry in currentPlayerList) CachedPlayerList.Add(entry);
                return;
            }
            
            var comparer = new EntryUidComparer();
            var disconnectedPlayers = CachedPlayerList.Except(currentPlayerList, comparer).ToList();
            var connectedPlayers = currentPlayerList.Except(CachedPlayerList, comparer).ToList();
            var renamedPlayers = (from currentEntry in currentPlayerList from cachedEntry in CachedPlayerList where currentEntry.Uid == cachedEntry.Uid && currentEntry.Nickname != cachedEntry.Nickname select cachedEntry).ToList();

            foreach (var hook in ConnectionHooks)
            {
                try
                {
                    foreach (var connectedPlayer in connectedPlayers)
                    {
                        if (Universe.TryGetEntityByGUID(connectedPlayer.Uid, out var playerEntity))
                        {
                            hook.OnPlayerConnect(playerEntity);
                        }
                    }
            
                    foreach (var disconnectedPlayer in disconnectedPlayers)
                    {
                        if (Universe.TryGetEntityByGUID(disconnectedPlayer.Uid, out var playerEntity))
                        {
                            hook.OnPlayerDisconnect(playerEntity);
                        }
                    }
            
                    foreach (var renamedPlayer in renamedPlayers)
                    {
                        if (Universe.TryGetEntityByGUID(renamedPlayer.Uid, out var playerEntity))
                        {
                            hook.OnPlayerRename(playerEntity, renamedPlayer.Nickname);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"Exception in Mod {hook.GetType().Name}", e);
                }
            }
            
            CachedPlayerList.Clear();
            foreach (var entry in currentPlayerList) CachedPlayerList.Add(entry);
        }
        
        public static void Initialize()
        {
            if (Harmony.HasAnyPatches(HarmonyPatchId)) return; 
            var harmonyInstance = new Harmony(HarmonyPatchId);
            
            var methodBase = AccessTools.Method(typeof(PlayerListController), nameof(PlayerListController.UpdatePlayers));
            var methodPostfix = AccessTools.Method(typeof(UpdatePlayersPatch), nameof(Postfix));
            
            harmonyInstance.Patch(methodBase, postfix: new HarmonyMethod(methodPostfix));
        }

        private class EntryUidComparer : IEqualityComparer<PlayerListEntry>
        {
            public bool Equals(PlayerListEntry x, PlayerListEntry y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Uid == y.Uid;
            }

            public int GetHashCode(PlayerListEntry obj)
            {
                return obj.Uid.GetHashCode();
            }
        }
    }
}
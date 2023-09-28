using System.Collections.Generic;
using AetharNet.Moonbow.Experimental.Interfaces;
using AetharNet.Moonbow.Experimental.Patches;
using AetharNet.Moonbow.Experimental.Templates;
using AetharNet.Moonbow.Experimental.Utilities;
using Plukit.Base;
using Staxel.Client;
using Staxel.Modding;

namespace AetharNet.Moonbow.Experimental.Hooks
{
    internal class ConnectionComponentHook : ModHookV4Template, IModHookV4
    {
        public static readonly List<IClientConnectionHook> ClientConnectionHooks = new();
        public static readonly List<IServerConnectionHook> ServerConnectionHooks = new();
        public static readonly List<PlayerListEntry> CachedClientPlayerList = new();

        static ConnectionComponentHook()
        {
            if (GameUtilities.IsClient)
            {
                UpdatePlayersPatch.Initialize();
                ClientMainLoopPatch.Initialize();

                foreach (var connectionHook in Loader.Instance<IClientConnectionHook>())
                {
                    ClientConnectionHooks.Add(connectionHook);
                }
            }
            else if (GameUtilities.IsServer)
            {
                SaveAllPlayerDataOnConnectPatch.Initialize();
                SaveDisconnectingPlayerPatch.Initialize();
                KickPlayerPatch.Initialize();
                BanUserPatch.Initialize();
                UnbanUserPatch.Initialize();
                MuteUserPatch.Initialize();
                UnmuteUserPatch.Initialize();
                PromoteUserPatch.Initialize();
                DemoteUserPatch.Initialize();

                foreach (var connectionHook in Loader.Instance<IServerConnectionHook>())
                {
                    ServerConnectionHooks.Add(connectionHook);
                }
            }
        }

        public override void Dispose()
        {
            if (GameUtilities.IsClient)
            {
                foreach (var connectionHook in ClientConnectionHooks)
                {
                    connectionHook.Dispose();
                }
                
                ClientConnectionHooks.Clear();
            }
            else if (GameUtilities.IsServer)
            {
                foreach (var connectionHook in ServerConnectionHooks)
                {
                    connectionHook.Dispose();
                }
                
                ServerConnectionHooks.Clear();
            }
        }

        public override void CleanupOldSession()
        {
            CachedClientPlayerList.Clear();
        }
    }
}
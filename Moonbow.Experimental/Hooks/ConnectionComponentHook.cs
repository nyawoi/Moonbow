using System.Collections;
using System.Collections.Generic;
using AetharNet.Moonbow.Experimental.Interfaces;
using AetharNet.Moonbow.Experimental.Patches;
using AetharNet.Moonbow.Experimental.Templates;
using AetharNet.Moonbow.Experimental.Utilities;
using Plukit.Base;
using Staxel;
using Staxel.Client;
using Staxel.Logic;
using Staxel.Modding;

namespace AetharNet.Moonbow.Experimental.Hooks
{
    internal class ConnectionComponentHook : ModHookTemplate, IModHookV4
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
            }
        }

        public override void Dispose()
        {
            ClientConnectionHooks.Clear();
            ServerConnectionHooks.Clear();
        }

        public override void CleanupOldSession()
        {
            CachedClientPlayerList.Clear();
        }
        
        public override void GameContextInitializeAfter()
        {
            ClientConnectionHooks.Clear();
            ServerConnectionHooks.Clear();

            var modHookInstances = GameContext.ModdingController.AccessField<IEnumerable>("_modHooks");
            foreach (var hookInstance in modHookInstances)
            {
                switch (hookInstance.AccessField<IModHookV4>("ModHookV4"))
                {
                    case IClientConnectionHook messagingHook:
                        ClientConnectionHooks.Add(messagingHook);
                        break;
                    case IServerConnectionHook messagingHook:
                        ServerConnectionHooks.Add(messagingHook);
                        break;
                }
            }
        }
    }
}
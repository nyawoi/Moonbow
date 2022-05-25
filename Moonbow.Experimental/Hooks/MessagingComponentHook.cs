using System.Collections;
using AetharNet.Moonbow.Experimental.Interfaces;
using AetharNet.Moonbow.Experimental.Patches;
using AetharNet.Moonbow.Experimental.Templates;
using AetharNet.Moonbow.Experimental.Utilities;
using Plukit.Base;
using Staxel;
using Staxel.Modding;
using Staxel.Translation;

namespace AetharNet.Moonbow.Experimental.Hooks
{
    internal class MessagingComponentHook : ModHookTemplate, IModHookV4
    {
        public static readonly Lyst<IClientMessagingHook> ClientMessagingHooks = new();
        public static readonly Lyst<IServerMessagingHook> ServerMessagingHooks = new();

        static MessagingComponentHook()
        {
            if (GameUtilities.IsClient)
            {
                OnInputPatch.Initialize();
                ReceiveEntitySayPatch.Initialize();
            }
            else if (GameUtilities.IsServer)
            {
                HandleConsoleMessagePatch.Initialize();
            }
        }

        public override void Dispose()
        {
            ClientMessagingHooks.Clear();
            ServerMessagingHooks.Clear();
        }

        public override void GameContextInitializeAfter()
        {
            ClientMessagingHooks.Clear();
            ServerMessagingHooks.Clear();

            var modHookInstances = GameContext.ModdingController.AccessField<IEnumerable>("_modHooks");
            foreach (var hookInstance in modHookInstances)
            {
                switch (hookInstance.AccessField<IModHookV4>("ModHookV4"))
                {
                    case IClientMessagingHook messagingHook:
                        ClientMessagingHooks.Add(messagingHook);
                        break;
                    case IServerMessagingHook messagingHook:
                        ServerMessagingHooks.Add(messagingHook);
                        break;
                }
            }
        }

        public override void ClientContextInitializeAfter()
        {
            // ISSUE 2022-05-19: Attempting to join an unmanaged server will result in an exception due to LanguageDatabase being null.
            if (ClientContext.LanguageDatabase == null) return;
            
            var englishDatabase = ClientContext.LanguageDatabase.AccessField<Language>("_english");
            
            if (englishDatabase.LanguageStrings.ContainsKey(GameUtilities.EchoTranslationKey)) return;
            
            englishDatabase.AddString(GameUtilities.EchoTranslationKey, "{0}");
        }
    }
}
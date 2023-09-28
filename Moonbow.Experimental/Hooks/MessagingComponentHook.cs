using AetharNet.Moonbow.Experimental.Interfaces;
using AetharNet.Moonbow.Experimental.Patches;
using AetharNet.Moonbow.Experimental.Templates;
using AetharNet.Moonbow.Experimental.Utilities;
using Plukit.Base;
using Staxel.Modding;

namespace AetharNet.Moonbow.Experimental.Hooks
{
    internal class MessagingComponentHook : ModHookV4Template, IModHookV4
    {
        public static readonly Lyst<IClientMessagingHook> ClientMessagingHooks = new();
        public static readonly Lyst<IServerMessagingHook> ServerMessagingHooks = new();

        static MessagingComponentHook()
        {
            if (GameUtilities.IsClient)
            {
                OnInputPatch.Initialize();
                ReceiveEntitySayPatch.Initialize();

                foreach (var messagingHook in Loader.Instance<IClientMessagingHook>())
                {
                    ClientMessagingHooks.Add(messagingHook);
                }
            }
            else if (GameUtilities.IsServer)
            {
                HandleConsoleMessagePatch.Initialize();

                foreach (var messagingHook in Loader.Instance<IServerMessagingHook>())
                {
                    ServerMessagingHooks.Add(messagingHook);
                }
            }
        }

        public override void Dispose()
        {
            if (GameUtilities.IsClient)
            {
                foreach (var messagingHook in ClientMessagingHooks)
                {
                    messagingHook.Dispose();
                }
                
                ClientMessagingHooks.Clear();
            }
            else if (GameUtilities.IsServer)
            {
                foreach (var messagingHook in ServerMessagingHooks)
                {
                    messagingHook.Dispose();
                }
                
                ServerMessagingHooks.Clear();
            }
        }
    }
}
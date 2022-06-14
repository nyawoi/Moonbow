using Staxel.Commands;
using Staxel.Logic;

namespace AetharNet.Moonbow.Experimental.Templates
{
    public class ServerMessagingHookTemplate
    {
        public bool InterceptPlayerCommand(EntityId entityId, ref string command, ICommandsApi api) => true;
        public bool InterceptPlayerMessage(EntityId entityId, ref string message, ICommandsApi api) => true;

        public void OnPlayerCommandReceived(EntityId entityId, string command, ICommandsApi api) {}
        public void OnPlayerMessageBlocked(EntityId entityId, string message, ICommandsApi api) {}
        public void OnPlayerMessageReceived(EntityId entityId, string message, ICommandsApi api) {}
    }
}
using Staxel.Commands;
using Staxel.Logic;

namespace AetharNet.Moonbow.Experimental.Templates
{
    public class ServerMessagingHookTemplate
    {
        public virtual bool InterceptPlayerCommand(EntityId entityId, ref string command, ICommandsApi api) => true;
        public virtual bool InterceptPlayerMessage(EntityId entityId, ref string message, ICommandsApi api) => true;

        public virtual void OnPlayerCommandReceived(EntityId entityId, string command, ICommandsApi api) {}
        public virtual void OnPlayerMessageBlocked(EntityId entityId, string message, ICommandsApi api) {}
        public virtual void OnPlayerMessageReceived(EntityId entityId, string message, ICommandsApi api) {}
    }
}
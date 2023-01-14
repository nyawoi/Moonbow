namespace AetharNet.Moonbow.Experimental.Templates
{
    public class ClientMessagingHookTemplate
    {
        public virtual void Dispose() {}
        public virtual bool InterceptOutgoingCommand(ref string command) => true;
        public virtual bool InterceptOutgoingMessage(ref string message) => true;
        public virtual bool InterceptIncomingMessage(string nick, ref string message) => true;

        public virtual void OnPlayerCommandSent(string command) {}
        public virtual void OnPlayerMessageSent(string message) {}
        public virtual void OnEntityMessageReceived(string nick, string message) {}
    }
}
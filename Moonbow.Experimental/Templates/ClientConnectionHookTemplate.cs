using Staxel.Logic;

namespace AetharNet.Moonbow.Experimental.Templates
{
    public class ClientConnectionHookTemplate
    {
        public virtual void Dispose() {}
        public virtual void OnPlayerConnect(Entity playerEntity) {}
        public virtual void OnPlayerDisconnect(Entity playerEntity) {}
        public virtual void OnPlayerRename(Entity playerEntity, string oldNickname) {}
    }
}
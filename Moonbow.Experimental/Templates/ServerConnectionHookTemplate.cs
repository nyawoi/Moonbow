using Staxel.Logic;

namespace AetharNet.Moonbow.Experimental.Templates
{
    public class ServerConnectionHookTemplate
    {
        public virtual void OnPlayerConnect(Entity playerEntity) {}
        public virtual void OnPlayerDisconnect(Entity playerEntity) {}
        public virtual void OnPlayerKicked(Entity playerEntity) {}
        public virtual void OnPlayerBanned(Entity playerEntity, string reason) {}
        public virtual void OnPlayerUnbanned(Entity playerEntity) {}
        public virtual void OnPlayerMuted(Entity playerEntity, int minutes) {}
        public virtual void OnPlayerUnmuted(Entity playerEntity) {}
        public virtual void OnPlayerPromoted(Entity playerEntity) {}
        public virtual void OnPlayerDemoted(Entity playerEntity) {}
    }
}
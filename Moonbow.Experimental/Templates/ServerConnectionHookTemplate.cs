using Staxel.Logic;

namespace AetharNet.Moonbow.Experimental.Templates
{
    public class ServerConnectionHookTemplate
    {
        public void OnPlayerConnect(Entity playerEntity) {}
        public void OnPlayerDisconnect(Entity playerEntity) {}
        public void OnPlayerKicked(Entity playerEntity) {}
        public void OnPlayerBanned(Entity playerEntity, string reason) {}
        public void OnPlayerUnbanned(Entity playerEntity) {}
        public void OnPlayerMuted(Entity playerEntity, int minutes) {}
        public void OnPlayerUnmuted(Entity playerEntity) {}
        public void OnPlayerPromoted(Entity playerEntity) {}
        public void OnPlayerDemoted(Entity playerEntity) {}
    }
}
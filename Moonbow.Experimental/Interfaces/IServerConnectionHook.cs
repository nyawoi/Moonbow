using System;
using Staxel.Logic;

namespace AetharNet.Moonbow.Experimental.Interfaces
{
    /// <summary>
    /// Mod hook with extended player connection functionality.
    /// Act upon events on the server.
    /// </summary>
    public interface IServerConnectionHook : IDisposable
    {
        /// <summary>
        /// Event fired whenever a player joins the game.
        /// </summary>
        /// <param name="playerEntity">The player who joined.</param>
        public void OnPlayerConnect(Entity playerEntity);
        /// <summary>
        /// Event fired whenever a player leaves the game.
        /// </summary>
        /// <param name="playerEntity">The player who left.</param>
        public void OnPlayerDisconnect(Entity playerEntity);
        /// <summary>
        /// Event fired whenever a player is kicked from the game.
        /// </summary>
        /// <param name="playerEntity">The player who was kicked.</param>
        public void OnPlayerKicked(Entity playerEntity);
        /// <summary>
        /// Event fired whenever a player is banned from the game.
        /// </summary>
        /// <param name="playerEntity">The player who was banned.</param>
        /// <param name="reason">The provided reason for the ban.</param>
        public void OnPlayerBanned(Entity playerEntity, string reason);
        /// <summary>
        /// Event fired whenever a player's ban is revoked.
        /// </summary>
        /// <param name="playerEntity">The player who was unbanned.</param>
        public void OnPlayerUnbanned(Entity playerEntity);
        /// <summary>
        /// Event fired whenever a player is muted from in-game chat.
        /// </summary>
        /// <param name="playerEntity">The player who was muted.</param>
        /// <param name="minutes">The duration of the mute, in minutes.</param>
        public void OnPlayerMuted(Entity playerEntity, int minutes);
        /// <summary>
        /// Event fired whenever a player's mute has expired.
        /// </summary>
        /// <param name="playerEntity">The player who was unmuted.</param>
        public void OnPlayerUnmuted(Entity playerEntity);
        /// <summary>
        /// Event fired whenever a player has been promoted to admin.
        /// </summary>
        /// <param name="playerEntity">The player who was promoted.</param>
        public void OnPlayerPromoted(Entity playerEntity);
        /// <summary>
        /// Event fired whenever a player has their admin status revoked.
        /// </summary>
        /// <param name="playerEntity">The player who was demoted.</param>
        public void OnPlayerDemoted(Entity playerEntity);
    }
}
using Staxel.Logic;
using Staxel.Modding;

namespace AetharNet.Moonbow.Experimental.Interfaces
{
    /// <summary>
    /// Mod hook with extended player connection functionality.
    /// Act upon events on the client.
    /// NOTE: Events are not immediate, as the current implementation relies on the client-side player list, which retrieves updates every so often.
    /// </summary>
    public interface IClientConnectionHook : IModHookV4
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
        /// Event fired whenever a player changes their in-game name.
        /// </summary>
        /// <param name="playerEntity">The player who has changed names.</param>
        /// <param name="oldNickname">The player's previous nickname.</param>
        public void OnPlayerRename(Entity playerEntity, string oldNickname);
    }
}
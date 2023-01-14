using System;
using Staxel.Commands;
using Staxel.Logic;
using Staxel.Modding;

namespace AetharNet.Moonbow.Experimental.Interfaces
{
    /// <summary>
    /// Mod hook with extended messaging functionality.
    /// Intercept/modify/act upon message events on the server.
    /// </summary>
    public interface IServerMessagingHook : IDisposable
    {
        /// <summary>
        /// Intercept or modify a command being received from a client.
        /// <example>
        /// Example usage to create a server-side command alias:
        /// <code>
        /// bool InterceptPlayerCommand(EntityId entityId, ref string command, ICommandsApi api)
        /// {
        ///     if (command == "/c") command = "/creative";
        ///     return true;
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="entityId">The EntityId of the player executing the command.</param>
        /// <param name="command">The command string received, including the command token. Can be modified.</param>
        /// <param name="api">The API object available in commands. Contains useful methods.</param>
        /// <returns>A boolean specifying if the command should be processed by the server or not. Return <c>true</c> to allow processing, <c>false</c> to ignore the command.</returns>
        bool InterceptPlayerCommand(EntityId entityId, ref string command, ICommandsApi api);
        /// <summary>
        /// Intercept or modify a chat message being received from a client.
        /// </summary>
        /// <param name="entityId">The EntityId of the player sending the message.</param>
        /// <param name="message">The chat message being received. Can be modified.</param>
        /// <param name="api">The API object available in commands. Contains useful methods.</param>
        /// <returns>A boolean specifying if the chat message should be sent to all players. Return <c>true</c> to send the message, <c>false</c> to block it.</returns>
        bool InterceptPlayerMessage(EntityId entityId, ref string message, ICommandsApi api);
        
        /// <summary>
        /// Act upon a command received from a client.
        /// </summary>
        /// <param name="entityId">The EntityId of the player executing the command.</param>
        /// <param name="command">The command string received, including the command token. Can be modified.</param>
        /// <param name="api">The API object available in commands. Contains useful methods.</param>
        void OnPlayerCommandReceived(EntityId entityId, string command, ICommandsApi api);
        /// <summary>
        /// Act upon a blocked chat message received from a muted client.
        /// <example>
        /// Example usage to display blocked messages to server admins:
        /// <code>
        /// void OnPlayerMessageBlocked(EntityId entityId, string message, ICommandsApi api) {
        ///     if (api.TryGetEntity(entityId, out Entity player)) {
        ///         string playerName = player.PlayerEntityLogic.DisplayName();
        ///         ServerMessaging.MessageAllOperatorsPlainText($"[MUTED] {playerName}: {message}");
        ///     }
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="entityId">The EntityId of the player who is muted.</param>
        /// <param name="message">The chat message that was blocked. Cannot be modified.</param>
        /// <param name="api">The API object available in commands. Contains useful methods.</param>
        void OnPlayerMessageBlocked(EntityId entityId, string message, ICommandsApi api);
        /// <summary>
        /// Act upon a chat message received from a client.
        /// <example>
        /// Example usage to smite all non-believers:
        /// <code>
        /// void OnPlayerMessageReceived(EntityId entityId, string message, ICommandsApi api)
        /// {
        ///     if (message == "kick me, you won't" && api.TryGetEntity(entityId, out Entity player))
        ///     {
        ///         api.KickPlayer(player.PlayerEntityLogic.Uid());
        ///     }
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="entityId">The EntityId of the player who sent the message.</param>
        /// <param name="message">The chat message that was received. Cannot be modified.</param>
        /// <param name="api">The API object available in commands. Contains useful methods.</param>
        void OnPlayerMessageReceived(EntityId entityId, string message, ICommandsApi api);
    }
}
using System;

namespace AetharNet.Moonbow.Experimental.Interfaces
{
    /// <summary>
    /// Mod hook with extended messaging functionality.
    /// Intercept/modify/act upon message events on the client.
    /// </summary>
    public interface IClientMessagingHook : IDisposable
    {
        /// <summary>
        /// Intercept or modify a command being sent by the client.
        /// <example>
        /// Example usage to create a client-side command alias:
        /// <code>
        /// bool InterceptOutgoingCommand(ref string command) {
        ///     if (command == "/c") command = "/creative";
        ///     return true;
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="command">The command string being sent to the server, including the command token. Can be modified.</param>
        /// <returns>A boolean specifying if the command should be sent to the server or not. Return <c>true</c> to send, <c>false</c> to block.</returns>
        bool InterceptOutgoingCommand(ref string command);
        /// <summary>
        /// Intercept or modify a chat message being sent by the client.
        /// <example>
        /// Example usage to add a message signature:
        /// <code>
        /// bool InterceptOutgoingMessage(ref string message) {
        ///     message = message + " ~uwu";
        ///     return true;
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="message">The chat message being sent to the server. Can be modified.</param>
        /// <returns>A boolean specifying if the chat message should be sent to the server or not. Return <c>true</c> to send, <c>false</c> to block.</returns>
        bool InterceptOutgoingMessage(ref string message);
        /// <summary>
        /// Intercept or modify a chat message being received from the server.
        /// <example>
        /// Example usage to ignore the farming fanatic:
        /// <code>
        /// bool InterceptIncomingMessage(string nick, ref string message) {
        ///     return nick != "Farm Fan";
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="nick">The message author's display name. Cannot currently be modified.</param>
        /// <param name="message">The chat message being received. Can be modified.</param>
        /// <returns>A boolean specifying if the chat message should be displayed on the client. Return <c>true</c> to display, <c>false</c> to block.</returns>
        bool InterceptIncomingMessage(string nick, ref string message);

        /// <summary>
        /// Act upon a command sent to the server.
        /// </summary>
        /// <param name="command">The command string that was sent to the server, including the command token. Cannot be modified.</param>
        void OnPlayerCommandSent(string command);
        /// <summary>
        /// Act upon a chat message sent to the server.
        /// </summary>
        /// <param name="message">The chat message that was sent to the server. Cannot be modified.</param>
        void OnPlayerMessageSent(string message);
        /// <summary>
        /// Act upon a chat message received from the server.
        /// </summary>
        /// <param name="nick">The message author's display name. Cannot be modified.</param>
        /// <param name="message">The chat message that was received. Cannot be modified.</param>
        void OnEntityMessageReceived(string nick, string message);
    }
}
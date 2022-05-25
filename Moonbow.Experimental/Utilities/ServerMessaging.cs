using System.Linq;
using Plukit.Base;
using Staxel;
using Staxel.Logic;

namespace AetharNet.Moonbow.Experimental.Utilities
{
    /// <summary>
    /// A messaging utility class, allowing modders to easily send messages server-side.
    /// </summary>
    public static class ServerMessaging
    {
        public static void MessagePlayer(Entity player, string translationKey, object[] translationParameters)
        {
            if (!GameUtilities.IsServer || player.PlayerEntityLogic == null) return;
            
            GameUtilities.ServerMainLoop.MessagePlayer(player.PlayerEntityLogic.Uid(), translationKey, translationParameters);
        }

        public static void MessagePlayerPlainText(Entity player, string message)
        {
            if (!GameUtilities.IsServer || player.PlayerEntityLogic == null) return;
            
            GameUtilities.ServerMainLoop.MessagePlayer(player.PlayerEntityLogic.Uid(), GameUtilities.EchoTranslationKey, new object[]{message});
        }
        
        /// <summary>
        /// Send a translation-based message to all players on the server.
        /// </summary>
        /// <param name="translationKey">Translation key for the message to send.</param>
        /// <param name="translationParameters">Parameters to format into the translation text.</param>
        public static void MessageAllPlayers(string translationKey, object[] translationParameters)
        {
            if (!GameUtilities.IsServer) return;
            
            var players = new Lyst<Entity>();
            GameUtilities.Universe.GetPlayers(players);

            foreach (var player in players)
            {
                GameUtilities.ServerMainLoop.MessagePlayer(player.PlayerEntityLogic.Uid(), translationKey, translationParameters);
            }
        }

        /// <summary>
        /// Send a text-based message to all players on the server.
        /// </summary>
        /// <param name="message">Plaintext message to send to all players.</param>
        public static void MessageAllPlayersPlainText(string message)
        {
            if (!GameUtilities.IsServer) return;
            
            var players = new Lyst<Entity>();
            GameUtilities.Universe.GetPlayers(players);

            foreach (var player in players)
            {
                GameUtilities.ServerMainLoop.MessagePlayer(player.PlayerEntityLogic.Uid(), GameUtilities.EchoTranslationKey, new object[]{message});
            }
        }

        /// <summary>
        /// Send a translation-based message to all server operators: players with admin permissions.
        /// </summary>
        /// <param name="translationKey">Translation key for the message to send.</param>
        /// <param name="translationParameters">Parameters to format into the translation text.</param>
        public static void MessageAllOperators(string translationKey, object[] translationParameters)
        {
            if (!GameUtilities.IsServer) return;
            
            var players = new Lyst<Entity>();
            GameUtilities.Universe.GetPlayers(players);
            var admins = players.Where(player => ServerContext.RightsManager.HasRight(player.PlayerEntityLogic.GetUsername(), player.PlayerEntityLogic.Uid(), "admin"));

            foreach (var admin in admins)
            {
                GameUtilities.ServerMainLoop.MessagePlayer(admin.PlayerEntityLogic.Uid(), translationKey, translationParameters);
            }
        }

        /// <summary>
        /// Send a text-based message to all server operators: players with admin permissions.
        /// </summary>
        /// <param name="message">Plaintext message to send to all operators.</param>
        public static void MessageAllOperatorsPlainText(string message)
        {
            if (!GameUtilities.IsServer) return;
            
            var players = new Lyst<Entity>();
            GameUtilities.Universe.GetPlayers(players);
            var admins = players.Where(player => ServerContext.RightsManager.HasRight(player.PlayerEntityLogic.GetUsername(), player.PlayerEntityLogic.Uid(), "admin"));

            foreach (var admin in admins)
            {
                GameUtilities.ServerMainLoop.MessagePlayer(admin.PlayerEntityLogic.Uid(), GameUtilities.EchoTranslationKey, new object[]{message});
            }
        }
    }
}
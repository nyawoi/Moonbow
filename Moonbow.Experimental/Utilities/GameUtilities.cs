using System.Diagnostics;
using AetharNet.Moonbow.Experimental.Patches;
using Staxel;
using Staxel.Client;
using Staxel.Logic;
using Staxel.Server;

namespace AetharNet.Moonbow.Experimental.Utilities
{
    /// <summary>
    /// A general utility class to facilitate accessing fields, as well as detecting the environment the mod is running on.
    /// </summary>
    public static class GameUtilities
    {
        /// <summary>
        /// Translation key for Moonbow's echo string, allowing for plaintext messages.
        /// </summary>
        public const string EchoTranslationKey = "AetharNet.Moonbow.MessagingComponent.echo";
        
        /// <summary>
        /// Boolean specifying whether or not the current process is a client.
        /// </summary>
        public static readonly bool IsClient = Process.GetCurrentProcess().ProcessName.Contains("Client");
        /// <summary>
        /// Boolean specifying whether or not the current process is a server.
        /// </summary>
        public static readonly bool IsServer = Process.GetCurrentProcess().ProcessName.Contains("Server");

        /// <summary>
        /// Property to retrieve the current Universe instance.
        /// </summary>
        public static Universe Universe
        {
            get
            {
                if (IsServer) return ServerContext.VillageDirector?.UniverseFacade?.AccessField<Universe>("_universe");
                return IsClient ? ClientMainLoop?.AccessField<UniverseManager>("_universeManager")?.AccessField<Universe>("_universe") : null;
            }
        }

        /// <summary>
        /// Property to retrieve the current ServerMainLoop instance.
        /// </summary>
        public static ServerMainLoop ServerMainLoop => ServerContext.VillageDirector?.UniverseFacade?.AccessField<ServerMainLoop>("_serverMainLoop");

        /// <summary>
        /// Property to retrieve the current ClientMainLoop instance.
        /// </summary>
        public static ClientMainLoop ClientMainLoop => ClientMainLoopPatch.ClientMainLoop;
    }
}
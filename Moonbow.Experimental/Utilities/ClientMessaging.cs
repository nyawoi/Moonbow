using Staxel;

namespace AetharNet.Moonbow.Experimental.Utilities
{
    /// <summary>
    /// A messaging utility class, allowing modders to display text client-side, as well as send messages on the client's behalf.
    /// </summary>
    public static class ClientMessaging
    {
        /// <summary>
        /// Display a text-based message in the client's chat box. This will not be sent to the server.
        /// </summary>
        /// <param name="message">Plaintext message to send to display.</param>
        public static void WriteText(string message)
        {
            if (!GameUtilities.IsClient) return;
            
            ClientContext.WebOverlayRenderer.Call("addServerMessage", new []{ message });
        }

        /// <summary>
        /// Display a translation-based message in the client's chat box. This will not be sent to the server.
        /// </summary>
        /// <param name="translationKey">Translation key for the message to display.</param>
        /// <param name="translationParameters">Parameters to format into the translation text.</param>
        public static void WriteTranslation(string translationKey, object[] translationParameters)
        {
            if (!GameUtilities.IsClient) return;
            
            var translationText = ClientContext.LanguageDatabase.GetTranslationString(translationKey);
            var formattedText = string.Format(translationText, translationParameters);
            
            ClientContext.WebOverlayRenderer.Call("addServerMessage", new []{ formattedText });
        }

        /// <summary>
        /// Send a text-based message to the server, to be shared with others.
        /// </summary>
        /// <param name="message">Plaintext message to send.</param>
        public static void SendText(string message)
        {
            if (!GameUtilities.IsClient) return;
            
            ClientContext.OverlayController.Chat.InvokeMethod("OnInput", message);
        }

        /// <summary>
        /// Send a translation-based message to the server, to be shared with others.
        /// </summary>
        /// <param name="translationKey">Translation key for the message to send.</param>
        /// <param name="translationParameters">Parameters to format into the translation text.</param>
        public static void SendTranslation(string translationKey, object[] translationParameters)
        {
            if (!GameUtilities.IsClient) return;

            var translationText = ClientContext.LanguageDatabase.GetTranslationString(translationKey);
            var formattedText = string.Format(translationText, translationParameters);
            
            ClientContext.OverlayController.Chat.InvokeMethod("OnInput", formattedText);
        }
    }
}
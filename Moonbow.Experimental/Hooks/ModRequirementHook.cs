using System;
using System.Collections.Generic;
using AetharNet.Moonbow.Experimental.Interfaces;
using AetharNet.Moonbow.Experimental.Templates;
using AetharNet.Moonbow.Experimental.Utilities;
using Staxel.Commands;
using Staxel.Core;
using Staxel.Logic;

namespace AetharNet.Moonbow.Experimental.Hooks
{
    internal class ModRequirementHook : ServerMessagingHookTemplate, IServerMessagingHook
    {
        internal const string Prefix = "MoonbowModRequirementRequest";
        private static readonly Dictionary<string, Action<bool>> Callbacks = new();

        public override bool InterceptPlayerMessage(EntityId entityId, ref string message, ICommandsApi api)
        {
            if (!message.StartsWith(Prefix + '|')) return true;
            
            var requestArgs = message.Split('|');

            if (requestArgs.Length != 3 || !bool.TryParse(requestArgs[2], out var result)) return true;

            var requestCode = requestArgs[1];

            if (!Callbacks.ContainsKey(requestCode)) return true;
            
            Callbacks[requestCode](result);
            Callbacks.Remove(requestCode);
            
            return false;
        }

        internal static void AddCallback(string requestCode, Action<bool> callback)
        {
            Callbacks.Add(requestCode, callback);
            Coroutine.AwaitTimedCondition(
                GameUtilities.Universe.Step + (Constants.TimestepsPerSecond * 3),
                () => !Callbacks.ContainsKey(requestCode),
                timeout =>
                {
                    if (!timeout) return;
                    callback(false);
                    Callbacks.Remove(requestCode);
                });
        }

        internal static void AddTimedCallback(long timeoutMs, string requestCode, Action<bool> callback)
        {
            Callbacks.Add(requestCode, callback);
            Coroutine.AwaitTimedCondition(
                GameUtilities.Universe.Step + (Constants.TimestepsPerMilliSecond * timeoutMs),
                () => !Callbacks.ContainsKey(requestCode),
                timeout =>
                {
                    if (!timeout) return;
                    callback(false);
                    Callbacks.Remove(requestCode);
                });
        }

        public override void Dispose()
        {
            Callbacks.Clear();
        }
    }
}
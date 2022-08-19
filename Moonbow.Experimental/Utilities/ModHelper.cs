using System;
using System.IO;
using System.Linq;
using AetharNet.Moonbow.Experimental.Effects;
using AetharNet.Moonbow.Experimental.Hooks;
using Plukit.Base;
using Staxel;
using Staxel.Effects;
using Staxel.Logic;

namespace AetharNet.Moonbow.Experimental.Utilities
{
    public static class ModHelper
    {
        private static readonly DirectoryInfo ModDirectory = new(Path.Combine(GameContext.ContentLoader.RootDirectory, "mods"));

        public static bool IsModInstalled(string modName)
        {
            return ModDirectory.GetDirectories().Any(dir => dir.Name == modName);
        }

        public static void RequestModCheck(Entity playerEntity, string modName, Action<bool> callback)
        {
            if (playerEntity.PlayerEntityLogic == null)
            {
                throw new Exception("Provided entity is not a player.");
            }

            var requestCode = Path.GetRandomFileName().Replace(".", "");
            var requestData = BlobAllocator.Blob(true);
            
            requestData.SetLong("entityId", playerEntity.Id.Id);
            requestData.SetString("modName", modName);
            requestData.SetString("requestCode", requestCode);
            
            ModRequirementHook.AddCallback(requestCode, callback);
            playerEntity.Effects.Trigger(new EffectTrigger(ModRequirementEffectBuilder.KindCode(), requestData));
        }

        public static void RequestModCheck(Entity playerEntity, string modName, long timeout, Action<bool> callback)
        {
            if (playerEntity.PlayerEntityLogic == null)
            {
                throw new Exception("Provided entity is not a player.");
            }

            var requestCode = Path.GetRandomFileName().Replace(".", "");
            var requestData = BlobAllocator.Blob(true);
            
            requestData.SetLong("entityId", playerEntity.Id.Id);
            requestData.SetString("modName", modName);
            requestData.SetString("requestCode", requestCode);
            
            ModRequirementHook.AddTimedCallback(timeout, requestCode, callback);
            playerEntity.Effects.Trigger(new EffectTrigger(ModRequirementEffectBuilder.KindCode(), requestData));
        }
    }
}
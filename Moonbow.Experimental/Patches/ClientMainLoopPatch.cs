using HarmonyLib;
using Staxel.Client;

namespace AetharNet.Moonbow.Experimental.Patches
{
    internal static class ClientMainLoopPatch
    {
        private const string HarmonyPatchId = "net.aethar.Moonbow.Patches.ClientMainLoopPatch";
        public static ClientMainLoop ClientMainLoop;

        private static void InitializePostfix(ClientMainLoop __instance) => ClientMainLoop = __instance;

        private static void DisposePostfix() => ClientMainLoop = null;

        public static void Initialize()
        {
            if (Harmony.HasAnyPatches(HarmonyPatchId)) return; 
            var harmonyInstance = new Harmony(HarmonyPatchId);

            var initializeBase = AccessTools.Method(typeof(ClientMainLoop), nameof(ClientMainLoop.Initialize));
            var initializePostfix = AccessTools.Method(typeof(ClientMainLoopPatch), nameof(InitializePostfix));

            var disposeBase = AccessTools.Method(typeof(ClientMainLoop), nameof(ClientMainLoop.Dispose));
            var disposePostfix = AccessTools.Method(typeof(ClientMainLoopPatch), nameof(DisposePostfix));

            harmonyInstance.Patch(initializeBase, postfix: new HarmonyMethod(initializePostfix));
            harmonyInstance.Patch(disposeBase, postfix: new HarmonyMethod(disposePostfix));
        }
    }
}
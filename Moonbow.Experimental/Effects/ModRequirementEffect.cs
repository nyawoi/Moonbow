using AetharNet.Moonbow.Experimental.Hooks;
using AetharNet.Moonbow.Experimental.Utilities;
using Plukit.Base;
using Staxel.Draw;
using Staxel.Effects;
using Staxel.Logic;
using Staxel.Rendering;

namespace AetharNet.Moonbow.Experimental.Effects
{
    internal class ModRequirementEffect : IEffect
    {
        private bool _completed;
        private string _modName;
        private string _requestCode;
        private readonly long _entityId;

        public ModRequirementEffect(Blob data)
        {
            _modName = data.GetString("modName");
            _requestCode = data.GetString("requestCode");
            _entityId = data.GetLong("entityId");
        }

        public void Dispose()
        {
            _modName = null;
            _requestCode = null;
        }

        public bool Completed() => _completed;

        public void Render(
            Entity entity,
            EntityPainter painter,
            Timestep renderTimestep,
            DeviceContext graphics,
            ref Matrix4F matrix,
            Vector3D renderOrigin,
            Vector3D position,
            RenderMode renderMode)
        {
            // Staxel will send the effect to all nearby players for rendering purposes.
            // Therefore, we must check that the client only responds to requests made to it.
            if (GameUtilities.ClientMainLoop?.Avatar()?.Id.Id == _entityId)
            {
                ClientMessaging.SendText($"{ModRequirementHook.Prefix}|{_requestCode}|{ModHelper.IsModInstalled(_modName)}");
            }
            _completed = true;
        }

        public void Stop() {}

        public void Pause() {}

        public void Resume() {}
    }
}
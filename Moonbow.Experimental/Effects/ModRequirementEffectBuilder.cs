using AetharNet.Moonbow.Experimental.Utilities;
using Plukit.Base;
using Staxel.Effects;
using Staxel.Logic;

namespace AetharNet.Moonbow.Experimental.Effects
{
    internal class ModRequirementEffectBuilder : IEffectBuilder
    {
        public static string KindCode() => "AetharNet.Moonbow.ModRequirementEffect";
        
        public void Dispose() {}

        public void Load() {}

        public string Kind() => KindCode();

        public IEffect Instance(
            Timestep step,
            Entity entity,
            EntityPainter painter,
            EntityUniverseFacade facade,
            Blob data,
            EffectDefinition definition,
            EffectMode mode)
        {
            return new ModRequirementEffect(data);
        }
    }
}
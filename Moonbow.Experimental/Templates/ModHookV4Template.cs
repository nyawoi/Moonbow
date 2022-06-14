using Plukit.Base;
using Staxel.Items;
using Staxel.Logic;
using Staxel.Tiles;

namespace AetharNet.Moonbow.Experimental.Templates
{
    /// <summary>
    /// A virtual template class to inherit from.
    /// Use the <c>override</c> keyword rather than <c>new</c>.
    /// </summary>
    public class ModHookV4Template
    {
        public virtual void Dispose() {}
        public virtual void CleanupOldSession() {}
        
        public virtual void UniverseUpdateBefore(Universe universe, Timestep step) {}
        public virtual void UniverseUpdateAfter() {}

        public virtual void GameContextInitializeInit() {}
        public virtual void GameContextInitializeBefore() {}
        public virtual void GameContextInitializeAfter() {}
        public virtual void GameContextDeinitialize() {}
        public virtual void GameContextReloadBefore() {}
        public virtual void GameContextReloadAfter() {}
        
        public virtual void ClientContextInitializeInit() {}
        public virtual void ClientContextInitializeBefore() {}
        public virtual void ClientContextInitializeAfter() {}
        public virtual void ClientContextDeinitialize() {}
        public virtual void ClientContextReloadBefore() {}
        public virtual void ClientContextReloadAfter() {}
        
        public virtual bool CanPlaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags) => true;
        public virtual bool CanReplaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags) => true;
        public virtual bool CanRemoveTile(Entity entity, Vector3I location, TileAccessFlags accessFlags) => true;
        
        public virtual bool CanInteractWithTile(Entity entity, Vector3F location, Tile tile) => true;
        public virtual bool CanInteractWithEntity(Entity entity, Entity lookingAtEntity) => true;
    }
}
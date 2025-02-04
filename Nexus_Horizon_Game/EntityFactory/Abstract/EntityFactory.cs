

namespace Nexus_Horizon_Game.EntityFactory
{
    /// <summary>
    /// Abstract Factory for entities
    /// </summary>
    internal abstract class EntityFactory
    {
        /// <summary>
        /// creates an entity that has similar components
        /// </summary>
        /// <returns> entity ID. </returns>
        public abstract int CreateEntity();

        /// <summary>
        /// Destroys an entity with specific needs.
        /// </summary>
        /// <param name="entity"> entity ID. </param>
        public abstract void DestroyEntity(int entity);
    }
}

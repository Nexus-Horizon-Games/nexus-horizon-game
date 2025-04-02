using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System.Linq;

namespace Nexus_Horizon_Game.Systems
{
    /// <summary>
    /// Handles entity health and death.
    /// </summary>
    internal static class HealthSystem
    {
        public static void Update(GameTime gameTime)
        {
            var entities = Scene.Loaded.ECS.GetEntitiesWithComponent<HealthComponent>();

            foreach (var entity in entities.ToList())
            {
                var component = Scene.Loaded.ECS.GetComponentFromEntity<HealthComponent>(entity);
                component.CheckForDeath(entity);
            }
        }
    }
}

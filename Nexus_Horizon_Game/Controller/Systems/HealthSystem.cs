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
            var components = Scene.Loaded.ECS.GetComponents<HealthComponent>();

            foreach (var component in components.ToList())
            {
                component.CheckForDeath();
            }
        }
    }
}

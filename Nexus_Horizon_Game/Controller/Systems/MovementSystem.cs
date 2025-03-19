using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

namespace Nexus_Horizon_Game.Controller.Systems
{
    internal static class MovementSystem
    {
        public static void Update(GameTime gameTime)
        {
            foreach (int entityID in Scene.Loaded.ECS.GetEntitiesWithComponent<MovementControllerComponent>())
            {
                Scene.Loaded.ECS.GetComponentFromEntity<MovementControllerComponent>(entityID).Controller.OnUpdate(gameTime, entityID);
            }
        }
    }
}


using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal static class ChefBossBehaviour
    {
        public static void OnUpdate(World world, int thisEntity, GameTime gameTime)
        {
            // do some stuff

            var transform = world.GetComponentFromEntity<TransformComponent>(thisEntity);
            transform.position.Y += (float)gameTime.ElapsedGameTime.TotalSeconds;
            world.SetComponentInEntity(thisEntity, transform);
        }
    }
}

using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System.Diagnostics;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal static class Bullet
    {
        public static void OnUpdate(World world, int thisEntity, GameTime gameTime)
        {
            var entityWithPhysics = world.GetEntitiesWithComponent<BulletComponent>();
            if (entityWithPhysics is not null)
            {
                foreach (var entity in entityWithPhysics)
                {
                    if (world.EntityHasComponent<TransformComponent>(entity))
                    {
                        DeleteOnOutOfBounds(world, entity);
                    }
                }
            }
        }

        private static void DeleteOnOutOfBounds(World world, int entity)
        {
            TransformComponent transform = world.GetComponentFromEntity<TransformComponent>(entity);

            if ((transform.position.X > Renderer.ScreenWidth || transform.position.X < 0) ||
                (transform.position.Y > Renderer.ScreenWidth || transform.position.Y < 0))
            {
                world.DestroyEntity(entity);
            }
        }
    }
}

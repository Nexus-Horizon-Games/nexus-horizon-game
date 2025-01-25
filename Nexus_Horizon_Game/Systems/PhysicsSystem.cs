using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game
{
    internal static class PhysicsSystem
    {
        public static void Update(World world, GameTime gameTime)
        {
            UpdatePositionFromVelocity(world, gameTime);
        }

        private static void UpdatePositionFromVelocity(World world, GameTime gameTime)
        {
            var entityWithPhysics = world.GetEntitiesWithComponent<PhysicsBody2DComponent>();
            if (entityWithPhysics is not null)
            {
                foreach (var entity in entityWithPhysics)
                {
                    if (world.EntityHasComponent<TransformComponent>(entity))
                    {
                        PhysicsBody2DComponent physicsBodyComponent = world.GetComponentFromEntity<PhysicsBody2DComponent>(entity);
                        TransformComponent transformComponent = world.GetComponentFromEntity<TransformComponent>(entity);

                        transformComponent.position = transformComponent.position +
                            new Vector2(physicsBodyComponent.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds, physicsBodyComponent.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);

                        world.SetComponentInEntity<TransformComponent>(entity, transformComponent);
                    }
                }
            }
        }
    }
}

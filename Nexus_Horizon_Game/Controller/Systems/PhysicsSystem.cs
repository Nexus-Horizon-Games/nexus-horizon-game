using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game
{
    internal static class PhysicsSystem
    {
        private const int unit = 10; // 10 pixels per unit

        public static void Update(GameTime gameTime)
        {
            UpdatePhysics(gameTime);
        }

        public static int Unit
        {
            get => unit;
        }

        private static void UpdatePhysics(GameTime gameTime)
        {
            var entityWithPhysics = Scene.Loaded.ECS.GetEntitiesWithComponent<PhysicsBody2DComponent>();
            if (entityWithPhysics is not null)
            {
                foreach (var entity in entityWithPhysics)
                {
                    if (Scene.Loaded.ECS.EntityHasComponent<TransformComponent>(entity))
                    {
                        PhysicsBody2DComponent physicsBodyComponent = Scene.Loaded.ECS.GetComponentFromEntity<PhysicsBody2DComponent>(entity);
                        TransformComponent transformComponent = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(entity);

                        // applies acceleration to velocity only if controlled by 
                        if (physicsBodyComponent.AccelerationEnabled)
                        {
                            physicsBodyComponent.Velocity = physicsBodyComponent.Velocity + (physicsBodyComponent.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds);
                        }

                        transformComponent.position = transformComponent.position + physicsBodyComponent.Velocity * unit * (float)gameTime.ElapsedGameTime.TotalSeconds;

                        Scene.Loaded.ECS.SetComponentInEntity<TransformComponent>(entity, transformComponent);
                        Scene.Loaded.ECS.SetComponentInEntity<PhysicsBody2DComponent>(entity, physicsBodyComponent);
                    }
                }
            }
        }
    }
}

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

        private static void UpdatePhysics(GameTime gameTime)
        {
            var entityWithPhysics = GameM.CurrentScene.World.GetEntitiesWithComponent<PhysicsBody2DComponent>();
            if (entityWithPhysics is not null)
            {
                foreach (var entity in entityWithPhysics)
                {
                    if (GameM.CurrentScene.World.EntityHasComponent<TransformComponent>(entity))
                    {
                        PhysicsBody2DComponent physicsBodyComponent = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(entity);
                        TransformComponent transformComponent = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(entity);

                        // applies acceleration to velocity only if controlled by 
                        if (physicsBodyComponent.AccelerationEnabled)
                        {
                            physicsBodyComponent.Velocity = physicsBodyComponent.Velocity + (physicsBodyComponent.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds);
                        }

                        transformComponent.position = transformComponent.position +
                            new Vector2(physicsBodyComponent.Velocity.X * unit * (float)gameTime.ElapsedGameTime.TotalSeconds, physicsBodyComponent.Velocity.Y * unit * (float)gameTime.ElapsedGameTime.TotalSeconds);

                        GameM.CurrentScene.World.SetComponentInEntity<TransformComponent>(entity, transformComponent);
                        GameM.CurrentScene.World.SetComponentInEntity<PhysicsBody2DComponent>(entity, physicsBodyComponent);
                    }
                }
            }
        }
    }
}

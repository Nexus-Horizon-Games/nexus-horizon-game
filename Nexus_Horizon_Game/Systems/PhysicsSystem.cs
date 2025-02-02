using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game
{
    internal static class PhysicsSystem
    {
        private const int unit = 10; // 10 pixels per unit

        public static void Update(GameTime gameTime)
        {
            UpdatePositionFromVelocity(gameTime);
        }

        private static void UpdatePositionFromVelocity(GameTime gameTime)
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

                        transformComponent.position = transformComponent.position +
                            new Vector2(physicsBodyComponent.Velocity.X * unit * (float)gameTime.ElapsedGameTime.TotalSeconds, physicsBodyComponent.Velocity.Y * unit * (float)gameTime.ElapsedGameTime.TotalSeconds);

                        GameM.CurrentScene.World.SetComponentInEntity<TransformComponent>(entity, transformComponent);
                    }
                }
            }
        }
    }
}

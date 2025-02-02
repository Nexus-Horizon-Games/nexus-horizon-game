using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal static class Bullet
    {

        public static void OnUpdate(int thisEntity, GameTime gameTime)
        {
            var entityWithPhysics = GameM.CurrentScene.World.GetEntitiesWithComponent<BulletComponent>();
            if (entityWithPhysics is not null)
            {
                foreach (var entity in entityWithPhysics)
                {
                    if (GameM.CurrentScene.World.EntityHasComponent<TransformComponent>(entity))
                    {
                        DeleteOnOutOfBounds(entity);
                    }
                }
            }
        }

        private static void DeleteOnOutOfBounds(int entity)
        {
            TransformComponent transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(entity);

            if ((transform.position.X > Renderer.ScreenWidth || transform.position.X < 0) ||
                (transform.position.Y > Renderer.ScreenWidth || transform.position.Y < 0))
            {
                GameM.CurrentScene.World.DestroyEntity(entity);
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nexus_Horizon_Game
{
    internal class Scene
    {
        private World world = new World();

        public void Update(GameTime gameTime)
        {
            PhysicsSystem.Update(world, gameTime);

            /*
            var entitiesWithBullet = world.GetEntitiesWithComponent<BulletComponent>();
            foreach (var entity in entitiesWithBullet)
            {
                if (world.EntityHasComponent<PhysicsBody2DComponent>(entity))
                {
                    PhysicsBody2DComponent physicsBodyComponent = world.GetComponentFromEntity<PhysicsBody2DComponent>(entity);
                    physicsBodyComponent = Bullet.Update(gameTime, physicsBodyComponent);
                    Debug.WriteLine($"Velocity: {physicsBodyComponent.Velocity}");
                    world.SetComponentInEntity<PhysicsBody2DComponent>(entity, physicsBodyComponent);
                }
            }
            */
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D spriteTexture) // spriteBatch and spriteTexture being passed here should only be temporary
        {
            RenderSystem.Draw(world, gameTime, spriteBatch, spriteTexture);
        }

        public World World { get { return world; } }
    }
}

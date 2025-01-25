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

        public void Draw(GameTime gameTime)
        {
            RenderSystem.Draw(world, gameTime);
            // Get entities with component
            // get component enumeration then through each component do the draw for it.
        }

        public World World { get { return world; } }
    }
}

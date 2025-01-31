using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.EntityFactory
{
    internal class BulletFactory : EntityFactory
    {
        private World world;
        private string textureName;
        public BulletFactory(World world, string textureName)
        {
            this.world = world;
            this.textureName = textureName;
        }

        /// <summary>
        /// Creates bullets
        /// </summary>
        /// <returns> entity ID. </returns>
        public override int CreateEntity()
        {
            return world.CreateEntity(new List<IComponent>
            { new TransformComponent(new Vector2(0.0f, 0.0f)),
                new SpriteComponent(textureName, color: Color.White, scale: 0.25f, spriteLayer: 0),
                new PhysicsBody2DComponent(),
              new BulletComponent() });
        }

        /// <summary>
        /// creates a moving bullet at a specific point and direction
        /// </summary>
        /// <param name="SpawnPoint"> where the bullet will spawn. </param>
        /// <param name="direction"> the direction the bullet will move (Will be normalized). </param>
        /// <param name="velocity"> the speed the bullet will move in that direction. </param>
        /// <returns> entity id. </returns>
        public int CreateEntity(Vector2 SpawnPoint, Vector2 direction, float velocity)
        {
            direction.Normalize();

            return world.CreateEntity(new List<IComponent>
            { new TransformComponent(SpawnPoint),
                new SpriteComponent(textureName, color: Color.White, scale: 0.25f, spriteLayer: 0),
                new PhysicsBody2DComponent()
                {
                    Velocity = new Vector2(velocity * direction.X, velocity * direction.Y),
                },
              new BulletComponent() });
        }
    }
}

using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System.Collections.Generic;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using System.Diagnostics;
//using Nexus_Horizon_Game.Behaviours;

namespace Nexus_Horizon_Game.EntityFactory
{
    internal class BulletFactory : EntityFactory
    {
        private string textureName;

        public BulletFactory(string textureName)
        {
            this.textureName = textureName;
        }

        /// <summary>
        /// Creates bullets
        /// </summary>
        /// <returns> entity ID. </returns>
        public override int CreateEntity()
        {
            int entityID = Scene.Loaded.ECS.CreateEntity(new List<IComponent>
            { new TransformComponent(new Vector2(0.0f, 0.0f)),
                new SpriteComponent(textureName, color: Color.White, scale: 0.25f, spriteLayer: 0),
                new PhysicsBody2DComponent(),
                new TagComponent(Tag.PLAYER_PROJECTILE)
            });

            Scene.Loaded.ECS.AddComponent<ColliderComponent>(entityID, new ColliderComponent(entityID, new Point(8, 8)));

            return entityID;
        }

        /// <summary>
        /// creates a moving bullet at a specific point and direction
        /// </summary>
        /// <param name="SpawnPoint"> where the bullet will spawn. </param>
        /// <param name="direction"> the direction the bullet will move (Will be normalized). </param>
        /// <param name="velocity"> the speed the bullet will move in that direction. </param>
        /// <returns> entity id. </returns>
        public int CreateEntity(Vector2 SpawnPoint, Vector2 direction, float velocity, Bullet.BulletAction bulletAction = null, float scale = 0.25f, uint spriteLayer = 0, bool isPlayerBullet = true)
        {
            direction.Normalize();

            int originalWidth = 32;
            int originalHeight = 32;

            int scaledWidth = (int)(originalWidth * scale);
            int scaledHeight = (int)(originalHeight * scale);

            // Sets tag for either player bullet or enemy bullet. 
            Tag bulletTag = isPlayerBullet ? Tag.PLAYER_PROJECTILE : Tag.ENEMY_PROJECTILE;

            // Debug.WriteLine(bulletTag);

            int entity = Scene.Loaded.ECS.CreateEntity(new List<IComponent>
            { new TransformComponent(SpawnPoint),
              new SpriteComponent(textureName, color: Color.White, scale: scale, spriteLayer: spriteLayer, centered: true),
              new PhysicsBody2DComponent()
              {
                  Velocity = new Vector2(velocity * direction.X, velocity * direction.Y),
              },
                       
                new TagComponent(bulletTag)
      
            });

            Scene.Loaded.ECS.AddComponent<ColliderComponent>(entity, new ColliderComponent(entity, new Point(scaledWidth, scaledHeight), new Point(-scaledWidth / 2, -scaledHeight / 2)));

            // bullet logic behavior attached to ECS
            Scene.Loaded.ECS.AddComponent(entity, new BehaviourComponent(new Bullet(entity, bulletAction)));
            // Attach collision behavior to handle collisions (destroy bullet on enemy impact).
            //Scene.Loaded.ECS.AddComponent(entity, new BehaviourComponent(new CollisionBehavior(entity)));
            return entity;
        }

        public override void DestroyEntity(int entity)
        {
            Scene.Loaded.ECS.DestroyEntity(entity);
        }
    }
}

using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.Prefab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Model.EntityPatterns
{
    internal abstract class AbstractFiringPattern
    {
        protected Vector2 GetPlayerPosition()
        {
            var entitesWithTag = Scene.Loaded.ECS.GetEntitiesWithComponent<TagComponent>();
            var playerEntity = -1;
            foreach (var entity in entitesWithTag)
            {
                var tag = Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(entity);
                if (tag.Tag == Tag.PLAYER)
                {
                    playerEntity = entity;
                    break;
                }
            }

            Vector2 playerPosition = Vector2.Zero;
            if (playerEntity != -1)
            {
                playerPosition = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(playerEntity).position;
            }
            // Debug.WriteLine("player position is " + playerPosition);
            return playerPosition;
        }

        protected Vector2 GetVectFromDirection(double direction, double variation)
        {
            direction += variation;
            float xComponent = (float)(Math.Cos(direction));
            float yComponent = (float)(Math.Sin(direction));
            return new Vector2(xComponent, yComponent);
        }

        /// <summary>
        /// returns the entityID of the created entity for extra editing
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="fireDirection"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        protected virtual int SpawnEntity(Vector2 position, Vector2 fireDirection, float velocity, PrefabEntity prefab)
        {
            List<IComponent> components = prefab.getComponents();
            SpriteComponent spriteComp;

            try
            { 
                spriteComp = (SpriteComponent)components.FirstOrDefault(x => x.GetType() == typeof(SpriteComponent));
            }
            catch (Exception ex) 
            {
                throw new Exception("Bullet must contain a SpriteComponent!!!!!");
            }

            int originalWidth = 16;
            int originalHeight = 16;

            int scaledWidth = (int)(originalWidth * spriteComp.Scale);
            int scaledHeight = (int)(originalHeight * spriteComp.Scale);

            components.RemoveAll(x => x.GetType() == typeof(TransformComponent));
            components.Add(new PhysicsBody2DComponent()
            {
                Velocity = new Vector2(velocity * fireDirection.X, velocity * fireDirection.Y)
            });
            components.Add(new TransformComponent()
            {
                position = position
            });
            int firedEntity = Scene.Loaded.ECS.CreateEntity(components);
            Scene.Loaded.ECS.AddComponent<ColliderComponent>(firedEntity, new ColliderComponent(new Point(scaledWidth, scaledHeight), new Point(-scaledWidth / 2, -scaledHeight / 2), entityIDFollowing: firedEntity));
            Scene.Loaded.ECS.AddComponent<BehaviourComponent>(firedEntity, new BehaviourComponent(new Bullet(firedEntity)));
            return firedEntity;
        }
    }
}

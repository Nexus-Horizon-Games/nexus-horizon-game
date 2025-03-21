using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Controller
{
    /// <summary>
    /// We Can use Spatial Partitioning if the n^2 time complexity starts lagging otherwise this'll work fine
    /// </summary>
    internal static class CollisionSystem
    {
        public static void Update(GameTime gametime)
        {
            // get entities with colliders
            List<int> colliderEntityIDList = Scene.Loaded.ECS.GetEntitiesWithComponent<ColliderComponent>();

            foreach (int bulletID in colliderEntityIDList) 
            {
                // gets TagComponent to check if this is a bullet
                TagComponent bulletTagComponent = Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(bulletID);

                // checks bullet on bullet collisions
                if (bulletTagComponent.Tag != Tag.PLAYER_PROJECTILE && bulletTagComponent.Tag != Tag.ENEMY_PROJECTILE)
                    continue;
              
                // gets collider and transform components for the bullet
                ColliderComponent collider1 = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(bulletID);
                TransformComponent transform1 = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(bulletID);              

                Rectangle worldBounds1 = new Rectangle(
                    collider1.Bounds.X + (int)transform1.position.X,
                    collider1.Bounds.Y + (int)transform1.position.Y,
                    collider1.Bounds.Width,
                    collider1.Bounds.Height);

                // determines the tag target
                // if player bullet, target an enemy; if enemy bullet, target a player
                Tag allowedTarget = (bulletTagComponent.Tag == Tag.PLAYER_PROJECTILE) ? Tag.ENEMY : Tag.PLAYER;

                // iterate over all other colliders
                foreach (int otherID in colliderEntityIDList)
                {
                    // skip
                    if (otherID == bulletID)
                        continue;

                    TagComponent tag2 = Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(otherID);
                    if (tag2.Tag != allowedTarget)
                        continue;
                    // gets collider and transform for target             
                    ColliderComponent collider2 = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(otherID);
                    TransformComponent transform2 = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(otherID);

                    Rectangle worldBounds2 = new Rectangle(
                        collider2.Bounds.X + (int)transform2.position.X,
                        collider2.Bounds.Y + (int)transform2.position.Y,
                        collider2.Bounds.Width,
                        collider2.Bounds.Height);

                    if (worldBounds1.Intersects(worldBounds2))
                    {
                        collider1.SendOnCollisionInfo(otherID);
                    }
                }
            }
        }
    }
}



  
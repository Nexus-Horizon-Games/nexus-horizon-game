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

            for (int i = 0; i < colliderEntityIDList.Count; i++) 
            {
                int entityID1 = colliderEntityIDList[i];

                ColliderComponent collider1 = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(entityID1);
                TransformComponent transform1 = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(entityID1);              

                Rectangle worldBounds1 = new Rectangle(
                    collider1.Bounds.X + (int)transform1.position.X,
                    collider1.Bounds.Y + (int)transform1.position.Y,
                    collider1.Bounds.Width,
                    collider1.Bounds.Height);


                for (int j = i + 1; j < colliderEntityIDList.Count; j++)
                {
                    int entityID2 = colliderEntityIDList[j];
                    ColliderComponent collider2 = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(entityID2);
                    TransformComponent transform2 = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(entityID2);

                    Rectangle worldBounds2 = new Rectangle(
                        collider2.Bounds.X + (int)transform2.position.X,
                        collider2.Bounds.Y + (int)transform2.position.Y,
                        collider2.Bounds.Width,
                        collider2.Bounds.Height);


                    if (worldBounds1.Intersects(worldBounds2))
                    {
                        //Debug.WriteLine($"Collision detected: Entity {entityID1} intersects with Entity {entityID2}");
                        collider1.SendOnCollisionInfo(entityID2);
                        collider2.SendOnCollisionInfo(entityID1);
                    }
                }
            }
        }
    }
}



  
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nexus_Horizon_Game.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        //stores updated subsrciptions by mapping entity ID
        private static Dictionary<int, ColliderComponent> colliderMap = new Dictionary<int, ColliderComponent>();

        // called on GameM when scene is loaded 
        public static void Init()
        {
            Scene.Loaded.ECS.OnAddComponentEvent += OnAddComponent;
        }

        // check this part if lag still happens becasue the loop?
        private static void OnAddComponent(int entity, Type componentType)
        {
            // when a collider is added it subscribes to OnCollisionEvent here
            if (componentType == typeof(ColliderComponent))
            {
                var collider = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(entity);

                // subscribe to OnCollisionEvent
                collider.OnCollision += (otherEntityID) => CollisionHandler(entity, otherEntityID);

                // keeps track of subscribtions/collisions
                colliderMap[entity] = collider;

                // updates ECS
                Scene.Loaded.ECS.SetComponentInEntity(entity, collider);
            }
        }

        private static void CollisionHandler(int bulletEntity, int otherEntityID)
        {
            if (!Scene.Loaded.ECS.IsEntityAlive(bulletEntity)) return;
            if (!Scene.Loaded.ECS.IsEntityAlive(otherEntityID)) return;

            TagComponent bulletTag = Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(bulletEntity);
            TagComponent otherTag = Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(otherEntityID);

            // if player bullet hits enemy, bullet gets destoryed.
            if ((bulletTag.Tag & Tag.PLAYER_PROJECTILE) == Tag.PLAYER_PROJECTILE && (otherTag.Tag & Tag.ENEMY) == Tag.ENEMY)
            {
                // health / collision component implemented for enemies 
                if (Scene.Loaded.ECS.EntityHasComponent<HealthComponent>(otherEntityID, out HealthComponent enemyHealth))
                {
                    // sets how much health is subtracted per bullet collision
                    enemyHealth.health -= 0.5f; // subtracts 1 health point
                    Scene.Loaded.ECS.SetComponentInEntity(otherEntityID, enemyHealth);
                }
                Scene.Loaded.ECS.DestroyEntity(bulletEntity);
            }
            // if enemy bullet hits player, destroy the bullet
            else if ((bulletTag.Tag & Tag.ENEMY_PROJECTILE) == Tag.ENEMY_PROJECTILE && (otherTag.Tag & Tag.PLAYER) == Tag.PLAYER)
            {
                Scene.Loaded.ECS.DestroyEntity(bulletEntity);
            }
        }
        public static void Update(GameTime gametime)
        {
            // gets every entity with ColliderComponent

            List<int> allColliderIDs = Scene.Loaded.ECS.GetEntitiesWithComponent<ColliderComponent>();

            // separates their IDs from their tags
            // player bullet
            List<int> playerBulletIDs = allColliderIDs.Where(id => (Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(id).Tag & Tag.PLAYER_PROJECTILE) == Tag.PLAYER_PROJECTILE).ToList();

            // enemy bullet
            List<int> enemyBulletIDs = allColliderIDs.Where(id => (Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(id).Tag & Tag.ENEMY_PROJECTILE) == Tag.ENEMY_PROJECTILE).ToList();

            // playerID
            List<int> playerIDs = allColliderIDs.Where(id => (Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(id).Tag & Tag.PLAYER) == Tag.PLAYER).ToList();

            // enemyIDs
            IEnumerable<int> enemyIDs = allColliderIDs.Where(id => (Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(id).Tag & Tag.ENEMY) == Tag.ENEMY);

            // checks player bullets against enemies
            foreach (int bulletID in playerBulletIDs)
            {
                ColliderComponent bulletCollider = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(bulletID);

                foreach (int enemyID in enemyIDs)
                {
                    
                    if (!Scene.Loaded.ECS.IsEntityAlive(bulletID)) // if bullet has been destroyed no need to keep checking if it hit something (Plus it creates errors with new bound checking)
                    {
                        break;
                    }


                    if (!Scene.Loaded.ECS.IsEntityAlive(enemyID)) // if enemy is already dead continue onto the next enemy no need to check (Plus it creates errors with new bound checking)
                    {
                        continue;
                    }
                    

                    ColliderComponent enemyCollider = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(enemyID);

                    if (bulletCollider.Bounds.Intersects(enemyCollider.Bounds))
                    {
                        //Debug.WriteLine($"[CollisionSystem] Player bullet {bulletID} hit enemy {enemyID}");
                        bulletCollider.SendOnCollisionInfo(enemyID);
                    }
                }
            }
            
            // checks enemy bullets against player
            foreach (int bulletID in enemyBulletIDs)
            {
                ColliderComponent bulletCollider = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(bulletID);

                foreach (int playerID in playerIDs)
                {
                    if (!Scene.Loaded.ECS.IsEntityAlive(bulletID)) // if bullet has been destroyed no need to keep checking if it hit something (Plus it creates errors with new bound checking)
                    {
                        break;
                    }


                    if (!Scene.Loaded.ECS.IsEntityAlive(playerID)) // if player is already dead continue onto the next player no need to check (Plus it creates errors with new bound checking)
                    {
                        continue;
                    }

                    ColliderComponent playerCollider = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(playerID);

                    if (bulletCollider.Bounds.Intersects(playerCollider.Bounds))
                    {
                        //Debug.WriteLine($"[CollisionSystem] Enemy bullet {bulletID} hit player {playerID}");
                        bulletCollider.SendOnCollisionInfo(playerID);
                    }
                }
            }
            
        }
    }
}




  
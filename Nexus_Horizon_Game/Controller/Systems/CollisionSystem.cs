using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model.GameManagers;
using System;
using System.Collections.Generic;

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
                collider.Initalize(entity);

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
                    enemyHealth.health -= (0.5f * GameplayManager.Instance.PowerMultiplier()); // subtracts 1 health point
                    Scene.Loaded.ECS.SetComponentInEntity(otherEntityID, enemyHealth);
                }
                GameplayManager.Instance.DealtDamage();
                Scene.Loaded.ECS.DestroyEntity(bulletEntity);
            }
            // if enemy bullet hits player, destroy the bullet
            else if ((bulletTag.Tag & Tag.ENEMY_PROJECTILE) == Tag.ENEMY_PROJECTILE && (otherTag.Tag & Tag.PLAYER) == Tag.PLAYER)
            {
                Scene.Loaded.ECS.DestroyEntity(bulletEntity);
            }
        }

        /// <summary>
        /// THIS WAY OF CHECKING FOR COLLISIONS IS JUST TO SLOW SO IT LAGS REALLY BAD
        /// </summary>
        /// <param name="gametime"></param>
        public static void Update(GameTime gametime)
        {
            // gets every entity with ColliderComponent

            List<int> allColliderIDs = Scene.Loaded.ECS.GetEntitiesWithComponent<ColliderComponent>();

            List<int> playerBulletIDs = new();
            List<int> checkPlayerColliderIDs = new();
            List<int> playerIDs = new();
            List<int> enemyIDs = new();


            // separates their IDs from their tags
            foreach (int enityID in allColliderIDs)
            {
                Tag entityTag = Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(enityID).Tag;

                if ((entityTag & Tag.PLAYER_PROJECTILE) == Tag.PLAYER_PROJECTILE)            // player bullet 
                {
                    playerBulletIDs.Add(enityID);
                }
                else if((entityTag & Tag.ENEMY_PROJECTILE) == Tag.ENEMY_PROJECTILE || (entityTag & Tag.POWERDROP) == Tag.POWERDROP || (entityTag & Tag.POINTDROP) == Tag.POINTDROP)            // enemy bullet
                {
                    checkPlayerColliderIDs.Add(enityID);
                }
                else if((entityTag & Tag.PLAYER) == Tag.PLAYER)            // playerID
                {
                    playerIDs.Add(enityID);
                }
                else if((entityTag & Tag.ENEMY) == Tag.ENEMY)            // enemyIDs
                {
                    enemyIDs.Add(enityID);
                }
            }

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
                    else
                    {
                        if (Scene.Loaded.ECS.EntityHasComponent<TagComponent>(bulletID, out TagComponent tagComp))
                        {
                            if ((tagComp.Tag & Tag.POWERDROP) == Tag.POWERDROP || (tagComp.Tag & Tag.POINTDROP) == Tag.POINTDROP)
                            {
                                DeleteOnOutOfBounds(bulletID);
                            }
                        }
                    }
                }
            }
            
            // checks enemy bullets against player
            foreach (int playerID in playerIDs)
            {
                ColliderComponent playerCollider = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(playerID);

                foreach (int checkPlayerID in checkPlayerColliderIDs)
                {
                    if (!Scene.Loaded.ECS.IsEntityAlive(checkPlayerID)) // if bullet has been destroyed no need to keep checking if it hit something (Plus it creates errors with new bound checking)
                    {
                        continue;
                    }

                    if (!Scene.Loaded.ECS.IsEntityAlive(playerID)) // if player is already dead continue onto the next player no need to check (Plus it creates errors with new bound checking)
                    {
                        break;
                    }

                    ColliderComponent checkPlayerCollider = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(checkPlayerID);

                    if (checkPlayerCollider.Bounds.Intersects(playerCollider.Bounds))
                    {
                        //Debug.WriteLine($"[CollisionSystem] Enemy bullet {bulletID} hit player {playerID}");
                        playerCollider.SendOnCollisionInfo(checkPlayerID);
                        checkPlayerCollider.SendOnCollisionInfo(playerID);

                        if (Scene.Loaded.ECS.EntityHasComponent<TagComponent>(checkPlayerID, out TagComponent tagComp))
                        { 
                            if ((tagComp.Tag & Tag.ENEMY_PROJECTILE) == Tag.ENEMY_PROJECTILE)
                            {
                                break; // im using this instead of the below findiing entity alive cause it propogates a ton of bullet collisions and for some reason playerID is still alive ?
                            }
                        }
                    }
                    else
                    {
                        if (Scene.Loaded.ECS.EntityHasComponent<TagComponent>(checkPlayerID, out TagComponent tagComp))
                        { 
                            if((tagComp.Tag & Tag.POWERDROP) == Tag.POWERDROP || (tagComp.Tag & Tag.POINTDROP) == Tag.POINTDROP)
                            { 
                                DeleteOnOutOfBounds(checkPlayerID);
                            }
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// deletes the entity of a bullet when out of the radius of the play area
        /// </summary>
        /// <param name="entity"></param>
        private static void DeleteOnOutOfBounds(int entity)
        {
            if (Scene.Loaded.ECS.EntityHasComponent<TransformComponent>(entity, out TransformComponent transformComp))
            {
                Vector2 arenaDirection = Arena.CheckEntityInArena(transformComp, out Vector2 boundaryIn, 2f, 2f);

                if (arenaDirection.X != 0 || arenaDirection.Y != 0)
                {
                    Scene.Loaded.ECS.DestroyEntity(entity);
                }
            }
            else
            {
                throw new Exception("Must Have Transform To Delete Out Of Bounds");
            }
        }
    }
}




  
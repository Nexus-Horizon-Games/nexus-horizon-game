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
                collider.OnCollisionEvent += (otherEntityID) => CollisionHandler(entity, otherEntityID);

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
            if (bulletTag.Tag == Tag.PLAYER_PROJECTILE && otherTag.Tag == Tag.ENEMY)
            {
                Scene.Loaded.ECS.DestroyEntity(bulletEntity);
            }
            // if enemy bullet hits player, destroy the bullet
            else if (bulletTag.Tag == Tag.ENEMY_PROJECTILE && otherTag.Tag == Tag.PLAYER)
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
            List<int> playerBulletIDs = allColliderIDs.Where(id => Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(id).Tag == Tag.PLAYER_PROJECTILE).ToList();

            // enemy bullet
            List<int> enemyBulletIDs = allColliderIDs.Where(id => Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(id).Tag == Tag.ENEMY_PROJECTILE).ToList();

            // playerID
            List<int> playerIDs = allColliderIDs.Where(id => Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(id).Tag == Tag.PLAYER).ToList();

            // enemyIDs
            List<int> enemyIDs = allColliderIDs.Where(id => Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(id).Tag == Tag.ENEMY).ToList();

            // checks player bullets against enemies
            foreach (int bulletID in playerBulletIDs)
            {
                ColliderComponent bulletCollider = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(bulletID);
                TransformComponent bulletTransform = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(bulletID);
                Rectangle bulletWorldBounds = new Rectangle(
                    bulletCollider.Bounds.X + (int)bulletTransform.position.X,
                    bulletCollider.Bounds.Y + (int)bulletTransform.position.Y,
                    bulletCollider.Bounds.Width,
                    bulletCollider.Bounds.Height);

                foreach (int enemyID in enemyIDs)
                {
                    ColliderComponent enemyCollider = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(enemyID);
                    TransformComponent enemyTransform = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(enemyID);
                    Rectangle enemyWorldBounds = new Rectangle(
                        enemyCollider.Bounds.X + (int)enemyTransform.position.X,
                        enemyCollider.Bounds.Y + (int)enemyTransform.position.Y,
                        enemyCollider.Bounds.Width,
                        enemyCollider.Bounds.Height);

                    if (bulletWorldBounds.Intersects(enemyWorldBounds))
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
                TransformComponent bulletTransform = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(bulletID);
                Rectangle bulletWorldBounds = new Rectangle(
                    bulletCollider.Bounds.X + (int)bulletTransform.position.X,
                    bulletCollider.Bounds.Y + (int)bulletTransform.position.Y,
                    bulletCollider.Bounds.Width,
                    bulletCollider.Bounds.Height);

                foreach (int playerID in playerIDs)
                {
                    ColliderComponent playerCollider = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(playerID);
                    TransformComponent playerTransform = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(playerID);
                    Rectangle playerWorldBounds = new Rectangle(
                        playerCollider.Bounds.X + (int)playerTransform.position.X,
                        playerCollider.Bounds.Y + (int)playerTransform.position.Y,
                        playerCollider.Bounds.Width,
                        playerCollider.Bounds.Height);

                    if (bulletWorldBounds.Intersects(playerWorldBounds))
                    {
                        Debug.WriteLine($"[CollisionSystem] Enemy bullet {bulletID} hit player {playerID}");
                        bulletCollider.SendOnCollisionInfo(playerID);
                    }
                }
            }
        }
            }
        }




  
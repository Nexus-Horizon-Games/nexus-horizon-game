using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using System.Diagnostics;

namespace Nexus_Horizon_Game.Pooling
{
    internal class BulletPool
    {
        private List<int> availableBullets;
        private BulletFactory bulletFactory;
        private int poolSize;

        // one instance of the bullet pool when the game runs
        private static BulletPool instance;
        // lets other code access the instance
        public static BulletPool Instance => instance;

        public BulletPool(BulletFactory factory, int startingPoolSize = 200)
        {
            bulletFactory = factory;
            poolSize = startingPoolSize;
            availableBullets = new List<int>();
            instance = this;

            // creates bullets offscreen for use.
            for (int i = 0; i < poolSize; i++)
            {
                // bullets for player
                int bullet = bulletFactory.CreateEntity(new Vector2(-100, -100), Vector2.Zero, 0f, null, 0.25f, 0, true);
                availableBullets.Add(bullet);
            }
        }
        
        // gets bullet from the pool and resets it.
        public int GetBullet(Vector2 position, Vector2 direction, float velocity, bool isPlayerBullet = true)
        {
            int bullet;
            /*if (availableBullets.Count > 0)
            {
                bullet = availableBullets[0];
                availableBullets.RemoveAt(0);

                // resets PhysicsBody2D and Transform Components
                Scene.Loaded.ECS.SetComponentInEntity(bullet, new TransformComponent(position));
                PhysicsBody2DComponent body = Scene.Loaded.ECS.GetComponentFromEntity<PhysicsBody2DComponent>(bullet);
                body.Velocity = new Vector2(velocity * direction.X, velocity * direction.Y);
                Scene.Loaded.ECS.SetComponentInEntity(bullet, body);

                // update TagComponent
                TagComponent tag = Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(bullet);
                tag.Tag = isPlayerBullet ? Tag.PLAYER_PROJECTILE : Tag.ENEMY_PROJECTILE;
                Scene.Loaded.ECS.SetComponentInEntity(bullet, tag);
            }
            else
            {
                // creates a new bullet if the pool is empty
            }*/

            bullet = bulletFactory.CreateEntity(position, direction, velocity, null, 0.25f, 0, isPlayerBullet);
            return bullet;
        }
        
        // returns bullet to pool
        public void BulletReturn(int bullet)
        {
            // resets bullet so it can be returned to the pool
            //Scene.Loaded.ECS.SetComponentInEntity(bullet, new TransformComponent(new Vector2(-100, -100)));
            Scene.Loaded.ECS.DestroyEntity(bullet);
            //availableBullets.Add(bullet);
        }
    }
}
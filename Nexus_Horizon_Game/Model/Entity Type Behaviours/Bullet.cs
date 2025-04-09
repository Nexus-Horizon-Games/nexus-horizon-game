using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System;
using System.Diagnostics;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal class Bullet : Behaviour
    {
        public delegate Vector2 BulletAction(GameTime gametime, Bullet bullet, int bulletEntity, Vector2 previousVelocity);

        private BulletAction bulletAction;
        private double timeAlive = 0.0f;

        /// <summary>
        /// Initialies the bullet.
        /// </summary>
        /// <param name="thisEntity"> the entity this behavior is attached to. </param>
        /// <param name="bulletBehavior"> possible bullet action that changes the bullet. </param>
        public Bullet(int thisEntity, BulletAction bulletBehavior = null) : base(thisEntity)
        {
            this.bulletAction = bulletBehavior;
        }

        public double TimeAlive
        {
            get => timeAlive;
        }

        /// <summary>
        /// updates the bullet
        /// </summary>
        /// <param name="gameTime"></param>
        public override void OnUpdate(GameTime gameTime)
        {
            timeAlive += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Scene.Loaded.ECS.EntityHasComponent<PhysicsBody2DComponent>(this.Entity, out PhysicsBody2DComponent physicsComponent) &&
                Scene.Loaded.ECS.EntityHasComponent<TransformComponent>(this.Entity, out TransformComponent transformComponent))
            {
                DeleteOnOutOfBounds(transformComponent);

                // calls bulletAction if it has been set from the constructor
                if (this.bulletAction != null)
                {
                    physicsComponent.Velocity = this.bulletAction(gameTime, this, this.Entity, physicsComponent.Velocity);
                    Scene.Loaded.ECS.SetComponentInEntity<PhysicsBody2DComponent>(this.Entity, physicsComponent);
                }
            }
            else
            {
                throw new Exception("~Could Not Retrieve A Component From Bullet~");
            }
        
        }

        /// <summary>
        /// sets the bullet to move in the opposite direction
        /// </summary>
        public void ReverseBulletDirection()
        {
            Vector2 currentVelocity = Scene.Loaded.ECS.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity).Velocity;
            Scene.Loaded.ECS.SetComponentInEntity<PhysicsBody2DComponent>(this.Entity, new PhysicsBody2DComponent { Velocity = -currentVelocity });
        }

        /// <summary>
        /// deletes the entity of a bullet when out of the radius of the play area
        /// </summary>
        /// <param name="entity"></param>
        private void DeleteOnOutOfBounds(TransformComponent transform)
        {
            Vector2 arenaDirection = Arena.CheckEntityInArena(transform, out Vector2 boundaryIn, 2f, 2f);

            if (arenaDirection.X != 0 || arenaDirection.Y != 0)
            {
                Scene.Loaded.ECS.DestroyEntity(this.Entity);
            }
        }
    }
}

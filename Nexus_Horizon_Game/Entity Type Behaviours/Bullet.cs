using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal class Bullet : Behaviour
    {
        public delegate Vector2 BulletAction(GameTime gametime, Bullet bullet, Vector2 previousVelocity);

        private BulletAction bulletAction;
        private float timeAlive = 0.0f;

        /// <summary>
        /// Initialies the bullet.
        /// </summary>
        /// <param name="thisEntity"> the entity this behavior is attached to. </param>
        /// <param name="bulletBehavior"> possible bullet action that changes the bullet. </param>
        public Bullet(int thisEntity, BulletAction bulletBehavior = null) : base(thisEntity)
        {
            this.bulletAction = bulletBehavior;
        }

        public float TimeAlive
        {
            get => timeAlive;
        }

        /// <summary>
        /// updates the bullet
        /// </summary>
        /// <param name="gameTime"></param>
        public override void OnUpdate(GameTime gameTime)
        {
            DeleteOnOutOfBounds(this.Entity);

            // calls bulletAction if it has been set from the constructor
            if (this.bulletAction != null)
            {
                if (GameM.CurrentScene.World.EntityHasComponent<PhysicsBody2DComponent>(this.Entity, out PhysicsBody2DComponent bulletPhysics))
                {
                    bulletPhysics.Velocity = this.bulletAction(gameTime, this, bulletPhysics.Velocity);
                    GameM.CurrentScene.World.SetComponentInEntity<PhysicsBody2DComponent>(this.Entity, bulletPhysics);
                }
            }
        }

        /// <summary>
        /// sets the bullet to move in the opposite direction
        /// </summary>
        public void ReverseBulletDirection()
        {
            Vector2 currentVelocity = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity).Velocity;
            GameM.CurrentScene.World.SetComponentInEntity<PhysicsBody2DComponent>(this.Entity, new PhysicsBody2DComponent { Velocity = -currentVelocity });
        }

        /// <summary>
        /// deletes the entity of a bullet when out of the radius of the play area
        /// </summary>
        /// <param name="entity"></param>
        private void DeleteOnOutOfBounds(int entity)
        {
            TransformComponent transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(entity);

            if ((transform.position.X > GameM.CurrentScene.ArenaRight || transform.position.X < GameM.CurrentScene.ArenaLeft) ||
                (transform.position.Y > GameM.CurrentScene.ArenaBottom || transform.position.Y < GameM.CurrentScene.ArenaTop))
            {
                GameM.CurrentScene.World.DestroyEntity(entity);
            }
        }
    }
}

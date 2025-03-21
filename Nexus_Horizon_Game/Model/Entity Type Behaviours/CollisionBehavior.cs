using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using System;
using System.Diagnostics;

namespace Nexus_Horizon_Game.Behaviours
{
    internal class CollisionBehavior : Behaviour
    {
        private bool hasBeenDestroyed = false;
        public CollisionBehavior(int entity) : base(entity)
        {
            // Subscribe to the collision event from this entity's ColliderComponent.
            ColliderComponent collider = Scene.Loaded.ECS.GetComponentFromEntity<ColliderComponent>(entity);
            collider.OnCollisionEvent += OnCollisionHandler;
            // updates collider into the ECS
            Scene.Loaded.ECS.SetComponentInEntity(entity, collider);
            //Debug.WriteLine($"CollisionBehavior attached to bullet entity {this.Entity}");
        }

        private void OnCollisionHandler(int otherEntityID)
        {
            if (hasBeenDestroyed)
                return;

            // Retrieve the TagComponent of the colliding entity.
            TagComponent tagComponent1 = Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(this.Entity);
            TagComponent tagComponent2 = Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(otherEntityID);
            //Debug.WriteLine($"Bullet {this.Entity} collided with entity {otherEntityID} (tag: {tagComponent.Tag})");

            // Check the tag and provide appropriate feedback.
            if (tagComponent1.Tag == Tag.PLAYER_PROJECTILE && tagComponent2.Tag ==Tag.ENEMY)
            {
                //Debug.WriteLine($"Player bullet {this.Entity} will be destroyed when it hits enemy {otherEntityID}.");
                hasBeenDestroyed = true;
                // returns bullets to the bullet pool
                Pooling.BulletPool.Instance.BulletReturn(this.Entity);
            }
            else if (tagComponent1.Tag == Tag.ENEMY_PROJECTILE && tagComponent2.Tag == Tag.PLAYER)
            {
                // if enemey projectile hits player it is destoryed.
                hasBeenDestroyed = true;
                // returns bullets to the bullet pool
                Pooling.BulletPool.Instance.BulletReturn(this.Entity);
            }
        }

        public override void OnUpdate(GameTime gameTime)
        {
            // Optionally update ongoing collision feedback effects here.
        }
    }
}
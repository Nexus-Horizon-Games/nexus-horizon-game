using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Threading;

namespace Nexus_Horizon_Game.Components
{
    public delegate void OnCollision(int entityID);

    internal struct ColliderComponent : IComponent
    {
        private bool isEmpty;

        private int entityIDFollowing = -1;
        private Rectangle rect; 

        public event OnCollision OnCollision; // This is where we can add listeners for actions to happen when a collision happens to a collider.
                                               // EXAMPLE: Lets say the player when touching another entityID,
                                               // it contains a specific tag by looking it up in its own function that is a listener to this event it can they do any logic it wants to itself
                                               // this requires these behaviours subscribing to onCollsion to carefully unsubscribe when they are no longer
                                               // alive.

        public ColliderComponent(Point size, Point? position = null)
        {
            this.isEmpty = false;
            OnCollision = null;
            
            Point setPosition = new Point(0, 0);
            if (position != null) 
            { 
                setPosition = (Point)position; 
            } 
            this.rect = new Rectangle(setPosition, size);
        }

        /// <summary>
        /// Initalizes this component, should be called before any calculations are done on it.
        /// </summary>
        /// <param name="entity">The entity that this collider should follow.</param>
        public void Initalize(int entity)
        {
            entityIDFollowing = entity;
        }

        public Rectangle Bounds 
        { 
            get 
            { 
                if (Scene.Loaded.ECS.EntityHasComponent<TransformComponent>(this.entityIDFollowing, out TransformComponent transform))
                {
                    rect.X = (int)transform.position.X;
                    rect.Y = (int)transform.position.Y;

                    return rect;
                }

                throw new System.MemberAccessException($"The Entity {this.entityIDFollowing} must contain a transform when using collisions");
            } 
        }

        /// <summary>
        /// this could be another entity its following or itself if the collider is on itself C:
        /// </summary>
        /// <param name="entityID"></param>
        public void SetEntityIDFollowingTo(int entityID)
        {
            this.entityIDFollowing = entityID;
        }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is ColliderComponent o)
            {
                if (rect == o.rect && entityIDFollowing == o.entityIDFollowing)
                {
                    return rect == o.rect && entityIDFollowing == o.entityIDFollowing;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsEmptyComponent()
        {
            return isEmpty;
        }

        /// <inheritdoc/>
        public static IComponent MakeEmptyComponent()
        {
            ColliderComponent collider = new ColliderComponent(new Point(0, 0));
            collider.isEmpty = true;
            return collider;
        }

        public void SendOnCollisionInfo(int entityIDCollided)
        {
            OnCollision?.Invoke(entityIDCollided); // invoke for every entityID that is currently colliding with another collider, ? so no risk of null exception
        }
    }
}

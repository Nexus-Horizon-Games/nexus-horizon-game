using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.Components
{
    public delegate void OnCollision(int entityID);

    internal struct ColliderComponent : IComponent
    {
        private bool isEmpty;

        private Rectangle bounds; 
        private TransformComponent transform; // needs to follow this transform where the sprite/original entity is.

        private event OnCollision onCollision; // This is where we can add listeners for actions to happen when a collision happens to a collider.
                                               // EXAMPLE: Lets say the player when touching another entityID,
                                               // it contains a specific tag by looking it up in its own function that is a listener to this event it can they do any logic it wants to itself
                                               // this requires these behaviours subscribing to onCollsion to carefully unsubscribe when they are no longer
                                               // alive.

        public ColliderComponent(Rectangle bounds)
        {
            this.bounds = bounds;
        }

        public Rectangle Bounds { get; }

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
                if (bounds == o.bounds)
                {
                    return true;
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
            ColliderComponent collider = new ColliderComponent(new Rectangle(new Point(0, 0), new Point(0, 0)));
            collider.isEmpty = true;
            return collider;
        }

        public void SendOnCollisionInfo(List<int> entityIDList)
        {
            foreach (int entity in entityIDList)
            {
                onCollision.Invoke(entity); // invoke for every entityID that is currently colliding with another collider
            }
        }
    }
}

using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Components
{
    internal struct PhysicsBody2DComponent : IComponent
    {
        private bool isEmpty;

        private Vector2 velocity;
        private Vector2 acceleration; // Not Supported Yet

        // private float gravityScale; // Not Supported Yet
        // private Vector2 gravityDirection; // Not Supported Yet

        public PhysicsBody2DComponent()
        {
            this.isEmpty = false;
            this.velocity = Vector2.Zero;
            this.acceleration = Vector2.Zero;
            // this.gravityScale = 0.0f;
            // this.gravityDirection = Vector2.Zero;
        }

        # region Properties

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public Vector2 Acceleration
        {
            get => acceleration;
            set => acceleration = value;
        }

        #endregion

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is PhysicsBody2DComponent o)
            {
                if ((object)this == (object)other)
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
            PhysicsBody2DComponent physicsBodyComponent = new PhysicsBody2DComponent();
            physicsBodyComponent.isEmpty = true;
            return physicsBodyComponent;
        }
    }
}

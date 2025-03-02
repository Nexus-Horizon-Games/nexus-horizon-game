using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Components
{
    internal struct PhysicsBody2DComponent : IComponent
    {
        private bool isEmpty;

        private Vector2 velocity;
        private Vector2 acceleration; // Not Supported Yet
        private bool accelerationEnabled; // allows acceleration to be set but not tracked
        private float mass;

        public PhysicsBody2DComponent(float mass = 1f, bool accelerationEnabled = false)
        {
            this.isEmpty = false;
            this.velocity = Vector2.Zero;
            this.acceleration = Vector2.Zero;
            this.accelerationEnabled = accelerationEnabled;
        }

        # region Properties

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        /// <summary>
        /// Gets and sets the velocity of the component (in units per second).
        /// </summary>
        public Vector2 Velocity
        {
            get => velocity;
            set
            {
                // sets the acceleration from change of velocity.
                if (!this.accelerationEnabled)
                {
                    float accelerationX = velocity.X - value.X;
                    float accelerationY = velocity.Y - value.Y;

                    acceleration = new Vector2(accelerationX, accelerationY);
                }

                velocity = value;
            }
        }

        /// <summary>
        /// Gets or sets the current acceleration of the object dependant on AccelerationEnabled.
        /// For more information <see cref="AccelerationEnabled"/>
        /// </summary>
        public Vector2 Acceleration
        {
            get => acceleration;
            set
            {
                // only sets acceleration if acceleration is enabled
                if (accelerationEnabled)
                {
                    acceleration = value;
                    return;
                }

                throw new System.AccessViolationException("Acceleration is not enabled.");
            }
        }

        /// <summary>
        /// Enabled: Allows Modification of acceleration but requires user to update and track the value indefinitely outside of the component.
        /// Disabled: Acceleration is calculated from the difference between the current velocity and the new velocity.
        /// </summary>
        public bool AccelerationEnabled
        {
            get => accelerationEnabled;
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

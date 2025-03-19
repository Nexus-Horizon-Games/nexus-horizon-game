using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Model
{
    internal class Movement
    {
        private float defaultSpeed;
        private float currentSpeed;

        public Movement(float speed)
        {
            this.defaultSpeed = speed;
            this.currentSpeed = speed;
        }

        public float CurrentSpeed
        {
            get => currentSpeed;
            set => currentSpeed = value;
        }

        public void OnUpdate(GameTime gameTime, int entityID)
        {
            // keeps the speed at the default speed unless changed in controller.
            currentSpeed = defaultSpeed;
        }

        // returns the direction speed vector of the movement.
        public Vector2 Move(Vector2 direction)
        {
            if (direction.X == 0 && direction.Y == 0)
            {
                return direction;
            }

            direction.Normalize();
            return direction * this.currentSpeed;
        }
    }
}

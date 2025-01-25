namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal static class Bullet
    {
        private static bool isWDown = false;
        private static bool isSDown = false;
        private static bool isADown = false;
        private static bool isDDown = false;

        /*
        public static PhysicsBody2DComponent Update(GameTime gameTime, PhysicsBody2DComponent physicsBodyComponent)
        {
            float speedAmount = 1000f;

            float xSpeed = 0;
            float ySpeed = 0;

            if (!isWDown && Keyboard.GetState().IsKeyDown(Keys.W)) { isWDown = true; ySpeed -= speedAmount; }

            if (isWDown && Keyboard.GetState().IsKeyUp(Keys.W)) { isWDown = false; ySpeed += speedAmount; }

            if (!isSDown && Keyboard.GetState().IsKeyDown(Keys.S)) { isSDown = true; ySpeed += speedAmount; }
            if (isSDown && Keyboard.GetState().IsKeyUp(Keys.S)) { isSDown = false; ySpeed -= speedAmount; }

            if (!isDDown && Keyboard.GetState().IsKeyDown(Keys.D)) { isDDown = true; xSpeed += speedAmount; }
            if (isDDown && Keyboard.GetState().IsKeyUp(Keys.D)) { isDDown = false; xSpeed -= speedAmount; }

            if (!isADown && Keyboard.GetState().IsKeyDown(Keys.A)) { isADown = true; xSpeed -= speedAmount; }
            if (isADown && Keyboard.GetState().IsKeyUp(Keys.A)) { isADown = false; xSpeed += speedAmount; }

            physicsBodyComponent.Velocity = new Vector2(physicsBodyComponent.Velocity.X + xSpeed, physicsBodyComponent.Velocity.Y + ySpeed);
            return physicsBodyComponent;
        }
        */
    }
}

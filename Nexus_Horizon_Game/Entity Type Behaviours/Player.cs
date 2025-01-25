using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal static class Player
    {
        private static int currentEntityID = -1;

        private static bool isWDown = false;
        private static bool isSDown = false;
        private static bool isADown = false;
        private static bool isDDown = false;

        static Player() { }

        /// <summary>
        /// Gets and sets the currentEntityID of a newly created player.
        /// </summary>
        public static int CurrentEntityID
        {
            get => currentEntityID;
            set => currentEntityID = value; 
        }

        /// <summary>
        /// updates the player only if it exists
        /// </summary>
        /// <param name="world"> world data of scene </param>
        /// <param name="gameTime"> the gametime of the running program. </param>
        public static void Update(World world, GameTime gameTime)
        {
            if (!world.IsEntityAlive(currentEntityID)) { return; }

            if (world.EntityHasComponent<PhysicsBody2DComponent>(currentEntityID))
            {
                PhysicsBody2DComponent physicsBodyComponent = world.GetComponentFromEntity<PhysicsBody2DComponent>(currentEntityID);
                world.SetComponentInEntity<PhysicsBody2DComponent>(currentEntityID, Player.Movement(physicsBodyComponent));
            }
        }

        /// <summary>
        /// moves player up, down, left, right using WASD
        /// </summary>
        /// <param name="physicsBodyComponent"> current physicsBody Component. </param>
        /// <returns> result of movement physicsBodyComponent. </returns>
        private static PhysicsBody2DComponent Movement(PhysicsBody2DComponent physicsBodyComponent)
        {
            float speedAmount = 100f;

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
    }
}

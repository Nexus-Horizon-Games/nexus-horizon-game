using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal static class Player
    {
        private static int currentEntityID = -1;

        private static bool isUpArrowDown = false;
        private static bool isDownArrowDown = false;
        private static bool isLeftArrowDown = false;
        private static bool isRightArrowDown = false;

        private static float splitTime = 5f; // the time between shots
        private static float currentTime = 0f; // the time between shots

        private static bool isZDown = false;

        private static BulletFactory bulletFactoryType;

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
        public static void Update(GameTime gameTime)
        {
            if (!GameM.CurrentScene.World.IsEntityAlive(currentEntityID)) { return; }

            if (GameM.CurrentScene.World.EntityHasComponent<PhysicsBody2DComponent>(currentEntityID))
            {
                // movement
                PhysicsBody2DComponent physicsBodyComponent = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(currentEntityID);
                GameM.CurrentScene.World.SetComponentInEntity<PhysicsBody2DComponent>(currentEntityID, Player.Movement(physicsBodyComponent));

                //projectiles
                ShootProjectile();
            }
        }

        /// <summary>
        /// moves player up, down, left, right using WASD
        /// </summary>
        /// <param name="physicsBodyComponent"> current physicsBody Component. </param>
        /// <returns> result of movement physicsBodyComponent. </returns>
        private static PhysicsBody2DComponent Movement(PhysicsBody2DComponent physicsBodyComponent)
        {
            float speedAmount = 8f;

            float xSpeed = 0;
            float ySpeed = 0;

            if (!isUpArrowDown && Keyboard.GetState().IsKeyDown(Keys.Up)) { isUpArrowDown = true; ySpeed -= speedAmount; }

            if (isUpArrowDown && Keyboard.GetState().IsKeyUp(Keys.Up)) { isUpArrowDown = false; ySpeed += speedAmount; }

            if (!isDownArrowDown && Keyboard.GetState().IsKeyDown(Keys.Down)) { isDownArrowDown = true; ySpeed += speedAmount; }
            if (isDownArrowDown && Keyboard.GetState().IsKeyUp(Keys.Down)) { isDownArrowDown = false; ySpeed -= speedAmount; }

            if (!isRightArrowDown && Keyboard.GetState().IsKeyDown(Keys.Right)) { isRightArrowDown = true; xSpeed += speedAmount; }
            if (isRightArrowDown && Keyboard.GetState().IsKeyUp(Keys.Right)) { isRightArrowDown = false; xSpeed -= speedAmount; }

            if (!isLeftArrowDown && Keyboard.GetState().IsKeyDown(Keys.Left)) { isLeftArrowDown = true; xSpeed -= speedAmount; }
            if (isLeftArrowDown && Keyboard.GetState().IsKeyUp(Keys.Left)) { isLeftArrowDown = false; xSpeed += speedAmount; }

            physicsBodyComponent.Velocity = new Vector2(physicsBodyComponent.Velocity.X + xSpeed, physicsBodyComponent.Velocity.Y + ySpeed);
            return physicsBodyComponent;
        }

        private static void ShootProjectile()
        {
            if (bulletFactoryType == null) { bulletFactoryType = new BulletFactory("BulletSample"); }

            if (!isZDown && Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                int bullet3 = bulletFactoryType.CreateEntity(GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(currentEntityID).position, new Vector2(0, -1), 100f);
                isZDown = true;
            }

            if (isZDown && Keyboard.GetState().IsKeyUp(Keys.Z))
            {
                isZDown = false;
            }
        }

    }
}

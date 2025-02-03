using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using System;
using Nexus_Horizon_Game.Systems;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Threading;
using System.Runtime.InteropServices.Marshalling;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal static class Player
    {
        // movement
        private const float baseSpeed = 12f;
        private static float slowMultiplier = 1f;
        private static float xSpeed = 0f;
        private static float ySpeed = 0f;

        // bullets
        private static float xBulletOffset = 4f;
        private static float yBulletOffset = -2f;
        private static float bulletSpeed = 50f;

        static Player() 
        {
            AddListeners();
        }

        /// <summary>
        /// updates the player only if it exists
        /// </summary>
        /// <param name="world"> world data of scene </param>
        /// <param name="gameTime"> the gametime of the running program. </param>
        public static void Update(GameTime gameTime)
        {
            foreach (int playerEntity in GameM.CurrentScene.World.GetEntitiesWithComponent<PlayerComponent>())
            { 
                if (!GameM.CurrentScene.World.IsEntityAlive(playerEntity)) { continue; }

                if (GameM.CurrentScene.World.EntityHasComponent<PhysicsBody2DComponent>(playerEntity))
                {
                    // movement
                    PhysicsBody2DComponent physicsBodyComponent = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(playerEntity);
                    GameM.CurrentScene.World.SetComponentInEntity<PhysicsBody2DComponent>(playerEntity, Player.Movement(physicsBodyComponent));

                    // updates the position of the visual
                    UpdateCollisionVisualPosition(playerEntity);

                    //projectiles
                    ContinuousProjectiles(playerEntity);
                }
            }
        }

        private static void AddListeners()
        {
            // add movement
            InputSystem.AddOnKeyDownListener(Keys.Right, (key) => {  xSpeed += baseSpeed; });
            InputSystem.AddOnKeyDownListener(Keys.Left, (key) => { xSpeed -= baseSpeed; });
            InputSystem.AddOnKeyDownListener(Keys.Up, (key) => { ySpeed -= baseSpeed; });
            InputSystem.AddOnKeyDownListener(Keys.Down, (key) => { ySpeed += baseSpeed; });
            
            // add slow
            InputSystem.AddOnKeyDownListener(Keys.LeftShift, TurnOnSlowAbility);

            // remove movement
            InputSystem.AddOnKeyUpListener(Keys.Right, (key) => { xSpeed -= baseSpeed; });
            InputSystem.AddOnKeyUpListener(Keys.Left, (key) => { xSpeed += baseSpeed; });
            InputSystem.AddOnKeyUpListener(Keys.Up, (key) => { ySpeed += baseSpeed; });
            InputSystem.AddOnKeyUpListener(Keys.Down, (key) => { ySpeed -= baseSpeed; });

            // remove slow
            InputSystem.AddOnKeyUpListener(Keys.LeftShift, TurnOffSlowAbility);
            // InputSystem.AddOnKeyUpListener(Keys.Z, OnStopShooting);
        }

        private static void UpdateCollisionVisualPosition(int playerEntity)
        {
            PlayerComponent playerComponent = GameM.CurrentScene.World.GetComponentFromEntity<PlayerComponent>(playerEntity);
            TransformComponent playerTransform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(playerEntity);

            GameM.CurrentScene.World.SetComponentInEntity<TransformComponent>(playerComponent.HitboxVisualEntityID, playerTransform);
        }

        /// <summary>
        /// moves player up, down, left, right using WASD
        /// </summary>
        /// <param name="physicsBodyComponent"> current physicsBody Component. </param>
        /// <returns> result of movement physicsBodyComponent. </returns>
        private static PhysicsBody2DComponent Movement(PhysicsBody2DComponent physicsBodyComponent)
        {
            if (MathF.Abs(xSpeed) == MathF.Abs(ySpeed))
            {
                physicsBodyComponent.Velocity = new Vector2(xSpeed * MathF.Cos(float.Pi/4) * slowMultiplier, ySpeed * MathF.Sin(float.Pi / 4) * slowMultiplier);
            }
            else
            {
                physicsBodyComponent.Velocity = new Vector2(xSpeed * slowMultiplier, ySpeed * slowMultiplier);
            }

            Debug.WriteLine(physicsBodyComponent.Velocity);

            return physicsBodyComponent;
        }

        private static void ContinuousProjectiles(int playerEntity)
        {
            BulletFactory bulletFactory = GameM.CurrentScene.World.GetComponentFromEntity<PlayerComponent>(playerEntity).BulletFactory;
            Vector2 playerPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(playerEntity).position;

            Vector2 shotDirection = new Vector2(0, -1);
            Vector2 leftBulletPosition = new Vector2(playerPosition.X - xBulletOffset, playerPosition.Y + yBulletOffset);
            Vector2 rightBulletPosition = new Vector2(playerPosition.X + xBulletOffset, playerPosition.Y + yBulletOffset);

            if(InputSystem.IsKeyDown(Keys.Z))
            {
                bulletFactory.CreateEntity(leftBulletPosition, shotDirection, bulletSpeed, scale: 0.3f);
                bulletFactory.CreateEntity(rightBulletPosition, shotDirection, bulletSpeed, scale: 0.3f);
            }
        }

        private static void TurnOnSlowAbility(Keys key)
        {
            slowMultiplier = 0.5f;
            foreach (int playerEntity in GameM.CurrentScene.World.GetEntitiesWithComponent<PlayerComponent>())
            {
                PlayerComponent playerComponent = GameM.CurrentScene.World.GetComponentFromEntity<PlayerComponent>(playerEntity);
                SpriteComponent collisionVisualSpriteComponent = GameM.CurrentScene.World.GetComponentFromEntity<SpriteComponent>(playerComponent.HitboxVisualEntityID);
                collisionVisualSpriteComponent.isVisible = true;
                GameM.CurrentScene.World.SetComponentInEntity<SpriteComponent>(playerComponent.HitboxVisualEntityID, collisionVisualSpriteComponent);
            }
        }

        private static void TurnOffSlowAbility(Keys key)
        {
            slowMultiplier = 1f;
            foreach (int playerEntity in GameM.CurrentScene.World.GetEntitiesWithComponent<PlayerComponent>())
            {
                PlayerComponent playerComponent = GameM.CurrentScene.World.GetComponentFromEntity<PlayerComponent>(playerEntity);
                SpriteComponent collisionVisualSpriteComponent = GameM.CurrentScene.World.GetComponentFromEntity<SpriteComponent>(playerComponent.HitboxVisualEntityID);
                collisionVisualSpriteComponent.isVisible = false;
                GameM.CurrentScene.World.SetComponentInEntity<SpriteComponent>(playerComponent.HitboxVisualEntityID, collisionVisualSpriteComponent);
            }
        }

        /*
         * 
            private static int intervalShots = 250; // 1000 mil to 1 sec
            private static int shotCounts = 8;
        private static void EndingShot(int playerEntity)
        {

            for (int i = 0; i < shotCounts; i++)
            {
                Vector2 playerPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(playerEntity).position;
                Vector2 shotDirection = new Vector2(0, -1);
                Vector2 leftBulletPosition = new Vector2(playerPosition.X - xBulletOffset, playerPosition.Y + yBulletOffset);
                Vector2 rightBulletPosition = new Vector2(playerPosition.X + xBulletOffset, playerPosition.Y + yBulletOffset);

                BulletFactory bulletFactory = GameM.CurrentScene.World.GetComponentFromEntity<PlayerComponent>(playerEntity).BulletFactory;

                bulletFactory.CreateEntity(leftBulletPosition, shotDirection, bulletSpeed, scale: 0.3f);
                bulletFactory.CreateEntity(rightBulletPosition, shotDirection, bulletSpeed, scale: 0.3f);

                Thread.Sleep(intervalShots);
            }
        }
        */
    }
}

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using System;
using Nexus_Horizon_Game.Systems;
using System.Collections.Generic;
using Nexus_Horizon_Game.Timers;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal class Player : Behaviour, IDisposable
    {
        // For Disposing
        private bool _disposed = false; // whenever the player is disposed this will be true.
        private List<(Keys ,Action<Keys>)> keyDownListeners = new List<(Keys, Action<Keys>)>();
        private List<(Keys, Action<Keys>)> keyUpListeners = new List<(Keys, Action<Keys>)>();

        // movement
        private const float baseSpeed = 13f;
        private const float slowMultiplier = 0.40f;
        private float activeMultiplier = 1f;
        private float xSpeed = 0f;
        private float ySpeed = 0f;

        // bullets
        private float xBulletOffset = 4f;
        private float yBulletOffset = -2f;
        private float bulletSpeed = 50f;
        private static BulletFactory hamsterBallBullets = new BulletFactory("BulletSample");
        private Timer bulletTimerConstant;
        private Timer bulletTimerEndShots;
        private const float bulletTimeInterval = 0.05f;
        // collision
        private int hitboxEntityID;

        public Player(int playerEntity, int hitboxEntity) : base(playerEntity)
        {
            this.hitboxEntityID = hitboxEntity;
            this.bulletTimerConstant = new LoopTimer(bulletTimeInterval, this.ShotConstant);
            this.bulletTimerEndShots = new LoopTimer(bulletTimeInterval, this.ShotConstant, stopAfter: 0.2f);
            AddListeners();
        }
        
        /// <summary>
        /// entity id of player collider.
        /// </summary>
        public int HitBoxEntityID
        {
            get => HitBoxEntityID;
        }


        /// <summary>
        /// updates the player only if it exists
        /// </summary>
        /// <param name="gameTime"> the gametime of the running program. </param>
        public override void OnUpdate(GameTime gameTime)
        {
            if (GameM.CurrentScene.World.EntityHasComponent<PhysicsBody2DComponent>(this.Entity))
            {
                // movement
                PhysicsBody2DComponent physicsBodyComponent = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
                GameM.CurrentScene.World.SetComponentInEntity<PhysicsBody2DComponent>(this.Entity, this.Movement(physicsBodyComponent));

                // updates the position of the visual
                UpdateCollisionVisualPosition();

                //projectiles
                ContinuousProjectiles();

                this.bulletTimerConstant.Update(gameTime);
                this.bulletTimerEndShots.Update(gameTime);
            }
        }

        /// <summary>
        /// Updates the viusal reperesentation position onto the player
        /// </summary>
        /// <param name="playerEntity"></param>
        private void UpdateCollisionVisualPosition()
        {
            TransformComponent playerTransform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);

            GameM.CurrentScene.World.SetComponentInEntity<TransformComponent>(this.hitboxEntityID, playerTransform);
        }

        /// <summary>
        /// moves player up, down, left, right using WASD
        /// </summary>
        /// <param name="physicsBodyComponent"> current physicsBody Component. </param>
        /// <returns> result of movement physicsBodyComponent. </returns>
        private PhysicsBody2DComponent Movement(PhysicsBody2DComponent physicsBodyComponent)
        {
            if (MathF.Abs(xSpeed) == MathF.Abs(ySpeed)) 
            {
                // fixes diagional movement speed
                physicsBodyComponent.Velocity = new Vector2(xSpeed * MathF.Cos(float.Pi/4) * activeMultiplier, ySpeed * MathF.Sin(float.Pi / 4) * activeMultiplier);
            }
            else
            {
                physicsBodyComponent.Velocity = new Vector2(xSpeed * activeMultiplier, ySpeed * activeMultiplier);
            }

            return physicsBodyComponent;
        }

        /// <summary>
        /// shoots bullets using a timer and only shoots a bullet every time that timer reaches an end.
        /// </summary>
        /// <param name="playerEntity">  </param>
        private void ContinuousProjectiles()
        {
            Vector2 playerPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;

            Vector2 shotDirection = new Vector2(0, -1);
            Vector2 leftBulletPosition = new Vector2(playerPosition.X - xBulletOffset, playerPosition.Y + yBulletOffset);
            Vector2 rightBulletPosition = new Vector2(playerPosition.X + xBulletOffset, playerPosition.Y + yBulletOffset);

            if(InputSystem.IsKeyDown(Keys.Z) && !this.bulletTimerConstant.IsOn)
            {
                bulletTimerConstant.Start();
            }
            else if(InputSystem.IsKeyUp(Keys.Z) && this.bulletTimerConstant.IsOn)
            {
                bulletTimerConstant.Stop();
                bulletTimerEndShots.Start();
            }
        }

        private void ShotConstant(GameTime gameTime, object? data)
        {
            Vector2 playerPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;

            Vector2 shotDirection = new Vector2(0, -1);
            Vector2 leftBulletPosition = new Vector2(playerPosition.X - xBulletOffset, playerPosition.Y + yBulletOffset);
            Vector2 rightBulletPosition = new Vector2(playerPosition.X + xBulletOffset, playerPosition.Y + yBulletOffset);

            hamsterBallBullets.CreateEntity(leftBulletPosition, shotDirection, bulletSpeed, scale: 0.25f, spriteLayer: 99);
            hamsterBallBullets.CreateEntity(rightBulletPosition, shotDirection, bulletSpeed, scale: 0.25f, spriteLayer: 99);
        }

        /// <summary>
        ///  Slows the player by adding a multiplier to the velocity 
        /// </summary>
        /// <param name="key"> the key tied to the slow ability. </param>
        private void TurnOnSlowAbility(Keys key)
        {
            this.activeMultiplier = Player.slowMultiplier;

            SpriteComponent hitboxSpriteComponent = GameM.CurrentScene.World.GetComponentFromEntity<SpriteComponent>(hitboxEntityID);
            hitboxSpriteComponent.isVisible = true;
            GameM.CurrentScene.World.SetComponentInEntity<SpriteComponent>(hitboxEntityID, hitboxSpriteComponent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"> the key tied to the slow ability. </param>
        private void TurnOffSlowAbility(Keys key)
        {
            this.activeMultiplier = 1f;

            SpriteComponent hitboxSpriteComponent = GameM.CurrentScene.World.GetComponentFromEntity<SpriteComponent>(hitboxEntityID);
            hitboxSpriteComponent.isVisible = false;
            GameM.CurrentScene.World.SetComponentInEntity<SpriteComponent>(hitboxEntityID, hitboxSpriteComponent);
        }

        /// <summary>
        /// adds all the listeners need for a player
        /// </summary>
        private void AddListeners()
        {
            Action<Keys> action = null;

            // add movement
            InputSystem.AddOnKeyDownListener(Keys.Right, action = (key) => { xSpeed += baseSpeed; });
            keyDownListeners.Add((Keys.Right, action));
            InputSystem.AddOnKeyDownListener(Keys.Left, action = (key) => { xSpeed -= baseSpeed; });
            keyDownListeners.Add((Keys.Left, action));
            InputSystem.AddOnKeyDownListener(Keys.Up, action = (key) => { ySpeed -= baseSpeed; });
            keyDownListeners.Add((Keys.Up, action));
            InputSystem.AddOnKeyDownListener(Keys.Down, action = (key) => { ySpeed += baseSpeed; });
            keyDownListeners.Add((Keys.Down, action));

            // add slow
            InputSystem.AddOnKeyDownListener(Keys.LeftShift, action = TurnOnSlowAbility);
            keyDownListeners.Add((Keys.LeftShift, action));

            // remove movement
            InputSystem.AddOnKeyUpListener(Keys.Right, action = (key) => { xSpeed -= baseSpeed; });
            keyUpListeners.Add((Keys.Right, action));
            InputSystem.AddOnKeyUpListener(Keys.Left, action = (key) => { xSpeed += baseSpeed; });
            keyUpListeners.Add((Keys.Left, action));
            InputSystem.AddOnKeyUpListener(Keys.Up, action = (key) => { ySpeed += baseSpeed; });
            keyUpListeners.Add((Keys.Up, action));
            InputSystem.AddOnKeyUpListener(Keys.Down, action = (key) => { ySpeed -= baseSpeed; });
            keyUpListeners.Add((Keys.Down, action));

            // remove slow
            InputSystem.AddOnKeyUpListener(Keys.LeftShift, action = TurnOffSlowAbility);
            keyUpListeners.Add((Keys.LeftShift, action));

            // InputSystem.AddOnKeyUpListener(Keys.Z, OnStopShooting);
        }

        /// <summary>
        /// Deconstuctor (known as finalizer by C#)
        /// </summary>
        ~Player()
        {
            this.Dispose(isDisposedManually: false);
        }

        /// <summary>
        /// disposes of the instances disposing of all necessary managed and unmanaged resources
        /// </summary>
        public void Dispose()
        {
            this.Dispose(isDisposedManually: true);
            GC.SuppressFinalize(this); // stops the finalizer (deconstuctor) from being called
        }

        /// <summary>
        /// disposes of the unmanaged and managed resources used by player
        /// </summary>
        /// <param name="isDisposedManually"> true when manually disposing of instance. </param>
        protected virtual void Dispose(bool isDisposedManually)
        {
            // if instance has not been disposed already go ahead and dispose
            if(!_disposed)
            {
                // if manually disposing go ahead and dispose managed resources
                if (isDisposedManually)
                {
                    // cleaning up managed resources (Has-A Relationship)
                }

                // cleaning up unmanaged resources (Is-Used-By Relationship)
                this.DisposeListenersUMR();

                _disposed = true;
            }
        }

        /// <summary>
        /// unsubscribes listeners from the input system
        /// </summary>
        private void DisposeListenersUMR()
        {
            // add movement
            foreach ((Keys, Action<Keys>) keyDownListener in this.keyDownListeners)
            {
                InputSystem.RemoveOnKeyDownListener(keyDownListener.Item1, keyDownListener.Item2);
            }

            foreach ((Keys, Action<Keys>) keyUpListener in this.keyUpListeners)
            {
                InputSystem.RemoveOnKeyUpListener(keyUpListener.Item1, keyUpListener.Item2);
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

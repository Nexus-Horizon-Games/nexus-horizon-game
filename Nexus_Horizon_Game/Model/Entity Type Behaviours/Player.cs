using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Timers;
using Nexus_Horizon_Game.View.InputSystem;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal class Player : Behaviour
    {
        // bullets
        private float xBulletOffset = 4f;
        private float yBulletOffset = -2f;
        private float bulletSpeed = 50f;
        private static BulletFactory hamsterBallBullets = new BulletFactory("laser_projectile");
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
        }

        /// <summary>
        /// entity id of player collider.
        /// </summary>
        public int HitboxEntityID
        {
            get => hitboxEntityID;
        }


        /// <summary>
        /// updates the player only if it exists
        /// </summary>
        /// <param name="gameTime"> the gametime of the running program. </param>
        public override void OnUpdate(GameTime gameTime)
        {
            // keeps the collision onto the the position of the visual of the player
            UpdateCollisionVisualPosition();

            //projectiles
            ContinuousProjectiles();

            this.bulletTimerConstant.Update(gameTime);
            this.bulletTimerEndShots.Update(gameTime);
        }

        /// <summary>
        /// Updates the viusal reperesentation position onto the player
        /// </summary>
        /// <param name="playerEntity"></param>
        private void UpdateCollisionVisualPosition()
        {
            TransformComponent playerTransform = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity);
            Scene.Loaded.ECS.SetComponentInEntity<TransformComponent>(this.hitboxEntityID, playerTransform);
        }

        /// <summary>
        /// shoots bullets using a timer and only shoots a bullet every time that timer reaches an end.
        /// </summary>
        private void ContinuousProjectiles()
        {
            Vector2 playerPosition = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position;

            Vector2 shotDirection = new Vector2(0, -1);
            Vector2 leftBulletPosition = new Vector2(playerPosition.X - xBulletOffset, playerPosition.Y + yBulletOffset);
            Vector2 rightBulletPosition = new Vector2(playerPosition.X + xBulletOffset, playerPosition.Y + yBulletOffset);

            if (InputSystem.IsKeyDown(Keys.Z) && !this.bulletTimerConstant.IsOn && !bulletTimerEndShots.IsOn)
            {
                bulletTimerConstant.Start();
            }
            else if (InputSystem.IsKeyUp(Keys.Z) && this.bulletTimerConstant.IsOn)
            {
                bulletTimerConstant.Stop();
                bulletTimerEndShots.Start();
            }
        }

        private void ShotConstant(GameTime gameTime, object? data)
        {
            Vector2 playerPosition = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position;

            Vector2 shotDirection = new Vector2(0, -1);
            Vector2 leftBulletPosition = new Vector2(playerPosition.X - xBulletOffset, playerPosition.Y + yBulletOffset);
            Vector2 rightBulletPosition = new Vector2(playerPosition.X + xBulletOffset, playerPosition.Y + yBulletOffset);

            int leftBulletEntity = hamsterBallBullets.CreateEntity(leftBulletPosition, shotDirection, bulletSpeed, null, 0.6f, 99, true);
            int rightBulletEntity = hamsterBallBullets.CreateEntity(rightBulletPosition, shotDirection, bulletSpeed, null, 0.6f, 99, true);
        }

        /// <summary>
        ///  Slows the player by adding a multiplier to the velocity 
        /// </summary>
        /// <param name="key"> the key tied to the slow ability. </param>
        public void TurnOnSlowAbility()
        {
            SpriteComponent hitboxSpriteComponent = Scene.Loaded.ECS.GetComponentFromEntity<SpriteComponent>(hitboxEntityID);
            hitboxSpriteComponent.isVisible = true;
            Scene.Loaded.ECS.SetComponentInEntity<SpriteComponent>(hitboxEntityID, hitboxSpriteComponent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"> the key tied to the slow ability. </param>
        public void TurnOffSlowAbility()
        {
            SpriteComponent hitboxSpriteComponent = Scene.Loaded.ECS.GetComponentFromEntity<SpriteComponent>(hitboxEntityID);
            hitboxSpriteComponent.isVisible = false;
            Scene.Loaded.ECS.SetComponentInEntity<SpriteComponent>(hitboxEntityID, hitboxSpriteComponent);
        }
    }
}

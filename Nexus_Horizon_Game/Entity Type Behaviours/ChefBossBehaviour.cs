using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Timers;
using System;
using System.Diagnostics;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    /// <summary>
    /// Class that contains the scripts and behavior of the chef boss (the final boss).
    /// </summary>
    internal class ChefBossBehaviour : Behaviour
    {
        private const float EnteringSpeed = 5.0f;
        private const float IdealY = 40.0f;
        private const int CircleBulletsCount = 16;

        private BulletFactory bulletFactory = new BulletFactory("BulletSample");

        private TimerContainer timerContainer = new TimerContainer();

        public ChefBossBehaviour(int thisEntity) : base(thisEntity)
        {
        }

        public enum ChefBossState : int
        {
            None,
            Start,          // The starting state
            EnteringArena,  // When the boss is entering the arena
            Stage1,         // The frist stage of the fight
            Stage2          // the second stage of the fight
        }

        public override void OnUpdate(GameTime gameTime)
        {
            var state = GameM.CurrentScene.World.GetComponentFromEntity<StateComponent>(this.Entity);

            timerContainer.Update(gameTime);

            if ((ChefBossState)state.state == ChefBossState.Start)
            {
                StartState();
            }
            else if ((ChefBossState)state.state == ChefBossState.EnteringArena)
            {
                EnteringArenaState();
            }
            else if ((ChefBossState)state.state == ChefBossState.Stage1)
            {
                // drag:
                var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
                body.Acceleration = body.Velocity * -3.0f;
                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);
            }
            else if ((ChefBossState)state.state == ChefBossState.Stage2)
            {

            }
        }

        private void StartState()
        {
            timerContainer.AddTimer(new LoopTimer(5.0f, OnMoveAction), "move_action");

            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new TransformComponent(new Vector2(Renderer.DrawAreaWidth / 2.0f, -20.0f)));

            // Start moving into the arena
            var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
            body.Velocity = new Vector2(0.0f, EnteringSpeed);
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);

            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(ChefBossState.EnteringArena));
        }

        private void EnteringArenaState()
        {
            var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);

            if (transform.position.Y >= IdealY) // If reached the start y position
            {
                var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
                body.Velocity = Vector2.Zero;
                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);

                timerContainer.GetTimer("move_action").Start();

                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(ChefBossState.Stage1));
            }
        }

        private void OnMoveAction(GameTime gameTime, object? data)
        {
            // Move:
            var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);
            var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);

            float left = 1.0f;
            float right = 1.0f;
            float down = 1.0f;
            float up = 1.0f;

            if (transform.position.X - GameM.CurrentScene.ArenaLeft < 20.0f)
            {
                left = 0.0f;
            }
            else if (GameM.CurrentScene.ArenaRight - transform.position.X < 20.0f)
            {
                right = 0.0f;
            }

            if (transform.position.Y - GameM.CurrentScene.ArenaTop < 20.0f)
            {
                up = 0.0f;
            }
            else if (GameM.CurrentScene.ArenaBottom - transform.position.Y < 50.0f)
            {
                down = 0.0f;
            }

            //Debug.WriteLine($"left {left} right {right} up: {up} down: {down}");

            var moveDirection = new Vector2(RandomGenerator.GetFloat(-left, right), RandomGenerator.GetFloat(-up, down));
            moveDirection.Normalize();

            body.Velocity = moveDirection * 8.0f;

            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);

            // Fire bullets:
            timerContainer.StartTemporaryTimer(new DelayTimer(0.6f, (gameTime, data) => FireBulletCircle(true)));
            timerContainer.StartTemporaryTimer(new DelayTimer(1.6f, (gameTime, data) => FireBulletCircle(false)));
        }

        private void FireBulletCircle(bool counterClockwise)
        {
            var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;

            float arcInterval = MathHelper.TwoPi / CircleBulletsCount;

            for (int i = 0; i < CircleBulletsCount; i++)
            {
                Vector2 direction = new Vector2((float)Math.Cos(arcInterval * i), (float)Math.Sin(arcInterval * i));
                Vector2 perpendicularDirection;

                if (counterClockwise)
                {
                    perpendicularDirection = new Vector2(direction.Y, -direction.X);
                }
                else
                {
                    perpendicularDirection = new Vector2(-direction.Y, direction.X);
                }

                var bullet = bulletFactory.CreateEntity(bossPosition + direction * 10.0f, direction + perpendicularDirection, 6.0f, bulletAction: (gametime, bullet, previousVelocity) =>
                {
                    if (bullet.TimeAlive > 0.3f && bullet.TimeAlive < 1.2f)
                    {
                        Vector2 acceleration = direction * 14.0f;
                        return previousVelocity - (acceleration * (float)gametime.ElapsedGameTime.TotalSeconds);
                    }
                    else if (bullet.TimeAlive < 2.5f)
                    {
                        Vector2 acceleration = direction * 8.0f;
                        return previousVelocity + (acceleration * (float)gametime.ElapsedGameTime.TotalSeconds);
                    }

                    return previousVelocity;
                });
            }
        }

        private Vector2 CircleBulletVelocity(GameTime gametime, Bullet bullet, Vector2 previousVelocity)
        {
            if (bullet.TimeAlive > 0.5f && bullet.TimeAlive < 1.5f)
            {
                Vector2 direction = previousVelocity;
                direction.Normalize();
                return previousVelocity - (direction * (float)gametime.ElapsedGameTime.TotalSeconds * 10.0f);
            }
            else if (bullet.TimeAlive < 2.0f)
            {
                Vector2 direction = previousVelocity;
                direction.Normalize();
                return previousVelocity + (direction * (float)gametime.ElapsedGameTime.TotalSeconds * 6.0f);
            }
            else if (bullet.TimeAlive < 3.0f)
            {
                Vector2 direction = previousVelocity;
                direction.Normalize();
                return previousVelocity - (direction * (float)gametime.ElapsedGameTime.TotalSeconds * 6.0f);
            }
            else if (bullet.TimeAlive < 4.0f)
            {
                Vector2 direction = previousVelocity;
                direction.Normalize();
                return previousVelocity + (direction * (float)gametime.ElapsedGameTime.TotalSeconds * 6.0f);
            }

            return previousVelocity;
        }
    }
}

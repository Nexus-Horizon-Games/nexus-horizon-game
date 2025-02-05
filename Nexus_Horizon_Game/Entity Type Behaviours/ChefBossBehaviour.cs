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
            var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
            body.Velocity = new Vector2(RandomGenerator.GetInteger(-1, 2), RandomGenerator.GetInteger(-1, 2)) * 0.5f;
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);

            timerContainer.StartTemporaryTimer(new DelayTimer(0.6f, (gameTime, data) => FireBulletCircle()));
            timerContainer.StartTemporaryTimer(new DelayTimer(1.6f, (gameTime, data) => FireBulletCircle()));
        }

        private void FireBulletCircle()
        {
            var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;

            float arcInterval = MathHelper.TwoPi / CircleBulletsCount;

            for (int i = 0; i < CircleBulletsCount; i++)
            {
                Vector2 unit = new Vector2((float)Math.Cos(arcInterval * i), (float)Math.Sin(arcInterval * i));
                var bullet = bulletFactory.CreateEntity(bossPosition + unit * 10.0f, unit, 6.0f, bulletAction: (gametime, bullet, previousVelocity) =>
                {
                    if (bullet.TimeAlive > 0.3f && bullet.TimeAlive < 1.2f)
                    {
                        return previousVelocity - (unit * (float)gametime.ElapsedGameTime.TotalSeconds * 14.0f);
                    }
                    else if (bullet.TimeAlive < 2.5f)
                    {
                        return previousVelocity + (unit * (float)gametime.ElapsedGameTime.TotalSeconds * 10.0f);
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

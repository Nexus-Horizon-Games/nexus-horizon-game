using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Paths;
using Nexus_Horizon_Game.Timers;
using System;

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
            timerContainer.AddTimer(new LoopTimer(2.0f, OnFireBullets), "fire_bullets");

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

                timerContainer.GetTimer("fire_bullets").Start();

                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(ChefBossState.Stage1));
            }
        }

        private void OnFireBullets(GameTime gameTime, object? data)
        {
            FireBulletCircle();
            timerContainer.StartTemporaryTimer(new DelayTimer(0.3f, (gameTime, data) => FireBulletCircle()));
        }

        private void FireBulletCircle()
        {
            var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;

            float arcInterval = MathHelper.TwoPi / CircleBulletsCount;

            for (int i = 0; i < CircleBulletsCount; i++)
            {
                Vector2 unit = new Vector2((float)Math.Cos(arcInterval * i), (float)Math.Sin(arcInterval * i));
                var bullet = bulletFactory.CreateEntity(bossPosition + unit * 10.0f, unit, 6.0f);
            }
        }
    }
}

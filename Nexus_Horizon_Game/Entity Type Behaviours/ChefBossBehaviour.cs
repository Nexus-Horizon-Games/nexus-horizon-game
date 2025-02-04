using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Paths;
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

        private float timer = 0f;
        private IPath some = new MultiPath([
            new CubicCurvePath(new Vector2(0, 0f), new Vector2(0, 80), new Vector2(40, -40), new Vector2(80f, 0f)),
            new QuadraticCurvePath(new Vector2(80.0f, 0f), new Vector2(40f, -40.0f), new Vector2(0f, 0f)),
            new LinePath(new Vector2(0f, 0f), new Vector2(80f, 80f))
            ]
            );

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
                if (timer < 1f)
                {
                    timer += some.GetDeltaT(timer, 0.2f);

                    var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);
                    transform.position = some.GetPoint(timer) + new Vector2(40.0f, 40.0f);
                    GameM.CurrentScene.World.SetComponentInEntity(this.Entity, transform);
                }
            }
            else if ((ChefBossState)state.state == ChefBossState.Stage2)
            {

            }
        }

        private void StartState()
        {
            var timerComp = new TimersComponent([]);
            timerComp.timers.Add("fire_bullets", new Timer(0.4f, OnFireBullets, this.Entity));
            GameM.CurrentScene.World.AddComponent(this.Entity, timerComp);

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

                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(ChefBossState.Stage1));
                var timers = GameM.CurrentScene.World.GetComponentFromEntity<TimersComponent>(this.Entity);
                timers.timers["fire_bullets"].Start();
            }
        }

        private void OnFireBullets(GameTime gameTime, object? data)
        {
            var bulletFactory = new BulletFactory("BulletSample");

            var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;

            int bullets = 16;
            float arcInterval = MathHelper.TwoPi / bullets;

            for (int i = 0; i < bullets; i++)
            {
                Vector2 unit = new Vector2((float)Math.Cos(arcInterval * i), (float)Math.Sin(arcInterval * i));
                var bullet = bulletFactory.CreateEntity(bossPosition + unit * 10.0f, unit, 6.0f);
            }
        }
    }
}

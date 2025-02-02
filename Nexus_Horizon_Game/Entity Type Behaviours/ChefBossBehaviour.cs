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
    internal static class ChefBossBehaviour
    {
        private const float EnteringSpeed = 5.0f;
        private const float IdealY = 40.0f;

        private static float timer = 0f;
        private static IPath some = new CubicCurvePath(new Vector2(0, 0f), new Vector2(0, 80), new Vector2(40, -40), new Vector2(80f, 0f));

        public enum ChefBossState : int
        {
            None,
            Start,          // The starting state
            EnteringArena,  // When the boss is entering the arena
            Stage1,         // The frist stage of the fight
            Stage2          // the second stage of the fight
        }

        public static void OnUpdate(int thisEntity, GameTime gameTime)
        {
            var state = GameM.CurrentScene.World.GetComponentFromEntity<StateComponent>(thisEntity);

            if ((ChefBossState)state.state == ChefBossState.Start)
            {
                StartState(thisEntity);
            }
            else if ((ChefBossState)state.state == ChefBossState.EnteringArena)
            {
                EnteringArenaState(thisEntity);
            }
            else if ((ChefBossState)state.state == ChefBossState.Stage1)
            {
                if (timer < 1f)
                {
                    timer += some.GetDeltaT(timer, 1.0f);
                }

                var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(thisEntity);
                transform.position = some.GetPoint(timer);
                GameM.CurrentScene.World.SetComponentInEntity(thisEntity, transform);
            }
            else if ((ChefBossState)state.state == ChefBossState.Stage2)
            {

            }
        }

        private static void StartState(int thisEntity)
        {
            var timerComp = new TimersComponent([]);
            timerComp.timers.Add("fire_bullets", new Timer(0.2f, OnFireBullets, thisEntity));
            GameM.CurrentScene.World.AddComponent(thisEntity, timerComp);

            GameM.CurrentScene.World.SetComponentInEntity(thisEntity, new TransformComponent(new Vector2(Renderer.DrawAreaWidth / 2.0f, -20.0f)));

            // Start moving into the arena
            var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(thisEntity);
            body.Velocity = new Vector2(0.0f, EnteringSpeed);
            GameM.CurrentScene.World.SetComponentInEntity(thisEntity, body);

            GameM.CurrentScene.World.SetComponentInEntity(thisEntity, new StateComponent(ChefBossState.EnteringArena));
        }

        private static void EnteringArenaState(int thisEntity)
        {
            var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(thisEntity);

            if (transform.position.Y >= IdealY) // If reached the start y position
            {
                var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(thisEntity);
                body.Velocity = Vector2.Zero;
                GameM.CurrentScene.World.SetComponentInEntity(thisEntity, body);

                GameM.CurrentScene.World.SetComponentInEntity(thisEntity, new StateComponent(ChefBossState.Stage1));
                var timers = GameM.CurrentScene.World.GetComponentFromEntity<TimersComponent>(thisEntity);
                timers.timers["fire_bullets"].Start();
            }
        }

        private static void OnFireBullets(GameTime gameTime, object? data)
        {
            var thisEntity = (int)data;

            var bulletFactory = new BulletFactory("BulletSample");

            var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(thisEntity).position;

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

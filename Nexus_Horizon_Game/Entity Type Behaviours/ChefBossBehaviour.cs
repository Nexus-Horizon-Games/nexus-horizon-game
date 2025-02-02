using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using System.Linq;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    /// <summary>
    /// Class that contains the scripts and behavior of the chef boss (the final boss).
    /// </summary>
    internal static class ChefBossBehaviour
    {
        private const float EnteringSpeed = 15.0f;
        private const float IdealY = 40.0f;

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
            var bullet = bulletFactory.CreateEntity();

            var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(thisEntity).position;

            var players = GameM.CurrentScene.World.GetEntitiesWithComponent<PlayerComponent>().ToList();
            var playerPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(players[0]).position;

            var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(bullet);
            transform.position = bossPosition;
            GameM.CurrentScene.World.SetComponentInEntity(bullet, transform);

            var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(bullet);
            var direction = playerPosition - transform.position;
            direction.Normalize();
            body.Velocity = direction * 60.0f;
            GameM.CurrentScene.World.SetComponentInEntity(bullet, body);
        }
    }
}

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

        public static void OnUpdate(World world, int thisEntity, GameTime gameTime)
        {
            var state = world.GetComponentFromEntity<StateComponent>(thisEntity);

            if ((ChefBossState)state.state == ChefBossState.Start)
            {
                StartState(world, thisEntity);
            }
            else if ((ChefBossState)state.state == ChefBossState.EnteringArena)
            {
                EnteringArenaState(world, thisEntity);
            }
            else if ((ChefBossState)state.state == ChefBossState.Stage1)
            {
                
            }
            else if ((ChefBossState)state.state == ChefBossState.Stage2)
            {

            }
        }

        private static void StartState(World world, int thisEntity)
        {
            var timerComp = new TimersComponent([]);
            timerComp.timers.Add("fire_bullets", new Timer(0.2f, OnFireBullets, (world, thisEntity)));
            world.AddComponent(thisEntity, timerComp);

            world.SetComponentInEntity(thisEntity, new TransformComponent(new Vector2(Renderer.DrawAreaWidth / 2.0f, -20.0f)));

            // Start moving into the arena
            var body = world.GetComponentFromEntity<PhysicsBody2DComponent>(thisEntity);
            body.Velocity = new Vector2(0.0f, EnteringSpeed);
            world.SetComponentInEntity(thisEntity, body);

            world.SetComponentInEntity(thisEntity, new StateComponent(ChefBossState.EnteringArena));
        }

        private static void EnteringArenaState(World world, int thisEntity)
        {
            var transform = world.GetComponentFromEntity<TransformComponent>(thisEntity);

            if (transform.position.Y >= IdealY) // If reached the start y position
            {
                var body = world.GetComponentFromEntity<PhysicsBody2DComponent>(thisEntity);
                body.Velocity = Vector2.Zero;
                world.SetComponentInEntity(thisEntity, body);

                world.SetComponentInEntity(thisEntity, new StateComponent(ChefBossState.Stage1));
                var timers = world.GetComponentFromEntity<TimersComponent>(thisEntity);
                timers.timers["fire_bullets"].Start();
            }
        }

        private static void OnFireBullets(GameTime gameTime, object? data)
        {
            var (world, thisEntity) = ((World, int))data;

            var bulletFactory = new BulletFactory(world, "BulletSample");
            var bullet = bulletFactory.CreateEntity();

            var bossPosition = world.GetComponentFromEntity<TransformComponent>(thisEntity).position;

            var players = world.GetEntitiesWithComponent<PlayerComponent>().ToList();
            var playerPosition = world.GetComponentFromEntity<TransformComponent>(players[0]).position;

            var transform = world.GetComponentFromEntity<TransformComponent>(bullet);
            transform.position = bossPosition;
            world.SetComponentInEntity(bullet, transform);

            var body = world.GetComponentFromEntity<PhysicsBody2DComponent>(bullet);
            var direction = playerPosition - transform.position;
            direction.Normalize();
            body.Velocity = direction * 60.0f;
            world.SetComponentInEntity(bullet, body);
        }
    }
}


using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

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
            }
        }
    }
}

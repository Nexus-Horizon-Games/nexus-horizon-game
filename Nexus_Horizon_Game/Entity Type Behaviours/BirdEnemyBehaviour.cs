using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using System.Linq;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    /// <summary>
    /// Class that contains the scripts and behavior of the chef boss (the final boss).
    /// </summary>
    internal static class BirdEnemyBehaviour
    {

        public enum BirdEnemyState : int
        {
            None,
            Start,          // The starting state
            EnteringArena,  // When the bird is pathing to the starting point
           Attacking, // When the bird is pathing and firing
           LeavingArena // when the bird is leaving the arena
        }

        public static void OnUpdate(int thisEntity, GameTime gameTime)
        {
            var state = GameM.CurrentScene.World.GetComponentFromEntity<StateComponent>(thisEntity);

            if ((BirdEnemyState)state.state == BirdEnemyState.Start)
            {
                StartState(thisEntity);
            }
            else if ((BirdEnemyState)state.state == BirdEnemyState.EnteringArena)
            {
                EnteringArenaState(thisEntity);
            }
            else if ((BirdEnemyState)state.state == BirdEnemyState.Attacking)
            {

            }
            else if ((BirdEnemyState)state.state == BirdEnemyState.LeavingArena)
            {

            }
        }

        private static void StartState(int thisEntity)
        {
        }

        private static void OnFireBullets(GameTime gameTime, object? data)
        {
        }
    }
}
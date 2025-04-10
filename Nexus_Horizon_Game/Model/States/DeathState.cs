
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model.GameManagers;
using Nexus_Horizon_Game.Paths;
using System.Diagnostics;

namespace Nexus_Horizon_Game.States
{
    /// <summary>
    /// The state for when an entity dies (TODO: update to add entity drops, death animation, etc.)
    /// </summary>
    internal class DeathState : State
    {
        public DeathState(int entity) : base(entity)
        {
        }

        public override void OnStart()
        {
            // Points Change on death
            GameplayManager.Instance.KilledEnemy(this.Entity);

            // Debug.WriteLine($"enity {this.Entity} died!!!");

            //OnStop();
            // use this to update the ECS instead of creating a loop with OnStop(), So no stack overflow exception is caused.
            Scene.Loaded.ECS.DestroyEntity(this.Entity);
        }
        
        public override void OnUpdate(GameTime gameTime)
        {
        }
    }
}



using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
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
            Debug.WriteLine($"enity {this.Entity} died!!!");
            OnStop();
        }

        public override void OnUpdate(GameTime gameTime)
        {
        }
    }
}


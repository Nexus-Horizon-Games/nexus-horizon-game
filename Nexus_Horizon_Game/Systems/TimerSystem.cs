using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System.Linq;

namespace Nexus_Horizon_Game.Systems
{
    internal static class TimerSystem
    {
        public static void Update(World world, GameTime gameTime)
        {
            var components = world.GetComponents<TimersComponent>().ToList();

            foreach (var component in components)
            {
                foreach (var timer in component.timers)
                {
                    timer.Value.Update(gameTime);
                }
            }
        }
    }
}

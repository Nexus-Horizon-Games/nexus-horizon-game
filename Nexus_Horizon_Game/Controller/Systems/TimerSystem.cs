﻿using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System.Linq;

namespace Nexus_Horizon_Game.Systems
{
    internal static class TimerSystem
    {
        public static void Update(GameTime gameTime)
        {
            var components = Scene.Loaded.ECS.GetComponents<TimersComponent>();

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

using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model.Prefab;
using Nexus_Horizon_Game.Timers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexus_Horizon_Game.Model.EntityPatterns
{
    internal class DirectFiringPattern : AbstractFiringPattern, IFiringPattern
    {
        private readonly float velocity;

        public DirectFiringPattern(float velocity)
        {
            this.velocity = velocity;
        }

        public void Fire(PrefabEntity prefab, GameTime gameTime, TimerContainer timerContainer)
        {
            List<int> firedEntities = new List<int>();

            Vector2 position = ((TransformComponent)prefab.Components.FirstOrDefault(x => x.GetType() == typeof(TransformComponent))).position;
            var playerPosition = GetPlayerPosition();
            double direction = Math.Atan2((double)(playerPosition.Y - position.Y), (double)(playerPosition.X - position.X));
            Vector2 fireDirection = GetVectFromDirection(direction, 0);
            SpawnEntity(position, fireDirection, velocity, prefab);
            return;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Model.Prefab;
using Nexus_Horizon_Game.Timers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Model.EntityPatterns
{
    internal class ClockwiseRingFiringPattern1 : AbstractFiringPattern, IFiringPattern
    {
        public void Fire(PrefabEntity prefab, GameTime gameTime, TimerContainer timerContainer)
        {
            List<int> firedEntities = new List<int>();
            List<IComponent> components = prefab.getComponents();
            Vector2 position = ((TransformComponent)prefab.getComponents().FirstOrDefault(x => x.GetType() == typeof(TransformComponent))).position;
            float velocity = 7f;
            var playerPosition = GetPlayerPosition();
            const float SpawnRadius = 10.0f;
            const float StartSpeed = 6.0f;
            int CircleBulletsCount = 16;
            float arcInterval = MathHelper.TwoPi / CircleBulletsCount;

            for (int i = 0; i < CircleBulletsCount; i++)
            {
                Vector2 direction = new Vector2((float)Math.Cos(arcInterval * i), (float)Math.Sin(arcInterval * i));
                Vector2 perpendicularDirection;

                perpendicularDirection = new Vector2(-direction.Y, direction.X);
                SpawnEntity(position + direction * SpawnRadius, direction + (perpendicularDirection * 0.25f), StartSpeed, prefab);
            }
            return;
        }
    }
}

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
    internal class ArcFiringPattern : AbstractFiringPattern, IFiringPattern
    {
        public void Fire(PrefabEntity prefab, GameTime gameTime, TimerContainer timerContainer)
        {
            List<int> firedEntities = new List<int>();
            List<IComponent> components = prefab.getComponents();
            Vector2 position = ((TransformComponent)prefab.getComponents().FirstOrDefault(x => x.GetType() == typeof(TransformComponent))).position;
            float velocity = 7f;
            var playerPosition = GetPlayerPosition();
            double direction = Math.Atan2((double)(playerPosition.Y - position.Y), (double)(playerPosition.X - position.X));
            Vector2 fireDirection = GetVectFromDirection(direction, 0);


            // Calculate direction from boss to player.
            Vector2 baseDirection = playerPosition - position;
            baseDirection.Normalize();

            // Base angle toward player.
            float baseAngle = (float)Math.Atan2(baseDirection.Y, baseDirection.X);
            float angleStep = MathHelper.ToRadians(15); // Angle between bullets.

            for (int burst = 0; burst < 3; burst++)
            {
                timerContainer.StartTemporaryTimer(new DelayTimer(0.3f * burst, (gameTime, data) =>
                {
                    for (int i = -3; i <= 3; i++) // Create a spread pattern.
                    {
                        float angle = baseAngle + (i * angleStep);
                        Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                        SpawnEntity(position, direction, 7.0f, prefab);
                    }
                }));
            }
            return;
        }
    }
}

using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model.Prefab;
using Nexus_Horizon_Game.Timers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexus_Horizon_Game.Model.EntityPatterns
{
    internal class ArcFiringPattern : AbstractFiringPattern, IFiringPattern
    {
        private readonly float velocity;
        private readonly float bulletAngle;
        private readonly int bursts;
        private readonly int spread;
        private readonly float burstInterval;

        /// <summary>
        /// Creates a new <see cref="ArcFiringPattern"/>.
        /// </summary>
        /// <param name="velocity">The velocity of the bullets when shot.</param>
        /// <param name="bulletAngle">The angle between the bullets in degrees.</param>
        public ArcFiringPattern(float velocity, float bulletAngle = 15.0f, int bursts = 3, int spread = 3, float burstInterval = 0.3f)
        {
            this.velocity = velocity;
            this.bulletAngle = bulletAngle;
            this.bursts = bursts;
            this.spread = spread;
            this.burstInterval = burstInterval;
        }

        public void Fire(PrefabEntity prefab, GameTime gameTime, TimerContainer timerContainer)
        {
            List<int> firedEntities = new List<int>();
            List<IComponent> components = prefab.Components;
            Vector2 position = ((TransformComponent)prefab.Components.FirstOrDefault(x => x.GetType() == typeof(TransformComponent))).position;

            var playerPosition = GetPlayerPosition();
            double direction = Math.Atan2((double)(playerPosition.Y - position.Y), (double)(playerPosition.X - position.X));
            Vector2 fireDirection = GetVectFromDirection(direction, 0);


            // Calculate direction from boss to player.
            Vector2 baseDirection = playerPosition - position;
            baseDirection.Normalize();

            // Base angle toward player.
            float baseAngle = (float)Math.Atan2(baseDirection.Y, baseDirection.X);
            float angleStep = MathHelper.ToRadians(bulletAngle); // Angle between bullets.

            for (int burst = 0; burst < bursts; burst++)
            {
                timerContainer.StartTemporaryTimer(new DelayTimer(burstInterval * burst, (gameTime, data) =>
                {
                    for (int i = -spread; i <= spread; i++) // Create a spread pattern.
                    {
                        float angle = baseAngle + (i * angleStep);
                        Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                        SpawnEntity(position, direction, velocity, prefab);
                    }
                }));
            }
            return;
        }
    }
}

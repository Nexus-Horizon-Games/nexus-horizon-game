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
    internal class ChefBossPattern2 : AbstractFiringPattern, IFiringPattern
    {
        public void Fire(PrefabEntity prefab, GameTime gameTime, TimerContainer timerContainer)
        {
            List<int> firedEntities = new List<int>();
            List<IComponent> components = prefab.getComponents();
            Vector2 position = ((TransformComponent)prefab.getComponents().FirstOrDefault(x => x.GetType() == typeof(TransformComponent))).position;
            float velocity = 7f;
            var playerPosition = GetPlayerPosition();
            const float SpawnTimeInterval = 0.01f;
            const float SpawnTimeLength = 1.5f;

            const float StartSpawnRadius = 5.0f;
            const float SpawnRadiusAcceleration = 15.0f;

            const float StartRotationSpeed = 10.0f;
            const float RotationSpeedAcceleration = 1.0f;

            const float Spawner1Angle = 0.0f;
            const float Spawner2Angle = MathHelper.TwoPi / 3.0f;
            const float Spawner3Angle = MathHelper.TwoPi * 2.0f / 3.0f;

            const float BulletSpeed = 6.0f;


            timerContainer.StartTemporaryTimer(new LoopTimer(SpawnTimeInterval, (gameTime, data) =>
            {
                double startTime = (double)data;
                double time = gameTime.TotalGameTime.TotalSeconds - startTime;

                float spawnRadius = StartSpawnRadius + (float)(time * time) * SpawnRadiusAcceleration;
                float rotationSpeed = StartRotationSpeed + (float)(time * time) * RotationSpeedAcceleration;

                var bossPosition = position;
                var playerPosition = GetPlayerPosition();

                // Spawner positions:
                Vector2 spawner1 = new Vector2((float)Math.Cos(Spawner1Angle + time * rotationSpeed), (float)Math.Sin(Spawner1Angle + time * rotationSpeed)) * spawnRadius;
                Vector2 spawner2 = new Vector2((float)Math.Cos(Spawner2Angle + time * rotationSpeed), (float)Math.Sin(Spawner2Angle + time * rotationSpeed)) * spawnRadius;
                Vector2 spawner3 = new Vector2((float)Math.Cos(Spawner3Angle + time * rotationSpeed), (float)Math.Sin(Spawner3Angle + time * rotationSpeed)) * spawnRadius;

                // Spawner shoot directions:
                Vector2 spawner1Direction = playerPosition - (spawner1 + bossPosition); // Point towards the player
                spawner1Direction.Normalize();

                // Get the angle between spawner1Direction and <1, 0>
                double angle = Math.Acos((double)Vector2.Dot(spawner1Direction, new Vector2(1.0f, 0.0f)));

                Vector2 spawner2Direction = new Vector2((float)Math.Cos(Spawner2Angle + angle), (float)Math.Sin(Spawner2Angle + angle));
                Vector2 spawner3Direction = new Vector2((float)Math.Cos(Spawner3Angle + angle), (float)Math.Sin(Spawner3Angle + angle));

                // Spawn the bullets:
                SpawnEntity(bossPosition + spawner1, spawner1Direction, BulletSpeed, prefab);
                SpawnEntity(bossPosition + spawner2, spawner2Direction, BulletSpeed, prefab);
                SpawnEntity(bossPosition + spawner3, spawner3Direction, BulletSpeed, prefab);

            }, data: gameTime.TotalGameTime.TotalSeconds, stopAfter: SpawnTimeLength));
            return;
        }
    }
}

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
    internal class ChefBossPattern1 : AbstractFiringPattern, IFiringPattern
    {
        public void Fire(PrefabEntity prefab, GameTime gameTime, TimerContainer timerContainer)
        {
            List<int> firedEntities = new List<int>();
            List<IComponent> components = prefab.Components;
            Vector2 position = ((TransformComponent)prefab.Components.FirstOrDefault(x => x.GetType() == typeof(TransformComponent))).position;
            float velocity = 7f;
            var playerPosition = GetPlayerPosition();
            const float SpawnTimeInterval = 0.01f;
            const float SpawnTimeLength = 0.8f;

            const float StartSpawnRadius = 1.0f;
            const float SpawnRadiusAcceleration = 50.0f;

            const float StartRotationSpeed = 10.0f;
            const float RotationSpeedAcceleration = 1.0f;

            const float Spawner1Angle = 0.0f;
            const float Spawner2Angle = MathHelper.Pi;

            const float BulletSpeed = 3.0f;


            timerContainer.StartTemporaryTimer(new LoopTimer(SpawnTimeInterval, (gameTime, data) =>
            {
                double startTime = (double)data;
                double time = gameTime.TotalGameTime.TotalSeconds - startTime;

                float spawnRadius = StartSpawnRadius + (float)(time * time) * SpawnRadiusAcceleration;
                float rotationSpeed = StartRotationSpeed + (float)(time * time) * RotationSpeedAcceleration;
                var playerPosition = GetPlayerPosition();

                // Spawner positions:
                Vector2 spawner1 = new Vector2((float)Math.Cos(Spawner1Angle + time * rotationSpeed), (float)Math.Sin(Spawner1Angle + time * rotationSpeed)) * spawnRadius;
                Vector2 spawner2 = new Vector2((float)Math.Cos(Spawner2Angle + time * rotationSpeed), (float)Math.Sin(Spawner2Angle + time * rotationSpeed)) * spawnRadius;

                // Spawner shoot directions:
                Vector2 spawner1Direction = playerPosition - (spawner1 + position);
                spawner1Direction.Normalize();

                Vector2 spawner2Direction = playerPosition - (spawner2 + position);
                spawner2Direction.Normalize();

                // Spawn the bullets:
                SpawnEntity(position + spawner1, spawner1Direction, BulletSpeed, prefab);
                SpawnEntity(position + spawner2, spawner2Direction, BulletSpeed, prefab);

            }, data: gameTime.TotalGameTime.TotalSeconds, stopAfter: SpawnTimeLength));
            return;
        }
    }
}

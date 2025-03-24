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
    internal class ChefBossPattern2 : IFiringPattern
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
                spawnEntity(bossPosition + spawner1, spawner1Direction, BulletSpeed, prefab);
                spawnEntity(bossPosition + spawner2, spawner2Direction, BulletSpeed, prefab);
                spawnEntity(bossPosition + spawner3, spawner3Direction, BulletSpeed, prefab);

            }, data: gameTime.TotalGameTime.TotalSeconds, stopAfter: SpawnTimeLength));
            return;
        }
        private void spawnEntity(Vector2 position, Vector2 fireDirection, float speed, PrefabEntity prefab)
        {
            List<IComponent> components = prefab.getComponents();
            components.RemoveAll(x => x.GetType() == typeof(TransformComponent));
            components.Add(new PhysicsBody2DComponent()
            {
                Velocity = new Vector2(speed * fireDirection.X, speed * fireDirection.Y)
            });
            components.Add(new TransformComponent()
            {
                position = position
            });
            int firedEntity = Scene.Loaded.ECS.CreateEntity(components);
            Scene.Loaded.ECS.SetComponentInEntity<BehaviourComponent>(firedEntity, new BehaviourComponent(new Bullet(firedEntity)));
        }
        public Vector2 GetVectFromDirection(double direction, double variation)
        {
            direction += variation;
            float xComponent = (float)(Math.Cos(direction));
            float yComponent = (float)(Math.Sin(direction));
            return new Vector2(xComponent, yComponent);
        }

        public Vector2 GetPlayerPosition()
        {
            var entitesWithTag = Scene.Loaded.ECS.GetEntitiesWithComponent<TagComponent>();
            var playerEntity = -1;
            foreach (var entity in entitesWithTag)
            {
                var tag = Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(entity);
                if (tag.Tag == Tag.PLAYER)
                {
                    playerEntity = entity;
                    break;
                }
            }

            Vector2 playerPosition = Vector2.Zero;
            if (playerEntity != -1)
            {
                playerPosition = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(playerEntity).position;
            }
            Debug.WriteLine("player position is " + playerPosition);
            return playerPosition;
        }
    }
}

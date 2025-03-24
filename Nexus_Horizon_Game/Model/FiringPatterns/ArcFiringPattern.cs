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
    internal class ArcFiringPattern : IFiringPattern
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
                        spawnEntity(position, direction, 7.0f, prefab);
                    }
                }));
            }
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

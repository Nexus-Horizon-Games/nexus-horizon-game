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
    internal class CounterClockwiseRingFiringPattern2 : AbstractFiringPattern, IFiringPattern
    {
        public void Fire(PrefabEntity prefab, GameTime gameTime, TimerContainer timerContainer)
        {
            List<int> firedEntities = new List<int>();
            List<IComponent> components = prefab.Components;
            Vector2 position = ((TransformComponent)prefab.Components.FirstOrDefault(x => x.GetType() == typeof(TransformComponent))).position;
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

                perpendicularDirection = new Vector2(direction.Y, -direction.X);
                SpawnEntity(position + direction * SpawnRadius, direction + (perpendicularDirection * 0.25f), StartSpeed, prefab);
            }
            return;
        }
        
        protected override int SpawnEntity(Vector2 position, Vector2 fireDirection, float speed, PrefabEntity prefab)
        {
            int firedEntity = base.SpawnEntity(position,fireDirection, speed, prefab);

            
            // this gets added first of the base function but is overriden by this behaviour all good for now
            Scene.Loaded.ECS.SetComponentInEntity<BehaviourComponent>(firedEntity, new BehaviourComponent(new Bullet(firedEntity, bulletBehavior: (gametime, bullet, bulletEntity, previousVelocity) =>
            {
                if (bullet.TimeAlive > 0.3f && bullet.TimeAlive < 1.2f)
                {
                    Vector2 acceleration = fireDirection * 15.0f;
                    return (previousVelocity - (acceleration * (float)gametime.ElapsedGameTime.TotalSeconds));
                }
                else if (bullet.TimeAlive < 2.5f)
                {
                    Vector2 acceleration = fireDirection * 10.0f;
                    return (previousVelocity + (acceleration * (float)gametime.ElapsedGameTime.TotalSeconds));
                }

                return previousVelocity;
            })));
            

            return firedEntity;
        }
    }
}

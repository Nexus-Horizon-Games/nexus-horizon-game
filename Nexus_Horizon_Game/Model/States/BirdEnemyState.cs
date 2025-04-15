using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.EntityFactory;
using Nexus_Horizon_Game.Model.EntityPatterns;
using Nexus_Horizon_Game.Paths;
using Nexus_Horizon_Game.Timers;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.States
{
    internal class BirdEnemyState : State
    {
        private float FireRate = 0.2f;
        private float t = 0;
        private float speed = 0.4f;
        private TimerContainer timerContainer = new TimerContainer();
        private MultiPath movementPath;
        private List<int> attackPaths;
        private int spawnerEntity;

        private Tag bulletsTag;

        public BirdEnemyState(MultiPath movementPath, List<int> attackPaths, Tag bulletsTag = 0)
        {
            this.movementPath = movementPath;
            this.attackPaths = attackPaths;
            this.bulletsTag = bulletsTag;
        }

        public override void OnStart()
        {
            this.spawnerEntity = EntitySpawnerFactory.CreateBulletSpawner("BulletSample", projectileTag: bulletsTag);
            Scene.Loaded.ECS.SetComponentInEntity(this.Entity, new TransformComponent(new Vector2(-100, -100)));
            timerContainer.AddTimer(new LoopTimer(FireRate, (gameTime, data) => OnFireBullets(gameTime)), "fire");
        }

        public override void OnUpdate(GameTime gameTime)
        {
            timerContainer.Update(gameTime);

            if (t >= 1) 
            {
                OnStop();
                return;
            }
            if (attackPaths.Contains(movementPath.getIndex(t)))
            {
                if (!timerContainer.GetTimer("fire").IsOn)
                {
                    timerContainer.GetTimer("fire").Start();
                }
            }
            else
            {
                if (timerContainer.GetTimer("fire").IsOn)
                {
                    timerContainer.GetTimer("fire").Stop();
                }
            }

            t += movementPath.GetDeltaT(t,speed);
            Scene.Loaded.ECS.SetComponentInEntity(this.Entity, new TransformComponent(movementPath.GetPoint(t)));
        }

        private void OnFireBullets(GameTime gameTime)
        {
            Scene.Loaded.ECS.SetComponentInEntity(spawnerEntity, new TransformComponent(Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position));
            EntitySpawnerBehaviour entitySpawner = (EntitySpawnerBehaviour)(Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(spawnerEntity).Behaviour);
            entitySpawner.SpawnEntitiesWithPattern(new DirectFiringPattern(), gameTime, timerContainer);
            //call on spawner to spawn bullets
        }

        public override State Clone()
        {
            var clone = new BirdEnemyState(movementPath, attackPaths);
            return clone;
        }
    }
}

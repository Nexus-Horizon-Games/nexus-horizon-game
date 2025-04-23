using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Timers;
using Nexus_Horizon_Game.Paths;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.EntityPatterns;
using Nexus_Horizon_Game.Model.EntityFactory;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.States
{
    internal class CatEnemyState : State
    {
        private float FireRate = 0.5f;
        private float t = 0;
        private float speed = 0.3f;
        private TimerContainer timerContainer = new TimerContainer();
        private MultiPath movementPath;
        private List<int> attackPaths;
        private int spawnerEntity;
        private IFiringPattern firingPattern;

        private Tag bulletsTag;

        public CatEnemyState(MultiPath movementPath, List<int> attackPaths, Tag bulletsTag = 0, IFiringPattern? firingPattern = null)
        {
            this.movementPath = movementPath;
            this.attackPaths = attackPaths;
            this.bulletsTag = bulletsTag;
            this.firingPattern = firingPattern ?? new TriangleFiringPattern(7.0f);
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
            var position = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position;
            
            Scene.Loaded.ECS.SetComponentInEntity(spawnerEntity, new TransformComponent(Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position));
            EntitySpawnerBehaviour entitySpawner = (EntitySpawnerBehaviour)(Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(spawnerEntity).Behaviour);
            entitySpawner.SpawnEntitiesWithPattern(firingPattern, gameTime, timerContainer);
        }

        public override State Clone()
        {
            var clone = new CatEnemyState(movementPath, attackPaths, firingPattern: firingPattern);
            return clone;
        }
    }
}

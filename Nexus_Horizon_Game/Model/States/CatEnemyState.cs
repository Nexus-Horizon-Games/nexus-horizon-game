using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Timers;
using Nexus_Horizon_Game.Paths;
using System;
using System.Linq;
using Nexus_Horizon_Game.Pooling;
using System.Diagnostics;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.EntityPatterns;
using Nexus_Horizon_Game.Model.EntityFactory;

namespace Nexus_Horizon_Game.States
{
    internal class CatEnemyState : State
    {
        private float FireRate = 0.5f;
        private float t;
        private float waitTime = 0;
        private float speed = 0.3f;
        private BulletFactory bulletFactory = new BulletFactory("BulletSample");
        private TimerContainer timerContainer = new TimerContainer();
        private Vector2[] attackPoints;
        private MultiPath movementPath;
        private int[] attackPaths;
        private float health = 0.7f;
        private bool isFiring = false;
        private bool isMoving = false;
        private int spawnerEntity;

        public CatEnemyState(int thisEntity, MultiPath movementPath, int[] attackPaths, float waitTime) : base(thisEntity)
        {
            this.movementPath = movementPath;
            this.attackPaths = attackPaths;
            this.waitTime = waitTime;
        }

        public CatEnemyState(MultiPath movementPath, int[] attackPaths, float waitTime)
        {
            this.movementPath = movementPath;
            this.attackPaths = attackPaths;
            this.waitTime = waitTime;
        }

        public override void OnStart()
        {
            isMoving = false;
            this.spawnerEntity = EntitySpawnerFactory.CreateBulletSpawner("BulletSample");
        }
        
        public override void OnUpdate(GameTime gameTime)
        {
            timerContainer.Update(gameTime);

            if (!isMoving)
            {
                if (waitTime > 0)
                {
                    Scene.Loaded.ECS.SetComponentInEntity(this.Entity, new TransformComponent(new Vector2(-100, -100)));
                    waitTime -= 0.01f;
                    return;
                }
                t = 0;
                timerContainer.AddTimer(new LoopTimer(FireRate, (gameTime, data) => OnFireBullets(gameTime)), "fire");
                isMoving = true;
            }
            else
            {
                MovingState(gameTime);
            }
        }

        private void MovingState(GameTime gameTime)
        {
            if (t >= 1) 
            {
                EndState(gameTime);
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



        private void EndState(GameTime gameTime)
        {
            OnStop();
        }

        private void OnFireBullets(GameTime gameTime)
        {
            var position = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position;
            
            Scene.Loaded.ECS.SetComponentInEntity(spawnerEntity, new TransformComponent(Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position));
            EntitySpawner entitySpawner = (EntitySpawner)(Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(spawnerEntity).Behaviour);
            entitySpawner.SpawnEntitiesWithPattern(new TriangleFiringPattern(), gameTime, timerContainer);
        }

        public override State Clone()
        {
            var clone = new CatEnemyState(movementPath, attackPaths, waitTime);
            return clone;
        }
    }
}

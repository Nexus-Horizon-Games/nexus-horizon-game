using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.EntityFactory;
using Nexus_Horizon_Game.Model.EntityPatterns;
using Nexus_Horizon_Game.Model.Prefab;
using Nexus_Horizon_Game.Paths;
using Nexus_Horizon_Game.Timers;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.States
{
    internal class GruntEnemyState : State
    {
        private float fireRate;
        private float speed;
        private MultiPath movementPath;
        private List<int> attackPaths;
        private IFiringPattern firingPattern;
        private PrefabEntity projectile;
        int animBuffer = 0;
        private TimerContainer timerContainer = new TimerContainer();
        private int spawnerEntity;
        private float t = 0;

        public GruntEnemyState(MultiPath movementPath, List<int> attackPaths, float speed, float fireRate, IFiringPattern? firingPattern = null, PrefabEntity? projectile = null)
        {
            this.movementPath = movementPath;
            this.attackPaths = attackPaths;
            this.speed = speed;
            this.fireRate = fireRate;
            this.firingPattern = firingPattern ?? new DirectFiringPattern(7.0f);
            this.projectile = projectile ?? new PrefabEntity(new List<IComponent>
            {
                new TransformComponent(Vector2.Zero),
                new SpriteComponent("BulletSample", color: Color.White, scale: 0.25f, spriteLayer: 0, centered: true),
                new TagComponent(Tag.ENEMY_PROJECTILE),
            });
        }

        public override void OnStart()
        {
            this.spawnerEntity = EntitySpawnerFactory.CreateEntitySpawner(projectile);
            Scene.Loaded.ECS.SetComponentInEntity(this.Entity, new TransformComponent(new Vector2(-100, -100)));
            timerContainer.AddTimer(new LoopTimer(fireRate, (gameTime, data) => OnFireBullets(gameTime)), "fire");
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
                incrementAnimationBuffered();
            }
            else
            {
                if (timerContainer.GetTimer("fire").IsOn)
                {
                    timerContainer.GetTimer("fire").Stop();
                }
                decrementAnimationBuffered();

            }

            t += movementPath.GetDeltaT(t, speed);
            Scene.Loaded.ECS.SetComponentInEntity(this.Entity, new TransformComponent(movementPath.GetPoint(t)));
        }

        private void OnFireBullets(GameTime gameTime)
        {
            Scene.Loaded.ECS.SetComponentInEntity(spawnerEntity, new TransformComponent(Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position));
            EntitySpawnerBehaviour entitySpawner = (EntitySpawnerBehaviour)(Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(spawnerEntity).Behaviour);
            entitySpawner.SpawnEntitiesWithPattern(firingPattern, gameTime, timerContainer);
        }

        public override State Clone()
        {
            var clone = new GruntEnemyState(movementPath, attackPaths, speed, fireRate, firingPattern: firingPattern, projectile: projectile);
            return clone;
        }
    }
}

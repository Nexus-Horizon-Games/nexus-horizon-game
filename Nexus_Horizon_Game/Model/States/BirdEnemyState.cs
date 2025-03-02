using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Paths;
using Nexus_Horizon_Game.Timers;
using System;
using System.Linq;

namespace Nexus_Horizon_Game.States
{
    internal class BirdEnemyState : State
    {
        private float FireRate = 0.2f;
        private float t;
        private float waitTime = 0;
        private float speed = 0.4f;
        private BulletFactory bulletFactory = new BulletFactory("BulletSample");
        private TimerContainer timerContainer = new TimerContainer();
        private Vector2[] attackPoints;
        private MultiPath movementPath;
        private int[] attackPaths;
        private float health = 0.5f;
        private bool isMoving = false;

        public BirdEnemyState(int thisEntity, MultiPath movementPath, int[] attackPaths, float waitTime) : base(thisEntity)
        {
            this.movementPath = movementPath;
            this.attackPaths = attackPaths;
            this.waitTime = waitTime;
        }

        public override void OnStart()
        {
            isMoving = false;
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
            var playerPosition = GetPlayerPosition();
            double direction = Math.Atan2((double)(playerPosition.Y - position.Y), (double)(playerPosition.X - position.X));
            Vector2 bulletDirection = GetVectFromDirection(direction, 0);
            bulletFactory.CreateEntity(position, bulletDirection, 7f);
        }
        private Vector2 GetVectFromDirection(double direction, double variation)
        {
            direction += variation;
            float xComponent = (float)(Math.Cos(direction));
            float yComponent = (float)(Math.Sin(direction));
            return new Vector2(xComponent, yComponent);
        }

        private Vector2 GetPlayerPosition()
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

            return playerPosition;
        }
    }
}

using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Timers;
using Nexus_Horizon_Game.Paths;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal class CatEnemyBehaviour : Behaviour
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

        public enum CatEnemyState : int
        {
            None,
            Start,
            Moving,
            End,
        }

        public CatEnemyBehaviour(int thisEntity, MultiPath movementPath, int[] attackPaths, float waitTime) : base(thisEntity)
        {
            this.movementPath = movementPath;
            this.attackPaths = attackPaths;
            this.waitTime = waitTime;
        }

        
        public override void OnUpdate(GameTime gameTime)
        {
            var state = GameM.CurrentScene.World.GetComponentFromEntity<StateComponent>(this.Entity);
            timerContainer.Update(gameTime);

            /*switch ((CatEnemyState)state.state)
            {
                case CatEnemyState.Start:
                    StartState();
                    break;
                case CatEnemyState.Moving:
                    MovingState(gameTime);
                    break;
                case CatEnemyState.End:
                    EndState(gameTime);
                    break;
            }*/
        }

        private void StartState()
        {
            if (waitTime > 0)
            {
                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new TransformComponent(new Vector2(-100, -100)));
                waitTime -= 0.01f;
                return;
            }
            t = 0;
            timerContainer.AddTimer(new LoopTimer(FireRate, (gameTime, data) => OnFireBullets(gameTime)), "fire");
            //GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(CatEnemyState.Moving));
        }
        private void MovingState(GameTime gameTime)
        {
            if (t >= 1) 
            {
                //GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(CatEnemyState.End));
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
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new TransformComponent(movementPath.GetPoint(t)));
        }



        private void EndState(GameTime gameTime)
        {
            if(!isFiring)
            {
                GameM.CurrentScene.World.DestroyEntity(this.Entity);
            }
        }

        private void OnFireBullets(GameTime gameTime)
        {
            var position = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;
            var playerPosition = GetPlayerPosition();
           
            float bulletSpeed = 5f;
            float timeInterval = 0.08f;
            int bulletNum = 1;

            timerContainer.StartTemporaryTimer(new LoopTimer(timeInterval, (gameTime, data) =>
            {
                isFiring = true;
                double startTime = (double)data;
                double time = gameTime.TotalGameTime.TotalSeconds - startTime;
                double direction = Math.Atan2((double)(playerPosition.Y - position.Y), (double)(playerPosition.X - position.X));
                if (bulletNum == 1)
                {
                    Vector2 bulletDirection = GetVectFromDirection(direction, 0);
                    bulletFactory.CreateEntity(position, bulletDirection, bulletSpeed);
                }
                if (bulletNum == 2)
                {
                    Vector2 bulletDirection = GetVectFromDirection(direction, MathHelper.ToRadians(1));
                    bulletFactory.CreateEntity(position, bulletDirection, bulletSpeed);
                    bulletDirection = GetVectFromDirection(direction, MathHelper.ToRadians(-1));
                    bulletFactory.CreateEntity(position, bulletDirection, bulletSpeed);
                }
                if (bulletNum == 3)
                {
                    Vector2 bulletDirection = GetVectFromDirection(direction, 0);
                    bulletFactory.CreateEntity(position, bulletDirection, bulletSpeed);
                    bulletDirection = GetVectFromDirection(direction, MathHelper.ToRadians(2));
                    bulletFactory.CreateEntity(position, bulletDirection, bulletSpeed);
                    bulletDirection = GetVectFromDirection(direction, MathHelper.ToRadians(-2));
                    bulletFactory.CreateEntity(position, bulletDirection, bulletSpeed);
                    isFiring = false;
                }
                bulletNum++;
            }, data: gameTime.TotalGameTime.TotalSeconds, stopAfter: timeInterval*4));
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
            var entitesWithTag = GameM.CurrentScene.World.GetEntitiesWithComponent<TagComponent>();
            var playerEntity = -1;
            foreach (var entity in entitesWithTag)
            {
                var tag = GameM.CurrentScene.World.GetComponentFromEntity<TagComponent>(entity);
                if (tag.Tag == Tag.PLAYER)
                {
                    playerEntity = entity;
                    break;
                }
            }

            Vector2 playerPosition = Vector2.Zero;
            if (playerEntity != -1)
            {
                playerPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(playerEntity).position;
            }

            return playerPosition;
        }
    }
}

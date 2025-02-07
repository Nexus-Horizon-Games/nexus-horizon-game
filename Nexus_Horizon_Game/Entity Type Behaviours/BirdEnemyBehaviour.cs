using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Timers;
using Nexus_Horizon_Game.Paths;
using System.Collections.Generic;
using System.Collections;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal class BirdEnemyBehaviour : Behaviour
    {
        private float Speed = 8.0f;
        private float AttackSpeed = 5.0f;
        private float LeaveSpeed = 10.0f;
        private float FireRate = 0.2f;
        private float t;
        private float waitTime = 0;
        private BulletFactory bulletFactory = new BulletFactory("BulletSample");
        private TimerContainer timerContainer = new TimerContainer();
        private Vector2[] attackPoints;
        private Dictionary<BirdEnemyState, IPath> pathPattern = new Dictionary<BirdEnemyState, IPath>();

        public enum BirdEnemyState : int
        {
            None,
            Start,
            EnteringArena,
            Attacking,
            LeavingArena
        }

        public BirdEnemyBehaviour(int thisEntity, Vector2[] attackPoints, float waitTime) : base(thisEntity) 
        {
            this.attackPoints = attackPoints;
            this.waitTime = waitTime;
        }

        public override void OnUpdate(GameTime gameTime)
        {
            var state = GameM.CurrentScene.World.GetComponentFromEntity<StateComponent>(this.Entity);
            timerContainer.Update(gameTime);

            switch ((BirdEnemyState)state.state)
            {
                case BirdEnemyState.Start:
                    StartState();
                    break;
                case BirdEnemyState.EnteringArena:
                    EnteringArenaState(gameTime);
                    break;
                case BirdEnemyState.Attacking:
                    AttackingState(gameTime);
                    break;
                case BirdEnemyState.LeavingArena:
                    LeavingState(gameTime);
                    break;
            }
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
            setAttackPoints();
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(BirdEnemyState.EnteringArena));
        }

        private void setAttackPoints()
        {
            pathPattern[BirdEnemyState.EnteringArena] = new QuadraticCurvePath(attackPoints[0], new Vector2(attackPoints[0].X, attackPoints[1].Y), attackPoints[1]);
            pathPattern[BirdEnemyState.Attacking] = new QuadraticCurvePath(attackPoints[1], new Vector2((attackPoints[1].X + attackPoints[2].X) / 2, ((attackPoints[1].Y + attackPoints[2].Y) / 2)), attackPoints[2]);
            pathPattern[BirdEnemyState.LeavingArena] = new QuadraticCurvePath(attackPoints[2], new Vector2(attackPoints[3].X, attackPoints[2].Y), attackPoints[3]);
        }
        private void EnteringArenaState(GameTime gameTime)
        {
            if (t >= 1) 
            {
                timerContainer.AddTimer(new LoopTimer(FireRate, (gameTime, data) => OnFireBullets(gameTime)), "fire");
                timerContainer.GetTimer("fire").Start();
                t = 0;
                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(BirdEnemyState.Attacking));
            }
            else
            {
                t += 0.01f;
                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new TransformComponent(pathPattern[BirdEnemyState.EnteringArena].GetPoint(t)));
            }
        }

        private void AttackingState(GameTime gameTime)
        {
            if (t >= 1)
            {
                timerContainer.GetTimer("fire").Stop();
                t = 0;
                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(BirdEnemyState.LeavingArena));
            }
            else
            {
                t += 0.01f;
                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new TransformComponent(pathPattern[BirdEnemyState.Attacking].GetPoint(t)));
            }
        }


        private void LeavingState(GameTime gameTime)
        {
            if (t >= 1)
            {
                GameM.CurrentScene.World.DestroyEntity(this.Entity);
            }
            else
            {
                t += 0.01f;
                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new TransformComponent(pathPattern[BirdEnemyState.LeavingArena].GetPoint(t)));
            }
        }

        private void OnFireBullets(GameTime gameTime)
        {
            var position = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;
            bulletFactory.CreateEntity(position, new Vector2(0, 1), 10.0f);
        }
    }
}

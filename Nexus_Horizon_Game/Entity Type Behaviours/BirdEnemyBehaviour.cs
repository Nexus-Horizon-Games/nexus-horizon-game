using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Timers;
using Nexus_Horizon_Game.Paths;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal class BirdEnemyBehaviour : Behaviour
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

        public enum BirdEnemyState : int
        {
            None,
            Start,
            Moving,
            End,
        }

        public BirdEnemyBehaviour(int thisEntity, MultiPath movementPath, int[] attackPaths, float waitTime) : base(thisEntity)
        {
            this.movementPath = movementPath;
            this.attackPaths = attackPaths;
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
                case BirdEnemyState.Moving:
                    MovingState(gameTime);
                    break;
                case BirdEnemyState.End:
                    EndState(gameTime);
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
            timerContainer.AddTimer(new LoopTimer(FireRate, (gameTime, data) => OnFireBullets(gameTime)), "fire");
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(BirdEnemyState.Moving));
        }
        private void MovingState(GameTime gameTime)
        {
            if (t >= 1) 
            {
                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(BirdEnemyState.End));
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
               GameM.CurrentScene.World.DestroyEntity(this.Entity);
        }

        private void OnFireBullets(GameTime gameTime)
        {
            var position = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;
            bulletFactory.CreateEntity(position, new Vector2(0, 1), 10.0f);
        }
    }
}

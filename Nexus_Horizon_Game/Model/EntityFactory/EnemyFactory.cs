using Nexus_Horizon_Game.Components;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Paths;
using Nexus_Horizon_Game.States;
using System.Transactions;

namespace Nexus_Horizon_Game.EntityFactory
{
    internal static class EnemyFactory
    {
        public static MultiPath sampleBirdPath1()
        {
            Vector2 point1 = new Vector2(0, 0);
            Vector2 point2 = new Vector2(0, 44);
            Vector2 point3 = new Vector2(44, 44);
            Vector2 point4 = new Vector2(132, 44);
            Vector2 point5 = new Vector2(176, 44);
            Vector2 point6 = new Vector2(176, 0);
            QuadraticCurvePath enteringPath = new QuadraticCurvePath(point1, point2, point3);
            LinePath straightPath = new LinePath(point3, point4);
            QuadraticCurvePath leavingPath = new QuadraticCurvePath(point4, point5, point6);
            List<IPath> pathList = new List<IPath>();
            pathList.Add(enteringPath);
            pathList.Add(straightPath);
            pathList.Add(leavingPath);
            MultiPath movementPath = new MultiPath(pathList);
            return movementPath;
        }

        public static MultiPath sampleBirdPath2(float startX)
        {
            Vector2 point1 = new Vector2(startX, 0);
            Vector2 point2 = new Vector2(startX, 44);
            LinePath enteringPath = new LinePath(point1, point2);
            WaitPath waitingPath = new WaitPath(point2, 40);
            LinePath leavingPath = new LinePath(point2, point1);
            List<IPath> pathList = new List<IPath>();
            pathList.Add(enteringPath);
            pathList.Add(waitingPath);
            pathList.Add(leavingPath);
            MultiPath movementPath = new MultiPath(pathList);
            return movementPath;
        }

        public static MultiPath sampleCatPath1(float startX)
        {
            Vector2 point1 = new Vector2(startX, 0);
            Vector2 point2 = new Vector2(startX, 85);
            Vector2 point3 = new Vector2(0, 150);
            LinePath enteringPath = new LinePath(point1, point2);
            WaitPath waitingPath = new WaitPath(point2, 90);
            LinePath leavingPath = new LinePath(point2, point3);
            List<IPath> pathList = new List<IPath>();
            pathList.Add(enteringPath);
            pathList.Add(waitingPath);
            pathList.Add(leavingPath);
            MultiPath movementPath = new MultiPath(pathList);
            return movementPath;
        }

        public static MultiPath sampleCatPath2(float startX)
        {
            Vector2 point1 = new Vector2(startX, 0);
            Vector2 point2 = new Vector2(startX, 85);
            Vector2 point3 = new Vector2(176, 150);
            LinePath enteringPath = new LinePath(point1, point2);
            WaitPath waitingPath = new WaitPath(point2, 90);
            LinePath leavingPath = new LinePath(point2, point3);
            List<IPath> pathList = new List<IPath>();
            pathList.Add(enteringPath);
            pathList.Add(waitingPath);
            pathList.Add(leavingPath);
            MultiPath movementPath = new MultiPath(pathList);
            return movementPath;
        }

        public static int CreateEnemy(string type, MultiPath multiPath, int[] attackPaths, float waitTime)
        {
            int enemyEntity = Scene.Loaded.ECS.CreateEntity(new List<IComponent>
            {
                new TransformComponent(new Vector2(0.0f, 0.0f)),
                new PhysicsBody2DComponent(),
            });
            
            if (type == "bird_enemy")
            {
                Scene.Loaded.ECS.AddComponent(enemyEntity, new SpriteComponent("bird", centered: true));
                Scene.Loaded.ECS.AddComponent(enemyEntity, new StateComponent(new List<State>
                {
                    new BirdEnemyState(enemyEntity, multiPath, attackPaths, waitTime)
                }));
            }
            if (type == "cat_enemy")
            {
                Scene.Loaded.ECS.AddComponent(enemyEntity, new SpriteComponent("cat", centered: true));
                Scene.Loaded.ECS.AddComponent(enemyEntity, new StateComponent(new List<State>
                {
                    new CatEnemyState(enemyEntity, multiPath, attackPaths, waitTime)
                }));

                Scene.Loaded.ECS.AddComponent(enemyEntity, new HealthComponent(3, () =>
                {
                    var stateComp = Scene.Loaded.ECS.GetComponentFromEntity<StateComponent>(enemyEntity);

                    // Remove states after the current state
                    if (stateComp.currentState + 1 != stateComp.states.Count)
                    {
                        stateComp.states.RemoveRange(stateComp.currentState + 1, stateComp.states.Count - stateComp.currentState + 1);
                    }

                    // Set to death state:
                    stateComp.states.Add(new DeathState(enemyEntity));
                    stateComp.states[stateComp.currentState].OnStop();

                    //Scene.Loaded.ECS.SetComponentInEntity<StateComponent>(enemyEntity, stateComp);
                }));
            }

            // create enemy
            return enemyEntity;
        }


        private const float EnteringSpeed = 30.0f;
        private const float Stage1Length = 35.0f;
        private const float Stage2Length = 30.0f;

        public static int CreateBoss(string type) // SCENE CAN BE the specfic Game scene since it'll change between menu's
        {
            int bossEntity = Scene.Loaded.ECS.CreateEntity(new List<IComponent>
            {
                new TransformComponent(new Vector2(Arena.Size.X / 2.0f, -20.0f)),
                new PhysicsBody2DComponent(accelerationEnabled: true),
            });

            if (type == "evil_guinea_pig_boss") // mid boss
            {
                Scene.Loaded.ECS.AddComponent(bossEntity, new SpriteComponent("evil_guinea_pig", centered: true));
                Scene.Loaded.ECS.AddComponent(bossEntity, new StateComponent(new List<State>
                {
                    new MoveToPointState(bossEntity, new Vector2(Arena.Size.X / 2.0f, 40.0f), EnteringSpeed),
                    new GuineaPigBossState(bossEntity, 15.0f),
                    new MoveToPointState(bossEntity, new Vector2(Arena.Size.X / 2.0f, -20.0f), EnteringSpeed),
                }));
            }
            else if (type == "chef_boss") // final boss
            {
                Scene.Loaded.ECS.AddComponent(bossEntity, new SpriteComponent("chef_boss", centered: true));
                Scene.Loaded.ECS.AddComponent(bossEntity, new StateComponent(new List<State>
                {
                    new MoveToPointState(bossEntity, new Vector2(Arena.Size.X / 2.0f, 40.0f), EnteringSpeed),
                    new ChefBossStage1State(bossEntity, Stage1Length),
                    new ChefBossStage2State(bossEntity, Stage2Length),
                    new MoveToPointState(bossEntity, new Vector2(Arena.Size.X / 2.0f, -20.0f), EnteringSpeed),
                }));
            }

            return bossEntity;
        }
    }
}

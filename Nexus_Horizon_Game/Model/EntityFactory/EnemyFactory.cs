using Nexus_Horizon_Game.Components;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Paths;
using Nexus_Horizon_Game.States;
using Nexus_Horizon_Game.Model.Prefab;

namespace Nexus_Horizon_Game.EntityFactory
{
    internal static class EnemyFactory
    {
        /// <summary>
        /// Called when an enemy dies.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public static void SetToDeathState(int entity)
        {
            var stateComp = Scene.Loaded.ECS.GetComponentFromEntity<StateComponent>(entity);

            // Add death state
            stateComp.states.Add(new DeathState(entity));

            // Set transtion from current state to death state
            stateComp.transitionFunction = stateComp.UseDictionaryTransitionFunction;
            stateComp.transitions[stateComp.currentState] = stateComp.states.Count - 1;

            // Stop current state to transition to death state
            stateComp.states[stateComp.currentState].OnStop();

            Scene.Loaded.ECS.SetComponentInEntity(entity, stateComp);
        }

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

        public static PrefabEntity CreateEnemyPrefab(string type, MultiPath multiPath, int[] attackPaths, float waitTime)
        {
            PrefabEntity prefabEntity = new PrefabEntity(new List<IComponent>
            {
                new TransformComponent(new Vector2(0.0f, 0.0f)),
                new PhysicsBody2DComponent(),
                new ColliderComponent(new Point(type == "bird_enemy" ? 10 : 10, type == "bird_enemy" ? 6 : 6)), // Needs to have a set entityID following or else it wont know where transform is
                new TagComponent(Tag.ENEMY)
            });

            if (type == "bird_enemy")
            {
                prefabEntity.Components.Add(new SpriteComponent("bird", centered: true));
                prefabEntity.Components.Add(new TagComponent(Tag.ENEMY | Tag.SMALLGRUNT));
                prefabEntity.Components.Add(new StateComponent(new List<State>
                {
                    new ChefBossStage1State(30.0f),
                    new BirdEnemyState(multiPath, attackPaths, waitTime)
                }));

                prefabEntity.Components.Add(new HealthComponent(1, (entity) =>
                {
                    SetToDeathState(entity);
                }));
            }

            if (type == "cat_enemy")
            {
                prefabEntity.Components.Add(new SpriteComponent("cat", centered: true));
                prefabEntity.Components.Add(new TagComponent(Tag.ENEMY | Tag.MEDIUMGRUNT));
                prefabEntity.Components.Add(new StateComponent(new List<State>
                {
                    new CatEnemyState(multiPath, attackPaths, waitTime)
                }));

                prefabEntity.Components.Add(new HealthComponent(7, (entity) =>
                {
                    SetToDeathState(entity);
                }));
            }

            return prefabEntity;
        }

        public static int CreateEnemy(string type, MultiPath multiPath, int[] attackPaths, float waitTime)
        {
            int enemyEntity = Scene.Loaded.ECS.CreateEntity(new List<IComponent>
            {
                new TransformComponent(new Vector2(0.0f, 0.0f)),
                new PhysicsBody2DComponent(),
            });

            Scene.Loaded.ECS.AddComponent<ColliderComponent>(enemyEntity, new ColliderComponent(new Point(type == "bird_enemy" ? 10 : 10, type == "bird_enemy" ? 6 : 6), entityIDFollowing: enemyEntity));

            if (type == "bird_enemy")
            {
                Scene.Loaded.ECS.AddComponent(enemyEntity, new SpriteComponent("bird", centered: true));
                Scene.Loaded.ECS.AddComponent(enemyEntity, new TagComponent(Tag.ENEMY | Tag.SMALLGRUNT));
                Scene.Loaded.ECS.AddComponent(enemyEntity, new StateComponent(new List<State>
                {
                    new BirdEnemyState(enemyEntity, multiPath, attackPaths, waitTime, bulletsTag: Tag.ENEMY_PROJECTILE)
                }));

                Scene.Loaded.ECS.AddComponent(enemyEntity, new HealthComponent(1, (_) =>
                {
                    SetToDeathState(enemyEntity);
                }));
            }
            if (type == "cat_enemy")
            {
                Scene.Loaded.ECS.AddComponent(enemyEntity, new SpriteComponent("cat", centered: true));
                Scene.Loaded.ECS.AddComponent(enemyEntity, new TagComponent(Tag.ENEMY | Tag.MEDIUMGRUNT));
                Scene.Loaded.ECS.AddComponent(enemyEntity, new StateComponent(new List<State>
                {
                    new CatEnemyState(enemyEntity, multiPath, attackPaths, waitTime, bulletsTag : Tag.ENEMY_PROJECTILE)
                }));

                Scene.Loaded.ECS.AddComponent(enemyEntity, new HealthComponent(7, (_) =>
                {
                    SetToDeathState(enemyEntity);
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
                Scene.Loaded.ECS.AddComponent(bossEntity, new TagComponent(Tag.ENEMY | Tag.HALFBOSS));
                // added hitbox cetering on sprite
                Scene.Loaded.ECS.AddComponent<ColliderComponent>(bossEntity, new ColliderComponent(new Point(17, 17), new Point(-8, -8), entityIDFollowing: bossEntity));
                Scene.Loaded.ECS.AddComponent(bossEntity, new StateComponent(new List<State>              
                {
                    new MoveToPointState(bossEntity, new Vector2(Arena.Size.X / 2.0f, 40.0f), EnteringSpeed),
                    new GuineaPigBossState(bossEntity, 15.0f, bulletsTag : Tag.ENEMY_PROJECTILE),
                    new MoveToPointState(bossEntity, new Vector2(Arena.Size.X / 2.0f, -20.0f), EnteringSpeed),
                }));

                Scene.Loaded.ECS.AddComponent(bossEntity, new HealthComponent(101, (_) =>
                {
                    SetToDeathState(bossEntity);
                }));
            }
            else if (type == "chef_boss") // final boss
            {
                Scene.Loaded.ECS.AddComponent(bossEntity, new SpriteComponent("chef_boss", centered: true));
                Scene.Loaded.ECS.AddComponent(bossEntity, new TagComponent(Tag.ENEMY | Tag.BOSS));
                // added hitbox cetering on sprite
                Scene.Loaded.ECS.AddComponent<ColliderComponent>(bossEntity, new ColliderComponent(new Point(12, 18), new Point(-6, -9), entityIDFollowing: bossEntity));
                Scene.Loaded.ECS.AddComponent(bossEntity, new StateComponent(new List<State>
                {
                    new MoveToPointState(bossEntity, new Vector2(Arena.Size.X / 2.0f, 40.0f), EnteringSpeed),
                    new ChefBossStage1State(Stage1Length, bulletsTag : Tag.ENEMY_PROJECTILE),
                    new ChefBossStage2State(bossEntity, Stage2Length, bulletsTag : Tag.ENEMY_PROJECTILE),
                    new MoveToPointState(bossEntity, new Vector2(Arena.Size.X / 2.0f, -20.0f), EnteringSpeed),
                }));

                Scene.Loaded.ECS.AddComponent(bossEntity, new HealthComponent(164, (_) =>
                {
                    SetToDeathState(bossEntity);
                }));
            }

            return bossEntity;
        }
    }
}

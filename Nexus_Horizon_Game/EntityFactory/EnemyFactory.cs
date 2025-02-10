using Nexus_Horizon_Game.Components;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Paths;

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

        public static int CreateEnemy(string type, MultiPath multiPath, int[] attackPaths, float waitTime)
        {
            int enemyEntity = GameM.CurrentScene.World.CreateEntity(new List<IComponent>
            {
                new TransformComponent(new Vector2(0.0f, 0.0f)),
                new PhysicsBody2DComponent(),
            });
            
            if (type == "bird_enemy")
            {
                GameM.CurrentScene.World.AddComponent(enemyEntity, new SpriteComponent("bird", centered: true));
                GameM.CurrentScene.World.AddComponent(enemyEntity, new BehaviourComponent(new BirdEnemyBehaviour(enemyEntity, multiPath, attackPaths, waitTime)));
                GameM.CurrentScene.World.AddComponent(enemyEntity, new StateComponent(BirdEnemyBehaviour.BirdEnemyState.Start));
            }
           

            // create enemy
            return enemyEntity;
        }


        public static int CreateBoss(string type)
        {
            int bossEntity = GameM.CurrentScene.World.CreateEntity(new List<IComponent>
            {
                new TransformComponent(new Vector2(0.0f, 0.0f)),
                new PhysicsBody2DComponent(accelerationEnabled: true),
            });

            if (type == "evil_guinea_pig_boss") // mid boss
            {
                GameM.CurrentScene.World.AddComponent(bossEntity, new SpriteComponent("evil_guinea_pig", centered: true));
                GameM.CurrentScene.World.AddComponent(bossEntity, new BehaviourComponent(new GuineaPigMidBossBehaviour(bossEntity)));
                GameM.CurrentScene.World.AddComponent(bossEntity, new StateComponent(GuineaPigMidBossBehaviour.GuineaPigMidBossState.Start));
            }
            else if (type == "chef_boss") // final boss
            {
                GameM.CurrentScene.World.AddComponent(bossEntity, new SpriteComponent("chef_boss", centered: true));
                GameM.CurrentScene.World.AddComponent(bossEntity, new BehaviourComponent(new ChefBossBehaviour(bossEntity)));
                GameM.CurrentScene.World.AddComponent(bossEntity, new StateComponent(ChefBossBehaviour.ChefBossState.Start));
            }

            return bossEntity;
        }
    }
}

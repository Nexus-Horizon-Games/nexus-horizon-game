
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.EntityFactory;


namespace Nexus_Horizon_Game
{
    internal static class SceneLoader
    {
        public static void LoadScene(ref Scene currentScene)
        {
            currentScene = new Scene();
            // TODO: parse the scene from JSON here

            var playerFactory = new PlayerFactory();
            int moveablePlayer2 = playerFactory.CreateEntity();

            //EnemyFactory.CreateBoss("chef_boss");
            Vector2 point1 = new Vector2(0,0);
            Vector2 point2 = new Vector2(44, 50);
            Vector2 point3 = new Vector2(132, 50);
            Vector2 point4 = new Vector2(176, 0);
            Vector2[] attackPoints = [point1, point2, point3, point4];
            float waitTime = 1;
            for (int i = 0; i < 10; i++)
            {
                EnemyFactory.CreateEnemy("bird_enemy", attackPoints, waitTime + i/5.0f);
            }
            waitTime += 4;
            for (int i = 0; i < 8; i++)
            {
                Vector2 point5 = new Vector2(i*25, 0);
                Vector2 point6 = new Vector2(i*25, 50);
                Vector2 point7 = new Vector2(i*25, 50);
                Vector2 point8 = new Vector2(i*25, 0);
                Vector2[] attackPoints2 = [point5, point6, point7, point8];
                EnemyFactory.CreateEnemy("bird_enemy", attackPoints2, waitTime + i / 7.0f);
            }

        }
    }
}

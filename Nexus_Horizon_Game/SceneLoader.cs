
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
            float waitTime = 1;
            int[] attack = { 1 };
            int[] catattack = { 1, 2 };
            for (int i = 0; i < 10; i++)
            {
                EnemyFactory.CreateEnemy("bird_enemy", EnemyFactory.sampleBirdPath1(),  attack, waitTime + i / 5.0f);
            }

            waitTime += 4;

            for (int i = 0; i < 8; i++)
            {
                EnemyFactory.CreateEnemy("bird_enemy", EnemyFactory.sampleBirdPath2(i*25), attack, waitTime + i / 7.0f);
            }
            EnemyFactory.CreateEnemy("cat_enemy", EnemyFactory.sampleCatPath1(80), catattack, waitTime + 1f);
            EnemyFactory.CreateEnemy("cat_enemy", EnemyFactory.sampleCatPath2(100), catattack, waitTime + 1.6f);
        }
    }
}

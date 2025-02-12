
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Timers;
using System.Collections.Generic;
using System.Diagnostics;


namespace Nexus_Horizon_Game
{
    internal static class SceneLoader
    {
        private static List<DelayTimer> activeTimers = new List<DelayTimer>();
        public static void LoadScene(ref Scene currentScene)
        {
            currentScene = new Scene();
            // TODO: parse the scene from JSON here

            var playerFactory = new PlayerFactory();
            int moveablePlayer2 = playerFactory.CreateEntity();
            float waitTime = 1;
            int[] attack = { 1 };
            int[] catattack = { 1, 2 };


            waitTime += 2;

            for (int i = 0; i < 8; i++)
            {
                EnemyFactory.CreateEnemy("bird_enemy", EnemyFactory.sampleBirdPath2(i*25), attack, waitTime + i / 7.0f);
            }
            waitTime += 2;
            EnemyFactory.CreateEnemy("cat_enemy", EnemyFactory.sampleCatPath1(80), catattack, waitTime + 1f);
            EnemyFactory.CreateEnemy("cat_enemy", EnemyFactory.sampleCatPath2(100), catattack, waitTime + 1.6f);
            waitTime += 3;
            for (int i = 0; i < 10; i++)
            {
                EnemyFactory.CreateEnemy("bird_enemy", EnemyFactory.sampleBirdPath1(), attack, waitTime + i / 5.0f);
            }


            // Mid-boss Timer
            waitTime += 2;
            DelayTimer midBossTimer = new DelayTimer(waitTime + 7f, (gameTime, data) =>
            {
                EnemyFactory.CreateBoss("evil_guinea_pig_boss");
            });
            midBossTimer.Start();
            activeTimers.Add(midBossTimer);


            // New Enemies
            waitTime += 15;

            for (int i = 0; i < 6; i++)
            {
                EnemyFactory.CreateEnemy("bird_enemy", EnemyFactory.sampleBirdPath2(i * 30), attack, waitTime + i / 6.0f);
            }

            EnemyFactory.CreateEnemy("cat_enemy", EnemyFactory.sampleCatPath1(80), catattack, waitTime + 1f);
            EnemyFactory.CreateEnemy("cat_enemy", EnemyFactory.sampleCatPath2(100), catattack, waitTime + 1.6f);
            EnemyFactory.CreateEnemy("cat_enemy", EnemyFactory.sampleCatPath1(40), catattack, waitTime + 1f);
            EnemyFactory.CreateEnemy("cat_enemy", EnemyFactory.sampleCatPath2(120), catattack, waitTime + 1.6f);




            waitTime += 5;
            for (int i = 0; i < 7; i++)
            {
                EnemyFactory.CreateEnemy("bird_enemy", EnemyFactory.sampleBirdPath2(i * 20), attack, waitTime + i / 3.0f);
            }

           
            waitTime += 4;
            for (int i = 0; i < 10; i++)
            {
                EnemyFactory.CreateEnemy("bird_enemy", EnemyFactory.sampleBirdPath1(), attack, waitTime + i / 5.0f);
            }

            // Final Boss Timer
            waitTime += 4;
            DelayTimer finalBossTimer = new DelayTimer(waitTime + 30f, (gameTime, data) =>
            {
                EnemyFactory.CreateBoss("chef_boss");
            });
            finalBossTimer.Start();
            activeTimers.Add(finalBossTimer);
        }

        public static void Update(GameTime gameTime)
        {
            foreach (var timer in activeTimers)
            {
                timer.Update(gameTime);
            }
        }

    }
    
}

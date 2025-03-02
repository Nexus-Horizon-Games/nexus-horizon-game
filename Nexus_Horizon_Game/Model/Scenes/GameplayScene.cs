using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Model.Scenes
{
    internal class GameplayScene : Scene
    {
        public GameplayScene() : base() { }

        protected override void Initialize()
        {
            
        }

        protected override void LoadContent()
        {
            Renderer.LoadContent(new List<string> { "guinea_pig" });
        }

        /// <summary>
        /// Where we would load a JSON FILE THAT LOADS THE GAME SCENE
        /// </summary>
        protected override void LoadScene()
        {
            // BOSS TIMERS
            int mbt_entity = this.ECS.CreateEntity(new List<IComponent> { new TimersComponent(new Dictionary<string, Timer>()) } );

            var playerFactory = new PlayerFactory();
            int moveablePlayer2 = playerFactory.CreateEntity();
            float waitTime = 1;
            int[] attack = { 1 };
            int[] catattack = { 1, 2 };


            waitTime += 2;

            for (int i = 0; i < 8; i++)
            {
                EnemyFactory.CreateEnemy("bird_enemy", EnemyFactory.sampleBirdPath2(i * 25), attack, waitTime + i / 7.0f);
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
            this.ECS.EntityHasComponent(mbt_entity, out TimersComponent bossTimersComponent);
            bossTimersComponent.timers.Add("Mid-Boss", new DelayTimer(waitTime + 7f, (gameTime, data) =>
            {
                EnemyFactory.CreateBoss("evil_guinea_pig_boss");
            }));
            bossTimersComponent.timers["Mid-Boss"].Start();
            this.ECS.SetComponentInEntity(mbt_entity, bossTimersComponent);

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
            this.ECS.EntityHasComponent(mbt_entity, out bossTimersComponent);
            bossTimersComponent.timers.Add("Final-Boss", new DelayTimer(waitTime + 30f, (gameTime, data) =>
            {
                EnemyFactory.CreateBoss("chef_boss");
            }));
            bossTimersComponent.timers["Final-Boss"].Start();
            this.ECS.SetComponentInEntity(mbt_entity, bossTimersComponent);

            //EnemyFactory.CreateBoss("evil_guinea_pig_boss");
        }
    }
}

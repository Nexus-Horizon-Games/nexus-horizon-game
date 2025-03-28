using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Timers;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Model.EntityFactory;
using Nexus_Horizon_Game.Model.Components;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nexus_Horizon_Game.Model.Scenes
{
    internal class GameplayScene : Scene
    {
        private int pauseMenuUI;
        private static int deathMenuUI;
        private static int winMenuUI;
        private static int livesFontID;

        public GameplayScene() : base() { }

        public int PauseMenuUIID
        {
            get => pauseMenuUI;
        }

        public int DeathMenuUIID
        {
            get => deathMenuUI;
        }

        public int WinMenuUIID
        {
            get => winMenuUI;
        }

        public static void UpdateLiveFont(int livesLeft)
        {
            SpriteFontComponent spriteFontComponent = Scene.Loaded.ECS.GetComponentFromEntity<SpriteFontComponent>(livesFontID);
            spriteFontComponent.Text = $"Lives: {livesLeft}";
            Scene.Loaded.ECS.SetComponentInEntity(livesFontID, spriteFontComponent);
        }

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
            dynamic json = JsonConvert.DeserializeObject("""
{
  "movementTypes": [
    {
      "typeNameId": "BirdMovement",
      "movementType": "path",
      "pathType": "quadratic",
      "points": [
        [0.0, 0.0],
        [50.0, 0.0],
        [50.0, 100.0]
      ]
    }
  ],
  "entityTypes": [
    {
      "typeNameId": "CoalBullet",
      "components": {
        "TransformComponent": "default",
        "PhysicsBody2DComponent": "default",
        "SpriteComponent": {
          "textureName": "BulletSample"
        }
      }
    },
    {
      "typeNameId": "Bird",
      "components": {
        "TransformComponent": "default",
        "PhysicsBody2DComponent": "default",
        "SpriteComponent": {
          "textureName": "bird"
        }
      }
    },
    {
      "typeNameId": "MidBoss",
      "components": {
        "TransformComponent": "default",
        "PhysicsBody2DComponent": "default",
        "SpriteComponent": {
          "textureName": "evil_guinea_pig"
        },
        "StateComponent": {
          "states": [
            {
              "stateClassName": "MidBossState1",
              "constructor": ["BulletA"]
            },
            {
              "stateClassName": "MidBossState2"
            }
          ]
        }
      }
    },
    {
      "typeNameId": "FinalBoss",
      "components": {
        "TransformComponent": "default",
        "PhysicsBody2DComponent": "default",
        "SpriteComponent": {
          "textureName": "evil_guinea_pig"
        },
        "StateComponent": {
          "states": [
            {
              "stateClassName": "FinalBossState1",
              "constructor": ["BulletA"]
            },
            {
              "stateClassName": "FinalBossState2"
            }
          ]
        }
      }
    }
  ],
  "stages": [
    {
      "duration": 40,
      "spawners": [
        {
          "spawnerType": "multiple",
          "time": 0,
          "interval": 1,
          "entityCount": 12,
          "entity": {
            "entityType": "Bird",
            "setComponents": {
              "MovementComponent": {
                "movementType": "BirdMovement",
                "direction": [-1, 0]
              }
            }
          },
          "position": [-10, 30]
        },
        {
          "spawnerType": "multiple",
          "time": 0,
          "interval": 1,
          "entityCount": 12,
          "entity": {
            "entityType": "Bird",
            "setComponents": {
              "MovementComponent": {
                "movementType": "BirdMovement",
                "direction": [-1, 0]
              }
            }
          },
          "position": [110, 30]
        }
      ]
    },
    {
      "duration": 50,
      "spawners": [
        {
          "spawnerType": "single",
          "time": 0,
          "entity": {
            "entityType": "MidBoss"
          },
          "position": [50, -10]
        }
      ]
    },
    {
      "duration": 65,
      "spawners": [
        {
          "spawnerType": "single",
          "time": 0,
          "entity": {
            "entityType": "FinalBoss"
          },
          "position": [50, -10]
        }
      ]
    }
  ]
}
""");

            dynamic movementTypes = json.movementTypes;
            dynamic entityTypes = json.entityTypes;
            JArray stages = json.stages;
            Debug.WriteLine($"stages: {stages.Count}");


            // BOSS TIMERS
            int mbt_entity = this.ECS.CreateEntity(new List<IComponent> { new TimersComponent(new Dictionary<string, Timer>()) });

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

            int GameplayUI = this.ECS.CreateEntity();
            this.ECS.AddComponent(GameplayUI, new TransformComponent(new Vector2(0, 0)));
            this.ECS.AddComponent(GameplayUI, new SpriteComponent("GamePlayUI", spriteLayer: int.MaxValue - 2, isUI: true));

            livesFontID = Scene.Loaded.ECS.CreateEntity();
            Scene.Loaded.ECS.AddComponent(livesFontID, new TransformComponent(new Vector2(500, 100)));
            Scene.Loaded.ECS.AddComponent(livesFontID, new SpriteFontComponent("NineteenNinetySeven", $"Lives: 3", spriteLayer: int.MaxValue - 1, scale: 1.5f, centered: true));


            pauseMenuUI = MenuPrefab.CreatePauseMenu(int.MaxValue - 1);

            deathMenuUI = MenuPrefab.CreateDeathMenu(int.MaxValue - 1);

            winMenuUI = MenuPrefab.CreateWinMenu(int.MaxValue - 1);
        }
    }
}

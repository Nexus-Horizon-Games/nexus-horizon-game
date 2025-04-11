using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Model.Components;
using Nexus_Horizon_Game.Model.EntityFactory;
using Nexus_Horizon_Game.Model.GameManagers;
using Nexus_Horizon_Game.View.InputSystem;
using System;
using System.Collections.Generic;
using Nexus_Horizon_Game.Controller;
using Nexus_Horizon_Game.Json;

namespace Nexus_Horizon_Game.Model.Scenes
{
    internal class GameplayScene : Scene
    {
        private int pauseMenuUI;
        private static int deathMenuUI;
        private static int winMenuUI;
        private static int livesFontID;
        private static int pointsFontID;
        private static int powerFontID;

        private static WaveHandler waveHandler;

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

        public static void UpdatePointsFont(Int64 points)
        {
            SpriteFontComponent spriteFontComponent = Scene.Loaded.ECS.GetComponentFromEntity<SpriteFontComponent>(pointsFontID);
            int padding0Count = 12 - Convert.ToString(points).Length;
            spriteFontComponent.Text = $"Points:\n   {new string('0', padding0Count)}{points}";
            Scene.Loaded.ECS.SetComponentInEntity(pointsFontID, spriteFontComponent);
        }

        public static void UpdatePowerFont(float power)
        {
            SpriteFontComponent spriteFontComponent = Scene.Loaded.ECS.GetComponentFromEntity<SpriteFontComponent>(powerFontID);
            spriteFontComponent.Text = $"Power: {power.ToString("F2")}";
            Scene.Loaded.ECS.SetComponentInEntity(powerFontID, spriteFontComponent);
        }

        protected override void Initialize()
        {
            new GameplayManager();
            GameplayManager.Instance.PointSystemChanged += OnGameManagerChanged;
        }

        protected override void LoadContent()
        {
            Renderer.LoadContent(new List<string> { "guinea_pig" });
        }

        public override void Update(GameTime gameTime)
        {
            if (!waveHandler.Started)
            {
                waveHandler.Start(gameTime);
            }

            waveHandler.Update(gameTime);
        }

        /// <summary>
        /// Where we would load a JSON FILE THAT LOADS THE GAME SCENE
        /// </summary>
        protected override void LoadScene()
        {
            var playerFactory = new PlayerFactory();
            int moveablePlayer2 = playerFactory.CreateEntity();

            waveHandler = new WaveHandler();

            var waves = JsonParser.Parse("level1.json");

            foreach (Wave wave in waves)
            {
                waveHandler.AddWave(wave);
            }

            // ---- OLD WAVE CODE:

            //float waitTime = 1;

            /*waitTime += 2;

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
            this.ECS.SetComponentInEntity(mbt_entity, bossTimersComponent);*/

            //EnemyFactory.CreateBoss("evil_guinea_pig_boss");

            int GameplayUI = this.ECS.CreateEntity();
            this.ECS.AddComponent(GameplayUI, new TransformComponent(new Vector2(0, 0)));
            this.ECS.AddComponent(GameplayUI, new SpriteComponent("GamePlayUI", spriteLayer: int.MaxValue - 10, isUI: true));

            int yOffset = 50;
            int xOffset = 530;
            livesFontID = Scene.Loaded.ECS.CreateEntity();
            Scene.Loaded.ECS.AddComponent(livesFontID, new TransformComponent(new Vector2(xOffset, yOffset + 100)));
            Scene.Loaded.ECS.AddComponent(livesFontID, new SpriteFontComponent("NineteenNinetySeven", $"Lives: 3", spriteLayer: int.MaxValue - 9, scale: 1f, centered: true));

            pointsFontID = Scene.Loaded.ECS.CreateEntity();
            Scene.Loaded.ECS.AddComponent(pointsFontID, new TransformComponent(new Vector2(xOffset, yOffset)));
            Scene.Loaded.ECS.AddComponent(pointsFontID, new SpriteFontComponent("NineteenNinetySeven", $"Points:\n   000000000000", spriteLayer: int.MaxValue - 9, scale: 0.90f, centered: true));

            powerFontID = Scene.Loaded.ECS.CreateEntity();
            Scene.Loaded.ECS.AddComponent(powerFontID, new TransformComponent(new Vector2(xOffset, yOffset + 50)));
            Scene.Loaded.ECS.AddComponent(powerFontID, new SpriteFontComponent("NineteenNinetySeven", $"Power: 0.00", spriteLayer: int.MaxValue - 9, scale: 1f, centered: true));

            pauseMenuUI = MenuPrefab.CreatePauseMenu(int.MaxValue - 1);

            deathMenuUI = MenuPrefab.CreateDeathMenu(int.MaxValue - 1);

            winMenuUI = MenuPrefab.CreateWinMenu(int.MaxValue - 1);
        }
    
        private void OnGameManagerChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is GameplayManager gameManager)
            {
                if (e.PropertyName == nameof(gameManager.Power))
                {
                    GameplayScene.UpdatePowerFont(gameManager.Power);
                }

                if (e.PropertyName == nameof(gameManager.Points))
                {
                    GameplayScene.UpdatePointsFont(gameManager.Points);
                }


                if (e.PropertyName == nameof(gameManager.Lives))
                {
                    GameplayScene.UpdateLiveFont(gameManager.Lives);

                    if (gameManager.Lives == 0)
                    {
                        GamePlayInput.SetToDiedMenu();
                    }
                }
            }
        }
    }
}

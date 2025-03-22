using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nexus_Horizon_Game.Controller;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Model.Scenes;
using Nexus_Horizon_Game.View.InputSystem;

namespace Nexus_Horizon_Game
{
    public class GameM : Game
    {
        // XNA Graphics 
        private GraphicsDeviceManager graphics;

        // Controller 
        private SystemsController systemsController;

        private static bool isGamePaused = false;
        private static bool exitGame = false;
        public GameM()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public static bool IsGamePaused
        {
            get => isGamePaused;
            set => isGamePaused = value;
        }

        public static bool ExitGame
        {
            get => exitGame;
            set => exitGame = value;
        }

        protected override void Initialize()
        {
            // set up systems controller
            InputSystem.SetInputSystem(new MenuInput());
            systemsController = new SystemsController();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Renderer.Init(graphics, 640, 480, 200.0f, new SpriteBatch(GraphicsDevice), Content);
            Scene.Loaded = new MenuScene();

            // Instantiate your bullet pool
            BulletFactory playerBulletFactory = new BulletFactory("BulletSample");
            // Create the pool with an appropriate starting size (e.g., 200)
            new Nexus_Horizon_Game.Pooling.BulletPool(playerBulletFactory, startingPoolSize: 10);
        }

        /// <summary>
        /// sends user event to controller
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.C) || exitGame)
                Exit();

            // update by sending current scene and game time
            InputSystem.Update(gameTime);
            if (!isGamePaused)
            {
                systemsController.Update(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Queries the model to draw sprites on screen 
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Renderer.BeginRender();

            systemsController.Draw(gameTime);

            Renderer.EndRender();

            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nexus_Horizon_Game.Controller;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Model.Scenes;

namespace Nexus_Horizon_Game
{
    public class GameM : Game
    {
        // XNA Graphics 
        private GraphicsDeviceManager graphics;

        // Controller 
        private SystemsController systemsController;


        public GameM()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // set up systems controller
            systemsController = new SystemsController();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Renderer.Init(graphics, 600, 680, 200.0f, new SpriteBatch(GraphicsDevice), Content);

            Scene.Loaded = new GameplayScene();

            // instantiate the bullet pool
            BulletFactory playerBulletFactory = new BulletFactory("BulletSample");
            // create the pool with a valid starting size 
            new Nexus_Horizon_Game.Pooling.BulletPool(playerBulletFactory, startingPoolSize: 150);

        }

        /// <summary>
        /// sends user event to controller
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // update by sending current scene and game time
            systemsController.Update(gameTime);

            CollisionSystem.Update(gameTime);

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

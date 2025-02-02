using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Nexus_Horizon_Game
{
    public class GameM : Game
    {
        private GraphicsDeviceManager graphics;
        private static Scene currentScene;

        public GameM()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// current scene of the Game.
        /// </summary>
        internal static Scene CurrentScene
        {
            get => currentScene;
        }

        protected override void Initialize()
        {
            SceneLoader.LoadScene(ref currentScene);
            currentScene.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Renderer.Init(graphics, 600, 680, 200.0f, new SpriteBatch(GraphicsDevice), Content);
            currentScene.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Renderer.BeginRender();

            currentScene.Draw(gameTime);

            Renderer.EndRender();

            base.Draw(gameTime);
        }
    }
}

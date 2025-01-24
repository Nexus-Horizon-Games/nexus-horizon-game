using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Nexus_Horizon_Game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private Scene currentScene;

        // temp:
        private Texture2D spriteTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            currentScene = SceneLoader.LoadScene();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Renderer.Init(graphics, 600, 650, new SpriteBatch(GraphicsDevice));

            // temp:
            spriteTexture = Content.Load<Texture2D>("guinea_pig");
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

            currentScene.Draw(gameTime, spriteTexture);

            Renderer.EndRender();

            base.Draw(gameTime);
        }
    }
}

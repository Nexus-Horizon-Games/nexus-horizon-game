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
        private SpriteBatch spriteBatch;
        private Texture2D spriteTexture;
        // private Texture2D spriteTextureBullet;
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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // temp:
            spriteTexture = Content.Load<Texture2D>("HowTo_DrawSprite_Character");
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

            spriteBatch.Begin();

            currentScene.Draw(gameTime, spriteBatch, spriteTexture);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

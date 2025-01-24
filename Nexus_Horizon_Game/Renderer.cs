using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Nexus_Horizon_Game
{
    internal static class Renderer
    {
        private static GraphicsDeviceManager graphics;
        private static SpriteBatch spriteBatch;

        private static int width;
        private static int height;
        private static float scale = 1.0f;

        public static int ScreenWidth { get { return width; } }
        public static int ScreenHeight { get { return height; } }

        public static float Scale { get { return scale; } set { scale = value; } }

        public static void Init(GraphicsDeviceManager graphics, int width, int height, SpriteBatch spriteBatch)
        {
            Renderer.graphics = graphics;

            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            Renderer.width = width;
            Renderer.height = height;

            Renderer.graphics.ApplyChanges();

            Renderer.spriteBatch = spriteBatch;
        }

        public static void BeginRender()
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        }

        public static void EndRender()
        {
            spriteBatch.End();
        }

        public static void Draw(Texture2D texture, Vector2 position, Color color)
        {
            spriteBatch.Draw(texture, position, color);
        }

        public static void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale * Renderer.scale, effects, layerDepth);
        }
    }
}

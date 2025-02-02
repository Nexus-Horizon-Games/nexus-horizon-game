using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Nexus_Horizon_Game
{
    /// <summary>
    /// A class that handles all rendering.
    /// </summary>
    internal static class Renderer
    {
        private static GraphicsDeviceManager graphics;
        private static SpriteBatch spriteBatch;
        private static ResourceManager<Texture2D> textureManager;

        private static int width;
        private static int height;
        private static float scale = 1.0f;

        public static int ScreenWidth { get { return width; } }
        public static int ScreenHeight { get { return height; } }

        /// <summary>
        /// The scaled width of the window (the number of in-game units that are needed to span the entire window width).
        /// </summary>
        public static float DrawAreaWidth { get { return width / scale; } }

        /// <summary>
        /// The scaled height of the window (the number of in-game units that are needed to span the entire window height).
        /// </summary>
        public static float DrawAreaHeight { get { return height / scale; } }

        public static float Scale { get { return scale; } set { scale = value; } }

        /// <summary>
        /// Intializes the renderer.
        /// </summary>
        /// <param name="graphics">The graphics device manager.</param>
        /// <param name="windowWidth">The width of the window.</param>
        /// <param name="windowHeight">The height of the window.</param>
        /// <param name="drawAreaHeight">The scaled height of the window (the number of in-game units that are needed to span the entire window height).</param>
        /// <param name="spriteBatch">A sprite batch.</param>
        /// <param name="contentManager">The content manager.</param>
        public static void Init(GraphicsDeviceManager graphics, int windowWidth, int windowHeight, float drawAreaHeight, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            Renderer.graphics = graphics;

            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            Renderer.width = windowWidth;
            Renderer.height = windowHeight;
            Renderer.scale = (float)windowHeight / drawAreaHeight;

            Renderer.graphics.ApplyChanges();

            Renderer.spriteBatch = spriteBatch;

            textureManager = new ResourceManager<Texture2D>(contentManager);
        }

        public static void LoadContent(List<string> names)
        {
            names.Add("square");
            textureManager.LoadResources(names);
        }

        /// <summary>
        /// Call this to begin a render.
        /// </summary>
        public static void BeginRender()
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        }

        /// <summary>
        /// Call this to end a render.
        /// </summary>
        public static void EndRender()
        {
            spriteBatch.End();
        }

        public static void Draw(Texture2D texture, Vector2 position, Color color)
        {
            spriteBatch.Draw(texture, position * Renderer.scale, color);
        }

        public static void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(texture, position * Renderer.scale, sourceRectangle, color, rotation, origin, scale * Renderer.scale, effects, layerDepth);
        }

        public static void Draw(string textureName, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(textureManager.GetResource(textureName), position * Renderer.scale, sourceRectangle, color, rotation, origin, scale * Renderer.scale, effects, layerDepth);
        }

        public static void DrawRectangle(Vector2 position, Vector2 size, Color color, float rotation = 0.0f, float layerDepth = 0.0f)
        {
            spriteBatch.Draw(textureManager.GetResource("square"),
                new Rectangle((int)(position.X * Renderer.scale), (int)(position.Y * Renderer.scale), (int)(size.X * Renderer.scale), (int)(size.Y * Renderer.scale)),
                null, color, rotation, Vector2.Zero, SpriteEffects.None, layerDepth);
        }

        public static Vector2 GetTextureBounds(string textureName)
        {
            var bounds = textureManager.GetResource(textureName).Bounds;
            return new Vector2(bounds.Width, bounds.Height);
        }

        public static float GetTextureWidth(string textureName)
        {
            return textureManager.GetResource(textureName).Bounds.Width;
        }

        public static float GetTextureHeight(string textureName)
        {
            return textureManager.GetResource(textureName).Bounds.Height;
        }
    }
}

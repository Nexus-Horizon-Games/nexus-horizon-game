using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nexus_Horizon_Game
{
    internal class Scene
    {
        private World world = new World();

        public void Update(GameTime gameTime)
        {
            // Get entities with component
            // get component enumeration then through each component do the update for it.
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D spriteTexture) // spriteBatch and spriteTexture being passed here should only be temporary
        {

            RenderSystem.Draw(world, gameTime, spriteBatch, spriteTexture);
            // Get entities with component
            // get component enumeration then through each component do the draw for it.
        }

        public World World { get { return world; } }
    }
}

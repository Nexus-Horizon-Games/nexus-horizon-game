using Microsoft.Xna.Framework;

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

        public void Draw(GameTime gameTime)
        {
            // Get entities with component
            // get component enumeration then through each component do the draw for it.
        }

        public World World { get { return world; } }
    }
}

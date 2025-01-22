using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Nexus_Horizon_Game
{
    internal class Scene
    {
        private World world = new World();

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime)
        {
        }

        public World World { get { return world; } }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Nexus_Horizon_Game
{
    internal class Scene
    {
        private EntityComponentSystem ecs = new EntityComponentSystem();

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime)
        {
        }

        public EntityComponentSystem ECS { get { return ecs; } }
    }
}

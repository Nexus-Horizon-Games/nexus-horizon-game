using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using System.Collections.Generic;
using Nexus_Horizon_Game.Systems;

namespace Nexus_Horizon_Game
{
    internal class Scene
    {
        private World world = new World();

        public void Initialize()
        {

        }

        public void LoadContent()
        {
            Renderer.LoadContent(new List<string> { "guinea_pig" });
        }

        public void Update(GameTime gameTime)
        {
            PhysicsSystem.Update(gameTime);
            Player.Update(gameTime);
            OnUpdateSystem.Update(gameTime);
            TimerSystem.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            RenderSystem.Draw(gameTime);
            // Get entities with component
            // get component enumeration then through each component do the draw for it.
        }

        public World World { get { return world; } }
    }
}

using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using Microsoft.Xna.Framework.Graphics;
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
            Renderer.LoadContent(new List<string>{ "guinea_pig" });
        }

        public void Update(GameTime gameTime)
        {
            PhysicsSystem.Update(world, gameTime);
            Player.Update(world, gameTime);
            OnUpdateSystem.Update(world, gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            RenderSystem.Draw(world, gameTime);
            // Get entities with component
            // get component enumeration then through each component do the draw for it.
        }

        public World World { get { return world; } }
    }
}

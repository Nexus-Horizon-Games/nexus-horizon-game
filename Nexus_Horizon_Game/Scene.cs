using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using System.Collections.Generic;
using Nexus_Horizon_Game.Systems;

namespace Nexus_Horizon_Game
{
    internal class Scene
    {
        private World world = new World();

        private Vector2 arenaPosition = new Vector2(0.0f, 0.0f);
        private Vector2 arenaSize = new Vector2(176.4f, 200.0f);

        public World World { get { return world; } }

        public Vector2 ArenaPosition { get { return arenaPosition; } }
        public Vector2 ArenaSize { get { return arenaSize; } }

        public float ArenaTop { get { return arenaPosition.Y; } }
        public float ArenaBottom { get { return arenaPosition.Y + arenaSize.Y; } }
        public float ArenaLeft { get { return arenaPosition.X; } }
        public float ArenaRight { get { return arenaPosition.X + arenaSize.X; } }

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
            InputSystem.Update();
            OnUpdateSystem.Update(gameTime);
            TimerSystem.Update(gameTime);
            BehaviourSystem.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            RenderSystem.Draw(gameTime);
            // Get entities with component
            // get component enumeration then through each component do the draw for it.
        }
    }
}

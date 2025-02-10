using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Nexus_Horizon_Game.Systems;
using Nexus_Horizon_Game.Components;

namespace Nexus_Horizon_Game
{
    internal class Scene
    {
        private World world = new World();

        private Vector2 arenaPosition = new Vector2(0.0f, 0.0f);
        private Vector2 arenaSize = new Vector2(176f, 200f);

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

        /// <summary>
        /// This will check if an entity is in the arena and then send A directional vector of where it is going off the Arena.
        /// [0 = Not On X or a Y Boundary Line]
        /// (1,0) = right-X 
        /// (-1, 0) = left-X
        /// (0, 1) = bottom-Y
        /// (0, -1) = top-Y
        /// Any Combination of these will be the direction the entity is going off the arena.
        /// </summary>
        /// <param name="entity"> entity checking. </param>
        /// <param name="xBoundaryScale"> Scale x from origin of arena. </param>
        /// <param name="yBoundaryScale"> Scale y from origin of arena. </param>
        /// 
        /// <returns> The direction the entity went off screen. </returns>
        public Vector2 CheckEntityInArena(TransformComponent entityTransform, out Vector2 boundaryIn, float xBoundaryScale = 1f, float yBoundaryScale = 1f)
        {
            Vector2 ArenaOrigin = arenaSize / 2;
            float width = arenaSize.X;
            float height = arenaSize.Y;

            Vector2 direction = new Vector2(0f, 0f);
            boundaryIn = new Vector2(0f, 0f);

            if ((entityTransform.position.Y >= (ArenaOrigin.Y + ((height / 2) * yBoundaryScale))))
            {
                direction.Y = -1f;
                boundaryIn.Y = (ArenaOrigin.Y + ((height / 2) * yBoundaryScale));
            }
            else if ((entityTransform.position.Y <= (ArenaOrigin.Y + (-(height / 2) * yBoundaryScale))))
            {
                direction.Y = 1f;
                boundaryIn.Y = (ArenaOrigin.Y + (-(height / 2) * yBoundaryScale));
            }

            if ((entityTransform.position.X >= (ArenaOrigin.X + ((width / 2) * xBoundaryScale))))
            {
                direction.X = 1f;
                boundaryIn.X = (ArenaOrigin.X + ((width / 2) * xBoundaryScale));
            }
            if (entityTransform.position.X <= (ArenaOrigin.X + (-(width / 2) * xBoundaryScale)))
            {
                direction.X = -1f;
                boundaryIn.X = (ArenaOrigin.X + (-(width / 2) * xBoundaryScale));
            }

            return direction;
        }
    }
}

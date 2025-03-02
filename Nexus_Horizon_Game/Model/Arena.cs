using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

namespace Nexus_Horizon_Game
{
    internal static class Arena
    {
        private static readonly Vector2 position = new Vector2(0.0f, 0.0f);
        private static readonly Vector2 size = new Vector2(176f, 200f);

        public static Vector2 Position { get { return position; } }
        public static Vector2 Size { get { return size; } }

        public static float Top { get { return position.Y; } }
        public static float Bottom { get { return position.Y + size.Y; } }
        public static float Left { get { return position.X; } }
        public static float Right { get { return position.X + size.X; } }

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
        public static Vector2 CheckEntityInArena(TransformComponent entityTransform, out Vector2 boundaryIn, float xBoundaryScale = 1f, float yBoundaryScale = 1f)
        {
            Vector2 ArenaOrigin = size / 2;
            float width = size.X;
            float height = size.Y;

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

using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Components
{
    internal struct SpriteComponent : IComponent
    {
        public int spriteID = -1;
        public Vector2 position = Vector2.Zero;
        public float rotation = 0.0f;
        public Color color = Color.White;
        public float scale = 1.0f;
        public float z = 0.0f; // between 0 and 1
        public Rectangle? sourceRectangle = null; // used to render only a section of an image (for tiles)

        public SpriteComponent(int spriteID)
        {
            this.spriteID = spriteID;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is SpriteComponent o)
            {
                if (spriteID == o.spriteID &&
                    position == o.position &&
                    rotation == o.rotation &&
                    color == o.color &&
                    scale == o.scale &&
                    z == o.z &&
                    sourceRectangle == o.sourceRectangle)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsEmptyComponent()
        {
            return spriteID == -1;
        }

        /// <inheritdoc/>
        public static IComponent MakeEmptyComponent()
        {
            return new SpriteComponent(-1);
        }
    }
}
